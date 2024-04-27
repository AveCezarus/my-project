using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class StreamWindow : Form
    {
        private string server;
        private TcpClient client;
        private NetworkStream stream;
        private readonly int updateInterval = 500;
        public StreamWindow()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            UpdateImagePeriodically();
        }
        public StreamWindow(string server)
        {
            InitializeComponent();
            this.server = server;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            UpdateImagePeriodically();
        }

        private void StreamWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 form1 = new Form1(server);
            form1.Show();
        }
        private async void UpdateImagePeriodically()
        {
            while (true)
            {
                await UpdateImage();
                Thread.Sleep(updateInterval);
            }
        }
        private async Task UpdateImage()
        {
            try
            {

                stream?.Close();
                client?.Close();

                int port = 5557;
                client = new TcpClient(server, port);
                stream = client.GetStream();

                byte[] requestBytes = Encoding.UTF8.GetBytes("GetStream");
                await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

                byte[] screenshotBytes = new byte[1024 * 1024];
                int bytesRead = await stream.ReadAsync(screenshotBytes, 0, screenshotBytes.Length);

                using (MemoryStream ms = new MemoryStream(screenshotBytes, 0, bytesRead))
                {
                    Image originalImage = Image.FromStream(ms);
                    int newWidth = this.ClientSize.Width;
                    int newHeight = this.ClientSize.Height;
                    Bitmap resizedImage = new Bitmap(newWidth, newHeight);

                    using (Graphics g = Graphics.FromImage(resizedImage))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }
                    pictureBox1.Image = resizedImage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        private void StreamWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            stream?.Close();
            client?.Close();
        }
    }
}
