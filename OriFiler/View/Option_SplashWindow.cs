using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Msio
{
    public partial class SplashWindow : Form
    {
        public SplashWindow()
        {
            InitializeComponent();
        }

        public void DrawMsg(string Message)
        {
            label1.Text = Message;
            this.Refresh();
            System.Threading.Thread.Sleep(1);
        }

    }
}
