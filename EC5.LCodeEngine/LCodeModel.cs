using System;
using System.Collections.Generic;
using System.Text;
using EC5.Utility;
using HWQ.Entity.LightModels;
using System.Diagnostics;
using HWQ.Entity;

namespace EC5.LCodeEngine
{




    /// <summary>
    /// 实体 
    /// </summary>
    [DebuggerDisplay("TableName={TableName}")]
    public  class LcModel
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string m_TableName;

        /// <summary>
        /// 数据表
        /// </summary>
        public string TableName
        {
            get { return m_TableName; }
            set { m_TableName = value; }
        }

        

        /// <summary>
        /// 有带公式的字段集合
        /// </summary>
        LcFieldRuleCollection m_Fields = new LcFieldRuleCollection();

        /// <summary>
        /// 监听的字段集合
        /// </summary>
        SortedList<string, LcFieldRuleCollection> m_ListenFields = new SortedList<string, LcFieldRuleCollection>();

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            m_Fields.Clear();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="field"></param>
        /// <param name="code"></param>
        public void Add(string field, string code)
        {
            if (StringUtil.IsBlank(code))
            {
                return;
            }

            if (!StringUtil.StartsWith(code,"="))
            {
                return;
            }


            LcFieldRule rule = new LcFieldRule();
            rule.Field = field;
            rule.Code = code;

            m_Fields.Add(rule);
        }

        /// <summary>
        /// 是否有监听
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool IsListen(string field)
        {
            return m_ListenFields.ContainsKey(field);
        }


        /// <summary>
        /// 调试输出
        /// </summary>
        /// <param name="model"></param>
        protected void DebugWrite(LModel model)
        {
            if (!log.IsDebugEnabled) return;

            if(model == null)
            {
                log.Debug("实体是空的哦.....");
                return;
            }

            LModelElement modelElem = model.GetModelElement();

            StringBuilder sb = new StringBuilder("实体:" + modelElem.DBTableName);
            sb.AppendLine();

            int i = 0;

            foreach (LModelFieldElement fieldElem in modelElem.Fields)
            {
                sb.AppendLine($"[{i++:00}] {fieldElem.DBField} = {model[fieldElem]}");
            }

            log.Debug(sb.ToString());
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="listenField">listenField</param>
        /// <param name="model"></param>
        /// <param name="outFields">outFields</param>
        /// <returns></returns>
        public object Exec(string listenField, LModel model,out string[] outFields)
        {
            List<string> updateFS = new List<string>();

            LcFieldRuleCollection rules = m_ListenFields[listenField];

            LModelElement modelElem = model.GetModelElement();

            LModelElement srcModelElem = modelElem;

            if (modelElem.IsTemp)
            {
                srcModelElem = LightModel.GetLModelElement(modelElem.DBTableName);
            }


            LModelFieldElement fieldElem,srcFieldElem;

            object result;
            string fieldName;

            foreach (LcFieldRule rule in rules)
            {
                fieldName = rule.Field;
                
                if (!modelElem.TryGetField(fieldName, out fieldElem))
                {
                    throw new Exception(string.Format("编码规则错误。数据表“{0}”没有“{1}”字段。", modelElem.DBTableName, fieldName));
                }

                try
                {
                    result = rule.Exec(model);
                }
                catch (Exception ex)
                {
                    DebugWrite(model);

                    throw new Exception($"执行编码规则错误。\"{rule.Code}\"", ex);
                }


                if (fieldElem.IsNumber)
                {
                    decimal resultDecimal = Convert.ToDecimal(result);

                    if (modelElem.IsTemp)
                    {
                        srcFieldElem = srcModelElem.Fields[fieldName];
                        fieldElem = srcFieldElem;
                    }

                    decimal targetDec = Math.Round(resultDecimal, fieldElem.DecimalDigits);

                    model[fieldName] = targetDec;
                }
                else
                {
                    model[fieldName] = result;
                }


                updateFS.Add(fieldName);
            }

            outFields = updateFS.ToArray();

            return null;
        }


        /// <summary>
        /// 重置规则
        /// </summary>
        public void Reset()
        {
            m_ListenFields.Clear();

            LcFieldRuleCollection rules = null;



            foreach (LcFieldRule fieldRule in m_Fields)
            {
                string[] fields = fieldRule.CodeParse();

                foreach (string field in fields)
                {
                    if (!m_ListenFields.TryGetValue(field, out rules))
                    {
                        rules = new LcFieldRuleCollection();

                        m_ListenFields.Add(field, rules);
                    }

                    rules.Add(fieldRule);
                }

            }
            
        }

        public override string ToString()
        {
            return string.Format("规则： 表={0}, 字段名={2}",
                m_TableName, m_Fields.ToString());
        }


    }






}
