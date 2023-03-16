using DES.HelpFunctions;
using DES.InterfacesDES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES.FeistelImplementation
{
    public class FeistelNetwork
    {

        private List<byte[]> _raundKeys;
        private readonly int _valueOfRaunds = 16;

        public List<byte[]> RaundKeys
        {
            get { return _raundKeys; }
        }

        public byte[] MainKey {
            get{
                return MainKey;
            } 
            init{
                MainKey = value;
                _raundKeys = KeyExpander.generateRoundKeys(MainKey);
            } }

        public IKeyExpansion KeyExpander { get; init; }
        public IFeistelFunction FeistelFunction { get; init; }
        public FeistelNetwork(IKeyExpansion keyExpander, IFeistelFunction feistelFunction){
            KeyExpander = keyExpander;
            FeistelFunction = feistelFunction;
        }

        public byte[] execute(in byte[] partOfText, int sizeInBits){
            CryptSimpleFunctions.sliceArrayOnTwoArrays(partOfText, sizeInBits, sizeInBits, out byte[] leftPart, out byte[] rightPart);
            byte[] nextLeftPart;
            byte[] nextRightPart;

            for(int i = 0; i < _valueOfRaunds - 1; i++){
                nextLeftPart = (byte[])rightPart.Clone();
                nextRightPart = CryptSimpleFunctions.xorByteArrays(leftPart, FeistelFunction.feistelFunction(ref rightPart, _raundKeys[i]));

                leftPart = nextLeftPart;
                rightPart = nextRightPart;
            }

            nextLeftPart = CryptSimpleFunctions.xorByteArrays(leftPart, FeistelFunction.feistelFunction(ref rightPart, _raundKeys[_valueOfRaunds-1]));
            nextRightPart = rightPart;

            return CryptSimpleFunctions.concatTwoBitParts(nextLeftPart, 32, nextRightPart, 32);
        }

    }
}
