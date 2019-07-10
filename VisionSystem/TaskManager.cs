using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using System.Windows.Forms;
using System.Threading;

using System.Net;

namespace VisionSystem
{
    public enum CommunicationType
    {
        串口,
        TCP服务器,
        PLC,
        None
    }

    public enum PLCType
    {
        Siemens,
        Mitsubishi,
        Omron
    }

    public class TaskManager
    {
        private CameraHelper[] camerahelper;
        private List<object> communicationlist = new List<object>();
        private Thread t;
        private bool isrun;

        public void Init()
        {
            int n;
            n = XmlHelper.Read(Global.ProjectConfigFilePath, "/ProjectManager/saveimagepath", out Global.SaveImagePath);

            Global.CameraSerialNumbers = new string[Global.CameraNumber];
            for (int i = 0; i < Global.CameraNumber; i++)
            {
                n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/param", i + 1), "sn", out Global.CameraSerialNumbers[i]);
            }

            Global.ConnectCameras();

            int lengthindex = 0;
            camerahelper = new CameraHelper[Global.CameraNumber];
            for (int i = 0; i < Global.CameraNumber; i++)
            {
                camerahelper[i] = new CameraHelper();
                camerahelper[i].Index = i + 1;
                camerahelper[i].Sn = Global.CameraSerialNumbers[i];
                camerahelper[i].Framegrabber = Global.FrameGrabbers[i];
                camerahelper[i].Acqfifo = Global.AcqFifos[i];
                camerahelper[i].eventSerialPortSendData += TaskManager_eventSerialPortSendData;
                camerahelper[i].eventTcpServerSendData += TaskManager_eventTcpServerSendData;
                camerahelper[i].eventPLCSiemensSendData += TaskManager_eventPLCSiemensSendData;
                camerahelper[i].eventPLCOmronSendData += TaskManager_eventPLCOmronSendData;
                camerahelper[i].Init();

                lengthindex = lengthindex + (i == 0 ? 0 : camerahelper[i - 1].DataLength);
                camerahelper[i].LastLength = lengthindex;

                camerahelper[i].Start();
            }
        }

        public bool Start()
        {
            CommunicationInit();

            return CommunicationStart();
        }

        public void Stop()
        {
            CommunicationStop();
        }

        public void Close()
        {
            Stop();

            for (int i = 0; i < Global.CameraNumber; i++)
            {
                if (camerahelper[i] != null)
                {
                    camerahelper[i].Stop();
                }
            }

            Global.DisConnectCameras();
        }

        public void Trig(int index)
        {
            camerahelper[index].SetTrigCmd();
            camerahelper[index].TriggrType = "None";
            camerahelper[index].Trig();
        }

        public void LoadImage(int index)
        {
            try
            {
                OpenFileDialog fd = new OpenFileDialog();
                fd.Title = "请选择一张图像";
                fd.Multiselect = false;
                fd.Filter = "图像文件|*.bmp;*.png;*.jpg;*.jpeg|All files|*.*";

                if (fd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                for (int i = 0; i < camerahelper[index].ToolblockNumber; i++)
                {
                    camerahelper[index].GetToolBlockHelper(i).Recorddisplay.Record = null;
                }

                camerahelper[index].SetReadImagePath(fd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        public void Edit(int cameraindex, int toolindex, string name)
        {
           if (toolindex < camerahelper[cameraindex].ToolblockNumber)
            {
                ToolBlockHelper toolBlockhelper = camerahelper[cameraindex].GetToolBlockHelper(toolindex);

                FormEdit fm = new FormEdit(toolBlockhelper);
                fm.ShowDialog(); 
            }
            else
            {
                string filename = Global.ProjectPath + name;
                System.Diagnostics.Process.Start("notepad.exe", filename);
            }
        }

        public void Config(int cameraindex)
        {
            FormCameraConfig fm = new FormCameraConfig(cameraindex);
            fm.ShowDialog();
        }

        public void Fresh()
        {
            for (int i = 0; i < Global.CameraNumber; i++)
            {
                for (int j = 0; j < camerahelper[i].ToolblockNumber; j++)
                {
                    camerahelper[i].GetToolBlockHelper(j).GetScriptHelper().Compile();
                }
            }
        }

        public void Live(int index, bool b)
        {
            if (Global.AcqFifos[index] == null)
            {
                return;
            }

            camerahelper[index].GetToolBlockHelper(0).Recorddisplay.StaticGraphics.Clear();
            camerahelper[index].GetToolBlockHelper(0).Recorddisplay.InteractiveGraphics.Clear();

            if (b)
            {
                //for (int i = 0; i < camerahelper[index].ToolblockNumber; i++)
                {
                    camerahelper[index].GetToolBlockHelper(0).Recorddisplay.StartLiveDisplay(Global.AcqFifos[index]);
                    camerahelper[index].GetToolBlockHelper(0).Recorddisplay.Fit(true);
                }

            }
            else
            {
                //for (int i = 0; i < camerahelper[index].ToolblockNumber; i++)
                {
                    camerahelper[index].GetToolBlockHelper(0).Recorddisplay.StopLiveDisplay();
                }
            }
        }

        private void TaskManager_eventSerialPortSendData(string id, string message)
        {
            for (int i = 0; i < this.communicationlist.Count; i++)
            {
                if (!(this.communicationlist[i] is SerialPortHelper)) continue;

                SerialPortHelper serialporthelper = this.communicationlist[i] as SerialPortHelper;

                if (serialporthelper.ID == id)
                {
                    serialporthelper.Write(message);
                }
            }
        }

        private void TaskManager_eventTcpServerSendData(string id, IPEndPoint point, string message)
        {
            for (int i = 0; i < this.communicationlist.Count; i++)
            {
                if (!(this.communicationlist[i] is TcpServerHelper)) continue;

                TcpServerHelper tcpserverhelper = this.communicationlist[i] as TcpServerHelper;

                if (tcpserverhelper.ID == id)
                {
                    tcpserverhelper.Send(point, message);
                }
            }
        }

        private void TaskManager_eventPLCSiemensSendData(string id, int cameraindex, int index, ushort[] data)
        {
            lock (this)
            {
                for (int i = 0; i < this.communicationlist.Count; i++)
                {
                    if (!(this.communicationlist[i] is PLCSiemensHelper)) continue;

                    PLCSiemensHelper plc = this.communicationlist[i] as PLCSiemensHelper;

                    if (plc.ID == id)
                    {
                        int step = 1;
                        bool b = true;
                        while (b && isrun)
                        {
                            switch (step)
                            {
                                case 1:
                                    plc.WriteIndex = index;
                                    plc.WriteData = data;
                                    plc.WriteEnable = true;

                                    step = 10;
                                    break;
                                case 10:
                                    if (plc.ReadData[cameraindex-1] == 0)
                                    {
                                        plc.WriteEnable = false;
                                        step = 20;
                                    }
                                    break;
                                case 20:
                                    Thread.Sleep(10);
                                    camerahelper[cameraindex-1].SetIsRunning(false);
                                    b = false;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                } 
            }
        }

        private void TaskManager_eventPLCOmronSendData(string id, int cameraindex, int index, ushort[] data)
        {
            lock (this)
            {
                for (int i = 0; i < this.communicationlist.Count; i++)
                {
                    if (!(this.communicationlist[i] is PLCOmronHelper)) continue;

                    PLCOmronHelper plc = this.communicationlist[i] as PLCOmronHelper;

                    if (plc.ID == id)
                    {
                        int step = 1;
                        bool b = true;
                        while (b && isrun)
                        {
                            switch (step)
                            {
                                case 1:
                                    plc.WriteIndex = index;
                                    plc.WriteData = data;
                                    plc.WriteEnable = true;

                                    step = 10;
                                    break;
                                case 10:
                                    if (plc.ReadData[cameraindex - 1] == 0)
                                    {
                                        plc.WriteEnable = false;
                                        step = 20;
                                    }
                                    break;
                                case 20:
                                    Thread.Sleep(10);
                                    camerahelper[cameraindex - 1].SetIsRunning(false);
                                    b = false;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void TcpServer_eventReceiveData(string id, System.Net.IPEndPoint point, string message)
        {
            string[] s1 = message.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < s1.Length; i++)
            {
                string s2 = s1[i];

                string[] s3 = s2.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                int cameraindex = Convert.ToInt32(s3[0]) - 1;

                bool[] cmd = new bool[s3.Length - 1];
                for (int j = 0; j < s3.Length - 1; j++)
                {
                    cmd[j] = s3[j + 1] == "1" ? true : false;
                }

                if (camerahelper[cameraindex].GetIsRunning())
                {
                    continue;
                }

                camerahelper[cameraindex].TriggrType = "Tcp服务器";
                camerahelper[cameraindex].TcpclientEndPoint = point;
                camerahelper[cameraindex].SetTrigId(id);
                camerahelper[cameraindex].SetTrigCmd(cmd);
                camerahelper[cameraindex].Trig();
            }
        }

        private void SerialPort_eventReceiveData(string id, string message)
        {
            string[] s1 = message.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < s1.Length; i++)
            {
                string s2 = s1[i];

                string[] s3 = s2.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                int cameraindex = Convert.ToInt32(s3[0]) - 1;

                bool[] cmd = new bool[s3.Length - 1];
                for (int j = 0; j < s3.Length - 1; j++)
                {
                    cmd[j] = s3[j + 1] == "1" ? true : false;
                }

                if (camerahelper[cameraindex].GetIsRunning())
                {
                    continue;
                }

                camerahelper[cameraindex].TriggrType = "串口";
                camerahelper[cameraindex].SetTrigId(id);
                camerahelper[cameraindex].SetTrigCmd(cmd);
                camerahelper[cameraindex].Trig();
            } 
        }

        private void CommunicationInit()
        {
            int n;
            List<List<string>> nodes;
            n = XmlHelper.Read(Global.CommunicationConfigFilePath, "/CommunicationManager/Communication[@id]", out nodes);

            this.communicationlist.Clear();

            for (int i = 0; i < nodes.Count; i++)
            {
                List<string> list = nodes[i];
                CommunicationType type = (CommunicationType)Enum.Parse(typeof(CommunicationType), list[1]);
                if (type == CommunicationType.串口)
                {
                    SerialPortHelper obj = new SerialPortHelper(list[2], Convert.ToInt32(list[3]), Convert.ToInt32(list[4]), Convert.ToInt32(list[5]));
                    obj.ID = list[0];
                    obj.eventReceiveData += SerialPort_eventReceiveData;
                    communicationlist.Add(obj);
                }
                else if (type == CommunicationType.TCP服务器)
                {
                    TcpServerHelper obj = new TcpServerHelper(list[2], Convert.ToInt32(list[3]));
                    obj.ID = list[0];
                    obj.eventReceiveData += TcpServer_eventReceiveData;
                    communicationlist.Add(obj);
                }
                else if (type == CommunicationType.PLC)
                {
                    int index = list[0].IndexOf('-');
                    PLCType type2 = (PLCType)Enum.Parse(typeof(PLCType), list[0].Substring(0, index));

                    if (type2 == PLCType.Siemens)
                    {
                        PLCSiemensHelper obj = new PLCSiemensHelper(list[2], list[3], Convert.ToInt32(list[4]), Convert.ToInt32(list[5]), list[6], Convert.ToInt32(list[7]), Convert.ToInt32(list[8]), list[9], Convert.ToInt32(list[10]));
                        obj.ID = list[0];

                        int sum = 0;
                        for (int j = 0; j < Global.CameraNumber; j++)
                        {
                            if (camerahelper[j].Trigger == obj.ID)
                            {
                                sum = sum + camerahelper[j].DataLength;
                            }
                        }
                        obj.SetWriteData(sum);
                        communicationlist.Add(obj);
                    }
                    else if (type2 == PLCType.Omron)
                    {
                        PLCOmronHelper obj = new PLCOmronHelper(list[2], Convert.ToInt32(list[3]), list[4], list[5], Convert.ToInt32(list[6]), Convert.ToInt32(list[7]), list[8], Convert.ToInt32(list[9]));
                        obj.ID = list[0];

                        int sum = 0;
                        for (int j = 0; j < Global.CameraNumber; j++)
                        {
                            if (camerahelper[j].Trigger == obj.ID)
                            {
                                sum = sum + camerahelper[j].DataLength;
                            }
                        }
                        obj.SetWriteData(sum);
                        communicationlist.Add(obj);
                    }
                }
            }
        }

        private bool CommunicationStart()
        {
            for (int i = 0; i < Global.CameraNumber; i++)
            {
                camerahelper[i].SetIsRunning(false);
            }

            try
            {
                for (int i = 0; i < this.communicationlist.Count; i++)
                {
                    if (this.communicationlist[i] is SerialPortHelper)
                    {
                        ((SerialPortHelper)this.communicationlist[i]).Start();
                    }
                    else if (this.communicationlist[i] is TcpServerHelper)
                    {
                        ((TcpServerHelper)this.communicationlist[i]).Start();
                    }
                    else if (this.communicationlist[i] is PLCSiemensHelper)
                    {
                        ((PLCSiemensHelper)this.communicationlist[i]).Open();
                    }
                    else if (this.communicationlist[i] is PLCOmronHelper)
                    {
                        ((PLCOmronHelper)this.communicationlist[i]).Open();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (t == null || t.ThreadState == ThreadState.Aborted || t.ThreadState == ThreadState.Stopped)
            {
                isrun = true;
                t = new Thread(PLC_Check);
                t.IsBackground = true;
                t.Start();
            }

            return true;
        }

        private void CommunicationStop()
        {
            isrun = false;
            //while (t.ThreadState != ThreadState.Stopped)
            //{
            //    Application.DoEvents();
            //    Thread.Sleep(50);
            //}

            for (int i = 0; i < this.communicationlist.Count; i++)
            {
                if (this.communicationlist[i] is SerialPortHelper)
                {
                    ((SerialPortHelper)this.communicationlist[i]).Stop();
                }
                else if (this.communicationlist[i] is TcpServerHelper)
                {
                    ((TcpServerHelper)this.communicationlist[i]).Stop();
                }
                else if (this.communicationlist[i] is PLCSiemensHelper)
                {
                    ((PLCSiemensHelper)this.communicationlist[i]).Close();
                }
                else if (this.communicationlist[i] is PLCOmronHelper)
                {
                    ((PLCOmronHelper)this.communicationlist[i]).Close();
                }
            }
        }

        private void PLC_Check()
        {
            Thread.Sleep(300);

            while (isrun)
            {
                for (int i = 0; i < this.communicationlist.Count; i++)
                {
                    if (this.communicationlist[i] is PLCSiemensHelper)
                    {
                        PLCSiemensHelper plc = this.communicationlist[i] as PLCSiemensHelper;

                        for (int j = 0; j < plc.ReadData.Length; j++)
                        {
                            if (j >= Global.CameraNumber) break;

                            if (camerahelper[j].GetIsRunning()) continue;

                            if (plc.ReadData[j] == 0) continue;

                            bool[] cmd = new bool[camerahelper[j].ToolblockNumber];

                            cmd = GetBitStatus(plc.ReadData[j], cmd.Length);

                            camerahelper[j].TriggrType = "PLC-Siemens";
                            camerahelper[j].SetTrigId(plc.ID);
                            camerahelper[j].SetTrigCmd(cmd);
                            camerahelper[j].Trig();
                        }
                    }
                    else if (this.communicationlist[i] is PLCOmronHelper)
                    {
                        PLCOmronHelper plc = this.communicationlist[i] as PLCOmronHelper;

                        for (int j = 0; j < plc.ReadData.Length; j++)
                        {
                            if (j >= Global.CameraNumber) break;

                            if (camerahelper[j].GetIsRunning()) continue;

                            if (plc.ReadData[j] == 0) continue;

                            bool[] cmd = new bool[camerahelper[j].ToolblockNumber];

                            cmd = GetBitStatus(plc.ReadData[j], cmd.Length);

                            camerahelper[j].TriggrType = "PLC-Omron";
                            camerahelper[j].SetTrigId(plc.ID);
                            camerahelper[j].SetTrigCmd(cmd);
                            camerahelper[j].Trig();
                        }
                    }
                }

                Thread.Sleep(20);
            }
        }

        private bool[] GetBitStatus(ushort x, int length)
        {
            bool[] b = new bool[length];

            string s1 = "0000000000000000" + Convert.ToString(x, 2);

            for (int i = length, j = 0; i > 0; i--, j++)
            {
                b[j] = s1[s1.Length - 1 - j] == '1' ? true : false;
            }

            return b;
        }
    }
}
