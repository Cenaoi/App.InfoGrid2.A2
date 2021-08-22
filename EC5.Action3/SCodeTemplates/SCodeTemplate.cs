using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.SCodeTemplates
{


    public class SCodeTemplate
    {
        public SCodeTemplate()
        {

        }

        public SCodeTemplate(string text)
        {
            m_Text = text;
        }

        string m_Text; 

        public string Text
        {
            get { return m_Text; }
        }

        SCodeItemCollection m_Items;

        public SCodeItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new SCodeItemCollection();
                }
                return m_Items;
            }
            internal set { m_Items = value; }
        }

        public bool IsOneCode()
        {
            if (m_Items.Count != 1)
            {
                return false;
            }

            return m_Items[0].SCodeType == SCodeType.Code;
        }

        public bool IsEmpty()
        {
            return m_Items.Count == 0;
        }

        public bool IsOneString()
        {
            if (m_Items.Count != 1)
            {
                return false;
            }

            return m_Items[0].SCodeType == SCodeType.String;
        }


    }
}
