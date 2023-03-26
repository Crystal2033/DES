using DES.InterfacesDES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.CypherModes.ModesImplementation
{
    public class ECBModeImpl : CryptModeImpl
    {
        public ECBModeImpl(byte[] mainKey, ISymmetricEncryption cryptAlgorithm) : base(mainKey, cryptAlgorithm)
        {

        }
        public override void DecryptWithMode(in string fileToDecrypt, in string decryptResultFile)
        {
            //TODO: ECB
            throw new NotImplementedException();
        }

        public override void EncryptWithMode(in string fileToEncrypt, in string encryptResultFile)
        {
            //TODO: ECB
            throw new NotImplementedException();
        }
    }
}
