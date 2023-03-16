using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.InterfacesDES
{
    internal interface IFeistelNetwork
    {
        public byte[] feistelTransform(byte[] bytes, in byte[] raundKey);
    }
}
