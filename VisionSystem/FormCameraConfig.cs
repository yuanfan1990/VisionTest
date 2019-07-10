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
    public partial class FormCameraConfig : Form
    {
        private int index;
        private string name, exposure, contrast, light;
        private string serialnumber;


        public FormCameraConfig(int index)
        {
            InitializeComponent();

            this.index = index;
            this.cogAcqFifoEditV21.Subject.Operator = Global.AcqFifos[this.index];
        }

        private void FormCameraConfig_Load(object sender, EventArgs e)
        {
            LoadConfig();

            WriteAcqFifoParam();
        }

        private void LoadConfig()
        { 
            int n;
            n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']", this.index + 1), "name", out this.name);
            n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/param", this.index + 1), "exposure", out this.exposure);
            n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/param", this.index + 1), "contrast", out this.contrast);
            n = XmlHelper.Read(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/param", this.index + 1), "light", out this.light);

            this.Text = this.name;
        }

        private void SaveConfig()
        {
            int n;
            n = XmlHelper.Write(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/param", this.index + 1), "exposure", this.exposure);
            n = XmlHelper.Write(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/param", this.index + 1), "contrast", this.contrast);
            n = XmlHelper.Write(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/param", this.index + 1), "light", this.light);
            n = XmlHelper.Write(Global.CameraConfigFilePath, string.Format("/CameraManager/Camera[@id='{0}']/param", this.index + 1), "sn", this.serialnumber);
        }

        private void ReadAcqFifoParam()
        {
            if (this.cogAcqFifoEditV21.Subject.Operator == null)
            {
                return;
            }

            this.serialnumber = this.cogAcqFifoEditV21.Subject.Operator.FrameGrabber.SerialNumber;

            ICogAcqExposure exposureParams = this.cogAcqFifoEditV21.Subject.Operator.OwnedExposureParams;
            if (exposureParams != null)
            {
                this.exposure = this.cogAcqFifoEditV21.Subject.Operator.OwnedExposureParams.Exposure.ToString();
            }

            ICogAcqContrast contrastParams = this.cogAcqFifoEditV21.Subject.Operator.OwnedContrastParams;
            if (contrastParams != null)
            {
                this.contrast = this.cogAcqFifoEditV21.Subject.Operator.OwnedContrastParams.Contrast.ToString();
            }

            ICogAcqLight lightParams = this.cogAcqFifoEditV21.Subject.Operator.OwnedLightParams;
            if (lightParams != null)
            {
                this.light = this.cogAcqFifoEditV21.Subject.Operator.OwnedLightParams.LightPower.ToString();
            }
        }

        private void WriteAcqFifoParam()
        {
            if (this.cogAcqFifoEditV21.Subject.Operator == null)
            {
                return;
            }

            ICogAcqExposure exposureParams = this.cogAcqFifoEditV21.Subject.Operator.OwnedExposureParams;
            if (exposureParams != null)
            {
                this.cogAcqFifoEditV21.Subject.Operator.OwnedExposureParams.Exposure = Convert.ToDouble(this.exposure);
                this.cogAcqFifoEditV21.Subject.Operator.Prepare();
            }

            ICogAcqContrast contrastParams = this.cogAcqFifoEditV21.Subject.Operator.OwnedContrastParams;
            if (contrastParams != null)
            {
                this.cogAcqFifoEditV21.Subject.Operator.OwnedContrastParams.Contrast = Convert.ToDouble(this.contrast);
                this.cogAcqFifoEditV21.Subject.Operator.Prepare();
            }

            ICogAcqLight lightParams = this.cogAcqFifoEditV21.Subject.Operator.OwnedLightParams;
            if (lightParams != null)
            {
                this.cogAcqFifoEditV21.Subject.Operator.OwnedLightParams.LightPower = Convert.ToDouble(this.light);
                this.cogAcqFifoEditV21.Subject.Operator.Prepare();
            }
        }

        private void FormCameraConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("是否保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ReadAcqFifoParam();

                SaveConfig();

                MessageBox.Show("保存完成", "提示");
            }
        }
    }
}
