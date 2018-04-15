using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;
using CaptchaOCR;
using System.Diagnostics;

namespace OCR
{
    public partial class Form2 : Form
    {
        OCRCharDB ocd;
        public Form2()
        {
            InitializeComponent();
            DataContractSerializer ds = new DataContractSerializer(typeof(OCRCharDB));
            ocd = (OCRCharDB)ds.ReadObject(new StreamReader("e:/OCRDB.db").BaseStream);
        }
        int nr_p=0;
        void recognise()
        {
            WebClient wc = new WebClient();
            wc.DownloadString("http://www.eregie.pub.ro/index.php?pgc=autentificare");
            wc.Headers.Set("Cookie", wc.ResponseHeaders.Get("Set-Cookie"));
            wc.Headers.Set("Referer", "www.eregie.pub.ro/index.php?pgc=autentificare");
            wc.Headers.Set("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

            Bitmap captcha = (Bitmap)Bitmap.FromStream(wc.OpenRead("http://www.eregie.pub.ro/image_code.jpg"));
            
            //Bitmap captcha = (Bitmap)Bitmap.FromFile("E:/ocr_in/image_code" + nr_p++ + ".jpg");
            picture_Original.Image = captcha;

            ERegieCaptchaOCR co = new ERegieCaptchaOCR(captcha);
            captcha = co.PregatesteImagine();
            picture_Noiseless.Image = captcha;

            ERegieCaptchaOCR co2 = new ERegieCaptchaOCR(captcha);        
            picture_Rotated.Image = co2.PregatesteImagine2();

            List<Rectangle> chars = co2.CautaCaractere();

            label1.Text = "";

            for (int i = 0; i < chars.Count; i++)
            {
                char c = ocd.Identify(Form1.cropImage((Bitmap)(picture_Rotated.Image), chars[i]));
                label1.Text += (c>='a'?(char)(c-32):c);

                Debug.WriteLine("{0} - {1}", c, chars[i].Width);
            }
        }

        private void Form2_KeyPress(object sender, KeyPressEventArgs e)
        {
            recognise();
        }
    }
}
