using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.InfoGrid2.Model.DataSet;
using EC5.Utility.Web;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Bll;
using EC5.Utility;
using System.Text;
using Newtonsoft.Json.Linq;
using EasyClick.Web.Mini2;
using App.InfoGrid2.Model;

namespace App.InfoGrid2.View.MoreView
{
    public partial class OneMapAction : Page, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {


            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CreateForLoad();
        }




        ViewSet m_ViewSet;




        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        private string GetJsonValue(JObject obj, string attr, string defualtValue)
        {
            string value = obj.Value<string>(attr);

            return StringUtil.NoBlank(value, defualtValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        private string GetJsonValue(JObject obj, string attr)
        {
            string value = obj.Value<string>(attr);


            return value;
        }


        /// <summary>
        /// 获取 T-SQL 的 Where 子语句
        /// </summary>
        /// <param name="filter2"></param>
        /// <returns></returns>
        private string GetTSqlWhere_ForFilter2(string filter2)
        {
            if (StringUtil.IsBlank(filter2))
            {
                return string.Empty;
            }

            JArray items = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(filter2);

            StringBuilder sb = new StringBuilder();

            int i = 0;

            foreach (JObject item in items)
            {
                //string field = item.Value<string>("field");
                //string logic = item.Value<string>("logic");
                //string value = item.Value<string>("value");

                //if (i++ > 0) { sb.Append(" AND "); }

                //sb.AppendFormat("({0} {1} '{2}')", field, logic, value);


                string p_type = GetJsonValue(item, "p_type", "DEFAULT");    //两种模式. Default | TSQL_WHERE 
                string field = GetJsonValue(item, "field");
                string logic = GetJsonValue(item, "logic", "=");
                string value = GetJsonValue(item, "value");

                if (i++ > 0) { sb.Append(" AND "); }

                p_type = p_type.ToUpper();

                if (p_type == "TSQL_WHERE")
                {
                    sb.Append("(").Append(value).Append(")");
                }
                else
                {
                    if (StringUtil.IsBlank(field))
                    {
                        throw new Exception("二次筛选的参数字段名不能为空。");
                    }

                    sb.AppendFormat("({0} {1} '{2}')", field, logic, value);
                }
            }


            return sb.ToString();
        }


        private void CreateForLoad()
        {
            string filter2 = WebUtil.QueryBase64("filter2");  //二次过滤参数

            filter2 =  GetTSqlWhere_ForFilter2(filter2);

            int viewId = WebUtil.QueryInt("viewId");

            string id = WebUtil.Query("id");
            string one_field = WebUtil.Query("one_field");

            if (!ValidateUtil.SqlInput(id) ||
                !ValidateUtil.SqlInput(one_field))
            {
                return;
            }

            #region 二次筛选的叠加
            //字段等于空就不处理
            if (!string.IsNullOrEmpty(one_field))
            {
                string pkFilter = string.Format("({0}='{1}')", one_field, id);

                if (!StringUtil.IsBlank(filter2))
                {
                    filter2 = " AND " + filter2;
                }

                filter2 = pkFilter + filter2;
            }
            #endregion


            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.SelectSID_0_5(decipher, viewId);

            IG2_TABLE table = tSet.Table;

            

            string tSql;

            if (table.TABLE_NAME.StartsWith("UV_"))
            {
                m_ViewSet = ViewSet.Select(decipher, viewId);

                tSql = ViewMgr.GetTSql(m_ViewSet, 0, 10, filter2);
            }
            else
            {
                tSql = TableMgr.GetTSql(tSet, 0, 10, filter2);
            }

            string json = string.Empty;
            List<LModel> models = null;
            try
            {

                models = decipher.GetModelList(tSql);

            }
            catch (Exception ex) 
            {
                log.Error("查询出错了！",ex);
            }

            if(models != null && models.Count == 1)
            {
                json = string.Format("{{result:'ok',message:'查询成功！',count:1,data:");
                json += HWQ.Entity.ModelHelper.ToJson(models[0]);
                json += "}";

            }
            else if(models != null && models.Count > 1)
            {
                json = string.Format("{{result:'ok',message:'查询成功！',count:{0},data:null}}",models.Count);
            }
            else if(models == null || models.Count == 0)
            {
                json = string.Format("{{result:'ok',message:'没找到数据！',count:0,data:null}}");
            }

            Response.Clear();
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            
            Response.Write(json);
            

        }


    }
}