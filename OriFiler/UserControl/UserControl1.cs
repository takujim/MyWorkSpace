using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Threading;

using System.Runtime.InteropServices;
using Msio;//for DLL

namespace MyWorkSpace
{
    /// <summary>
    /// エクスプローラユーザコントロール
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        // -----------------------------------------------------------------【Const】

        #region 固定値

        const string C_ContextMenuStripItem_property = "プロパティ";
        const string C_ContextMenuStripItem_PathCopy = "ファイルパスをコピー";
        const string C_ContextMenuStripItem_OpenFile = "開く";
        const string C_ContextMenuStripItem_OpenDirectory = "エクスプローラで開く";
        const string C_ContextMenuStripItem_Create = "新規作成";
        const string C_ContextMenuStripItem_CreateFile = "ファイル作成";
        const string C_ContextMenuStripItem_CreateFolder = "フォルダ作成";
        const string C_ContextMenuStripItem_NewTabOpen = "新規タブで開く";
        const string C_ContextMenuStripItem_CurrentTabOpen = "このタブで開く";
        const string C_ContextMenuStripItem_Delete = "削除";
        const string C_ContextMenuStripItem_Rename = "名前の変更";
        const string C_ContextMenuStripItem_Copy = "コピー";
        const string C_ContextMenuStripItem_Move = "切り取り";
        const string C_ContextMenuStripItem_Paste = "貼り付け";
        const string C_ContextMenuStripItem_UserCustom = "プログラムを指定して開く";

        const string C_Root = "[Root]";

        const int ThumbnailMaxHeight = 160;
        const int ThumbnailMaxWidth = 160;
        //const int TrackMaxCount = 9;
        const int TrackMaxCount = 1;

        #endregion

        // -----------------------------------------------------------------【Member Variable】

        #region メンバ変数

        private Form1 m_parentForm;

        /// <summary>
        /// ユーザメニューの実行パスリスト
        /// </summary>
        private NameValueCollection m_ContextExecList = null;

        /// <summary>
        /// ファイル操作オブジェクト
        /// </summary>
        private CopyPasteMoveManager m_CopyPasteMoveManager = null;

        /// <summary>
        /// メッセージ表示操作オブジェクト
        /// </summary>
        private Msio.MessageManager m_MessageManager = null;

        #endregion

        // -----------------------------------------------------------------【Constructer】

        #region コンストラクタ

        public UserControl1(Form1 parent, string strRootAddress, CopyPasteMoveManager objCopyPasteMoveManager, Msio.MessageManager objMessageManager)
        {
            InitializeComponent();

            // 引数をメンバに設定
            m_parentForm = parent;
            m_CopyPasteMoveManager = objCopyPasteMoveManager;
            m_MessageManager = objMessageManager;
            m_MessageManager.ToSplashMsg("「 " + strRootAddress + " 」表示");
            if (strRootAddress != string.Empty)
            {
                txtAddress.Text = strRootAddress;
            }

            // メモリ消費に注意
            this.DoubleBuffered = true;

            // 初期設定
            InitContextMenuStrip();
            InitListView();
            InitTreeView();
        }

        /// <summary>
        /// 親コントロールチェンジイベント（タブページに関連付けされた際に一回発生する）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl1_ParentChanged(object sender, EventArgs e)
        {
            DrawTree();
            txtAddress.Text = txtBoxBeautifulShow(txtAddress.Text);
        }

        /// <summary>
        /// 【初期設定】ツリー
        /// </summary>
        private void InitTreeView()
        {
            ImageList imgList = new ImageList();
            imgList.Images.Add(new Icon("img\\GenericDir.ico"));
            imgList.Images.Add(new Icon("img\\GenericOpenDir.ico"));

            //// サンプルプログラムからフォルダ表示用のリソースを無理やり取得した
            //// 以下のソース+resxファイルをxmlで編集した（"TreeNodeImageList.ImageStream"）を追加した
            //ImageList imgList = new ImageList(components);
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl1));
            //imgList.ImageStream = (ImageListStreamer)resources.GetObject("TreeNodeImageList.ImageStream");
            //imgList.Images.SetKeyName(0, "ClosedFolder");
            //imgList.Images.SetKeyName(1, "OpenFolder");

            treeViewMain.ImageList = imgList;
            treeViewMain.HideSelection = true;

            treeViewMain.TreeViewNodeSorter = new NodeSorter();
        }

        enum listViewColumn
        {
            名前,
            更新日時,
            作成日時,
            拡張子,
            サイズ
        }

        /// <summary>
        ///  【初期設定】リスト
        /// </summary>
        private void InitListView()
        {
            listViewMain.GridLines = true;
            listViewMain.Sorting = SortOrder.Ascending;

            ColumnHeader columnName = new ColumnHeader();
            ColumnHeader columnUpdateDate = new ColumnHeader();
            ColumnHeader columnCreateDate = new ColumnHeader();
            ColumnHeader columnExtention = new ColumnHeader();
            ColumnHeader columnSize = new ColumnHeader();

            columnName.Text = listViewColumn.名前.ToString();
            columnUpdateDate.Text = listViewColumn.更新日時.ToString();
            columnCreateDate.Text = listViewColumn.作成日時.ToString();
            columnExtention.Text = listViewColumn.拡張子.ToString();
            columnSize.Text = listViewColumn.サイズ.ToString();

            listViewMain.Columns.Add(columnName);
            listViewMain.Columns.Add(columnUpdateDate);
            listViewMain.Columns.Add(columnCreateDate);
            listViewMain.Columns.Add(columnExtention);
            listViewMain.Columns.Add(columnSize);

            //listViewMain.FullRowSelect = true;
        }

        /// <summary>
        /// 【初期設定】右クリックメニュー設定取得
        /// </summary>
        private void InitContextMenuStrip()
        {
            // 新タブで開く
            contextMenuTreeViewMain.Items.Add(C_ContextMenuStripItem_NewTabOpen);
            // 現在のタブで開く
            contextMenuTreeViewMain.Items.Add(C_ContextMenuStripItem_CurrentTabOpen);
            contextMenuTreeViewMain.Items.Add(new ToolStripSeparator());

            // 開く
            contextMenuListViewMain.Items.Add(C_ContextMenuStripItem_OpenFile);

            // このフォルダを開く
            contextMenuTreeViewMain.Items.Add(C_ContextMenuStripItem_OpenDirectory);
            contextMenuListViewMain.Items.Add(C_ContextMenuStripItem_OpenDirectory);

            // カスタムメニュー
            m_ContextExecList = new NameValueCollection();
            ToolStripMenuItem CustomMenuTree = new ToolStripMenuItem(C_ContextMenuStripItem_UserCustom);
            ToolStripMenuItem CustomMenuList = new ToolStripMenuItem(C_ContextMenuStripItem_UserCustom);

            contextMenuTreeViewMain.Items.Add(CustomMenuTree);
            contextMenuListViewMain.Items.Add(CustomMenuList);

            //using (StreamReader StReader = new StreamReader(Program.CONFDIR + "ContextSetting.ini", Encoding.UTF8))
            //{
            //    while (StReader.Peek() > 0)
            //    {
            //        string strTmp = StReader.ReadLine();
            //        string[] strTmp2 = strTmp.Split('\t');
            //        CustomMenu.DropDownItems.Add(strTmp2[0], null, this.CustomMenuSelectedFromTree);
            //        CustomMenu2.DropDownItems.Add(strTmp2[0], null, this.CustomMenuSelectedFromList);
            //        m_ContextExecList.Add(strTmp2[0], strTmp2[1]);

            //        contextMenuTreeViewMain.Items.AddRange(new ToolStripItem[] { CustomMenu });
            //        contextMenuListViewMain.Items.AddRange(new ToolStripItem[] { CustomMenu2 });
            //    }
            //}

            m_ContextExecList = ToolStripProgramStartMenuFactory.getStripMenu(
                "TreeContext",
                CustomMenuTree,
                CustomMenuSelectedFromTree_DropDownItemClicked,
                CustomMenuSelectedFromTree_SearchMenuItem_KeyDown,
                Path.Combine(Program.CONFDIR, "ContextSetting.ini")
                );

            ToolStripProgramStartMenuFactory.getStripMenu(
                "ListContext",
                CustomMenuList,
                CustomMenuSelectedFromList_DropDownItemClicked,
                CustomMenuSelectedFromList_SearchMenuItem_KeyDown,
                Path.Combine(Program.CONFDIR, "ContextSetting.ini")
                );


            // パスをコピー
            contextMenuTreeViewMain.Items.Add(C_ContextMenuStripItem_PathCopy);
            contextMenuListViewMain.Items.Add(C_ContextMenuStripItem_PathCopy);

            contextMenuListViewMain.Items.Add(new ToolStripSeparator());
            contextMenuTreeViewMain.Items.Add(new ToolStripSeparator());

            // 新規作成
            ToolStripMenuItem CreateMenuList = new ToolStripMenuItem(C_ContextMenuStripItem_Create);
            CreateMenuList.DropDownItems.Add(C_ContextMenuStripItem_CreateFile, null, Create_File);
            CreateMenuList.DropDownItems.Add(C_ContextMenuStripItem_CreateFolder, null, Create_Folder);
            contextMenuListViewMain.Items.AddRange(new ToolStripItem[] { CreateMenuList });
            ToolStripMenuItem CreateMenuTree = new ToolStripMenuItem(C_ContextMenuStripItem_Create);
            CreateMenuTree.DropDownItems.Add(C_ContextMenuStripItem_CreateFolder, null, Create_Folder);
            contextMenuTreeViewMain.Items.AddRange(new ToolStripItem[] { CreateMenuTree });

            // 削除
            contextMenuListViewMain.Items.Add(C_ContextMenuStripItem_Delete);
            contextMenuTreeViewMain.Items.Add(C_ContextMenuStripItem_Delete);

            // 名前の変更
            contextMenuListViewMain.Items.Add(C_ContextMenuStripItem_Rename);
            contextMenuTreeViewMain.Items.Add(C_ContextMenuStripItem_Rename);
            contextMenuListViewMain.Items.Add(new ToolStripSeparator());
            contextMenuTreeViewMain.Items.Add(new ToolStripSeparator());

            // ファイル操作
            contextMenuListViewMain.Items.Add(C_ContextMenuStripItem_Copy);
            contextMenuListViewMain.Items.Add(C_ContextMenuStripItem_Move);
            contextMenuListViewMain.Items.Add(C_ContextMenuStripItem_Paste);
            contextMenuTreeViewMain.Items.Add(C_ContextMenuStripItem_Copy);
            contextMenuTreeViewMain.Items.Add(C_ContextMenuStripItem_Move);
            contextMenuTreeViewMain.Items.Add(C_ContextMenuStripItem_Paste);
            contextMenuListViewMain.Items.Add(new ToolStripSeparator());
            contextMenuTreeViewMain.Items.Add(new ToolStripSeparator());

            // プロパティ
            contextMenuTreeViewMain.Items.Add(C_ContextMenuStripItem_property);
            contextMenuListViewMain.Items.Add(C_ContextMenuStripItem_property);

            // 最後にバインドする
            treeViewMain.ContextMenuStrip = contextMenuTreeViewMain;
            listViewMain.ContextMenuStrip = contextMenuListViewMain;
        }

        #endregion

        // -----------------------------------------------------------------【Public】

        #region ルートアドレスを設定

        public string p_RootAddress
        {
            get
            {
                var path = stopBeautifulShow(txtAddress.Text);
                if (path.Length > 0 && path.Last() == ':')
                {
                    path += @"\";
                }

                return path;
            }
        }

        #endregion

        public string p_SelectAddress
        {
            get
            {
                // ルートがドライブだと、この実装ダメ。
                //return Path.Combine(p_RootAddress, stopBeautifulShow(txtSelectTreeNode.Text).Replace(C_Root, ""));

                var rootAddress = p_RootAddress;
                if (rootAddress.Last().Equals(Convert.ToChar(@"\")))
                {
                    rootAddress = rootAddress.Substring(0, p_RootAddress.Length - 1);
                }
                return stopBeautifulShow(txtSelectTreeNode.Text).Replace(C_Root, rootAddress);
            }
        }

        #region リストビュー表示形式設定

        public void SetListViewMode(View csView)
        {
            listViewMain.View = csView;
        }

        #endregion

        // -----------------------------------------------------------------【Event Handler】

        #region イベントハンドラ

        #region フォルダ選択ボタンクリックイベント

        /// <summary>
        /// 参照フォルダ選択ボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog_TargetDir.RootFolder = Environment.SpecialFolder.Desktop;
            folderBrowserDialog_TargetDir.Description = "ルートフォルダを選択してください。";
            if (folderBrowserDialog_TargetDir.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtAddress.Text = folderBrowserDialog_TargetDir.SelectedPath;
                DrawTree();
            }
        }

        #endregion

        #region ルートアドレステキストボックスキープレスイベント

        /// <summary>
        /// ルートアドレステキストボックスキープレスイベント
        /// (オートコンプリートを使った場合、KeyPressだとEnterが認識されないためこの対応)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAddress_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // Enterキーが押された場合はツリーを再描画
            if (e.KeyCode == Keys.Enter)
            {
                txtAddress.Text = GetFileText(txtAddress.Text);
                DrawTree();
                txtSelectTreeNode.Text = C_Root;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (treeViewMain.TopNode != null)
                {
                    txtAddress.Text = treeViewMain.TopNode.Text;
                }

            }
        }

        private void txtAddress_KeyUp(object sender, KeyEventArgs e)
        {
            ////  TODO 一回目はこれで良いが、Ctrl+A 2回目押されるとなぜか値消える。。
            //if (e.KeyCode == Keys.A)
            //{
            //    if (e.Modifiers == (Keys.Control))
            //    {
            //        //txtAddress.SelectAll();
            //        e.Handled = true;
            //    }
            //}
            //e.Handled = true;
        }

        #endregion

        #region ディレクトリOnly表示切り替えイベント

        /// <summary>
        /// ツリー表示（ディレクトリorファイル含む）切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DrawTree();
        }

        #endregion

        //#region トラックバー値変更イベント

        List<ImageList> m_imgList = null;
        //private void trackBar1_ValueChanged(object sender, EventArgs e)
        //{
        //    if (listViewMain.Items.Count > 0)
        //    {
        //        listViewMain.LargeImageList = m_imgList[1];
        //        listViewMain.SmallImageList = m_imgList[1];
        //    }
        //}

        //#endregion


        #region ツリー選択前イベント

        /// <summary>
        /// ツリー選択前イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            // これを有効にするとこのタブで開くのEnterでエクスプローラが開いてしまうため削除
            //m_contextEnterFlg = false;
            // 背景色を戻す
            SetNodeBackColor(Color.Transparent);
        }

        #endregion

        #region ツリー選択後イベント

        /// <summary>
        /// ツリー選択後イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            txtSelectTreeNode.Text = GetFullPathFromSelectNode()[0];
            txtSelectTreeNode.Text = txtSelectBeautifulShow(txtSelectTreeNode.Text);
            // リストを描画
            ListViewUpdate();
        }

        #endregion

        #region ツリーノードクリックイベント

        /// <summary>
        /// ツリーノードクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // 右クリックでも選択されるようにここで設定している
            treeViewMain.SelectedNode = e.Node;
        }

        #endregion

        #region ツリー開くイベント

        /// <summary>
        /// ツリー開くイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Expand)
            {
                // 開かれたノード配下を描画
                treeViewMain.BeginUpdate();

                string[] strDirs = Directory.GetDirectories(e.Node.FullPath);
                e.Node.Nodes.Clear();

                foreach (string strAddNode in strDirs)
                {
                    AddSingleNode(e.Node.Nodes, Path.Combine(e.Node.FullPath, strAddNode));
                }

                treeViewMain.EndUpdate();
                treeViewMain.SelectedNode = e.Node;
            }
        }

        #endregion

        #region ツリーラベル編集イベント、フォルダリネーム処理

        private void treeViewMain_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (treeViewMain.SelectedNode == treeViewMain.TopNode)
            {
                MessageBox.Show("トップノードの名前は変更できません");
                e.CancelEdit = true;
                listViewMain.LabelEdit = false;
            }
        }

        /// <summary>
        /// ツリーリネームイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewMain_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                try
                {
                    DirectoryInfo a = new DirectoryInfo(GetFullPathFromSelectNode()[0]);

                    string RenamePath = Path.Combine(treeViewMain.SelectedNode.Parent.FullPath, e.Label);
                    string RenameConnectPath = GetFileText(GetFullPathFromSelectNode()[0]);
                    // 大文字・小文字変換対応
                    if (Directory.Exists(RenamePath))
                    {
                        int suffix = 0;
                        while (File.Exists(Path.Combine(treeViewMain.SelectedNode.Parent.FullPath, e.Label + suffix.ToString())))
                        {
                            suffix++;
                        }

                        RenameConnectPath = Path.Combine(treeViewMain.SelectedNode.Parent.FullPath, e.Label + suffix.ToString());
                        Directory.Move(GetFileText(GetFullPathFromSelectNode()[0]), RenameConnectPath);
                    }

                    Directory.Move(RenameConnectPath, RenamePath);
                    //Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(GetFullPathFromSelectNode()[0], e.Label);
                    //txtSelectTreeNode.Text = GetDirectoryText(Path.Combine(GetFullPathFromSelectNode().First().Substring(0,
                    //    GetFullPathFromSelectNode().First().LastIndexOf(treeViewMain.SelectedNode.Text))
                    //    ,e.Label));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    e.CancelEdit = true;
                }
            }
            treeViewMain.LabelEdit = false;
        }

        #endregion

        #region ツリービューキーアップイベント

        public void treeviewUpdate()
        {
            treeViewMain.BeginUpdate();

            if (treeViewMain.SelectedNode != null)
            {
                var selectedNode = treeViewMain.SelectedNode;
                selectedNode.Collapse();
                selectedNode.Expand();
            }

            treeViewMain.EndUpdate();
        }

        public class NodeSorter : System.Collections.IComparer
        {
            // Compare the length of the strings, or the strings
            // themselves, if they are the same length.
            public int Compare(object x, object y)
            {
                TreeNode tx = x as TreeNode;
                TreeNode ty = y as TreeNode;

                // Compare the length of the strings, returning the difference.
                //if (tx.Text.Length != ty.Text.Length)
                //    return tx.Text.Length - ty.Text.Length;

                // If they are the same length, call Compare.
                return string.Compare(tx.Text, ty.Text);
            }
        }

        /// <summary>
        /// ツリーキーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewMain_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    m_contextEnterFlg = true;
                    // ノード名編集
                    StartRename();
                    break;

                case Keys.F5:
                    // 再描画
                    treeviewUpdate();
                    ListViewUpdate();
                    break;

                case Keys.Enter:
                    if (m_contextEnterFlg)
                    {
                        m_contextEnterFlg = false;
                    }
                    else
                    {
                        // いろんなとこで反応してしまうためやめた。ダイアログのEnterとか
                        //ProcessStart(GetFullPathFromSelectNode().First());
                    }
                    break;

                case Keys.Delete:
                    DeleteTargetItem(GetFullPathFromSelectNode());
                    break;

                case (Keys.ShiftKey):
                    if (e.Modifiers == Keys.Control)
                    {
                        contextMenuTreeViewMain.Show(treeViewMain.Location);
                    }
                    break;

                case Keys.C:
                    // コピー
                    if (e.Modifiers == (Keys.Shift | Keys.Control))
                    {
                        PathCopy(GetFullPathFromSelectNode());
                    }
                    else if (e.Modifiers == Keys.Control)
                    {
                        Copy(GetFullPathFromSelectNode());
                    }
                    break;

                case Keys.X:
                    // 切り取り
                    if (e.Modifiers == Keys.Control)
                    {
                        MoveCut(GetFullPathFromSelectNode());
                    }
                    break;

                case Keys.V:
                    // 貼り付け
                    if (e.Modifiers == Keys.Control)
                    {
                        Paste();
                    }
                    break;
            }
        }

        #endregion


        #region リスト上マウスクリックイベント

        /// <summary>
        /// リスト上マウスクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewMain_MouseClick(object sender, MouseEventArgs e)
        {
            // 右クリックでも選択されるように設定
            listViewMain.FocusedItem = listViewMain.GetItemAt(e.X, e.Y);
        }

        #endregion

        #region リストダブルクリックイベント

        /// <summary>
        /// リストダブルクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ProcessStart(GetFullPathFromSelectList()[0]);
        }

        #endregion

        #region リストヘッダークリックイベント（ソート）

        private int sortOrder = 1;
        private int sortColumn = 0;
        /// <summary>
        /// リストヘッダークリックイベント並び替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            listViewMain.ListViewItemSorter = new ListViewItemComparer(e.Column, sortOrder);
            sortOrder *= -1;
            sortColumn = e.Column;

            //listView1.Columns[e.Column].Text += "▼";
        }

        #endregion

        #region リストスクロールイベント

        private ListViewItem m_tmpItem = null;
        /// <summary>
        /// カスタムイベント（リストのスクロールイベント）TODO:修正要
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewMain_VerticalScrolled(object sender, EventArgs e)
        {
            //if (listViewMain.View == View.LargeIcon)
            //{
            //    ListViewItem firstItem = listViewMain.GetItemAt(20, 20);

            //    if (firstItem != null && m_tmpItem != firstItem)
            //    {
            //        m_tmpItem = firstItem;
            //        int startIndex = listViewMain.Items.IndexOf(firstItem);

            //        for (int n = startIndex; listViewMain.Items.Count > n && n < startIndex + 30; n++)
            //        {
            //            ListViewItem tmpItem = listViewMain.Items[n];

            //            string strExtention = Path.GetExtension(tmpItem.Text).ToUpper();
            //            if (strExtention == ".JPG" || strExtention == ".BMP" || strExtention == ".GIF" || strExtention == ".PNG")
            //            {
            //                // falseオプションでかなり高速化した
            //                Image orig = Image.FromStream(
            //                    new FileStream(Path.Combine(txtSelectTreeNode.Text, tmpItem.Text)
            //                    , FileMode.Open, FileAccess.Read), false, false);
            //                Image thumbnail = orig.GetThumbnailImage(ThumbnailMaxHeight, ThumbnailMaxWidth, delegate { return false; }, IntPtr.Zero);

            //                for (int i = 0; i <= 1; i++)
            //                {
            //                    m_imgList[i].Images.Add(thumbnail);
            //                }

            //                tmpItem.ImageIndex = m_imgList[0].Images.Count - 1;
            //            }
            //            else
            //            {
            //            }
            //        }
            //    }
            //}
        }

        #endregion

        #region リストラベル編集イベント、ファイルリネーム処理

        private void listViewMain_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
        }

        /// <summary>
        /// リストリネームイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewMain_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                try
                {
                    File.Move(GetFullPathFromSelectList()[0], Path.Combine(p_SelectAddress, e.Label));
                    //Microsoft.VisualBasic.FileIO.FileSystem.RenameFile(GetFullPathFromSelectList()[0], e.Label);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    e.CancelEdit = true;
                }
            }
            listViewMain.LabelEdit = false;
        }

        #endregion

        #region リストビューキーアップイベント

        /// <summary>
        /// リストキーイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewMain_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    m_contextEnterFlg = true;
                    StartRename();
                    break;

                case Keys.F5:
                    ListViewUpdate();
                    break;

                case Keys.Enter:
                    if (m_contextEnterFlg)
                    {
                        m_contextEnterFlg = false;
                    }
                    else
                    {
                        ProcessStart(GetFullPathFromSelectList()[0]);
                    }
                    break;
                //case Keys.E:
                //    if (e.Modifiers == Keys.Control)
                //    {
                //        ProcessStart(GetFullPathFromSelectList()[0]);
                //    }
                //    break;

                case Keys.Delete:
                    DeleteTargetItem(GetFullPathFromSelectList());
                    break;

                case (Keys.ShiftKey):
                    if (e.Modifiers == Keys.Control)
                    {
                        contextMenuListViewMain.Show(listViewMain.Location);
                    }
                    break;

                case Keys.C:
                    // コピー
                    if (e.Modifiers == (Keys.Shift | Keys.Control))
                    {
                        PathCopy(GetFullPathFromSelectList());
                    }
                    else if (e.Modifiers == Keys.Control)
                    {
                        Copy(GetFullPathFromSelectList());
                    }
                    break;

                case Keys.X:
                    // 切り取り
                    if (e.Modifiers == Keys.Control)
                    {
                        MoveCut(GetFullPathFromSelectList());
                    }
                    break;

                case Keys.V:
                    // 貼り付け
                    if (e.Modifiers == Keys.Control)
                    {
                        Paste();
                    }
                    break;
            }

        }

        #endregion

        #region 【右クリックメニュー】

        #region ツリー右クリックメニュークリックイベント(第一階層)

        /// <summary>
        /// ツリー右クリックメニュークリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            contextMenuClick(e.ClickedItem.Text, GetFullPathFromSelectNode());
        }

        #endregion

        #region リスト右クリックメニュークリックイベント(第一階層)

        /// <summary>
        /// リスト右クリックメニュークリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            contextMenuClick(e.ClickedItem.Text, GetFullPathFromSelectList());
        }

        #endregion

        #region ファイル新規作成

        /// <summary>
        /// ファイル新規作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Create_File(object sender, EventArgs e)
        {
            int i = 0;
            while (File.Exists(Path.Combine(p_SelectAddress, i.ToString())))
            {
                i++;
            }

            using (StreamWriter stWriter = new StreamWriter(Path.Combine(p_SelectAddress, i.ToString())))
            {
                // 出力しなくてもファイルは作られるようだ
                //File.Create(Path.Combine(GetFullPathFromSelectNode(),i.ToString()));
                //stWriter.Write(new byte[0]);
            }

            ListViewItem newFile = new ListViewItem(i.ToString());
            listViewMain.Items.Add(newFile);
            listViewMain.LabelEdit = true;
            newFile.BeginEdit();

            m_contextEnterFlg = true;
        }

        #endregion

        #region フォルダ新規作成

        /// <summary>
        /// フォルダ新規作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Create_Folder(object sender, EventArgs e)
        {
            m_contextEnterFlg = true;

            treeViewMain.SelectedNode.Expand();
            Form3 FolderNameWindow = new Form3();

            string strFolderName = string.Empty;
            while (true)
            {
                FolderNameWindow.Text = "フォルダ名を入力してください。";
                if (FolderNameWindow.ShowDialog() == DialogResult.OK)
                {
                    strFolderName = GetDirectoryText(Path.Combine(p_SelectAddress, FolderNameWindow.p_ResultText));
                    if (Directory.Exists(strFolderName))
                    {
                        MessageBox.Show("既に存在するフォルダです。別の名前を指定してください。");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            if (FolderNameWindow.DialogResult == DialogResult.OK)
            {
                Directory.CreateDirectory(strFolderName);

                TreeNode newDir = new TreeNode(FolderNameWindow.p_ResultText);
                AddSingleNode(treeViewMain.SelectedNode.Nodes, strFolderName);
            }

            FolderNameWindow.Close();

            treeviewUpdate();
        }

        #endregion

        #region ユーザメニュー選択イベント

        /*
        private void CustomMenuSelectedFromList(object sender, EventArgs e)
        {
            ProcessStart(m_ContextExecList[sender.ToString()], ConvertListToString(GetFullPathFromSelectList()));
        }

        private void CustomMenuSelectedFromTree(object sender, EventArgs e)
        {
            ProcessStart(m_ContextExecList[sender.ToString()], p_SelectAddress);
        }
        */

        private void CustomMenuSelectedFromList_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            m_contextEnterFlg = true;
            ProcessStart(m_ContextExecList[e.ClickedItem.Text], ConvertListToString(GetFullPathFromSelectList()));
        }
        private void CustomMenuSelectedFromList_SearchMenuItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ProcessStart(m_ContextExecList[((ToolStripTextBox)sender).Text], ConvertListToString(GetFullPathFromSelectList()));
                ((ToolStripTextBox)sender).Text = string.Empty;
            }
        }

        private void CustomMenuSelectedFromTree_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            m_contextEnterFlg = true;
            ProcessStart(m_ContextExecList[e.ClickedItem.Text], p_SelectAddress);
        }
        private void CustomMenuSelectedFromTree_SearchMenuItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ((ToolStripTextBox)sender).Select(((ToolStripTextBox)sender).Text.Length, 0);
                ProcessStart(m_ContextExecList[((ToolStripTextBox)sender).Text], p_SelectAddress);
            }
        }

        #endregion

        #region 指定ノードからタブを開く

        public TabPage OpenTab()
        {
            var selectNode = GetFullPathFromSelectNode();
            if (selectNode.Count != 0)
            {
                return m_parentForm.AddTab(Path.GetDirectoryName(selectNode[0]));
            }
            else
            {
                return m_parentForm.AddTab();
            }
        }

        #endregion

        #region タブを複製する

        public TabPage createReplicaTab()
        {
            if (p_RootAddress != "")
            {
                return m_parentForm.AddTab(p_RootAddress);
            }
            else
            {
                return m_parentForm.AddTab();
            }
        }

        #endregion

        #endregion

        #endregion

        // -----------------------------------------------------------------【Private Method】

        #region プライベートメソッド

        #region 渡された文字列の末尾に\がついていた場合は、削除して返す

        private string GetFileText(string strAddress)
        {
            if (strAddress.Last() == '\\' && strAddress.Length > 3)
            {
                strAddress = strAddress.Substring(0, strAddress.Length - 1);
            }

            return strAddress;
        }

        #endregion

        #region 渡された文字列の末尾に\がついていない場合は、\を付加して返す

        private string GetDirectoryText(string strAddress)
        {
            if (strAddress.Last() != '\\')
            {
                strAddress = strAddress + "\\";
            }

            return strAddress;
        }

        #endregion


        #region ツリーノード描画

        #region ルートノードからツリーを描画

        /// <summary>
        /// ルートツリー描画
        /// </summary>
        private void DrawTree()
        {
            if (p_RootAddress != string.Empty)
            {
                try
                {
                    if (new Uri(p_RootAddress).IsUnc)
                    // && new Microsoft.VisualBasic.Devices.Network().Ping(new Uri(txtAddress.Text).Host) Ping確認をする場合
                    {
                        try
                        {
                            Directory.GetDirectories(p_RootAddress);
                        }
                        catch (IOException ex)
                        {
                            // ((System.Exception)(ex)).HResult == -2147023570　としたいが、アクセスできないプロパティ
                            if (ex.Message == "ログオン失敗: ユーザー名を認識できないか、またはパスワードが間違っています。\r\n")
                            {
                                // エクスプローラ開いて認証（WaitForExitで待とうとしたが、対象プロセスは起動後即終了しているようだ）
                                Process tmpProcess = new Process();
                                tmpProcess.StartInfo.FileName = "explorer";
                                tmpProcess.StartInfo.Arguments = "\\\\" + new Uri(p_RootAddress).Host;
                                tmpProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                tmpProcess.Start();

                                MessageBox.Show("認証ウインドウで認証後、OKボタンを押してください",
                                    tmpProcess.StartInfo.Arguments + "認証", MessageBoxButtons.OK);
                                tmpProcess.Close();
                            }
                        }
                    }

                    if (Directory.Exists(GetDirectoryText(p_RootAddress)))
                    {
                        // タブ文字列も描画
                        string tabText = p_RootAddress;
                        if (tabText.Length > 3)
                        {
                            tabText = Path.GetFileName(p_RootAddress);
                            if (tabText.Length > 15)
                            {
                                tabText = tabText.Substring(0, 15) + "...";
                            }
                        }
                        ((TabPage)this.Parent).Text = tabText;

                        // ---
                        treeViewMain.Nodes.Clear();
                        AddSingleNode(treeViewMain.Nodes, p_RootAddress);
                        treeViewMain.Nodes[0].Text = p_RootAddress;
                        treeViewMain.SelectedNode = treeViewMain.Nodes[0];
                    }
                }
                catch (Exception ex)
                {
                    // TODO:パス不正の場合を握り潰し
                }
            }
        }

        #endregion

        #region ノード追加

        /// <summary>
        /// 指定ノード以下全てのノードを作成(ExpandAllで描画できてしまうのでいらないか？)
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="FolderPath"></param>
        /// <param name="DirectoryOnly"></param>
        private void AddAllNode(TreeNodeCollection Nodes, string FolderPath, bool DirectoryOnly)
        {
            TreeNode Node = null;

            // アクセス権ない場合はノードを追加したくないため、ここでDirectoryを取得してみる
            string[] Dirs = Directory.GetDirectories(FolderPath);
            Node = Nodes.Add(Path.GetFileName(FolderPath));
            Node.ImageIndex = 0;
            Node.SelectedImageIndex = 1;
            foreach (string NextDirectory in Dirs)
            {
                AddAllNode(Node.Nodes, NextDirectory, DirectoryOnly);
            }

            if (!DirectoryOnly)
            {
                foreach (string TargetFile in Directory.GetFiles(FolderPath))
                {
                    TreeNode NodeFile = Node.Nodes.Add(Path.GetFileName(TargetFile));
                    NodeFile.ImageIndex = 2;
                    NodeFile.SelectedImageIndex = 2;
                }
            }
        }

        /// <summary>
        /// 指定ノード以下一階層のノードを作成
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="FolderPath"></param>
        /// <param name="DirectoryOnly"></param>
        private void AddSingleNode(TreeNodeCollection Nodes, string FolderPath)
        {
            try
            {
                TreeNode Node = null;

                // アクセス権ない場合はノードを追加したくないため、ここでDirectoryを取得してみる
                string[] Dirs = Directory.GetDirectories(FolderPath);
                Node = Nodes.Add(GetDirectoryText(FolderPath), Path.GetFileName(GetFileText(FolderPath)));
                Node.ImageIndex = 0;
                Node.SelectedImageIndex = 1;

                foreach (string NextDirectory in Dirs)
                {
                    Node.Nodes.Add(Path.GetFileName(NextDirectory));
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 指定フォルダ内のファイルを指定ノードに追加する
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="FolderPath"></param>
        private void AddFileNode(TreeNodeCollection Nodes, string FolderPath)
        {
            foreach (string TargetFile in Directory.GetFiles(FolderPath))
            {
                TreeNode NodeFile = Nodes.Add(Path.GetFileName(TargetFile));
                NodeFile.ImageIndex = 2;
                NodeFile.SelectedImageIndex = 2;
            }
        }

        #endregion

        #endregion

        #region 貼り付け後ツリー描画

        private void AfterPasteTreeDraw(CopyPasteMoveManager.OperationResult PasteResult)
        {
            if (PasteResult.p_SrcTargetFiles.Count > 0 && PasteResult.p_SrcTargetFiles[0] != string.Empty)
            {
                if (PasteResult.p_OperationType == CopyPasteMoveManager.OperationType.Move)
                {
                    // 切り取り貼り付けだったらツリーから削除
                    foreach (string DeletePath in PasteResult.p_SrcTargetFiles)
                    {
                        TreeNode[] DelNode = treeViewMain.Nodes.Find(GetDirectoryText(DeletePath), true);
                        if (DelNode != null && DelNode.Length != 0)
                        {
                            DelNode.First().Remove();
                        }
                    }

                    m_MessageManager.ToMDIMsg(string.Empty);
                }

                if (treeViewMain.SelectedNode.Nodes.Count > 0)
                {
                    treeviewUpdate();
                }
                else if (PasteResult.p_DstDirectory != string.Empty && Directory.Exists(PasteResult.p_SrcTargetFiles[0]))
                {
                    foreach (var dir in PasteResult.p_SrcTargetFiles)
                    {
                        AddSingleNode(
                            treeViewMain.SelectedNode.Nodes,
                            Path.Combine(p_SelectAddress, Path.GetFileName(Path.GetDirectoryName(dir))));
                    }
                }
            }
        }

        #endregion

        #region 選択ノードの背景色設定

        /// <summary>
        /// 選択ノードの背景色設定
        /// </summary>
        /// <param name="setColor"></param>
        private void SetNodeBackColor(Color setColor)
        {
            if (treeViewMain.SelectedNode != null)
            {
                treeViewMain.SelectedNode.BackColor = setColor;
            }
        }

        #endregion

        #region リスト更新

        /// <summary>
        /// リスト更新 TODO:修正要
        /// </summary>
        public void ListViewUpdate()
        {
            listViewMain.BeginUpdate();
            listViewMain.Items.Clear();

            string Target = p_SelectAddress;

            if (Directory.Exists(Target))
            {
                try
                {
                    List<string> Files = new List<string>();
                    List<Size> SizeList = new List<Size>();

                    var imgList = new ImageList();
                    //for (int i = 0; i <= 1; i++)
                    //{
                    //    m_imgList.Add(new ImageList());
                    //    // TODO: TrackMaxCountを9以下に設定した場合、画像が小さくなってしまうため、以下の処理をコメント
                    //    // 9に設定した場合は必要
                    //    //m_imgList[i].ImageSize = new Size(ThumbnailMaxHeight / (trackBar1.Maximum - i + 1), ThumbnailMaxWidth / (trackBar1.Maximum - i + 1));
                    //    SizeList.Add(m_imgList[i].ImageSize);
                    //}

                    ImageConverter imgConv = new ImageConverter();
                    if (Target.Last() != '\\')
                    {
                        Target += '\\';
                    }
                    foreach (string strFileName in Directory.GetFiles(Target))
                    {
                        FileInfo tmpFile = new FileInfo(strFileName);
                        string[] tmpItem = {
                                           Path.GetFileName(strFileName),
                                           tmpFile.LastWriteTime.ToShortDateString() + " " + tmpFile.LastWriteTime.ToShortTimeString(),
                                           tmpFile.CreationTime.ToShortDateString() + " " + tmpFile.CreationTime.ToShortTimeString(),
                                           tmpFile.Extension,
                                           tmpFile.Length.ToString()
                                       };

                        switch (Program.ICON_SETTING)
                        {

                            case Program.ListViewIconSetting.none:
                                listViewMain.Items.Add(new ListViewItem(tmpItem));
                                break;

                            case Program.ListViewIconSetting.icon:
                                //// 拡張子同一 = アイコン同じというロジック
                                //if (imgList.Images.Keys.Contains(tmpFile.Extension))
                                //{
                                //}
                                //else
                                //{
                                //    imgList.Images.Add(tmpFile.Extension, IconUtil.getIconFromFilePath(strFileName).ToBitmap());
                                //}
                                //listViewMain.Items.Add(new ListViewItem(tmpItem, imgList.Images.IndexOfKey(tmpFile.Extension)));
                                //break;

                                // 全て取得
                                imgList.Images.Add(tmpFile.Extension, IconUtil.getIconFromFilePath(strFileName));
                                listViewMain.Items.Add(new ListViewItem(tmpItem, imgList.Images.Count - 1));
                                break;
                        }

                        //// 画像サムネイル設定
                        //string strExtention = Path.GetExtension(strFileName).ToUpper();
                        ////if (大きいアイコンToolStripMenuItem.Checked &&
                        ////    strExtention == ".JPG" || strExtention == ".BMP" || strExtention == ".GIF" || strExtention == ".PNG")
                        ////{
                        ////    // falseオプションでかなり高速化した
                        ////    Image orig = Image.FromStream(new FileStream(strFileName, FileMode.Open, FileAccess.Read), false, false);
                        ////    Image thumbnail = orig.GetThumbnailImage(ThumbnailMaxHeight, ThumbnailMaxWidth, delegate { return false; }, IntPtr.Zero);

                        ////    for (int i = 0; i <= trackBar1.Maximum; i++)
                        ////    {
                        ////        m_imgList[i].Images.Add(thumbnail);
                        ////    }

                        ////    listViewMain.Items.Add(new ListViewItem(tmpItem, m_imgList[0].Images.Count - 1));
                        ////}
                        ////else
                        ////{

                        //// ----------------------------
                        //// UNCパスはアイコン取得できないためチェック
                        //Uri CheckUnc = new Uri(strFileName);
                        //if (CheckUnc.IsUnc)
                        //{
                        //    listViewMain.Items.Add(new ListViewItem(tmpItem));
                        //}
                        //else
                        //{
                        //    #region 拡張子を判別してアイコンを設定
                        //    if (m_imgList[0].Images.Keys.Contains(tmpFile.Extension))
                        //    {
                        //    }
                        //    else
                        //    {
                        //        Icon tmpIcon = Icon.ExtractAssociatedIcon(strFileName);
                        //        for (int i = 0; i <= 1; i++)
                        //        {
                        //            m_imgList[i].Images.Add(tmpFile.Extension, tmpIcon);
                        //        }
                        //    }
                        //    listViewMain.Items.Add(new ListViewItem(tmpItem, m_imgList[0].Images.IndexOfKey(tmpFile.Extension)));
                        //    //listViewMain.Items.Add(new ListViewItem(tmpItem, count++));
                        //    Files.Add(strFileName);
                        //    #endregion

                        //    #region 対応するアイコンをそれぞれ取得する場合
                        //    //Icon tmpIcon = Icon.ExtractAssociatedIcon(strFileName);
                        //    //for (int i = 0; i <= trackBar1.Maximum; i++)
                        //    //{
                        //    //    m_imgList[i].Images.Add(tmpFile.Extension, tmpIcon);
                        //    //}

                        //    //listViewMain.Items.Add(new ListViewItem(tmpItem, m_imgList[0].Images.Count-1));
                        //    #endregion
                        //}
                        //// ----------------------------

                        ////}

                        ////listViewMain.Items.Add(new ListViewItem(tmpItem));
                    }

                    listViewMain.LargeImageList = imgList;
                    listViewMain.SmallImageList = imgList;

                    #region 別スレッドでサムネイル作成を想定
                    //m_Process = new DrawThumbnailThread(txtSelectTreeNode.Text, Files);
                    //m_Thread = new Thread(new ThreadStart(m_Process.Main));
                    //m_Thread.Start();
                    #endregion

                }
                catch (UnauthorizedAccessException)
                {
                }

                // 列幅自動調整
                listViewMain.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            }

            listViewMain.EndUpdate();

            SetNodeBackColor(Color.Silver);
        }

        #endregion


        #region 選択ノードから絶対パスを取得

        /// <summary>
        /// 選択されているノードから絶対パスを取得する
        /// ツリー選択後の処理では、txtSelectTreeNode.Textから取得する
        /// </summary>
        /// <returns></returns>
        private List<string> GetFullPathFromSelectNode()
        {
            List<string> RtObj = new List<string>();

            if (treeViewMain.SelectedNode == null)
            {
                if (treeViewMain.TopNode == null)
                {
                    return RtObj;
                }
                else
                {
                    RtObj.Add(treeViewMain.TopNode.Text);
                    return RtObj;
                }
            }

            string strPath = treeViewMain.SelectedNode.FullPath;

            if (treeViewMain.SelectedNode.ImageIndex == 0 || treeViewMain.SelectedNode.ImageIndex == 1)
            {
                strPath = GetDirectoryText(strPath);
            }

            if (strPath.First() == '\\')
            {
                // UNCパス用の置換防止
                strPath = "\\" + strPath;
            }

            RtObj.Add(strPath.Replace("\\\\", "\\"));
            return RtObj;
        }

        #endregion

        #region 選択リストから絶対パスを取得

        /// <summary>
        /// 選択されているリストから絶対パスを取得する
        /// </summary>
        /// <returns></returns>
        private List<string> GetFullPathFromSelectList()
        {
            List<string> colRt = new List<string>();

            if (listViewMain.SelectedItems.Count > 0 && listViewMain.SelectedItems[0].Text != null && listViewMain.SelectedItems[0].Text != string.Empty)
            {
                foreach (ListViewItem tmpItem in listViewMain.SelectedItems)
                {
                    colRt.Add(Path.Combine(p_SelectAddress, tmpItem.Text));
                }
            }
            else
            {
                colRt.Add(string.Empty);
            }

            return colRt;
        }

        #endregion

        #region 文字列のコレクションを、半角スペース区切りの文字列へ変換

        private string ConvertListToString(List<string> colList)
        {
            string strRt = string.Empty;

            foreach (string tmpItem in colList)
            {
                if (strRt != string.Empty)
                {
                    strRt = strRt + " ";
                }

                strRt = strRt + "\"" + tmpItem + "\"";
            }

            return strRt;
        }

        #endregion

        #region 外部プロセス起動

        /// <summary>
        /// 外部プロセス開始
        /// </summary>
        /// <param name="TargetPath"></param>
        private void ProcessStart(string TargetPath)
        {
            ProcessStart(TargetPath, string.Empty);
        }

        /// <summary>
        /// 外部プロセス開始（引数有り）
        /// </summary>
        /// <param name="TargetPath"></param>
        /// <param name="strArgument"></param>
        private void ProcessStart(string TargetPath, string strArgument)
        {
            if (TargetPath != string.Empty)
            {
                Process ExecProcess = new Process();
                ExecProcess.StartInfo.FileName = TargetPath;
                ExecProcess.StartInfo.Arguments = strArgument;
                // 実行ディレクトリを対象ファイルと同じディレクトリに設定
                ExecProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(TargetPath);
                ExecProcess.Start();
            }
        }

        #endregion


        #region 削除処理

        private void DeleteTargetItem(List<string> colTarget)
        {
            if (DialogResult.OK == MessageBox.Show("選択されたデータをすべて削除します。よろしいですか？", "削除確認", MessageBoxButtons.OKCancel))
            {

                if (treeViewMain.ContainsFocus)
                {
                    foreach (string strTarget in colTarget)
                    {
                        try
                        {
                            // 普通のエクスプローラのように一括で処理したいが一個ずつやるメソッドしかない。。。
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(GetFileText(strTarget), Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin, Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                            TreeNode[] DelNode = treeViewMain.Nodes.Find(GetDirectoryText(strTarget), true);
                            if (DelNode != null && DelNode.Length != 0)
                            {
                                DelNode.First().Remove();
                            }
                        }
                        catch (OperationCanceledException)
                        {
                        }
                    }
                }
                else if (listViewMain.ContainsFocus)
                {
                    foreach (string strTarget in colTarget)
                    {
                        try
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(strTarget, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin, Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                            ListViewUpdate();
                        }
                        catch (OperationCanceledException)
                        {
                        }
                    }
                }
            }

        }

        #endregion

        #region 名前変更開始

        private void StartRename()
        {
            // ToDo:変更前にフォルダ情報の最新化、変更後に情報の最新化を行う。
            if (treeViewMain.ContainsFocus)
            {
                treeViewMain.LabelEdit = true;
                treeViewMain.SelectedNode.BeginEdit();
            }
            else if (listViewMain.ContainsFocus)
            {
                listViewMain.LabelEdit = true;
                listViewMain.SelectedItems[0].BeginEdit();
            }
        }

        #endregion


        #region 右クリックメニュー（第一階層）実行処理

        private void contextMenuClick(string strItemText, string strTarget)
        {
            List<String> colTarget = new List<String>();
            colTarget.Add(strTarget);
            contextMenuClick(strItemText, colTarget);
        }

        /// <summary>
        /// 右クリックメニュー処理
        /// </summary>
        /// <param name="strItemText"></param>
        /// <param name="strTarget"></param>
        private void contextMenuClick(string strItemText, List<string> colTarget)
        {
            if (strItemText != string.Empty && colTarget != null)// && colTarget.Count > 0 && colTarget[0] != string.Empty)
            {
                switch (strItemText)
                {
                    case C_ContextMenuStripItem_property:
                        Property(colTarget);
                        break;

                    case C_ContextMenuStripItem_PathCopy:
                        PathCopy(colTarget);
                        break;

                    case C_ContextMenuStripItem_OpenFile:
                        Exec(colTarget);
                        break;

                    case C_ContextMenuStripItem_OpenDirectory:
                        ProcessStart(p_SelectAddress);
                        break;

                    case C_ContextMenuStripItem_NewTabOpen:
                        OpenTab();
                        break;

                    case C_ContextMenuStripItem_CurrentTabOpen:
                        if (CheckTarget(colTarget))
                        {
                            txtAddress.Text = txtBoxBeautifulShow(GetFileText(colTarget[0]));
                            DrawTree();
                            txtSelectTreeNode.Text = C_Root;
                        }
                        else
                        {
                            ShowWarning("ノードが選択されていません");
                        }
                        break;

                    case C_ContextMenuStripItem_Delete:
                        Delete(colTarget);
                        break;

                    case C_ContextMenuStripItem_Rename:
                        Rename(colTarget);
                        break;

                    case C_ContextMenuStripItem_Copy:
                        Copy(colTarget);
                        break;

                    case C_ContextMenuStripItem_Move:
                        MoveCut(colTarget);
                        break;

                    case C_ContextMenuStripItem_Paste:
                        Paste();
                        break;

                    default:
                        break;
                }
            }
        }

        #endregion

        #region プロパティ

        private void Property(List<String> colTarget)
        {
            if (CheckTarget(colTarget))
            {
                ShowProperty(colTarget[0]);
            }
            else
            {
                ShowProperty(p_SelectAddress);
            }
        }

        #endregion

        #region パスコピー
        private void PathCopy(List<String> colTarget)
        {
            if (CheckTarget(colTarget))
            {
                // エラーすることあるのでリトライ対応
                var count = 0;
                while (count < 5)
                {
                    try
                    {
                        Clipboard.SetText(colTarget.First(), TextDataFormat.Text);
                        break;
                    }
                    catch
                    {
                        count++;
                    }
                }
            }
            else
            {
                ShowWarning("パスコピー対象が選択されていません");
            }
        }

        #endregion

        #region 実行
        private void Exec(List<String> colTarget)
        {
            if (CheckTarget(colTarget))
            {
                ProcessStart(colTarget[0]);
            }
            else
            {
                ShowWarning("実行対象が選択されていません");
            }
        }
        #endregion

        #region 名前変更
        private void Rename(List<string> colTarget)
        {
            if (CheckTarget(colTarget))
            {
                StartRename();
            }
            else
            {
                ShowWarning("リネーム対象が選択されていません");
            }
        }
        #endregion

        #region 削除
        private void Delete(List<string> colTarget)
        {
            if (CheckTarget(colTarget))
            {
                DeleteTargetItem(colTarget);
            }
            else
            {
                ShowWarning("削除対象が選択されていません");
            }
        }
        #endregion

        #region コピー
        private void Copy(List<string> colTarget)
        {
            if (CheckTarget(colTarget))
            {
                m_MessageManager.ToMDIMsg(m_CopyPasteMoveManager.Copy(colTarget));
            }
            else
            {
                ShowWarning("コピー対象が選択されていません");
            }
        }
        #endregion

        #region 切り取り
        private void MoveCut(List<string> colTarget)
        {
            if (CheckTarget(colTarget))
            {
                m_MessageManager.ToMDIMsg(m_CopyPasteMoveManager.Move(colTarget));
            }
            else
            {
                ShowWarning("移動対象が選択されていません");
            }
        }
        #endregion

        #region 貼り付け

        private void Paste()
        {
            // 再描画
            AfterPasteTreeDraw(m_CopyPasteMoveManager.Paste(p_SelectAddress));
            ListViewUpdate();
        }
        #endregion

        #region 処理対象存在チェック

        /// <summary>
        /// 処理対象存在チェック
        /// </summary>
        /// <param name="colTarget"></param>
        /// <returns></returns>
        private bool CheckTarget(List<string> colTarget)
        {
            if (colTarget != null
                && colTarget.Count > 0
                && colTarget.First() != string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 警告ダイアログ表示

        /// <summary>
        /// 警告ダイアログ表示
        /// </summary>
        /// <param name="message"></param>
        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "確認してください", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        #endregion


        #endregion

        #region Shell32.dll functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shinfo"></param>
        /// <returns></returns>
        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        public static extern int ShellExecuteEx(SHELLEXECUTEINFO shinfo);

        /// <summary>
        /// プロパティ表示
        /// </summary>
        /// <param name="strPath"></param>
        private void ShowProperty(string strPath)
        {
            SHELLEXECUTEINFO shinfo = new SHELLEXECUTEINFO();
            shinfo.cbSize = Marshal.SizeOf(typeof(SHELLEXECUTEINFO));
            shinfo.fMask = 0x0000000c; ;
            shinfo.lpVerb = "Properties";
            shinfo.hwnd = (int)this.Handle;
            shinfo.lpParameters = null;
            shinfo.lpDirectory = null;
            shinfo.lpFile = strPath;
            ShellExecuteEx(shinfo);
        }

        #endregion

        private void txtSelectTreeNode_Click(object sender, EventArgs e)
        {
            txtSelectTreeNode.SelectAll();
        }

        private void contextMenuListViewMain_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            MessageBox.Show("test");
        }

        private bool m_contextEnterFlg;
        private void contextMenuListViewMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                m_contextEnterFlg = true;
            }
        }

        private void contextMenuTreeViewMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                m_contextEnterFlg = true;
            }
        }

        private void listViewMain_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // 参考：http://www.atmarkit.co.jp/fdotnet/dotnettips/384expdragdrop/expdragdrop.html

            DataObject dataObj = new DataObject(DataFormats.FileDrop, GetFullPathFromSelectList().ToArray());
            DragDropEffects effect = DragDropEffects.All; //DragDropEffects.Copy | DragDropEffects.Move;
            listViewMain.DoDragDrop(dataObj, effect);
            ListViewUpdate();
        }

        private void listViewMain_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void listViewMain_DragDrop(object sender, DragEventArgs e)
        {
            // ドロップされるのはエクスプローラからしかないはず。
            // 参考：http://www.atmarkit.co.jp/fdotnet/csharptips/003dragdrop/003dragdrop.html
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                List<string> colTarget = new List<string>(); ;
                foreach (string fileName in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    if (p_SelectAddress != Path.GetDirectoryName(fileName))
                    {
                        colTarget.Add(fileName);
                    }
                }

                if (colTarget.Count > 0)
                {
                    m_CopyPasteMoveManager.Move(colTarget);
                    m_CopyPasteMoveManager.Paste(p_SelectAddress);
                    treeviewUpdate();
                    ListViewUpdate();
                }
            }
        }

        private void treeViewMain_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DataObject dataObj = new DataObject(DataFormats.FileDrop, GetFullPathFromSelectNode().ToArray());
            DragDropEffects effect = DragDropEffects.All; //DragDropEffects.Copy | DragDropEffects.Move;
            listViewMain.DoDragDrop(dataObj, effect);
            treeviewUpdate();
        }


        private string m_txtAddressEnterValue;

        private void txtAddress_Leave(object sender, EventArgs e)
        {
            if (p_RootAddress != m_txtAddressEnterValue && Directory.Exists(p_RootAddress))
            {
                DrawTree();
                txtSelectTreeNode.Text = C_Root;
            }

            txtAddress.AutoCompleteMode = AutoCompleteMode.None;
            txtAddress.Text = txtBoxBeautifulShow(txtAddress.Text);
        }

        private void txtAddress_Enter(object sender, EventArgs e)
        {
            m_txtAddressEnterValue = p_RootAddress;

            //↓この一文によって　Ctrl+A 2回目押されるとなぜか値消える。。selectの方は問題ないのに。。ま、気にしないどくか。
            txtAddress.Text = p_RootAddress; //stopBeautifulShow(txtAddress.Text);
            txtAddress.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtAddress.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
            txtAddress.SelectAll();
        }

        private void txtSelectTreeNode_Leave(object sender, EventArgs e)
        {
            txtSelectTreeNode.Text = txtSelectBeautifulShow(txtSelectTreeNode.Text);
        }

        private void txtSelectTreeNode_Enter(object sender, EventArgs e)
        {
            txtSelectTreeNode.Text = p_SelectAddress; //stopBeautifulShow(txtSelectTreeNode.Text);
            txtSelectTreeNode.SelectAll();
        }

        private string txtBoxBeautifulShow(String pathArg)
        {
            var path = pathArg;
            if (path.Length > 0)
            {
                if (path[path.Length - 1] == (char)'\\')
                {
                    path = path.Remove(path.Length - 1, 1);
                }
                path = path.Replace(@"\\", @";placefolder");
                path = path.Replace(@"\", @" → ");
                path = path.Replace(@";placefolder", @"\\");
            }

            return path;
        }

        private string txtSelectBeautifulShow(String path)
        {
            var rtPath = txtBoxBeautifulShow(path);
            if (rtPath != string.Empty)
            {
                rtPath = rtPath.Replace(txtAddress.Text, C_Root);
            }
            return rtPath;
        }

        public string stopBeautifulShow(String path)
        {
            return path.Replace(@" → ", @"\");
        }

        private void txtSelectTreeNode_Click_1(object sender, EventArgs e)
        {
            txtSelectTreeNode.SelectAll();
        }

        private void UserControl1_Enter(object sender, EventArgs e)
        {
            treeviewUpdate();
            ListViewUpdate();
        }

        private void listViewMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_contextEnterFlg = false;
        }

    }

    // -----------------------------

    #region ソート用クラス

    class ListViewItemComparer : System.Collections.IComparer
    {
        private int col;
        private int sortOrder;

        // コンストラクタ
        public ListViewItemComparer(int col, int sortOrder)
        {
            this.col = col;
            this.sortOrder = sortOrder;
        }
        // 比較メソッド
        public int Compare(object x, object y)
        {
            int ret = 0;

            //ListViewItemの取得
            ListViewItem itemx = (ListViewItem)x;
            ListViewItem itemy = (ListViewItem)y;

            switch (col)
            {
                case 1:
                case 2:
                    ret = DateTime.Compare(
                        DateTime.Parse(itemx.SubItems[col].Text), DateTime.Parse(itemy.SubItems[col].Text));
                    break;

                case 4:    // サイズ列は数値でソート
                    ret = int.Parse(itemx.SubItems[col].Text).CompareTo(int.Parse(itemy.SubItems[col].Text));
                    //ret = int.Parse(itemy.SubItems[col].Text) - int.Parse(itemx.SubItems[col].Text);
                    //ret = int.Parse(((ListViewItem)x).SubItems[col].Text) <
                    //   int.Parse(((ListViewItem)y).SubItems[col].Text) ? -1 : 1;
                    break;

                default:    // 文字列でソート
                    ret = string.Compare(
                        itemx.SubItems[col].Text, itemy.SubItems[col].Text);
                    break;
            }

            //xとyを文字列として比較する
            //ret = string.Compare(itemx.SubItems[col].Text,
            //    itemy.SubItems[col].Text);
            return ret * sortOrder;
        }
    }

    #endregion

    #region プロパティ画面表示用クラス

    // ---------------------
    [StructLayout(LayoutKind.Sequential)]
    public class SHELLEXECUTEINFO
    {
        public int cbSize;
        public int fMask;
        public int hwnd;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpVerb;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpFile;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpParameters;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpDirectory;
        public int nShow;
        public int hInstApp;
        public int lpIDList;
        public string lpClass;
        public int hkeyClass;
        public int dwHotKey;
        public int hIcon;
        public int hProcess;
    }

    #endregion


    #region 別スレッドでサムネイル作成処理を行うとした

    public class DrawThumbnailThread
    {
        // ユーザコントロールの
        //　タイマーで以下のような処理が必要
        //Image[] addImage = m_Process.GetImageList;
        //foreach (ImageList tmpImageList in m_imgList)
        //{
        //    tmpImageList.Images.AddRange(addImage);
        //}


        const int ThumbnailMaxHeight = 160;
        const int ThumbnailMaxWidth = 160;

        private List<string> m_strFiles = null;
        private List<Image> m_Images = null;
        private string m_DirPath = null;

        private bool m_StopFlg = false;

        //public DrawThumbnailThread(string DirPath, List<string> strFiles, List<ImageList> Images ,int ImageCount)
        public DrawThumbnailThread(string DirPath, List<string> strFiles)
        {
            m_DirPath = DirPath;
            m_strFiles = strFiles;
            m_Images = new List<Image>();
        }

        public void Main()
        {
            //foreach(string tmpItem in m_strFiles)
            //{
            //    string strExtention = Path.GetExtension(tmpItem);
            //    if (strExtention == ".JPG" || strExtention == ".BMP" || strExtention == ".GIF" || strExtention == ".PNG")
            //    {
            //        // falseオプションでかなり高速化した
            //        Image orig = Image.FromStream(new FileStream(Path.Combine(m_DirPath, tmpItem), FileMode.Open, FileAccess.Read), false, false);
            //        Image thumbnail = orig.GetThumbnailImage(ThumbnailMaxHeight, ThumbnailMaxWidth, delegate { return false; }, IntPtr.Zero);
            //        lock (m_Images)
            //        {
            //            m_Images.Add(thumbnail);
            //        }
            //    }
            //    else
            //    {
            //        Icon tmpIcon = Icon.ExtractAssociatedIcon(Path.Combine(m_DirPath, tmpItem));
            //        lock (m_Images)
            //        {
            //            m_Images.Add(tmpIcon.ToBitmap());
            //        }
            //    }
            //}
            //while (!m_StopFlg)
            //{
            //    Thread.Sleep(100);
            //}
        }

        public void RequestStop()
        {
            m_StopFlg = true;
        }

        public Image[] GetImageList
        {
            get
            {
                lock (m_Images)
                {
                    List<Image> RtObj = new List<Image>(); ;
                    foreach (Image tmpImage in m_Images)
                    {
                        #region シリアライズ
                        ////ImageListはシリアライズできない
                        //using (MemoryStream stream = new MemoryStream())
                        //{
                        //    // バイナリシリアル化を行うためのフォーマッタを作成
                        //    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter f =
                        //        new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                        //    // 現在のインスタンスをシリアル化してMemoryStreamに格納
                        //    f.Serialize(stream, tmpImage);

                        //    // ストリームの位置を先頭に戻す
                        //    stream.Position = 0L;

                        //    // MemoryStreamに格納されている内容を逆シリアル化する
                        //    Image newImage = (Image)f.Deserialize(stream);
                        //    for (int i = 0; i < m_SizeList.Count; i++)
                        //    {
                        //        RtObj.Add(newImage);
                        //    }
                        //}
                        #endregion

                        RtObj.Add(tmpImage);
                        m_Images = new List<Image>();
                    }

                    return RtObj.ToArray();
                }
            }
        }
    }

    #endregion
}
