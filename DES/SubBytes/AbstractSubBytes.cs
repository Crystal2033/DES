using DES.HelpFunctions;
using DES.HelpFunctionsAndData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.SubBytes
{
    public abstract class AbstractSubBytes
    {
        /*blockSizeInBits for DES is 48 bits (6 * 8)
         * groupSizeInBits for DES if 6
         * newGroupBitsSize for DES is 4
         */
        public virtual void subBytes(ref byte[] bytes, int blockSizeInBits, int groupSizeInBits, in List<byte[,]> sMatrix, int newGroupBitsSize)
        {
            int valueOfGroups = blockSizeInBits / groupSizeInBits;
            byte[] reducedBytes = new byte[(newGroupBitsSize * valueOfGroups) / CryptConstants.BITS_IN_BYTE];

            for (int k = 0; k < valueOfGroups; k++) {
                (int i, int j) = getSBlockIndexes(in bytes, groupSizeInBits, k);
                createReducedData(ref reducedBytes, newGroupBitsSize, (i, j), sMatrix, k);
            }
            bytes = (byte[])reducedBytes.Clone();
            CryptSimpleFunctions.clearBytes(ref reducedBytes);
        }

        virtual protected void createReducedData(ref byte[] reducedBytes, int newSizeofBlock, (int i, int j) indexes, in List<byte[,]> sMatrix, int groupIndex){
            for(int i =0; i < newSizeofBlock; i++){
                int valueFromSBlock = sMatrix[groupIndex][indexes.i,indexes.j];
                byte copyingBit = (byte)((valueFromSBlock >> (newSizeofBlock - i - 1)) & 1);
                int bitIndex = groupIndex * newSizeofBlock + i;
                int byteIndex = bitIndex / CryptConstants.BITS_IN_BYTE;
                reducedBytes[byteIndex] |= copyingBit;

                if(bitIndex % CryptConstants.BITS_IN_BYTE != CryptConstants.BITS_IN_BYTE - 1){
                    reducedBytes[byteIndex] <<= 1;
                }
                
            }
        }
        protected abstract (int i, int j) getSBlockIndexes(in byte[] bytes, int groupSize, int groupIndex);

    }
}
