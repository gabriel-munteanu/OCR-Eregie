using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR
{
    class TestCode
    {
        Bitmap poza;        
        int[,] matr = new int[50,150];
        int nr = 0;

        Point a = new Point(500, 500);
        Point b = new Point(500, 500);
        Point c = new Point(500, 500);

        public TestCode()
        {
            
            poza = new Bitmap("e:/img.jpg");
            Binarizare(0.09f);
            EliminaMargine(1);

            EliminaZgomot2();
            EliminaRamasite();
            double unghi = Unghi();

            MatrToImg();
            Bitmap poza_t = new Bitmap(poza);
            using (Graphics g = Graphics.FromImage(poza))
            {
                
                g.RotateTransform((float)unghi);
                g.Clear(Color.White);
                g.DrawImage(poza_t, new Point(0, 0));

            }
            Binarizare(0.5f);
            EliminaZgomot2();
            MatrToImg();
            //Delimitare();//NEW

            /*Binarizare();
            EliminaMargine(5);

            EliminaZgomot2();
            EliminaRamasite();
            MatrToImg();*/
            //
            
            /*pictureb_Preview.Image = poza;
            poza.Save("e:/out.png", ImageFormat.Png);  
            pictureb_Preview.Image = poza;*/
        }

        void Delimitare()
        {
            int limSup = 0;
            int limInf = 49;
            
            limInf = limSup + 20;

            bool caracter = false;
            int inceput = 0, sfarsit = 0;
            for (int j = 0; j < 150; j++)
            {
                bool coloanaGoala = true;
                for (int i = 0; i < 50; i++)
                    if (caracter == false)
                    {
                        if (matr[i, j] == 1)
                        {
                            caracter = true;
                            inceput = j;
                        }
                    }
                    else
                        if (matr[i, j] == 1)
                            coloanaGoala = false;
                if(caracter == true)//daca sunt in interiorul caracterului caut sfarsitul
                    if (coloanaGoala == true)
                    {
                        sfarsit = j - 1;
                        caracter = false;

                        limSup = LimSuperioara(inceput, sfarsit);
                        limInf = limSup + 20;

                        Debug.WriteLine("Caracter: " + inceput + " " + sfarsit + " " + (sfarsit - inceput));

                        string name = MD5Hash.GetMd5Hash(DateTime.Now.ToString() + inceput) + ".png";
                        Image car = cropImage(poza, new Rectangle(inceput, limSup, sfarsit - inceput, limInf - limSup));
                        car.Save("E:/SkyDrive/ocr/" + name, ImageFormat.Png);
                    }
            }
        }

        int LimSuperioara(int limStanga, int limDreapta)
        {
            for (int i = 0; i < 50; i++)
                for (int j = limStanga; j <= limDreapta; j++)
                    if (matr[i, j] == 1)                    
                        return i;
            return 0;         
                    
        }

        static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }

        #region Obligatorii
        void Binarizare(float factor)
        {
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 150; j++)
                {
                    Color clr = poza.GetPixel(j, i);
                    if (clr.GetBrightness() > factor)
                        matr[i, j] = 0;//poza.SetPixel(j, i, Color.White);
                    else
                        matr[i, j] = 1;//poza.SetPixel(j, i, Color.Black);
                }

            
            
        }
        void MatrToImg()
        {
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 150; j++)
                    if (matr[i, j] == 0)
                        poza.SetPixel(j, i, Color.White);
                    else
                        poza.SetPixel(j, i, Color.Black);
        }

        void EliminaMargine( int pixeli)
        {
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 150; j++)                                
                    for(int k=0; k<pixeli; k++)
                    {
                        if(j == 0)
                            matr[i, j+k] = 0;
                        if(j == 149)
                            matr[i, j-k] = 0;
                        if(i == 0)
                            matr[i+k, j] = 0;
                        if(i == 49)
                            matr[i-k, j] = 0;
                    }                        

        }
        #endregion

        #region Folositoare
        void EliminaZgomot2()//daca avem un element fara vecini pe linie sau coloana il eliminam
        {
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 150; j++)
                    if (matr[i, j] == 1)
                    {
                        if (matr[i - 1, j] == 0 && matr[i + 1, j] == 0)
                        {
                            matr[i, j] = 0;
                            continue;
                        }
                        if (matr[i, j - 1] == 0 && matr[i, j + 1] == 0)
                        {
                            matr[i, j] = 0;
                            continue;
                        }
                    }                       
        }

        void Umple2(int i, int j)
        {
            
            matr[i, j] = 2;
            nr++;
            if (matr[i + 1, j] == 1)
                Umple2(i + 1, j);
            if (matr[i - 1, j] == 1)
                Umple2(i - 1, j);
            if (matr[i, j + 1] == 1)
                Umple2(i, j + 1);
            if (matr[i, j - 1] == 1)
                Umple2(i, j - 1);
            return;
        }

        void Umple0(int i, int j)
        {

            matr[i, j] = 0;
            nr++;
            if (matr[i + 1, j] != 0)
                Umple0(i + 1, j);
            if (matr[i - 1, j] != 0)
                Umple0(i - 1, j);
            if (matr[i, j + 1] != 0)
                Umple0(i, j + 1);
            if (matr[i, j - 1] != 0)
                Umple0(i, j - 1);
            return;
        }

        void EliminaRamasite()
        {
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 150; j++)
                {
                    if (matr[i, j] == 1)
                    {
                        nr = 0;
                        Umple2(i, j);
                        if (nr <= 16)
                            Umple0(i, j);
                    }
                }
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 150; j++)
                    if (matr[i, j] != 0)
                        matr[i, j] = 1;

        }

        #endregion


        double Unghi()
        {
            
            bool exit = false;
            bool poz = false;//unghi pozitiv
            int count = 6;
            //caut punctul din stanga sus al textului
            for (int i = 0; i < 150 && !exit; i++)
                for (int j = 0; j < 50; j++)
                    if (matr[j, i] == 1)
                    {
                        count--;
                        if(j < a.Y)
                            a = new Point(i, j);
                        if (count == 0)
                        {
                            exit = true;
                            break;
                        }
                        i++;
                        j = 0;
                    }

            //caut punctul din dreapta sus al textului
            count = 6;
            exit = false;
            for (int i = 149; i > 0 && !exit; i--)
                for (int j = 0; j < 50; j++)
                    if (matr[j, i] == 1)
                    {
                        count--;
                        if (j < b.Y)
                            b = new Point(i, j);
                        if (count == 0)
                        {
                            exit = true;
                            break;
                        }
                        i--;
                        j = 0;

                    }

            if (a.Y < b.Y)
            {
                c = new Point(b.X, a.Y);
                poz = false;
            }
            else
            {
                c = new Point(a.X, b.Y);
                poz = true;
            }
            double unghi;
            if (poz)
                unghi = Math.Atan((a.Y-c.Y)/(double)(b.X-c.X));
            else
                unghi = -Math.Atan((b.Y - c.Y) / (double)(b.X - a.X));

            unghi = unghi * 180 / Math.PI;
            return unghi;
        }

        int sumaV(int y, int x)//suma vecinilor
        {
            int suma = 0;
            for (int i = y - 1; i <= y + 1; i++)
                for (int j = x - 1; j <= x + 1; j++)
                {
                    if (i == y && x == j)
                        continue;
                    if (i < 0 || j < 0)
                        continue;
                    if (i >= 50 || j >= 150)
                        continue;
                    suma += matr[i, j];
                }
            return suma;
        }

        void EliminaZgomot()//daca suma vecinilor e mai mica sau egala cu 2 se elimina elementul
        {
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 150; j++)
                    if (matr[i, j] == 1 && sumaV(i, j) <= 2)
                        matr[i, j] = 0;
        }
    }
}
