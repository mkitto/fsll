using System;
using System.IO;

namespace FSTextool
{
    class Program
    {
        public static string m;
        static void Main(string[] args)
        {
            Console.Title = "FSTextool, based on tool by swuforce, LeHieu, Qchan";
            if (args.Length > 0)
            {
                foreach (string input in args)
                {
                    string ext = Path.GetExtension(input).ToLower();
                    string name = Path.GetFileName(input);

                    switch (ext)
                    {
                        case ".dat":
                            Console.Write("eng - 英 \nall - 所有 \n请输入:");
                            m = Console.ReadLine();
                            Interaction.Unpacki(input);
                            break;
                        case ".txt":
                            Interaction.Repacki(input);
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Skip: {name}. Reason: 文件格式不支持.");
                            Console.ReadKey();
                            break;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("按任意键退出");
                Console.ReadKey();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("拖拽 Dat/Txt 文件到此exe上(无需双击打开) \n来 【解包】/【打包(原dat需存在)】，\n按任意键退出");
                Console.ReadKey();
            }
        }

    }
}
