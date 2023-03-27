using DES.CypherEnums;
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
    public class ECBThreadWork
    {
        private static int _absID;
        private int _threadId;
        private Barrier _barrier;
        private FileDataLoader _loader;
        private int _bytesTransformed;
        private ISymmetricEncryption _algorithm;
        public int BytesTransformed { get => _bytesTransformed; set { _bytesTransformed = value; } }
        public ECBThreadWork(FileDataLoader loader, ISymmetricEncryption algorithm, Barrier barrier)
        {
            _algorithm = algorithm;
            _loader = loader;
            _threadId = _absID++;
            _barrier = barrier;
        }   

        private byte[] GetPartOfTextBlock(int posInTextBlock)
        {
            byte[] partOfTextBlock = new byte[8];
            for (int i = 0; i < partOfTextBlock.Length; i++)
            {
                partOfTextBlock[i] = _loader.TextBlock[posInTextBlock + i];
            }
            return partOfTextBlock;
        }

        private void insertPartInTextBlock(int posInTextBlock, byte[] source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                _loader.TextBlock[posInTextBlock + i] = source[i];
            }
        }

        private byte[] GetBytesAfterCryptOperation(CryptOperation operation, ref byte[] partOfText)
        {
            if(operation == CryptOperation.ENCRYPT)
            {
                return _algorithm.Encrypt(ref partOfText);
            }
            else
            {
                return _algorithm.Decrypt(ref partOfText);
            }
        }

        public void Run(object obj)
        {
            CryptOperation cryptOperation = (CryptOperation)obj;
            int posInTextBlock = _threadId * 8;

            while(_loader.FactTextBlockSize != 0)
            {
                while (posInTextBlock <= _loader.FactTextBlockSize)
                {
                    byte[] partOfTextBlock = GetPartOfTextBlock(posInTextBlock);

                    byte[] newBytes = GetBytesAfterCryptOperation(cryptOperation, ref partOfTextBlock);
                    insertPartInTextBlock(posInTextBlock, newBytes);
                    _bytesTransformed++;
                    posInTextBlock = (_bytesTransformed * ThreadsInfo.VALUE_OF_THREAD + _threadId) * 8;
                }
                _barrier.SignalAndWait();
            }

        }
    }
}
