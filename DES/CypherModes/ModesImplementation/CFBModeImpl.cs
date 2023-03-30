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
            int posInTextBlock;
            byte[] plainPartOfText;
            byte[] partOfTextBlock;
            byte[] prevCypheredPartOfText = _initVector;
            int realPlainTextPartSize = CryptConstants.DES_PART_TEXT_BYTES;

            while (loader.FactTextBlockSize != 0)
            {
                posInTextBlock = 0;
                while (posInTextBlock < loader.FactTextBlockSize)
                {
                    byte[] decryptedPrevCypheredPart = CryptSimpleFunctions.GetBytesAfterCryptOperation(
                        CryptOperation.ENCRYPT, ref prevCypheredPartOfText, _cryptAlgorithm); // HERE IS REALLY ENCRYPT TO DECRYPT!!!

                    prevCypheredPartOfText = decryptedPrevCypheredPart;

                    partOfTextBlock = TextBlockOperations.GetPartOfTextBlockWithoutPadding(posInTextBlock, loader.TextBlock);

                    plainPartOfText = CryptSimpleFunctions.XorByteArrays(partOfTextBlock, decryptedPrevCypheredPart);
                    //CryptSimpleFunctions.GetLettersFromBytes(plainPartOfText);
                    realPlainTextPartSize = CryptSimpleFunctions.GetPureTextWithoutPaddingSize(ref plainPartOfText, loader);

                    TextBlockOperations.InsertPartInTextBlock(posInTextBlock, plainPartOfText, realPlainTextPartSize, loader);

                    posInTextBlock += realPlainTextPartSize;
                }
                loader.reloadTextBlockAndOutputInFile();
            }
            loader.CloseStreams();
        }

        public override void EncryptWithMode(string fileToEncrypt, string encryptResultFile)
        {
            FileDataLoader loader = new(fileToEncrypt, encryptResultFile);
            int curPosInTextBlock;
            byte[] partOfTextBlock;
            byte[] xoredMessage;
            byte[] cryptedPartOfText = _initVector;

            while (loader.FactTextBlockSize != 0)
            {
                curPosInTextBlock = 0;
                while (curPosInTextBlock < loader.FactTextBlockSize)
                {
                    cryptedPartOfText = CryptSimpleFunctions.GetBytesAfterCryptOperation(CypherEnums.CryptOperation.ENCRYPT, ref cryptedPartOfText, _cryptAlgorithm);

                    partOfTextBlock = TextBlockOperations.GetPartOfTextBlock(curPosInTextBlock, loader);

                    xoredMessage = CryptSimpleFunctions.XorByteArrays(partOfTextBlock, cryptedPartOfText);

                    TextBlockOperations.InsertPartInTextBlock(curPosInTextBlock, xoredMessage, xoredMessage.Length, loader);
                    curPosInTextBlock += partOfTextBlock.Length;
                }
                loader.reloadTextBlockAndOutputInFile();
            }
            loader.CloseStreams();
        }
    }
}
