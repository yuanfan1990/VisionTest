namespace VisionSystem
{
    partial class FormRecordDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecordDisplay));
            this.cogRecordDisplay1 = new Cognex.VisionPro.CogRecordDisplay();
            this.cogDisplayStatusBarV21 = new Cognex.VisionPro.CogDisplayStatusBarV2();
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).BeginInit();
            this.SuspendLayout();
            // 
            // cogRecordDisplay1
            // 
            this.cogRecordDisplay1.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogRecordDisplay1.ColorMapLowerRoiLimit = 0D;
            this.cogRecordDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogRecordDisplay1.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogRecordDisplay1.ColorMapUpperRoiLimit = 1D;
            this.cogRecordDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogRecordDisplay1.DoubleTapZoomCycleLength = 2;
            this.cogRecordDisplay1.DoubleTapZoomSensitivity = 2.5D;
            this.cogRecordDisplay1.Location = new System.Drawing.Point(0, 0);
            this.cogRecordDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogRecordDisplay1.MouseWheelSensitivity = 1D;
            this.cogRecordDisplay1.Name = "cogRecordDisplay1";
            this.cogRecordDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogRecordDisplay1.OcxState")));
            this.cogRecordDisplay1.Size = new System.Drawing.Size(933, 525);
            this.cogRecordDisplay1.TabIndex = 0;
            this.cogRecordDisplay1.DoubleClick += new System.EventHandler(this.cogRecordDisplay1_DoubleClick);
            // 
            // cogDisplayStatusBarV21
            // 
            this.cogDisplayStatusBarV21.CoordinateSpaceName = "*\\#";
            this.cogDisplayStatusBarV21.CoordinateSpaceName3D = "*\\#";
            this.cogDisplayStatusBarV21.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cogDisplayStatusBarV21.Location = new System.Drawing.Point(0, 499);
            this.cogDisplayStatusBarV21.Name = "cogDisplayStatusBarV21";
            this.cogDisplayStatusBarV21.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cogDisplayStatusBarV21.Size = new System.Drawing.Size(933, 26);
            this.cogDisplayStatusBarV21.TabIndex = 1;
            this.cogDisplayStatusBarV21.Use3DCoordinateSpaceTree = false;
            // 
            // FormRecordDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 525);
            this.Controls.Add(this.cogDisplayStatusBarV21);
            this.Controls.Add(this.cogRecordDisplay1);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormRecordDisplay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormRecordDisplay";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormRecordDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.CogRecordDisplay cogRecordDisplay1;
        private Cognex.VisionPro.CogDisplayStatusBarV2 cogDisplayStatusBarV21;
    }
}