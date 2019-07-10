using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Threading;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ImageFile;

namespace VisionSystem
{
    public partial class FormMain : Form
    {
        private TaskManager taskmanager;
        private CogRecordDisplay[] recorddisplays;
        private static System.Windows.Forms.Label[] labels;


        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2Collapsed = true;
            this.toolStripManual.Image = MyResource.折叠左;

            int n;
            string lastproject;
            n = XmlHelper.Read(Global.ApplicationConfigFilePath, "/AppManager/IsLoadLastProject", out lastproject);

            if (lastproject == "1")
            {
                OpenProject();

                //taskmanager.Start();

                //this.toolStripAutorun.Image = MyResource.start;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (taskmanager != null)
            {
                taskmanager.Close();
            }
        }

        private void toolStripManual_Click(object sender, EventArgs e)
        {
            if (this.splitContainer1.Panel2Collapsed)
            {
                this.splitContainer1.Panel2Collapsed = false;
                this.toolStripManual.Image = MyResource.折叠右;
            }
            else
            {
                this.splitContainer1.Panel2Collapsed = true;
                this.toolStripManual.Image = MyResource.折叠左;
            }
        }

        private void toolStripAutorun_Click(object sender, EventArgs e)
        {
            if (taskmanager.Start())
            {
                this.toolStripAutorun.Image = MyResource.start;
            }  
        }

        private void toolStripStop_Click(object sender, EventArgs e)
        {
            taskmanager.Stop();

            this.toolStripAutorun.Image = MyResource.waitstart;
        }

        private void toolStripRunOnce_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeViewCameraList.SelectedNode;
            if (node == null)
            {
                return;
            }

            taskmanager.Trig((int)node.Tag);
        }

        private void toolStripLoadImage_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeViewCameraList.SelectedNode;
            if (node == null)
            {
                return;
            }

            taskmanager.LoadImage((int)node.Tag);
        }

        private void toolStripLiveOn_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeViewCameraList.SelectedNode;
            if (node == null)
            {
                return;
            }

            int index = (int)node.Tag;

            taskmanager.Live(index, true);
        }

        private void toolStripLiveOff_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeViewCameraList.SelectedNode;
            if (node == null)
            {
                return;
            }

            int index = (int)node.Tag;

            taskmanager.Live(index, false);
        }

        private void toolStripOnOffLine_Click(object sender, EventArgs e)
        {
            Global.OnOffLineState = !Global.OnOffLineState;
            if (Global.OnOffLineState)
            {
                this.toolStripOnOffLine.Text = "在线";
                this.toolStripOnOffLine.Image = MyResource.camera_online;
            }
            else
            {
                this.toolStripOnOffLine.Text = "离线";
                this.toolStripOnOffLine.Image = MyResource.camera_offline;
            }
        }

        private void 新建项目ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 管理项目ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormManageProject fm = new FormManageProject();
            fm.eventOpenProject += OpenProject;
            fm.ShowDialog();
        }

        private void 通讯设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCommunicationConfig fm = new FormCommunicationConfig();
            fm.ShowDialog();
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            taskmanager.Fresh();
        }

        private void treeViewCameraList_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = true;
            return;
        }

        private void OpenProject()
        {
            int n;
            n = XmlHelper.Read(Global.ApplicationConfigFilePath, "/AppManager/ProjectName", out Global.ProjectName);


            string[] projects = Directory.GetDirectories(Global.BaseProjectFolder);
            List<string> names = new List<string>();
            for (int i = 0; i < projects.Length; i++)
            {
                names.Add(Path.GetFileNameWithoutExtension(projects[i]));
            }

            if (names.Count == 0 || names.IndexOf(Global.ProjectName) == -1)
            {
                MessageBox.Show("无法加载 " + Global.ProjectName, "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Global.ProjectPath = Global.BaseProjectFolder + Global.ProjectName + "\\";
            Global.ProjectConfigFilePath = Global.ProjectPath + "project.xml";
            Global.CameraConfigFilePath = Global.ProjectPath + "camera.xml";
            Global.ToolBlockConfigFilePath = Global.ProjectPath + "toolblock.xml";
            Global.CommunicationConfigFilePath = Global.ProjectPath + "communication.xml";

            n = XmlHelper.Read(Global.CameraConfigFilePath, "/CameraManager/Camera[@name]", out Global.CameraNumber);
            n = XmlHelper.Read(Global.ToolBlockConfigFilePath, "/ToolBlockManager/ToolBlock[@id]", out Global.ToolBlockNumber);
            Global.RecordDisplays = new CogRecordDisplay[Global.ToolBlockNumber];
            Global.CameraNames = new string[Global.CameraNumber];
            Global.ImageSize = new int[Global.CameraNumber, 2];

            DestoryForm();

            InitForm();

            this.Text = Global.ProjectName;

            if (taskmanager != null)
            {
                taskmanager.Stop();
            }

            taskmanager = new TaskManager();
            taskmanager.Init();

            LoadCameraList();

            this.WindowState = FormWindowState.Maximized;
        }

        private void LoadCameraList()
        {
            this.treeViewCameraList.Nodes.Clear();

            for (int i = 0; i < Global.CameraNumber; i++)
            {
                if (Global.FrameGrabbers[i] != null)
                {
                    string info = Global.CameraNames[i] + " " + Global.CameraSerialNumbers[i];
                    TreeNode node = this.treeViewCameraList.Nodes.Add(info);
                    node.SelectedImageIndex = 0;
                    node.Tag = i;
                    node.ImageIndex = 0;
                }
                else
                {
                    string info = Global.CameraNames[i] + " " + "???";

                    TreeNode node = this.treeViewCameraList.Nodes.Add(info);
                    node.SelectedImageIndex = 1;
                    node.Tag = i;
                    node.ImageIndex = 1;
                }
            }
        }

        private void InitForm()
        {
            this.SuspendLayout();

            labels = new Label[Global.ToolBlockNumber];
            recorddisplays = new CogRecordDisplay[Global.ToolBlockNumber];
            Global.RecordDisplays = new CogRecordDisplay[Global.ToolBlockNumber];

            for (int i = 0; i < Global.ToolBlockNumber; i++)
            {
                labels[i] = CreateLabel(i);
                recorddisplays[i] = CreateCogRecordDisplay(i, labels[i], ShowRecordDisplay);
            }

            Global.RecordDisplays = this.recorddisplays;

            this.splitContainer1.Panel1.Controls.Add(CreateTableLayout(Global.ToolBlockNumber, this.recorddisplays));

            for (int i = 0; i < Global.RecordDisplays.Length; i++)
            {
                Global.RecordDisplays[i].HorizontalScrollBar = false;
                Global.RecordDisplays[i].VerticalScrollBar = false;
            }

            AddMenuItem1();

            AddMenuItem2();

            this.ResumeLayout(false);
        }

        private void DestoryForm()
        {
            this.SuspendLayout();

            this.splitContainer1.Panel1.Controls.Clear();
            this.相机设置ToolStripMenuItem.DropDownItems.Clear();
            this.程序编辑ToolStripMenuItem.DropDownItems.Clear();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label CreateLabel(int index)
        {
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();

            label.AutoSize = true;
            label.Location = new System.Drawing.Point(1, 1);
            label.Name = "userdef_label" + index;
            label.BackColor = Color.White;
            label.Size = new System.Drawing.Size(41, 12);
            label.Text = (index + 1).ToString();
            label.TextAlign = ContentAlignment.MiddleCenter;

            return label;
        }

        private Cognex.VisionPro.CogRecordDisplay CreateCogRecordDisplay(int index, System.Windows.Forms.Label label, EventHandler func)
        {
            Cognex.VisionPro.CogRecordDisplay cogRecordDisplay = new Cognex.VisionPro.CogRecordDisplay();
            ((System.ComponentModel.ISupportInitialize)(cogRecordDisplay)).BeginInit();

            cogRecordDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            cogRecordDisplay.Location = new System.Drawing.Point(3, 3);
            cogRecordDisplay.Name = "userdef_cogRecordDisplay" + index;

            cogRecordDisplay.DoubleClick += func;
            cogRecordDisplay.Tag = index;

            cogRecordDisplay.Controls.Add(label);
            ((System.ComponentModel.ISupportInitialize)(cogRecordDisplay)).EndInit();

            return cogRecordDisplay;
        }

        private System.Windows.Forms.TableLayoutPanel CreateTableLayout(int num, CogRecordDisplay[] recorddisplay)
        {
            int row = 1, column = 1;

            if (num == 1)
            {
                row = 1; column = 1;
            }
            else if (num == 2)
            {
                row = 1; column = 2;
            }
            else if (num == 3)
            {
                row = 2; column = 2;
            }
            else if (num == 4)
            {
                row = 2; column = 2;
            }
            else if (num == 5)
            {
                row = 2; column = 3;
            }
            else if (num == 6)
            {
                row = 2; column = 3;
            }
            else if (num == 7)
            {
                row = 3; column = 3;
            }
            else if (num == 8)
            {
                row = 3; column = 3;
            }
            else if (num == 9)
            {
                row = 3;column = 3;
            }

            System.Windows.Forms.TableLayoutPanel tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Dock = DockStyle.Fill;

            tableLayoutPanel1.ColumnCount = column;
            tableLayoutPanel1.RowCount = row;

            for (int i = 0; i < row; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            }
            
            for (int i = 0; i < column; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            }

            if (num == 1)
            {
                tableLayoutPanel1.Controls.Add(recorddisplay[0], 0, 0);
            }
            else if (num == 2)
            {
                tableLayoutPanel1.Controls.Add(recorddisplay[0], 0, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[1], 1, 0);
            }
            else if (num == 3)
            {
                tableLayoutPanel1.Controls.Add(recorddisplay[0], 0, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[1], 1, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[2], 0, 1);
            }
            else if (num == 4)
            {
                tableLayoutPanel1.Controls.Add(recorddisplay[0], 0, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[1], 1, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[2], 0, 1);
                tableLayoutPanel1.Controls.Add(recorddisplay[3], 1, 1);
            }
            else if (num == 5)
            {
                tableLayoutPanel1.Controls.Add(recorddisplay[0], 0, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[1], 1, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[2], 2, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[3], 0, 1);
                tableLayoutPanel1.Controls.Add(recorddisplay[4], 1, 1);
            }
            else if (num == 6)
            {
                tableLayoutPanel1.Controls.Add(recorddisplay[0], 0, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[1], 1, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[2], 2, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[3], 0, 1);
                tableLayoutPanel1.Controls.Add(recorddisplay[4], 1, 1);
                tableLayoutPanel1.Controls.Add(recorddisplay[5], 2, 1);
            }
            else if (num == 7)
            {
                tableLayoutPanel1.Controls.Add(recorddisplay[0], 0, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[1], 1, 0);
                tableLayoutPanel1.Controls.Add(recorddisplays[2], 2, 0);
                tableLayoutPanel1.Controls.Add(recorddisplays[3], 0, 1);
                tableLayoutPanel1.Controls.Add(recorddisplays[4], 1, 1);
                tableLayoutPanel1.Controls.Add(recorddisplays[5], 2, 1);
                tableLayoutPanel1.Controls.Add(recorddisplays[6], 0, 2);
            }
            else if (num == 8)
            {
                tableLayoutPanel1.Controls.Add(recorddisplay[0], 0, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[1], 1, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[2], 2, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[3], 0, 1);
                tableLayoutPanel1.Controls.Add(recorddisplay[4], 1, 1);
                tableLayoutPanel1.Controls.Add(recorddisplay[5], 2, 1);
                tableLayoutPanel1.Controls.Add(recorddisplay[6], 0, 2);
                tableLayoutPanel1.Controls.Add(recorddisplay[7], 1, 2);
            }
            else if (num == 9)
            {
                tableLayoutPanel1.Controls.Add(recorddisplay[0], 0, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[1], 1, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[2], 2, 0);
                tableLayoutPanel1.Controls.Add(recorddisplay[3], 0, 1);
                tableLayoutPanel1.Controls.Add(recorddisplay[4], 1, 1);
                tableLayoutPanel1.Controls.Add(recorddisplay[5], 2, 1);
                tableLayoutPanel1.Controls.Add(recorddisplay[6], 0, 2);
                tableLayoutPanel1.Controls.Add(recorddisplay[7], 1, 2);
                tableLayoutPanel1.Controls.Add(recorddisplay[8], 2, 2);
            }

            return tableLayoutPanel1;
        }

        private ToolStripMenuItem CreateMeauItem(string name, int[] index, string txt, bool enable, int type, System.EventHandler f)
        {
            ToolStripMenuItem menuItem = new System.Windows.Forms.ToolStripMenuItem();

            menuItem.Name = name;
            menuItem.Text = txt;
            menuItem.Tag = index;
            menuItem.Enabled = enable;
            if (f != null)
            {
                menuItem.Click += f;
            }
            if (type == 1) menuItem.Image = Image.FromHbitmap(MyResource.CogAcqFifoTool.ToBitmap().GetHbitmap());
            if (type == 2) menuItem.Image = Image.FromHbitmap(MyResource.CogToolBlock.ToBitmap().GetHbitmap());
            if (type == 3) menuItem.Image = Image.FromHbitmap(MyResource.script.ToBitmap().GetHbitmap());

            return menuItem;
        }

        private void AddMenuItem1()
        {
            for (int i = 0; i < Global.CameraNumber; i++)
            {
                int n;
                string toolblockid;
                n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']", i + 1), "name", out Global.CameraNames[i]);
                n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/toolblockid", i + 1), out toolblockid);

                string[] tools = toolblockid.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                ToolStripMenuItem[] items1 = new ToolStripMenuItem[tools.Length];
                for (int j = 0; j < tools.Length; j++)
                {
                    string vppname;
                    n = XmlHelper.Read(Global.ToolBlockConfigFilePath, string.Format("/ToolBlockManager/ToolBlock[@id='{0}']/vppname", tools[j]), out vppname);

                    items1[j] = CreateMeauItem("userdef_程序编辑MenuItem_vpp" + i + j, new int[] { i, j }, vppname, true, 2, OpenProgramEditForm);
                }

                ToolStripMenuItem[] items2= new ToolStripMenuItem[tools.Length];
                for (int j = 0; j < tools.Length; j++)
                {
                    string scriptname;
                    n = XmlHelper.Read(Global.ToolBlockConfigFilePath, string.Format("/ToolBlockManager/ToolBlock[@id='{0}']/scriptname", tools[j]), out scriptname);

                    items2[j] = CreateMeauItem("userdef_程序编辑MenuItem_script" + i + j, new int[] { i, j + tools.Length }, scriptname, true, 3, OpenProgramEditForm);
                }

                ToolStripMenuItem meunItem = CreateMeauItem("userdef_程序编辑MenuItem" + i, new int[] { }, Global.CameraNames[i], true, 1, null);

                meunItem.DropDownItems.AddRange(items1);
                meunItem.DropDownItems.Add(new ToolStripSeparator());
                meunItem.DropDownItems.AddRange(items2);

                this.程序编辑ToolStripMenuItem.DropDownItems.Add(meunItem);
            }
        }

        private void AddMenuItem2()
        {
            for (int i = 0; i < Global.CameraNumber; i++)
            {
                ToolStripMenuItem meunItem = CreateMeauItem("userdef_相机设置MenuItem" + i, new int[] { i, 0 }, Global.CameraNames[i], true, 1, OpenCameraConfigForm);
                this.相机设置ToolStripMenuItem.DropDownItems.Add(meunItem);
            }
        }

        private void OpenCameraConfigForm(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            int[] index = (int[])menuItem.Tag;

            taskmanager.Config(index[0]);
        }

        private void OpenProgramEditForm(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            int[] index = (int[])menuItem.Tag;
            string name = menuItem.Text;

            taskmanager.Edit(index[0], index[1], name);
        }

        private void ShowRecordDisplay(object sender, EventArgs e)
        {
            CogRecordDisplay recorddisplay = (CogRecordDisplay)sender;

            int index = (int)recorddisplay.Tag;

            string toolid;
            XmlHelper.Read(Global.ToolBlockConfigFilePath, string.Format("/ToolBlockManager/ToolBlock[@display='{0}']", index+1), "id", out toolid);

            bool b = false;
            int cindex = 0;
            for (int i = 0; i < Global.CameraNumber; i++)
            {
                cindex = i;

                string toolids;
                XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/toolblockid", i + 1), out toolids);

                string[] id = toolids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < id.Length; j++)
                {
                    if (toolid == id[j])
                    {
                        b = true;
                        break;
                    }
                }

                if (b) break;
            }

            if (cindex >= Global.CameraNumber || b == false)
            {
                return;
            }

            FormRecordDisplay fm = new FormRecordDisplay(recorddisplay, cindex);
            fm.ShowDialog();
        }

        private void UpdateRichTextBox(RichTextBox richtxt, string message)
        {
            if (richtxt.InvokeRequired)
            {
                richtxt.BeginInvoke(new Action<RichTextBox, string>(UpdateRichTextBox), new object[] { richtxt, message });
            }
            else
            {
                if (richtxt.TextLength > 20000)
                {
                    richtxt.Clear();
                }

                string s = DateTime.Now.ToString("HH:mm:ss.fff") + " -> " + message + "\r\n";

                richtxt.Focus();
                richtxt.Select(richtxt.TextLength, 0);
                richtxt.ScrollToCaret();
                richtxt.AppendText(s);
            }
        }
    }
}
