﻿using DES.HelpFunctions;
using DES.HelpFunctionsAndData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.SubBytes
{
    internal class DesSybBytes : AbstractSubBytes
    {
        protected override (int i, int j) GetSBlockIndexes(in byte[] bytes, int groupSize, int groupIndex) //checked
        {
            //getting I index
            //Getting byte and shift bit for first bit in group
            int startByteIndex = (groupSize * groupIndex) / CryptConstants.BITS_IN_BYTE;
            int startBitIndex = (groupSize * groupIndex) % CryptConstants.BITS_IN_BYTE;

            //Getting byte and shift bit for last bit in group
            int endByteIndex = (groupSize * (groupIndex + 1) - 1) / CryptConstants.BITS_IN_BYTE;
            int endBitIndex = (groupSize * (groupIndex + 1) - 1) % CryptConstants.BITS_IN_BYTE;

            int firstPartOfI = (bytes[startByteIndex] >> (CryptConstants.BITS_IN_BYTE - startBitIndex - 1) & 1) << 1;
            int secondPartOfI = (bytes[endByteIndex] >> (CryptConstants.BITS_IN_BYTE - endBitIndex - 1) & 1);
            int i = firstPartOfI | secondPartOfI;

            //Getting J Index
            int j = 0;
            for(int k = 0; k < groupSize - 2; k++){
                int shiftingAfterStartBit = startBitIndex + 1 + k;

                int middleBitForJ = bytes[startByteIndex + (shiftingAfterStartBit) / CryptConstants.BITS_IN_BYTE] >> 
                    (CryptConstants.BITS_IN_BYTE - (shiftingAfterStartBit % CryptConstants.BITS_IN_BYTE) - 1) & 1;
                j = j | middleBitForJ;

                if(k != groupSize - 3) { //Last iteration
                    j <<= 1;
                } 
            }
            return (i, j);
        }
    }
}
