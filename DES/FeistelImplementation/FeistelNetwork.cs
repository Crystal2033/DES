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
        private readonly byte[] _mainKey;
        public byte[] MainKey {
            get{
                return MainKey;
            } 
            init{
                _mainKey = value;
                _raundKeys = KeyExpander.generateRoundKeys(_mainKey);
            } }

        public IKeyExpansion KeyExpander { get; init; }
        public IFeistelFunction FeistelFunction { get; init; }
        public FeistelNetwork(IKeyExpansion keyExpander, IFeistelFunction feistelFunction){
            KeyExpander = keyExpander;
            FeistelFunction = feistelFunction;
        }

        public void reverseRaundKeys(){
            _raundKeys.Reverse();
        }

        public byte[] execute(in byte[] partOfText, int sizeInBits){ //not checked
            CryptSimpleFunctions.sliceArrayOnTwoArrays(partOfText, sizeInBits / 2, sizeInBits / 2, out byte[] leftPart, out byte[] rightPart);
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

            return CryptSimpleFunctions.concatTwoBitParts(nextLeftPart, sizeInBits / 2, nextRightPart, sizeInBits / 2);
        }

    }
}
