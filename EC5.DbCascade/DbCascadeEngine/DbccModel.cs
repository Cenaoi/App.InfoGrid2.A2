using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade.DbCascadeEngine
{

    /// <summary>
    /// 
    /// </summary>
    public class DbccModel
    {

        public int ID { get; set; }

        public string R_Table { get; set; }

        public string R_Display { get;set;}

        public string R_ActCode { get; set; }


        public string L_Table { get; set; }

        public string L_Display { get; set; }

        public string L_ActCode { get; set; }

        /// <summary>
        /// 如果更新的记录不存在，就创建一条记录。
        /// </summary>
        public string L_NotExist_Then { get; set; }

        /// <summary>
        /// 过滤子项集合
        /// </summary>
        public bool R_IsSubFilter { get; set; }

        /// <summary>
        /// 过滤子项集合
        /// </summary>
        public bool L_IsSubFilter { get; set; }

        /// <summary>
        /// 激活
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 监听逻辑
        /// </summary>
        public DbccLogic ListenLogic { get; set; }




        DbccItemCollection m_Items = new DbccItemCollection();

        DbccFilterItemCollection m_FilterLeft = new DbccFilterItemCollection();

        /// <summary>
        /// 条件集合
        /// </summary>
        DbccThenCollection m_Thens = new DbccThenCollection();

        DbccFilterItemCollection m_FilterRigth = new DbccFilterItemCollection();

        /// <summary>
        /// 监听字段
        /// </summary>
        DbccListenCollection m_ListenFields = null;

        /// <summary>
        /// 是否有需要监听的字段
        /// </summary>
        /// <returns></returns>
        public bool HasListen()
        {
            if (m_ListenFields == null)
            {
                return false;
            }

            return (m_ListenFields.Count > 0);
        }

        /// <summary>
        /// 监听字段集合
        /// </summary>
        public DbccListenCollection ListenFields
        {
            get
            {
                if (m_ListenFields == null)
                {
                    m_ListenFields = new DbccListenCollection();
                }
                return m_ListenFields;
            }
        }


        /// <summary>
        /// 对字段赋值
        /// </summary>
        public DbccItemCollection Items
        {
            get { return m_Items; }
        }

        /// <summary>
        /// 条件集合
        /// </summary>
        public DbccThenCollection Thens
        {
            get { return m_Thens; }
        }

        /// <summary>
        /// 左边过滤条件
        /// </summary>
        public DbccFilterItemCollection FilterLeft
        {
            get { return m_FilterLeft; }
        }

        /// <summary>
        /// 右边过滤条件
        /// </summary>
        public DbccFilterItemCollection FilterRight
        {
            get { return m_FilterRigth; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 自动继续触发联动
        /// </summary>
        public bool AutoContinue { get; set; }

        /// <summary>
        /// 激活新建的脚本
        /// </summary>
        public bool ActNewEnabeld { get; set; }
        /// <summary>
        /// 新建记录后触发的脚本
        /// </summary>
        public string ActNewSCode { get; set; }

        /// <summary>
        /// 激活更新的脚本
        /// </summary>
        public bool ActUpdateEnabled { get; set; }

        /// <summary>
        /// 更新记录后触发的脚本
        /// </summary>
        public string ActUpdateSCode { get; set; }

    }


}
