using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptchaOCR
{
    class ERegieCaptchaOCR
    {
        Bitmap poza;
        int[,] matr = new int[50, 150];
        int nrElementeSuprafata = 0;

        Point a = new Point(500, 500);
        Point b = new Point(500, 500);
        Point c = new Point(500, 500);

        public ERegieCaptchaOCR(Bitmap captcha)
        {
            poza = new Bitmap(captcha);
        }

        //Elimina zgomotul din imagine si roteste textul sa fie la orizontala
        public Bitmap PregatesteImagine()
        {
            Binarizare(0.2f);
            EliminaMargine(1);

            EliminaZgomot();
            EliminaRamasite(32);//elimin suprafetele mai mici de 32 pixeli
            /*double unghi = Unghi();

            MatrToImg();
            using(Bitmap poza_t = new Bitmap(poza))//rotesc textul
            using (Graphics g = Graphics.FromImage(poza))
            {

                g.RotateTransform((float)unghi);//rotirea este in sens orar
                g.Clear(Color.White);
                g.DrawImage(poza_t, new Point(0, 0));

            }
            Binarizare(0.5f);
            EliminaZgomot();//elimin pixelii 'singuratici'*/
            MatrToImg();

            return new Bitmap(poza);
        }

        public Bitmap PregatesteImagine2()
        {
            Binarizare(0.2f);
            EliminaMargine(1);

            EliminaZgomot();
            EliminaRamasite(32);//elimin suprafetele mai mici de 32 pixeli
            double unghi = Unghi();

            MatrToImg();
            using(Bitmap poza_t = new Bitmap(poza))//rotesc textul
            using (Graphics g = Graphics.FromImage(poza))
            {

                g.RotateTransform((float)unghi);//rotirea este in sens orar
                g.Clear(Color.White);
                g.DrawImage(poza_t, new Point(0, 0));

            }
            Binarizare(0.5f);
            EliminaZgomot();//elimin pixelii 'singuratici'
            MatrToImg();

            return new Bitmap(poza);
        }

        //Caut caracterele in poza si returnez 'coordonatele' lor
        public List<Rectangle> CautaCaractere()
        {
            List<Rectangle> caractere = new List<Rectangle>();
            int limSup = 0;
            int limInf = 49;

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
                //daca sunt in interiorul caracterului caut sfarsitul
                if (caracter == true)
                    if (coloanaGoala == true)
                    {
                        sfarsit = j - 1;
                        caracter = false;

                        limSup = LimSuperioara(inceput, sfarsit);
                        limInf = limSup + 20;
                        if (limInf >= 50)
                            limInf = 49;

                        if (sfarsit - inceput < 1)
                            sfarsit = inceput + 1;

                        caractere.Add(new Rectangle(inceput, limSup, sfarsit - inceput, limInf - limSup));
                    }
            }

            return caractere;
        }

        //Caut de unde incepe caracterul in partea superioara(cum privesc)
        //pe matrice caut invers
        int LimSuperioara(int limStanga, int limDreapta)
        {
            for (int i = 0; i < 50; i++)
                for (int j = limStanga; j <= limDreapta; j++)
                    if (matr[i, j] == 1)
                        return i;
            return 0;

        }

        //Crearea matricei binare din poza. Se iau in considerare pixelii cu o luminozitate
        //mai mica decat valoarea specificata
        void Binarizare(float luminozitate)
        {
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 150; j++)
                {
                    Color clr = poza.GetPixel(j, i);
                    if (clr.GetBrightness() > luminozitate)
                        matr[i, j] = 0;
                    else
                        matr[i, j] = 1;
                }



        }
        
        //Creare pozei din matricea binara
        void MatrToImg()
        {
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 150; j++)
                    if (matr[i, j] == 0)
                        poza.SetPixel(j, i, Color.White);
                    else
                        poza.SetPixel(j, i, Color.Black);
        }

        
        void EliminaMargine(int pixeli)
        {
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 150; j++)
                    for (int k = 0; k < pixeli; k++)
                    {
                        if (j == 0)
                            matr[i, j + k] = 0;
                        if (j == 149)
                            matr[i, j - k] = 0;
                        if (i == 0)
                            matr[i + k, j] = 0;
                        if (i == 49)
                            matr[i - k, j] = 0;
                    }

        }

        //Daca avem un element fara vecini pe linie sau coloana il eliminam
        void EliminaZgomot()
        {
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 150; j++)
                    if (matr[i, j] == 1 && i!=0 && i!=49 && j!=0 && j!=149)
                    {   //fara vecini pe coloana
                        if (matr[i - 1, j] == 0 && matr[i + 1, j] == 0)
                        {
                            matr[i, j] = 0;
                            continue;
                        }
                        //fara vecini pe linie
                        if (matr[i, j - 1] == 0 && matr[i, j + 1] == 0)
                        {
                            matr[i, j] = 0;
                            continue;
                        }
                    }
        }

        //Umplu cu 2 o suprafata pentru a o marca si a numara cati pixeli are
        void Umple2(int i, int j)
        {

            matr[i, j] = 2;
            nrElementeSuprafata++;
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

        //Umplu cu 0 o suprafata
        void Umple0(int i, int j)
        {

            matr[i, j] = 0;
            
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

        //Elimin suprafetele care au un numar mic de pixeli
        void EliminaRamasite(int marime = 16)
        {
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 150; j++)
                {
                    if (matr[i, j] == 1)
                    {
                        nrElementeSuprafata = 0;
                        Umple2(i, j);
                        if (nrElementeSuprafata <= marime)
                            Umple0(i, j);
                    }
                }
            //exista posibilitatea sa ramana suprafete umplute cu 2
            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 150; j++)
                    if (matr[i, j] != 0)
                        matr[i, j] = 1;

        }

        //Calculez unghiul la care este scris text-ul
        double Unghi()
        {

            bool exit = false;
            bool poz = false;//unghi pozitiv
            int count = 6;//intru cu cautarea 6 pixeli in interiorul textului
            //caut punctul din stanga sus al textului
            for (int i = 0; i < 150 && !exit; i++)
                for (int j = 0; j < 50; j++)
                    if (matr[j, i] == 1)
                    {
                        count--;
                        if (j < a.Y)//daca punctui i,j se afla mai sus decat maximul actual
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
            count = 6;//intru cu cautarea 6 pixeli in interiorul textului
            exit = false;
            for (int i = 149; i > 0 && !exit; i--)
                for (int j = 0; j < 50; j++)
                    if (matr[j, i] == 1)
                    {
                        count--;
                        if (j < b.Y)//daca punctul i,j se afla mai sus decat maximul actual
                            b = new Point(i, j);
                        if (count == 0)
                        {
                            exit = true;
                            break;
                        }
                        i--;
                        j = 0;
                    }
            //Compar inaltimea punctelor pentru a determina punctul C al triunghiului
            //dreptunghic cu ajutorul caruia o sa determin unghiul
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
            //calculez unghiul cunoscand catetele triunghiului
            double unghi;
            if (poz)
                unghi = Math.Atan((a.Y - c.Y) / (double)(b.X - c.X));
            else
                unghi = -Math.Atan((b.Y - c.Y) / (double)(b.X - a.X));

            //implicit unghiul este in radiani; aici il transform in grade
            unghi = unghi * 180 / Math.PI;

            return unghi;
        }

    }
}
