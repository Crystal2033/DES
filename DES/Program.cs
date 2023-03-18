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

        DemonstrationCypher demo = new DemonstrationCypher(desImpl);
        demo.encrypt(@"D:\Paul\Programming\C#\DES\DES\MyFile.txt", @"D:\Paul\Programming\C#\DES\DES\encrypt.txt");
        demo.decrypt(@"D:\Paul\Programming\C#\DES\DES\encrypt.txt", @"D:\Paul\Programming\C#\DES\DES\decrypt.txt");
    }
}