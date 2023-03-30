using DES.CypherEnums;
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
    public class ECBModeImpl : CryptModeImpl
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ECBModeImpl(byte[] mainKey, ISymmetricEncryption cryptAlgorithm) : base(mainKey, cryptAlgorithm)
        {

        }
        public override void DecryptWithMode(string fileToDecrypt, string decryptResultFile)
        {
            Execute(fileToDecrypt, decryptResultFile, CryptOperation.DECRYPT);
        }

        public override void EncryptWithMode(string fileToEncrypt, string encryptResultFile)
        {
            Task.Run(() => {
                Execute(fileToEncrypt, encryptResultFile, CryptOperation.ENCRYPT);
            }).Wait();
            Console.WriteLine("Cyphering is ended");
        }

        private void Execute(string inputFile, string outputFile, CryptOperation cryptOperation)
        {
            FileDataLoader loader = new(inputFile, outputFile);
            if(cryptOperation == CryptOperation.DECRYPT)
            {
                if(loader.TextReadSize % 8 != 0)
                {
                    _log.Error($"Text for decryption in {inputFile} is not compatible. Size % 8 != 0.");
                    loader.CloseStreams();
                    return;
                }
            }

            BaseModeThread[] ecbThreads = new ECBThread[ThreadsInfo.VALUE_OF_THREAD];
            Console.WriteLine($"Thread is :{Thread.CurrentThread.Name}");

            Barrier barrier = new Barrier(ThreadsInfo.VALUE_OF_THREAD, (bar) =>
            {
                loader.reloadTextBlockAndOutputInFile();

                if (loader.FactTextBlockSize == 0) // There is nothing to read
                {

                    for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
                    {
                        ecbThreads[i].SetThreadToStartPosition();
                    }
                    //Wake up main thread
                }
            });

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
            {
                ecbThreads[i] = new ECBThread(i, loader, _cryptAlgorithm, barrier);
                
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
            loader.CloseStreams();

        }
    }
}
