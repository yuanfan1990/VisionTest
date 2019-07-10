using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using S7.Net;

namespace VisionSystem
{
    public class PLCSiemensHelper : AbstractPLC
    {
        //字段
        private Plc myPlc;
        private CpuType cputype;
        private string ip;
        private short rack;
        private short slot;
        private string readblock;
        private int readstart;
        private int readlength;
        private string writeblock;
        private int writestart;
        private Thread thread_read;
        private bool isrun;

        //属性
        public ushort[] ReadData { get; set; }     
        public bool[] RunState { get; set; }
        public string ID { get; set; }
        public bool WriteEnable { get; set; }
        public int WriteIndex { get; set; }
        public ushort[] WriteData { get; set; }


        //事件

        //构造
        public PLCSiemensHelper(string cputype, string ip, int rack, int slot, string readblock, int readstart, int readlength, string writeblock, int writestart)
        {
            this.cputype = (CpuType)Enum.Parse(typeof(CpuType), cputype);
            this.ip = ip;
            this.rack = (short)rack;
            this.slot = (short)slot;

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
            try
            {
                myPlc = new Plc(this.cputype, this.ip, this.rack, this.slot);
                myPlc.Open();

                if (myPlc.IsConnected)
                {
                    StartRead();
                }
                return myPlc.IsConnected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Close()
        {
            try
            {
                if (thread_read != null)
                {
                    isrun = false;
                    //while (thread_read.ThreadState != ThreadState.Stopped)
                    //{
                    //    Thread.Sleep(10);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Read(string variable, out ushort value)
        {
            object r = myPlc.Read(variable);

            value = (ushort)r;

            return true;
        }

        public override bool Read(int start, int len, out ushort[] buffer)
        {
            DataType type;
            int db;
            Prase(this.readblock, out type, out db);

            object r = myPlc.Read(type, db, start, VarType.Word, len);

            buffer = (ushort[])r;

            return true;
        }

        public override bool Write(int start, ushort[] buffer)
        {
            lock (this)
            {
                if (buffer == null)
                {
                    return false;
                }

                DataType type;
                int db;
                Prase(this.writeblock, out type, out db);

                myPlc.Write(type, db, start, buffer);

                return true; 
            }
        }

        public bool Write(string variable, object value)
        {
            myPlc.Write(variable, value);

            return true;
        }

        public void SetWriteData(int length)
        {
            WriteData = new ushort[length];
        }

        private void Prase(string block, out DataType type, out int num)
        {
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            foreach (char c in block)
            {
                if (Convert.ToInt32(c) >= 48 && Convert.ToInt32(c) <= 57)
                {
                    sb2.Append(c);
                }
                else
                {
                    sb1.Append(c);
                }
            }

            switch (sb1.ToString())
            {
                case "DB":
                    type = DataType.DataBlock;
                    break;
                default:
                    type = DataType.DataBlock;
                    break;
            }

            num = Convert.ToInt32(sb2.ToString());
        }

        private void StartRead()
        {
            if (thread_read == null || thread_read.ThreadState == ThreadState.Aborted || thread_read.ThreadState == ThreadState.Stopped)
            {
                isrun = true;
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

                Console.Write("READ DATA-----------------------");
                for (int i = 0; i < readdata.Length; i++)
                {
                    Console.Write(readdata[i] + " ");
                }
                Console.WriteLine();

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

            if (myPlc != null && myPlc.IsConnected)
            {
                myPlc.Close();
            }
        }
    }
}
