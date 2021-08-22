using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using HWQ.Entity.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace App.InfoGrid2.Bll
{
    /// <summary>
    /// 工作表管理
    /// </summary>
    public static class TmpTableMgr 
    {
        /// <summary>
        /// 临时数据转换为正式数据
        /// </summary>
        public static int TempTable2Table(Guid tempGuid)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            TmpTableSet tTableSet = TmpTableSet.Select(decipher, tempGuid);

            IG2_TMP_TABLE tmpTable = tTableSet.Table;

            if (tmpTable == null)
            {
                throw new Exception("创建失败: 可能数据表已经创建,或临时数据丢失.");
            }



            List<IG2_TMP_TABLECOL> tmpCols = tTableSet.Cols;

            //获取最终产生的表名
            tmpTable.TABLE_NAME = BizCommon.BillIdentityMgr.NewCodeForNum("USER_TABLE", "UT_", 3);

            if (!StringUtil.IsBlank(tmpTable.REMARK))
            {
                string exTable = tmpTable.REMARK.Trim().ToUpper();

                exTable = exTable.Replace(" ", "_").Replace("__", "_");

                if (!StringUtil.StartsWith(exTable, "_"))
                {
                    exTable = "_" + exTable;
                }

                tmpTable.TABLE_NAME += exTable;

                tmpTable.REMARK = string.Empty;
            }


            IG2_TABLE table = new IG2_TABLE();
            tmpTable.CopyTo(table, true);

            LModelList<IG2_TABLE_COL> cols = new LModelList<IG2_TABLE_COL>();


            decipher.BeginTransaction();

            try
            {
                
                decipher.InsertModel(table);

                CreateDBField(tmpCols, tmpCols, cols, table.IG2_TABLE_ID);


                Link(table, "IG2_TABLE_ID", (IList)cols, "IG2_TABLE_ID");
                Link(table, "TABLE_UID", (IList)cols, "TABLE_UID");

                decipher.InsertModels<IG2_TABLE_COL>(cols);

                CreateDBTableForTmpTable(tmpTable, tmpCols);

                TmpTableSet.Delete(decipher, tempGuid);

                decipher.TransactionCommit();
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                throw new Exception("创建工作表失败", ex);
            }

            return table.IG2_TABLE_ID;

        }


        /// <summary>
        /// 临时数据转换为正式数据  专门给CMS用的
        /// 小渔夫
        /// 创建于 2016-12-23
        /// </summary>
        public static IG2_TABLE TempTable2TableCMS(Guid tempGuid)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            TmpTableSet tTableSet = TmpTableSet.Select(decipher, tempGuid);

            IG2_TMP_TABLE tmpTable = tTableSet.Table;

            if (tmpTable == null)
            {
                throw new Exception("创建失败: 可能数据表已经创建,或临时数据丢失.");
            }



            List<IG2_TMP_TABLECOL> tmpCols = tTableSet.Cols;

            //如果为空才用自动生成表名
            if (string.IsNullOrWhiteSpace(tmpTable.TABLE_NAME))
            {

                //获取最终产生的表名
                tmpTable.TABLE_NAME = BizCommon.BillIdentityMgr.NewCodeForNum("CMS_USER_TABLE", "CMS_UT_", 3);

                if (!StringUtil.IsBlank(tmpTable.REMARK))
                {
                    string exTable = tmpTable.REMARK.Trim().ToUpper();

                    exTable = exTable.Replace(" ", "_").Replace("__", "_");

                    if (!StringUtil.StartsWith(exTable, "_"))
                    {
                        exTable = "_" + exTable;
                    }

                    tmpTable.TABLE_NAME += exTable;

                    tmpTable.REMARK = string.Empty;
                }
            }

            IG2_TABLE table = new IG2_TABLE();
            tmpTable.CopyTo(table, true);

            LModelList<IG2_TABLE_COL> cols = new LModelList<IG2_TABLE_COL>();


            decipher.BeginTransaction();

            try
            {

                decipher.InsertModel(table);

                CreateDBField(tmpCols, tmpCols, cols, table.IG2_TABLE_ID);


                Link(table, "IG2_TABLE_ID", (IList)cols, "IG2_TABLE_ID");
                Link(table, "TABLE_UID", (IList)cols, "TABLE_UID");

                decipher.InsertModels<IG2_TABLE_COL>(cols);

                CreateDBTableForTmpTable(tmpTable, tmpCols);

                TmpTableSet.Delete(decipher, tempGuid);

                decipher.TransactionCommit();
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                throw new Exception("创建工作表失败", ex);
            }

            return table;

        }


        /// <summary>
        /// 填充获取变动的字段
        /// </summary>
        /// <param name="tTableSet"></param>
        /// <param name="del_Cols"></param>
        /// <param name="edit_Cols"></param>
        /// <param name="new_Cols"></param>
        private static void FullChangeFields(TmpTableSet tTableSet, 
            List<IG2_TMP_TABLECOL> del_Cols,
            List<IG2_TMP_TABLECOL> edit_Cols,
            List<IG2_TMP_TABLECOL> new_Cols)
        {

            foreach (IG2_TMP_TABLECOL tmpCol in tTableSet.Cols)
            {
                if (tmpCol.ROW_SID == -3 && tmpCol.TMP_OP_ID == "D")
                {
                    //如果还没有字段名，就是临时字段删除。还没有具体字段
                    if (!StringUtil.IsBlank(tmpCol.DB_FIELD))
                    {
                        del_Cols.Add(tmpCol);
                    }
                }
                else if (tmpCol.TMP_OP_ID == "E")
                {
                    if (tmpCol.IG2_TABLE_COL_ID > 0)
                    {
                        edit_Cols.Add(tmpCol);
                    }
                    else
                    {
                        new_Cols.Add(tmpCol);
                    }
                }
                else
                {
                    if (tmpCol.IG2_TABLE_COL_ID <= 0 )
                    {
                        new_Cols.Add(tmpCol);
                    }
                }
            }
        }


        /// <summary>
        /// 联动删除 IG2_VIEW 表的字段.
        /// </summary>
        /// <param name="del_Cols"></param>
        private static void CascadeDeleteViewFields(DbDecipher decipher,  string tableName, List<IG2_TMP_TABLECOL> del_Cols)
        {
            //删除视图表的字段.
            foreach (var item in del_Cols)
            {
                LightModelFilter filter = new LightModelFilter(typeof(IG2_VIEW_FIELD));

                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter.And("TABLE_NAME", tableName);
                filter.And("FIELD_NAME", item.DB_FIELD);

                decipher.UpdateProps(filter, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });
            }

            //删除映射表的字段
            foreach (var item in del_Cols)
            {

                LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE_COL));

                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter.And("VIEW_FIELD_SRC", tableName + "." + item.DB_FIELD);

                decipher.UpdateProps(filter, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });
            }
            
        }


        /// <summary>
        /// 保存已经发生变化的字段
        /// </summary>
        private static void SaveChangeFields(DbDecipher decipher, IG2_TABLE table,
            List<IG2_TMP_TABLECOL> del_Cols,
            List<IG2_TMP_TABLECOL> edit_Cols,
            List<IG2_TMP_TABLECOL> new_Cols)
        {

            foreach (IG2_TMP_TABLECOL tmpCol in del_Cols)
            {
                LightModelFilter delFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
                delFilter.And("TABLE_UID", table.TABLE_UID);
                delFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                delFilter.And("DB_FIELD", tmpCol.DB_FIELD);

                decipher.UpdateProps(delFilter, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });
            }



            foreach (IG2_TMP_TABLECOL tmpCol in edit_Cols)
            {
                LightModelFilter editFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
                editFilter.And("TABLE_UID", table.TABLE_UID);
                editFilter.And("DB_FIELD", tmpCol.DB_FIELD);
                editFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

                int dbDot = 0;
                int dbLen = 0;

                string dbType = tmpCol.DB_TYPE.ToLower();

                switch (dbType)
                {
                    case "currency":
                        dbDot = 2;
                        dbLen = 18;
                        break;
                    case "decimal":
                        dbDot = 6;
                        dbLen = 18;
                        break;
                    case "string":
                        dbDot = 0;
                        dbLen = tmpCol.DB_LEN;
                        break;
                    case "boolean":
                        dbDot = 0;
                        dbLen = 0;
                        break;
                }

                decipher.UpdateProps(editFilter, new object[] {
                        "F_NAME",tmpCol.F_NAME,
                        "IS_MANDATORY", tmpCol.IS_MANDATORY,
                        "IS_REMARK", tmpCol.IS_REMARK,
                        "IS_VIEW_FIELD", tmpCol.IS_VIEW_FIELD,
                        "DB_TYPE", tmpCol.DB_TYPE,
                        "DB_DOT", dbDot,
                        "VIEW_SQL", tmpCol.VIEW_SQL
                    });

            }

            if (new_Cols.Count > 0)
            {
                LightModelFilter newTableFilter = new LightModelFilter(typeof(IG2_TABLE));
                newTableFilter.And("TABLE_UID", table.TABLE_UID);
                newTableFilter.Fields = new string[] { "IG2_TABLE_ID", "TABLE_UID" };

                LModelList<LModel> tables = decipher.GetModelList(newTableFilter);


                foreach (LModel tab in tables)
                {
                    Guid tUid = tab.Get<Guid>("TABLE_UID");
                    int tabId = tab.Get<int>("IG2_TABLE_ID");

                    foreach (IG2_TMP_TABLECOL tmpCol in new_Cols)
                    {
                        IG2_TABLE_COL tCol = new IG2_TABLE_COL();
                        tmpCol.CopyTo(tCol, true);
                        tCol.IG2_TABLE_ID = tabId;
                        tCol.TABLE_UID = tUid;
                        tCol.TABLE_NAME = table.TABLE_NAME;
                        

                        decipher.InsertModel(tCol);
                    }
                }
            }

        }




        /// <summary>
        /// 临时数据转换为正式数据
        /// </summary>
        public static int TempTable2Table_Edit(Guid tempGuid)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();
            DatabaseBuilder db = decipher.DatabaseBuilder;


            TmpTableSet tTableSet = TmpTableSet.Select(decipher, tempGuid);

            IG2_TMP_TABLE tmpTable = tTableSet.Table;
            List<IG2_TMP_TABLECOL> tmpCols = tTableSet.Cols;


            List<IG2_TMP_TABLECOL> del_Cols = new List<IG2_TMP_TABLECOL>();
            List<IG2_TMP_TABLECOL> edit_Cols = new List<IG2_TMP_TABLECOL>();
            List<IG2_TMP_TABLECOL> new_Cols = new List<IG2_TMP_TABLECOL>();

            FullChangeFields(tTableSet, del_Cols, edit_Cols, new_Cols);



            IG2_TABLE table = new IG2_TABLE();
            tmpTable.CopyTo(table, true);



            LModelList<IG2_TABLE_COL> cols = new LModelList<IG2_TABLE_COL>();

            decipher.UpdateModel(table);

            //CreateDBField(tmpCols, cols, table.IG2_TABLE_ID);
            CreateDBField(tmpCols, new_Cols, cols, table.IG2_TABLE_ID);

            foreach (var col in new_Cols)
            {
                col.IS_READONLY = true;
                col.IS_VISIBLE = false;
                col.CAN_APPLY_VIEW = false;
            }

            decipher.BeginTransaction();

            try
            {
                
                //准备创建临时表名
                string srcTableName = tmpTable.TABLE_NAME;

                string tmpTableName = srcTableName + "__Temp";

                tmpTable.TABLE_NAME = tmpTableName;

                //创建临时表
                try
                {
                    CreateDBTableForTmpTable(tmpTable, tmpCols);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("创建临时表“{0}”失败。", tmpTable.TABLE_NAME),ex);
                }

                //拷贝数据
                try
                {
                    CopyData(srcTableName, tmpTableName);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("拷贝数据到临时表失败。“{0}” -> “{1}”", srcTableName, tmpTableName), ex);
                }




                Link(table, "IG2_TABLE_ID", (IList)new_Cols, "IG2_TABLE_ID");
                Link(table, "TABLE_UID", (IList)new_Cols, "TABLE_UID");
                Link(table, "TABLE_NAME", (IList)new_Cols, "TABLE_NAME");

                SaveChangeFields(decipher, table,
                    del_Cols,
                    edit_Cols,
                    new_Cols);

                CascadeDeleteViewFields(decipher,  table.TABLE_NAME, del_Cols);

                //删除原表，并把临时表改名为原表
                db.DropTable(srcTableName);
                db.RenameTable(tmpTableName, srcTableName);

                //删除原实体元素
                LModelElement srcModelElem = LModelDna.GetElementByName(srcTableName);
                LModelDna.BeginEdit();
                LModelDna.Remove(srcModelElem);
                LModelDna.EndEdit();

                //删除临时数据
                TmpTableSet.Delete(decipher,tempGuid);

                decipher.TransactionCommit();
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                throw new Exception("创建工作表失败：" + ex.Message, ex);
            }

            return table.IG2_TABLE_ID;

        }


        /// <summary>
        /// 拷贝数据
        /// </summary>
        /// <param name="srcTable"></param>
        /// <param name="targetTable"></param>
        public static void CopyData(string srcTable, string targetTable)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            DatabaseBuilder db = decipher.DatabaseBuilder;

            //XmlModelElem srcXModelElem = db.GetModelElemByTable(srcTable);
            XmlModelElem tagXModelElem = db.GetModelElemByTable(targetTable);

            //LModelElement srcLModelElem = ModelConvert.ToModelElem(srcXModelElem);
            LModelDna.BeginEdit();
            
            tagXModelElem.DbIdentity = string.Empty;
            tagXModelElem.DbIdentityTable = string.Empty;

            LModelElement tagLModelElem = ModelConvert.ToModelElem(tagXModelElem);
            LModelDna.EndEdit();

            LightModelFilter filter = new LightModelFilter(srcTable);
            filter.Locks.Add(LockType.NoLock);

            IList models = decipher.SelectModels(filter);

            LModel srcModel;


            int rowIndex = 0;

            foreach (var model in models)
            {
                srcModel = model as LModel;

                if (srcModel == null)
                {
                    continue;
                }

                LModel tagModel = new LModel(tagLModelElem);

                try
                {
                    ModelCopy(srcModel, tagModel);

                    //srcModel.CopyTo(tagModel, true);
                }
                catch (Exception ex)
                {
                    throw new Exception("数据拷贝失败", ex);
                }

                try
                {
                    decipher.InsertModel(tagModel);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("第 {0} 行插入错误", rowIndex), ex);
                }

                rowIndex++;
            }

        }


        static DateTime m_1753 = new DateTime(1753, 1, 1,12,0,0);

        private static void ModelCopy_ConvertValue(LModel srcModel, LModel tarModel,object value, LModelFieldElement field, LModelFieldElement tarField)
        {
            if (value == null || DBNull.Value.Equals(value) || string.Empty.Equals(value))
            {
                if (tarField.IsNumber)
                {
                    if (tarField.Mandatory)
                        value = 0;
                    else
                        value = null;
                }
                else if(tarField.DBType == LMFieldDBTypes.Boolean)
                {
                    if (tarField.Mandatory)
                    {
                        value = 0;
                    }
                    else
                    {
                        value = null;
                    }
                }
                else if (tarField.DBType == LMFieldDBTypes.DateTime)
                {
                    if (tarField.Mandatory)
                        value = m_1753; //DateTime.MinValue;
                    else
                        value = null;
                }
                else
                {
                    if (tarField.Mandatory)
                        value = string.Empty;
                    else
                        value = null;
                }

            }
            else
            {
                Type valueType = ModelConvert.ToType(tarField.DBType);

                value = Convert.ChangeType(value, valueType);
            }

            LightModel.SetFieldValue(tarModel, tarField, value);

        }

        private static void ModelCopy(LModel srcModel, LModel tarModel)
        {
            LModelElement srcModelElem = srcModel.GetModelElement();
            LModelElement tarModelElem = tarModel.GetModelElement();

            object tarValue;

            LModelFieldElement tarField;
            object value;

            foreach (LModelFieldElement field in srcModelElem.Fields)
            {
                if (!tarModelElem.TryGetField(field.DBField, out tarField))
                {
                    continue;
                }

                value = LightModel.GetFieldValue(srcModel, field);

                try
                {
                    ModelCopy_ConvertValue(srcModel, tarModel,value, field, tarField);
                }
                catch (Exception ex)
                {
                    throw new Exception($"转换数据类型错误“{field.DBType}”->“{tarField.DBType}”，值“{value}”" , ex);
                }
            }
        }


        private static string NewDbField(int tableId, List<IG2_TMP_TABLECOL> srcCols,IG2_TMP_TABLECOL tmpCol)
        {

            string field = string.Empty;

            for (int i = 0; i < 999; i++)
            {
                field = "COL_" + GetNewColID(tableId);

                bool isExist = IsRepeat(srcCols, tmpCol, field);

                if (!isExist)
                {
                    break;
                }
            }

            return field;
        }

        /// <summary>
        /// 创建字段名，和描述
        /// </summary>
        /// <param name="srcCols">原临时字段，用于匹配重复字段名的问题</param>
        /// <param name="cols"></param>
        /// <param name="tableId"></param>
        public static void CreateDBField(List<IG2_TMP_TABLECOL> srcCols, List<IG2_TMP_TABLECOL> tmpCols, List<IG2_TABLE_COL> cols, int tableId)
        {

            foreach (IG2_TMP_TABLECOL tmpCol in tmpCols)
            {
                if (tmpCol.ROW_SID == -3)
                {
                    continue;
                }

                IG2_TABLE_COL col = new IG2_TABLE_COL();

                if (StringUtil.IsBlank(tmpCol.DB_FIELD))
                {
                    tmpCol.DB_FIELD = NewDbField(tableId, srcCols, tmpCol);

                    if (tmpCol.IS_VIEW_FIELD)
                    {
                        tmpCol.DB_FIELD = "V_" + tmpCol.DB_FIELD;
                    }
                }
                else
                {
                    bool isExist = IsRepeat( tmpCols, tmpCol,tmpCol.DB_FIELD);

                    if (isExist)
                    {
                        throw new Exception("字段名重复:" + tmpCol.DB_FIELD);
                    }

                }

                if (StringUtil.IsBlank(tmpCol.DISPLAY))
                {
                    tmpCol.DISPLAY = tmpCol.F_NAME;
                }

                tmpCol.CopyTo(col, true);               


                cols.Add(col);
            }

        }

        /// <summary>
        /// 判断字段名是否重复
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="newCol"></param>
        /// <returns></returns>
        private static bool IsRepeat(List<IG2_TMP_TABLECOL> cols,IG2_TMP_TABLECOL tmpCol, string field)
        {
            bool isExist = false;

            foreach (var item in cols)
            {
                if (item == tmpCol)
                {
                    continue;
                }

                if (field == item.DB_FIELD)
                {
                    isExist = true;
                    break;
                }
            }

            return isExist;
        }

        /// <summary>
        /// 链接两个表
        /// </summary>
        /// <param name="headModel"></param>
        /// <param name="headField"></param>
        /// <param name="itemModels"></param>
        /// <param name="linkField"></param>
        public static void Link(LightModel headModel, string headField, IList itemModels,string linkField)
        {
            object headId = LightModel.GetFieldValue(headModel, headField);

            foreach (object item in itemModels)
            {
                LightModel.SetFieldValue(item, linkField, headId);
            }
        }

        /// <summary>
        /// 获取字段顺序号
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int GetNewColID(int tableId)
        {

            string sqlUpdate = string.Format("UPDATE IG2_TABLE SET COL_IDENTITY =  COL_IDENTITY + 1 WHERE IG2_TABLE_ID={0} and TABLE_TYPE_ID='TABLE'", tableId);

            string sqlSelect = string.Format("SELECT COL_IDENTITY FROM IG2_TABLE WHERE IG2_TABLE_ID={0} and TABLE_TYPE_ID='TABLE'", tableId);

            int newId = -1;

            DbDecipher decipher = ModelAction.OpenDecipher();


            try
            {
                //LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
                //filter.And("IG2_TABLE_ID", tableId);
                //filter.Locks.Add(LockType.RowLock);
                //filter.Fields = new string[] { "COL_IDENTITY"};

                //decipher.GetModel(filter);
                
                decipher.ExecuteNonQuery(sqlUpdate);

                newId = decipher.ExecuteScalar<int>(sqlSelect);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return newId;
        }

        /// <summary>
        /// 根据临时记录，创建工作表
        /// </summary>
        /// <param name="guid"></param>
        public static void CreateDBTableForTmpTable(IG2_TMP_TABLE tmpTable, List<IG2_TMP_TABLECOL> tmpCols)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            DatabaseBuilder db = decipher.DatabaseBuilder;

            LModelDna.BeginEdit();

            XmlModelElem xModelElem = GetXmlModel_ForTempTable(tmpTable, tmpCols);

            db.CreateTable(xModelElem);

            LModelDna.EndEdit();
        }


        /// <summary>
        /// 根据临时表
        /// </summary>
        /// <param name="tmpTable"></param>
        /// <param name="tmpCols"></param>
        public static XmlModelElem GetXmlModel_ForTempTable(IG2_TMP_TABLE tmpTable, List<IG2_TMP_TABLECOL> tmpCols)
        {
            XmlModelElem xModelElem = new XmlModelElem(tmpTable.TABLE_NAME);
            xModelElem.DbIdentity = tmpTable.IDENTITY_FIELD;
            xModelElem.Description = tmpTable.DISPLAY;

            foreach (IG2_TMP_TABLECOL tmpCol in tmpCols)
            {
                if (tmpCol.ROW_SID == -3)
                {
                    continue;
                }

                if (StringUtil.IsBlank(tmpCol.DB_FIELD))
                {
                    throw new Exception("出现空字段名, 无法创建临时表.");
                }


                tmpCol.DB_FIELD = tmpCol.DB_FIELD.Trim();

                XmlFieldElem xFieldElem = new XmlFieldElem();
                xFieldElem.DBField = tmpCol.DB_FIELD;
                xFieldElem.Description = xFieldElem.Caption = tmpCol.DISPLAY;
                xFieldElem.Mandatory = tmpCol.IS_MANDATORY;
                xFieldElem.DBType = ModelConvert.ToDbType(tmpCol.DB_TYPE);

                if (xFieldElem.DBType == LMFieldDBTypes.Currency)
                {
                    xFieldElem.MaxLen = 18;
                    xFieldElem.DecimalDigits = tmpCol.DB_DOT;
                }
                else if (xFieldElem.DBType == LMFieldDBTypes.Decimal)
                {
                    xFieldElem.MaxLen = 18;
                    xFieldElem.DecimalDigits = tmpCol.DB_DOT;
                }
                else
                {
                    xFieldElem.MaxLen = tmpCol.DB_LEN;
                    xFieldElem.DecimalDigits = tmpCol.DB_DOT;
                }

                xFieldElem.IsRemark = tmpCol.IS_REMARK;


                if (xFieldElem.DBField == tmpTable.ID_FIELD)
                {
                    xFieldElem.IsKey = true;
                }

                xFieldElem.Type = tmpCol.IS_VIEW_FIELD ? LModelFieldTypes.ViewField : LModelFieldTypes.General;

                xModelElem.Fields.Add(xFieldElem);
            }
            
            return xModelElem;
        }
    }
}
