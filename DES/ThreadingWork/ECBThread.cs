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
    public sealed class ECBThread : BaseModeThread
    {
        public ECBThread(int id, FileDataLoader loader, ISymmetricEncryption algorithm, Barrier barrier) : base(id, loader, algorithm, barrier)
        { }   

        public override void Run(object obj)
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
                    
                    BytesTransformed++;
                    posInTextBlock = (BytesTransformed * ThreadsInfo.VALUE_OF_THREAD + _threadId) * 8;
                }
                BytesTransformed = 0;
                posInTextBlock = 0;
                _barrier.SignalAndWait();
            }

        }
    }
}
