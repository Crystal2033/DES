using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.InterfacesDES
{
    internal interface IKeyExpansion
    {
        public List<byte[]> generateRoundKeys(in byte[] mainKey);
    }
}
