using DES.HelpFunctions;
using DES.HelpFunctionsAndData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.Presenters
{
    
    internal class DemonstrationCypher
    {
        private string _userFile;
        private string _encryptedFile;
        private string _decryptedFile;
        private readonly DESImplementation DESImplementation;
        public DemonstrationCypher(string userFile, string encryptedFile, string decryptedFile, DESImplementation dESImplementation)
        {
            _userFile = userFile;
            _encryptedFile = encryptedFile;
            _decryptedFile = decryptedFile;
            DESImplementation = dESImplementation;
        }

        public void encrypt(){
            using (var inStream = File.Open(_userFile, FileMode.OpenOrCreate))
            using (var outSream = File.Open(_encryptedFile, FileMode.OpenOrCreate))
            {

                using (BinaryReader reader = new BinaryReader(inStream, Encoding.UTF8))
                using (BinaryWriter writer = new BinaryWriter(outSream, Encoding.UTF8))
                {
                    byte[] textBlock = new byte[512];
                    byte[] currentPart = new byte[8];

                    while (reader.Read(textBlock, 0, textBlock.Length) != 0)
                    {
                        int textPartsCounter = 0;
                        while (textPartsCounter * CryptConstants.DES_PART_TEXT_BYTES < textBlock.Length)
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

        public void decrypt(){
            //need to check that in encrypt file is always divided by 8. 1 byte in encrypted file is not correct file value
            using (var inStream = File.Open(_encryptedFile, FileMode.OpenOrCreate))
            using (var outSream = File.Open(_decryptedFile, FileMode.OpenOrCreate))
            {

                using (BinaryReader reader = new BinaryReader(inStream, Encoding.UTF8))
                using (BinaryWriter writer = new BinaryWriter(outSream, Encoding.UTF8))
                {
                    byte[] cryptedTextBlock = new byte[512];
                    byte[] currentPart = new byte[8];

                    while (reader.Read(cryptedTextBlock, 0, cryptedTextBlock.Length) != 0)
                    {
                        int textPartsCounter = 0;
                        while (textPartsCounter * CryptConstants.DES_PART_TEXT_BYTES < cryptedTextBlock.Length)
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
