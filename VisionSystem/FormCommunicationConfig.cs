using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionSystem
{
    public partial class FormCommunicationConfig : Form
    {
        public FormCommunicationConfig()
        {
            InitializeComponent();
        }

        private void FormCommunicationConfig_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.TabControl.TabPageCollection ps = this.tabCommunicationInterface.TabPages;
            if (ps.Count == 0)
            {
                return;
            }

            int i = 0;
            foreach (CommunicationType item in Enum.GetValues(typeof(CommunicationType)))
            {
                string name = Enum.GetName(typeof(CommunicationType), item);
                ps[i].Text = name;
                i++;
                if (i == ps.Count)
                {
                    break;
                }
            }

            System.Windows.Forms.TabControl.TabPageCollection ps2 = this.tabPLC.TabPages;
            if (ps2.Count == 0)
            {
                return;
            }

            int j = 0;
            foreach (PLCType item in Enum.GetValues(typeof(PLCType)))
            {
                string name = Enum.GetName(typeof(PLCType), item);
                ps2[j].Text = name;
                j++;
            }

            this.cboSerialPortNumber.Items.AddRange(SerialPortHelper.GetSerialPort());
            this.cboSerialPortBaudrate.Items.AddRange(new object[] { 9600, 38400, 115200 });
            this.cboSerialPortDatabits.Items.AddRange(new object[] { 7, 8 });
            this.cboSerialPortStopbits.Items.AddRange(new object[] { 0, 1, 1.5, 2 });

            this.cboPLC_S_CpuType.Items.AddRange(new object[] { "S7200", "S7300", "S7400", "S71200", "S71500" });

            ReadFromFile();
        }

        private void AddItem()
        {
            CommunicationType type1 = (CommunicationType)Enum.Parse(typeof(CommunicationType), this.tabCommunicationInterface.SelectedTab.Text);

            List<string> list = new List<string>();

            if (type1 == CommunicationType.串口)
            {
                if (this.cboSerialPortNumber.SelectedIndex == -1 || this.cboSerialPortBaudrate.SelectedIndex == -1 || this.cboSerialPortDatabits.SelectedIndex == -1 || this.cboSerialPortStopbits.SelectedIndex == -1)
                {
                    return;
                }
                list.Add(type1.ToString() + "-" + DateTime.Now.ToString("yyMMddHHmmssf"));
                list.Add(type1.ToString());
                list.Add(this.cboSerialPortNumber.SelectedItem.ToString());
                list.Add(this.cboSerialPortBaudrate.SelectedItem.ToString());
                list.Add(this.cboSerialPortDatabits.SelectedItem.ToString());
                list.Add(this.cboSerialPortStopbits.SelectedItem.ToString());
            }
            else if (type1 == CommunicationType.TCP服务器)
            {
                if (string.IsNullOrEmpty(this.txtTcpServerIP.Text.Trim()) || string.IsNullOrEmpty(this.txtTcpServerPort.Text.Trim()))
                {
                    return;
                }
                list.Add(type1.ToString() + "-" + DateTime.Now.ToString("yyMMddHHmmssf"));
                list.Add(type1.ToString());
                list.Add(this.txtTcpServerIP.Text.Trim());
                list.Add(this.txtTcpServerPort.Text.Trim());
            }
            else if (type1 == CommunicationType.PLC)
            {
                PLCType type2 = (PLCType)Enum.Parse(typeof(PLCType), this.tabPLC.SelectedTab.Text);
                if (type2 == PLCType.Siemens)
                {
                    if (this.cboPLC_S_CpuType.SelectedIndex == -1 || string.IsNullOrEmpty(this.txtPLC_S_IP.Text.Trim()) || string.IsNullOrEmpty(this.txtPLC_S_Rack.Text.Trim()) || string.IsNullOrEmpty(this.txtPLC_S_Slot.Text.Trim()) || string.IsNullOrEmpty(this.txtReadblock.Text.Trim()) || string.IsNullOrEmpty(this.txtReadstart.Text.Trim()) || string.IsNullOrEmpty(this.txtReadlength.Text.Trim()) || string.IsNullOrEmpty(this.txtWriteblock.Text.Trim()) || string.IsNullOrEmpty(this.txtWritestart.Text.Trim()))
                    {
                        return;
                    }
                    list.Add(type2.ToString() + "-" + DateTime.Now.ToString("yyMMddHHmmssf"));
                    list.Add(type1.ToString());
                    list.Add(this.cboPLC_S_CpuType.SelectedItem.ToString());
                    list.Add(this.txtPLC_S_IP.Text.Trim());
                    list.Add(this.txtPLC_S_Rack.Text.Trim());
                    list.Add(this.txtPLC_S_Slot.Text.Trim());

                    list.Add(this.txtReadblock.Text.Trim());
                    list.Add(this.txtReadstart.Text.Trim());
                    list.Add(this.txtReadlength.Text.Trim());
                    list.Add(this.txtWriteblock.Text.Trim());
                    list.Add(this.txtWritestart.Text.Trim());
                }
                else if (type2 == PLCType.Mitsubishi)
                {
                    if (string.IsNullOrEmpty(this.txtPLC_M_StationNumber.Text.Trim()) || string.IsNullOrEmpty(this.txtReadblock.Text.Trim()) || string.IsNullOrEmpty(this.txtReadstart.Text.Trim()) || string.IsNullOrEmpty(this.txtReadlength.Text.Trim()) || string.IsNullOrEmpty(this.txtWriteblock.Text.Trim()) || string.IsNullOrEmpty(this.txtWritestart.Text.Trim()))
                    {
                        return;
                    }
                    list.Add(type2.ToString() + "-" + DateTime.Now.ToString("yyMMddHHmmssf"));
                    list.Add(type1.ToString());
                    list.Add(this.txtPLC_M_StationNumber.Text.Trim());

                    list.Add(this.txtReadblock.Text.Trim());
                    list.Add(this.txtReadstart.Text.Trim());
                    list.Add(this.txtReadlength.Text.Trim());
                    list.Add(this.txtWriteblock.Text.Trim());
                    list.Add(this.txtWritestart.Text.Trim());
                }
                else if (type2 == PLCType.Omron)
                {
                    if (string.IsNullOrEmpty(this.txtPLC_O_IP.Text.Trim()) || string.IsNullOrEmpty(this.txtPLC_O_Port.Text.Trim()) || string.IsNullOrEmpty(this.txtPLC_O_LocalIP.Text.Trim()) || string.IsNullOrEmpty(this.txtReadblock.Text.Trim()) || string.IsNullOrEmpty(this.txtReadstart.Text.Trim()) || string.IsNullOrEmpty(this.txtReadlength.Text.Trim()) || string.IsNullOrEmpty(this.txtWriteblock.Text.Trim()) || string.IsNullOrEmpty(this.txtWritestart.Text.Trim()))
                    {
                        return;
                    }
                    list.Add(type2.ToString() + "-" + DateTime.Now.ToString("yyMMddHHmmssf"));
                    list.Add(type1.ToString());
                    list.Add(this.txtPLC_O_IP.Text.Trim());
                    list.Add(this.txtPLC_O_Port.Text.Trim());
                    list.Add(this.txtPLC_O_LocalIP.Text.Trim());

                    list.Add(this.txtReadblock.Text.Trim());
                    list.Add(this.txtReadstart.Text.Trim());
                    list.Add(this.txtReadlength.Text.Trim());
                    list.Add(this.txtWriteblock.Text.Trim());
                    list.Add(this.txtWritestart.Text.Trim());
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

            ListViewItem item = new ListViewItem(list.ToArray());
            this.listViewCommunication.Items.Add(item);
        }

        private void DeleteItem()
        {
            if (this.listViewCommunication.SelectedItems.Count <= 0)
            {
                return;
            }

            int index = this.listViewCommunication.SelectedItems[0].Index;

            this.listViewCommunication.Items[index].Remove();
        }

        private void WriteToFile()
        {
            //将ListView中的内容写入到文件中

            if (File.Exists(Global.CommunicationConfigFilePath))
            {
                File.Delete(Global.CommunicationConfigFilePath);
            }

            XmlHelper.FileCreate(Global.CommunicationConfigFilePath, "CommunicationManager");

            int count = this.listViewCommunication.Items.Count;
            if (count <= 0)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                System.Windows.Forms.ListViewItem item = this.listViewCommunication.Items[i];

                //获取当前行 代表的类型
                System.Windows.Forms.ListViewItem.ListViewSubItem sub = item.SubItems[1];

                CommunicationType type1 = (CommunicationType)Enum.Parse(typeof(CommunicationType), sub.Text);

                Dictionary<string, string> dic = new Dictionary<string, string>();

                switch (type1)
                {
                    case CommunicationType.串口:
                        //节点
                        dic["id"] = item.SubItems[0].Text;
                        dic["type"] = item.SubItems[1].Text;
                        XmlHelper.AddNode(Global.CommunicationConfigFilePath, "/CommunicationManager", "Communication", "", dic);
                        //节点的子节点
                        XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "portname", item.SubItems[2].Text, null);
                        XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "baudrate", item.SubItems[3].Text, null);
                        XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "databits", item.SubItems[4].Text, null);
                        XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "stopbits", item.SubItems[5].Text, null);
                        break;
                    case CommunicationType.TCP服务器:
                        //节点
                        dic["id"] = item.SubItems[0].Text;
                        dic["type"] = item.SubItems[1].Text;
                        XmlHelper.AddNode(Global.CommunicationConfigFilePath, "/CommunicationManager", "Communication", "", dic);
                        //节点的子节点
                        XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "ip", item.SubItems[2].Text, null);
                        XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "port", item.SubItems[3].Text, null);
                        break;
                    case CommunicationType.PLC:

                        int index = item.SubItems[0].Text.IndexOf('-');
                        PLCType type2 = (PLCType)Enum.Parse(typeof(PLCType), item.SubItems[0].Text.Substring(0, index));

                        if (type2 == PLCType.Siemens)
                        {
                            //节点
                            dic["id"] = item.SubItems[0].Text;
                            dic["type"] = item.SubItems[1].Text;
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, "/CommunicationManager", "Communication", "", dic);
                            //节点的子节点
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "cputype", item.SubItems[2].Text, null);
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "ip", item.SubItems[3].Text, null);
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "rack", item.SubItems[4].Text, null);
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "slot", item.SubItems[5].Text, null);

                            Dictionary<string, string> read1 = new Dictionary<string, string>();
                            read1.Add("block", item.SubItems[6].Text);
                            read1.Add("start", item.SubItems[7].Text);
                            read1.Add("length", item.SubItems[8].Text);
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "Read", "", read1);

                            Dictionary<string, string> write1 = new Dictionary<string, string>();
                            write1.Add("block", item.SubItems[9].Text);
                            write1.Add("start", item.SubItems[10].Text);
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "Write", "", write1);
                        }
                        else if (type2 == PLCType.Mitsubishi)
                        {
                            //节点
                            dic["id"] = item.SubItems[0].Text;
                            dic["type"] = item.SubItems[1].Text;
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, "/CommunicationManager", "Communication", "", dic);
                            //节点的子节点
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "StationNumber", item.SubItems[2].Text, null);

                            Dictionary<string, string> read2 = new Dictionary<string, string>();
                            read2.Add("block", item.SubItems[3].Text);
                            read2.Add("start", item.SubItems[4].Text);
                            read2.Add("length", item.SubItems[5].Text);
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "Read", "", read2);

                            Dictionary<string, string> write2 = new Dictionary<string, string>();
                            write2.Add("block", item.SubItems[6].Text);
                            write2.Add("start", item.SubItems[7].Text);
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "Write", "", write2);
                        }
                        else if (type2 == PLCType.Omron)
                        {
                            //节点
                            dic["id"] = item.SubItems[0].Text;
                            dic["type"] = item.SubItems[1].Text;
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, "/CommunicationManager", "Communication", "", dic);
                            //节点的子节点
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "ip", item.SubItems[2].Text, null);
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "port", item.SubItems[3].Text, null);
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "localip", item.SubItems[4].Text, null);

                            Dictionary<string, string> read2 = new Dictionary<string, string>();
                            read2.Add("block", item.SubItems[5].Text);
                            read2.Add("start", item.SubItems[6].Text);
                            read2.Add("length", item.SubItems[7].Text);
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "Read", "", read2);

                            Dictionary<string, string> write2 = new Dictionary<string, string>();
                            write2.Add("block", item.SubItems[8].Text);
                            write2.Add("start", item.SubItems[9].Text);
                            XmlHelper.AddNode(Global.CommunicationConfigFilePath, string.Format("/CommunicationManager/Communication[@id='{0}']", dic["id"]), "Write", "", write2);
                        }

                        break;
                    case CommunicationType.None:
                        break;
                    default:
                        break;
                }
            }
        }

        private void ReadFromFile()
        {
            if (!File.Exists(Global.CommunicationConfigFilePath))
            {
                return;
            }

            List<List<string>> nodes;
            int n = XmlHelper.Read(Global.CommunicationConfigFilePath, "/CommunicationManager/Communication[@id]", out nodes);

            if (nodes == null)
            {
                return;
            }

            this.listViewCommunication.Items.Clear();

            for (int i = 0; i < nodes.Count; i++)
            {
                List<string> list = nodes[i];
                ListViewItem item = new ListViewItem(list.ToArray());
                this.listViewCommunication.Items.Add(item);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddItem();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            try
            {
                WriteToFile();

                ReadFromFile();

                MessageBox.Show("保存成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
