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
        virtual protected void MainKeyPreparation(in byte[] mainKey, out byte[] preparedKey)
        {
            preparedKey = (byte[])mainKey.Clone();
        }

        abstract protected List<byte[]> GenerateKeys(in byte[] mainKey);

        public List<byte[]> GenerateRoundKeys(in byte[] mainKey)
        {
            MainKeyPreparation(in mainKey, out byte[] preparedKey);
            return GenerateKeys(in preparedKey);
        }
    }
}
