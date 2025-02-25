using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snipping_Tool
{
    public partial class Form1 : Form
    {
        Image img;
        string imgPath;
        string format="jpg";
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        
        bool Delete()
        {
            if (File.Exists(imgPath))
            {
                File.Delete(imgPath);
                return true;
            }
            else
            {
                MessageBox.Show("File Not Found","Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        bool Save()
        {
            if (img != null)
            {
                string fileName = $"{textBox1.Text}_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + $".{format}";
                imgPath = desktopPath + $@"\{fileName}";
                img.Save(imgPath); // or img.Save(desktopPath + "\\erdem.jpg");
                return true;
            }
            else MessageBox.Show("Take a Screenshot First", "Warning"); return false;
        }
        void Screenshot()
        {
            try
            {
                Hide();
                SendKeys.Send("{PRTSC}");
                Thread.Sleep(1000);
                img = Clipboard.GetImage();
                pictureBox1.Image = img;
                Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
        }
        public Form1()
        {
            InitializeComponent();
            radioButton1.Select();
        }

        private void screenshot_Click(object sender, EventArgs e)
        {
            Screenshot();
            if (checkBox1.Checked) Save();
        }

        private async void save_Click(object sender, EventArgs e)
        {
            if (Save()) 
            {
                save.BackColor = Color.Green;
                await Task.Delay(1500);
                save.BackColor = SystemColors.Control;
            }
        }

        private async void delete_Click(object sender, EventArgs e)
        {
            if (Delete()) 
            {
                delete.BackColor = Color.Red;
                await Task.Delay(1500);
                delete.BackColor = SystemColors.Control;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            save.Enabled= checkBox1.Checked==true?false:true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            format = "jpg";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            format = "png";
        }
    }
}
