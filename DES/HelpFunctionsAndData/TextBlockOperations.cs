using DES.HelpFunctions;
using DES.ThreadingWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.HelpFunctionsAndData
{
    public static class TextBlockOperations
    {
        public static byte[] GetPartOfTextBlock(int posInTextBlock, FileDataLoader loader)
        {
            byte[] partOfTextBlock = new byte[8];
            int readTextSize = (loader.FactTextBlockSize - posInTextBlock < 8) ? loader.FactTextBlockSize - posInTextBlock : partOfTextBlock.Length;
            for (int i = 0; i < readTextSize; i++)
            {
                partOfTextBlock[i] = loader.TextBlock[posInTextBlock + i];
            }
            loader.FactTextBlockSize += partOfTextBlock.Length - readTextSize;
            CryptSimpleFunctions.PKCS7Padding(partOfTextBlock, readTextSize);
            return partOfTextBlock;
        }

        public static void InsertPartInTextBlock(int posInTextBlock, byte[] source, int sourceSize, FileDataLoader loader)
        {
            for (int i = 0; i < sourceSize; i++)
            {
                loader.TextBlock[posInTextBlock + i] = source[i];
            }
        }
    }
}
