using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using System.Collections;
using HWQ.Entity.Decipher.LightDecipher;

namespace App.InfoGrid2.Excel_Template
{
    /// <summary>
    /// 模板项目类型
    /// </summary>
    public enum JItemType
    {
        /// <summary>
        /// 普通字符串
        /// </summary>
        String,
        /// <summary>
        /// 代码
        /// </summary>
        Code
    }


    /// <summary>
    /// 模板项目
    /// </summary>
    public class JItem
    {
       

        public JItem()
        {
        }

        public JItem(JItemType itemType, string itemText)
        {
            this.ItemType = itemType;
            this.Text = itemText;
        }

        /// <summary>
        /// 条目类型
        /// </summary>
        public JItemType ItemType { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string Text { get; set; }



    }

    /// <summary>
    /// 模板项目集合
    /// </summary>
    public class JItemCollection : List<JItem>
    {

    }

    /// <summary>
    /// 模板引擎
    /// </summary>
    public class JTemplate
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 原字符串
        /// </summary>
        string m_SrcText;

        /// <summary>
        /// 返回的字符串
        /// </summary>
        string m_ResultText;

        /// <summary>
        /// 执行
        /// </summary>
        JItemCollection m_Items;



        /// <summary>
        /// 序号
        /// </summary>
        public int RowIndex { get; set; }


        public JTemplate()
        {

        }

        /// <summary>
        /// (构造函数)模板引擎
        /// </summary>
        /// <param name="srcText"></param>
        public JTemplate(string srcText)
        {
            m_SrcText = srcText;
        }

        /// <summary>
        /// 处理后返回的字符串
        /// </summary>
        public string ResultText
        {
            get { return m_ResultText; }
            set { m_ResultText = value; }
        }

        /// <summary>
        /// 原字符串
        /// </summary>
        public string SrcText
        {
            get { return m_SrcText; }
            set { m_SrcText = value; }
        }


        LModel m_Model;

        List<LModel> m_ModelList;


        public List<LModel> ModelList
        {
            get { return m_ModelList; }
            set { m_ModelList = value; }
        }

        public LModel Model
        {
            get { return m_Model; }
            set { m_Model = value; }
        }

        /// <summary>
        /// 处理执行
        /// </summary>
        /// <returns></returns>
        public string Exec()
        {
            if (m_Model == null)
            {
                throw new Exception("没有给数据实体,无法解析执行.");
            }

            m_Items = CodeAnalysis();

            StringBuilder sb = new StringBuilder();

            LModelElement modelElem = m_Model.GetModelElement();

            foreach (JItem item in m_Items)
            {
                switch (item.ItemType)
                {
                    case JItemType.String: sb.Append(item.Text);
                        break;
                    case JItemType.Code:

                        string result = ParseItem_ForCode_Table(item, modelElem);

                        sb.Append(result);

                        break;
                }
            }

            m_ResultText = sb.ToString();

            return m_ResultText;
        }


        /// <summary>
        /// 当前对象
        /// </summary>
        /// <param name="item"></param>
        /// <param name="modelElem"></param>
        /// <returns></returns>
        private string ParseItem_ForCode_Table(JItem item, LModelElement modelElem)
        {
            string result = null;

            LModelFieldElement fieldElem = null;

            string text = item.Text;

            int remarkIndex = text.LastIndexOf(':');

            if (remarkIndex > 0)
            {
                text = text.Substring(0, remarkIndex);
            }

            string[] paramList = EC5.Utility.StringUtil.Split(text, ",");

            string field = paramList[0];

            if (modelElem.TryGetField(field, out fieldElem))
            {
                object mValue = m_Model[fieldElem];

                if (mValue == null)
                {
                    return string.Empty;
                }

                if (mValue != null)
                {
                    string fieldValue = mValue.ToString();
                    result = fieldValue;
                }
                else if (fieldElem.IsNumber)
                {
                    result = "0";
                }
                else 
                {

                }

                if (paramList.Length > 1)
                {
                    //暂时只取一个参数

                    string ps = paramList[1];

                    if (ps == "RMB_DX")
                    {
                        result = EC5.Utility.Culture.Zh.RmbUtil.ConvertSum(result);
                    }
                    else if (ps == "F2" || ps == "F0" || ps == "F1" || ps == "F3" || ps == "F4")
                    {
                        decimal value = decimal.Parse(result);

                        log.Debug(string.Format("{0}:{1}", ps, value.ToString(ps)));

                        result = value.ToString(ps);
                    }
                    else if (ps.StartsWith("0."))
                    {
                        decimal value = decimal.Parse(result);

                        log.Debug(string.Format("{0}:{1}", ps, value.ToString(ps)));

                        result = value.ToString(ps);
                    }
                    else if (ps == "DATE")
                    {
                        DateTime value;

                        if (DateTime.TryParse(result, out value))
                        {
                            result = value.ToString("yyyy-MM-dd");
                        }
                    }
                    else if (ps == "TIME")
                    {
                        DateTime value;
                        if (DateTime.TryParse(result, out value))
                        {
                            result = value.ToString("HH:mm:ss");
                        }
                    }
                    else if (ps == "DATETIME")
                    {
                        DateTime valeu;
                        if (DateTime.TryParse(result, out valeu))
                        {
                            result = valeu.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    else if (m_ModelList != null)
                    {
                        if (ps == "SUM")
                        {
                            result = LModelMath.Sum(m_ModelList, field).ToString("0.##########");
                        }
                        else if (ps == "COUNT")
                        {
                            result = m_ModelList.Count.ToString();
                        }
                        else if (ps == "AVG")
                        {
                            result = LModelMath.Avg<decimal>(m_ModelList, field).ToString("0.##########");
                        }
                        else if (ps == "MIN")
                        {
                            result = LModelMath.Min<decimal>(m_ModelList, field).ToString("0.##########");
                        }
                        else if (ps == "MAX")
                        {
                            result = LModelMath.Max<decimal>(m_ModelList, field).ToString("0.##########");
                        }
                    }
                    

                }
            }
            else
            {
                //如果是P的话就直接返回参数
                if(item.Prefix == "P")
                {
                    if (item.Text == "ROW_INDEX")
                    {

                        result = (RowIndex + 1).ToString();

                    }
                    else
                    {

                        result = "{$" + item.Prefix + "." + item.Text + "}";
                    }

                }

                if(item.Prefix == "SQL")
                {

                    // 变量示例  <$T.COL_1/>
                    // 整个示例  {$SQL.select COL_1 from COL_2 = '<$T.COL_2/>' }

                    string sqlFormat = item.Text;

                    JSqlTemplate jsql = new JSqlTemplate(sqlFormat);

                    jsql.Model = this.Model;
                    jsql.ModelList = this.ModelList;

                    string tSql = jsql.Exec();

                    result = GetDataForSQL(tSql);



                }


                //这是字段找不到的话，直接返回空字符了，不用返回原生的数据了
                //result = "{$" + item.Prefix + "." + item.Text + "}";
            }

            return result;
        }


        private string GetDataForSQL(string tSql)
        {
            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    return GetDataForSql(decipher, tSql);
                }
            }
            catch(Exception ex)
            {
                log.Error("获取数据错误, T-SQL = " + tSql, ex);
                return "(获取数据出错)";
            }

        }

        private string GetDataForSql(DbDecipher decipher, string tSql)
        {
            SModelList smList = decipher.GetSModelList(tSql);

            StringBuilder sb = new StringBuilder();


            foreach (var sm in smList)
            {
                string[] fields = sm.GetFields();


                int n = 0;

                foreach (var field in fields)
                {
                    object value = sm[field];

                    if(n++ > 0) { sb.Append(" "); }

                    sb.Append(field).Append(":");

                    if (value != null)
                    {
                        if (value.GetType() == typeof(DateTime))
                        {
                            sb.Append($"{value:yyyy-MM-dd HH:mm}");
                        }
                        else
                        {
                            sb.Append(value);
                        }
                    }                    

                }
                //这是要不要换行的意思
                sb.Append(" ; ");
            }

            string data = sb.ToString();

            return data;

        }


       



        /// <summary>
        /// 代码解析
        /// </summary>
        private JItemCollection CodeAnalysis()
        {
            string srcText = m_SrcText;

            int n1 = srcText.IndexOf("{$");
            int lastN = 0;
            int n2 = 0;

            string itemFirstStr;
            string itemStr;

            JItemCollection items = new JItemCollection();

            while (n1 >= 0)
            {
                lastN = srcText.IndexOf('}', n1 + 1);

                if (lastN == -1)
                {
                    throw new Exception("模板错误,没有结束符号。" + m_SrcText);
                }

                if (n1 > 0)
                {
                    itemFirstStr = srcText.Substring(n2, n1 - n2);

                    JItem jItem = new JItem(JItemType.String, itemFirstStr);
                    items.Add(jItem);
                }

                itemStr = srcText.Substring(n1 + 1, lastN - n1 - 1);

                int dotN = itemStr.IndexOf('.');
                string prefix = string.Empty;
                string field;

                if (dotN > 0)
                {
                    prefix = itemStr.Substring(1, dotN - 1);
                    field = itemStr.Substring(dotN + 1);
                }
                else
                {
                    field = itemStr;
                }

                JItem jItem2 = new JItem(JItemType.Code, field);
                jItem2.Prefix = prefix;
                items.Add(jItem2);

                n2 = lastN + 1;

                n1 = srcText.IndexOf("{$", n2);
            }


            if (n2 < srcText.Length)
            {
                itemStr = srcText.Substring(n2);
                JItem jItem = new JItem(JItemType.String, itemStr);
                items.Add(jItem);
            }


            return items;

        }



    }
}
