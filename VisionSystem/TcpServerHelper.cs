using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VisionSystem
{
    public class TcpServerHelper
    {
        //字段
        private TcpListener tcpListener;
        private Dictionary<IPEndPoint, TcpClient> dicClients = new Dictionary<IPEndPoint, TcpClient>();
        private string ip;
        private int port;
        private Thread thread_accept;
        private bool isrun;

        //属性
        public string ID { get; set; }
        public string Message { get; set; }
        public Queue<string> queue { get; set; }

        //事件
        public event Action<string, IPEndPoint, string> eventReceiveData;
        public event Action<IPEndPoint> eventConnect;
        public event Action<IPEndPoint> eventLostConnect;

        //构造
        public TcpServerHelper(string ip, int port)
        {
            this.ip = ip;
            this.port = port;

            this.queue = new Queue<string>();
        }

        //方法
        public void Start()
        {
            tcpListener = new TcpListener(IPAddress.Parse(this.ip), this.port);
            tcpListener.Start();

            if (thread_accept == null || thread_accept.ThreadState == ThreadState.Aborted || thread_accept.ThreadState == ThreadState.Stopped)
            {
                thread_accept = new Thread(Accept);
                thread_accept.IsBackground = true;
                thread_accept.Start();
            }
        }

        public void Stop()
        {
            if (thread_accept != null)
            {
                thread_accept.Abort();
            }

            isrun = false;

            if (tcpListener != null)
            {
                tcpListener.Stop();
            }
        }

        public void Send(string ip, string s)
        {
            foreach (IPEndPoint item in dicClients.Keys)
            {
                if (item.Address.ToString() == ip)
                {
                    TcpClient client = dicClients[item];
                    NetworkStream ns = client.GetStream();

                    byte[] buffer = Encoding.Default.GetBytes(s);
                    ns.Write(buffer, 0, buffer.Length);
                }
            }
        }

        public void Send(string ip, byte[] buffer)
        {
            foreach (IPEndPoint item in dicClients.Keys)
            {
                if (item.Address.ToString() == ip)
                {
                    TcpClient client = dicClients[item];
                    NetworkStream ns = client.GetStream();

                    ns.Write(buffer, 0, buffer.Length);
                }
            }
        }

        public void Send(IPEndPoint point, string s)
        {
            lock (this)
            {
                foreach (IPEndPoint item in dicClients.Keys)
                {
                    if (item == point)
                    {
                        TcpClient client = dicClients[item];
                        NetworkStream ns = client.GetStream();

                        byte[] buffer = Encoding.Default.GetBytes(s);
                        ns.Write(buffer, 0, buffer.Length);
                    }
                } 
            }
        }

        public void Send(IPEndPoint point, byte[] buffer)
        {
            foreach (IPEndPoint item in dicClients.Keys)
            {
                if (item == point)
                {
                    TcpClient client = dicClients[item];
                    NetworkStream ns = client.GetStream();

                    ns.Write(buffer, 0, buffer.Length);
                }
            }
        }

        public void BroadCast(string s)
        {
            foreach (IPEndPoint item in dicClients.Keys)
            {
                TcpClient client = dicClients[item];
                NetworkStream ns = client.GetStream();

                byte[] buffer = Encoding.Default.GetBytes(s);
                ns.Write(buffer, 0, buffer.Length);
            }
        }

        private void Accept()
        {
            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                IPEndPoint p = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
                dicClients.Add(p, tcpClient);
                OnConnect(p);

                isrun = true;
                Thread t = new Thread(Receive);
                t.IsBackground = true;
                t.Start(tcpClient);
            }
        }

        private void Receive(object obj)
        {
            TcpClient client = (TcpClient)obj;
            IPEndPoint p = (IPEndPoint)client.Client.RemoteEndPoint;
            NetworkStream ns = client.GetStream();
            byte[] buffer = new byte[1024 * 1024];

            while (isrun)
            {
                try
                {
                    if (client.Connected)
                    {
                        int r = ns.Read(buffer, 0, buffer.Length);
                        if (r == 0)
                        {
                            dicClients.Remove(p);
                            OnLostConnect(p);
                            break;
                        }
                        string s = Encoding.Default.GetString(buffer, 0, r);
                        OnReceive(this.ID, (IPEndPoint)client.Client.RemoteEndPoint, s);
                        ns.Flush();
                        this.Message = s;
                    }
                    else
                    {
                        dicClients.Remove(p);
                        OnLostConnect(p);
                        break;
                    }
                }
                catch (System.IO.IOException)
                {
                    dicClients.Remove(p);
                    OnLostConnect(p);
                    break;
                }
            }
        }

        private void OnConnect(IPEndPoint p)
        {
            if (eventConnect != null)
            {
                eventConnect(p);
            }
        }

        private void OnLostConnect(IPEndPoint p)
        {
            if (eventLostConnect != null)
            {
                eventLostConnect(p);
            }
        }

        private void OnReceive(string id, IPEndPoint p, string s)
        {
            if (eventReceiveData != null)
            {
                eventReceiveData(id, p, s);
            }
        }
    }
}
