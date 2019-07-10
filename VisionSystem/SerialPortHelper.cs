using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionSystem
{
    public class SerialPortHelper
    {
        //字段
        private SerialPort serialport;

        //属性
        public string ID { get; set; }
        public string Message { get; set; }
        public Queue<string> queue { get; set; }

        //事件
        public event Action<string, string> eventReceiveData;

        //构造
        public SerialPortHelper(string portname, int baudrate, int databits, double stopbits)
        {
            this.serialport = new System.IO.Ports.SerialPort();

            this.serialport.PortName = portname;
            this.serialport.BaudRate = baudrate;
            this.serialport.DataBits = databits;
            if (stopbits == 0)
            {
                this.serialport.StopBits = StopBits.None;
            }
            else if (stopbits == 1)
            {
                this.serialport.StopBits = StopBits.One;
            }
            else if (stopbits == 2)
            {
                this.serialport.StopBits = StopBits.Two;
            }
            else if (stopbits == 1.5)
            {
                this.serialport.StopBits = StopBits.OnePointFive;
            }
            this.serialport.Parity = Parity.None;
            this.serialport.DataReceived += new SerialDataReceivedEventHandler(DataReceived);

            this.queue = new Queue<string>();
        }

        //方法
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[1024];

            int r = this.serialport.Read(buffer, 0, buffer.Length);

            string receive = System.Text.Encoding.Default.GetString(buffer, 0, r);

            OnReceive(this.ID, receive);

            this.Message = receive;

            this.serialport.DiscardInBuffer();
        }

        public void Start()
        {
            try
            {
                this.serialport.Open();
                this.serialport.DiscardInBuffer();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Stop()
        {
            try
            {
                if (serialport.IsOpen)
                {
                    this.serialport.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Write(string s)
        {
            try
            {
                lock (this)
                {
                    if (serialport.IsOpen)
                    {
                        this.serialport.Write(s);
                    } 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Write(byte[] buffer)
        {
            try
            {
                if (serialport.IsOpen)
                {
                    this.serialport.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OnReceive(string id, string message)
        {
            if (eventReceiveData != null)
            {
                eventReceiveData(id, message);
            }
        }

        public static string[] GetSerialPort()
        {
            return SerialPort.GetPortNames();
        }
    }
}
