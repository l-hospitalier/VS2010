// This program measures 0-5 V analog voltage using FSIO30 usb IO adaptor
// provided by Km2Net Co., in JAPAN

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using USB_IO_Family;

namespace USB_FSIO30_5V
{
    public partial class Form1 : Form
	{
        const int n = 2;
        const int k = 15; //15
        const int A = 800;
            
        int i, j;

        ioCtl io = new ioCtl(); //インスタンス作成
		ioCtl.ST_CTL_AD[] an = new ioCtl.ST_CTL_AD[8];
 
		private int[] data = new int[2560 + 1];        // 波形データの保存用
        int odata1, odata2, odata3, odata4, odata5, odata6, odata7, odata8;

      
        //ImageオブジェクトのGraphicsオブジェクトを作成する

        static Bitmap canvas = new Bitmap(2560, 1920);
        static Graphics g = Graphics.FromImage(canvas);
		static Pen p = new Pen(Color.Black, 1);

        public Form1()
		{
			InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            g.FillRectangle(Brushes.Black, j, 0, 50, 1024);
 		
            g.DrawLine(Pens.White, j, A - 0, j + k, A - 0);
            g.DrawLine(Pens.White, j, A - 1024/n, j + k, A - 1024/n);

            an[0].Chanel = 1;
			i = io.ctlADIn(ref an);
			textBox1.Text = String.Format("{0:0000}", an[0].AD);
			textBox2.Text = String.Format("{0:000} mV", (an[0].AD * 5000) / 1023);
            data[i] = an[0].AD / n;
            
            g.DrawLine(Pens.White, j, A - odata1, j + k, A - data[i]);
            odata1 = data[i];

            an[0].Chanel = 2;
            i = io.ctlADIn(ref an);
            textBox4.Text = String.Format("{0:0000}", an[0].AD);
            textBox3.Text = String.Format("{0:000} mV", (an[0].AD * 5000) / 1023);
            data[i]  = an[0].AD / n;

            g.DrawLine(Pens.Cyan, j, A - odata2, j + k, A - data[i]);
            odata2 = data[i];

            an[0].Chanel = 3;
            i = io.ctlADIn(ref an);
            textBox6.Text = String.Format("{0:0000}", an[0].AD);
            textBox5.Text = String.Format("{0:000} mV", (an[0].AD * 5000) / 1023);
            data[i] = an[0].AD / n;

            g.DrawLine(Pens.Lime, j, A - odata3, j + k, A - data[i]);
            odata3 = data[i];

            an[0].Chanel = 4;
            i = io.ctlADIn(ref an);
            textBox8.Text = String.Format("{0:0000}", an[0].AD);
            textBox7.Text = String. Format("{0:000} mV", (an[0].AD * 5000) / 1023);
            data[i] = an[0].AD / n;

            g.DrawLine(Pens.Olive, j, A - odata4, j + k, A - data[i]);
            odata4 = data[i];

            an[0].Chanel = 5;
            i = io.ctlADIn(ref an);
            textBox16.Text = String.Format("{0:0000}", an[0].AD);
            textBox15.Text = String.Format("{0:000} mV", (an[0].AD * 5000) / 1023);
            data[i] = an[0].AD / n;

            g.DrawLine(Pens.Yellow, j, A - odata5, j + k, A - data[i]);
            odata5 = data[i];

            an[0].Chanel = 6;
            i = io.ctlADIn(ref an);
            textBox14.Text = String.Format("{0:0000}", an[0].AD);
            textBox13.Text = String.Format("{0:000} mV", (an[0].AD * 5000) / 1023);
            data[i] = an[0].AD / n;

            g.DrawLine(Pens.Green, j, A - odata6, j + k, A - data[i]);
            odata6 = data[i];

            an[0].Chanel = 7;
            i = io.ctlADIn(ref an);
            textBox12.Text = String.Format("{0:0000}", an[0].AD);
            textBox11.Text = String.Format("{0:000} mV", (an[0].AD * 5000) / 1023);
            data[i] = an[0].AD / n;

            g.DrawLine(Pens.Magenta, j, A - odata7, j + k, A - data[i]);
            odata7 = data[i];

            an[0].Chanel = 8;
            i = io.ctlADIn(ref an);
            textBox10.Text = String.Format("{0:0000}", an[0].AD);
            textBox9.Text = String.Format("{0:000} mV", (an[0].AD * 5000) / 1023);
            data[i] = an[0].AD / n;

            g.DrawLine(Pens.Red, j, A - odata8, j + k, A - data[i]);
            odata8 = data[i];

            j = j + k;

               //if (j > pictureBox1.Width)
               if (j > this.Width)
               {
                   j = 0;
                   //g.Clear(Color.Black); 
               }
            pictureBox1.Image = canvas;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (io.openDevice() <= 0)
            {
                MessageBox.Show("製品が見つかりません");
                timer1.Enabled = false;
            }
            else timer1.Enabled = true;
        }

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
            //リソースを解放する
            timer1.Enabled = false;
            io.closeDevice();
		}
    }
}
