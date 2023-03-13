using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.InterfacesDES
{
    internal interface ISymmetricEncryption
    {
        public byte[] encrypt(in byte[] bytes, in byte[] raundKey);
        public byte[] decrypt(in byte[] bytes, in byte[] raundKey);
    }
}
