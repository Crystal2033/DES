using DES.HelpFunctions;
    
internal class Program
{
    private static void Main(string[] args)
    {
        byte[] myTextBytes = new byte[8];
        for (byte i = 0; i < myTextBytes.Length; i++) {
            myTextBytes[i] = i;
        }
        byte[] pBlock = new byte[64];
        CryptSimpleFunctions.Permutation(ref myTextBytes, pBlock);
    }
}