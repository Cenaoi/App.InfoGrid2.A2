using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity.Xml;
using HWQ.Entity;
using App.InfoGrid2.Model;
using EC5.Utility;
using EC5.SystemBoard;

namespace App.InfoGrid2.Bll
{
    public static class ViewMgr
    {
        public static LModelElement GetModelElem(ViewSet vSet)
        {
            string viewName = vSet.View.VIEW_NAME;


            LModelElement modelElem = null;

            if (LModelDna.ContainsByName(viewName))
            {
                modelElem = LModelDna.GetElementByName(viewName);

                return modelElem;
            }



            LModelDna.BeginEdit();

            try
            {
                modelElem = CreateLModelElem(vSet);

                LModelDna.Add(modelElem);

                LModelDna.EndEdit();
            }
            catch (Exception ex)
            {
                LModelDna.EndEdit();
                throw new Exception("转换数据错误.", ex);
            }


            return modelElem;
        }

        public static LModelElement CreateLModelElem(ViewSet vSet)
        {

            XmlModelElem xModelElem = CreateXModelElem(vSet);

            LModelElement modelElem = ModelConvert.ToModelElem(xModelElem);

            return modelElem;
        }



        /// <summary>
        /// 根据 IG2_TABLE 表，创建实体元素
        /// </summary>
        /// <param name="tableSet"></param>
        /// <returns></returns>
        public static XmlModelElem CreateXModelElem(ViewSet vSet)
        {

            IG2_VIEW view = vSet.View;

            XmlModelElem xModelElem = new XmlModelElem(view.VIEW_NAME);
            xModelElem.Description = view.DISPLAY;
            xModelElem.Mode = DBTableMode.Virtual;


            foreach (IG2_VIEW_FIELD col in vSet.Fields)
            {
                if (col.ROW_SID == -3)
                {
                    continue;
                }

                LModelElement modelElem = LightModel.GetLModelElement(col.TABLE_NAME);



                LMFieldDBTypes dbType = modelElem.Fields[col.FIELD_NAME].DBType;

                string colName = StringUtil.IsBlank(col.ALIAS) ? col.TABLE_NAME + "_" + col.FIELD_NAME : col.ALIAS;


                XmlFieldElem fieldElem = new XmlFieldElem();
                fieldElem.DBField = colName;
                fieldElem.PropName = colName;

                fieldElem.DBType = dbType;

                fieldElem.Caption = fieldElem.Description = col.FIELD_TEXT;

                if (fieldElem.DBField == view.MAIN_TABLE_NAME + "_" + view.MAIN_ID_FIELD)
                {
                    fieldElem.IsKey = true;
                }

                xModelElem.Fields.Add(fieldElem);
            }

            return xModelElem;
        }



        /// <summary>
        /// 获取一条 T-SQL 语句
        /// </summary>
        /// <param name="vSet">视图数据集的定义类</param>
        /// <param name="startRow">开始行</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="filter2">二次过滤</param>
        /// <returns></returns>
        public static string GetTSql(ViewSet vSet, int startRow, int pageSize,string filter2)
        {
            List<string> fields = new List<string>();

            IG2_VIEW view = vSet.View;

            string tSqlSelect = GetTSqlSelect(vSet, ref fields);

            string tSqlForm = GetTSqlForm(vSet);

            string tSqlWhere = GetTSqlWhere(vSet);

            string tSqlOrder = GetTSqlOrder(vSet, fields);

            StringBuilder tSql = new StringBuilder();


            tSql.AppendLine("SELECT * FROM (");
            {
                tSql.Append("SELECT ").Append(tSqlSelect);

                if (!StringUtil.IsBlank(tSqlOrder))
                {
                    tSql.AppendFormat(",row_number() over(order by {0}) as ROW_NUMBER", tSqlOrder);
                }
                else
                {
                    tSql.Append(",row_number() as ROW_NUMBER");
                }


                tSql.AppendLine().Append(" FROM ").Append(tSqlForm);

                if (!StringUtil.IsBlank(filter2))
                {
                    if (!StringUtil.IsBlank(tSqlWhere))
                    {
                        tSqlWhere += " AND ";
                    }

                    tSqlWhere += filter2;
                }

                if (!StringUtil.IsBlank(tSqlWhere))
                {
                    tSql.AppendLine().Append(" WHERE ").Append(tSqlWhere);
                }
            }

            tSql.AppendLine().Append(") as T where ");
            tSql.AppendFormat("ROW_NUMBER between {0} and {1}", startRow + 1, startRow + pageSize);


            //if (StringUtil.IsBlank(tSqlOrder))
            //{
            //    tSql.AppendLine().Append(" ORDER BY ").Append(tSqlOrder);
            //}


            return tSql.ToString();
        }


        /// <summary>
        /// 获取 T-SQL 语句
        /// </summary>
        /// <param name="vSet"></param>
        /// <param name="filter2">二次过滤参数</param>
        /// <returns></returns>
        public static string GetTSql(ViewSet vSet, string filter2)
        {
            List<string> fields = new List<string>();

            IG2_VIEW view = vSet.View;

            string tSqlSelect = GetTSqlSelect(vSet, ref fields);

            string tSqlForm = GetTSqlForm(vSet);

            string tSqlWhere = GetTSqlWhere(vSet);

            string tSqlOrder = GetTSqlOrder(vSet, fields);

            StringBuilder tSql = new StringBuilder();



            tSql.Append("SELECT ").Append(tSqlSelect);

            tSql.AppendLine().Append(" FROM ").Append(tSqlForm);

            if (!StringUtil.IsBlank(filter2))
            {
                if (!StringUtil.IsBlank(tSqlWhere))
                {
                    tSqlWhere += " AND ";
                }

                tSqlWhere += filter2;
            }

            if (!StringUtil.IsBlank(tSqlWhere))
            {
                tSql.AppendLine().Append(" WHERE ").Append(tSqlWhere);
            }

            if (!StringUtil.IsBlank(tSqlOrder))
            {
                tSql.AppendLine().Append(" ORDER BY ").Append(tSqlOrder);
            }


            return tSql.ToString();
        }


        public static string GetCountForTSql(ViewSet vSet)
        {
            return GetCountForTSql(vSet, null);
        }

        /// <summary>
        /// 获取记录数量的 T-SQL 语句
        /// </summary>
        /// <param name="vSet"></param>
        /// <param name="filter2">二次过滤参数</param>
        /// <returns></returns>
        public static string GetCountForTSql(ViewSet vSet, string filter2)
        {
            List<string> fields = new List<string>();

            IG2_VIEW view = vSet.View;

            string tSqlSelect = "COUNT(*)";//GetTSqlSelect(vSet, ref fields);

            string tSqlForm = GetTSqlForm(vSet);

            string tSqlWhere = GetTSqlWhere(vSet);

            //string tSqlOrder = GetTSqlOrder(vSet, fields);

            StringBuilder tSql = new StringBuilder();



            tSql.Append("SELECT ").Append(tSqlSelect);

            tSql.AppendLine().Append(" FROM ").Append(tSqlForm);

            if (!StringUtil.IsBlank(filter2))
            {
                if (!StringUtil.IsBlank(tSqlWhere))
                {
                    tSqlWhere += " AND ";
                }

                tSqlWhere += filter2;
            }

            if (!StringUtil.IsBlank(tSqlWhere))
            {
                tSql.AppendLine().Append(" WHERE ").Append(tSqlWhere);
            }

            //if (!StringUtil.IsBlank(tSqlOrder))
            //{
            //    tSql.AppendLine().Append(" ORDER BY ").Append(tSqlOrder);
            //}


            return tSql.ToString();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="vSet"></param>
        /// <param name="startRow">开始行</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <returns></returns>
        public static string GetTSql(ViewSet vSet, int startRow, int pageSize)
        {
            return GetTSql(vSet, startRow, pageSize, null);
        }




        public static string GetTSql(ViewSet vSet)
        {
            List<string> fields = new List<string>();

            string tSqlSelect = GetTSqlSelect(vSet, ref fields);

            string tSqlForm = GetTSqlForm(vSet);

            string tSqlWhere = GetTSqlWhere(vSet);

            string tSqlOrder = GetTSqlOrder(vSet,fields);

            StringBuilder tSql = new StringBuilder("SELECT ");

            tSql.Append(tSqlSelect);

            tSql.AppendLine().Append(" FROM ").Append(tSqlForm);

            if (!StringUtil.IsBlank(tSqlWhere))
            {
                tSql.AppendLine().Append(" WHERE ").Append(tSqlWhere);
            }

            if (!StringUtil.IsBlank(tSqlOrder))
            {
                tSql.AppendLine().Append(" ORDER BY ").Append(tSqlOrder);
            }


            return tSql.ToString();
        }

        public static string GetTSqlWhere(ViewSet vSet)
        {
            StringBuilder sb = new StringBuilder();

            int n = 0;

            for (int i = 0; i < vSet.Fields.Count; i++)
            {
                IG2_VIEW_FIELD vField = vSet.Fields[i];


                if (StringUtil.IsBlank(vField.FILTER_1) && 
                    StringUtil.IsBlank(vField.FILTER_2) &&
                    StringUtil.IsBlank(vField.FILTER_3))
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

        /// <summary>
        /// 过滤参数替换
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static string FilterReplace(string filter)
        {
            EcContext context = EcContext.Current;
            EcUserState user = context.User;
            string userCode = user.ExpandPropertys["USER_CODE"];

            string newFilter =  filter.Replace("(GetBizUserCode())", $"'{userCode}'");

            return newFilter;
        }
        
        private static string GetFieldFilter(IG2_VIEW_FIELD vField)
        {


            StringBuilder sb = new StringBuilder();

            string tf = string.Concat(vField.TABLE_NAME, ".", vField.FIELD_NAME);

            sb.Append("(");
            
            sb.AppendFormat("{0} {1}", tf, FilterReplace(vField.FILTER_1));

            if (!StringUtil.IsBlank(vField.FILTER_2))
            {
                sb.AppendFormat("OR {0} {1}", tf, FilterReplace(vField.FILTER_2));
            }

            if (!StringUtil.IsBlank(vField.FILTER_3))
            {
                sb.AppendFormat("OR {0} {1}", tf, FilterReplace(vField.FILTER_3));
            }

            sb.Append(")");

            return sb.ToString();
        }


        public static string GetTSqlSelect(ViewSet vSet)
        {
            List<string> fields = new List<string>();
            return GetTSqlSelect(vSet, ref fields);
        }

        public static string GetTSqlSelect(ViewSet vSet,ref List<string> outFields)
        {
            List<string> fields = outFields;    // new List<string>();

            if (fields == null)
            {
                fields = new List<string>();
            }

            StringBuilder sb = new StringBuilder();


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

                if (StringUtil.IsBlank(vField.ALIAS))
                {
                    field = vField.TABLE_NAME + "_" + vField.FIELD_NAME;
                }
                else
                {
                    field = vField.ALIAS;
                }


                if (fields.Contains(field))
                {
                    continue;
                }


                fields.Add(field);
                
                sb.Append(", ");

                sb.AppendFormat("{0}.{1} AS {2}", vField.TABLE_NAME, vField.FIELD_NAME, field);
            }

            return sb.ToString();
        }


        public static string GetTSqlForm(ViewSet vSet)
        {

            IG2_VIEW view = vSet.View;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < vSet.Tables.Count - 1; i++)
            {
                IG2_VIEW_TABLE vTab = vSet.Tables[i];

                if (i == 0)
                {
                    sb.AppendFormat("{0} FULL OUTER JOIN {1} ", vTab.TABLE_NAME, vTab.JOIN_TABLE_NAME);
                }
                else if(i > 0)
                {
                    sb.AppendFormat(" FULL OUTER JOIN {0} ", vTab.JOIN_TABLE_NAME);
                }


                sb.AppendFormat("ON {0}.{1} = {2}.{3} ", vTab.TABLE_NAME, vTab.DB_FIELD,
                    vTab.JOIN_TABLE_NAME, vTab.JOIN_DB_FIELD);

            }


            string tSql = sb.ToString();

            return tSql;
        }


        public static string GetTSqlOrder(ViewSet vSet,List<string> fields)
        {
            IG2_VIEW view = vSet.View;

            List<IG2_VIEW_FIELD> orderByFields = new List<IG2_VIEW_FIELD>();
            
            IG2_VIEW_FIELD vField;

            string field;

            for (int i = 0; i < vSet.Fields.Count; i++)
            {
                vField = vSet.Fields[i];

                if (StringUtil.IsBlank(vField.SORT_TYPE) )
                {
                    continue;
                }

                orderByFields.Add(vField);
            }

            if (orderByFields.Count == 0)
            {
                if (!StringUtil.IsBlank(view.MAIN_TABLE_NAME) && !StringUtil.IsBlank(view.MAIN_ID_FIELD))
                {
                    field = view.MAIN_TABLE_NAME + "." + view.MAIN_ID_FIELD;
                }
                else
                {
                    field = string.Empty;
                }

                return field;
            }


            orderByFields.Sort(new Comparison<IG2_VIEW_FIELD>(delegate( IG2_VIEW_FIELD a,IG2_VIEW_FIELD b){
                return a.SORT_ORDER - b.SORT_ORDER;
            }));


            StringBuilder sb = new StringBuilder();


            for (int i = 0; i < orderByFields.Count; i++)
            {
                vField = orderByFields[i];

                if (i > 0) { sb.Append(", "); }


                if (StringUtil.IsBlank(vField.ALIAS))
                {
                    field = vField.TABLE_NAME + "_" + vField.FIELD_NAME;
                }
                else
                {
                    field = vField.ALIAS;
                }

                if (!fields.Contains(field))
                {
                    field = vField.TABLE_NAME + "." + vField.FIELD_NAME;
                }


                sb.Append(field);

                if (vField.SORT_TYPE == "DESC")
                {
                    sb.Append(" DESC");
                }
                else
                {
                    sb.Append(" ASC");
                }
            }


            return sb.ToString();
        }




    }
}
