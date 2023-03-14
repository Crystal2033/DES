using DES.HelpFunctions;
using DES.InterfacesDES;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.KeyManipulations
{
    sealed public class DESKeysGenerator : RaundKeysGenerator
    {
        protected override List<byte[]> generateKeys(in byte[] preparedKey)
        {
            //main work
            List<byte[]> raundKeys = new();

            return raundKeys;
        }

        protected override void mainKeyPreparing(in byte[] mainKey, out byte[] preparedKey)
        {
            preparedKey = new byte[8];// DES specify
            int bitsWithValueOneCounter = 0;
            int blockCounter = 0;
            int j = 0;
            for (int i = 0; i < mainKey.Length * CryptConstants.BITS_IN_BYTE; i++, j++){
                
                byte currBit = (byte)(mainKey[i / CryptConstants.BITS_IN_BYTE] >> 
                    (CryptConstants.BITS_IN_BYTE - (i % CryptConstants.BITS_IN_BYTE) - 1) & 1);
                blockCounter++;
                if (currBit == 1) {
                    bitsWithValueOneCounter++;
                }

                byte valueForORWithRes = (byte)(currBit << (CryptConstants.BITS_IN_BYTE - (j % CryptConstants.BITS_IN_BYTE)) - 1);
                preparedKey[j / CryptConstants.BITS_IN_BYTE] |= valueForORWithRes;

                if (blockCounter == CryptConstants.BITS_IN_BYTE - 1)
                {
                    blockCounter = 0;
                    if(bitsWithValueOneCounter % 2 == 0){
                        preparedKey[j / CryptConstants.BITS_IN_BYTE] |= (byte)1;
                    }
                    else
                    {
                        preparedKey[j / CryptConstants.BITS_IN_BYTE] |= (byte)0;
                    }
                    j++;
                    bitsWithValueOneCounter = 0;
                    continue;
                }
            }
            CryptSimpleFunctions.showBinaryView(preparedKey, "Expanded with test bits MainKey");
        }
    }
}
