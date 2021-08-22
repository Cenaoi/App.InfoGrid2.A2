using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2
{
    public class LoadPageEventArgs : EventArgs
    {
        int m_CurPage = 0;

        public LoadPageEventArgs(int curPage)
        {
            m_CurPage = curPage;
        }

        public int CurPage
        {
            get { return m_CurPage; }
        }
    }
}
