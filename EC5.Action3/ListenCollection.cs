using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{

    /// <summary>
    /// 监听集合
    /// </summary>
    public class ListenCollection : ActionList<ListenBase>, IEnumerable<ListenBase>
    {
        public IEnumerator<ListenBase> GetEnumerator()
        {
            return this.Items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.Values.GetEnumerator();
        }
    }
}
