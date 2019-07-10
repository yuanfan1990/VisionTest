namespace VisionSystem
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.管理项目ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.相机设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系统设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.通讯设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.程序编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.用户ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripAutorun = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripOnOffLine = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLiveOn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLiveOff = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLoadImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripRunOnce = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripManual = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeViewCameraList = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.richTextMessage = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.设置ToolStripMenuItem,
            this.程序编辑ToolStripMenuItem,
            this.用户ToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1008, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.管理项目ToolStripMenuItem,
            this.刷新ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 管理项目ToolStripMenuItem
            // 
            this.管理项目ToolStripMenuItem.Name = "管理项目ToolStripMenuItem";
            this.管理项目ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.管理项目ToolStripMenuItem.Text = "管理";
            this.管理项目ToolStripMenuItem.Click += new System.EventHandler(this.管理项目ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.相机设置ToolStripMenuItem,
            this.系统设置ToolStripMenuItem,
            this.通讯设置ToolStripMenuItem});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // 相机设置ToolStripMenuItem
            // 
            this.相机设置ToolStripMenuItem.Name = "相机设置ToolStripMenuItem";
            this.相机设置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.相机设置ToolStripMenuItem.Text = "相机设置";
            // 
            // 系统设置ToolStripMenuItem
            // 
            this.系统设置ToolStripMenuItem.Name = "系统设置ToolStripMenuItem";
            this.系统设置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.系统设置ToolStripMenuItem.Text = "系统设置";
            // 
            // 通讯设置ToolStripMenuItem
            // 
            this.通讯设置ToolStripMenuItem.Name = "通讯设置ToolStripMenuItem";
            this.通讯设置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.通讯设置ToolStripMenuItem.Text = "通讯设置";
            this.通讯设置ToolStripMenuItem.Click += new System.EventHandler(this.通讯设置ToolStripMenuItem_Click);
            // 
            // 程序编辑ToolStripMenuItem
            // 
            this.程序编辑ToolStripMenuItem.Name = "程序编辑ToolStripMenuItem";
            this.程序编辑ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.程序编辑ToolStripMenuItem.Text = "程序编辑";
            // 
            // 用户ToolStripMenuItem
            // 
            this.用户ToolStripMenuItem.Name = "用户ToolStripMenuItem";
            this.用户ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.用户ToolStripMenuItem.Text = "用户";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.关于ToolStripMenuItem.Text = "关于";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAutorun,
            this.toolStripSeparator2,
            this.toolStripStop,
            this.toolStripSeparator3,
            this.toolStripOnOffLine,
            this.toolStripSeparator5,
            this.toolStripLiveOn,
            this.toolStripSeparator7,
            this.toolStripLiveOff,
            this.toolStripSeparator8,
            this.toolStripLoadImage,
            this.toolStripSeparator6,
            this.toolStripRunOnce,
            this.toolStripSeparator9,
            this.toolStripManual,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1008, 55);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripAutorun
            // 
            this.toolStripAutorun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAutorun.Image = global::VisionSystem.MyResource.waitstart;
            this.toolStripAutorun.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripAutorun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAutorun.Name = "toolStripAutorun";
            this.toolStripAutorun.Size = new System.Drawing.Size(52, 52);
            this.toolStripAutorun.Text = "自动运行";
            this.toolStripAutorun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripAutorun.Click += new System.EventHandler(this.toolStripAutorun_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 55);
            // 
            // toolStripStop
            // 
            this.toolStripStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStop.Image = global::VisionSystem.MyResource.stop;
            this.toolStripStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStop.Name = "toolStripStop";
            this.toolStripStop.Size = new System.Drawing.Size(52, 52);
            this.toolStripStop.Text = "停止";
            this.toolStripStop.Click += new System.EventHandler(this.toolStripStop_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 55);
            // 
            // toolStripOnOffLine
            // 
            this.toolStripOnOffLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOnOffLine.Image = global::VisionSystem.MyResource.camera_offline;
            this.toolStripOnOffLine.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripOnOffLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOnOffLine.Name = "toolStripOnOffLine";
            this.toolStripOnOffLine.Size = new System.Drawing.Size(52, 52);
            this.toolStripOnOffLine.Text = "离线";
            this.toolStripOnOffLine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripOnOffLine.Click += new System.EventHandler(this.toolStripOnOffLine_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 55);
            // 
            // toolStripLiveOn
            // 
            this.toolStripLiveOn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLiveOn.Image = global::VisionSystem.MyResource.camera_liveon;
            this.toolStripLiveOn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripLiveOn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripLiveOn.Name = "toolStripLiveOn";
            this.toolStripLiveOn.Size = new System.Drawing.Size(52, 52);
            this.toolStripLiveOn.Text = "实时开";
            this.toolStripLiveOn.Click += new System.EventHandler(this.toolStripLiveOn_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 55);
            // 
            // toolStripLiveOff
            // 
            this.toolStripLiveOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLiveOff.Image = global::VisionSystem.MyResource.camera_liveoff;
            this.toolStripLiveOff.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripLiveOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripLiveOff.Name = "toolStripLiveOff";
            this.toolStripLiveOff.Size = new System.Drawing.Size(52, 52);
            this.toolStripLiveOff.Text = "实时关";
            this.toolStripLiveOff.Click += new System.EventHandler(this.toolStripLiveOff_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 55);
            // 
            // toolStripLoadImage
            // 
            this.toolStripLoadImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLoadImage.Image = global::VisionSystem.MyResource.loadimage;
            this.toolStripLoadImage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripLoadImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripLoadImage.Name = "toolStripLoadImage";
            this.toolStripLoadImage.Size = new System.Drawing.Size(52, 52);
            this.toolStripLoadImage.Text = "加载图像";
            this.toolStripLoadImage.Click += new System.EventHandler(this.toolStripLoadImage_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 55);
            // 
            // toolStripRunOnce
            // 
            this.toolStripRunOnce.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripRunOnce.Image = global::VisionSystem.MyResource.run;
            this.toolStripRunOnce.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripRunOnce.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripRunOnce.Name = "toolStripRunOnce";
            this.toolStripRunOnce.Size = new System.Drawing.Size(55, 52);
            this.toolStripRunOnce.Text = "单次运行";
            this.toolStripRunOnce.Click += new System.EventHandler(this.toolStripRunOnce_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 55);
            // 
            // toolStripManual
            // 
            this.toolStripManual.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripManual.Image = global::VisionSystem.MyResource.折叠右;
            this.toolStripManual.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripManual.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripManual.Name = "toolStripManual";
            this.toolStripManual.Size = new System.Drawing.Size(52, 52);
            this.toolStripManual.Text = "操作";
            this.toolStripManual.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripManual.Click += new System.EventHandler(this.toolStripManual_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 55);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 558);
            this.panel1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 558);
            this.splitContainer1.SplitterDistance = 754;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.treeViewCameraList, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.richTextMessage, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(253, 558);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // treeViewCameraList
            // 
            this.treeViewCameraList.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewCameraList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewCameraList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewCameraList.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeViewCameraList.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewCameraList.HideSelection = false;
            this.treeViewCameraList.ImageIndex = 0;
            this.treeViewCameraList.ImageList = this.imageList1;
            this.treeViewCameraList.Location = new System.Drawing.Point(3, 3);
            this.treeViewCameraList.Name = "treeViewCameraList";
            this.treeViewCameraList.SelectedImageIndex = 0;
            this.treeViewCameraList.Size = new System.Drawing.Size(247, 273);
            this.treeViewCameraList.TabIndex = 1;
            this.treeViewCameraList.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeViewCameraList_DrawNode);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "connect.png");
            this.imageList1.Images.SetKeyName(1, "disconnect.png");
            // 
            // richTextMessage
            // 
            this.richTextMessage.BackColor = System.Drawing.SystemColors.Control;
            this.richTextMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextMessage.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextMessage.Location = new System.Drawing.Point(3, 282);
            this.richTextMessage.Name = "richTextMessage";
            this.richTextMessage.Size = new System.Drawing.Size(247, 273);
            this.richTextMessage.TabIndex = 2;
            this.richTextMessage.Text = "";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 638);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "主界面";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 管理项目ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 相机设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用户ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton toolStripAutorun;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripManual;
        private System.Windows.Forms.ToolStripButton toolStripStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripButton toolStripOnOffLine;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripLoadImage;
        private System.Windows.Forms.ToolStripMenuItem 通讯设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripLiveOn;
        private System.Windows.Forms.ToolStripButton toolStripLiveOff;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton toolStripRunOnce;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.TreeView treeViewCameraList;
        private System.Windows.Forms.RichTextBox richTextMessage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 程序编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
    }
}

