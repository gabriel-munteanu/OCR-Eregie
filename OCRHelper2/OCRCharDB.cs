using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OCRHelper2;
using System.Runtime.Serialization;

namespace CaptchaOCR
{
    [DataContract]
    class OCRCharDB
    {
        [DataMember]
        Dictionary<char, OCRChar> DB = new Dictionary<char, OCRChar>();

        public void AddToDB(char c, Bitmap img)
        {
            if (DB.ContainsKey(c))
                DB[c].AddLetter(img);
            else
                DB[c] = new OCRChar(c, img);
        }

        public char Identify(Bitmap img)
        {
            char car = '-';
            double identified = 0;

            byte[] byte_img = OCRChar.BitmapToByteArray(img);

            foreach (var pair in DB)
            {
                double cur_val = pair.Value.IdentifyImg(byte_img);
                if (identified < cur_val)
                {
                    car = pair.Key;
                    identified = cur_val;
                }                    
            }

            return car;
        }
    }
}
