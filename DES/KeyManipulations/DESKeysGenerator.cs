﻿using DES.HelpFunctions;
using DES.HelpFunctionsAndData;
using DES.InterfacesDES;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DES.KeyManipulations
{
    sealed public class DESKeysGenerator : RaundKeysGenerator
    {

        public static readonly int keySize = 56;
        protected override List<byte[]> GenerateKeys(in byte[] preparedKey)
        {
            List<byte[]> raundKeys = new();
            byte[] workingKey = (byte[])preparedKey.Clone();
            CryptSimpleFunctions.Permutation(ref workingKey, DESStandartBlocks.keyPermutationBlock);
            CryptSimpleFunctions.SliceArrayOnTwoArrays(workingKey, keySize/2, keySize/2, out byte[] C0, out byte[] D0);

            List<byte[]> CRaundKeys = new List<byte[]>();
            CRaundKeys.Add(C0);
            
            List<byte[]> DRaundKeys = new List<byte[]>();
            DRaundKeys.Add(D0);

            for (int i = 0; i < DESStandartBlocks.keyRaundLeftShifts.Length; i++){
                CRaundKeys.Add(CryptSimpleFunctions.CycleLeftShift(CRaundKeys[i], keySize / 2, DESStandartBlocks.keyRaundLeftShifts[i]));

                DRaundKeys.Add(CryptSimpleFunctions.CycleLeftShift(DRaundKeys[i], keySize / 2, DESStandartBlocks.keyRaundLeftShifts[i]));

                byte[] CDKey = CryptSimpleFunctions.ConcatTwoBitParts(CRaundKeys[i + 1], keySize / 2, DRaundKeys[i + 1], keySize / 2);

                CryptSimpleFunctions.Permutation(ref CDKey, DESStandartBlocks.raundKeyCompressionBlock);
                raundKeys.Add(CDKey);

            }

            return raundKeys;
        }

        

        

        protected override void MainKeyPreparation(in byte[] mainKey, out byte[] preparedKey)
        {
            preparedKey = new byte[8];
            int bitsWithValueOneCounter = 0;
            int blockCounter = 0;
            int j = 0;
            for (int i = 0; i < mainKey.Length * CryptConstants.BITS_IN_BYTE; i++, j++){

                byte currBit = CryptSimpleFunctions.GetBitFromPos(mainKey[i / CryptConstants.BITS_IN_BYTE], (byte)(i % CryptConstants.BITS_IN_BYTE));
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
                        preparedKey[j / CryptConstants.BITS_IN_BYTE] |= 1;
                    }
                    else
                    {
                        preparedKey[j / CryptConstants.BITS_IN_BYTE] |= 0;
                    }
                    j++;
                    bitsWithValueOneCounter = 0;
                    continue;
                }
            }
        }
    }
}
