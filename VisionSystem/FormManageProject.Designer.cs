namespace VisionSystem
{
    partial class FormManageProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManageProject));
            this.btnOpenProject = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDeleteProject = new System.Windows.Forms.Button();
            this.listViewProject = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // btnOpenProject
            // 
            this.btnOpenProject.Location = new System.Drawing.Point(580, 395);
            this.btnOpenProject.Name = "btnOpenProject";
            this.btnOpenProject.Size = new System.Drawing.Size(113, 34);
            this.btnOpenProject.TabIndex = 8;
            this.btnOpenProject.Text = "打开";
            this.btnOpenProject.UseVisualStyleBackColor = true;
            this.btnOpenProject.Click += new System.EventHandler(this.btnOpenProject_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "列表";
            // 
            // btnDeleteProject
            // 
            this.btnDeleteProject.Location = new System.Drawing.Point(580, 469);
            this.btnDeleteProject.Name = "btnDeleteProject";
            this.btnDeleteProject.Size = new System.Drawing.Size(113, 34);
            this.btnDeleteProject.TabIndex = 8;
            this.btnDeleteProject.Text = "删除";
            this.btnDeleteProject.UseVisualStyleBackColor = true;
            this.btnDeleteProject.Click += new System.EventHandler(this.btnDeleteProject_Click);
            // 
            // listViewProject
            // 
            this.listViewProject.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewProject.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewProject.FullRowSelect = true;
            this.listViewProject.Location = new System.Drawing.Point(22, 54);
            this.listViewProject.Name = "listViewProject";
            this.listViewProject.Size = new System.Drawing.Size(523, 449);
            this.listViewProject.TabIndex = 10;
            this.listViewProject.UseCompatibleStateImageBehavior = false;
            this.listViewProject.View = System.Windows.Forms.View.Details;
            this.listViewProject.SelectedIndexChanged += new System.EventHandler(this.listViewProject_SelectedIndexChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1.bmp");
            this.imageList1.Images.SetKeyName(1, "2.bmp");
            this.imageList1.Images.SetKeyName(2, "3.bmp");
            this.imageList1.Images.SetKeyName(3, "4.bmp");
            this.imageList1.Images.SetKeyName(4, "5.bmp");
            this.imageList1.Images.SetKeyName(5, "6.bmp");
            this.imageList1.Images.SetKeyName(6, "7.bmp");
            this.imageList1.Images.SetKeyName(7, "8.bmp");
            this.imageList1.Images.SetKeyName(8, "9.bmp");
            // 
            // FormManageProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 536);
            this.Controls.Add(this.listViewProject);
            this.Controls.Add(this.btnDeleteProject);
            this.Controls.Add(this.btnOpenProject);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Name = "FormManageProject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "管理";
            this.Load += new System.EventHandler(this.FormManageProject_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOpenProject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDeleteProject;
        private System.Windows.Forms.ListView listViewProject;
        private System.Windows.Forms.ImageList imageList1;
    }
}