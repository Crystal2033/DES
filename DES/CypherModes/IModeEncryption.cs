using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.CypherModes
{
    public interface IModeEncryption
    {
        public void EncryptWithMode(in string fileToEncrypt, in string encryptResultFile);
               
        public void DecryptWithMode(in string fileToDecrypt, in string decryptResultFile);
    }
}
