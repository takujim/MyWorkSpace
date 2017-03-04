namespace MyWorkSpace
{
    partial class UserControl1
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl1));
            this.treeViewMain = new System.Windows.Forms.TreeView();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblRootDir = new System.Windows.Forms.Label();
            this.btnDir = new System.Windows.Forms.Button();
            this.folderBrowserDialog_TargetDir = new System.Windows.Forms.FolderBrowserDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtSelectTreeNode = new System.Windows.Forms.TextBox();
            this.lblSelectItem = new System.Windows.Forms.Label();
            this.contextMenuTreeViewMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuListViewMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.listViewMain = new MyWorkSpace.ListViewEX();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewMain
            // 
            this.treeViewMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewMain.Location = new System.Drawing.Point(3, 3);
            this.treeViewMain.Name = "treeViewMain";
            this.treeViewMain.Size = new System.Drawing.Size(196, 336);
            this.treeViewMain.TabIndex = 2;
            this.treeViewMain.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewMain_BeforeLabelEdit);
            this.treeViewMain.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewMain_AfterLabelEdit);
            this.treeViewMain.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeViewMain.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
            this.treeViewMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeViewMain.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeViewMain.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeViewMain_KeyUp);
            // 
            // txtAddress
            // 
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtAddress.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.txtAddress.Location = new System.Drawing.Point(115, 9);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(472, 19);
            this.txtAddress.TabIndex = 0;
            this.txtAddress.Enter += new System.EventHandler(this.txtAddress_Enter);
            this.txtAddress.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtAddress_KeyUp);
            this.txtAddress.Leave += new System.EventHandler(this.txtAddress_Leave);
            this.txtAddress.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtAddress_PreviewKeyDown);
            // 
            // lblRootDir
            // 
            this.lblRootDir.AutoSize = true;
            this.lblRootDir.Location = new System.Drawing.Point(13, 12);
            this.lblRootDir.Name = "lblRootDir";
            this.lblRootDir.Size = new System.Drawing.Size(68, 12);
            this.lblRootDir.TabIndex = 6;
            this.lblRootDir.Text = "ルートフォルダ";
            // 
            // btnDir
            // 
            this.btnDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDir.BackColor = System.Drawing.Color.Transparent;
            this.btnDir.Image = ((System.Drawing.Image)(resources.GetObject("btnDir.Image")));
            this.btnDir.Location = new System.Drawing.Point(593, 8);
            this.btnDir.Name = "btnDir";
            this.btnDir.Size = new System.Drawing.Size(20, 19);
            this.btnDir.TabIndex = 8;
            this.btnDir.TabStop = false;
            this.btnDir.UseVisualStyleBackColor = false;
            this.btnDir.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Location = new System.Drawing.Point(3, 58);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.Controls.Add(this.treeViewMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel2.Controls.Add(this.listViewMain);
            this.splitContainer1.Size = new System.Drawing.Size(616, 342);
            this.splitContainer1.SplitterDistance = 202;
            this.splitContainer1.TabIndex = 2;
            this.splitContainer1.TabStop = false;
            // 
            // txtSelectTreeNode
            // 
            this.txtSelectTreeNode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSelectTreeNode.Location = new System.Drawing.Point(115, 33);
            this.txtSelectTreeNode.Name = "txtSelectTreeNode";
            this.txtSelectTreeNode.ReadOnly = true;
            this.txtSelectTreeNode.Size = new System.Drawing.Size(498, 19);
            this.txtSelectTreeNode.TabIndex = 1;
            this.txtSelectTreeNode.Click += new System.EventHandler(this.txtSelectTreeNode_Click_1);
            this.txtSelectTreeNode.Enter += new System.EventHandler(this.txtSelectTreeNode_Enter);
            this.txtSelectTreeNode.Leave += new System.EventHandler(this.txtSelectTreeNode_Leave);
            // 
            // lblSelectItem
            // 
            this.lblSelectItem.AutoSize = true;
            this.lblSelectItem.Location = new System.Drawing.Point(13, 36);
            this.lblSelectItem.Name = "lblSelectItem";
            this.lblSelectItem.Size = new System.Drawing.Size(64, 12);
            this.lblSelectItem.TabIndex = 5;
            this.lblSelectItem.Text = "選択フォルダ";
            // 
            // contextMenuTreeViewMain
            // 
            this.contextMenuTreeViewMain.Name = "contextMenuStrip1";
            this.contextMenuTreeViewMain.Size = new System.Drawing.Size(61, 4);
            this.contextMenuTreeViewMain.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            this.contextMenuTreeViewMain.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.contextMenuTreeViewMain_PreviewKeyDown);
            // 
            // contextMenuListViewMain
            // 
            this.contextMenuListViewMain.Name = "contextMenuStrip2";
            this.contextMenuListViewMain.Size = new System.Drawing.Size(61, 4);
            this.contextMenuListViewMain.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip2_ItemClicked);
            this.contextMenuListViewMain.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.contextMenuListViewMain_PreviewKeyDown);
            // 
            // listViewMain
            // 
            this.listViewMain.AllowDrop = true;
            this.listViewMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewMain.Location = new System.Drawing.Point(3, 3);
            this.listViewMain.Name = "listViewMain";
            this.listViewMain.Size = new System.Drawing.Size(404, 336);
            this.listViewMain.TabIndex = 3;
            this.listViewMain.UseCompatibleStateImageBehavior = false;
            this.listViewMain.View = System.Windows.Forms.View.Details;
            this.listViewMain.VerticalScrolled += new MyWorkSpace.ListViewEX.custumVerticalScrolled(this.listViewMain_VerticalScrolled);
            this.listViewMain.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewMain_AfterLabelEdit);
            this.listViewMain.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewMain_BeforeLabelEdit);
            this.listViewMain.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listViewMain.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewMain_ItemDrag);
            this.listViewMain.SelectedIndexChanged += new System.EventHandler(this.listViewMain_SelectedIndexChanged);
            this.listViewMain.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewMain_DragDrop);
            this.listViewMain.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewMain_DragEnter);
            this.listViewMain.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listViewMain.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listViewMain_KeyUp);
            this.listViewMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewMain_MouseClick);
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.txtSelectTreeNode);
            this.Controls.Add(this.lblSelectItem);
            this.Controls.Add(this.lblRootDir);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.btnDir);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(622, 403);
            this.Enter += new System.EventHandler(this.UserControl1_Enter);
            this.ParentChanged += new System.EventHandler(this.UserControl1_ParentChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewMain;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblRootDir;
        private System.Windows.Forms.Button btnDir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_TargetDir;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtSelectTreeNode;
        private System.Windows.Forms.Label lblSelectItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuTreeViewMain;
        private System.Windows.Forms.ContextMenuStrip contextMenuListViewMain;
        private ListViewEX listViewMain;
    }
}
