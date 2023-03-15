﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.HelpFunctions
{
    internal class CryptSimpleFunctions  
    {
        
        public static void showBinaryView(in byte[] viewBytes, in string message="") {
            Console.WriteLine(message);
            for (int i = 0; i < viewBytes.Length; i++)
            {
                Console.Write(Convert.ToString(viewBytes[i], 2).PadLeft(8, '0'));
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        public static void clearBytes(ref byte[] bytes){
            for(int i = 0; i < bytes.Length; i++) {
                bytes[i] = 0;
            }
        }

        public static byte getBitFromPos(in byte myByte, byte index0FromLeft){
            return (byte)(myByte >> (CryptConstants.BITS_IN_BYTE - index0FromLeft - 1) & 1);
        }

        public static void setBitOnPos(ref byte myByte, byte index0FromLeft, byte currBit){
            byte mask = (byte)(1 << (CryptConstants.BITS_IN_BYTE - index0FromLeft - 1));
             myByte = (byte)((myByte & ~mask) |
                   ((currBit << (CryptConstants.BITS_IN_BYTE - index0FromLeft - 1)) & mask));
        }

        public static void permutation(ref byte[] bytes, in byte[] pBlock)
        {
            showBinaryView(bytes, "Start bytes view");
            byte[] result = new byte[pBlock.Length / CryptConstants.BITS_IN_BYTE];
            for (byte i = 0; i < pBlock.Length; i++)
            {
                byte byteIndex = (byte)((pBlock[i] - 1) / CryptConstants.BITS_IN_BYTE);

                byte currBit = (byte)(bytes[byteIndex] >> ((byteIndex + 1) * CryptConstants.BITS_IN_BYTE - pBlock[i]) & 1);

                result[i / CryptConstants.BITS_IN_BYTE] = (byte)(result[i / CryptConstants.BITS_IN_BYTE] | (currBit << CryptConstants.BITS_IN_BYTE - (i % CryptConstants.BITS_IN_BYTE + 1)));
            }
            bytes = (byte[]) result.Clone();
            clearBytes(ref result);
            //showBinaryView(bytes, "Result bytes");
        }


        public static void sliceKeyOnTwoKeys(in byte[] key, int leftBlockSize, int rightBlockSize, out byte[] leftPart, out byte[] rightPart)
        {
            leftPart = new byte[(int)Math.Ceiling((double)leftBlockSize / (double)CryptConstants.BITS_IN_BYTE)];
            rightPart = new byte[(int)Math.Ceiling((double)rightBlockSize / (double)CryptConstants.BITS_IN_BYTE)];
           //showBinaryView(key, "Full key");
            setRangeOfBits(key, 0, 0, leftBlockSize, ref leftPart, 0, 0);
            //showBinaryView(leftPart, "Left part");
            setRangeOfBits(key, leftBlockSize/CryptConstants.BITS_IN_BYTE, (byte)(leftBlockSize % CryptConstants.BITS_IN_BYTE),
                rightBlockSize, ref rightPart, 0, 0);
            //showBinaryView(rightPart, "Right part");
        }

        public static byte[] concatTwoBitParts(in byte[] leftPart, int leftSize, in byte[] rightPart, int rightSize)
        {
            return new byte[(int)Math.Ceiling(((double)(leftSize + rightSize)) / (double)(CryptConstants.BITS_IN_BYTE))];
        }

        /**
         * copyFrom byte array from which copying bits
         * startByteFrom copyFrom start BYTE to copy
         * startBitFrom copyFrom start BIT to copy from 0 to 7 little-endian (0 1 2 3 4 5 6 7)
         * valueOfBits how many bits need to insert
         * copyTo resultArr
         * startByteTo copyTo start BYTE to copy
         * startBitTo copyTo start BIT to copy from 0 to 7 little-endian (0 1 2 3 4 5 6 7)
         */
        public static void setRangeOfBits(in byte[] copyFrom, int startByteFrom, byte startBitFrom, int valueOfBits, 
                                         ref byte[] copyTo, int startByteTo, byte startBitTo){
            for(int i = 0; i < valueOfBits; i++){
                byte currBit = getBitFromPos(copyFrom[startByteFrom + ((i + startBitFrom) / CryptConstants.BITS_IN_BYTE)], (byte)((startBitFrom + (i % CryptConstants.BITS_IN_BYTE) ) % CryptConstants.BITS_IN_BYTE));
                setBitOnPos(ref copyTo[startByteTo + ((i + startBitTo) / CryptConstants.BITS_IN_BYTE)],
                    (byte)((startBitTo + (i % CryptConstants.BITS_IN_BYTE)) % CryptConstants.BITS_IN_BYTE), currBit);
            }
        }
    }
}
