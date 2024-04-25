using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private StreamWindow streamWindow;
        public Form1()
        {
            
            InitializeComponent();
            textBox1.ReadOnly = true;
            InitializeTimer();
        }
        private void InitializeTimer()
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 2000;
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }
        private void GetPrograms()
        {
            try
            {
                int port = 5555;
                TcpClient client = new TcpClient("127.0.0.1", port);
                NetworkStream stream = client.GetStream();
                string message = "GetRunningPrograms";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                data = new byte[1024];
                StringBuilder response = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);
                listBox1.Items.Clear();
                string[] programs = response.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string program in programs)
                {
                    listBox1.Items.Add(program);
                }
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void GetProgram()
        {
            try
            {
                int port = 5556;
                TcpClient client = new TcpClient("127.0.0.1", port);
                NetworkStream stream = client.GetStream();
                string message = "GetActiveWindow";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                data = new byte[1024];
                StringBuilder response = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);
                textBox1.Clear();
                textBox1.Text = response.ToString();
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            streamWindow = new StreamWindow();
            streamWindow.Show();
            this.Hide();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetPrograms();
            GetProgram();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                timer1.Stop();
                if (streamWindow != null && !streamWindow.IsDisposed)
                {
                    streamWindow.Close();
                    streamWindow.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during application shutdown: " + ex.Message);
            }
        }
    }
}


//Програма для стеження за активністю користувача
// -сервер встановлюється на компьютер користувача
// - клиєнт - можна підключитися та подивитися які програми запущені, яке активне вікно, отримати зображення з екрану
 