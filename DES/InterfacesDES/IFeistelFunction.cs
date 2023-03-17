using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.InterfacesDES
{
    public interface IFeistelFunction
    {
        public byte[] FeistelFunction(ref byte[] bytes, in byte[] raundKey);
    }
}
