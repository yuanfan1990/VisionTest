namespace VisionSystem
{
    partial class FormCameraConfig
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cogAcqFifoEditV21 = new Cognex.VisionPro.CogAcqFifoEditV2();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogAcqFifoEditV21)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1042, 693);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cogAcqFifoEditV21);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1034, 667);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "采集";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cogAcqFifoEditV21
            // 
            this.cogAcqFifoEditV21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogAcqFifoEditV21.Location = new System.Drawing.Point(3, 3);
            this.cogAcqFifoEditV21.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogAcqFifoEditV21.Name = "cogAcqFifoEditV21";
            this.cogAcqFifoEditV21.Size = new System.Drawing.Size(1028, 661);
            this.cogAcqFifoEditV21.SuspendElectricRuns = false;
            this.cogAcqFifoEditV21.TabIndex = 0;
            // 
            // FormCameraConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 693);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "FormCameraConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "相机设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCameraConfig_FormClosing);
            this.Load += new System.EventHandler(this.FormCameraConfig_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogAcqFifoEditV21)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Cognex.VisionPro.CogAcqFifoEditV2 cogAcqFifoEditV21;
    }
}