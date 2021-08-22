using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{
    /// <summary>
    /// 操作集合
    /// </summary>
    public class OperateCollection : ActionList<OperateBase>, IEnumerable<OperateBase>
    {
        public IEnumerator<OperateBase> GetEnumerator()
        {
            return this.Items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.Values.GetEnumerator();
        }
    }
}
