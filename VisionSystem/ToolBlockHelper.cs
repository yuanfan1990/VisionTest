using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using System.Threading;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ImageFile;

using CSScriptLibrary;

namespace VisionSystem
{
    public class ToolBlockHelper
    {
        //字段
        private ScriptHelper scripthelper;
        private ICogImage image;
        private string vppname;
        private string recordname;

        private Thread t;
        private bool trig;
        private bool isrun;
        private bool isruncomplete;
        private int count;

        //属性
        public int Index { get; set; }
        public CogRecordDisplay Recorddisplay { get; set; }
        public int ToolId { get; set; }
        public string SaveImagePath { get; set; }
        public string VppPath { get; set; }
        public string Result { get; set; }
        public int DataLength { get; set; }
        public CogToolBlock Toolblock { get; set; }

        public ToolBlockHelper()
        {

        }

        public void Init()
        {
            int n;
            string display;
            n = XmlHelper.Read(Global.ToolBlockConfigFilePath, string.Format("/ToolBlockManager/ToolBlock[@id='{0}']", this.ToolId), "display", out display);

            string datalength;
            n = XmlHelper.Read(Global.ToolBlockConfigFilePath, string.Format("/ToolBlockManager/ToolBlock[@id='{0}']/datalength", this.ToolId), out datalength);

            string vppname, recordname;
            n = XmlHelper.Read(Global.ToolBlockConfigFilePath, string.Format("/ToolBlockManager/ToolBlock[@id='{0}']/vppname", this.ToolId), out vppname);
            n = XmlHelper.Read(Global.ToolBlockConfigFilePath, string.Format("/ToolBlockManager/ToolBlock[@id='{0}']/recordname", this.ToolId), out recordname);

            this.vppname = vppname;
            this.recordname = recordname;
            this.DataLength = Convert.ToInt32(datalength);
            this.VppPath = Global.ProjectPath + vppname;

            this.Recorddisplay = Global.RecordDisplays[this.ToolId - 1];

            ThreadPool.QueueUserWorkItem((x)=> { this.Toolblock = CogSerializer.LoadObjectFromFile(x.ToString()) as CogToolBlock; }, this.VppPath);
            //this.Toolblock = CogSerializer.LoadObjectFromFile(this.VppPath) as CogToolBlock;

            this.scripthelper = new ScriptHelper();
            this.scripthelper.Index = this.ToolId;
            this.scripthelper.Init();
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
            while (this.isrun)
            {
                if (this.trig)
                {
                    this.trig = false;

                    VppRun();

                    Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.ffff") + ":stop  " + this.Index);

                    this.isruncomplete = true;
                }

                Thread.Sleep(5);
            }
        }

        public void Stop()
        {
            this.isrun = false;
        }

        public void Trig()
        {
            this.trig = true;
        }

        public bool GetIsRunComplete()
        {
            return this.isruncomplete;
        }

        public void SetIsRunComplete(bool b)
        {
            this.isruncomplete = b;
        }

        public ScriptHelper GetScriptHelper()
        {
            return this.scripthelper;
        }

        public void SetImage(ICogImage image)
        {
            this.image = image;
        }

        private void VppRun()
        {
            try
            {
                PreRun();

                this.Toolblock.Run();

                ICogRecord temprecord = this.Toolblock.CreateLastRunRecord();
                temprecord = temprecord.SubRecords[this.recordname];

                //this.Recorddisplay.InteractiveGraphics.Clear();
                //this.Recorddisplay.StaticGraphics.Clear();

                this.Recorddisplay.Record = temprecord;
                this.Recorddisplay.Fit(true);

                AfterRun();
            }
            catch
            {
                
            }
        }

        private void PreRun()
        {
            Dictionary<string, object> dicInput = this.scripthelper.Script.InputRun();

            GetInput(dicInput);

            this.Toolblock.Inputs["InputImage"].Value = this.image;

            this.Toolblock.Inputs.Run();
        }

        private void AfterRun()
        {
            Dictionary<string, object> dicOutput = GetOutput();

            List<string> funs = this.scripthelper.Script.OutputRun(dicOutput);

            AnalysisFunc(funs);

            AddLabel(0, 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), CogColorConstants.Red, 15);

            AddLabel(0, 60, count.ToString(), CogColorConstants.Cyan, 25);

            this.count++;
        }

        private void AnalysisFunc(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int r1 = list[i].IndexOf("(");
                int r2 = list[i].IndexOf(")");

                string cmd = list[i].Substring(0, r1);
                string par = list[i].Substring(r1 + 1, r2 - r1 - 1);
                string[] pars = par.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                switch (cmd)
                {
                    case "AddStaticLabel":
                        AddStaticLabel(pars[0].Trim(), pars[1].Trim(), pars[2].Trim(), pars[3].Trim(), pars[4].Trim());
                        break;
                    case "SendResult":
                        string s = "";
                        for (int j = 0; j < pars.Length; j++)
                        {
                            s = s + pars[j] + ",";
                        }
                        SendResult(s.Substring(0, s.Length-1));
                        break;
                    case "SaveImage":
                        SaveImage(pars[0].ToLower());
                        break;
                    default:
                        break;
                }
            }
        }

        private void SendResult(string r)
        {
            this.Result = r;
        }

        private void SaveImage(string r)
        {
            if (r == "ng")
            {
                SaveResultImage(this.SaveImagePath + "NG\\T" + this.Index);
            }
            if (r == "ok")
            {
                SaveResultImage(this.SaveImagePath + "OK\\T" + this.Index);
            }
            if (r == "input")
            {
                SaveInputImage(this.image, this.SaveImagePath + "Input\\T" + this.Index);
            }
        }

        private void AddLabel(int x, int y, string s, CogColorConstants color, int size)
        {
            CogGraphicLabel label = new CogGraphicLabel();
            label.Font = new Font("console", size);
            label.Color = color;
            label.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
            label.SetXYText(x, y, s);
            label.SelectedSpaceName = "#";

            this.Recorddisplay.StaticGraphics.Add(label, "");
        }

        private void AddStaticLabel(string txt, string x, string y, string color, string size)
        {
            CogGraphicLabel label = new CogGraphicLabel();

            label.Font = new Font("Console", Convert.ToInt32(size));

            label.Alignment = CogGraphicLabelAlignmentConstants.BaselineLeft;

            label.SetXYText(Convert.ToDouble(x), Convert.ToDouble(y), txt);

            label.SelectedSpaceName = "#";

            CogColorConstants c = CogColorConstants.None;

            foreach (int v in Enum.GetValues(typeof(CogColorConstants)))
            {
                string strName = Enum.GetName(typeof(CogColorConstants), v);
                if (color.ToLower() == strName.ToLower())
                {
                    c = (CogColorConstants)Enum.Parse(typeof(CogColorConstants), strName);
                }
            }

            label.Color = c;

            this.Recorddisplay.StaticGraphics.Add(label, "");
        }

        private void SaveResultImage(string path)
        {
            string folderpath = path + "\\" + DateTime.Now.ToString("yyyyMMdd");
            if (!System.IO.Directory.Exists(folderpath))
            {
                System.IO.Directory.CreateDirectory(folderpath);
            }

            string filepath = folderpath + "\\" + DateTime.Now.ToString("HH-mm-ss.fff") + ".bmp";
            if (!System.IO.Directory.Exists(filepath))
            {
                this.Recorddisplay.CreateContentBitmap(Cognex.VisionPro.Display.CogDisplayContentBitmapConstants.Display).Save(filepath);
            }
        }

        private void SaveInputImage(ICogImage Image, string path)
        {
            if (Image == null)
            {
                return;
            }
            string folderpath = path + "\\" + DateTime.Now.ToString("yyyyMMdd");
            if (!System.IO.Directory.Exists(folderpath))
            {
                System.IO.Directory.CreateDirectory(folderpath);
            }

            string filepath = folderpath + "\\" + DateTime.Now.ToString("HH-mm-ss.fff") + ".bmp";
            if (!System.IO.Directory.Exists(filepath))
            {
                CogImageFile ImageFile = new CogImageFile();
                ImageFile.Open(filepath, CogImageFileModeConstants.Write);
                ImageFile.Append(Image);
                ImageFile.Close();
            }
        }

        private void GetInput(Dictionary<string, object> dic)
        {
            CogToolBlockTerminalCollection inputs = this.Toolblock.Inputs;

            for (int i = 0; i < inputs.Count; i++)
            {
                foreach (KeyValuePair<string, object> item in dic)
                {
                    if (inputs[i].Name == item.Key)
                    {
                        inputs[i].Value = item.Value;
                    }
                }
            }
        }

        private Dictionary<string, object> GetOutput()
        {
            CogToolBlockTerminalCollection outputs = this.Toolblock.Outputs;

            Dictionary<string, object> dic = new Dictionary<string, object>();

            foreach (CogToolBlockTerminal item in outputs)
            {
                dic[item.Name] = item.Value;
            }

            return dic;
        }

        private void UpdateRecordDisplay(CogRecordDisplay recorddislpay, ICogRecord record)
        {
            if (recorddislpay.InvokeRequired)
            {
                recorddislpay.BeginInvoke(new Action<CogRecordDisplay, ICogRecord>(UpdateRecordDisplay), new object[] { recorddislpay, record});
            }
            else
            {
                recorddislpay.Record = record;
                recorddislpay.Fit(true);
            }
        }
    }
}
