using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CaptchaOCR
{
    [DataContract]
    class OCRChar
    {
        [DataMember]
        List<byte[]> chars = new List<byte[]>();
        [DataMember]
        List<double> magnitudes = new List<double>();
        [DataMember]
        int nr_elem;
        [DataMember]
        char letter;
        public char Letter { get { return letter; } set { } }

        public OCRChar(char letter, Bitmap img)
        {
            this.letter = letter;
            AddLetter(img);
        }

        public void AddLetter(Bitmap bletter)
        {  
            chars.Add(BitmapToByteArray(bletter));
            magnitudes.Add(Magnitude(chars[nr_elem++]));           
        }

        static double Magnitude(byte[] array)
        {
            double magnitude = 0;
            for (int i = 0; i < array.Length; i++)
                if (array[i] == 1)
                    magnitude++;
            return Math.Sqrt(magnitude);
        }

        public static byte[] BitmapToByteArray(Bitmap bmap)
        {
            byte[] arr = new byte[bmap.Width * bmap.Height];
            int arr_pos = 0;
            for (int j = 0; j < bmap.Width; j++)
                for (int i = 0; i < bmap.Height; i++)
                {
                    if (bmap.GetPixel(j, i).Name.Contains('0'))
                        arr[arr_pos++] = 1;
                    else
                        arr[arr_pos++] = 0;

                }

            return arr;
        }

        public double IdentifyImg(byte[] input)
        {
            double recognised = 0;
            double input_mag = Magnitude(input);
            for (int i = 0; i < chars.Count; i++)
            {
                int min = (input.Length < chars[i].Length ? input.Length : chars[i].Length);
                int sum = 0;
                for (int j = 0; j < min; j++)
                    sum += chars[i][j] * input[j];
                double rez = sum / (magnitudes[i] * input_mag);
                if (rez > recognised)
                    recognised = rez;
            }

            return recognised;
        }

    }
}
