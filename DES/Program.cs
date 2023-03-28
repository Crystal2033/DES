using DES;
using DES.CypherModes;
using DES.FeistelImplementation;
using DES.HelpFunctions;
using DES.HelpFunctionsAndData;
using DES.InterfacesDES;
using DES.KeyManipulations;
using DES.Presenters;
using DES.SandBox;
using DES.SubBytes;
using log4net;
using System.Reflection;
using System.Text;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
internal class Program
{
    private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static void Main(string[] args)
    {
        byte[] mainKey = new byte[7] { 123,43,135,23,233,231,23 };
        //IKeyExpansion keyExpansion = new DESKeysGenerator();
        //IFeistelFunction feistelFunction = new DESFeistelFunction();

        //FeistelNetwork feistelKernel = new FeistelNetwork(keyExpansion, feistelFunction) { MainKey= mainKey };
        //ISymmetricEncryption desImpl = new DESImplementation(feistelKernel);

        //DemonstrationCypher demo = new DemonstrationCypher(desImpl);
        //demo.encrypt(@"D:\Paul\Programming\C#\DES\DES\Videos\2022-09-06 18-43-02.mp4", @"D:\Paul\Programming\C#\DES\DES\Videos\encrypt.mp4");
        //demo.decrypt(@"D:\Paul\Programming\C#\DES\DES\Videos\encrypt.mp4", @"D:\Paul\Programming\C#\DES\DES\Videos\decrypt.mp4");
        AdvancedCypherSym advancedCypherSym = new(mainKey, DES.CypherEnums.CypherMode.ECB, DES.CypherEnums.SymmetricAlgorithm.DES);
        advancedCypherSym.Encrypt(@"D:\Paul\Programming\C#\\DES\DES\TextCheck\MyFile.txt", @"D:\Paul\Programming\C#\DES\DES\TextCheck\EncryptMode.txt");
        Console.WriteLine("Encrypt is done");
        advancedCypherSym.Decrypt(@"D:\Paul\Programming\C#\\DES\DES\TextCheck\EncryptMode.txt", @"D:\Paul\Programming\C#\DES\DES\TextCheck\DecryptMode.txt");
        //ThreadBarierTesting testinObj = new();
        //Task.Run( async () =>
        //{
        //    testinObj.test();
        //}).Wait();
        Console.WriteLine("Decrypt is done");
        

    }
}