using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VisionSystem
{
    public class PLCOmronHelper : AbstractPLC
    {
        //字段
        private OmronFinsHelper fins;
        private string ip;
        private int port;
        private string localip;
        private Thread thread_read;
        private string readblock;
        private int readstart;
        private int readlength;
        private string writeblock;
        private int writestart;
        private bool isrun;

        //属性
        public ushort[] ReadData { get; set; }
        public bool[] RunState { get; set; }
        public string ID { get; set; }
        public bool WriteEnable { get; set; }
        public int WriteIndex { get; set; }
        public ushort[] WriteData { get; set; }

        //构造
        public PLCOmronHelper(string ip, int port, string localip, string readblock, int readstart, int readlength, string writeblock, int writestart)
        {
            this.ip = ip;
            this.port = port;
            this.localip = localip;

            this.readblock = readblock;
            this.readstart = readstart;
            this.readlength = readlength;
            this.writeblock = writeblock;
            this.writestart = writestart;

            RunState = new bool[this.readlength];
            ReadData = new ushort[this.readlength];
        }

        public override bool Open()
        {
            fins = new OmronFinsHelper(this.ip, this.port, this.localip);

            bool b1 = fins.FINS_Connect();

            if (b1)
            {
                bool b2 = fins.FINS_HandShake_Command();

                if (b2)
                {
                    StartRead();
                }
            }

            return b1;
        }

        public override void Close()
        {
            try
            {
                if (thread_read != null)
                {
                    isrun = false;
                    while (thread_read.ThreadState != ThreadState.Stopped)
                    {
                        Thread.Sleep(10);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Read(int start, int len, out ushort[] buffer)
        {
            bool b = fins.FINS_Read_Command(this.readblock, (ushort)start, 0, (ushort)len, out buffer);

            return b;
        }

        public override bool Write(int start, ushort[] buffer)
        {
            if (buffer == null)
            {
                return false;
            }

            bool b = fins.FINS_Write_Command(this.writeblock, (ushort)start, buffer);

            return b;
        }

        public void SetWriteData(int length)
        {
            WriteData = new ushort[length];
        }

        private void StartRead()
        {
            if (thread_read == null || thread_read.ThreadState == ThreadState.Aborted || thread_read.ThreadState == ThreadState.Stopped)
            {
                isrun = false;
                thread_read = new Thread(Run);
                thread_read.IsBackground = true;
                thread_read.Start();
            }
        }

        private void Run()
        {
            ushort[] readdata;

            while (isrun)
            {
                Read(this.readstart, this.readlength, out readdata);

                //Console.Write("READ DATA-----------------------");
                //for (int i = 0; i < readdata.Length; i++)
                //{
                //    Console.Write(readdata[i] + " ");
                //}
                //Console.WriteLine();

                this.ReadData = readdata;

                if (WriteEnable)
                {
                    //Console.Write("Write DATA-------------------------------------------------------------- " + this.writestart + this.WriteIndex);
                    //for (int k = 0; k < WriteData.Length; k++)
                    //{
                    //    Console.Write(WriteData[k] + " ");
                    //}
                    //Console.WriteLine();

                    Write(this.writestart + this.WriteIndex, this.WriteData);
                }

                Thread.Sleep(2);
            }

            if (fins != null)
            {
                fins.FINS_DisConnect();
            }
        }
    }
}
