using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FSTextool
{
    static class Dat
    {
        public static bool unpackf = false;
        public static bool repackf = false;

        public static void Unpack(string datFile)
        {
            int Lang = 0;
            List<string> Text = new List<string>();
            if (Program.m == "eng") Lang = 1;
            else if (Program.m == "all") Lang = 6;
            else
            {
                Console.WriteLine("按任意键退出");
                Console.ReadKey();
                Environment.Exit(0);
            }
            BinaryReader reader = new BinaryReader(File.OpenRead(datFile));
            reader.BaseStream.Seek(0x34, SeekOrigin.Begin);
            int dwStringsCount = reader.ReadInt32();
            for (int i = 0; i < dwStringsCount; i++)
            {
                int Pad = 0;
                int dwStringSize = 0;
                for (int j = 0; j < 2; j++)
                {
                    dwStringSize = reader.ReadInt32();
                    reader.ReadBytes(dwStringSize);
                    Pad = 4 - (dwStringSize % 4);
                    if (Pad > 0 & Pad < 4)
                    {
                        reader.ReadBytes(Pad);
                    }
                    reader.ReadInt32();
                }
                for (int j = 0; j < Lang; j++)
                {
                    dwStringSize = reader.ReadInt32();
                    Encoding encoding = Encoding.UTF8;
                    string String = encoding.GetString(reader.ReadBytes(dwStringSize))
                    .Replace("\r\n", "<cf>")
                    .Replace("\n", "<lf>")
                    .Replace("\r", "<cr>");
                    Text.Add(String); //LeHieu替代了swuforce的 字串 + crlf(cf)换行符的办法
                    Pad = 4 - (dwStringSize % 4);
                    if (Pad > 0 & Pad < 4)
                    {
                        reader.ReadBytes(Pad);
                    }
                }
                if (Lang == 1)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        dwStringSize = reader.ReadInt32();
                        reader.ReadBytes(dwStringSize);
                        Pad = 4 - (dwStringSize % 4);
                        if (Pad > 0 & Pad < 4)
                        {
                            reader.ReadBytes(Pad);
                        }
                    }
                }
                reader.ReadBytes(56);
            }
            string txtFile = Path.Combine(Path.GetDirectoryName(datFile), $"{Path.GetFileNameWithoutExtension(datFile)}.txt");
            File.WriteAllLines(txtFile, Text.ToArray());
            reader.Close();
            unpackf = true;
        }

        public static void Repack(string txtFile)
        {
            int Num = 0;
            int Lang = 0;
            string datFile = Path.Combine(Path.GetDirectoryName(txtFile), $"{Path.GetFileNameWithoutExtension(txtFile)}.dat");
            string newDatFile = Path.Combine(Path.GetDirectoryName(txtFile), $"NEW_{Path.GetFileNameWithoutExtension(txtFile)}.dat");
            string[] text = File.ReadAllLines(txtFile);
            BinaryReader reader = new BinaryReader(File.OpenRead(datFile));
            if (File.Exists(newDatFile)) File.Delete(newDatFile);
            BinaryWriter Newfile = new BinaryWriter(File.OpenWrite(newDatFile));
            reader.BaseStream.Seek(0x34, SeekOrigin.Begin);
            int dwStringsCount = reader.ReadInt32();
            if (text.Length == dwStringsCount) Lang = 1;
            else Lang = 6;
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            Newfile.Write(reader.ReadBytes(56));
            for (int i = 0; i < dwStringsCount; i++)
            {
                int Pad = 0;
                int dwStringSize = 0;
                for (int j = 0; j < 2; j++)
                {
                    dwStringSize = reader.ReadInt32();
                    Pad = 4 - (dwStringSize % 4);
                    if (Pad > 0 & Pad < 4)
                    { 
                        Newfile.Write(dwStringSize);//au3用&= 和write的作用应该相同 无赋值操作
                        Newfile.Write(reader.ReadBytes(dwStringSize));
                        Newfile.Write(reader.ReadBytes(Pad));
                    }
                    else
                    {
                        Newfile.Write(dwStringSize);
                        Newfile.Write(reader.ReadBytes(dwStringSize));
                    }
                    Newfile.Write(reader.ReadInt32());
                }
                for (int j = 0; j < Lang; j++, Num++)
                {
                    dwStringSize = reader.ReadInt32();
                    reader.ReadBytes(dwStringSize);
                    Pad = 4 - (dwStringSize % 4);
                    if (Pad > 0 & Pad < 4)
                    {
                        reader.ReadBytes(Pad);
                    }
                    string str = text[Num]
                        .Replace("<cf>", "\r\n")
                        .Replace("<lf>", "\n")
                        .Replace("<cr>", "\r");
                    byte[] NewText = Encoding.UTF8.GetBytes(str);
                    int Len = NewText.Length;
                    Newfile.Write(Len);
                    Newfile.Write(NewText);
                    Pad = 4 - (Len % 4);
                    if (Pad > 0 & Pad < 4)
                    {
                        for (int k = 0; k < Pad; k++)
                        {
                            byte zero = 0;
                            Newfile.Write(zero);
                        }
                    }
                }
                if (Lang == 1)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        dwStringSize = reader.ReadInt32();
                        Pad = 4 - (dwStringSize % 4);
                        if (Pad > 0 & Pad < 4)
                        {
                            Newfile.Write(dwStringSize);
                            Newfile.Write(reader.ReadBytes(dwStringSize));
                            Newfile.Write(reader.ReadBytes(Pad));
                        }
                        else
                        {
                            Newfile.Write(dwStringSize);
                            Newfile.Write(reader.ReadBytes(dwStringSize));
                        }
                    }
                }
                Newfile.Write(reader.ReadBytes(56));
            }
            Newfile.Write(reader.ReadBytes(196));
            reader.Close();
            Newfile.Close();
            repackf = true;
        }
    } 
}
