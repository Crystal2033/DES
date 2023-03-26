using DES.InterfacesDES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.CypherModes.ModesImplementation
{
    public abstract class CryptModeImpl : IModeEncryption
    {
        protected byte[] _mainKey;
        protected ISymmetricEncryption _cryptAlgorithm;

        public CryptModeImpl(byte[] mainKey, ISymmetricEncryption cryptAlgorithm)
        {
            _mainKey = mainKey;
            _cryptAlgorithm = cryptAlgorithm;
        }
        public abstract void DecryptWithMode(in string fileToDecrypt, in string decryptResultFile);


        public abstract void EncryptWithMode(in string fileToEncrypt, in string encryptResultFile);
        
    }
}
