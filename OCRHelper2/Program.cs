using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.Serialization;
using CaptchaOCR;

namespace OCRHelper2
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(@"E:\SkyDrive\ocr");
            Dictionary<char, int> chars = new Dictionary<char, int>();
            OCRCharDB ocd = new OCRCharDB();

            foreach (string file in files)
            {
                string file_name = file.Substring(file.LastIndexOf('\\') + 1);
                char p = file_name[0];
                if (p != '-' && p != '=')
                    ocd.AddToDB(p, new Bitmap(file));
            }
            DataContractSerializer ds = new DataContractSerializer(typeof(OCRCharDB));
            ds.WriteObject(new StreamWriter("E:/OCRDB.db").BaseStream, ocd);
        }

    }
}
