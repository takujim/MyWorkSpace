using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Msio
{
    // 同じメニューは一個しか作らない！メモリ削減。

    public static class ToolStripProgramStartMenuFactory
    {
        private static List<KeyValuePair<string, ToolStripProgramStartCreator>> colStripMenuList =
            new List<KeyValuePair<string, ToolStripProgramStartCreator>>();

        public static NameValueCollection getStripMenu(
            string key,
            ToolStripMenuItem root,
            ToolStripItemClickedEventHandler clickHandler, KeyEventHandler KeyHandler,
            string settingFile
                )
        {
            //と思ったができなようだ。。。
            //for (int i = 0; i < colStripMenuList.Count; i++)
            //{
            //    if (colStripMenuList[i].Key == key)
            //    {
            //        root.DropDownItems.Add(colStripMenuList[i].Value.getMenuItem());
            //        return colStripMenuList[i].Value.getNameAndPathList();
            //    }
            //}

            var newMenuItem = new ToolStripProgramStartCreator();
            colStripMenuList.Add(new KeyValuePair<string, ToolStripProgramStartCreator>(key, newMenuItem));
            return newMenuItem.buildProgramStartMenu(root, clickHandler, KeyHandler, settingFile);
        }

    }

    public class ToolStripProgramStartCreator
    {
        private ToolStripMenuItem m_MenuItem;
        private NameValueCollection m_NameAndPathList;

        public ToolStripMenuItem getMenuItem()
        {
            return m_MenuItem;
        }

        public NameValueCollection getNameAndPathList()
        {
            return m_NameAndPathList;
        }

        public NameValueCollection buildProgramStartMenu(
            ToolStripMenuItem root,
            ToolStripItemClickedEventHandler clickHandler, KeyEventHandler KeyHandler,
            string userMenuSettingPath)
        {
            var MenuLauncherExecList = new NameValueCollection();

            ToolStripMenuItem userProgramMenu = new ToolStripMenuItem("ユーザ設定プログラム");
            userProgramMenu.DropDownItemClicked += clickHandler;

            using (StreamReader StReader = new StreamReader(userMenuSettingPath, Encoding.UTF8))
            {
                #region ユーザメニュー構築
                // ユーザメニュー構築
                string strBeforeKbn = string.Empty;

                while (StReader.Peek() > 0)
                {
                    string strTmp = StReader.ReadLine();
                    if (strTmp != string.Empty)
                    {
                        string[] strTmp2 = strTmp.Split('\t');

                        ToolStripMenuItem tmpMenuItem = null;
                        // 統一感のために画像表示なしとする
                        //try
                        //{
                        //    tmpMenuItem = new ToolStripMenuItem(
                        //        strTmp2[1],
                        //        IconUtil.getIconFromFilePath(Path.GetFullPath(strTmp2[2])).ToBitmap());
                        //}
                        //catch
                        //{
                        //    // 指定パス不正の場合、エラーにするまででもないので。
                        //    tmpMenuItem = new ToolStripMenuItem(strTmp2[1]);
                        //}
                        tmpMenuItem = new ToolStripMenuItem(strTmp2[1]);

                        if (strTmp2[0] == string.Empty)
                        {
                            userProgramMenu.DropDownItems.Add(tmpMenuItem);
                        }
                        else if (strBeforeKbn == strTmp2[0])
                        {
                            ((ToolStripMenuItem)userProgramMenu.DropDownItems[userProgramMenu.DropDownItems.Count - 1]).DropDownItems.Add(tmpMenuItem);
                        }
                        else
                        {
                            ToolStripMenuItem tmpMenuItemParent = new ToolStripMenuItem(strTmp2[0]);
                            tmpMenuItemParent.DropDownItemClicked += clickHandler;
                            tmpMenuItemParent.DropDownItems.Add(tmpMenuItem);
                            userProgramMenu.DropDownItems.Add(tmpMenuItemParent);
                        }
                        MenuLauncherExecList.Add(tmpMenuItem.ToString(), strTmp2[2]);

                        strBeforeKbn = strTmp2[0];
                    }
                }
                #endregion
            }
            root.DropDownItems.Add(userProgramMenu);

            // 全てのプログラム構築
            root.DropDownItems.Add(new ToolStripSeparator());
            root.DropDownItems.Add(ToolStripUtil.createAllProgramStripMenuItem(clickHandler, MenuLauncherExecList));

            // 検索メニュー構築
            root.DropDownItems.Add(new ToolStripSeparator());
            root.DropDownItems.Add(ToolStripUtil.createSearchMenuItem(MenuLauncherExecList.AllKeys, KeyHandler));

            m_MenuItem = root;
            m_NameAndPathList = MenuLauncherExecList;

            return MenuLauncherExecList;
        }

    }
}
