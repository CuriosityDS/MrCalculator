using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mr.Calc
{
    public partial class Form1 : Form
    {
        int counter = 0;
        string PastText1 = " ";
        string PastText2 = " ";
        // надо создать катологи Logs and Results in Debug 
        FileStream log = new FileStream($@"Logs\log_{DateTime.Now.ToString("dd.MM.yy HH.mm")}.txt", FileMode.CreateNew, FileAccess.Write);
        FileStream res = new FileStream($@"Results\result_{DateTime.Now.ToString("dd.MM.yy HH.mm")}.txt", FileMode.CreateNew, FileAccess.Write);
        string name = System.Environment.MachineName;
        public Form1()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            string time = DateTime.Now.ToString("HH.mm.ss");
            counter++;
            byte[] buffer = Encoding.Default.GetBytes($"[{time}] [{name}] нажал на кнопку {counter} раз\n");
            log.WriteAsync(buffer, 0, buffer.Length);
            try
            {
                if (textBox1.Text == "" && textBox2.Text == "") throw new Exception("Поля пусты");

                if (textBox1.Text != PastText1) {
                    byte[] text1 = Encoding.Default.GetBytes($"[{time}] Изменение содержимого  поля [textBox1]\n");
                    log.WriteAsync(text1, 0, text1.Length);
                }
                if (textBox2.Text != PastText2)
                {
                    byte[] text2 = Encoding.Default.GetBytes($"[{time}] Изменение содержимого  поля [textBox2]\n");
                    log.WriteAsync(text2, 0, text2.Length);
                }
                PastText1 = textBox1.Text;
                PastText2 = textBox2.Text;

                textBox3.Text = textBox1.Text.Trim() + textBox2.Text;
                byte[] wordplus = Encoding.Default.GetBytes($"[{time}] [{name}] получил словосочетание [{textBox3.Text}]\n");
                res.WriteAsync(wordplus, 0, wordplus.Length);
            }
            catch (Exception m)
            {
                textBox3.Text = "";
                MessageBox.Show
                (m.Message,
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                byte[] warn = Encoding.Default.GetBytes($"[{time}] [{name}] вызвал ошибку [{m.Message}]\n");
                log.WriteAsync(warn, 0, warn.Length);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string time = DateTime.Now.ToString("HH.mm.ss");
            byte[] closing = Encoding.Default.GetBytes($"[{time}] [{name}] закрыл программу\n");
            log.WriteAsync(closing, 0, closing.Length);
            res.WriteAsync(closing, 0, closing.Length);
            log.Close();
            res.Close();
        }
    }
}
