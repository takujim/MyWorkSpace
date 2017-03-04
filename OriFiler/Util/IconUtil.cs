using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Msio
{
    public class IconUtil
    {
        public static Icon getIconFromFilePath(string filepath)
        {
            Icon rtIcon;

            Uri CheckUnc = new Uri(filepath);
            if (CheckUnc.IsUnc)
            {
                rtIcon = getIconUnmanage(filepath);
            }
            else
            {
                rtIcon = Icon.ExtractAssociatedIcon(filepath);
            }

            return rtIcon;
        }

        [DllImport("shell32.dll", EntryPoint = "ExtractAssociatedIcon")]
        private extern static IntPtr ExtractAssociatedIcon(IntPtr hInst, [MarshalAs(UnmanagedType.LPStr)] string lpIconPath, ref int lpiIcon);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        private static Icon getIconUnmanage(string filepath)
        {
            IntPtr hInst = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]);
            Int32 iIcon = 0;
            IntPtr hIcon;

            // ファイルに関連付けられたアイコンのハンドルを取得する
            hIcon = ExtractAssociatedIcon(hInst, filepath, ref iIcon);

            // アイコンハンドルからIconオブジェクトを作成する
            Icon rtIcon = Icon.FromHandle(hIcon);
            //DestroyIcon(hIcon);

            return rtIcon;
        }
    }



}
