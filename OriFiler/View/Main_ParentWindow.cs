using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections.Specialized;
using System.IO;
using Msio;
using System.Diagnostics;

namespace MyWorkSpace
{
    public partial class MDIParent1 : Form
    {

        // -----------------------------------------------------------------【Member Variable】

        #region メンバ変数

        private int childFormNumber = 0;
        private CopyPasteMoveManager m_CopyPasteMoveManager = null;
        private NameValueCollection m_MenuLauncherExecList = null;
        private List<Form1> m_Form = null;
        private Msio.SplashWindow m_SplashWindow = null;

        private bool 初期化完了フラグ = false;

        /// <summary>
        /// メッセージ表示操作オブジェクト
        /// </summary>
        private Msio.MessageManager m_MessageManager = null;

        #endregion

        // -----------------------------------------------------------------【Constructer】

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MDIParent1()
        {
            InitializeComponent();
        }

        public void exec()
        {
            // this.IsMdiContainer = true;としているだけなので、MDIコントロールは以下のように取得するしかない。
            MdiClient mc = GetMdiClient(this);
            if (mc != null)
            {
                //背景色を変更し、再描画する
                mc.BackColor = Color.FromArgb(53, 73, 106);
                mc.Invalidate();
            }

            // スプラッシュウインドウ表示
            Application.Idle += new EventHandler(Application_Idle);
            m_SplashWindow = new Msio.SplashWindow();
            m_SplashWindow.Show(this);

            // メッセージ管理オブジェクト生成
            m_MessageManager = new Msio.MessageManager(this, m_SplashWindow);

            // ランチャ作成
            InitMenuLauncherStrip();

            // 設定読み込み
            ReadSetting();
            this.menuStrip.Font = fontDialog1.Font;

            // ワークスペース作成
            OpenWorkSpace(Program.DATADIR + "CurrentForm.wsp");

            初期化完了フラグ = true;
        }

        /// <summary>
        /// フォームのMdiClientコントロールを探して返す
        /// </summary>
        /// <param name="tmpForm">MdiClientコントロールを探すフォーム</param>
        /// <returns>見つかったMdiClientコントロール</returns>
        public static MdiClient GetMdiClient(Form tmpForm)
        {
            foreach (Control tmpControl in tmpForm.Controls)
            {
                if (tmpControl is MdiClient)
                {
                    return (MdiClient)tmpControl;
                }
            }
            return null;
        }

        #endregion

        // -----------------------------------------------------------------【Event Handler】

        #region ランチャアイテムクリックイベント

        /// <summary>
        /// ランチャアイテムクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ランチャToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.GetType().Equals(typeof(ToolStripMenuItem)))
            {
                if (((ToolStripMenuItem)e.ClickedItem).DropDownItems.Count == 0)
                {
                    if (m_MenuLauncherExecList[e.ClickedItem.ToString()] != null)
                    {
                        System.Diagnostics.Process.Start(m_MenuLauncherExecList[e.ClickedItem.ToString()]);
                    }
                }
            }
        }

        #endregion

        #region 選択されているフォームを最大化クリックイベント

        private void 選択されているフォームを最大化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild.WindowState != FormWindowState.Maximized)
            {
                ActiveMdiChild.WindowState = FormWindowState.Maximized;
            }
            else
            {
                ActiveMdiChild.WindowState = FormWindowState.Normal;
            }
        }

        #endregion

        private void 次のウインドウを選択ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MdiChildren.Count(); i++)
            {
                if (MdiChildren[i].Name == ActiveMdiChild.Name.ToString())
                {
                    int WindowNo = i + 1;
                    if (MdiChildren.Length > WindowNo)
                    {
                        MdiChildren[WindowNo].Activate(); // ActiveMdiChild.No
                    }
                    else
                    {
                        MdiChildren[0].Activate();
                    }
                    break;
                }
            }
        }

        #region 新規ウインドウクリックイベント

        private void ShowNewForm(object sender, EventArgs e)
        {
            CreateWindow(string.Empty, string.Empty, string.Empty);
        }

        #endregion

        #region タブリストからウインドウを開くクリックイベント

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                CreateWindow(string.Empty, string.Empty, openFileDialog1.FileName);
            }
        }

        #endregion

        #region ワークスペースを開くクリックイベント

        private void OpenFile(object sender, EventArgs e)
        {
            //openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = "ワークスペース定義 ファイル (*.wsp)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenWorkSpace(openFileDialog1.FileName);
            }
        }

        #endregion

        #region ワークスペースを保存クリックイベント

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                SaveWorkSpace(saveFileDialog1.FileName);
            }
        }

        #endregion

        #region 終了クリックイベント

        /// <summary>
        /// 終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region ウインドウメニュークリックイベント

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 整列順序を固定するため一回ずつ触れる
            for (int i = this.MdiChildren.Length; i > 0; i--)
            {
                this.MdiChildren[i - 1].Activate();
            }
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowClose();
        }

        #endregion

        #region このフォームを閉じるイベント

        private void MDIParent1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.MdiChildren.Length > 0)
            {
                saveAndClose();
            }
        }

        private void saveAndClose()
        {
            SaveSetting();
            SaveWorkSpace("CurrentForm.wsp");

            WindowClose(false);

        }

        #endregion

        #region フォント変更イベント

        private void フォントToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowEffects = true;
            fontDialog1.ShowColor = true;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                //ChangeFont();
                saveAndClose();
                OpenWorkSpace(Program.DATADIR + "CurrentForm.wsp");
            }
        }

        #endregion

        #region スプラッシュウインドウ非表示対応（アプリケーションアイドルイベント）

        //アプリケーションがアイドル状態になった時
        private void Application_Idle(object sender, EventArgs e)
        {
            //Splashフォームがあるか調べる
            if (m_SplashWindow != null && m_SplashWindow.IsDisposed == false)
            {
                m_MessageManager.ToMsg(string.Empty);
                //Splashフォームを非表示にする
                m_SplashWindow.Hide();
            }
            //Application.Idleイベントハンドラの削除
            Application.Idle -= new EventHandler(Application_Idle);
        }

        #endregion

        // -----------------------------------------------------------------【Public Method】
        public void DrawMsg(string Message)
        {
            toolStripStatusLabel.Text = Message;
            System.Threading.Thread.Sleep(1);
        }

        // -----------------------------------------------------------------【Private Method】

        #region ランチャメニュー初期化

        /// <summary>
        /// ランチャメニュー初期化
        /// </summary>
        private void InitMenuLauncherStrip()
        {
            m_MessageManager.ToSplashMsg("ランチャ初期化");

            m_MenuLauncherExecList = ToolStripProgramStartMenuFactory.getStripMenu(
                "Lancher",
                ランチャToolStripMenuItem,
                ランチャToolStripMenuItem_DropDownItemClicked,
                searchMenuItem_KeyDown,
                Path.Combine(Program.CONFDIR, "LauncherSetting.ini")
                );
        }

        private void searchMenuItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (m_MenuLauncherExecList[((ToolStripTextBox)sender).Text] != null)
                {
                    System.Diagnostics.Process.Start(m_MenuLauncherExecList[((ToolStripTextBox)sender).Text]);
                    ((ToolStripTextBox)sender).Text = string.Empty;
                }
            }
        }

        #endregion

        #region ワークスペースを開く

        private void OpenWorkSpace(string strPath)
        {
            if (this.MdiChildren.Length == 0 ||
                MessageBox.Show("現在の子画面をすべて閉じ、ワークスペースを開きます。よろしいですか？",
                "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                m_MessageManager.ToSplashMsg("ワークスペース初期化");

                WindowClose(false);

                m_CopyPasteMoveManager = new CopyPasteMoveManager();
                m_Form = new List<Form1>();
                CreateWorkSpaceFromFile(strPath);
            }
        }

        #endregion

        #region ウインドウ作成

        private void CreateWindow(string BeforeWindowNo, string DisplayText, string TabListFilePath)
        {
            Form3 FolderNameWindow = new Form3();
            string FormText = DisplayText;

            if (String.IsNullOrEmpty(FormText))
            {
                // フォーム表示名が設定されていない場合は入力要求する
                FolderNameWindow.Text = "ウインドウ名を入力してください。";
                if (FolderNameWindow.ShowDialog() == DialogResult.OK)
                {
                    FormText = FolderNameWindow.p_ResultText;
                }
            }

            if (String.IsNullOrEmpty(FormText))
            {
                MessageBox.Show("ウインドウ名が入力されませんでした。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                m_Form.Add(new Form1((++childFormNumber).ToString(), BeforeWindowNo, TabListFilePath, m_CopyPasteMoveManager, m_MessageManager, fontDialog1.Font, fontDialog1.Color));
                m_Form.Last().MdiParent = this;
                m_Form.Last().Text = FormText;
                m_Form.Last().Name = childFormNumber.ToString();
                m_Form.Last().Show();
                //m_Form.Last().ChangeFont(fontDialog1.Font, fontDialog1.Color);
                //m_Form.Last().isInit = false;
            }
        }

        #endregion

        #region 子ウインドウを閉じる

        private bool WindowClose()
        {
            return WindowClose(true);
        }

        private bool WindowClose(bool CheckFlg)
        {
            bool bolRt = true;

            if (CheckFlg)
            {
                if (MessageBox.Show("子画面を全て閉じます。よろしいですか?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    bolRt = false;
                }
            }

            if (bolRt)
            {
                foreach (Form childForm in MdiChildren)
                {
                    childForm.Close();
                }
            }

            return bolRt;
        }

        #endregion

        #region ワークスペースを保存

        public void SaveWorkSpace(string strPath)
        {
            if (this.MdiChildren.Length > 0)
            {
                using (StreamWriter stwriter = new StreamWriter(Program.DATADIR + strPath, false, Encoding.UTF8))
                {
                    foreach (Form tmpForm in this.MdiChildren)
                    {
                        string TabListPath = strPath + "_" + tmpForm.Name + ".tab";
                        ((Form1)tmpForm).SaveTabList(TabListPath);
                        stwriter.WriteLine(tmpForm.Name + "\t" + tmpForm.Text + "\t" + TabListPath);
                    }
                }
            }
        }

        #endregion

        #region ワークスペースを開く

        public void CreateWorkSpaceFromFile(string strPath)
        {
            if (File.Exists(strPath))
            {
                using (StreamReader stReader = new StreamReader(strPath, Encoding.UTF8))
                {
                    while (stReader.Peek() > 0)
                    {
                        string tmp = stReader.ReadLine();
                        string[] strArray = tmp.Split('\t');
                        CreateWindow(strArray[0], strArray[1], strArray[2]);
                    }
                }
            }
        }

        #endregion

        #region 各種設定を保存

        private void SaveSetting()
        {
            string strPath = "Font.setting";

            using (StreamWriter stwriter = new StreamWriter(Program.CONFDIR + strPath, false, Encoding.UTF8))
            {
                stwriter.WriteLine(new FontConverter().ConvertToString(fontDialog1.Font));
                stwriter.WriteLine(new ColorConverter().ConvertToString(fontDialog1.Color));
            }
        }

        #endregion

        #region 各種設定を読み込み

        private void ReadSetting()
        {
            string strPath = Program.CONFDIR + "Font.setting";

            if (File.Exists(strPath))
            {
                using (StreamReader stReader = new StreamReader(strPath, Encoding.UTF8))
                {
                    FontConverter Converter = new FontConverter();

                    fontDialog1.Font = ((Font)new FontConverter().ConvertFromString(stReader.ReadLine()));
                    fontDialog1.Color = ((Color)new ColorConverter().ConvertFromString(stReader.ReadLine()));
                }
            }
        }

        #endregion

        #region フォント変更

        private void ChangeFont()
        {
            //this.Font = fontDialog1.Font;
            this.menuStrip.Font = fontDialog1.Font;

            foreach (Form1 tmpForm in MdiChildren)
            {
                tmpForm.ChangeFont(fontDialog1.Font, fontDialog1.Color);
            }

            this.Refresh();
        }

        #endregion

        private void MDIParent1_Activated(object sender, EventArgs e)
        {
            m_MessageManager.ToMDIMsg(m_CopyPasteMoveManager.GetTargetItemString());

            if(初期化完了フラグ && this.ActiveMdiChild != null)
            {
                ((Form1)this.ActiveMdiChild).updateListView();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Msio.AboutBox1 InfoWindow = new Msio.AboutBox1();
            InfoWindow.ShowDialog();
        }

        private void MDIParent1_ResizeEnd(object sender, EventArgs e)
        {
            // これをしないとタスクバーのウインドウを閉じるで終了しないことがある。
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Focus();
            }
        }

        private void ヘルプToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://usewow.official.jp/MyWorkSpace.html"));
        }
    }
}
