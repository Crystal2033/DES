using DES.HelpFunctions;
using DES.InterfacesDES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.FeistelImplementation
{
    internal class DESFeistelFunction : IFeistelNetwork
    {
        /**
         * bytes is 32 bits value
         * raundKey is 48 bits value
         * result is 32 bits value Feistel function result
         */
        public byte[] feistelTransform(byte[] bytes, in byte[] raundKey)
        {
            byte[] result = new byte[bytes.Length];

            CryptSimpleFunctions.showBinaryView(bytes, "Part of plain text");
            CryptSimpleFunctions.permutation(ref bytes, DESStandartBlocks.textExpansionBlockPermutation);
            CryptSimpleFunctions.showBinaryView(bytes, "After classic permutation");


            return result;
        }
    }
}
