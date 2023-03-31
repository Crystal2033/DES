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
    public sealed class OFBModeImpl : CryptModeImpl
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private byte[] _initVector;

        public OFBModeImpl(byte[] mainKey, ISymmetricEncryption algorithm, byte[] initVector) : base(mainKey, algorithm)
        {
            _initVector = initVector;
        }

        public override void DecryptWithMode(string fileToDecrypt, string decryptResultFile)
        {
            FileDataLoader loader = new(fileToDecrypt, decryptResultFile);
            int posInTextBlock;
            byte[] prevCypheredPartOfText;
            byte[] cypheredInitVector = _initVector;
            int realPlainTextPartSize = CryptConstants.DES_PART_TEXT_BYTES;

            while (loader.FactTextBlockSize != 0)
            {
                posInTextBlock = 0;
                while (posInTextBlock < loader.FactTextBlockSize)
                {
                    prevCypheredPartOfText = GetCryptValue(cypheredInitVector, loader, posInTextBlock, out cypheredInitVector);
                    realPlainTextPartSize = CryptSimpleFunctions.GetPureTextWithoutPaddingSize(ref prevCypheredPartOfText, loader);
                    TextBlockOperations.InsertPartInTextBlock(posInTextBlock, prevCypheredPartOfText, realPlainTextPartSize, loader);

                    posInTextBlock += realPlainTextPartSize;
                }
                loader.reloadTextBlockAndOutputInFile();
            }
            loader.CloseStreams();
        }

        private byte[] GetCryptValue(byte[] bytesToCrypt, FileDataLoader loader, int curPosInText, out byte[] nextCryptInitValue)
        {
            byte[] cryptedInitValue;
            byte[] partOfTextBlock;

            cryptedInitValue = CryptSimpleFunctions.GetBytesAfterCryptOperation(CypherEnums.CryptOperation.ENCRYPT, ref bytesToCrypt, _cryptAlgorithm);
            nextCryptInitValue = cryptedInitValue;

            partOfTextBlock = TextBlockOperations.GetPartOfTextBlock(curPosInText, loader);

            return CryptSimpleFunctions.XorByteArrays(partOfTextBlock, cryptedInitValue);
        }
        public override void EncryptWithMode(string fileToEncrypt, string encryptResultFile)
        {
            FileDataLoader loader = new(fileToEncrypt, encryptResultFile);
            int curPosInTextBlock;
            byte[] cypherText;
            byte[] cryptedInitVector = _initVector;

            while (loader.FactTextBlockSize != 0)
            {
                curPosInTextBlock = 0;
                while (curPosInTextBlock < loader.FactTextBlockSize)
                {
                    cypherText = GetCryptValue(cryptedInitVector, loader, curPosInTextBlock, out cryptedInitVector);
                    TextBlockOperations.InsertPartInTextBlock(curPosInTextBlock, cypherText, cypherText.Length, loader);
                    curPosInTextBlock += cypherText.Length;
                }
                loader.reloadTextBlockAndOutputInFile();
            }
            loader.CloseStreams();
        }
    }
}
