using HidLibrary;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace USB_IO_9   // for Km2net USB-IO FS-30  A/D Converter
{
    public partial class Form1 : Form
    {
        const int duration_1 = 10;   //10

        int[] ch = new int[24];
        public double[] Adata = new double[9];

        USBIO_Device device = USBIO_Device.Connect();

        private int data;          // 波形データの保存用
        int odata1, odata2, odata3, odata4, odata5, odata6, odata7, odata8;

        //ImageオブジェクトのGraphicsオブジェクトを作成する

        static Bitmap canvas = new Bitmap(2560, 1920);
        static Graphics g = Graphics.FromImage(canvas);

        public Form1()
        {
            InitializeComponent();
        }

        public void WriteAD()
        {
            textBox1.Clear();
            int j = 2;
            double n = 500.0 / 1023;

            ch[j] = USBIO_Device.receiveData[j + 1] + USBIO_Device.receiveData[j + 2] << 2;
            Adata[0] = ch[j] * n;

            j = j + 3;
            ch[j] = USBIO_Device.receiveData[j + 1] + USBIO_Device.receiveData[j + 2] << 2;
            Adata[1] = ch[j] * n;

            j = j + 3;
            ch[j] = USBIO_Device.receiveData[j + 1] + USBIO_Device.receiveData[j + 2] << 2;
            Adata[2] = ch[j] * n;

            j = j + 3;
            ch[j] = USBIO_Device.receiveData[j + 1] + USBIO_Device.receiveData[j + 2] << 2;
            Adata[3] = ch[j] * n;
            //
            j = j + 3;
            ch[j] = USBIO_Device.receiveData[j + 1] + USBIO_Device.receiveData[j + 2] << 2;
            Adata[4] = ch[j] * n;

            j = j + 3;
            ch[j] = USBIO_Device.receiveData[j + 1] + USBIO_Device.receiveData[j + 2] << 2;
            Adata[5] = ch[j] * n;

            j = j + 3;
            ch[j] = USBIO_Device.receiveData[j + 1] + USBIO_Device.receiveData[j + 2] << 2;
            Adata[6] = ch[j] * n;

            j = j + 3;
            ch[j] = USBIO_Device.receiveData[j + 1] + USBIO_Device.receiveData[j + 2] << 2;
            Adata[7] = ch[j] * n;

            for (int i = 0; i < 8; i++)
            {
                textBox1.Text += " " + (Adata[i]/100.0).ToString("f3") + " V  ";
            }
        }

        public void WriteData()
        {
            for (int i = 0; i < 26; i++)
                textBox1.Text += USBIO_Device.receiveData[i].ToString() + " ";
            textBox1.Text += "\r\n";
        }

        public void WriteDeviceList()
        {
            var deviceList = HidDevices.Enumerate().ToArray();

            foreach (var data in deviceList) textBox1.Text += data.ToString();
            // for (int i = 0; i < deviceList.Length; i++) textBox1.Text +=  deviceList[i].ToString();

            textBox1.Text += "\r\n";

            textBox1.HideSelection = false;
            //カレット位置を末尾に移動
            textBox1.SelectionStart = textBox1.Text.Length;
            //テキストボックスにフォーカスを移動
            textBox1.Focus();
            //カレット位置までスクロール
            textBox1.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            g.Clear(Color.Black);
            textBox1.Clear();
            pictureBox1.Image = canvas;
        }

        int j = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            device.SendReceive();

            if (this.InvokeRequired) Invoke((MethodInvoker)delegate () { WriteAD(); });
            else WriteAD();

                const int n = 2;  //2
                const int k = 15; //15
              
                g.DrawLine(Pens.Red, j, 600 - 0, j + k, 600 - 0);
                g.DrawLine(Pens.Red, j, 600 - 1024 / n, j + k, 600 - 1024 / n);
 
                data = (int)Adata[0];
                g.DrawLine(Pens.White, j, 600 - odata1, j + k, 600 - data);
                odata1 = data;

                data = (int)Adata[1];
                g.DrawLine(Pens.Cyan, j, 600 - odata2, j + k, 600 - data);
                odata2 = data;
  
                data = (int)Adata[2];
                g.DrawLine(Pens.Lime, j, 600 - odata3, j + k, 600 - data);
                odata3 = data;
 
                data = (int)Adata[3];
                g.DrawLine(Pens.Olive, j, 600 - odata4, j + k, 600 - data);
                odata4 = data;

                data = (int)Adata[4];
                g.DrawLine(Pens.Yellow, j, 600 - odata5, j + k, 600 - data);
                odata5 = data;

                data = (int)Adata[5];
                g.DrawLine(Pens.Green, j, 600 - odata6, j + k, 600 - data);
                odata6 = data;

                data = (int)Adata[6]; 
                g.DrawLine(Pens.Magenta, j, 600 - odata7, j + k, 600 - data);
                odata7 = data;

                data = (int)Adata[7];
                g.DrawLine(Pens.SkyBlue, j, 600 - odata8, j + k, 600 - data);
                odata8 = data;

                j += k;

                //if (j > pictureBox1.Width)
                if (j > this.Width)
                {
                    j = 0;
                    g.Clear(Color.Black);
                }
         pictureBox1.Image = canvas;
        }
    }
}

public class USBIO_Device : IDisposable
{
    private const int VendorID = 0x1352;
    private const int ProductID = 0x0111; // FS-30

    public static byte[] receiveData = new byte[64];   // byte 配列の定義 
    public static byte[] sendData = new byte[64];

    bool disposed = false;
    public readonly HidDevice device;

    //   public static HidDevice[] ListDevices() => HidDevices.Enumerate(VendorID, ProductID).ToArray();
    //   public static USBIO_Device Connect(HidDevice device) => new USBIO_Device(device);

    public static USBIO_Device Connect()
    {
        var device = HidDevices.Enumerate(VendorID, ProductID).FirstOrDefault();
        if (device == null)
        {
            throw new InvalidOperationException("Deviceが見つかりません");
        }
        return new USBIO_Device(device);
    }

    public USBIO_Device(HidDevice device)
    {
        this.device = device;
    }

    public void Dispose()
    {
        Dispose(true);    // Dispose of unmanaged resources.
        GC.SuppressFinalize(this);    // Suppress finalization.
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed) return;
        if (disposing) device.Dispose();
        disposed = true;
    }

    public void SendReceive()
    {
        // var sendData = new byte[64];  // A/D コマンド 
        sendData[1] = 0x2A;  // A/D コマンド 
        sendData[2] = 0x01;  // 1ch
        sendData[5] = 0x02;
        sendData[8] = 0x03;
        sendData[11] = 0x04;
        sendData[14] = 0x05;
        sendData[17] = 0x06;
        sendData[20] = 0x07;
        sendData[23] = 0x08;

        var res = device.Write(sendData);
        if (!res) throw new InvalidOperationException("送信に失敗しました");

        var rec = device.Read();
        if (rec.Status != HidDeviceData.ReadStatus.Success)
            throw new InvalidOperationException($"受信に失敗しました: {rec.Status}");

        for (int i = 0; i < 25; i++) receiveData[i] = rec.Data[i];  // receiveData に格納  64
    }
}



