using DES.CypherEnums;
using DES.HelpFunctions;
using DES.HelpFunctionsAndData;
using DES.InterfacesDES;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.ThreadingWork
{
    internal class CFBDecryptThread : BaseModeThread
    {
        private byte[] _initVector;
        private byte[] _copiedPrevCyphredTextBlock;
        private byte[] _copiedCurrCyphredTextBlock;
        private int writtenTextBlocks = 0;
        public CFBDecryptThread(int id, FileDataLoader loader, ISymmetricEncryption algorithm, Barrier barrier,
            byte[] initVector, byte[] copiedPrevCyphredTextBlock, byte[] copiedCurrCyphredTextBlock)
            : base(id, loader, algorithm, barrier)
        {
            _initVector = initVector;
            SetNewPrevAndCurrentCypheredTextBlocks(copiedPrevCyphredTextBlock, copiedCurrCyphredTextBlock);
        }


        public override void Run(object obj = null)
        {
            int posInTextBlock = _threadId * 8;
            int realPlainTextPartSize = CryptConstants.DES_PART_TEXT_BYTES;
            //byte[] partOfTextBlock;
            byte[] plainPartOfText;
            byte[] prevCypheredPartOfText = _initVector;

            while (_loader.FactTextBlockSize != 0)
            {
                while (posInTextBlock < _loader.FactTextBlockSize)
                {
                    prevCypheredPartOfText = GetPrevCypherText(posInTextBlock);
                    plainPartOfText = GetDecryptValue(prevCypheredPartOfText, _loader, posInTextBlock, out prevCypheredPartOfText);

                    realPlainTextPartSize = CryptSimpleFunctions.GetPureTextWithoutPaddingSize(ref plainPartOfText, _loader);
                    TextBlockOperations.InsertPartInTextBlock(posInTextBlock, plainPartOfText, realPlainTextPartSize, _loader);

                    BytesTransformedInBlock++;
                    posInTextBlock = (BytesTransformedInBlock * ThreadsInfo.VALUE_OF_THREAD + _threadId) * 8;
                }

                BytesTransformedInBlock = 0;
                posInTextBlock = _threadId * 8;
                writtenTextBlocks++;
                _barrier.SignalAndWait();
            }
        }

        private byte[] GetPrevCypherText(int posInTextBlock)
        {
            if (_threadId == 0 && writtenTextBlocks == 0 && posInTextBlock == 0) //its initial case, need to take init vector. The start of decrypting
            {
                return _initVector;
            }
            else if (_threadId == 0 && posInTextBlock == 0) //need to take prev part of block from prev textBlock
            {
                return TextBlockOperations.GetPartOfTextBlockWithoutPadding(
                    FileDataLoader.TextBlockSize - CryptConstants.DES_PART_TEXT_BYTES, _copiedPrevCyphredTextBlock);
            }
            else // Normal case, need to take prev cypher block
            {
                return TextBlockOperations.GetPartOfTextBlockWithoutPadding(
                    posInTextBlock - CryptConstants.DES_PART_TEXT_BYTES, _copiedCurrCyphredTextBlock);
            }
        }

        private byte[] GetDecryptValue(byte[] bytesToCrypt, FileDataLoader loader, int curPosInText, out byte[] prevCypheredPartOfText)
        {
            byte[] cryptedInitValue;
            byte[] cryptedPartOfText;

            cryptedInitValue = CryptSimpleFunctions.GetBytesAfterCryptOperation(CypherEnums.CryptOperation.ENCRYPT, ref bytesToCrypt, _algorithm);

            cryptedPartOfText = TextBlockOperations.GetPartOfTextBlock(curPosInText, loader);
            prevCypheredPartOfText = (byte[])cryptedPartOfText.Clone();

            return CryptSimpleFunctions.XorByteArrays(cryptedPartOfText, cryptedInitValue);
        }


        public void SetNewPrevAndCurrentCypheredTextBlocks(byte[] copiedPrevCyphredTextBlock, byte[] copiedCurrCyphredTextBlock)
        {
            _copiedPrevCyphredTextBlock = copiedPrevCyphredTextBlock;
            _copiedCurrCyphredTextBlock = copiedCurrCyphredTextBlock;
        }
    }
}
