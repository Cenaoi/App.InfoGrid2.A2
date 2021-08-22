
using System;
using System.Collections.Generic;
using System.Text;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model;
using HWQ.Entity.Filter;
using HWQ.Entity;
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity.Xml;
using System.Web;
using System.IO;

namespace App.InfoGrid2.Bll
{

    /// <summary>
    /// IG2 的结构验证
    /// </summary>
    public static class SysValidateMgr
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 

        private static SortedList<string, IG2_TABLE_COL> ToGroupByField(string tableName, List<IG2_TABLE_COL> cols)
        {
            SortedList<string, IG2_TABLE_COL> dict = new SortedList<string, IG2_TABLE_COL>();

            foreach (IG2_TABLE_COL tCol in cols)
            {
                if (dict.ContainsKey(tCol.DB_FIELD))
                {
                    throw new Exception(string.Format("  严重错误，表 “{0}”的字段名“{1}”重复。 ", tableName, tCol.DB_FIELD));
                }

                dict.Add(tCol.DB_FIELD, tCol);
            }

            return dict;
        }

        private static void TableStruceValid(DbDecipher decipher,StringBuilder sb, int id)
        {
            DatabaseBuilder db = decipher.DatabaseBuilder;

            TableSet ts = TableSet.Select(decipher, id);

            string tableName = ts.Table.TABLE_NAME;

            if (!db.ExistTable(tableName))
            {
                sb.AppendLine("数据表已经不存在: " + tableName).AppendLine();

                return;
            }

            sb.AppendFormat("验证表 “{0}”", tableName).AppendLine();


            SortedList<string, IG2_TABLE_COL> tCols = null;

            try
            {
                tCols = ToGroupByField(tableName, ts.Cols);
            }
            catch (Exception ex)
            {
                log.Error(ex);

                sb.AppendFormat(ex.Message).AppendLine().AppendLine();

                return;
            }

            XmlModelElem xModelElem = db.GetModelElemByTable(tableName);

            foreach (IG2_TABLE_COL tCol in ts.Cols)
            {
                if (!xModelElem.Fields.Contains(tCol.DB_FIELD))
                {
                    sb.AppendFormat("  缺少“{0}”", tCol.DB_FIELD).AppendLine();
                }

                
            }

            foreach (IG2_TABLE_COL tCol in ts.Cols)
            {
                if (!xModelElem.Fields.Contains(tCol.DB_FIELD))
                {
                    continue;
                }

                XmlFieldElem xFieldElem = xModelElem.Fields[tCol.DB_FIELD];

                if (xFieldElem.DBType == LMFieldDBTypes.String &&
                    tCol.DB_LEN > 0 &&
                    tCol.DB_LEN != xFieldElem.MaxLen)
                {
                    sb.AppendFormat("  “{0}”长度异常,定义:{1}, 实际:{2}", tCol.DB_FIELD, tCol.DB_LEN, xFieldElem.MaxLen).AppendLine();
                }

                if (xFieldElem.DBField == "BIZ_SID" && xFieldElem.DBType != LMFieldDBTypes.Int)
                {
                    sb.Append("  BIZ_SID 字段必须是 int 整形。").AppendLine();
                }

                if (xFieldElem.DBField == "ROW_SID" && xFieldElem.DBType != LMFieldDBTypes.Int)
                {
                    sb.Append("  BIZ_SID 字段必须是 int 整形。").AppendLine();
                }

            }


            foreach (XmlFieldElem xFieldElem in xModelElem.Fields)
            {
                if (!tCols.ContainsKey(xFieldElem.DBField))
                {
                    sb.AppendFormat("  多余“{0}”", xFieldElem.DBField).AppendLine();
                }
            }

            sb.AppendLine();
        }

        /// <summary>
        /// IG2_TABLE 数据表结构验证
        /// </summary>
        public static string TableStruceValids()
        {
            HttpContext context = HttpContext.Current;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("验证时间 " + DateTime.Now.ToString()).AppendLine();

            DbDecipher decipher = ModelAction.OpenDecipher();

            DatabaseBuilder db = decipher.DatabaseBuilder;

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("TABLE_TYPE_ID", "TABLE");
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.TSqlOrderBy = "TABLE_NAME";
            filter.Fields = new string[] { "IG2_TABLE_ID" };


            LModelReader reader = decipher.GetModelReader(filter);

            int[] ids = ModelHelper.GetColumnData<int>(reader);


            foreach (int id in ids)
            {
                try
                {
                    TableStruceValid(decipher, sb, id);
                }
                catch (Exception ex)
                {
                    log.Error("处理表“" + id + "”有大错误。", ex);
                }

            }

            string filename = "IG2字段验证报告_" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".txt";

            string aDir = "/_Temporary/IG2_数据表字段验证报告/";
            string dir = context.Request.MapPath(aDir);

            System.IO.Directory.CreateDirectory(dir);

            File.WriteAllText(dir + filename, sb.ToString());

            return (aDir + filename);
        }


        /// <summary>
        /// 验证关联表的数据
        /// </summary>
        /// <returns></returns>
        public static string ViewStruceValids()
        {
            HttpContext context = HttpContext.Current;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("验证时间 " + DateTime.Now.ToString()).AppendLine();

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_VIEW));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[]{"IG2_VIEW_ID"};

            LModelReader reader = decipher.GetModelReader(filter);

            int[] ids = ModelHelper.GetColumnData<int>(reader);
            
            foreach (int id in ids)
            {
                try
                {
                    ViewStruceValid(decipher, sb, id);
                }
                catch (Exception ex)
                {
                    log.Error("处理表“" + id + "”有大错误。", ex);
                }
            }


            string filename = "IG2 视图字段验证报告_" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".txt";

            string aDir = "/_Temporary/IG2_视图字段验证报告/";
            string dir = context.Request.MapPath(aDir);

            System.IO.Directory.CreateDirectory(dir);

            File.WriteAllText(dir + filename, sb.ToString());

            return (aDir + filename);

        }





        /// <summary>
        /// 获取关联表映射出去的表，IG2_TABLE
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static TableSet GetMapTableCols(DbDecipher decipher, string tableName)
        {
            LightModelFilter tFilter = new LightModelFilter(typeof(IG2_TABLE));
            tFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            tFilter.And("TABLE_TYPE_ID", "MORE_VIEW");
            tFilter.And("TABLE_NAME", tableName);

            IG2_TABLE table = decipher.SelectToOneModel<IG2_TABLE>(tFilter);


            LightModelFilter cFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
            cFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            cFilter.And("IG2_TABLE_ID", table.IG2_TABLE_ID);

            LModelList<IG2_TABLE_COL> cols = decipher.SelectModels<IG2_TABLE_COL>(cFilter);

            TableSet tSet = new TableSet();
            tSet.Table = table;
            tSet.Cols = cols;

            return tSet;

        }


        private static void ViewStruceValid(DbDecipher decipher,StringBuilder sb, int id)
        {

            SortedList<string, IG2_TABLE_COL> tCols = null;
            SortedDictionary<string, IG2_VIEW_FIELD> viewFields = null;

            ViewSet viewSet = ViewSet.Select(decipher, id);


            TableSet mapTableSet = GetMapTableCols(decipher, viewSet.View.VIEW_NAME);  //映射的字段

           

            sb.AppendLine();
            sb.AppendFormat("关联表“{0}”[{1}]。", viewSet.View.VIEW_NAME, viewSet.View.DISPLAY).AppendLine();




            foreach (var item in viewSet.Tables)
            {


                TableSet tSet = TableSet.Select(decipher, item.TABLE_NAME);

                if (tSet.Table == null)
                {
                    sb.AppendFormat("    关联的表“{0}”已经不存在。", item.TABLE_NAME).AppendLine();
                    return;
                }




                try
                {
                    tCols = ToGroupByField(item.TABLE_NAME, tSet.Cols);
                }
                catch (Exception ex)
                {
                    log.Error(ex);

                    sb.AppendFormat(ex.Message).AppendLine().AppendLine();

                    return;
                }


                //开始对碰

                viewFields = ToGroup(item.TABLE_NAME, viewSet.Fields);

                foreach (var vField in viewFields.Values)
                {
                    if (!tCols.ContainsKey(vField.FIELD_NAME))
                    {
                        sb.AppendFormat("    物理字段“{0}.{1}”[{2}] 已经不存在,可删除。", vField.TABLE_NAME, vField.FIELD_NAME, vField.FIELD_TEXT).AppendLine();
                    }
                }




            }
        }


        private static SortedDictionary<string, IG2_VIEW_FIELD> ToGroup(string tableName, List<IG2_VIEW_FIELD> viewFields)
        {
            SortedDictionary<string, IG2_VIEW_FIELD> groups = new SortedDictionary<string, IG2_VIEW_FIELD>();

            foreach (var item in viewFields)
            {
                if(item.TABLE_NAME != tableName)
                {
                    continue;
                }
                


                if (!groups.ContainsKey(item.FIELD_NAME))
                {
                    groups.Add(item.FIELD_NAME, item);
                }
            }

            return groups;

        }


    }
}
