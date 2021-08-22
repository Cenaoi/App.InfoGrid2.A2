using System;
using System.Collections.Generic;
using System.Text;
using EC5.Utility;
using HWQ.Entity.LightModels;
using System.Diagnostics;
using HWQ.Entity;

namespace EasyClick.BizWeb2.LCodeEngine
{

    public class LcModelSorted : SortedList<string, LcModel>
    {

    }


    /// <summary>
    /// 规则管理器
    /// </summary>
    public static class LcModelManager
    {
        static LcModelSorted m_Models = new LcModelSorted();

        public static LcModelSorted Models
        {
            get { return m_Models; }
            set { m_Models = value; }
        }


    }



    /// <summary>
    /// 实体 
    /// </summary>
    [DebuggerDisplay("TableName={TableName}")]
    public  class LcModel
    {
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

            if (!code.StartsWith("="))
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
        /// 执行
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object Exec(string listenField, LModel model,out string[] outFields)
        {
            List<string> updateFS = new List<string>();

            LcFieldRuleCollection rules = m_ListenFields[listenField];

            LModelElement modelElem = model.GetModelElement();

            LModelFieldElement fieldElem;

            foreach (LcFieldRule rule in rules)
            {
                object result =  rule.Exec(model);

                fieldElem = modelElem.Fields[rule.Field];

                if (ModelHelper.IsNumberType(fieldElem.DBType))
                {
                    decimal resultDecimal = (decimal)result;

                    resultDecimal = Math.Round(resultDecimal, fieldElem.DecimalDigits);

                    model[rule.Field] = resultDecimal;
                }
                else
                {
                    model[rule.Field] = result;
                }


                updateFS.Add(rule.Field);
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


    }

    /// <summary>
    /// 代码规则集合
    /// </summary>
    public class LcFieldRuleCollection:List<LcFieldRule>
    {

    }

    /// <summary>
    /// 代码规则
    /// </summary>
    [DebuggerDisplay("Field={Field}")]
    public class LcFieldRule
    {
        /// <summary>
        /// 字段名
        /// </summary>
        string m_Field;
        string m_Code;

        /// <summary>
        /// 监听的字段集合
        /// </summary>
        string[] m_ListenFields;

        /// <summary>
        /// 监听的字段集合
        /// </summary>
        public string[] ListenFields
        {
            get { return m_ListenFields; }
            set { m_ListenFields = value;}
        }

        /// <summary>
        /// 原代码
        /// </summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Field
        {
            get { return m_Field; }
            set { m_Field = value; }
        }


        JTemplate m_jt;


        /// <summary>
        /// 代码进行解析,并设置监听的字段
        /// </summary>
        public string[] CodeParse()
        {
            m_jt = new JTemplate(m_Code);

            JItem[] items = m_jt.GetCodeItems();

            List<string> fields = new List<string>();

            foreach (JItem item in items)
            {
                fields.Add(item.Text);
            }

            m_ListenFields = fields.ToArray();

            return fields.ToArray();

        }

        public object Exec(LModel model)
        {
            m_jt.Model = model;

            string code = m_jt.Exec();

            if (code.StartsWith("="))
            {
                code = code.Substring(1);
            }

            /**** 下列代码 Lua51 在IIS 上面无法运转起来. ****/
            //LuaInterface.Lua lua = new LuaInterface.Lua();

            //string code = "(23 * 3) + 1 + (2 * 3)";
            
            //object[] luaResult = lua.DoString("return (" + code + ")" );


            // 表达式对象
            EC5.Antlr.ExpLexer.SimpleExp _exp = new EC5.Antlr.ExpLexer.SimpleExp();

            // 将运算操作赋给表达式
            _exp.Operator = new EC5.Antlr.ExpLexer.SimpleOperator();

            _exp.Expression = code;

            object result = _exp.Eval();

            if (result is string)
            {

            }
            else
            {
                result = Convert.ChangeType(result, TypeCode.Decimal);
            }

            return result ;

        }


    }



}
