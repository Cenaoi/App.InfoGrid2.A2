using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using EC5.Utility;
using System.Diagnostics;

namespace EC5.DbCascade
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class LogOpData:List<EC5.DbCascade.LogOpData.LogSet>
    {
        public string Table { get; set; }

        public string TableText { get; set; }

        public string Op { get; set; }

        /// <summary>
        /// 主键值
        /// </summary>
        public string TablePk { get; set; }

        public string OpTime { get; set; }

        [DebuggerDisplay("Field={Field}, SrcValue={SrcValue}, TarValue={TarValue}")]
        public class LogSet
        {
            /// <summary>
            /// 操作
            /// </summary>
            public string Op { get; set; }

            public string OpId { get; set; }

            /// <summary>
            /// 字段名
            /// </summary>
            public string Field { get; set; }

            public string FieldText { get; set; }

            /// <summary>
            /// 原始值
            /// </summary>
            public string SrcValue { get; set; }

            /// <summary>
            /// 目标值
            /// </summary>
            public string TarValue { get; set; }

        }



    }


    /// <summary>
    /// 操作步骤
    /// </summary>
    [DebuggerDisplay("StepType={StepType},Table={Table}")]
    public class BizDbStep:IDisposable
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 激活 日志
        /// </summary>
        bool m_LogEnabled = true;

        BizDbStepType m_StepType;

        BizDbStepCollection m_Childs;

        List<LModel> m_Models;

        string m_Table;

        /// <summary>
        /// 深度
        /// </summary>
        int m_Depth = 0;

        
        /// <summary>
        /// 操作数据的日志
        /// </summary>
        List<LogOpData> m_OpDataList;

        /// <summary>
        /// 动作ID
        /// </summary>
        public int ActionId { get; set; }

        /// <summary>
        /// 数据表描述
        /// </summary>
        public string TableText { get; set; }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// 是否需要弹出
        /// </summary>
        public bool IsDialogMsg { get; set; }

        /// <summary>
        /// 操作日志
        /// </summary>
        public List<LogOpData> OpDataList
        {
            get { return m_OpDataList; }
        }

        /// <summary>
        /// 激活日志记录.默认启动
        /// </summary>
        public bool LogEnabled
        {
            get { return m_LogEnabled; }
            set { m_LogEnabled = value; }
        }

        public string Table
        {
            get { return m_Table; }
            set { m_Table = value; }
        }

        /// <summary>
        /// 深度
        /// </summary>
        public int Depth
        {
            get { return m_Depth; }
            set { m_Depth = value; }
        }

        #region 构造函数

        /// <summary>
        /// 步骤（构造函数）
        /// </summary>
        public BizDbStep()
        {
        }

        /// <summary>
        /// 步骤（构造函数）
        /// </summary>
        /// <param name="stepType">执行步骤的类型</param>
        public BizDbStep(BizDbStepType stepType)
        {
            m_StepType = stepType;
        }

        /// <summary>
        /// 步骤（构造函数）
        /// </summary>
        /// <param name="stepType">执行步骤的类型</param>
        /// <param name="model">收影响的实体</param>
        public BizDbStep(BizDbStepType stepType, LModel model)
        {
            m_StepType = stepType;

            this.Models.Add(model);
        }


        /// <summary>
        /// 步骤（构造函数）
        /// </summary>
        /// <param name="stepType">执行步骤的类型</param>
        /// <param name="model">收影响的实体集合</param>
        public BizDbStep(BizDbStepType stepType, IList<LModel> models)
        {
            m_StepType = stepType;

            this.Models.AddRange(models);
        }

        /// <summary>
        /// 步骤（构造函数）
        /// </summary>
        /// <param name="stepType">执行步骤的类型。INSERT,UPDATE,DELETE</param>
        public BizDbStep(string stepType)
        {
            m_StepType = EnumUtil.Parse<BizDbStepType>(stepType,true);
        }

        /// <summary>
        /// 步骤（构造函数）
        /// </summary>
        /// <param name="stepType">执行步骤的类型</param>
        /// <param name="model">收影响的实体</param>
        public BizDbStep(string stepType, LModel model)
        {
            m_StepType = EnumUtil.Parse<BizDbStepType>(stepType, true);

            this.Models.Add(model);
        }

        /// <summary>
        /// 步骤（构造函数）
        /// </summary>
        /// <param name="stepType">执行步骤的类型</param>
        /// <param name="models">收影响的实体</param>
        public BizDbStep(string stepType, IList<LModel> models)
        {
            m_StepType = EnumUtil.Parse<BizDbStepType>(stepType, true);

            this.Models.AddRange(models);
        }

        #endregion


        /// <summary>
        /// 是否有子节点
        /// </summary>
        /// <returns></returns>
        public bool HasChild()
        {
            return (m_Childs != null && m_Childs.Count > 0);
        }

        /// <summary>
        /// 子步骤集合
        /// </summary>
        public BizDbStepCollection Childs
        {
            get
            {
                if (m_Childs == null)
                {
                    m_Childs = new BizDbStepCollection(this);
                }

                return m_Childs;
            }
        }

        /// <summary>
        /// 步骤名称
        /// </summary>
        public BizDbStepType StepType
        {
            get { return m_StepType; }
            set { m_StepType = value; }
        }

        /// <summary>
        /// 当前步骤，受到影响的实体集
        /// </summary>
        public List<LModel> Models
        {
            get
            {
                if (m_Models == null)
                {
                    m_Models = new List<LModel>();
                }
                return m_Models;
            }
            set { m_Models = value; }
        }

        /// <summary>
        /// 创建日志
        /// </summary>
        public void CreateLog()
        {
            try
            {
                ProCreateLog();
            }
            catch (Exception ex)
            {
                log.Error("创建联动日志失败.", ex);
            }

           
        }


        public void ProCreateLog()
        {
            if (!m_LogEnabled || m_Models == null || m_Models.Count == 0)
            {
                return;
            }

            //如果没有操作，或阻止。跳掉
            if (m_StepType == BizDbStepType.NONE)
            {
                return;
            }

            if (m_OpDataList == null)
            {
                m_OpDataList = new List<LogOpData>();
            }

            LModelElement modelElem;


            foreach (var model in m_Models)
            {

                string[] bFields;

                modelElem = model.GetModelElement();

                if (this.m_StepType == BizDbStepType.UPDATE)
                {
                    bFields = LightModel.GetBlemishPropNames(model);
                }
                else
                {
                    bFields = modelElem.Fields.ToStringArray(LModelFieldTypes.General);
                }

                if (bFields == null || bFields.Length == 0)
                {
                    continue;
                }

                LModelFieldElement fieldElem;

                LogOpData opData = new LogOpData();
                opData.Table = m_Table;
                opData.TableText = modelElem.Description;
                opData.TablePk = model.GetPk().ToString();

                opData.Op = m_StepType.ToString();

                opData.OpTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");


                object srcValue = null;
                object tarValue = null;

                foreach (var field in bFields)
                {
                    if (model.GetTakeChange())
                    {
                        srcValue = model.GetOriginalValue(field);
                    }
                    else
                    {
                        srcValue = null;
                    }

                    tarValue = model[field];

                    LogOpData.LogSet logSet = new LogOpData.LogSet();
                    logSet.Field = field;

                    if (srcValue != null && srcValue.GetType() == typeof(DateTime))
                    {
                        logSet.SrcValue = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", srcValue);
                    }
                    else
                    {
                        logSet.SrcValue = (srcValue == null ? "" : srcValue.ToString());
                    }

                    if (tarValue != null && tarValue.GetType() == typeof(DateTime))
                    {
                        logSet.TarValue = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", tarValue);
                    }
                    else
                    {
                        logSet.TarValue = (tarValue == null ? "" : tarValue.ToString());
                    }

                    



                    if (modelElem.TryGetField(field, out fieldElem))
                    {
                        logSet.FieldText = StringUtil.NoBlank(fieldElem.Description, fieldElem.Caption);
                    }

                    opData.Add(logSet);
                }

                m_OpDataList.Add(opData);

            }
        }

        /// <summary>
        /// 递归查找下级数据
        /// </summary>
        /// <param name="pStep"></param>
        /// <param name="items"></param>
        private void Recursion(BizDbStep pStep, BizDbStepCollection items)
        {

            if (!pStep.HasChild())
            {
                return;
            }

            foreach (BizDbStep step in pStep.m_Childs)
            {
                items.Add(step);

                Recursion(step, items);
            }

        }

        /// <summary>
        /// 树结构转换为一维数组
        /// </summary>
        /// <returns></returns>
        public BizDbStep[] ToArray()
        {
            BizDbStepCollection items = new BizDbStepCollection(null);

            items.Add(this);

            Recursion(this, items);

            return items.ToArray();
        }


        /// <summary>
        /// 释放内存
        /// </summary>
        public void Dispose()
        {
            if (m_Childs != null)
            {
                m_Childs.Dispose();
                m_Childs = null;
            }

            m_Models = null;
            m_OpDataList = null;

            GC.SuppressFinalize(this);
        }


    }

}
