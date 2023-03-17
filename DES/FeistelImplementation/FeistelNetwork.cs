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
        public enum CryptStatus
        {
            ENCRYPT, DECRYPT
        };

        private List<byte[]> _raundKeys;
        private readonly int _valueOfRaunds = 16;
        private readonly byte[] _mainKey;
        public byte[] MainKey {
            get{
                return MainKey;
            } 
            init{
                _mainKey = value;
                _raundKeys = KeyExpander.GenerateRoundKeys(_mainKey);
            } }

        public IKeyExpansion KeyExpander { get; init; }
        public IFeistelFunction FeistelFunction { get; init; }
        public FeistelNetwork(IKeyExpansion keyExpander, IFeistelFunction feistelFunction){
            KeyExpander = keyExpander;
            FeistelFunction = feistelFunction;
        }

        public byte[] Execute(in byte[] partOfText, int sizeInBits, CryptStatus cryptStatus)
        { //not checked
            CryptSimpleFunctions.SliceArrayOnTwoArrays(partOfText, sizeInBits / 2, sizeInBits / 2, out byte[] leftPart, out byte[] rightPart);
            byte[] nextLeftPart;
            byte[] nextRightPart;

            for(int i = 0; i < _valueOfRaunds - 1; i++){
                nextLeftPart = (byte[])rightPart.Clone();
                nextRightPart = CryptSimpleFunctions.XorByteArrays(leftPart, 
                    FeistelFunction.FeistelFunction(ref rightPart, _raundKeys[(cryptStatus == CryptStatus.ENCRYPT) ? i : _valueOfRaunds - i -1]));

                leftPart = nextLeftPart;
                rightPart = nextRightPart;
            }

            nextLeftPart = CryptSimpleFunctions.XorByteArrays(leftPart, 
                FeistelFunction.FeistelFunction(ref rightPart, _raundKeys[(cryptStatus == CryptStatus.ENCRYPT) ? _valueOfRaunds - 1 : 0]));
            nextRightPart = rightPart;

            return CryptSimpleFunctions.ConcatTwoBitParts(nextLeftPart, sizeInBits / 2, nextRightPart, sizeInBits / 2);
        }

    }
}
