using DES.CypherEnums;
using DES.CypherModes.ModesImplementation;
using DES.FeistelImplementation;
using DES.InterfacesDES;
using DES.KeyManipulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DES.CypherModes
{
    public sealed class AdvancedCypherSym
    {
        private byte[] _cypherKey;
        private CypherMode _mode;
        private byte[] _initVector;
        private object[] _modeParams;

        private CryptModeImpl _encriptionModeImpl;
        private ISymmetricEncryption _cryptAlgorithm;

        public AdvancedCypherSym(byte[] cypherKey, CypherMode mode, SymmetricAlgorithm symmetricAlgorithm, byte[] initVector = null, params object[] modeParams)
        {
            _cypherKey = cypherKey;
            _mode = mode;
            _initVector = initVector;
            _modeParams = modeParams;

            _cryptAlgorithm = GetSymmetricAlgorithm(symmetricAlgorithm);
            _encriptionModeImpl = GetModeImplementation();
        }

        private ISymmetricEncryption GetSymmetricAlgorithm(SymmetricAlgorithm algType)
        {
            switch (algType)
            {
                case SymmetricAlgorithm.DES:
                    IKeyExpansion keyExpansion = new DESKeysGenerator();
                    IFeistelFunction feistelFunction = new DESFeistelFunction();
                    FeistelNetwork feistelKernel = new FeistelNetwork(keyExpansion, feistelFunction) { MainKey = _cypherKey };
                    return new DESImplementation(feistelKernel);

                case SymmetricAlgorithm.RIJNDAEL:
                    //TODO:
                    return null;

                default:
                    return null;

            }
        } 
        private CryptModeImpl GetModeImplementation()
        {
            switch (_mode)
            {
                case CypherMode.ECB:
                    return new ECBModeImpl(_cypherKey, _cryptAlgorithm);
                case CypherMode.CBC:
                    return new CBCModeImpl(_cypherKey, _cryptAlgorithm, _initVector);
                case CypherMode.CFB:
                    return new CFBModeImpl(_cypherKey, _cryptAlgorithm, _initVector);
                case CypherMode.OFB:
                    return null;
                case CypherMode.CTR:
                    return null;
                case CypherMode.RD:
                    return null;
                case CypherMode.RDH:
                    return null;
                default:
                    return null;
            }
        }

        public void Encrypt(in string fileToEncrypt, in string encryptResultFile)
        {
            _encriptionModeImpl?.EncryptWithMode(fileToEncrypt, encryptResultFile);
        }

        public void Decrypt(in string fileToDecrypt, in string decryptResultFile)
        {
            _encriptionModeImpl?.DecryptWithMode(fileToDecrypt, decryptResultFile);
        }

    }
}
