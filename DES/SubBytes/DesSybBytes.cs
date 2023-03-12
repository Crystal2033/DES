using DES.HelpFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.SubBytes
{
    internal class DesSybBytes : AbstractSubBytes
    {
        protected override (int i, int j) getSBlockIndexes(in byte[] bytes, int groupSize, int groupIndex)
        {
            CryptSimpleFunctions.showBinaryView(bytes, "Start view");
            //getting I index
            //Getting byte and shift bit for first bit in group
            int startByteIndex = (groupSize * groupIndex) / CryptConstants.BITS_IN_BYTE;
            int startBitIndex = (groupSize * groupIndex) % CryptConstants.BITS_IN_BYTE;

            //Getting byte and shift bit for last bit in group
            int endByteIndex = (groupSize * (groupIndex + 1) - 1) / CryptConstants.BITS_IN_BYTE;
            int endBitIndex = (groupSize * (groupIndex + 1) - 1) % CryptConstants.BITS_IN_BYTE;

            int firstPartOfI = (bytes[startByteIndex] >> (CryptConstants.BITS_IN_BYTE - startBitIndex - 1) & 1);
            int secondPartOfI = (bytes[endByteIndex] >> (CryptConstants.BITS_IN_BYTE - endBitIndex - 1) & 1) << 1;
            int i = firstPartOfI | secondPartOfI;

            //Getting J Index
            int j = 0;

            return (i, j);
        }
    }
}
