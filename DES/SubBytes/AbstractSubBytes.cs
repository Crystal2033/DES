using DES.HelpFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.SubBytes
{
    public abstract class AbstractSubBytes
    {
        public virtual void subBytes(ref byte[] bytes, int blockSize, int groupSize, in List<byte[,]> sMatrix, int newBlockBitsSize)
        {
            int valueOfGroups = blockSize / groupSize;
            byte[] reducedBytes = new byte[(newBlockBitsSize * valueOfGroups) / CryptConstants.BITS_IN_BYTE];

            for (int k = 0; k < valueOfGroups; k++) {
                (int i, int j) = getSBlockIndexes(in bytes, groupSize, k);
                createReducedData(ref reducedBytes, newBlockBitsSize, (i, j), sMatrix, k);
                Console.WriteLine($"i={i}  j={j}  value[{k}][{i},{j}]={sMatrix[k][i, j]} ");
                CryptSimpleFunctions.showBinaryView(reducedBytes, $"Iteration {k}");
                Console.WriteLine();
                Console.WriteLine();
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
