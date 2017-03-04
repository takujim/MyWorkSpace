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
    /// 【参考】http://www.ipentec.com/document/document.aspx?page=csharp-visual-component-get-scrollbar-event&culture=ja-jp
    /// </summary>
    class WindowsConst
    {
        // window style constants for scrollbars
        public const int WS_VSCROLL = 0x00200000;
        public const int WS_HSCROLL = 0x00100000;
        public const int WM_LBUTTONDOWN = 0x00000201;
        public const int WM_RBUTTONDOWN = 0x00000204;

        public const int WM_HSCROLL = 0x00000114;
        public const int WM_VSCROLL = 0x00000115;

        /*
         * Scroll Bar Commands
         */
        public const int SB_LINEUP = 0;
        public const int SB_LINELEFT = 0;
        public const int SB_LINEDOWN = 1;
        public const int SB_LINERIGHT = 1;
        public const int SB_PAGEUP = 2;
        public const int SB_PAGELEFT = 2;
        public const int SB_PAGEDOWN = 3;
        public const int SB_PAGERIGHT = 3;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_THUMBTRACK = 5;
        public const int SB_TOP = 6;
        public const int SB_LEFT = 6;
        public const int SB_BOTTOM = 7;
        public const int SB_RIGHT = 7;
        public const int SB_ENDSCROLL = 8;
    }


    /// <summary>
    /// http://watcher.moe-nifty.com/memo/2009/02/ctabcontrol-b32.html
    /// </summary>
    public partial class ListViewEX : ListView
    {
        public ListViewEX()
        {
            InitializeComponent();
        }

        public delegate void custumVerticalScrolled(object sender, EventArgs e);
        public event custumVerticalScrolled VerticalScrolled;

        [System.Security.Permissions.SecurityPermission(
    System.Security.Permissions.SecurityAction.LinkDemand,
    Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            const int WM_VSCROLL = 0x115;

            if (m.Msg == WM_VSCROLL)
            {
                // 垂直スクロール
                if (LoWord((long)m.WParam) == WindowsConst.SB_ENDSCROLL)
                {
                    // スクロールが終わった場合
                    VerticalScrolled(this, new EventArgs());
                }
            }

            base.WndProc(ref m);
        }

        protected short LoWord(long input)
        {
            return (short)((int)input & 0xFFFF);
        }

//        Public Class ExListView
//  ’ListViewコントロールを継承
//  Inherits System.Windows.Forms.ListView

//  ’外部に公開されたイベントを定義します（引数はプログラマの自由です）
//  Public Event VerticalScrolled(ByVal sender As Object, ByVal e As EventArgs)

//  ’this::WndProc
//  Protected Overrides Sub WndProc(ByRef m As Message)
//    Const WM_VSCROLL As Integer = &H115

//    If m.Msg = WM_VSCROLL Then
//      RaiseEvent VerticalScrolled(Me, New EventArgs)
//    End If

//    MyBase.WndProc(m)
//  End Sub
//End Class

 
    }
}
