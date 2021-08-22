using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;

namespace EC5.Entity.Expanding.ExpandV1
{
    /// <summary>
    /// 表单过滤项
    /// </summary>
    public struct ModelTreeFilterCondition
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public object Value2 { get; set; }

        public Logic Logic { get; set; } 

        public ModelTreeFilterCondition(string name, object value)
        {
            this.Name = name;
            this.Value = value;

            this.Value2 = null;
            this.Logic = Logic.Equality;
        }


        public ModelTreeFilterCondition(string name, object value, Logic logic )
        {
            this.Name = name;
            this.Value = value;

            this.Value2 = null;
            this.Logic = logic;
        }
    }

    public class ModelTreeFilterConditionCollection :List<ModelTreeFilterCondition>
    {
        public void Add(string field, object value)
        {
            base.Add(new ModelTreeFilterCondition(field, value));
        }

        public void Add(string field, object value, Logic logic)
        {
            base.Add(new ModelTreeFilterCondition(field, value, logic));
        }
    }

    /// <summary>
    /// 实体表单过滤
    /// </summary>
    public class ModelTreeFilter:ModelTreeFilterNode
    {
        /// <summary>
        /// 过滤函数
        /// </summary>
        /// <param name="mainTable">主表名称</param>
        /// <param name="childTable">子表名称</param>
        public ModelTreeFilter(string mainTable)
        {
            this.Table = mainTable;
            
        }

        public ModelTreeFilter(string mainTable, ModelTeeeNodeAssc assc)
        {
            this.Table = mainTable;

            ModelTreeFilterNode node = new ModelTreeFilterNode();
            node.Table = assc.Table;
            node.Assc = new FormNodeAssociation
            {
                CurField = assc.CurField,
                TargetField = assc.TargetField
            };

            this.Childs.Add(node);
        }
        

    }

    public class ModelTeeeNodeAssc
    {
        public string Table { get; set; }

        public string CurField { get; set; }

        public string TargetField { get; set; }

        public static ModelTeeeNodeAssc Join(string table, string curField, string targetField)
        {
            ModelTeeeNodeAssc fa = new ModelTeeeNodeAssc
            {
                Table = table,
                CurField = curField,
                TargetField = targetField
            };

            return fa;
        }
    }

    public class ModelTreeFilterNodeCollecton :IEnumerable<ModelTreeFilterNode>
    {
        ModelTreeFilterNode m_Owner;

        List<ModelTreeFilterNode> m_Items = new List<ModelTreeFilterNode>();

        public ModelTreeFilterNodeCollecton()
        {

        }

        public ModelTreeFilterNodeCollecton(ModelTreeFilterNode owner)
        {
            m_Owner = owner;
        }


        public ModelTreeFilterNode this[int index]
        {
            get { return m_Items[index]; }
        }

        public void Add(ModelTreeFilterNode item)
        {
            m_Items.Add(item);
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public void Clear()
        {
            m_Items.Clear();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Items.GetEnumerator() ;
        }

        IEnumerator<ModelTreeFilterNode> IEnumerable<ModelTreeFilterNode>.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }
    }

    /// <summary>
    /// 表单节点
    /// </summary>
    public class ModelTreeFilterNode
    {
        ModelTreeFilterNodeCollecton m_Childs;

        ModelTreeFilterConditionCollection m_FilterConditions;

        public string Table { get; set; }


        public void And(string field,object value)
        {
            this.FilterConditions.Add(field, value);
        }

        /// <summary>
        /// 过滤条件
        /// </summary>
        public ModelTreeFilterConditionCollection FilterConditions
        {
            get
            {
                if (m_FilterConditions == null)
                {
                    m_FilterConditions = new ModelTreeFilterConditionCollection();
                }
                return m_FilterConditions;
            }
            set { m_FilterConditions = value; }
        }

        public ModelTreeFilterNodeCollecton Childs
        {
            get
            {
                if(m_Childs == null)
                {
                    m_Childs = new ModelTreeFilterNodeCollecton(this);
                }

                return m_Childs;
            }
        }

        /// <summary>
        /// 关联
        /// </summary>
        FormNodeAssociation m_Assc;

        /// <summary>
        /// 关联
        /// </summary>
        public FormNodeAssociation Assc
        {
            get
            {
                if(m_Assc == null)
                {
                    m_Assc = new FormNodeAssociation();
                }
                return m_Assc;
            }
            set { m_Assc = value; }
        }



    }

    /// <summary>
    /// 节点关联
    /// </summary>
    public class FormNodeAssociation
    {

        /// <summary>
        /// 自身字段
        /// </summary>
        public string CurField { get; set; }

        /// <summary>
        /// 目标表
        /// </summary>
        public string TargetTable { get; set; }

        /// <summary>
        /// 关联的目标
        /// </summary>
        public string TargetField { get; set; }
    }
    
}
