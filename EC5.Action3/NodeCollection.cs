using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{

    /// <summary>
    /// 节点集合
    /// </summary>
    public class NodeCollection :ActionList<ActionItemBase>, IEnumerable<ActionItemBase>
    {
        public IEnumerator<ActionItemBase> GetEnumerator()
        {
            return this.Items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.Values.GetEnumerator();
        }
    }
}
