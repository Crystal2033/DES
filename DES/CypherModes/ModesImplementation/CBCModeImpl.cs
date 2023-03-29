using DES.CypherEnums;
using DES.HelpFunctions;
using DES.HelpFunctionsAndData;
using DES.InterfacesDES;
using DES.ThreadingWork;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DES.CypherModes.ModesImplementation
{
    public sealed class CBCModeImpl : CryptModeImpl
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private byte[] _initVector;
        private byte[] copiedTextBlockForDecryptParallel;
        public CBCModeImpl(byte[] mainKey, ISymmetricEncryption algorithm, byte[] initVector) : base(mainKey, algorithm)
        {
            _initVector = initVector;
        }

        public override void DecryptWithMode(string fileToDecrypt, string decryptResultFile)
        {
            FileDataLoader loader = new(fileToDecrypt, decryptResultFile);

            if (loader.TextReadSize % 8 != 0)
            {
                _log.Error($"Text for decryption in {fileToDecrypt} is not compatible. Size % 8 != 0.");
                loader.CloseStreams();
                return;
            }
            

            CBCDecryptThread[] cbcThreads = new CBCDecryptThread[ThreadsInfo.VALUE_OF_THREAD];

            Console.WriteLine($"Thread is :{Thread.CurrentThread.Name}");

            Barrier barrier = new Barrier(ThreadsInfo.VALUE_OF_THREAD, (bar) =>
            {
                loader.reloadTextBlockAndOutputInFile();

                if (loader.FactTextBlockSize == 0) // There is nothing to read
                {

                    for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
                    {
                        cbcThreads[i].BytesTransformed = 0;
                    }
                    //Wake up main thread
                }
            });

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
            {
                ecbThreads[i] = new ECBThread(loader, _cryptAlgorithm, barrier);

            }
            for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
            {
                var task = ecbThreads[i];
                tasks.Add(Task.Run(() =>
                {

                    task.Run(cryptOperation);
                }));
            }

            Task.WaitAll(tasks.ToArray());
            ECBThread.AbsIdProp = 0;
            loader.CloseStreams();
            throw new NotImplementedException();
        }

        public override void EncryptWithMode(string fileToEncrypt, string encryptResultFile)
        {
            FileDataLoader loader = new(fileToEncrypt, encryptResultFile);
            int curPosInTextBlock;
            byte[] partOfTextBlock;
            byte[] xoredMessage;
            byte[] cryptedPartOfText = _initVector;

            while(loader.FactTextBlockSize != 0)
            {
                curPosInTextBlock = 0;
                while (curPosInTextBlock < loader.FactTextBlockSize)
                {
                    partOfTextBlock = TextBlockOperations.GetPartOfTextBlock(curPosInTextBlock, loader);
                    
                    xoredMessage = CryptSimpleFunctions.XorByteArrays(partOfTextBlock, cryptedPartOfText); //cryptedPartOfText(i-1)

                    cryptedPartOfText = CryptSimpleFunctions.GetBytesAfterCryptOperation(CypherEnums.CryptOperation.ENCRYPT, ref xoredMessage, _cryptAlgorithm); //cryptedPartOfText(i)
                    
                    TextBlockOperations.InsertPartInTextBlock(curPosInTextBlock, cryptedPartOfText, cryptedPartOfText.Length, loader);
                    curPosInTextBlock += partOfTextBlock.Length;
                }
                loader.reloadTextBlockAndOutputInFile();
            }
            loader.CloseStreams();
        }
    }
}
