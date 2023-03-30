using DES.CypherEnums;
using DES.HelpFunctions;
using DES.HelpFunctionsAndData;
using DES.InterfacesDES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.ThreadingWork
{
    public sealed class CBCDecryptThread : BaseModeThread
    {
        private byte[] _initVector;
        private byte[] _copiedPrevCyphredTextBlock;
        private byte[] _copiedCurrCyphredTextBlock;
        private int writtenTextBlocks = 0;
        public CBCDecryptThread(int id, FileDataLoader loader, ISymmetricEncryption algorithm, Barrier barrier,
            byte[] initVector, byte[] copiedPrevCyphredTextBlock, byte[] copiedCurrCyphredTextBlock)
            : base(id, loader, algorithm, barrier)
        {
            _initVector = initVector;
            SetNewPrevAndCurrentCypheredTextBlocks(copiedPrevCyphredTextBlock, copiedCurrCyphredTextBlock);
        }

       
        public override void Run(object obj=null)
        {
            int posInTextBlock = _threadId * 8;
            int realPlainTextPartSize = CryptConstants.DES_PART_TEXT_BYTES;
            byte[] partOfTextBlock;
            byte[] plainPartOfText;
            byte[] prevCypheredPartOfText;
            while (_loader.FactTextBlockSize != 0)
            {
                while (posInTextBlock < _loader.FactTextBlockSize)
                {
                    partOfTextBlock = TextBlockOperations.GetPartOfTextBlockWithoutPadding(posInTextBlock, _loader.TextBlock);

                    byte[] decryptedBytes = CryptSimpleFunctions.GetBytesAfterCryptOperation(CryptOperation.DECRYPT, ref partOfTextBlock, _algorithm);
                    
                    prevCypheredPartOfText = GetPrevCypherText(posInTextBlock);

                    plainPartOfText = CryptSimpleFunctions.XorByteArrays(prevCypheredPartOfText, decryptedBytes);

                    realPlainTextPartSize = CryptSimpleFunctions.GetPureTextWithoutPaddingSize(ref plainPartOfText, _loader);

                    CryptSimpleFunctions.GetLettersFromBytes(plainPartOfText);
                    TextBlockOperations.InsertPartInTextBlock(posInTextBlock, plainPartOfText, realPlainTextPartSize, _loader);

                    BytesTransformed++;
                    posInTextBlock = (BytesTransformed * ThreadsInfo.VALUE_OF_THREAD + _threadId) * 8;
                }

                BytesTransformed = 0;
                posInTextBlock = _threadId * 8;
                writtenTextBlocks++;
                _barrier.SignalAndWait();
            }
        }

        private byte[] GetPrevCypherText(int posInTextBlock)
        {
            if(_threadId == 0 && writtenTextBlocks == 0 && posInTextBlock == 0) //its initial case, need to take init vector. The start of decrypting
            {
                return _initVector;
            }
            else if(_threadId == 0 && posInTextBlock == 0) //need to take prev part of block from prev textBlock
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

        public void SetNewPrevAndCurrentCypheredTextBlocks(byte[] copiedPrevCyphredTextBlock, byte[] copiedCurrCyphredTextBlock)
        {
            _copiedPrevCyphredTextBlock = copiedPrevCyphredTextBlock;
            _copiedCurrCyphredTextBlock = copiedCurrCyphredTextBlock;
        }

    }
}
