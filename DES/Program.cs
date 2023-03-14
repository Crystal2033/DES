using DES.HelpFunctions;
using DES.SubBytes;

internal class Program
{
    private static void Main(string[] args)
    {
        byte[] myTextBytes = new byte[8] { 117, 85, 93, 53, 78, 146, 200, 190 };

        CryptSimpleFunctions.permutation(ref myTextBytes, DESStandartBlocks.IPBlock);

        CryptSimpleFunctions.permutation(ref myTextBytes, DESStandartBlocks.InvIPBlock);

        //byte[] mySwapBytes = new byte[6] { 64, 255, 211, 135, 67, 91 };
        //AbstractSubBytes subBytes = new DesSybBytes();
        //subBytes.subBytes(ref mySwapBytes, 48, 6, in DESStandartBlocks.SMatrix, 4);
        //CryptSimpleFunctions.showBinaryView(mySwapBytes, $"Result");
    }
}