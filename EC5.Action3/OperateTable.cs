using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{
    


    /// <summary>
    /// 操作表基类
    /// </summary>
    public class OperateTable : OperateBase
    {
        public OperateTable()
        {

        }

        public OperateTable(string code) : base(code)
        {
            this.Table = code;
        }

        
        public OperateTable(string code, OperateMethod method):base(code)
        {
            this.Table = code;
            this.Method = method;
        }


        /// <summary>
        /// 自动继续运行, 并触发新的联动..如果不关闭,有可能会出现死循环
        /// </summary>
        public bool AutoContinue { get; set; } = true;


        public string Table { get; set; }


        public string TableText { get; set; }

        /// <summary>
        /// 操作函数
        /// </summary>
        public OperateMethod Method { get; set; }



        /// <summary>
        /// 过滤脚本
        /// </summary>
        public ScriptBase FilterScript { get; set; }

        /// <summary>
        /// 条件过滤 ( 通过 FilterScript 编译后产生, XML 或 Json 产生的对象)
        /// </summary>
        public FilterItem Filter { get; set; }


        /// <summary>
        /// 操作的混合模式
        /// </summary>
        public OperateMixingMode MixingMode { get; set; } = OperateMixingMode.None;

        /// <summary>
        /// 动作操作脚本
        /// </summary>
        public ScriptBase Script { get; set; }

        /// <summary>
        /// 图形脚本
        /// </summary>
        public ScriptBase DwgScript { get; set; }



        /// <summary>
        /// 更新的字段
        /// </summary>
        OperateFieldCollection m_UpdateFields;

        /// <summary>
        /// 新建字段
        /// </summary>
        OperateFieldCollection m_NewFields;

        /// <summary>
        /// 更新字段
        /// </summary>
        public OperateFieldCollection UpdateFields
        {
            get
            {
                if(m_UpdateFields == null)
                {
                    m_UpdateFields = new OperateFieldCollection();
                }

                return m_UpdateFields;
            }
        }

        /// <summary>
        /// 新建字段
        /// </summary>
        public OperateFieldCollection NewFields
        {
            get
            {
                if(m_NewFields == null)
                {
                    m_NewFields = new OperateFieldCollection();
                }

                return m_NewFields;
            }
        }

        /// <summary>
        /// 更新后的代码
        /// </summary>
        public ScriptBase UpdatedScript { get; set; }

        /// <summary>
        /// 更新前编码
        /// </summary>
        public ScriptBase UpdatingScript { get; set; }

        /// <summary>
        /// 插入后代码
        /// </summary>
        public ScriptBase InsertedScript { get; set; }

        /// <summary>
        /// 插入前代码
        /// </summary>
        public ScriptBase InsertingScript { get; set; }


        /// <summary>
        /// 删除前的编码
        /// </summary>
        public ScriptBase DeletingScript { get; set; }

        /// <summary>
        /// 删除后的编码
        /// </summary>
        public ScriptBase DeletedScript { get; set; }
    }
}
