using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Msio
{
    public class ToolStripUtil
    {
        #region 検索用メニューを作成

        /// <summary>
        /// 検索用メニューを作成
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static ToolStripTextBox createSearchMenuItem(string[] dataSource, KeyEventHandler handler)
        {
            var searchMenuItem = new ToolStripTextBox();

            searchMenuItem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            searchMenuItem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            searchMenuItem.Width = 320;
            searchMenuItem.AutoSize = false;
            searchMenuItem.Text = "検索";
            searchMenuItem.KeyDown += handler;
            searchMenuItem.GotFocus += searchMenuItem_GotFocus;
            searchMenuItem.LostFocus += searchMenuItem_LostFocus;
            searchMenuItem.BorderStyle = BorderStyle.FixedSingle;

            searchMenuItem.AutoCompleteCustomSource.AddRange(dataSource);

            //var searchMenuItem = new ToolStripComboBox();

            //searchMenuItem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //searchMenuItem.AutoCompleteSource = AutoCompleteSource.ListItems;
            //searchMenuItem.DropDownWidth = 320;
            //searchMenuItem.Width = 320;
            //searchMenuItem.Text = "※検索";
            //searchMenuItem.KeyDown += handler;
            //searchMenuItem.DropDownStyle = ComboBoxStyle.DropDown;

            //searchMenuItem.Items.AddRange(dataSource);

            return searchMenuItem;
        }

        private static void searchMenuItem_LostFocus(object sender, EventArgs e)
        {
            ((ToolStripTextBox)sender).Text = "検索";
        }

        private static void searchMenuItem_GotFocus(object sender, EventArgs e)
        {
            ((ToolStripTextBox)sender).Text = "";
        }

        #endregion

        #region Windowsメニュー：すべてのプログラムと同等のものを作成

        /// <summary>
        /// Windowsメニュー：すべてのプログラムと同等のものを作成
        /// </summary>
        /// <returns></returns>
        public static ToolStripMenuItem createAllProgramStripMenuItem(ToolStripItemClickedEventHandler handler, NameValueCollection infoList)
        {
            var rootDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonPrograms);

            ToolStripMenuItem allProgramMenu = new ToolStripMenuItem("すべてのプログラム");
            allProgramMenu.DropDownItemClicked += handler;
            infoList = createAllProgramLoopDirectory(allProgramMenu, rootDir, handler, infoList);
            return allProgramMenu;
        }

        private static NameValueCollection createAllProgramLoopDirectory(ToolStripMenuItem menuItemParent, string targetDir, ToolStripItemClickedEventHandler handler, NameValueCollection infoList)
        {
            infoList = createAllProgramFile(menuItemParent.DropDownItems, Directory.EnumerateFiles(targetDir), infoList);

            foreach (var innerFolder in Directory.EnumerateDirectories(targetDir))
            {
                ToolStripMenuItem parent = new ToolStripMenuItem(Path.GetFileName(innerFolder));
                createAllProgramLoopDirectory(parent, innerFolder, handler, infoList);
                parent.DropDownItemClicked += handler;
                menuItemParent.DropDownItems.Add(parent);
            }

            return infoList;
        }

        private static NameValueCollection createAllProgramFile(ToolStripItemCollection stripContainer, IEnumerable<string> fileList, NameValueCollection infoList)
        {
            foreach (var file in fileList)
            {
                var item = new ToolStripMenuItem(Path.GetFileName(file));
                stripContainer.Add(item);

                infoList.Add(item.ToString(), file);
            }

            return infoList;
        }

        #endregion
    }



}
