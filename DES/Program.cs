using DES.HelpFunctions;
    
internal class Program
{
    private static void Main(string[] args)
    {
        byte[] myTextBytes = new byte[8] {117,85,93,53, 78,146,200,190};
 
        CryptSimpleFunctions.Permutation(ref myTextBytes, DESStandartBlocks.IPBlock);

        CryptSimpleFunctions.Permutation(ref myTextBytes, DESStandartBlocks.InvIPBlock);
    }
}