using DES.CypherEnums;
using DES.HelpFunctions;
using DES.InterfacesDES;
using DES.ThreadingWork;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DES.CypherModes.ModesImplementation
{
    public sealed class RDModeImpl : CryptModeImpl
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private byte[] _initVector;
        private int _delta;
        private byte[] _hash;
        public byte[] HashCode { 
                get => _hash;
                init
                {
                    _hash = CryptSimpleFunctions.XorByteArrays(value, _initVector);
                }
            }

        public RDModeImpl(byte[] mainKey, ISymmetricEncryption algorithm, byte[] initVector, int delta=1) : base(mainKey, algorithm)
        {
            _initVector = initVector;
            _delta = delta;
        }
        public override void DecryptWithMode(string fileToDecrypt, string decryptResultFile)
        {
            Execute(fileToDecrypt, decryptResultFile, CryptOperation.DECRYPT);
        }

        public override void EncryptWithMode(string fileToEncrypt, string encryptResultFile)
        {
            Execute(fileToEncrypt, encryptResultFile, CryptOperation.ENCRYPT);
        }

        private void Execute(string inputFile, string outputFile, CryptOperation cryptOperation)
        {
            FileDataLoader loader = new(inputFile, outputFile);
            if (cryptOperation == CryptOperation.DECRYPT)
            {
                if (loader.TextReadSize % 8 != 0)
                {
                    _log.Error($"Text for decryption in {inputFile} is not compatible. Size % 8 != 0.");
                    loader.CloseStreams();
                    return;
                }
            }

            BaseModeThread[] rdThreads = new RDThread[ThreadsInfo.VALUE_OF_THREAD];


            Barrier barrier = new Barrier(ThreadsInfo.VALUE_OF_THREAD, (bar) =>
            {
                loader.reloadTextBlockAndOutputInFile();

                if (loader.FactTextBlockSize == 0) // There is nothing to read
                {
                    for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
                    {
                        rdThreads[i].SetThreadToStartPosition();
                    }
                }
            });

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
            {
                rdThreads[i] = new RDThread(i, loader, _cryptAlgorithm, barrier, _delta, _initVector);

            }
            for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
            {
                var task = rdThreads[i];
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
