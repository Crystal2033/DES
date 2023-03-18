﻿using DES.HelpFunctions;
using DES.HelpFunctionsAndData;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DES.Presenters
{
    
    internal sealed class DemonstrationCypher
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly DESImplementation DESImplementation;
        public DemonstrationCypher(DESImplementation dESImplementation)
        {
            DESImplementation = dESImplementation;
        }

        private bool isCorrectDecryption(string startFile, string decryptedFile)
        {
            return false;
        }

        public void encrypt(string userFile, string encryptTo)
        {
            try
            {
                using (var inStream = File.Open(userFile, FileMode.Open))
                using (var outSream = File.Open(encryptTo, FileMode.OpenOrCreate))
                {

                    using (BinaryReader reader = new BinaryReader(inStream, Encoding.UTF8))
                    using (BinaryWriter writer = new BinaryWriter(outSream, Encoding.UTF8))
                    {
                        byte[] textBlock = new byte[2048];
                        byte[] currentPart = new byte[8];
                        int writtenBytes;
                        while ((writtenBytes = reader.Read(textBlock, 0, textBlock.Length)) != 0)
                        {
                            int textPartsCounter = 0;
                            while (textPartsCounter * CryptConstants.DES_PART_TEXT_BYTES < writtenBytes)
                            {
                                CryptSimpleFunctions.GetNewPartOfText(textBlock, currentPart, textPartsCounter * CryptConstants.DES_PART_TEXT_BYTES);
                                textPartsCounter++;
                                byte[] cipher = DESImplementation.Encrypt(ref currentPart);
                                writer.Write(cipher);
                            }
                        }

                    }

                }
            }
            catch (Exception ae)
            {
                _log.Error(ae);
            }
        }

        public void decrypt(string encryptedFile, string decryptTo)
        {
            //need to check that in encrypt file is always divided by 8. 1 byte in encrypted file is not correct file value
            using (var inStream = File.Open(encryptedFile, FileMode.OpenOrCreate))
            using (var outSream = File.Open(decryptTo, FileMode.OpenOrCreate))
            {

                using (BinaryReader reader = new BinaryReader(inStream, Encoding.UTF8))
                using (BinaryWriter writer = new BinaryWriter(outSream, Encoding.UTF8))
                {
                    byte[] cryptedTextBlock = new byte[2048];
                    byte[] currentPart = new byte[8];
                    if(inStream.Length % CryptConstants.BITS_IN_BYTE != 0){
                        Console.WriteLine($"Encryption file error! Size is not divide by 8. Length = {inStream.Length}");
                    }

                    int writtenBytes;
                    while ((writtenBytes = reader.Read(cryptedTextBlock, 0, cryptedTextBlock.Length)) != 0)
                    {
                        int textPartsCounter = 0;
                        while (textPartsCounter * CryptConstants.DES_PART_TEXT_BYTES < writtenBytes)
                        {
                            CryptSimpleFunctions.GetNewPartOfText(cryptedTextBlock, currentPart, textPartsCounter * CryptConstants.DES_PART_TEXT_BYTES);
                            textPartsCounter++;
                            byte[] plainText = DESImplementation.Decrypt(ref currentPart);
                            int remainedBytes = cryptedTextBlock.Length - textPartsCounter * CryptConstants.DES_PART_TEXT_BYTES;
                            if (remainedBytes < 0)
                            {
                                CryptSimpleFunctions.ClearBytes(ref plainText, CryptConstants.DES_PART_TEXT_BYTES - Math.Abs(remainedBytes));
                            }
                            writer.Write(plainText);
                        }
                    }

                }

            }


            
        }
    }
}
