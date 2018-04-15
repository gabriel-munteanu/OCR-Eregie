using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CaptchaOCR;

namespace OCR
{
    public partial class Form1 : Form
    {
        Bitmap car, captcha;
        int pos = 0;
        List<Rectangle> chars = new List<Rectangle>();
        

        public Form1()
        {
            InitializeComponent();
           /* Bitmap poza = new Bitmap("e:/img.jpg");
            CaptchaOCR co = new CaptchaOCR(poza);
            poza = co.PregatesteImagine();
            
            pictureb_Preview.Image = poza;
            poza.Save("e:/out.png", ImageFormat.Png);*/  
            //pictureb_Preview.Image = poza;
            GetChar();
        }

        int nr_poza = 1;
        void GetChar()
        {
            if (pos == chars.Count)
            {
                //captcha = new Bitmap("e:/img.jpg");
                WebClient wc = new WebClient();
                wc.DownloadString("http://www.eregie.pub.ro/index.php?pgc=autentificare");
                wc.Headers.Set("Cookie", wc.ResponseHeaders.Get("Set-Cookie"));
                wc.Headers.Set("Referer", "www.eregie.pub.ro/index.php?pgc=autentificare");
                wc.Headers.Set("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                
                //wc.DownloadFile("http://www.eregie.pub.ro/image_code.jpg", "E:/captcha.jpg");
                //return;
                captcha = (Bitmap)Bitmap.FromStream(wc.OpenRead("http://www.eregie.pub.ro/image_code.jpg"));
                pictureb_Preview.Image = captcha;
                //return;

                ERegieCaptchaOCR co = new ERegieCaptchaOCR(captcha);
                captcha = co.PregatesteImagine();
                chars = co.CautaCaractere();
                pos = 0;
                nr_poza++;
            }
            car = cropImage(captcha, chars[pos++]);
            pictureb_Preview.Image = car;
        }

        public static Bitmap cropImage(Bitmap img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (bmpCrop);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string name = e.KeyChar+MD5Hash.GetMd5Hash(DateTime.Now.ToString() + pos) + ".png";
            
            car.Save("E:/SkyDrive/ocr/" + name, ImageFormat.Png);
            GetChar();
        }

        
    }
}
