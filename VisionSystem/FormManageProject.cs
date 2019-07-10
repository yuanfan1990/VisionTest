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

namespace VisionSystem
{
    public partial class FormManageProject : Form
    {
        //字段
        private string[] projects;
        private string selectprojectname;

        //事件
        public event Action eventOpenProject;

        //构造
        public FormManageProject()
        {
            InitializeComponent();
        }

        private void FormManageProject_Load(object sender, EventArgs e)
        {
            LoadProjectList();
        }

        private void LoadProjectList()
        {
            this.listViewProject.Items.Clear();
            this.listViewProject.Columns.Clear();
            this.listViewProject.Columns.Add("项目名称", 200, HorizontalAlignment.Left);
            //this.listViewProject.Columns.Add("创建时间", 350, HorizontalAlignment.Left);

            this.btnOpenProject.Enabled = false;
            this.btnDeleteProject.Enabled = false;

            this.projects = Directory.GetDirectories(Global.BaseProjectFolder);

            if (projects == null || projects.Length == 0)
            {
                MessageBox.Show("未发现任何项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }

            for (int i = 0; i < projects.Length; i++)
            {
                ListViewItem listitem = new ListViewItem();
                listitem.Text = Path.GetFileNameWithoutExtension(this.projects[i]);
                //listitem.SubItems.Add(new DirectoryInfo(this.projects[i]).CreationTime.ToString());
                this.listViewProject.Items.Add(listitem);
            }
        }

        private void listViewProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.listViewProject.FocusedItem.Index;
            if (index == -1 || this.listViewProject.SelectedIndices.Count < 1)
            {
                this.btnDeleteProject.Enabled = false;
                this.btnOpenProject.Enabled = false;
                return;
            }

            this.selectprojectname = this.listViewProject.SelectedItems[0].SubItems[0].Text;

            this.btnOpenProject.Enabled = true;
            this.btnDeleteProject.Enabled = this.selectprojectname != Global.ProjectName ? true : false;
        }

        private void btnOpenProject_Click(object sender, EventArgs e)
        {
            this.Hide();

            int n;
            n = XmlHelper.Write(Global.ApplicationConfigFilePath, "/AppManager/ProjectName", this.selectprojectname);

            OnOpenProject();

            this.Close();
        }

        private void btnDeleteProject_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(string.Format("确定删除 {0}?", this.selectprojectname), "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            Directory.Delete(Global.BaseProjectFolder + this.selectprojectname, true);        

            MessageBox.Show(string.Format("{0} 删除完成！", selectprojectname), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadProjectList();
        }

        private void OnOpenProject()
        {
            if (eventOpenProject != null)
            {
                eventOpenProject();
            }
        }
    }
}
