using DES.CypherEnums;
using DES.InterfacesDES;
using DES.ThreadingWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.CypherModes.ModesImplementation
{
    public class ECBModeImpl : CryptModeImpl
    {
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
            bool isMainThreadSleep = true;
            ECBThreadWork[] ecbThreads = new ECBThreadWork[ThreadsInfo.VALUE_OF_THREAD];


            Barrier barrier = new Barrier(ThreadsInfo.VALUE_OF_THREAD + 1, (bar) =>
            {
                loader.reloadTextBlockAndOutputInFile();

                if (loader.FactTextBlockSize == 0) // There is nothing to read
                {

                    for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
                    {
                        ecbThreads[i].BytesTransformed = 0;
                    }
                    isMainThreadSleep = false;
                    //Wake up main thread
                }
            });

            

            for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
            {
                ecbThreads[i] = new ECBThreadWork(loader, _cryptAlgorithm, barrier);
                ThreadPool.QueueUserWorkItem(new WaitCallback(ecbThreads[i].Run), cryptOperation);
            }

            while (isMainThreadSleep)
            {
                barrier.SignalAndWait();
            }
            //Main thread has to sleep
        }
    }
}
