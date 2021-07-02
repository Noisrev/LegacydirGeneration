using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LegacydirGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Any())
            {
                foreach (string path in args)
                {
                    if (Directory.Exists(path))
                    {
                        List<string> files = new List<string>();
                        GetFiles(path, ref files);

                        using BinaryWriter bw = new BinaryWriter(File.Create($"{path}\\{Path.GetFileName(path)}.legacydirlistinfo"));

                        bw.Write(files.Count);
                        foreach (var item in files.Select(x =>
                        {
                            return x.Replace(path, "")[1..].Replace("\\", "/");
                        }))
                        {
                            byte[] buffer = Encoding.UTF8.GetBytes(item);
                            bw.Write(buffer.Length);
                            bw.Write(buffer);
                        }
                        bw.Flush();
                        bw.Dispose();
                    }
                }
            }
        }
        static void GetFiles(string directory, ref List<string> list)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            foreach (FileInfo info in directoryInfo.GetFiles())
            {
                list.Add(info.FullName);
            }
            foreach (DirectoryInfo info in directoryInfo.GetDirectories())
            {
                GetFiles(info.FullName, ref list);
            }
        }
    }
}
