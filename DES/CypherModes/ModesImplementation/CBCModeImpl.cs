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
