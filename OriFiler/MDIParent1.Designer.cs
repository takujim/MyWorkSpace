namespace MyWorkSpace
{
    partial class MDIParent1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIParent1));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ランチャToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.tileVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.選択されているフォームを最大化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.次のウインドウを選択ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.カスタマイズToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.フォントToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ヘルプToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(247)))));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.ランチャToolStripMenuItem,
            this.windowsMenu,
            this.カスタマイズToolStripMenuItem,
            this.helpMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.MdiWindowListItem = this.windowsMenu;
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(632, 26);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator5,
            this.exitToolStripMenuItem});
            this.fileMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder;
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(85, 22);
            this.fileMenu.Text = "ファイル(&F)";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.newToolStripMenuItem.Text = "新規ウインドウ";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.ShowNewForm);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(205, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.exitToolStripMenuItem.Text = "アプリケーションの終了";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolsStripMenuItem_Click);
            // 
            // ランチャToolStripMenuItem
            // 
            this.ランチャToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.ランチャToolStripMenuItem.Name = "ランチャToolStripMenuItem";
            this.ランチャToolStripMenuItem.Size = new System.Drawing.Size(86, 22);
            this.ランチャToolStripMenuItem.Text = "ランチャ(&R)";
            this.ランチャToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ランチャToolStripMenuItem_DropDownItemClicked);
            // 
            // windowsMenu
            // 
            this.windowsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tileVerticalToolStripMenuItem,
            this.cascadeToolStripMenuItem,
            this.toolStripSeparator1,
            this.選択されているフォームを最大化ToolStripMenuItem,
            this.次のウインドウを選択ToolStripMenuItem});
            this.windowsMenu.Name = "windowsMenu";
            this.windowsMenu.Size = new System.Drawing.Size(102, 22);
            this.windowsMenu.Text = "ウィンドウ(&W)";
            // 
            // tileVerticalToolStripMenuItem
            // 
            this.tileVerticalToolStripMenuItem.Name = "tileVerticalToolStripMenuItem";
            this.tileVerticalToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.tileVerticalToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.tileVerticalToolStripMenuItem.Text = "左右に並べて表示(&V)";
            this.tileVerticalToolStripMenuItem.Click += new System.EventHandler(this.TileVerticalToolStripMenuItem_Click);
            // 
            // cascadeToolStripMenuItem
            // 
            this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
            this.cascadeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.cascadeToolStripMenuItem.Text = "重ねて表示(&C)";
            this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.CascadeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(279, 6);
            // 
            // 選択されているフォームを最大化ToolStripMenuItem
            // 
            this.選択されているフォームを最大化ToolStripMenuItem.Name = "選択されているフォームを最大化ToolStripMenuItem";
            this.選択されているフォームを最大化ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
            this.選択されているフォームを最大化ToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.選択されているフォームを最大化ToolStripMenuItem.Text = "選択ウインドウを最大化";
            this.選択されているフォームを最大化ToolStripMenuItem.Click += new System.EventHandler(this.選択されているフォームを最大化ToolStripMenuItem_Click);
            // 
            // 次のウインドウを選択ToolStripMenuItem
            // 
            this.次のウインドウを選択ToolStripMenuItem.Name = "次のウインドウを選択ToolStripMenuItem";
            this.次のウインドウを選択ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Tab)));
            this.次のウインドウを選択ToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.次のウインドウを選択ToolStripMenuItem.Text = "次のウインドウを選択";
            this.次のウインドウを選択ToolStripMenuItem.Click += new System.EventHandler(this.次のウインドウを選択ToolStripMenuItem_Click);
            // 
            // カスタマイズToolStripMenuItem
            // 
            this.カスタマイズToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.フォントToolStripMenuItem});
            this.カスタマイズToolStripMenuItem.Name = "カスタマイズToolStripMenuItem";
            this.カスタマイズToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.カスタマイズToolStripMenuItem.Text = "カスタマイズ";
            // 
            // フォントToolStripMenuItem
            // 
            this.フォントToolStripMenuItem.Name = "フォントToolStripMenuItem";
            this.フォントToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.フォントToolStripMenuItem.Text = "フォント";
            this.フォントToolStripMenuItem.Click += new System.EventHandler(this.フォントToolStripMenuItem_Click);
            // 
            // helpMenu
            // 
            this.helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.ヘルプToolStripMenuItem});
            this.helpMenu.Name = "helpMenu";
            this.helpMenu.Size = new System.Drawing.Size(75, 22);
            this.helpMenu.Text = "ヘルプ(&H)";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.aboutToolStripMenuItem.Text = "バージョン情報";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // ヘルプToolStripMenuItem
            // 
            this.ヘルプToolStripMenuItem.Name = "ヘルプToolStripMenuItem";
            this.ヘルプToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.ヘルプToolStripMenuItem.Text = "ヘルプ";
            this.ヘルプToolStripMenuItem.Click += new System.EventHandler(this.ヘルプToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(247)))));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 430);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(632, 23);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(32, 18);
            this.toolStripStatusLabel.Text = "状態";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MDIParent1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(632, 453);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MDIParent1";
            this.Text = "MyWorkSpace";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.MDIParent1_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MDIParent1_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.MDIParent1_ResizeEnd);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsMenu;
        private System.Windows.Forms.ToolStripMenuItem cascadeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileVerticalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenu;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem ランチャToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ヘルプToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem 選択されているフォームを最大化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 次のウインドウを選択ToolStripMenuItem;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ToolStripMenuItem カスタマイズToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem フォントToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}



