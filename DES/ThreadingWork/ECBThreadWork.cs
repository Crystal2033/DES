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
    public class ECBThreadWork
    {
        private static int _absID;
        public static int AbsIdProp { get { return _absID; } set { _absID = value; } }
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
            int readTextSize = (_loader.FactTextBlockSize - posInTextBlock < 8) ? _loader.FactTextBlockSize - posInTextBlock : partOfTextBlock.Length;
            for (int i = 0; i < readTextSize; i++)
            {
                partOfTextBlock[i] = _loader.TextBlock[posInTextBlock + i];
            }
            _loader.FactTextBlockSize += partOfTextBlock.Length - readTextSize;
            CryptSimpleFunctions.PKCS7Padding(partOfTextBlock, readTextSize);
            return partOfTextBlock;
        }

        private void insertPartInTextBlock(int posInTextBlock, byte[] source, int sourceSize)
        {
            for (int i = 0; i < sourceSize; i++)
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
            int realCypherPartSize = CryptConstants.DES_PART_TEXT_BYTES;

            while (_loader.FactTextBlockSize != 0)
            { 
                while (posInTextBlock < _loader.FactTextBlockSize)
                {
                    byte[] partOfTextBlock = GetPartOfTextBlock(posInTextBlock);

                    byte[] newBytes = GetBytesAfterCryptOperation(cryptOperation, ref partOfTextBlock);
                    if (cryptOperation == CryptOperation.DECRYPT) // checking padding for decryption
                    {
                        byte lastByteValue = newBytes[newBytes.Length - 1];
                        if (lastByteValue < CryptConstants.DES_PART_TEXT_BYTES) //There is a padding PKCS7
                        {
                            _loader.FactTextBlockSize -= lastByteValue;
                            realCypherPartSize = CryptConstants.DES_PART_TEXT_BYTES - lastByteValue;
                            CryptSimpleFunctions.ClearBytes(ref newBytes, newBytes.Length - lastByteValue);
                        }
                    }

                    insertPartInTextBlock(posInTextBlock, newBytes, realCypherPartSize);
                    
                    _bytesTransformed++;
                    posInTextBlock = (_bytesTransformed * ThreadsInfo.VALUE_OF_THREAD + _threadId) * 8;
                }
                _bytesTransformed = 0;
                posInTextBlock = 0;
                _barrier.SignalAndWait();
            }

        }
    }
}
