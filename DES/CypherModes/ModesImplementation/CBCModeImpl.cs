using DES.InterfacesDES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.CypherModes.ModesImplementation
{
    public sealed class CBCModeImpl : CryptModeImpl
    {
        private byte[] _initVector;
        public CBCModeImpl(byte[] mainKey, ISymmetricEncryption algorithm, byte[] initVector) : base(mainKey, algorithm)
        {
            _initVector = initVector;
        }

        public override void DecryptWithMode(string fileToDecrypt, string decryptResultFile)
        {
            throw new NotImplementedException();
        }

        public override void EncryptWithMode(string fileToEncrypt, string encryptResultFile)
        {
            throw new NotImplementedException();
        }
    }
}
