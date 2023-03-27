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
        public override void DecryptWithMode(in string fileToDecrypt, in string decryptResultFile)
        {
            Execute(fileToDecrypt, decryptResultFile, CryptOperation.DECRYPT);
        }

        public override void EncryptWithMode(in string fileToEncrypt, in string encryptResultFile)
        {
            Execute(fileToEncrypt, encryptResultFile, CryptOperation.ENCRYPT);
        }

        private void Execute(in string inputFile, in string outputFile, CryptOperation cryptOperation)
        {
            FileDataLoader loader = new(inputFile, outputFile);

            Barrier barrier = new Barrier(ThreadsInfo.VALUE_OF_THREAD, (bar) =>
            {
                loader.reloadTextBlockAndOutputInFile();
            });
            ECBThreadWork[] ecbThreads = new ECBThreadWork[ThreadsInfo.VALUE_OF_THREAD];
            for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
            {
                ecbThreads[i] = new ECBThreadWork(loader, _cryptAlgorithm, barrier);
                ThreadPool.QueueUserWorkItem(new WaitCallback(ecbThreads[i].Run), cryptOperation);
            }
        }
    }
}
