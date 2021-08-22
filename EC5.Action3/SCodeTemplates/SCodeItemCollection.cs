using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.SCodeTemplates
{

    public class SCodeItemCollection : List<SCodeItem>
    {
        public void Add(string text)
        {
            SCodeItem item = new SCodeItem(text);
            base.Add(item);
        }

        public void Add(string text, SCodeType scodeType)
        {
            SCodeItem item = new SCodeItem(text, scodeType);
            base.Add(item);
        }

        public void AddCode(string text)
        {
            SCodeItem item = new SCodeItem(text, SCodeType.Code);
            base.Add(item);
        }
    }

}
