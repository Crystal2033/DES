using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.HelpFunctions
{
    internal class CryptSimpleFunctions

        
    {
        private static void showBinaryView(in byte[] viewBytes) {
            for (int i = 0; i < viewBytes.Length; i++)
            {
                Console.Write(Convert.ToString(viewBytes[i], 2).PadLeft(8, '0'));
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        public static void Permutation(ref byte[] bytes, in byte[] pBlock)
        {
            showBinaryView(bytes);


        }
    }
}
