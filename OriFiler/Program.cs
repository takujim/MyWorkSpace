using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MyWorkSpace
{
    static class Program
    {
        public static string CONFDIR = @"conf";
        public static string DATADIR = @"data";
        public static Form ROOT_WINDOW;
        public static ListViewIconSetting ICON_SETTING;

        public static ToolStripMenuItem SEARCH_MENU_ITEM;
        public static ToolStripMenuItem ALL_PROGRAM_MENU_ITEM;

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            CONFDIR = Path.Combine(System.Environment.CurrentDirectory,CONFDIR) + @"\";
            DATADIR = Path.Combine(System.Environment.CurrentDirectory, DATADIR) + @"\";
            setIconSetting(ListViewIconSetting.none);
            var mdi = new MDIParent1();
            ROOT_WINDOW = mdi;
            mdi.exec();
            Application.Run(mdi);
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show("エラー：" + e.Exception.Message);
            MessageBox.Show("詳細：" + e.Exception.ToString(),"エラーが発生しました。問題がある場合はお問い合わせください。");
        }

        public enum ListViewIconSetting
        {
            none,
            icon
        }
        public static void setIconSetting(ListViewIconSetting setting)
        {
            ICON_SETTING = setting;
        }


    }
}
