using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        bot_snake bot_Snake = new bot_snake();
        PictureBox pixel;
        PictureBox[] dizi = new PictureBox[99999];
        Random rastgele = new Random();
        int rast_x, rast_y; //elmanın rastgele y konumu
        byte mod_no = 0; //rgb modu için (örn: 1 ise farklı 2 ise farklı)
        int puan = 0;
        float coin = 50f;
        int i = 0; //yılanın kafasının dizideki yeri
        int x = 100, y = 100; //yılanın x, y konumu
        public int uzunluk = 10; //yılanın uzunluğu
        bool sol, sag, ust, alt, kilit; //hangi yöne gicedeğini belirtmek için

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (kilit) //yılanın her bir birimi oluşmadan zıt yönlere gitmesini engellemek için
            {
                //burdaki ifler yılanın herhangibir yönde giderken zıt yöne dönmemesi için
                if (e.KeyCode == Keys.Right && !sol)//sağ
                {
                    sag = true;
                    sol = false;
                    ust = false;
                    alt = false;
                }
                else if (e.KeyCode == Keys.Left && !sag) //sol
                {
                    sag = false;
                    sol = true;
                    ust = false;
                    alt = false;
                }
                else if (e.KeyCode == Keys.Up && !alt) //yukarı
                {
                    sag = false;
                    sol = false;
                    ust = true;
                    alt = false;
                }
                else if (e.KeyCode == Keys.Down && !ust) //aşağı
                {
                    sag = false;
                    sol = false;
                    ust = false;
                    alt = true;
                }
                if (e.KeyCode == Keys.Space) //oyunu durdurmak
                {
                    oyun_dur();
                }
                kilit = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pixel_olustur(); //Yılanın her bir biriminin oluşmasını sağlar
            duvardan_ve_elmadan_gecme(); // bazı şeyleri kontrol eder (181. satır'a git)
            bot_Snake.veri_gonder(i, uzunluk, x, y);
            //bot_Snake.duvardan_ve_elmadan_gecme();

            bot_yilandan_gelenler_kontrol();

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            rgb();
        }

        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            bot_Snake.panel1 = panel1;
            bot_Snake.main_dizi = dizi;
            bot_Snake.radioButton1= radioButton1;
            bot_Snake.radioButton2 = radioButton2;
            bot_Snake.label3 = label3;
            rast_y = 0;
            rast_x = 0;
            sag = true; //Oyun başladığında sağa doğtu giderek başlasın diye
            for (int w = 0; w < uzunluk; w++) //Oyun başladığında yılanın 5 birimlik kısmının oluşması
            {
                pixel_olustur();
            }
            elma_olustur();
            timer1.Start(); //oyun başlar
            label2.Text = "Coin : "+coin.ToString(); //parayı ekrana yazdırır
        }

        private void button1_Click(object sender, EventArgs e)
        {
            para_ode(3, false, false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mod_no = 1;
            para_ode(10, true, false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            para_ode(0.25f, false, true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mod_no = 2;
            para_ode(10, true, false);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mod_no = 3;
            para_ode(10, true, false);
        }
        void bot_yilandan_gelenler_kontrol()
        {
            if (bot_Snake.elma_olustur == true) { elma_olustur(); }
            if (bot_Snake.temas == true)
            {
                bot_Snake.temas = false;
                oyun_dur();
                MessageBox.Show("Yılana çarptın!", "GAME OVER", MessageBoxButtons.OK);
                restart();
            }
            //if (bot_Snake.elma_olustur == true) { bot_Snake.elma_olustur = false; }

        }
        void pixel_olustur() //Yılanın her bir biriminin oluşmasını sağlar
        {
            pixel = new PictureBox();
            pixel.Size = new Size(10, 10);
            pixel.BackColor = colorDialog1.Color;
            if (sag) //sağa doğru oluşturur
            {
                pixel.Location = new Point(x += 10, y);
            }
            else if (sol) //sola doğru oluşturur
            {
                pixel.Location = new Point(x -= 10, y);
            }
            else if (ust) //yukarıya doğru oluşturur
            {
                pixel.Location = new Point(x, y -= 10);
            }
            else if (alt) //aşağıya doğru oluşturur
            {
                pixel.Location = new Point(x, y += 10);
            }
            i++;
            panel1.Controls.Add(pixel);
            //panel1.Controls.Add(dizi2[i] = pixel_olustur2(pixel));
            kilit = true;
            dizi[i] = pixel;
            dizi[i].BringToFront();
            bot_Snake.pixel_olustur();

        }
        void elma_olustur() //rastgele elma oluşturur
        {
            panel1.Controls.Remove(dizi[dizi.Length - 1]);
            rast_x = rastgele.Next(-10, 500);
            rast_x = rast_x - (rast_x % 10);
            rast_y = rastgele.Next(-10, 500);
            rast_y = rast_y - (rast_y % 10);
            pixel = new PictureBox();
            pixel.Size = new Size(10, 10);
            pixel.BackColor = Color.Red;
            pixel.Location = new Point(rast_x, rast_y);
            panel1.Controls.Add(pixel);
            dizi[dizi.Length - 1] = pixel;
            if (bot_Snake.elma_olustur){ bot_Snake.elma_olustur = false; }
            else { label1.Text = "Score : " + puan++.ToString(); }
            bot_Snake.elma_kordinatları(rast_x, rast_y);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                MessageBox.Show("KOLAY");

            }
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            oyun_devam();
        }

        void duvardan_ve_elmadan_gecme() // bazı şeyleri kontrol eder
        {
            if (i >= uzunluk) //yılanın uzunluğunu ayarlar (uzunluğa göre diziden eleman siler)
            {
                //panel1.Controls.Remove(dizi[i - uzunluk]);
                dizi[i - uzunluk].Dispose();
            }
            if (dizi[i].Location.X >= 500) //Sağa geçtiğinde soldan çıkması
            {
                dizi[i].Location = new Point(x = 0, y);
            }
            else if (dizi[i].Location.X <= -10) //Sola geçtiğinde sağdan çıkması
            {
                dizi[i].Location = new Point(x = 490, y);
            }
            else if (dizi[i].Location.Y >= 500) //Aşağıya geçtiğinde yukardan çıkması
            {
                dizi[i].Location = new Point(x, y = 0);
            }
            else if (dizi[i].Location.Y <= -10) //Yukardan geçtiğinde aşağıdan çıkması
            {
                dizi[i].Location = new Point(x, y = 490);
            }
            if (x == rast_x && y == rast_y) //yılanın elmaya olan temasını kontrol eder
            {
                uzunluk += 2;
                label2.Text = "Coin : " + (coin += 0.25f).ToString();
                elma_olustur();
            }
            for (int g = i - uzunluk; g < i; g++) //yılanın kendine olan temasını kontrol eder
            {
                if (dizi[g].Location.X == x && dizi[g].Location.Y == y)
                {
                    oyun_dur();
                    MessageBox.Show("Kendide çarptın!", "GAME OVER", MessageBoxButtons.OK);
                    restart();
                }
            }
        }
        void restart()//eğer yılan kendine çarparsa oyunudaki değişkenleri sıfırlar ve yeniden başlatır
        {
            i = 1; x = 100; y = 100; uzunluk = 10; mod_no = 0; sag = true; sol = false; alt = false; ust = false; colorDialog1.Color = Color.Black; coin = 0;puan = -1;
            panel1.Controls.Clear();
            for (int w = 0; w < uzunluk; w++)
            {
                pixel_olustur();
            }
            elma_olustur();
            oyun_devam();
            label1.Text = "Score : " + puan.ToString();
            label2.Text = "Coin : " + coin.ToString();
        }
        void oyun_devam() //oyunun devam etmesi
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            timer1.Start();
            if (mod_no != 0) { timer2.Enabled = false; timer2.Start(); }//eğer rgb satın alınmış ise başlatır (aksi takdirde oyun durup devam
                                                                        //ettiğinde rgb renk özelliğini satın almamış olsa bile başlatır)
        }

        void oyun_dur() //oyunun durması
        {
            timer1.Stop();
            timer2.Stop();
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
        }
        void rgb()//3 modlu rgb renk 
        {
            if (mod_no == 1)
            {
                for (int q = i - uzunluk; q <= i; q++)
                {
                    dizi[q].BackColor = Color.FromArgb(q % 255, q % 255, uzunluk % 255);
                }
            }
            else if (mod_no == 2)
            {
                for (int q = i - uzunluk; q <= i; q++)
                {
                    dizi[q].BackColor = Color.FromArgb(q % 255, uzunluk % 255, q % 255);
                }
            }
            else
            {
                for (int q = i - uzunluk; q <= i; q++)
                {
                    dizi[q].BackColor = Color.FromArgb(uzunluk % 255, q % 255, q % 255);
                }
            }
        }
        void para_ode(float sent_coin, bool rgb_enabled, bool uzunluk_enabled)//rgb renk aktifleştirmek için para kontrolü
        {
            if (coin >= sent_coin)
            {
                label2.Text = "Coin : " + (coin -= sent_coin).ToString();
                if (rgb_enabled) { timer2.Enabled = true; timer2.Start(); }
                else if (uzunluk_enabled) { uzunluk++; }
                else { timer2.Stop(); timer2.Enabled = false; mod_no = 0; colorDialog1.ShowDialog(); }
                oyun_devam();
            }
            else
            {
                if (!timer2.Enabled) { mod_no = 0; }
                MessageBox.Show("Yetersiz Coin", "COİN!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                oyun_devam();
            }
        }
    }
    public class bot_snake
    {
        PictureBox pixel;
        PictureBox[] dizi = new PictureBox[99999];
        public PictureBox[] main_dizi = new PictureBox[99999];
        Random rastgele = new Random();
        int rast_adim;
        int rast_x, rast_y;
        int rast_yon = 1;
        int i = 0, main_i;
        int x = 400, y = 400;
        int main_x = 100, main_y = 100;
        int uzunluk = 10, main_uzunluk;
        int yon_kontrol, puan = 0;
        public bool elma_olustur;
        public bool temas;
        public Panel panel1;
        public RadioButton radioButton1;
        public RadioButton radioButton2;
        public Label label3;

        public void pixel_olustur()
        {
            pixel = new PictureBox();
            pixel.Size = new Size(10, 10);
            pixel.BackColor = Color.Blue;
            //if (rast_adim == 0)
            //{
            //    yon_kontrol = rastgele.Next(1, 5);
            //    while (yon_kontrol % 2 == 0 && rast_yon % 2 == 0 || yon_kontrol % 2 != 0 && rast_yon % 2 != 0)
            //    {
            //        yon_kontrol = rastgele.Next(1, 5);
            //    }
            //    rast_yon = yon_kontrol;
            //    rast_adim = rastgele.Next(1, 10);
            //}
            //else { rast_adim--; }
            if (radioButton1.Checked){ kolay(); }
            else { zor(); }
            panel1.Controls.Add(pixel);
            i++;
            dizi[i] = pixel;
            dizi[i].BringToFront();
            dizi[i].BringToFront();
        }

        void restart()
        {
            for (int q = i - uzunluk; q <= i; q++)
            {
                panel1.Controls.Remove(dizi[q]);
            }
            i = 2; x = 400; y = 400; uzunluk = 10;
            for (int w = 0; w < uzunluk; w++)
            {
                pixel_olustur();
            }
            //Array.Clear(dizi);
        }
        public void elma_kordinatları(int rast_x, int rast_y)
        {
            this.rast_x = rast_x;
            this.rast_y = rast_y;

        }
        public void veri_gonder(int main_i, int main_uzunluk, int main_x, int main_y)
        {
            this.main_i = main_i;
            this.main_uzunluk = main_uzunluk;
            this.main_x = main_x;
            this.main_y = main_y;
            duvardan_ve_elmadan_gecme();
        }
        void zor()
        {
            if (x > rast_x) { pixel.Location = new Point(x -= 10, y); }//sag
            else if (x < rast_x) { pixel.Location = new Point(x += 10, y); }//sol
            else if (y < rast_y) { pixel.Location = new Point(x, y += 10); }//ust
            else if (y > rast_y) { pixel.Location = new Point(x, y -= 10); }//alt
        }
        void kolay()
        {
            if (rast_adim == 0)
            {
                yon_kontrol = rastgele.Next(1, 5);
                while (yon_kontrol % 2 == 0 && rast_yon % 2 == 0 || yon_kontrol % 2 != 0 && rast_yon % 2 != 0)
                {
                    yon_kontrol = rastgele.Next(1, 5);
                }
                rast_yon = yon_kontrol;
                rast_adim = rastgele.Next(1, 10);
            }
            else { rast_adim--; }
            if (rast_yon == 1) { pixel.Location = new Point(x += 10, y); }//sag
            else if (rast_yon == 3) { pixel.Location = new Point(x -= 10, y); }//sol
            else if (rast_yon == 2) { pixel.Location = new Point(x, y -= 10); }//ust
            else if (rast_yon == 4) { pixel.Location = new Point(x, y += 10); }//alt
        }

        public void duvardan_ve_elmadan_gecme()
        {
            
            if (i >= uzunluk) //yılanın uzunluğunu ayarlar (uzunluğa göre diziden eleman siler)
            {
                //panel1.Controls.Remove(dizi[i - uzunluk]);
                dizi[i-uzunluk].Dispose();
            }
            if (dizi[i].Location.X >= 500) //Sağa geçtiğinde soldan çıkması
            {
                dizi[i].Location = new Point(x = 0, y);
            }
            else if (dizi[i].Location.X <= -10) //Sola geçtiğinde sağdan çıkması
            {
                dizi[i].Location = new Point(x = 490, y);
            }
            else if (dizi[i].Location.Y >= 500) //Aşağıya geçtiğinde yukardan çıkması
            {
                dizi[i].Location = new Point(x, y = 0);
            }
            else if (dizi[i].Location.Y <= -10) //Yukardan geçtiğinde aşağıdan çıkması
            {
                dizi[i].Location = new Point(x, y = 490);
            }
            if (x == rast_x && y == rast_y) //yılanın elmaya olan temasını kontrol eder
            {
                uzunluk += 2;
                elma_olustur = true;
                puan++;
                label3.Text = "Rakip Score : " + puan.ToString();

            }
            for (int g = i - uzunluk; g < i; g++) //yılanın diğer yılana olan temasını kontrol eder
            {
                if (dizi[g].Location.X == main_x && dizi[g].Location.Y == main_y)
                {
                    i = 2; x = 400; y = 400; uzunluk = 10;
                    temas = true;
                    break;
                }
                if (dizi[g].Location.X == x && dizi[g].Location.Y == y)
                {
                    restart();
                    break;
                }
            }
            for (int q = main_i - main_uzunluk + 1; q < main_i; q++)
            {
                if (main_dizi[q].Location.X == x && main_dizi[q].Location.Y == y)
                {
                    restart();
                    break;
                }
            }
        }
    }
}
