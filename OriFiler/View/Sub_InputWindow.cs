using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyWorkSpace
{
    /// <summary>
    /// 文字列入力汎用フォーム
    /// </summary>
    public partial class Form3 : Form
    {
        // -----------------------------------------------------------------【Constructer】

        #region コンストラクタ

        public Form3()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
        }

        public Form3(string strMsg) : this()
        {
            textBox1.Text = strMsg;
        }

        #endregion

        // -----------------------------------------------------------------【Public Property】

        #region 入力文字列取得

        public string p_ResultText
        {
            get { return textBox1.Text; }
        }

        #endregion

        // -----------------------------------------------------------------【Event Handler】

        /// <summary>
        /// OKクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Hide();
        }

        /// <summary>
        /// キャンセルクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Hide();
        }

        private void Form3_Shown(object sender, EventArgs e)
        {
            var position = new Point();
            position.X = (Program.ROOT_WINDOW.DesktopLocation.X + Program.ROOT_WINDOW.Width / 2) - (this.Width / 2);
            position.Y = (Program.ROOT_WINDOW.DesktopLocation.Y + Program.ROOT_WINDOW.Height / 2) - (this.Height / 2);
            this.DesktopLocation = position;
        }

    }
}
