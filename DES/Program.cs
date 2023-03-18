using DES;
using DES.FeistelImplementation;
using DES.HelpFunctions;
using DES.HelpFunctionsAndData;
using DES.InterfacesDES;
using DES.KeyManipulations;
using DES.Presenters;
using DES.SubBytes;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        byte[] mainKey = new byte[7] { 123,43,135,23,233,231,23 };
        IKeyExpansion keyExpansion = new DESKeysGenerator();
        IFeistelFunction feistelFunction = new DESFeistelFunction();
        FeistelNetwork feistelKernel = new FeistelNetwork(keyExpansion, feistelFunction) { MainKey= mainKey };
        DESImplementation desImpl = new DESImplementation(feistelKernel);

        DemonstrationCypher demo = new DemonstrationCypher(@"C:\Paul\Programming\С#\DES\DES\MyFile.txt",
            @"C:\Paul\Programming\С#\DES\DES\encrypt.txt",
            @"C:\Paul\Programming\С#\DES\DES\decrypt.txt", desImpl);
        demo.encrypt();
        demo.decrypt();

        //byte[] myTextBytes = new byte[8] { 117, 85, 93, 53, 78, 146, 200, 190 };

        ////byte[] myText = Encoding.ASCII.GetBytes("He world from C# WPF and MVVM IS COOL iTS MY DATA ./,] 123890");


        //byte[] currentPart = new byte[8];
        //using (var reader = new StreamReader(@"C:\Paul\Programming\С#\DES\DES\MyFile.txt"))
        //{
        //    string line;
        //    while ((line = reader.ReadLine()) != null)
        //    {
        //        int textPartsCounter = 0;
        //        while (textPartsCounter * CryptConstants.DES_PART_TEXT_BYTES < line.Length)
        //        {
        //            CryptSimpleFunctions.GetNewPartOfText(Encoding.ASCII.GetBytes(line), currentPart, textPartsCounter * CryptConstants.DES_PART_TEXT_BYTES);
        //            textPartsCounter++;
        //            byte[] cipher = desImpl.Encrypt(ref currentPart);

        //            byte[] plainText = desImpl.Decrypt(ref cipher);
        //            int remainedBytes = line.Length - textPartsCounter * CryptConstants.DES_PART_TEXT_BYTES;
        //            if (remainedBytes < 0)
        //            {
        //                CryptSimpleFunctions.ClearBytes(ref plainText, CryptConstants.DES_PART_TEXT_BYTES - Math.Abs(remainedBytes));
        //            }
        //            Console.WriteLine(Encoding.ASCII.GetString(plainText));
        //        }
        //    }
        //}



    }
}