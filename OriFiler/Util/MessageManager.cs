using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Msio
{
    public class MessageManager
    {
        private SplashWindow m_SplashWindow = null;
        private MyWorkSpace.MDIParent1 m_MDIForm = null;

        public MessageManager(MyWorkSpace.MDIParent1 mdi, SplashWindow splash)
        {
            m_MDIForm = mdi;
            m_SplashWindow = splash;
        }

        public void ToMsg(string Message)
        {
            m_MDIForm.DrawMsg(Message);
            m_SplashWindow.DrawMsg(Message);
        }

        public void ToMDIMsg(string Message)
        {
            m_MDIForm.DrawMsg(Message);
        }

        public void ToSplashMsg(string Message)
        {
            m_SplashWindow.DrawMsg(Message);
        }
    }
}
