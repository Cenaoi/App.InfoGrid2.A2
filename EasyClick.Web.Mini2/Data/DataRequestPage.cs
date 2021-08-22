using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2.Data
{
    public class DataRequestPage
    {
        public static readonly DataRequestPage Empty = new DataRequestPage();

        int m_CurrentPage;

        int m_Start;
        int m_Limit;
        int m_End;

        public int CurrentPage
        {
            get { return m_CurrentPage; }
            set { m_CurrentPage = value; }
        }

        public int Start
        {
            get { return m_Start; }
            set { m_Start = value; }
        }

        public int Limit
        {
            get { return m_Limit; }
            set { m_Limit = value; }
        }

        public int End
        {
            get { return m_End; }
            set { m_End = value; }
        }



    }
}
