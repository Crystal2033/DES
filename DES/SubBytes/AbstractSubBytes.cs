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
        public virtual void SubBytes(ref byte[] bytes, int blockSizeInBits, int groupSizeInBits, in List<byte[,]> sMatrix, int newGroupBitsSize)
        {
            int valueOfGroups = blockSizeInBits / groupSizeInBits;
            byte[] reducedBytes = new byte[(newGroupBitsSize * valueOfGroups) / CryptConstants.BITS_IN_BYTE];

            for (int k = 0; k < valueOfGroups; k++) {
                (int i, int j) = GetSBlockIndexes(in bytes, groupSizeInBits, k);
                CreateReducedData(ref reducedBytes, newGroupBitsSize, (i, j), sMatrix, k);
            }
            bytes = (byte[])reducedBytes.Clone();
            CryptSimpleFunctions.ClearBytes(ref reducedBytes);
        }

        virtual protected void CreateReducedData(ref byte[] reducedBytes, int newSizeofBlock, (int i, int j) indexes, in List<byte[,]> sMatrix, int groupIndex){

            byte[] valueFromSBlock = new byte[1] { sMatrix[groupIndex][indexes.i, indexes.j] };
            CryptSimpleFunctions.SetRangeOfBits(valueFromSBlock, 0, 4, 4, ref reducedBytes,
                    groupIndex * newSizeofBlock / CryptConstants.BITS_IN_BYTE, (byte)((groupIndex * newSizeofBlock) % CryptConstants.BITS_IN_BYTE));
        }
        protected abstract (int i, int j) GetSBlockIndexes(in byte[] bytes, int groupSize, int groupIndex);

    }
}
