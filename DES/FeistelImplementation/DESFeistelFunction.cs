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
        public byte[] FeistelFunction(ref byte[] bytes, in byte[] raundKey) //checked
        {
            byte[] result;

            CryptSimpleFunctions.Permutation(ref bytes, DESStandartBlocks.textExpansionBlockPermutation);
            CryptSimpleFunctions.ShowBinaryView(bytes, $"After permutation");

            try
            {
                CryptSimpleFunctions.ShowBinaryView(raundKey, $"Raund key");
                result = CryptSimpleFunctions.XorByteArrays(bytes, raundKey);
                CryptSimpleFunctions.ShowBinaryView(result, $"After XOR with raund key");
            }
            catch(ArgumentException exc){
                Console.WriteLine(exc.Message);
                return new byte[0];
            }
            
            AbstractSubBytes desSubBytes = new DesSybBytes();

            desSubBytes.SubBytes(ref result, 48, 6, DESStandartBlocks.SMatrix, 4);
            CryptSimpleFunctions.ShowBinaryView(result, $"After SubBytes");

            CryptSimpleFunctions.Permutation(ref result, DESStandartBlocks.finalFeistelPermutationDES);
            CryptSimpleFunctions.ShowBinaryView(result, $"After last Feistel permutation");

            return result;
        }
    }
}
