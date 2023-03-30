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
    public sealed class CFBDecryptThread : BaseModeThread
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
            //SetNewPrevAndCurrentCypheredTextBlocks(copiedPrevCyphredTextBlock, copiedCurrCyphredTextBlock);
        }

        public override void Run(object obj = null)
        {
            int posInTextBlock = _threadId * 8;
            int realPlainTextPartSize = CryptConstants.DES_PART_TEXT_BYTES;
            byte[] partOfTextBlock;
            byte[] plainPartOfText;
            byte[] prevCypheredPartOfText = _initVector;

            while (_loader.FactTextBlockSize != 0)
            {
                while (posInTextBlock < _loader.FactTextBlockSize)
                {
                    byte[] decryptedPrevCypheredPart = CryptSimpleFunctions.GetBytesAfterCryptOperation(
                        CryptOperation.ENCRYPT, ref prevCypheredPartOfText, _algorithm); // HERE IS REALLY ENCRYPT TO DECRYPT!!!

                    prevCypheredPartOfText = decryptedPrevCypheredPart;

                    partOfTextBlock = TextBlockOperations.GetPartOfTextBlockWithoutPadding(posInTextBlock, _loader.TextBlock);

                    plainPartOfText = CryptSimpleFunctions.XorByteArrays(partOfTextBlock, decryptedPrevCypheredPart);
                    CryptSimpleFunctions.GetLettersFromBytes(plainPartOfText);
                    realPlainTextPartSize = CryptSimpleFunctions.GetPureTextWithoutPaddingSize(ref plainPartOfText, _loader);

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

        public void SetNewPrevAndCurrentCypheredTextBlocks(byte[] copiedPrevCyphredTextBlock, byte[] copiedCurrCyphredTextBlock)
        {
            _copiedPrevCyphredTextBlock = copiedPrevCyphredTextBlock;
            _copiedCurrCyphredTextBlock = copiedCurrCyphredTextBlock;
        }
    }
}
