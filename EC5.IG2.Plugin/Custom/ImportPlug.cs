using System;
using System.Collections.Generic;
using System.Web;
using EasyClick.Web.Mini2;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using EasyClick.Web.Mini2.Data;
using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using System.Web.UI;
using EC5.Utility;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model.DataSet;
using EC5.IG2.BizBase;

namespace EC5.IG2.Plugin.Custom
{
    /// <summary>
    /// 导入数据(插件)
    /// </summary>
    public class ImportPlug : PagePlugin
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        


        /// <summary>
        /// 弹出模式窗体来选择的.
        /// </summary>
        /// <param name="storeUi"></param>
        /// <param name="tableUi"></param>
        /// <param name="plugParam"></param>
        public void TableDialog()
        {
            Store storeUi = this.SrcStore;
            Table tableUi = this.SrcTable;
            string srcUrl = this.SrcUrl; 
            string plugParam = this.Params;



            JObject ppm;
            string table_id;

            try
            {
                ppm = (JObject)JsonConvert.DeserializeObject(plugParam);

                table_id = ppm.Value<string>("dialog_id");
            }
            catch (Exception ex)
            {
                log.Error("解析命令参数出错",ex);
                MessageBox.Alert("解析命令参数出错.");
                return;
            }

            


            
            string urlStr = string.Format("/App/InfoGrid2/View/MoreView/DataImportDialog.aspx?id={0}", table_id);

            var plug = new
            {
                id = this.ToolbarItemId,
                plug_class = this.GetType().FullName,
                plug_method = "NextSetp",
                src_url = this.SrcUrl,
                src_store_id = storeUi.ID,
                src_table_id = tableUi.ID
                
            };

            StringBuilder plugSb = new StringBuilder();

            plugSb.Append("{");

            plugSb.AppendFormat("id: {0}", plug.id);
            plugSb.AppendFormat(", plug_class: \"{0}\"", plug.plug_class);
            plugSb.AppendFormat(", plug_method: \"{0}\"", plug.plug_method.Replace("\"", "\\\""));
            plugSb.AppendFormat(", src_url: \"{0}\"", plug.src_url);
            plugSb.AppendFormat(", src_store_id: \"{0}\"", plug.src_store_id);
            plugSb.AppendFormat(", src_table_id: \"{0}\"", plug.src_table_id);
            plugSb.AppendFormat(", ps: \"{0}\"", plugParam.Replace("\"","\\\""));

            plugSb.Append("}");

            //string plugJson = JsonConvert.ToString(plug);

            string base64PlugPs = Base64Util.ToString(plugSb.ToString(), Base64Mode.Http);
            

            Window win = new Window("数据导入", 800, 600);
            win.ContentPath = urlStr;
            win.Mode = true;
            win.State = WindowState.Max;

            win.FormClosedForJS = string.Concat("function(e){",

                "if(!e || e.result != 'ok'){ return; } ",

                "widget1.submit('form:first', {",
                    "action: 'ExecPlugin_NextStep',",
                    "actionPs: { plug: '",
                        base64PlugPs,
                        "', user_ps:e",
                    "}",
                "});",

            "}");


            win.ShowDialog();

        }



        /// <summary>
        /// 下一步步骤
        /// </summary>
        /// <param name="storeUi">数据源</param>
        /// <param name="tableUi">数据源表格</param>
        /// <param name="plugParam"></param>
        public void NextSetp()
        {           
            JObject ppm;

            string mapId ;

            string p_id_field;  //父级id字段名称

            string dialogId;

            JObject userPs = null;

            string idsStr = string.Empty;

            try
            {
                ppm = (JObject)JsonConvert.DeserializeObject(this.Params);

                dialogId = ppm.Value<string>("dialog_id");  //弹出表 ID

                mapId = ppm.Value<string>("map_id");        //映射表ID

                p_id_field = ppm.Value<string>("parent_id_field");  //主表的父字段名称

                
                if(!StringUtil.IsBlank(this.UserParams))
                {
                    userPs = (JObject)JsonConvert.DeserializeObject(this.UserParams);

                    idsStr = userPs.Value<string>("ids");


                }
            }
            catch (Exception ex)
            {
                log.Error("解析命令参数出错",ex);
                MessageBox.Alert("解析命令参数出错.");
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            //查找映射规则
            IG2_MAP map = decipher.SelectToOneModel<IG2_MAP>("ROW_SID >=0 AND IG2_MAP_ID={0} ",mapId);
            List<IG2_MAP_COL> mapCols = decipher.SelectModels<IG2_MAP_COL>("ROW_SID >= 0 AND IG2_MAP_ID={0}", mapId);

            if (map == null)
            {
                MessageBox.Alert("映射规则已经失效! MAP_ID = " + mapId);
            }

            //获取当前主表的ID
            int parentId = StringUtil.ToInt( this.MainStore.CurDataId );

            List<LModel> rModels;

            //如果映射表的 R_TABLE 是 UV_ 视图表，就特殊处理.
            if (StringUtil.StartsWith(map.R_TABLE,"UV_"))
            {
                ViewSet vSet = ViewSet.Select(decipher, StringUtil.ToInt(dialogId));

                string tSqlFrom = ConvertSQL(vSet);

                string tWhere = string.Format(" {0}.{1} in ({2})", 
                    vSet.View.MAIN_TABLE_NAME, vSet.View.MAIN_ID_FIELD,
                    idsStr);

                string tSql = tSqlFrom + " WHERE " + tWhere;

                rModels = decipher.GetModelList(tSql);
                
            }
            else
            {
                int[] ids = StringUtil.ToIntList(idsStr);

                LModelElement rModelElem = LModelDna.GetElementByName(map.R_TABLE);
                LightModelFilter filter = new LightModelFilter(map.R_TABLE);


                filter.And(rModelElem.PrimaryKey, ids, HWQ.Entity.Filter.Logic.In);

                rModels = decipher.GetModelList(filter);
            }



            List<LModel> lModels = new List<LModel>();

            foreach (LModel rModel in rModels)
            {

                LModel lModel = new LModel(map.L_TABLE);

                //映射数据
                App.InfoGrid2.Bll.MapMgr.MapData(map,mapCols, rModel, lModel);


                lModel[p_id_field] = parentId;
                lModels.Add(lModel);
            }


            try
            {
                decipher.BeginTransaction();


                foreach (LModel model in lModels)
                {

                   // decipher.InsertModel(model);

                    DbCascadeRule.Insert(model);
                    

                }


                decipher.TransactionCommit();

            }
            catch (Exception ex)
            {

                decipher.TransactionRollback();

                log.Error("插入数据失败了！", ex);

                MessageBox.Alert("插入数据失败了！");

            }

            if (this.SrcStore != null)
            {
                this.SrcStore.Refresh();
            }
        }



        private string ConvertTSqlWhere(ViewSet vSet)
        {
            StringBuilder sb = new StringBuilder();

            int n = 0;

            for (int i = 0; i < vSet.Fields.Count; i++)
            {
                IG2_VIEW_FIELD vField = vSet.Fields[i];


                if (StringUtil.IsBlank(vField.FILTER_1))
                {
                    continue;
                }


                if (n++ > 0)
                {
                    sb.Append(" AND ");
                }

                string fieldTWhere = GetFieldFilter(vField);

                sb.Append(fieldTWhere);

            }

            return sb.ToString();
        }

        private string GetFieldFilter(IG2_VIEW_FIELD vField)
        {

            StringBuilder sb = new StringBuilder();

            string tf = string.Concat(vField.TABLE_NAME, ".", vField.FIELD_NAME);

            sb.Append("(");

            sb.AppendFormat("({0} {1})", tf, vField.FILTER_1);

            if (!StringUtil.IsBlank(vField.FILTER_2))
            {
                sb.AppendFormat("OR ({0} {1})", tf, vField.FILTER_2);
            }

            if (!StringUtil.IsBlank(vField.FILTER_3))
            {
                sb.AppendFormat("OR ({0} {1})", tf, vField.FILTER_3);
            }

            sb.Append(")");

            return sb.ToString();
        }

        private string ConvertSQL(ViewSet vSet)
        {
            List<string> fields = new List<string>();

            StringBuilder sb = new StringBuilder("SELECT ");

            int n = 0;
            string field;

            IG2_VIEW view = vSet.View;

            field = view.MAIN_TABLE_NAME + "_" + view.MAIN_ID_FIELD;

            fields.Add(field);

            sb.AppendFormat("{0}.{1} AS {2}", view.MAIN_TABLE_NAME, view.MAIN_ID_FIELD, field);

            for (int i = 0; i < vSet.Fields.Count; i++)
            {
                IG2_VIEW_FIELD vField = vSet.Fields[i];

                if (!vField.FIELD_VISIBLE)
                {
                    continue;
                }

                field = vField.TABLE_NAME + "_" + vField.FIELD_NAME;

                if (fields.Contains(field))
                {
                    continue;
                }


                sb.Append(", ");



                sb.AppendFormat("{0}.{1} AS {2}", vField.TABLE_NAME, vField.FIELD_NAME, field);
            }



            sb.Append(" FROM ");

            for (int i = 0; i < vSet.Tables.Count - 1; i++)
            {
                IG2_VIEW_TABLE vTab = vSet.Tables[i];

                sb.AppendFormat("{0} FULL OUTER JOIN {1} ", vTab.TABLE_NAME, vTab.JOIN_TABLE_NAME);
                sb.AppendFormat("ON {0}.{1} = {2}.{3} ", vTab.TABLE_NAME, vTab.DB_FIELD,
                    vTab.JOIN_TABLE_NAME, vTab.JOIN_DB_FIELD);

            }


            string tSql = sb.ToString();

            return tSql;
        }




    }
}