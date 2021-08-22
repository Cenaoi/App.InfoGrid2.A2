using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.Steps
{



    public class StepGroup : CodeRegion
    {
        public object Data { get; set; }

        /// <summary>
        /// 当前监听的索引
        /// </summary>
        int m_ListenIndex = 0;

        /// <summary>
        /// 操作步骤索引
        /// </summary>
        int m_OperateIndex = 0;

        List<StepNode> m_Listens;

        List<StepNode> m_Operates;


        public List<StepNode> Listens
        {
            get
            {
                if (m_Listens == null)
                {
                    m_Listens = new List<StepNode>();
                }
                return m_Listens;
            }
        }

        public List<StepNode> Operates
        {
            get
            {
                if (m_Operates == null)
                {
                    m_Operates = new List<StepNode>();
                }
                return m_Operates;
            }
        }

        public override bool Read()
        {
            return base.Read();
        }

    }
}
