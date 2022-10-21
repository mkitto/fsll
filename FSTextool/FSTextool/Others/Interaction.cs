using System;
using System.IO;

namespace FSTextool
{
    class Interaction
    {
        public static void Unpacki(string datFile)
        {
            string name = Path.GetFileName(datFile);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("开始解包: {0}", name);
            if (StartUnpack(datFile))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("解包完成: {0}", name);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("解包失败: {0}", name);
            }
        }

        public static void Repacki(string txtFile)
        {
            string name = Path.GetFileName(txtFile);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("开始打包: {0}", name);
            if (StartRepack(txtFile))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("打包完成: {0}", name);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("打包失败: {0}", name);
            }
        }



        static bool StartUnpack(string datFilename)
        {
            Dat.Unpack(datFilename);

            var outFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Path.GetFileNameWithoutExtension(datFilename)}.txt");
            Console.WriteLine("outFileName:" + outFileName);
            if (!File.Exists(outFileName))
            {
                return false;
            }
            else if (!Dat.unpackf)
            {
                return false;
            }
            return true;
        }

        static bool StartRepack(string txtFilename)
        {
            Dat.Repack(txtFilename);

            var outFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"NEW_{Path.GetFileNameWithoutExtension(txtFilename)}.dat");
            Console.WriteLine("outFileName:" + outFileName);
            if (!File.Exists(outFileName))
            {
                return false;
            }
            else if (!Dat.repackf)
            {
                return false;
            }
            return true;
        }
    }
}
