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

        public CFBModeImpl(byte[] mainKey, ISymmetricEncryption algorithm, byte[] initVector) : base(mainKey, algorithm)
        {
            _initVector = initVector;
        }
        public override void DecryptWithMode(string fileToDecrypt, string decryptResultFile)
        {
            FileDataLoader loader = new(fileToDecrypt, decryptResultFile);
            int posInTextBlock;
            byte[] plainPartOfText;
            byte[] prevCypheredPartOfText = _initVector;
            int realPlainTextPartSize = CryptConstants.DES_PART_TEXT_BYTES;

            while (loader.FactTextBlockSize != 0)
            {
                posInTextBlock = 0;
                while (posInTextBlock < loader.FactTextBlockSize)
                {
                    plainPartOfText = GetDecryptValue(prevCypheredPartOfText, loader, posInTextBlock, out prevCypheredPartOfText);

                    realPlainTextPartSize = CryptSimpleFunctions.GetPureTextWithoutPaddingSize(ref plainPartOfText, loader);
                    TextBlockOperations.InsertPartInTextBlock(posInTextBlock, plainPartOfText, realPlainTextPartSize, loader);
                    posInTextBlock += realPlainTextPartSize;
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

        private byte[] GetDecryptValue(byte[] bytesToCrypt, FileDataLoader loader, int curPosInText, out byte[] prevCypheredPartOfText)
        {
            byte[] cryptedInitValue;
            byte[] cryptedPartOfText;

            cryptedInitValue = CryptSimpleFunctions.GetBytesAfterCryptOperation(CypherEnums.CryptOperation.ENCRYPT, ref bytesToCrypt, _cryptAlgorithm);

            cryptedPartOfText = TextBlockOperations.GetPartOfTextBlock(curPosInText, loader);
            prevCypheredPartOfText = (byte[])cryptedPartOfText.Clone();

            return CryptSimpleFunctions.XorByteArrays(cryptedPartOfText, cryptedInitValue);
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
    }
}
