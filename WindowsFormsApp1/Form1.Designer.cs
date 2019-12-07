namespace WindowsFormsApp1
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.项目管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProject = new System.Windows.Forms.Button();
            this.deleteProject = new System.Windows.Forms.Button();
            this.importProject = new System.Windows.Forms.Button();
            this.exportMergeData = new System.Windows.Forms.Button();
            this.loadProject = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.项目管理ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1166, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 项目管理ToolStripMenuItem
            // 
            this.项目管理ToolStripMenuItem.Name = "项目管理ToolStripMenuItem";
            this.项目管理ToolStripMenuItem.Size = new System.Drawing.Size(83, 24);
            this.项目管理ToolStripMenuItem.Text = "项目管理";
            // 
            // newProject
            // 
            this.newProject.Location = new System.Drawing.Point(6, 32);
            this.newProject.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.newProject.Name = "newProject";
            this.newProject.Size = new System.Drawing.Size(140, 68);
            this.newProject.TabIndex = 1;
            this.newProject.Text = "新增项目";
            this.newProject.UseVisualStyleBackColor = true;
            this.newProject.Click += new System.EventHandler(this.button1_Click);
            // 
            // deleteProject
            // 
            this.deleteProject.Location = new System.Drawing.Point(6, 106);
            this.deleteProject.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.deleteProject.Name = "deleteProject";
            this.deleteProject.Size = new System.Drawing.Size(140, 68);
            this.deleteProject.TabIndex = 2;
            this.deleteProject.Text = "删除项目";
            this.deleteProject.UseVisualStyleBackColor = true;
            this.deleteProject.Click += new System.EventHandler(this.button2_Click);
            // 
            // importProject
            // 
            this.importProject.Location = new System.Drawing.Point(6, 179);
            this.importProject.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.importProject.Name = "importProject";
            this.importProject.Size = new System.Drawing.Size(140, 68);
            this.importProject.TabIndex = 3;
            this.importProject.Text = "导入信息";
            this.importProject.UseVisualStyleBackColor = true;
            this.importProject.Click += new System.EventHandler(this.button3_Click);
            // 
            // exportMergeData
            // 
            this.exportMergeData.Location = new System.Drawing.Point(6, 252);
            this.exportMergeData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.exportMergeData.Name = "exportMergeData";
            this.exportMergeData.Size = new System.Drawing.Size(140, 68);
            this.exportMergeData.TabIndex = 4;
            this.exportMergeData.Text = "合并数据导出";
            this.exportMergeData.UseVisualStyleBackColor = true;
            this.exportMergeData.Click += new System.EventHandler(this.button4_Click);
            // 
            // loadProject
            // 
            this.loadProject.Location = new System.Drawing.Point(6, 326);
            this.loadProject.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loadProject.Name = "loadProject";
            this.loadProject.Size = new System.Drawing.Size(140, 68);
            this.loadProject.TabIndex = 5;
            this.loadProject.Text = "加载当前项目";
            this.loadProject.UseVisualStyleBackColor = true;
            this.loadProject.Click += new System.EventHandler(this.button5_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(4, 22);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 30;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(977, 531);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(11, 29);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(987, 575);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.newProject);
            this.groupBox2.Controls.Add(this.deleteProject);
            this.groupBox2.Controls.Add(this.loadProject);
            this.groupBox2.Controls.Add(this.importProject);
            this.groupBox2.Controls.Add(this.exportMergeData);
            this.groupBox2.Location = new System.Drawing.Point(998, 30);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(156, 552);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 587);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "身份证阅读图像采集系统";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 项目管理ToolStripMenuItem;
        private System.Windows.Forms.Button newProject;
        private System.Windows.Forms.Button deleteProject;
        private System.Windows.Forms.Button importProject;
        private System.Windows.Forms.Button exportMergeData;
        private System.Windows.Forms.Button loadProject;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

