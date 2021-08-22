using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{
    /// <summary>
    /// 带 T-SQL 格式的字符串
    /// </summary>
    public sealed class ScriptTSql : ScriptBase
    {
        public ScriptTSql()
        {

        }

        public ScriptTSql(string code)
        {
            this.Code = code;
        }
    }
}
