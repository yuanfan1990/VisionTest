using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Cognex.VisionPro;

namespace VisionSystem
{
    public partial class FormRecordDisplay : Form
    {
        private System.Windows.Forms.Timer timer = new Timer();
        private int index;
        private CogRecordDisplay recorddisplay;

        public FormRecordDisplay(CogRecordDisplay display, int index)
        {
            InitializeComponent();

            this.recorddisplay = display;
            this.index = index;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.recorddisplay.LiveDisplayRunning)
            {
                CreateLine(cogRecordDisplay1, Global.ImageSize[this.index, 0], Global.ImageSize[this.index, 1]);
            }
            else
            {
                cogRecordDisplay1.Image = this.recorddisplay.Image;
                cogRecordDisplay1.Record = this.recorddisplay.Record;

                if (cogRecordDisplay1.Image == null)
                {
                    return;
                }

                CreateLine(cogRecordDisplay1, cogRecordDisplay1.Image.Width, cogRecordDisplay1.Image.Height);
            }

        }

        private void cogRecordDisplay1_DoubleClick(object sender, EventArgs e)
        {
            timer.Stop();
            this.Close();
        }

        private void CreateLine(CogRecordDisplay record, int imagewidth, int imageheight)
        {
            record.StaticGraphics.Clear();
            CogLine lineX = new CogLine();
            CogLine lineY = new CogLine();

            lineX.SetXYRotation(imagewidth / 2, imageheight / 2, 0);
            lineX.Interactive = false;
            lineX.Selected = false;
            lineX.Color = CogColorConstants.LightGrey;
            lineX.LineStyle = CogGraphicLineStyleConstants.Solid;
            lineX.SelectedSpaceName = "#";

            lineY.SetXYRotation(imagewidth / 2, imageheight / 2, Math.PI / 2);
            lineY.Interactive = false;
            lineY.Selected = false;
            lineY.Color = CogColorConstants.LightGrey;
            lineY.LineStyle = CogGraphicLineStyleConstants.Solid;
            lineY.SelectedSpaceName = "#";

            record.StaticGraphics.Add(lineX, "");
            record.StaticGraphics.Add(lineY, "");
        }

        private void FormRecordDisplay_Load(object sender, EventArgs e)
        {
            if (this.recorddisplay.LiveDisplayRunning)
            {
                cogRecordDisplay1.StartLiveDisplay(Global.AcqFifos[this.index]);
            }
            else
            {
                cogRecordDisplay1.Image = this.recorddisplay.Image;
                cogRecordDisplay1.Record = this.recorddisplay.Record;
            }

            cogDisplayStatusBarV21.Display = cogRecordDisplay1;
            cogRecordDisplay1.Fit();


            timer.Interval = 300;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
    }
}
