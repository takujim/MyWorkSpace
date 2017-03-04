namespace MyWorkSpace
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.contextMenuTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.タブ追加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ウインドウ名を変更するToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ウインドウを閉じるToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new MyWorkSpace.TabControlEX();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuTab
            // 
            this.contextMenuTab.Name = "contextMenuTab";
            this.contextMenuTab.Size = new System.Drawing.Size(61, 4);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // タブ追加ToolStripMenuItem
            // 
            this.タブ追加ToolStripMenuItem.Name = "タブ追加ToolStripMenuItem";
            this.タブ追加ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T)));
            this.タブ追加ToolStripMenuItem.Size = new System.Drawing.Size(81, 21);
            this.タブ追加ToolStripMenuItem.Text = "タブ追加(&T)";
            this.タブ追加ToolStripMenuItem.Click += new System.EventHandler(this.タブ追加ToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(247)))));
            this.menuStrip1.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem,
            this.タブ追加ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(632, 25);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ウインドウ名を変更するToolStripMenuItem,
            this.toolStripSeparator1,
            this.ウインドウを閉じるToolStripMenuItem});
            this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            this.ファイルToolStripMenuItem.Size = new System.Drawing.Size(64, 21);
            this.ファイルToolStripMenuItem.Text = "ファイル";
            // 
            // ウインドウ名を変更するToolStripMenuItem
            // 
            this.ウインドウ名を変更するToolStripMenuItem.Name = "ウインドウ名を変更するToolStripMenuItem";
            this.ウインドウ名を変更するToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.ウインドウ名を変更するToolStripMenuItem.Text = "ウインドウ名変更";
            this.ウインドウ名を変更するToolStripMenuItem.Click += new System.EventHandler(this.ウインドウ名を変更するToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(172, 6);
            // 
            // ウインドウを閉じるToolStripMenuItem
            // 
            this.ウインドウを閉じるToolStripMenuItem.Name = "ウインドウを閉じるToolStripMenuItem";
            this.ウインドウを閉じるToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.ウインドウを閉じるToolStripMenuItem.Text = "ウインドウを閉じる";
            this.ウインドウを閉じるToolStripMenuItem.Click += new System.EventHandler(this.ウインドウを閉じるToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Location = new System.Drawing.Point(1, 23);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(632, 430);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(632, 453);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TabControlEX tabControl1;
        private System.Windows.Forms.ContextMenuStrip contextMenuTab;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem タブ追加ToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ウインドウ名を変更するToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ウインドウを閉じるToolStripMenuItem;
    }
}

