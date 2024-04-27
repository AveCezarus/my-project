using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace Server
{
   
    public partial class Form1 : Form
    {
        TcpListener server;
        Thread listenThread;
        List<TcpListener> listeners;

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public Form1()
        {
            InitializeComponent();
            this.Shown += Form1_Load;
        }
        private void StartServerInBackground()
        {
            ThreadStart threadStart = new ThreadStart(StartServer);
            Thread serverThread = new Thread(threadStart);
            serverThread.IsBackground = true;
            serverThread.Start();
        }
        private void StartServer()
        {
            try
            {
                int[] ports = { 5555, 5556, 5557 };
                listeners = new List<TcpListener>();

                foreach (int port in ports)
                {
                    IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                    TcpListener listener = new TcpListener(localAddr, port);
                    listener.Start();
                    listeners.Add(listener);
                    Thread listenThread = new Thread(() => ListenForClients(listener));
                    listenThread.Start();
                    Console.WriteLine($"Server started on port {port}");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ListenForClients(TcpListener listener)
        {
            try
            {
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();
            try
            {
                byte[] data = new byte[256];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);
                string request = builder.ToString();
                if (request == "GetRunningPrograms")
                {
                    var runningProcesses = System.Diagnostics.Process.GetProcesses();
                    StringBuilder responseData = new StringBuilder();
                    foreach (var process in runningProcesses)
                    {
                        if (!string.IsNullOrEmpty(process.MainWindowTitle))
                        {
                            responseData.AppendLine(process.MainWindowTitle);
                        }
                    }
                    byte[] msg = Encoding.UTF8.GetBytes(responseData.ToString());
                    stream.Write(msg, 0, msg.Length);
                }
                if (request == "GetActiveWindow")
                {
                    IntPtr foregroundWindow = GetForegroundWindow();
                    StringBuilder windowTitle = new StringBuilder(256);
                    GetWindowText(foregroundWindow, windowTitle, windowTitle.Capacity);
                    byte[] msg = Encoding.UTF8.GetBytes(windowTitle.ToString());
                    stream.Write(msg, 0, msg.Length);
                }
                if (request == "GetStream")
                {
                    try
                    {
                        Bitmap screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                        Graphics gfxScreenshot = Graphics.FromImage(screenshot);
                        gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                        MemoryStream ms = new MemoryStream();
                        screenshot.Save(ms, ImageFormat.Png);
                        byte[] screenshotBytes = ms.ToArray();
                        stream.Write(screenshotBytes, 0, screenshotBytes.Length);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                foreach (TcpListener listener in listeners)
                {
                    listener.Stop();
                }

                if (listenThread != null)
                {
                    listenThread.Join();
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartServerInBackground();
            this.Hide();
        }
    }
}
