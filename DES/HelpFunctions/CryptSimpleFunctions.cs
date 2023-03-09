using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.HelpFunctions
{
    internal class CryptSimpleFunctions  
    {
        private const byte BITS_IN_BYTE = 8;
        private static void showBinaryView(in byte[] viewBytes, in string message) {
            Console.WriteLine(message);
            for (int i = 0; i < viewBytes.Length; i++)
            {
                Console.Write(Convert.ToString(viewBytes[i], 2).PadLeft(8, '0'));
                Console.Write(" ");
            }
            Console.WriteLine();
        }


        public static void Permutation(ref byte[] bytes, in byte[] pBlock)
        {
            showBinaryView(bytes, "Start bytes view");
            byte[] result = new byte[bytes.Length];
            for (byte i = 0; i < pBlock.Length; i++)
            {
                byte currBlockIndex = (byte)((pBlock[i] - 1) / BITS_IN_BYTE);

                byte currBit = (byte)((bytes[currBlockIndex] >> ((currBlockIndex + 1) * BITS_IN_BYTE - pBlock[i]) & 1));

                result[i / BITS_IN_BYTE] = (byte)(result[i / BITS_IN_BYTE] | (currBit << BITS_IN_BYTE - (i % BITS_IN_BYTE + 1)));
            }
            bytes = result;
            showBinaryView(bytes, "Result bytes");

            
        }
    }
}
