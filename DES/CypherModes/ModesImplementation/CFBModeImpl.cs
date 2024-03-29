﻿using DES.CypherEnums;
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
using System.Threading;
using System.Threading.Tasks;

namespace DES.CypherModes.ModesImplementation
{
    internal class CFBModeImpl : CryptModeImpl
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private byte[] _initVector;
        private byte[] _prevEncryptedTextBlock;
        private byte[] _currEncryptedTextBlock;

        public CFBModeImpl(byte[] mainKey, ISymmetricEncryption algorithm, byte[] initVector) : base(mainKey, algorithm)
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


            BaseModeThread[] cfbThreads = new CFBDecryptThread[ThreadsInfo.VALUE_OF_THREAD];

            Barrier barrier = new Barrier(ThreadsInfo.VALUE_OF_THREAD, (bar) =>
            {
                _prevEncryptedTextBlock = (byte[])_currEncryptedTextBlock.Clone();
                loader.reloadTextBlockAndOutputInFile();
                _currEncryptedTextBlock = (byte[])loader.TextBlock.Clone();
                for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
                {
                    ((CFBDecryptThread)cfbThreads[i]).SetNewPrevAndCurrentCypheredTextBlocks(_prevEncryptedTextBlock, _currEncryptedTextBlock);
                }

                if (loader.FactTextBlockSize == 0) // There is nothing to read
                {
                    for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
                    {
                        cfbThreads[i].SetThreadToStartPosition();
                    }
                }
            });

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
            {
                cfbThreads[i] = new CFBDecryptThread(i, loader, _cryptAlgorithm, barrier, _initVector, _prevEncryptedTextBlock, _currEncryptedTextBlock);
            }

            for (int i = 0; i < ThreadsInfo.VALUE_OF_THREAD; i++)
            {
                var task = cfbThreads[i];
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
            byte[] cypherText = _initVector;
            byte[] cryptedInitVector;

            while (loader.FactTextBlockSize != 0)
            {
                curPosInTextBlock = 0;
                while (curPosInTextBlock < loader.FactTextBlockSize)
                {
                    cypherText = GetEncryptValue(cypherText, loader, curPosInTextBlock);
                    TextBlockOperations.InsertPartInTextBlock(curPosInTextBlock, cypherText, cypherText.Length, loader);
                    curPosInTextBlock += cypherText.Length;
                }
                loader.reloadTextBlockAndOutputInFile();
            }
            loader.CloseStreams();
        }

        private byte[] GetEncryptValue(byte[] bytesToCrypt, FileDataLoader loader, int curPosInText)
        {
            byte[] cryptedInitValue;
            byte[] partOfTextBlock;

            cryptedInitValue = CryptSimpleFunctions.GetBytesAfterCryptOperation(CypherEnums.CryptOperation.ENCRYPT, ref bytesToCrypt, _cryptAlgorithm);
            
            partOfTextBlock = TextBlockOperations.GetPartOfTextBlock(curPosInText, loader);

            return CryptSimpleFunctions.XorByteArrays(partOfTextBlock, cryptedInitValue);
        }
        
    }
}
