using DES.InterfacesDES;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DES.CypherModes.ModesImplementation
{
    public sealed class CTRModeImpl : CryptModeImpl
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private byte[] _initVector;

        public CTRModeImpl(byte[] mainKey, ISymmetricEncryption algorithm, byte[] initVector) : base(mainKey, algorithm)
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
