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
                createReducedData(ref reducedBytes, (i, j), sMatrix, newBlockBitsSize);
            }
            bytes = (byte[])reducedBytes.Clone();
            CryptSimpleFunctions.clearBytes(ref reducedBytes);
        }

        virtual protected void createReducedData(ref byte[] reducedBytes, (int, int) indexes, in List<byte[,]> sMatrix,  int sizeofBlock){

        }
        protected abstract (int i, int j) getSBlockIndexes(in byte[] bytes, int groupSize, int groupIndex);

        virtual protected void hey()
        {

        }
    }
}
