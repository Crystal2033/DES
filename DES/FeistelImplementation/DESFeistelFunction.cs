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
    internal class DESFeistelFunction : IFeistelFunction
    {
        /**
         * bytes is 32 bits value
         * raundKey is 48 bits value
         * result is 32 bits value Feistel function result
         */
        public byte[] feistelFunction(ref byte[] bytes, in byte[] raundKey) //checked
        {
            byte[] result;

            CryptSimpleFunctions.permutation(ref bytes, DESStandartBlocks.textExpansionBlockPermutation);

            try{
                result = CryptSimpleFunctions.xorByteArrays(bytes, raundKey);
            }
            catch(ArgumentException exc){
                Console.WriteLine(exc.Message);
                return new byte[0];
            }
            
            AbstractSubBytes desSubBytes = new DesSybBytes();

            desSubBytes.subBytes(ref result, 48, 6, DESStandartBlocks.SMatrix, 4);

            CryptSimpleFunctions.permutation(ref result, DESStandartBlocks.finalFeistelPermutationDES);

            return result;
        }
    }
}
