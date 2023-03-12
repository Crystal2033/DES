using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.SubBytes
{
    public abstract class AbstractSubBytes
    {
        public virtual void subBytes(ref byte[] bytes, int blockSize, int groupSize, in List<byte[][]> sMatrix)
        {
            int valueOfGroups = blockSize / groupSize;
            for(int k = 0; k < valueOfGroups; k++) {
                (int i, int j) = getSBlockIndexes(in bytes, groupSize, k);
            }
        }
        protected abstract (int i, int j) getSBlockIndexes(in byte[] bytes, int groupSize, int groupIndex);

        virtual protected void hey()
        {

        }
    }
}
