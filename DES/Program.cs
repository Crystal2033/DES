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

        //CryptSimpleFunctions.permutation(ref myTextBytes, DESStandartBlocks.IPBlock);

        //CryptSimpleFunctions.permutation(ref myTextBytes, DESStandartBlocks.InvIPBlock);

        //byte[] mySwapBytes = new byte[6] { 64, 255, 211, 135, 67, 91 };
        //AbstractSubBytes subBytes = new DesSybBytes();
        //subBytes.subBytes(ref mySwapBytes, 48, 6, in DESStandartBlocks.SMatrix, 4);
        //CryptSimpleFunctions.showBinaryView(mySwapBytes, $"Result");

        //RaundKeysGenerator dESKeysGenerator = new DESKeysGenerator();
        //byte[] mainKey = new byte[7] { (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6', (byte)'A' };
        ////CryptSimpleFunctions.showBinaryView(mainKey, "Input key:");
        //var listOfKeys = dESKeysGenerator.generateRoundKeys(mainKey);

        //byte[] partOfText = new byte[4] { (byte)'1', (byte)'2', (byte)'3', (byte)'4'};
        //byte[] raundKey = new byte[6] { 45, 74, 5, 165, 69, 41 };
        //DESFeistelFunction dESFeistelFunction = new DESFeistelFunction();
        //byte[] resOfFeistel = dESFeistelFunction.feistelFunction(ref partOfText, raundKey);


        //byte[] test = new byte[] { 6 };
        //CryptSimpleFunctions.showBinaryView(test);
        //CryptSimpleFunctions.setBitOnPos(ref test[0], 2, 1);
        //CryptSimpleFunctions.showBinaryView(test);
        //for(int i = 0; i < 8; i++)
        //{
        //    Console.WriteLine(CryptSimpleFunctions.getBitFromPos(test[0], (byte)i));

        //}


        byte[] mainKey = new byte[7] { (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6', (byte)'A' };
        IKeyExpansion keyExpansion = new DESKeysGenerator();
        IFeistelFunction feistelFunction = new DESFeistelFunction();
        FeistelNetwork feistelKernel = new FeistelNetwork(keyExpansion, feistelFunction) { MainKey= mainKey };
        DESImplementation desImpl = new DESImplementation(feistelKernel);

        byte[] myTextBytes = new byte[8] { 117, 85, 93, 53, 78, 146, 200, 190 };
        CryptSimpleFunctions.showBinaryView(myTextBytes, "Plain text");
        byte[] cipher = desImpl.encrypt(ref myTextBytes);
        CryptSimpleFunctions.showBinaryView(cipher, "Cypher");
        byte[] plainText = desImpl.decrypt(ref cipher);
        CryptSimpleFunctions.showBinaryView(plainText, "Text is back");




    }
}