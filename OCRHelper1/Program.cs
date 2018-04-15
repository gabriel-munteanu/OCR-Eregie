using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRHelper1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(@"E:\SkyDrive\ocr");
            Dictionary<char, int> chars = new Dictionary<char, int>();

            foreach (string file in files)
            {
                string file_name = file.Substring(file.LastIndexOf('\\') + 1);
                char p = file_name[0];
                if (p != '-' && p != '=')
                    if (chars.ContainsKey(p))
                        chars[p]++;
                    else
                        chars.Add(p, 1);
            }

            for (char i = 'a'; i <= 'z'; i++)
                if (chars.ContainsKey(i))
                    Console.WriteLine(i+" "+chars[i]);
                else
                    Console.WriteLine(i + "  -");
            for (char i = '0'; i <= '9'; i++)
                if (chars.ContainsKey(i))
                    Console.WriteLine(i + " " + chars[i]);
                else
                    Console.WriteLine(i + "  -");
            Console.ReadKey();
        }
    }
}
