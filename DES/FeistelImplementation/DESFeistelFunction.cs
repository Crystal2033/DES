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
        private readonly AbstractSubBytes desSubBytes = new DesSybBytes();
        public byte[] FeistelFunction(ref byte[] bytes, in byte[] raundKey)
        {
            byte[] result;

            CryptSimpleFunctions.Permutation(ref bytes, DESStandartBlocks.textExpansionBlockPermutation);

            try
            {
                result = CryptSimpleFunctions.XorByteArrays(bytes, raundKey);
            }
            catch(ArgumentException exc){//Agregate exception
                Console.WriteLine(exc.Message);
                return new byte[0];
            }
            
            

            desSubBytes.SubBytes(ref result, 48, 6, DESStandartBlocks.SMatrix, 4);

            CryptSimpleFunctions.Permutation(ref result, DESStandartBlocks.finalFeistelPermutationDES);

            return result;
        }
    }
}
