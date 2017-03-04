using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Forms;

namespace MyWorkSpace
{
    public class CopyPasteMoveManager
    {
        #region 列挙対

        #region ファイル操作タイプ

        public enum OperationType
        {
            None,
            Copy,
            Move,
        }

        #endregion

        #region ファイル、フォルダ種別

        private enum TargetType
        {
            None,
            File,
            Directory,
        }

        #endregion

        #endregion

        /// <summary>
        /// 現在のクリップボードの状態を表す文字列を取得する
        /// </summary>
        /// <returns></returns>
        public string GetTargetItemString()
        {
            StringBuilder rtMsg = new StringBuilder();

            try
            {
                //クリップボードのデータを取得する
                IDataObject data = Clipboard.GetDataObject();
                if (data != null && data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = GetTargetFiles(data);
                    //DragDropEffectsを取得する
                    DragDropEffects dde = GetPreferredDropEffect(data);
                    if (dde == DragDropEffects.Move)
                    {
                        //ファイルが切り取られていた時
                        rtMsg.Append("切り取り元：");
                    }
                    else
                    {
                        //ファイルがコピーされていた時
                        rtMsg.Append("コピー元：");
                    }

                    foreach (string strItem in files)
                    {
                        string strTarget = strItem;
                        if (strItem.Last() == '\\' && strItem.Length > 3)
                        {
                            strTarget = strItem.Substring(0, strItem.Length - 1);
                        }

                        if (File.Exists(strTarget))
                        {
                            rtMsg.Append("【ファイル】");
                        }
                        else if (Directory.Exists(strTarget))
                        {
                            rtMsg.Append("【フォルダ】");
                        }
                        else
                        {
                            rtMsg.Append("【存在しません】");
                        }

                        rtMsg.Append(strTarget);
                        rtMsg.Append(", ");
                    }
                }
            }
            catch (Exception)
            {
                // 変なデータがあった場合にエラーする可能性がある。
            }

            return rtMsg.ToString();
        }

        public void Copy(string strPath)
        {
            List<string> tmpCol = new List<string>();
            tmpCol.Add(strPath);
            Copy(tmpCol);
        }

        public string Copy(List<string> colPath)
        {
            // エラーすることあるのでリトライ対応
            var count = 0;
            while (count < 5)
            {
                try
                {
                    // --- クリップボードからのコピー対応 START ---

                    System.Collections.Specialized.StringCollection ItemCollection = new System.Collections.Specialized.StringCollection();
                    ItemCollection.AddRange(colPath.ToArray());
                    Clipboard.SetFileDropList(ItemCollection);

                    // --- クリップボードからのコピー対応 END ---

                    //m_Type = OperationType.Copy;
                    return GetTargetItemString();
                }
                catch
                {
                    count++;
                }
            }
            return "コピー：エラー";
        }

        public void Move(string strPath)
        {
            List<string> tmpCol = new List<string>();
            tmpCol.Add(strPath);
            Move(tmpCol);
        }

        public string Move(List<string> colPath)
        {
            // エラーすることあるのでリトライ対応
            var count = 0;
            while (count < 5)
            {
                try
                {
                    // --- クリップボードからの切り取り対応 START ---

                    //ファイルドロップ形式のDataObjectを作成する
                    IDataObject data = new DataObject(DataFormats.FileDrop, colPath.ToArray());

                    //DragDropEffects.Moveを設定する（DragDropEffects.Move は 2）
                    byte[] bs = new byte[] { (byte)DragDropEffects.Move, 0, 0, 0 };
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(bs);
                    data.SetData("Preferred DropEffect", ms);

                    //クリップボードに切り取る
                    Clipboard.SetDataObject(data);

                    // --- クリップボードからの切り取り対応 END ---

                    //m_Type = OperationType.Move;
                    return GetTargetItemString();
                }
                catch
                {
                    count++;
                }
            }
            return "切り取り：エラー";
        }

        /// <summary>
        /// 貼り付け
        /// </summary>
        /// <param name="strDstPath"></param>
        /// <returns></returns>
        public OperationResult Paste(string strDstPath)
        {
            OperationResult RtObj = null;

            //クリップボードのデータを取得する
            IDataObject data = Clipboard.GetDataObject();
            //クリップボードにファイルドロップ形式のデータがあるか確認
            if (data != null && data.GetDataPresent(DataFormats.FileDrop))
            {
                //DragDropEffectsを取得する
                DragDropEffects dde = GetPreferredDropEffect(data);
                if (dde == DragDropEffects.Move)
                {
                    //ファイルが切り取られていた時
                    RtObj = PasteMain(GetTargetFiles(data), strDstPath, OperationType.Move);
                }
                else
                {
                    //ファイルがコピーされていた時
                    RtObj = PasteMain(GetTargetFiles(data), strDstPath, OperationType.Copy);
                }
            }
            else
            {
                MessageBox.Show("コピー元ファイルが選択されていません");
                RtObj = new OperationResult(new List<string>(), strDstPath, OperationType.None);
            }

            #region Copy-Moveのグローバル化により削除

            //if (m_TargetItem.Count > 0)
            //{
            //    List<string> PasteItem = new List<string>();

            //    foreach (KeyValuePair<string, TargetType> tmpTarget in m_TargetItem)
            //    {
            //        #region 手動チェックをする場合、やってくれるので必要ない
            //        //bool tmpFlg = true;
            //        //if (tmpTarget.Value == TargetType.File && 
            //        //    File.Exists(Path.Combine(strDstPath, Path.GetFileName(tmpTarget.Key))))
            //        //{
            //        //    if (DialogResult.OK == MessageBox.Show(Path.GetFileName(tmpTarget.Key) + "が既に存在します。上書きしますか？", "上書き確認", MessageBoxButtons.OKCancel))
            //        //    {
            //        //        //File.Delete(Path.Combine(strDstPath, Path.GetFileName(tmpTarget.Key)));
            //        //    }
            //        //    else
            //        //    {
            //        //        tmpFlg = false;
            //        //    }
            //        //}
            //        //else if (tmpTarget.Value == TargetType.Directory && 
            //        //    Directory.Exists(Path.Combine(strDstPath, Path.GetFileName(tmpTarget.Key))))
            //        //{
            //        //    if (DialogResult.OK == MessageBox.Show(Path.GetFileName(tmpTarget.Key) + "が既に存在します。上書きしますか？", "上書き確認", MessageBoxButtons.OKCancel))
            //        //    {
            //        //    }
            //        //    else
            //        //    {
            //        //        tmpFlg = false;
            //        //    }
            //        //}
            //        //if (tmpFlg)
            //        //{
            //        //}
            //        #endregion

            //        bool ExecFlg = true;
            //        string strNewPath = Path.Combine(strDstPath, Path.GetFileName(tmpTarget.Key));
            //        while (tmpTarget.Key == strNewPath)
            //        {
            //            MessageBox.Show("既に存在するため、別名を指定してください。");
            //            using (Form3 NameForm = new Form3(Path.GetFileName(tmpTarget.Key)))
            //            {
            //                if (NameForm.ShowDialog() == DialogResult.OK)
            //                {
            //                    strNewPath = Path.Combine(strDstPath, NameForm.p_ResultText);
            //                }
            //                else
            //                {
            //                    ExecFlg = false;
            //                    break;
            //                }
            //            }
            //        }

            //        if (ExecFlg)
            //        {
            //            #region 処理実行

            //            switch (m_Type)
            //            {
            //                case OperationType.Copy:
            //                    if (tmpTarget.Value == TargetType.File)
            //                    {
            //                        //File.Copy(tmpTarget.Key, Path.Combine(strDstPath, Path.GetFileName(tmpTarget.Key)), true);

            //                        try
            //                        {
            //                            Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(
            //                                tmpTarget.Key, strNewPath,
            //                                Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
            //                                Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
            //                            PasteItem.Add(tmpTarget.Key);
            //                        }
            //                        catch (OperationCanceledException)
            //                        {
            //                        }
            //                    }
            //                    else if (tmpTarget.Value == TargetType.Directory)
            //                    {
            //                        try
            //                        {
            //                            Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(
            //                                tmpTarget.Key, strNewPath,
            //                                Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
            //                                Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
            //                            PasteItem.Add(tmpTarget.Key);
            //                        }
            //                        catch (OperationCanceledException)
            //                        {
            //                        }
            //                    }
            //                    break;

            //                case OperationType.Move:
            //                    if (tmpTarget.Value == TargetType.File)
            //                    {
            //                        try
            //                        {
            //                            //File.Move(tmpTarget.Key, Path.Combine(strDstPath, Path.GetFileName(tmpTarget.Key)));
            //                            Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(
            //                                    tmpTarget.Key, strNewPath,
            //                                    Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
            //                                    Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
            //                            PasteItem.Add(tmpTarget.Key);
            //                        }
            //                        catch (OperationCanceledException)
            //                        {
            //                        }
            //                    }
            //                    else if (tmpTarget.Value == TargetType.Directory)
            //                    {
            //                        try
            //                        {
            //                            Microsoft.VisualBasic.FileIO.FileSystem.MoveDirectory(
            //                            tmpTarget.Key, strNewPath,
            //                            Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
            //                            Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
            //                            PasteItem.Add(tmpTarget.Key);
            //                        }
            //                        catch (OperationCanceledException)
            //                        {
            //                        }
            //                    }
            //                    break;
            //            }

            //            #endregion
            //        }

            //        RtObj = new OperationResult(PasteItem, strDstPath, m_Type);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("コピー元ファイルが選択されていません");
            //    RtObj = new OperationResult(new List<string>(), strDstPath, OperationType.None);
            //}

            #endregion

            return RtObj;
        }

        /// <summary>
        /// 複数のファイルを指定したフォルダにコピーまたは移動する
        /// </summary>
        private OperationResult PasteMain(string[] sourceFiles, string strDstPath, OperationType CopyMoveType)
        {
            List<string> PasteItem = new List<string>();
            OperationResult RtObj = null;

            if (sourceFiles.Length > 0)
            {
                foreach (string sourcePath in sourceFiles)
                {
                    bool ExecFlg = true;
                    string strSrcPath = sourcePath;
                    string strNewPath = string.Empty;
                    TargetType FileOrDirectory = TargetType.File;
                    if (Directory.Exists(strSrcPath))
                    {
                        FileOrDirectory = TargetType.Directory;
                        if (strSrcPath.Last() != Path.DirectorySeparatorChar)
                        {
                            strSrcPath = strSrcPath + Path.DirectorySeparatorChar;
                        }
                        strNewPath = Path.Combine(strDstPath, Path.GetFileName(Path.GetDirectoryName(strSrcPath)));
                    }
                    else if (File.Exists(strSrcPath))
                    {
                        strNewPath = Path.Combine(strDstPath, Path.GetFileName(strSrcPath));
                    }
                    else
                    {
                        ExecFlg = false;
                    }

                    if (ExecFlg)
                    {
                        while (strSrcPath == strNewPath)
                        {
                            MessageBox.Show("既に存在するため、別名を指定してください。");
                            using (Form3 NameForm = new Form3(Path.GetFileName(strSrcPath)))
                            {
                                NameForm.Text = "別名を入力してください。";
                                if (NameForm.ShowDialog() == DialogResult.OK)
                                {
                                    strNewPath = Path.Combine(strDstPath, NameForm.p_ResultText);
                                }
                                else
                                {
                                    ExecFlg = false;
                                    break;
                                }
                            }
                        }

                        if (ExecFlg)
                        {
                            #region 処理実行

                            switch (CopyMoveType)
                            {
                                case OperationType.Copy:
                                    if (FileOrDirectory == TargetType.File)
                                    {
                                        //File.Copy(tmpTarget.Key, Path.Combine(strDstPath, Path.GetFileName(tmpTarget.Key)), true);

                                        try
                                        {
                                            Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(
                                                strSrcPath, strNewPath,
                                                Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
                                                Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                                            PasteItem.Add(strSrcPath);
                                        }
                                        catch (OperationCanceledException)
                                        {
                                        }
                                    }
                                    else if (FileOrDirectory == TargetType.Directory)
                                    {
                                        try
                                        {
                                            try
                                            {
                                                Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(
                                                    strSrcPath, strNewPath,
                                                    Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
                                                    Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                                            }
                                            catch (InvalidOperationException)
                                            {
                                                MessageBox.Show("コピー元とコピー先のフォルダが同じフォルダです。");
                                                // コピー元フォルダとコピー先フォルダが同じ場合の処理(おかしいと思うが規定の動作らしい…と思ったが普通だ！！)
                                                //var tempDirName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(Path.GetDirectoryName(strNewPath));
                                                //var tempPath = Path.Combine(Path.GetTempPath(), tempDirName);

                                                //Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(strSrcPath, tempPath);
                                                //Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(
                                                //    tempPath, strNewPath,
                                                //    Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
                                                //    Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                                                //Directory.Delete(tempPath, true);
                                            }
                                            PasteItem.Add(strSrcPath);
                                        }
                                        catch (OperationCanceledException)
                                        {
                                        }
                                    }
                                    break;

                                case OperationType.Move:
                                    if (FileOrDirectory == TargetType.File)
                                    {
                                        try
                                        {
                                            //File.Move(tmpTarget.Key, Path.Combine(strDstPath, Path.GetFileName(tmpTarget.Key)));
                                            Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(
                                                    strSrcPath, strNewPath,
                                                    Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
                                                    Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                                            PasteItem.Add(strSrcPath);
                                        }
                                        catch (OperationCanceledException)
                                        {
                                        }
                                    }
                                    else if (FileOrDirectory == TargetType.Directory)
                                    {
                                        try
                                        {
                                            Microsoft.VisualBasic.FileIO.FileSystem.MoveDirectory(
                                            strSrcPath, strNewPath,
                                            Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
                                            Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                                            PasteItem.Add(strSrcPath);
                                        }
                                        catch (OperationCanceledException)
                                        {
                                        }
                                    }
                                    break;
                            }

                            #endregion
                        }
                    }

                    RtObj = new OperationResult(PasteItem, strDstPath, CopyMoveType);
                }
            }
            else
            {
                MessageBox.Show("コピー元ファイルが選択されていません");
                RtObj = new OperationResult(new List<string>(), strDstPath, OperationType.None);
            }

            return RtObj;
        }

        /// <summary>
        /// 処理対象ファイル取得
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string[] GetTargetFiles(IDataObject data)
        {
            //コピーされたファイルのリストを取得する
            string[] files = (string[])data.GetData(DataFormats.FileDrop);

            return files;
        }

        /// <summary>
        /// クリップボードの"Preferred DropEffect"を調べる
        /// </summary>
        private DragDropEffects GetPreferredDropEffect(IDataObject data)
        {
            // Clipboard.SetFileDropListを使用してコピーを行った場合は、Preferred DropEffect形式のデータを取得できない。
            // ⇒Msio上でコピーした場合は取得不可
            // エクスプローラからコピーした時は取得可能
            // したがって、デフォルト値をコピーとする。
            // 切り取りの場合は、Msio上でもPreferred DropEffect形式のbyteを設定している。
            DragDropEffects dde = DragDropEffects.Copy;

            if (data != null)
            {
                //Preferred DropEffect形式のデータを取得する
                System.IO.MemoryStream ms =
                    (System.IO.MemoryStream)data.GetData("Preferred DropEffect");
                if (ms != null)
                {
                    //先頭のバイトからDragDropEffectsを取得する
                    dde = (DragDropEffects)ms.ReadByte();

                    if (dde == (DragDropEffects.Copy | DragDropEffects.Link))
                    {
                        Console.WriteLine("コピー");
                    }
                    else if (dde == DragDropEffects.Move)
                    {
                        Console.WriteLine("切り取り");
                    }
                }
            }

            return dde;
        }


        #region 処理したファイルに関する情報オブジェクト
        public class OperationResult
        {
            private List<string> m_SrcTargetFiles = null;
            private string m_DstDirectory = null;
            private CopyPasteMoveManager.OperationType m_OperationType = CopyPasteMoveManager.OperationType.None;

            public OperationResult(List<string> SrcTargetFiles, string DstDirectory, CopyPasteMoveManager.OperationType OperationType)
            {
                m_SrcTargetFiles = SrcTargetFiles;
                m_DstDirectory = DstDirectory;
                m_OperationType = OperationType;
            }

            public List<string> p_SrcTargetFiles
            {
                get
                {
                    return m_SrcTargetFiles;
                }
            }

            public string p_DstDirectory
            {
                get
                {
                    return m_DstDirectory;
                }
            }

            public CopyPasteMoveManager.OperationType p_OperationType
            {
                get
                {
                    return m_OperationType;
                }
            }
        }
        #endregion
    }
}
