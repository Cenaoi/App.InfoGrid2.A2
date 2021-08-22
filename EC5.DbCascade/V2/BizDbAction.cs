using EC5.DbCascade.DbCascadeEngine;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade.V2
{
    
    public class BizDbActionItem
    {
        object m_Data;

        BizDbStep m_Step;

        DbccModel m_DbccModel;

        object m_SrcModel;

        public object SrcModel
        {
            get { return m_SrcModel; }
            set { m_SrcModel = value; }
        }

        public BizDbActionItem()
        {
        }

        public BizDbActionItem(object data)
        {
            m_Data = data;
        }

        public DbccModel DbccModel
        {
            get { return m_DbccModel; }
            set { m_DbccModel = value; }
        }


        public object Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }


        public BizDbStep Step
        {
            get { return m_Step; }
            set { m_Step = value; }
        }


    }



    public class BizDbAction
    {
        DbOperate m_Op;


        string m_Table;

        /// <summary>
        /// 执行中
        /// </summary>
        public bool m_Shelling = true;

        DbccModel m_DbccModel;

        Queue<BizDbActionItem> m_Items = new Queue<BizDbActionItem>();
        Queue<BizDbActionItem> m_History = new Queue<BizDbActionItem>();

        object m_SrcModel;


        public object SrcModel
        {
            get { return m_SrcModel; }
            set { m_SrcModel = value; }
        }


        public DbccModel DbccModel
        {
            get { return m_DbccModel; }
            set { m_DbccModel = value; }
        }


        /// <summary>
        /// 操作类型
        /// </summary>
        public DbOperate Op
        {
            get { return m_Op; }
            set { m_Op = value; }
        }

        /// <summary>
        /// 执行中
        /// </summary>
        public bool Shelling
        {
            get { return m_Shelling; }
            set { m_Shelling = value; }
        }

        /// <summary>
        /// 需要处理的实体
        /// </summary>
        public Queue<BizDbActionItem> Items
        {
            get { return m_Items; }
        }

        /// <summary>
        /// 已经操作的历史记录
        /// </summary>
        public Queue<BizDbActionItem> History
        {
            get { return m_History; }
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string Table
        {
            get { return m_Table; }
            set { m_Table = value; }
        }

        
        public BizDbAction(DbOperate op, LModelList<LModel> models)
        {
            m_Op = op;

            foreach (var item in models)
            {
                BizDbActionItem aItem = new BizDbActionItem(item);
                m_Items.Enqueue(aItem);
            }

        }

        public BizDbAction(DbOperate op, LModel model)
        {
            m_Op = op;

            m_Items.Enqueue(new BizDbActionItem(model));
        }

        public BizDbAction(DbOperate op, int[] pkList)
        {
            m_Op = op;

            foreach (var item in pkList)
            {
                BizDbActionItem aItem = new BizDbActionItem(item);
                m_Items.Enqueue(aItem);
            }
        }


        public BizDbAction(DbOperate op, int pk)
        {
            m_Op = op;

            m_Items.Enqueue(new BizDbActionItem(pk));

        }

        public BizDbAction(DbOperate op, DbccModel dbccModel, object srcModel)
        {
            m_Op = op;
            m_Items.Enqueue(new BizDbActionItem() { DbccModel = dbccModel, SrcModel = srcModel });
        }

        public BizDbAction(DbOperate op)
        {
            m_Op = op;
        }

        /// <summary>
        /// 联动规则的 ID
        /// </summary>
        public int ActionID { get; set; }
    }


}
