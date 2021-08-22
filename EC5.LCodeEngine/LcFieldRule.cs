using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EC5.LCodeEngine
{
    /// <summary>
    /// 代码规则
    /// </summary>
    [DebuggerDisplay("Field={Field}")]
    public class LcFieldRule : IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        /// 数据库操作对象
        /// </summary>
        public DbDecipher DbDecipher { get; set; }

        /// <summary>
        /// 监听的字段集合
        /// </summary>
        public string[] ListenFields
        {
            get { return m_ListenFields; }
            set { m_ListenFields = value; }
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


        JTemplate m_JTemplate = new JTemplate();


        public void SetParam(string paramName, object value)
        {
            m_JTemplate.SetParam(paramName, value);
        }

        public void ClearParamAll()
        {
            m_JTemplate.ClearParamAll();
        }


        /// <summary>
        /// 代码进行解析,并设置监听的字段
        /// </summary>
        public string[] CodeParse()
        {

            if(m_Code.LastIndexOf(';') > 0)
            {
                List<string> fields = new List<string>();


                string txt = m_Code;

                int n1 = -1, n2 = -1;

                do
                {
                    if (n1 > 0)
                    {
                        string nameTxt = txt.Substring(n1 + 2, n2 - n1 - 2);

                        nameTxt = nameTxt.Trim();

                        nameTxt = nameTxt.Trim('\"');


                        fields.Add(nameTxt);

                        txt = txt.Substring(n2 + 1);
                    }

                    n1 = txt.IndexOf("T[");
                    n2 = txt.IndexOf("]", n1 + 1);

                } while (n1 > -1);




                return fields.ToArray();
            }

            m_JTemplate.SrcText = m_Code.Trim();

            try
            {
                JItem[] items = m_JTemplate.GetCodeItems();

                List<string> fields = new List<string>();

                foreach (JItem item in items)
                {
                    fields.Add(item.Text);
                }

                m_ListenFields = fields.ToArray();
                return fields.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("模板计算公式解析错误,公式“{0}”", m_Code), ex);
            }

        }

        /// <summary>
        /// 字符串连接
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="fields">字段集合</param>
        /// <param name="tSqlJoinOn">两个表关联连接的 on 语句</param>
        /// <param name="tSqlWhere">筛选过滤</param>
        /// <param name="itemFormat">格式化</param>
        /// <param name="splceChar">连接字符串</param>
        private string F_StringJoin(string lab, string table, string fields, string tSqlJoinOn, string tSqlWhere, 
            string itemFormat, string splceChar)
        {
            string[] fs = StringUtil.Split(fields,",");

            for (int i = 0; i < fs.Length; i++)
            {
                fs[i] = table + "." + fs[i];
            }

            if (m_JTemplate == null)
            {
                throw new Exception("模板不能为空。");
            }

            if (m_JTemplate.Model == null)
            {
                throw new Exception("模板的实体不能为空。");
            }

            

            LModel model = m_JTemplate.Model;

            LModelElement modelElem = model.GetModelElement();

            if (modelElem == null)
            {
                throw new Exception("实体元素不能为空。");
            }


            LightModelFilter filter;

            try
            {
                filter = new LightModelFilter(table);

                filter.Joins.Add(modelElem.DBTableName, tSqlJoinOn);

                filter.And(modelElem.DBTableName + "." + modelElem.PrimaryKey, model.GetPk());

                filter.TSqlWhere = tSqlWhere;
                filter.Fields = fs;
            }
            catch (Exception ex)
            {
                throw new Exception("过滤构造对象失败.", ex);
            }


            //List<LModel> modelChilds = this.DbDecipher.GetModelList(filter);
            

            List<string> resultList = new List<string>();

            LModelReader reader;

            try
            {
                reader = this.DbDecipher.GetModelReader(filter);
            }
            catch (Exception ex)
            {
                throw new Exception("数据查询错误。", ex);
            }

            try
            { 
                while (reader.Read())
                {
                    object objValue = reader.GetObject(0);

                    if (objValue == null || DBNull.Value.Equals(objValue))
                    {
                        continue;
                    }

                    string value = objValue.ToString();

                    //if (StringUtil.IsBlank(itemFormat))
                    //{
                    //    value = mc[0].ToString();
                    //}
                    //else
                    //{
                    //    value = mc.ToString(itemFormat);
                    //}

                    if (StringUtil.IsBlank(value))
                    {
                        continue;
                    }

                    if (!resultList.Contains(value))
                    {
                        resultList.Add(value);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("等待", ex);
            }
            finally
            {
                reader.Close();
            }

            if (resultList.Count == 0)
            {
                return string.Empty;
            }


            resultList.Sort();

            splceChar = StringUtil.NoBlank(splceChar, ",");
            

            //StringBuilder sb = new StringBuilder();

            //for (int i = 0; i < resultList.Count; i++)
            //{
            //    if (i > 0)
            //    {
            //        sb.Append(splceChar);
            //    }

            //    sb.Append(resultList[i]);
            //}


            string result = StringUtil.Join(splceChar, resultList.ToArray(), 0);

            return result;
        }

        /// <summary>
        /// 根据函数执行代码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public object Exec_Func(string code)
        {
            LModel model = m_JTemplate.Model;

            if (StringUtil.StartsWith(code, "StringJoin"))
            {
                int g0 = code.IndexOf('(');
                int g1 = code.LastIndexOf(')');

                string psCode = code.Substring(g0 + 1, g1 - g0 - 1);


                try
                {
                    JArray jo = (JArray)JsonConvert.DeserializeObject(psCode);

                    string lab = jo[0].Value<string>();

                    string table = jo[1].Value<string>();

                    string fields = jo[2].Value<string>();

                    string tSqlJson = jo[3].Value<string>();

                    string tSqlWhere = jo[4].Value<string>();

                    string itemFormat = jo[5].Value<string>();

                    string spaceChar = jo[6].Value<string>();


                    string result = this.F_StringJoin(lab, table, fields, tSqlJson,tSqlWhere, itemFormat, spaceChar);

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("公式解析失败: " + code,ex);
                }

            }

            return null;
        }


        const string SCRIPT = "script=";

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public object Exec(LModel model)
        {

            string code;
            object result;


            if ((StringUtil.StartsWith(m_Code, "=")  &&　m_Code.LastIndexOf(";")  > 0) ||
                m_Code.LastIndexOf("return") > -1)
            {
                string script;

                if (m_Code.LastIndexOf("return") == -1)
                {
                    script = "return " + m_Code.Substring(1);
                }
                else
                {
                    script = m_Code.Substring(1);
                }
              

                ScriptTemplate st = ScriptTemplate.Parse(script);

                result = st.Exec(model);

                return result;
            }

            if (string.IsNullOrWhiteSpace(m_JTemplate.SrcText))
            {
                m_JTemplate.SrcText = m_Code;
            }

            m_JTemplate.Model = model;

            try
            {
                code = m_JTemplate.Exec();
            }
            catch (Exception ex)
            {
                throw new Exception($"公式解析失败, 公式不符合规则: \"{this.m_Code}\".", ex);
            }

            if (StringUtil.StartsWith(code, "="))
            {
                code = code.Substring(1);
            }


            if (StringUtil.StartsWith(code, "'"))
            {
                code = code.Substring(1);
                return code;
            }


            if (StringUtil.StartsWith(code, "F:"))
            {
                string funCode = code.Substring(2);
                result = Exec_Func(funCode);

                return result;
            }


            /**** 下列代码 Lua51 在IIS 上面无法运转起来. ****/
            //LuaInterface.Lua lua = new LuaInterface.Lua();

            //string code = "(23 * 3) + 1 + (2 * 3)";

            //object[] luaResult = lua.DoString("return (" + code + ")" );


            //// 表达式对象
            //EC5.Antlr.ExpLexer.SimpleExp _exp = new EC5.Antlr.ExpLexer.SimpleExp();

            //// 将运算操作赋给表达式
            //_exp.Operator = new EC5.Antlr.ExpLexer.SimpleOperator();

            //_exp.Expression = code;

            //try
            //{
            //    result = _exp.Eval();
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception($"执行表达式错误, 最终表达式 Code=\"{code}\" ", ex);
            //}


            try
            {
                NCalc.Expression eee = new NCalc.Expression(code);


                result = eee.Evaluate();
            }
            catch(Exception ex)
            {
                throw new Exception($"执行表达式错误, 最终表达式 Code=\"{code}\" ", ex);
            }

            if (result is string)
            {

            }
            else
            {
                try
                {
                    result = Convert.ToDecimal(result);
                }
                catch (Exception ex)
                {
                    LModelElement modelElem = model.GetModelElement();

                    throw new Exception(string.Format("表“{3}”：“{0}”无法转化为 Decimal 类型,目标字段={1}, 原公式=“{2}”。",
                        result, Field, m_Code, modelElem.DBTableName), ex);
                }
            }

            

            return result;

        }


        /// <summary>
        /// 释放内存
        /// </summary>
        public void Dispose()
        {
            m_JTemplate.Dispose();

            m_JTemplate = null;

            m_ListenFields = null;

            this.DbDecipher = null;


            GC.SuppressFinalize(this);
        }
    }
}
