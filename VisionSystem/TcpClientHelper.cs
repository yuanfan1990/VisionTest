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
    public class TcpClientHelper
    {
        //字段
        private TcpClient tcpClient;
        private NetworkStream ns;
        private Thread thread_receive;
        private string ip;
        private int port;

        public int Count { get; set; }

        //事件
        public event Action<string> evtReceiveData;
        public event Action evtConnect;
        public event Action evtLostConnect;

        //构造
        public TcpClientHelper(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        //方法
        public void Start()
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(new IPEndPoint(IPAddress.Parse(this.ip), this.port));
                ns = tcpClient.GetStream();
                OnConnect();

                //if (thread_receive == null || thread_receive.ThreadState == ThreadState.Aborted || thread_receive.ThreadState == ThreadState.Stopped)
                //{
                //    thread_receive = new Thread(Receive);
                //    thread_receive.IsBackground = true;
                //    thread_receive.Start();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Close()
        {
            if (thread_receive != null)
            {
                thread_receive.Abort();
            }

            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }

        public void Send(string s)
        {
            byte[] buffer = Encoding.Default.GetBytes(s);
            ns.Write(buffer, 0, buffer.Length);
        }

        public void Send(byte[] buffer)
        {
            ns.Write(buffer, 0, buffer.Length);
        }

        public byte[] SendReceive(byte[] buffer)
        {
            byte[] buffer2 = new byte[1024 * 1024];

            ns.Write(buffer, 0, buffer.Length);

            Thread.Sleep(1);

            this.Count = ns.Read(buffer2, 0, buffer2.Length);

            List<byte> list = new List<byte>();
            for (int i = 0; i < this.Count; i++)
            {
                list.Add(buffer2[i]);
            }

            return list.ToArray();
        }

        private void Receive()
        {
            byte[] buffer = new byte[1024 * 1024];

            while (true)
            {
                try
                {
                    int r = ns.Read(buffer, 0, buffer.Length);
                    string s = Encoding.Default.GetString(buffer, 0, r);
                    OnReceive(s);
                }
                catch (System.IO.IOException)
                {
                    OnLostConnect();
                    break;
                }
            }
        }

        private void OnConnect()
        {
            if (evtConnect != null)
            {
                evtConnect();
            }
        }

        private void OnLostConnect()
        {
            if (evtLostConnect != null)
            {
                evtLostConnect();
            }
        }

        private void OnReceive(string s)
        {
            if (evtReceiveData != null)
            {
                evtReceiveData(s);
            }
        }
    }
}
