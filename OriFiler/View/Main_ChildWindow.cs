using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Diagnostics;
using System.Collections.Specialized;

using System.Runtime.InteropServices;//for DLL

namespace MyWorkSpace
{
    /// <summary>
    /// メインホストフォーム
    /// </summary>
    public partial class Form1 : Form
    {
        // -----------------------------------------------------------------【Member Variable】

        #region メンバ変数

        /// <summary>
        /// ファイル操作オブジェクト
        /// </summary>
        private CopyPasteMoveManager m_CopyPasteMoveManager = null;

        /// <summary>
        /// 今回起動時の画面番号
        /// </summary>
        private string m_WindowNo = string.Empty;

        /// <summary>
        /// 前回起動時の画面番号
        /// </summary>
        private string m_BeforeWindowNo = string.Empty;

        /// <summary>
        /// メッセージ表示操作オブジェクト
        /// </summary>
        private Msio.MessageManager m_MessageManager = null;

        #endregion

        // -----------------------------------------------------------------【Constructer】

        #region コンストラクタ

        public Form1(string windowNo, string BeforeWindowNo, string strTabListFilePath, CopyPasteMoveManager objPasteMoveManager, Msio.MessageManager objMessageManager, Font font, Color color)
        {
            InitializeComponent();

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = font;

            // MDIを使用した場合に、親画面のメニューへマージされてしまうため、マージしないように設定
            menuStrip1.AllowMerge = false;

            // 引数をメンバに設定
            m_WindowNo = windowNo;
            m_BeforeWindowNo = BeforeWindowNo;
            m_CopyPasteMoveManager = objPasteMoveManager;
            m_MessageManager = objMessageManager;
            m_MessageManager.ToSplashMsg("ウインドウNo「 " + windowNo + " 」作成開始");

            // タブ初期化
            InitTab(Program.DATADIR + strTabListFilePath);

            ChangeColor(this, color);
        }

        #endregion

        // -----------------------------------------------------------------【Event Handler】

        #region 右クリックでもタブ選択できるよう、MouseDownイベントで処理している

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                //タブとマウス位置を比較し、クリックしたタブを選択
                if (tabControl1.GetTabRect(i).Contains(e.X, e.Y))
                {
                    tabControl1.SelectedTab = tabControl1.TabPages[i];
                    break;
                }
            }
        }

        #endregion

        #region 【メニュー】対象タブ削除イベント

        /// <summary>
        /// タブ閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseTab(object sender, EventArgs e)
        {
            CloseTabLogic();
        }

        private void CloseTabLogic()
        {
            var nextSelect = GetSelectedBeforeTabPageIndex();
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            if (nextSelect > 0)
            {
                tabControl1.SelectedTab = tabControl1.TabPages[nextSelect];
            }
        }

        #endregion

        #region 【メニュー】対象タブ以外全てのタブを削除イベント

        /// <summary>
        /// 選択タブ以外全て閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseTabSelectAll(object sender, EventArgs e)
        {
            CloseTabSelectAllLogic();
        }

        private void CloseTabSelectAllLogic()
        {
            while (tabControl1.TabPages.Count > 1)
            {
                if (tabControl1.TabPages[0] != tabControl1.SelectedTab)
                {
                    tabControl1.TabPages.Remove(tabControl1.TabPages[0]);
                }
                else
                {
                    tabControl1.TabPages.Remove(tabControl1.TabPages[1]);
                }
            }
        }

        #endregion

        #region 終了イベント

        private void ウインドウを閉じるToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        // -----------------------------------------------------------------【Public Method】

        #region フォント変更

        /// <summary>
        /// 表示フォント変更要求
        /// </summary>
        /// <param name="SetFont"></param>
        public void ChangeFont(Font SetFont, Color SetColor)
        {
            Form Backup = this.MdiParent;
            this.Hide();
            this.MdiParent = null;

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.menuStrip1.Font = SetFont;
            this.Font = SetFont;
            ChangeColor(this, SetColor);

            this.MdiParent = Backup;
            this.Refresh();
            this.Show();
        }

        /// <summary>
        /// 表示文字色変更
        /// </summary>
        /// <param name="ParentControl"></param>
        /// <param name="SetColor"></param>
        private void ChangeColor(Control ParentControl, Color SetColor)
        {
            foreach (Control tmpControl in ParentControl.Controls)
            {
                tmpControl.ForeColor = SetColor;
                ChangeColor(tmpControl, SetColor);
            }
        }

        #endregion

        public void updateListView()
        {
            if(tabControl1.TabCount > 0)
            {
                GetTargetUserControl(tabControl1.SelectedTab).ListViewUpdate();
            }
        }


        public void 色設定(Color 背景色)
        {
            this.BackColor = 背景色;
            menuStrip1.BackColor = 背景色;
        }

        // -----------------------------------------------------------------【Private Method】

        #region 初期化

        /// <summary>
        /// タブ初期化
        /// </summary>
        private void InitTab(string strFilePath)
        {
            contextMenuTab.Items.Add("このタブを複製する", null, create複製タブ);
            contextMenuTab.Items.Add(new ToolStripSeparator());
            contextMenuTab.Items.Add("このタブを閉じる", null, CloseTab);
            contextMenuTab.Items.Add("このタブ以外全て閉じる", null, CloseTabSelectAll);
            tabControl1.ContextMenuStrip = contextMenuTab;
            tabControl1.HotTrack = true;

            if (strFilePath != string.Empty)
            {
                AddTabFromFile(strFilePath);
            }

            tabControl1.Selected += tabControl1_Selected;
        }

        #endregion

        #region タブ追加


        public TabPage AddTab()
        {
            //return AddTab(string.Empty);
            return AddTab(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        public TabPage AddTab(string strTargetAddress)
        {
            UserControl1 tmpUserControl = new UserControl1(this, strTargetAddress, m_CopyPasteMoveManager, m_MessageManager);

            // タブ追加で崩れ起こさないためにこれが必要
            tmpUserControl.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            tmpUserControl.Font = this.Font;

            TabPage tmpTabPage = new TabPage(tabControl1.TabPages.Count.ToString());

            // タブ追加で崩れ起こさないためにこれが必要
            tmpUserControl.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            tmpTabPage.Font = this.Font;

            tmpTabPage.BorderStyle = BorderStyle.FixedSingle;
            tmpTabPage.Controls.Add(tmpUserControl);
            tabControl1.TabPages.Add(tmpTabPage);

            // MDI子フォームの場合、コントロール描画時にうまく動作しない、一度Show()を行う必要がある。Refresh()等だとうまく表示できない。
            tmpUserControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
            tmpUserControl.Size = GetLastBeforeTabPage().Size;

            tabControl1.SelectedTab = tmpTabPage;

            return tmpTabPage;
        }

        void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            // セレクト時更新したがったが、ドラッグ＆ドロップの実装と競合するため見送り
            //var targetPage = GetTargetUserControl(e.TabPage);
            //targetPage.treeviewUpdate();
            //targetPage.ListViewUpdate();
        }

        public void AddTabFromFile(string strPath)
        {
            if (File.Exists(strPath))
            {
                using (StreamReader stReader = new StreamReader(strPath, Encoding.UTF8))
                {
                    while (stReader.Peek() > 0)
                    {
                        AddTab(stReader.ReadLine());
                    }
                    tabControl1.SelectedTab = tabControl1.TabPages[0];
                }
            }
            else
            {
                AddTab();
            }
        }

        #endregion

        #region タブリスト保存

        public void SaveTabList(string strPath)
        {
            if (tabControl1.TabPages.Count > 0)
            {
                using (StreamWriter stwriter = new StreamWriter(Program.DATADIR + strPath, false, Encoding.UTF8))
                {
                    foreach (TabPage tmpTab in tabControl1.TabPages)
                    {
                        stwriter.WriteLine(GetTargetUserControl(tmpTab).p_RootAddress);
                    }
                }
            }
        }

        #endregion

        #region タブページ・ユーザコントロール・共通機能

        /// <summary>
        /// 一つ前のタブページを取得
        /// </summary>
        /// <returns></returns>
        private TabPage GetLastBeforeTabPage()
        {
            if (tabControl1.TabPages != null && tabControl1.TabPages.Count > 0)
            {
                return tabControl1.TabPages[tabControl1.TabPages.Count - 1];
            }
            else
            {
                return tabControl1.TabPages[tabControl1.TabPages.Count];
            }
        }

        /// <summary>
        /// 選択されている一つ前のタブページを取得
        /// </summary>
        /// <returns></returns>
        private int GetSelectedBeforeTabPageIndex()
        {
            if (tabControl1.TabPages != null && tabControl1.TabPages.Count > 0 && tabControl1.SelectedIndex > 0)
            {
                return tabControl1.SelectedIndex - 1;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// 対象タブページ内のユーザコントロールを取得
        /// </summary>
        /// <param name="TargetTabPage"></param>
        /// <returns></returns>
        private UserControl1 GetTargetUserControl(TabPage TargetTabPage)
        {
            return (UserControl1)TargetTabPage.Controls[typeof(UserControl1).Name];
        }

        #endregion


        private void ウインドウ名を変更するToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form3 NameForm = new Form3())
            {
                // Enterに反応して実行しようとするため回避
                this.tabControl1.Focus();

                NameForm.Text = "ウインドウ名を入力してください。";
                if (NameForm.ShowDialog() == DialogResult.OK)
                {
                    this.Text = NameForm.p_ResultText;
                }
                else
                {
                    MessageBox.Show("ウインドウ名が入力されませんでした。");
                }

            }
        }

        private void create複製タブ(object sender, EventArgs e)
        {
            GetTargetUserControl(tabControl1.SelectedTab).createReplicaTab();
            this.Show();
        }

        private void タブ追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTab();
            this.Show();
        }

        private void ウインドウ色変更ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog a = new ColorDialog();
            a.FullOpen = true;
            a.Color = this.BackColor;
            if (a.ShowDialog() == DialogResult.OK)
            {
                色設定(a.Color);
            }
        }

    }
}
