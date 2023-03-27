using DES.CypherEnums;
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

        private byte[] MakeCryptOperation(ref byte[] bytes, CryptOperation cryptStatus)
        {
            CryptSimpleFunctions.Permutation(ref bytes, DESStandartBlocks.IPBlock);
            byte[] value = _feistel.Execute(in bytes, 64, cryptStatus);
            CryptSimpleFunctions.Permutation(ref value, DESStandartBlocks.InvIPBlock);
            return value;
        }
        public byte[] Encrypt(ref byte[] bytes)
        {
            return MakeCryptOperation(ref bytes, CryptOperation.ENCRYPT);
        }


        public byte[] Decrypt(ref byte[] bytes)
        {
            return MakeCryptOperation(ref bytes, CryptOperation.DECRYPT);
        }

        
    }
}
