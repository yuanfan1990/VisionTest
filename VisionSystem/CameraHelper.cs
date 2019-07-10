using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using System.Threading;
using Cognex.VisionPro.ImageFile;

using System.Net;

namespace VisionSystem
{
    public class CameraHelper
    {
        private ToolBlockHelper[] toolblockhelper;
        private CogImageFileTool imagetool = new CogImageFileTool();
        private ICogImage image = null;
        private double exposure;
        private double contrast;
        private double light;
        private string name;
        

        private Thread t;
        private bool isrun;
        private bool trig;
        private bool[] trigcmd;
        private bool isrunning;
        private string trig_id;


        public int Index { get; set; }
        public string Sn { get; set; }
        public string Readimagepath { get; set; }
        public ICogFrameGrabber Framegrabber { get; set; }
        public ICogAcqFifo Acqfifo { get; set; }
        public int ToolblockNumber { get; set; }
        public string Trigger { get; set; }
        public string TriggrType { get; set; }
        public IPEndPoint TcpclientEndPoint { get; set; }
        public int DataLength { get; set; }
        public int LastLength { get; set; }


        public event Action<string, string> eventSerialPortSendData;
        public event Action<string, IPEndPoint, string> eventTcpServerSendData;
        public event Action<string, int, int, ushort[]> eventPLCSiemensSendData;
        public event Action<string, int, int, ushort[]> eventPLCOmronSendData;


        public void Init()
        {
            int n;
            string name, exposure, contrast, light;
            n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']", this.Index), "name", out name);
            n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/param", this.Index), "exposure", out exposure);
            n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/param", this.Index), "contrast", out contrast);
            n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/param", this.Index), "light", out light);

            string width, height;
            n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']", this.Index), "width", out width);
            n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']", this.Index), "height", out height);
            Global.ImageSize[this.Index - 1, 0] = Convert.ToInt32(width);
            Global.ImageSize[this.Index - 1, 1] = Convert.ToInt32(height);

            string toolblockid;
            n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/toolblockid", this.Index), out toolblockid);

            string trigger;
            n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/com", this.Index), "trigger", out trigger);

            this.name = name;
            this.exposure = Convert.ToDouble(exposure);
            this.contrast = Convert.ToDouble(contrast);
            this.light = Convert.ToDouble(light);
            this.Readimagepath = Global.BaseTestImageFolder + this.Index + ".bmp";
            this.Trigger = trigger;


            string[] ids = toolblockid.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            this.ToolblockNumber = ids.Length;

            this.trigcmd = new bool[this.ToolblockNumber];
            SetTrigCmd();

            int sum = 0;
            toolblockhelper = new ToolBlockHelper[this.ToolblockNumber];
            for (int i = 0; i < ids.Length; i++)
            {
                toolblockhelper[i] = new ToolBlockHelper();
                toolblockhelper[i].Index = i + 1;
                toolblockhelper[i].ToolId = Convert.ToInt32(ids[i]);
                toolblockhelper[i].SaveImagePath = Global.SaveImagePath + "\\" + this.name + "\\";
                toolblockhelper[i].Init();
                toolblockhelper[i].Start();

                sum = sum + toolblockhelper[i].DataLength;
            }
            this.DataLength = sum;

            WriteAcqFifoParam();
        }

        public void Start()
        {
            if (t == null || t.ThreadState == ThreadState.Aborted || t.ThreadState == ThreadState.Stopped)
            {
                this.isrun = true;
                t = new Thread(Run);
                t.IsBackground = true;
                t.Start();
            }
        }

        private void Run()
        {
            int step = 1;
            
            List<bool> cmd = new List<bool>();
            string data = "";
            string[] data2 = new string[this.ToolblockNumber];

            while (isrun)
            {
                switch (step)
                {
                    case 1:
                        if (this.trig)
                        {
                            this.isrunning = true;
                            this.trig = false;
                            cmd = this.trigcmd.ToList();
                            step = 2;
                        }
                        break;
                    case 2:

                        if (Global.OnOffLineState)
                        {
                            image = OneShot();
                        }
                        else
                        {
                            image = ReadImage(this.Readimagepath);
                        }
                        step = 3;
                        break;
                    case 3:
                        for (int i = 0; i < cmd.Count; i++)
                        {
                            if (cmd[i])
                            {
                                toolblockhelper[i].SetIsRunComplete(false);
                                if (image != null)
                                {
                                    toolblockhelper[i].SetImage(image.CopyBase(CogImageCopyModeConstants.CopyPixels));
                                }
                                toolblockhelper[i].Trig();
                            }
                        }
                        step = 4;
                        break;
                    case 4:
                        bool r = true;
                        for (int i = 0; i < cmd.Count; i++)
                        {
                            if (cmd[i])
                            {
                                r = r && toolblockhelper[i].GetIsRunComplete(); 
                            }
                        }
                        if (r)
                        {
                            data = "";
                            data2 = new string[this.ToolblockNumber];
                            for (int i = 0; i < cmd.Count; i++)
                            {
                                if (cmd[i])
                                {
                                    toolblockhelper[i].SetIsRunComplete(false);
                                    data2[i] = toolblockhelper[i].Result;
                                    data = data + toolblockhelper[i].Result + "!"; 
                                }
                                else
                                {
                                    data2[i] = GetZero(toolblockhelper[i].DataLength, true);
                                }
                            }
                            data = this.Index + ":" + data.Substring(0, data.Length - 1) + ";";
                            step = 5;
                        }
                        break;
                    case 5:
                        switch (this.TriggrType)
                        {
                            case "串口":
                                OnSerialPortSendData(this.trig_id, data);
                                this.isrunning = false;
                                break;
                            case "Tcp服务器":
                                OnTcpServerSendData(this.trig_id, this.TcpclientEndPoint, data);
                                this.isrunning = false;
                                break;
                            case "PLC-Siemens":
                                ushort[] senddata1;
                                if (GetData(data2, out senddata1))
                                {
                                    OnPLCSiemensSendData(this.trig_id, this.Index, this.LastLength * 2, senddata1);
                                }
                                else
                                {
                                    this.isrunning = false;
                                }
                                break;
                            case "PLC-Omron":
                                ushort[] senddata2;
                                if (GetData(data2, out senddata2))
                                {
                                    OnPLCOmronSendData(this.trig_id, this.Index, this.LastLength * 2, senddata2);
                                }
                                else
                                {
                                    this.isrunning = false;
                                }
                                break;
                            case "PLC-Mitsubishi":
                                break;
                            default:
                                this.isrunning = false;
                                break;
                        }
                        step = 6;
                        break;
                    case 6:
                        if (!this.isrunning)
                        {
                            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.ffff") + ":end CAMERA " + this.Index);
                            step = 1;
                        }
                        break;
                    default:
                        break;
                }

                Thread.Sleep(10);
            }
        }

        public void Trig()
        {
            this.trig = true;
        }

        public void SetTrigCmd(bool[] b)
        {
            if (b.Length > this.ToolblockNumber)
            {
                bool[] temp = new bool[this.ToolblockNumber];
                Array.Copy(b, temp, this.ToolblockNumber);
                this.trigcmd = temp;
            }
            else
            {
                this.trigcmd = b;
            }
        }

        public void SetTrigCmd()
        {
            for (int i = 0; i < this.ToolblockNumber; i++)
            {
                this.trigcmd[i] = true;
            }
        }

        public void SetTrigId(string id)
        {
            this.trig_id = id;
        }

        public bool GetIsRunning()
        {
            return this.isrunning;
        }

        public void SetIsRunning(bool b)
        {
            this.isrunning = b;
        }

        public void SetReadImagePath(string path)
        {
            this.Readimagepath = path;

            for (int i = 0; i < this.ToolblockNumber; i++)
            {
                this.toolblockhelper[i].Recorddisplay.InteractiveGraphics.Clear();
                this.toolblockhelper[i].Recorddisplay.StaticGraphics.Clear();
                this.toolblockhelper[i].Recorddisplay.Image = ReadImage(this.Readimagepath);
            }
        }

        public ToolBlockHelper GetToolBlockHelper(int index)
        {
            return this.toolblockhelper[index];
        }

        public void Stop()
        {
            isrun = false;

            for (int i = 0; i < this.ToolblockNumber; i++)
            {
                if (toolblockhelper[i] != null)
                {
                    toolblockhelper[i].Stop();
                }
            }
        }

        private ICogImage OneShot()
        {
            if (this.Framegrabber == null)
            {
                return null;
            }
            Int64 COUNTS_PER_IMAGE = 100;
            Int64 lastTimeStamp = 0;

            lastTimeStamp = (long)this.Framegrabber.OwnedGigEAccess.TimeStampCounter;
            int acqTicket = this.Acqfifo.StartAcquire();

            CogAcqInfo info = new CogAcqInfo();
            info.RequestedTicket = acqTicket;

            ICogImage image = this.Acqfifo.CompleteAcquireEx(info);
            if (info.TimeStamp - lastTimeStamp > COUNTS_PER_IMAGE)
            {
                // handle missed image
            }
            lastTimeStamp = info.TimeStamp;
            return image;
        }

        private ICogImage ReadImage(string path)
        {
            if (path == null)
            {
                return null;
            }

            ICogImage image = null;
            try
            {
                imagetool.Operator.Open(path, CogImageFileModeConstants.Read);
                imagetool.Run();
                image = imagetool.OutputImage;
                imagetool.Operator.Close();
                GC.Collect();
            }
            catch
            {

            }

            return image;
        }

        private void OnSerialPortSendData(string id, string message)
        {
            if (eventSerialPortSendData != null)
            {
                eventSerialPortSendData(id, message);
            }
        }

        private void OnTcpServerSendData(string id, IPEndPoint point, string message)
        {
            if (eventTcpServerSendData != null)
            {
                eventTcpServerSendData(id, point, message);
            }
        }

        private void OnPLCSiemensSendData(string id, int cameraindex, int index, ushort[] data)
        {
            if (eventPLCSiemensSendData != null)
            {
                eventPLCSiemensSendData(id, cameraindex, index, data);
            }
        }

        private void OnPLCOmronSendData(string id, int cameraindex, int index, ushort[] data)
        {
            if (eventPLCOmronSendData != null)
            {
                eventPLCOmronSendData(id, cameraindex, index, data);
            }
        }

        private string GetZero(int num, bool comma)
        {
            string s = "";
            for (int i = 0; i < num; i++)
            {
                s = s + "0" + (comma ? "," : "");
            }

            return s.Substring(0, s.Length-1);
        }

        private bool GetData(string[] str, out ushort[] data)
        {
            List<ushort> list = new List<ushort>();
            for (int i = 0; i < str.Length; i++)
            {
                if (string.IsNullOrEmpty(str[i]))
                {
                    data = list.ToArray();
                    return false;
                }

                string[] d = str[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < d.Length; j++)
                {
                    list.Add(Convert.ToUInt16(d[j]));
                }
            }

            data = list.ToArray();
            return true;
        }

        private void WriteAcqFifoParam()
        {
            if (this.Acqfifo == null)
            {
                return;
            }

            ICogAcqExposure exposureParams = this.Acqfifo.OwnedExposureParams;
            if (exposureParams != null)
            {
                this.Acqfifo.OwnedExposureParams.Exposure = Convert.ToDouble(this.exposure);
                this.Acqfifo.Prepare();
            }

            ICogAcqContrast contrastParams = this.Acqfifo.OwnedContrastParams;
            if (contrastParams != null)
            {
                this.Acqfifo.OwnedContrastParams.Contrast = Convert.ToDouble(this.contrast);
                this.Acqfifo.Prepare();
            }

            ICogAcqLight lightParams = this.Acqfifo.OwnedLightParams;
            if (lightParams != null)
            {
                this.Acqfifo.OwnedLightParams.LightPower = Convert.ToDouble(this.light);
                this.Acqfifo.Prepare();
            }
        }
    }
}
