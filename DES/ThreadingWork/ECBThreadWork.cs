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

        public void Run(object obj)
        {
            CryptOperation cryptOperation = (CryptOperation)obj;
            int posInTextBlock = _threadId * 8;
            int realCypherPartSize = CryptConstants.DES_PART_TEXT_BYTES;

            while (_loader.FactTextBlockSize != 0)
            { 
                while (posInTextBlock < _loader.FactTextBlockSize)
                {
                    byte[] partOfTextBlock = TextBlockOperations.GetPartOfTextBlock(posInTextBlock, _loader);

                    byte[] newBytes = CryptSimpleFunctions.GetBytesAfterCryptOperation(cryptOperation, ref partOfTextBlock, _algorithm);
                    if (cryptOperation == CryptOperation.DECRYPT) // checking padding for decryption
                    {
                        realCypherPartSize = CryptSimpleFunctions.GetPureTextWithoutPaddingSize(ref newBytes, _loader);
                    }

                    TextBlockOperations.InsertPartInTextBlock(posInTextBlock, newBytes, realCypherPartSize, _loader);
                    
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
