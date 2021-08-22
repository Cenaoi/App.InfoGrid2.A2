using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EC5.Action3.SCodeTemplates
{
    [DebuggerDisplay("{SCodeType}: {Text}")]
    public class SCodeItem
    {
        public string Text { get; set; }

        public SCodeType SCodeType { get; set; } = SCodeType.String;

        public SCodeItem()
        {

        }

        public SCodeItem(string text)
        {
            this.Text = text;
        }

        public SCodeItem(string text, SCodeType scodeType)
        {
            this.Text = text;
            this.SCodeType = scodeType;
        }
    }








}
