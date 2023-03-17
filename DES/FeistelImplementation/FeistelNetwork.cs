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
            CryptSimpleFunctions.ShowBinaryView(partOfText, "Before slice");
            CryptSimpleFunctions.SliceArrayOnTwoArrays(partOfText, sizeInBits / 2, sizeInBits / 2, out byte[] leftPart, out byte[] rightPart);
            CryptSimpleFunctions.ShowBinaryView(leftPart, "L0");
            CryptSimpleFunctions.ShowBinaryView(rightPart, "R0");
            byte[] nextLeftPart = default;
            byte[] nextRightPart = default;

            for(int i = 0; i < _valueOfRaunds; i++){
                nextLeftPart = (byte[])rightPart.Clone();
                CryptSimpleFunctions.ShowBinaryView(nextLeftPart, $"L{i + 1}");
                nextRightPart = CryptSimpleFunctions.XorByteArrays(leftPart, 
                    FeistelFunction.FeistelFunction(ref rightPart, _raundKeys[(cryptStatus == CryptStatus.ENCRYPT) ? i : _valueOfRaunds - i -1]));
                CryptSimpleFunctions.ShowBinaryView(nextRightPart, $"R{i + 1}");

                leftPart = nextLeftPart;
                rightPart = nextRightPart;
                CryptSimpleFunctions.ShowBinaryView(leftPart, $"L{i}");
                CryptSimpleFunctions.ShowBinaryView(rightPart, $"R{i}");
            }

            //nextLeftPart = CryptSimpleFunctions.XorByteArrays(leftPart, 
            //    FeistelFunction.FeistelFunction(ref rightPart, _raundKeys[(cryptStatus == CryptStatus.ENCRYPT) ? _valueOfRaunds - 1 : 0]));
            //nextRightPart = rightPart;
            //CryptSimpleFunctions.ShowBinaryView(nextLeftPart, $"Last left part");
            //CryptSimpleFunctions.ShowBinaryView(nextRightPart, $"Last right part");

            return CryptSimpleFunctions.ConcatTwoBitParts(rightPart, sizeInBits / 2, leftPart, sizeInBits / 2);
        }

    }
}
