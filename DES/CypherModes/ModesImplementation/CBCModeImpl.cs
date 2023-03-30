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
        private byte[] _prevEncryptedTextBlock;
        private byte[] _currEncryptedTextBlock;
        public CBCModeImpl(byte[] mainKey, ISymmetricEncryption algorithm, byte[] initVector) : base(mainKey, algorithm)
        {
            _initVector = initVector;
        }

        public override void DecryptWithMode(string fileToDecrypt, string decryptResultFile)
        {
            FileDataLoader loader = new(fileToDecrypt, decryptResultFile);
            _prevEncryptedTextBlock = (byte[])loader.TextBlock.Clone();
            _currEncryptedTextBlock = (byte[])loader.TextBlock.Clone();

            if (loader.TextReadSize % 8 != 0)
            {
                _log.Error($"Text for decryption in {fileToDecrypt} is not compatible. Size % 8 != 0.");
                loader.CloseStreams();
                return;
            }


            BaseModeThread[] cbcThreads = new CBCDecryptThread[ThreadsInfo.VALUE_OF_THREAD];

            Barrier barrier = new Barrier(ThreadsInfo.VALUE_OF_THREAD, (bar) =>
            {
                _prevEncryptedTextBlock = (byte[])_currEncryptedTextBlock.Clone();
                loader.reloadTextBlockAndOutputInFile();
                _currEncryptedTextBlock = (byte[])loader.TextBlock.Clone();
                for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
                {
                    ((CBCDecryptThread)cbcThreads[i]).SetNewPrevAndCurrentCypheredTextBlocks(_prevEncryptedTextBlock, _currEncryptedTextBlock);
                }

                if (loader.FactTextBlockSize == 0) // There is nothing to read
                {
                    for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
                    {
                        cbcThreads[i].SetThreadToStartPosition();
                    }
                }
            });

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
            {
                cbcThreads[i] = new CBCDecryptThread(i, loader, _cryptAlgorithm, barrier, _initVector, _prevEncryptedTextBlock, _currEncryptedTextBlock);
            }

            for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
            {
                var task = cbcThreads[i];
                tasks.Add(Task.Run(() =>
                {
                    task.Run(CryptOperation.DECRYPT);
                }));
            }

            Task.WaitAll(tasks.ToArray());
            loader.CloseStreams();
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
