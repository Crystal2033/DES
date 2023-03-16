using DES.HelpFunctions;
using DES.InterfacesDES;
using DES.SubBytes;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
        public byte[] feistelTransform(ref byte[] bytes, in byte[] raundKey)
        {
            byte[] result = new byte[bytes.Length];

            //CryptSimpleFunctions.showBinaryView(bytes, "Part of plain text");
            CryptSimpleFunctions.permutation(ref bytes, DESStandartBlocks.textExpansionBlockPermutation);

            //CryptSimpleFunctions.showBinaryView(bytes, "After classic permutation");
            //CryptSimpleFunctions.showBinaryView(raundKey, "Raund key");
            //byte[] xoredArrays;
            try{
                result = CryptSimpleFunctions.xorByteArrays(bytes, raundKey);
            }
            catch(ArgumentException exc){
                Console.WriteLine(exc.Message);
                return new byte[0];
            }
            
            //CryptSimpleFunctions.showBinaryView(result, "XOR");
            AbstractSubBytes desSubBytes = new DesSybBytes();

            desSubBytes.subBytes(ref result, 48, 6, DESStandartBlocks.SMatrix, 4);
            //CryptSimpleFunctions.showBinaryView(result, "After subBytes");

            CryptSimpleFunctions.permutation(ref result, DESStandartBlocks.finalFeistelPermutationDES);

            //CryptSimpleFunctions.showBinaryView(result, "After final permutation");
            return result;
        }
    }
}
