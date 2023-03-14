using DES.InterfacesDES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.KeyManipulations
{
    public abstract class RaundKeysGenerator : IKeyExpansion
    {
        virtual protected void mainKeyPreparing(in byte[] mainKey, out byte[] preparedKey)
        {
            preparedKey = (byte[])mainKey.Clone();
        }

        abstract protected List<byte[]> generateKeys(in byte[] mainKey);

        public List<byte[]> generateRoundKeys(in byte[] mainKey)
        {
            mainKeyPreparing(in mainKey, out byte[] preparedKey);
            return generateKeys(in preparedKey);
        }
    }
}
