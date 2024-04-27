using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private StreamWindow streamWindow;
        private string server;
        private bool a = false;
        public Form1(string server)
        {
            InitializeComponent();
            this.server = server;
            textBox1.ReadOnly = true;
        }
        public Form1()
        {
            InitializeComponent();
            textBox1.ReadOnly = true;
        }

        private async void GetPrograms()
        {
            try
            {
                int port = 5555;
                using (TcpClient client = new TcpClient(server, port))
                using (NetworkStream stream = client.GetStream())
                {
                    string message = "GetRunningPrograms";
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(data, 0, data.Length);

                    StringBuilder response = new StringBuilder();
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        response.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                    }

                    listBox1.Items.Clear();
                    string[] programs = response.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string program in programs)
                    {
                        listBox1.Items.Add(program);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                a = false;
            }
        }

        private async void GetProgram()
        {
            try
            {
                int port = 5556;
                using (TcpClient client = new TcpClient(server, port))
                using (NetworkStream stream = client.GetStream())
                {
                    string message = "GetActiveWindow";
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(data, 0, data.Length);

                    StringBuilder response = new StringBuilder();
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        response.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                    }

                    textBox1.Clear();
                    textBox1.Text = response.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                a = false;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            server = textBox2.Text;
            a = true;
            while (a)
            {
                await Task.Delay(2000);
                GetPrograms();
                GetProgram();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            streamWindow = new StreamWindow(server);
            streamWindow.Show();
            this.Hide();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (streamWindow != null && !streamWindow.IsDisposed)
                {
                    streamWindow.Close();
                    streamWindow.Dispose();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}

//127.0.0.1
//Програма для стеження за активністю користувача
// -сервер встановлюється на компьютер користувача
// - клиєнт - можна підключитися та подивитися які програми запущені, яке активне вікно, отримати зображення з екрану
