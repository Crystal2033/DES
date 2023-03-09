using DES.HelpFunctions;
    
internal class Program
{
    private static void Main(string[] args)
    {
        byte[] myTextBytes = new byte[8] {117,85,93,53, 78,146,200,190};
        //byte[] myTextBytes = new byte[8] { 1,2,3,4,5,6,'A','B','C','D',1,3,2,5,3,6 };
        Random random = new Random();
        //for (byte i = 0; i < myTextBytes.Length; i++) {
        //    myTextBytes[i] = (byte)random.Next(0, 255);

 
        CryptSimpleFunctions.Permutation(ref myTextBytes, DESStandartBlocks.IPBlock);

        CryptSimpleFunctions.Permutation(ref myTextBytes, DESStandartBlocks.InvIPBlock);
    }
}