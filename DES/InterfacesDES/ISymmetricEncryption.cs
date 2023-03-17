using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.InterfacesDES
{
    public interface ISymmetricEncryption
    {
        public byte[] Encrypt(ref byte[] bytes);
        public byte[] Decrypt(ref byte[] bytes);
    }
}
