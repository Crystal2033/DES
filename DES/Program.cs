using DES;
using DES.FeistelImplementation;
using DES.HelpFunctions;
using DES.InterfacesDES;
using DES.KeyManipulations;
using DES.SubBytes;

internal class Program
{
    private static void Main(string[] args)
    {
        //byte[] myTextBytes = new byte[8] { 117, 85, 93, 53, 78, 146, 200, 190 };

        //CryptSimpleFunctions.Permutation(ref myTextBytes, DESStandartBlocks.IPBlock);

        //CryptSimpleFunctions.Permutation(ref myTextBytes, DESStandartBlocks.InvIPBlock);

        //byte[] mySwapBytes = new byte[6] { 64, 255, 211, 135, 67, 91 };
        //AbstractSubBytes SubBytes = new DesSybBytes();
        //SubBytes.SubBytes(ref mySwapBytes, 48, 6, in DESStandartBlocks.SMatrix, 4);
        //CryptSimpleFunctions.ShowBinaryView(mySwapBytes, $"Result");

        //RaundKeysGenerator dESKeysGenerator = new DESKeysGenerator();
        //byte[] mainKey = new byte[7] { (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6', (byte)'A' };
        ////CryptSimpleFunctions.ShowBinaryView(mainKey, "Input key:");
        //var listOfKeys = dESKeysGenerator.GenerateRoundKeys(mainKey);

        //byte[] partOfText = new byte[4] { (byte)'1', (byte)'2', (byte)'3', (byte)'4'};
        //byte[] raundKey = new byte[6] { 45, 74, 5, 165, 69, 41 };
        //DESFeistelFunction dESFeistelFunction = new DESFeistelFunction();
        //byte[] resOfFeistel = dESFeistelFunction.FeistelFunction(ref partOfText, raundKey);


        //byte[] test = new byte[] { 6 };
        //CryptSimpleFunctions.ShowBinaryView(test);
        //CryptSimpleFunctions.SetBitOnPos(ref test[0], 2, 1);
        //CryptSimpleFunctions.ShowBinaryView(test);
        //for(int i = 0; i < 8; i++)
        //{
        //    Console.WriteLine(CryptSimpleFunctions.GetBitFromPos(test[0], (byte)i));

        //}


        byte[] mainKey = new byte[7] { (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6', (byte)'A' };
        IKeyExpansion keyExpansion = new DESKeysGenerator();
        IFeistelFunction feistelFunction = new DESFeistelFunction();
        FeistelNetwork feistelKernel = new FeistelNetwork(keyExpansion, feistelFunction) { MainKey= mainKey };
        DESImplementation desImpl = new DESImplementation(feistelKernel);

        byte[] myTextBytes = new byte[8] { 117, 85, 93, 53, 78, 146, 200, 190 };
        CryptSimpleFunctions.ShowBinaryView(myTextBytes, "Plain text");
        byte[] cipher = desImpl.Encrypt(ref myTextBytes);
        CryptSimpleFunctions.ShowBinaryView(cipher, "Cypher");
        byte[] plainText = desImpl.Decrypt(ref cipher);
        CryptSimpleFunctions.ShowBinaryView(plainText, "Text is back");




    }
}