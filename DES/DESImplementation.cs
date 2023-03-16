using DES.FeistelImplementation;
using DES.HelpFunctions;
using DES.InterfacesDES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES
{
    public class DESImplementation : ISymmetricEncryption
    {
        private readonly FeistelNetwork _feistel;

        public DESImplementation(FeistelNetwork feistel){
            _feistel = feistel;
        }

        public byte[] encrypt(ref byte[] bytes)
        {
            CryptSimpleFunctions.permutation(ref bytes, DESStandartBlocks.IPBlock);
            byte[] cipher = _feistel.execute(in bytes, 64);
            CryptSimpleFunctions.permutation(ref cipher, DESStandartBlocks.InvIPBlock);
            return cipher;
        }


        public byte[] decrypt(ref byte[] bytes)
        { 
            CryptSimpleFunctions.permutation(ref bytes, DESStandartBlocks.IPBlock);
            _feistel.reverseRaundKeys();
            byte[] plainText = _feistel.execute(in bytes, 64);
            CryptSimpleFunctions.permutation(ref plainText, DESStandartBlocks.InvIPBlock);
            return plainText;
        }

        
    }
}
