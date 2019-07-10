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
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;

namespace VisionSystem
{
    public partial class FormEdit : Form
    {
        private string path;

        public FormEdit(ToolBlockHelper tool)
        {
            InitializeComponent();

            this.path = tool.VppPath;
            this.Text = Path.GetFileName(this.path);
            this.cogToolBlockEditV21.Subject = tool.Toolblock;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.cogToolBlockEditV21.Subject = null;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(string.Format("是否保存 {0} ？", this.Text), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                CogSerializer.SaveObjectToFile(this.cogToolBlockEditV21.Subject, path);

                MessageBox.Show(string.Format("保存完成 {0}", this.Text), "提示");
            }
        }
    }
}
