using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model.Report;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Mall
{
    /// <summary>
    /// 这是 服装商城的库存报表
    /// </summary>
    public partial class StockReport : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }

        protected override void OnInitCustomControls(EventArgs e)
        {


            this.store1.PageLoading += Store1_PageLoading;

            this.store2.PageLoading += Store2_PageLoading;

            this.store3.PageLoading += Store3_PageLoading;

            this.store4.PageLoading += Store4_PageLoading;



        }

        private void Store4_PageLoading(object sender, CancelPageEventArags e)
        {
            e.Cancel = true;


            string tableName = this.hTableName4.Value;

            if (string.IsNullOrEmpty(tableName))
            {
                log.Error($"缓冲表 '{tableName}'不存在 . ");
                return;
            }

            BufferTableHelper.PageLoading(store4, tableName, e.Page, e.TSqlSort);

        }

        private void Store3_PageLoading(object sender, CancelPageEventArags e)
        {
            e.Cancel = true;


            string tableName = this.hTableName3.Value;

            if (string.IsNullOrEmpty(tableName))
            {
                log.Error($"缓冲表 '{tableName}'不存在 . ");
                return;
            }

            BufferTableHelper.PageLoading(store3, tableName, e.Page, e.TSqlSort);
        }

        private void Store2_PageLoading(object sender, CancelPageEventArags e)
        {
            e.Cancel = true;


            string tableName = this.hTableName2.Value;

            if (string.IsNullOrEmpty(tableName))
            {
                log.Error($"缓冲表 '{tableName}'不存在 . ");
                return;
            }

            BufferTableHelper.PageLoading(store2, tableName, e.Page, e.TSqlSort);

        }

        private void Store1_PageLoading(object sender, CancelPageEventArags e)
        {
            e.Cancel = true;


            string tableName = this.hTableName1.Value;

            if (string.IsNullOrEmpty(tableName))
            {
                log.Error($"缓冲表 '{tableName}'不存在 . ");
                return;
            }

            BufferTableHelper.PageLoading(store1, tableName, e.Page, e.TSqlSort);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                InitData();

            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {

            InsertStore1();

            InsertStore2();

            InsertStore3();

            InsertStore4();

            ProductSale();

            GetBX();


        }


        /// <summary>
        /// 插入长T恤库存数据
        /// </summary>
        void InsertStore1()
        {

            Dictionary<string, ColType> cols = new Dictionary<string, ColType>();

            DbDecipher decipher = ModelAction.OpenDecipher();

            //昨天
            string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            string begTime = yesterday + " 00:00:00";

            string endTime = yesterday + " 23:23:59";
            

            #region  这是创建有多少个列

            //这是获取进仓数据
            string sql121 = $"select SUM(COL_20) as COL_20,COL_4,COL_3 from UT_117 where ROW_SID >= 0 and COL_4 is not null and COL_3 is not null and COL_37 = '121' and COL_61 >= '{begTime}' and COL_61 <= '{endTime}' group by COL_25,COL_4,COL_3";

            SModelList sms121 = decipher.GetSModelList(sql121);

            //这是获取出仓数据
            string sql103 = $"select SUM(COL_20) as COL_20,COL_4,COL_3 from UT_117 where ROW_SID >= 0 and COL_4 is not null and COL_3 is not null and COL_37 = '103' and COL_61 >= '{begTime}' and COL_61 <= '{endTime}' group by COL_25,COL_4,COL_3";

            SModelList sms103 = decipher.GetSModelList(sql103);

            //这是获取退货数据
            string sql105 = $"select SUM(COL_20) as COL_20,COL_4,COL_3 from UT_117 where ROW_SID >= 0 and COL_4 is not null and COL_3 is not null and COL_37 = '105' and COL_61 >= '{begTime}' and COL_61 <= '{endTime}' group by COL_25,COL_4,COL_3";

            SModelList sms105 = decipher.GetSModelList(sql105);


            //这是获取所有库存的
            string sqlTotal = $"select SUM(COL_20) as COL_20,COL_4,COL_3 from UT_117 where ROW_SID >= 0 and COL_4 is not null and COL_3 is not null and COL_37 in ('105','103','121') and COL_61 >= '{begTime}' and COL_61 <= '{endTime}' group by COL_25,COL_4,COL_3";

            SModelList smsTotal = decipher.GetSModelList(sqlTotal);


            int i = 0;

            CreateDataBaseFields(sms121, cols,ref i);
            CreateDataBaseFields(sms103, cols, ref i);
            CreateDataBaseFields(sms105, cols, ref i);
            CreateDataBaseFields(smsTotal, cols, ref i);

            #endregion

            DataTable dt = CreateDataTable(cols);

            SetDataTable(dt, "进仓", sms121, cols);

            SetDataTable(dt, "出仓", sms103, cols);

            SetDataTable(dt, "退货", sms105, cols);

            SetDataTable(dt, "库存", smsTotal, cols);

            decipher = DbDecipherManager.GetDecipherOpen("ReportBuffer");

            try
            {
                LModelElement modelElem = BufferTableHelper.CreateReportBuffer(decipher, dt);

                this.hTableName1.Value = modelElem.DBTableName;

                BufferTableHelper.FillReportBuffer(decipher, dt, modelElem.DBTableName);


                this.store1.DataBind();

            }
            catch (Exception ex)
            {
                log.Error("创建收入统计数据临时表出错了！", ex);
                throw ex;
            }
            finally
            {
                decipher.Dispose();
            }


            Dictionary<string, ViewTableColumnTree> view_fields = CreateViewColumn(cols);

            BufferTableHelper.UpdateTableCellByFields(table1, view_fields);

            BufferTableHelper.CreateViewTableCell(table1);

           
        }

        /// <summary>
        /// 插入秋冬装库存数据
        /// </summary>
        void InsertStore2()
        {
            Dictionary<string, ColType> cols = new Dictionary<string, ColType>();

            DbDecipher decipher = ModelAction.OpenDecipher();

            #region  这是创建有多少个列

            //这是获取进仓数据
            string sql121 = "select SUM(COL_20) as COL_20,COL_60 from UT_117 where ROW_SID >= 0 and COL_60 is not null  and COL_37 = '121' group by COL_60";

            SModelList sms121 = decipher.GetSModelList(sql121);

            //这是获取出仓数据
            string sql103 = "select SUM(COL_20) as COL_20,COL_60 from UT_117 where ROW_SID >= 0 and COL_60 is not null  and COL_37 = '103' group by COL_60";

            SModelList sms103 = decipher.GetSModelList(sql103);

            //这是获取退货数据
            string sql105 = "select SUM(COL_20) as COL_20,COL_60 from UT_117 where ROW_SID >= 0 and COL_60 is not null  and COL_37 = '105' group by COL_60";

            SModelList sms105 = decipher.GetSModelList(sql105);

            //这是获取所有库存的
            string sqlTotal = "select SUM(COL_20) as COL_20,COL_60 from UT_117 where ROW_SID >= 0 and COL_60 is not null and COL_37 in ('105','103','121') group by COL_60";

            SModelList smsTotal = decipher.GetSModelList(sqlTotal);


            int i = 0;

            CreateDataBaseFields(sms121, cols, "COL_60",ref i);
            CreateDataBaseFields(sms103, cols, "COL_60", ref i);
            CreateDataBaseFields(sms105, cols, "COL_60", ref i);
            CreateDataBaseFields(smsTotal, cols, "COL_60", ref i);

            #endregion

            DataTable dt = CreateDataTable(cols);

            SetDataTable(dt, "进仓", sms121, cols, "COL_60");

            SetDataTable(dt, "出仓", sms103, cols, "COL_60");

            SetDataTable(dt, "退货", sms105, cols, "COL_60");


            SetDataTable(dt, "库存", smsTotal, cols, "COL_60");

            decipher = DbDecipherManager.GetDecipherOpen("ReportBuffer");

            try
            {
                LModelElement modelElem = BufferTableHelper.CreateReportBuffer(decipher, dt);

                this.hTableName2.Value = modelElem.DBTableName;

                BufferTableHelper.FillReportBuffer(decipher, dt, modelElem.DBTableName);


                this.store2.DataBind();

            }
            catch (Exception ex)
            {
                log.Error("创建收入统计数据临时表出错了！", ex);
                throw ex;
            }
            finally
            {
                decipher.Dispose();
            }


            Dictionary<string, ViewTableColumnTree> view_fields = CreateViewColumn(cols);

            BufferTableHelper.UpdateTableCellByFields(table2, view_fields);

            BufferTableHelper.CreateViewTableCell(table2);



        }

        void InsertStore3()
        {
            Dictionary<string, ColType> cols = new Dictionary<string, ColType>();

            DbDecipher decipher = ModelAction.OpenDecipher();

            #region  这是创建有多少个列

            //这是获取进仓数据
            string sql121 = "select SUM(COL_20) as COL_20,COL_58 from UT_117 where ROW_SID >= 0 and COL_58 is not null  and COL_37 = '121' group by COL_58";

            SModelList sms121 = decipher.GetSModelList(sql121);

            //这是获取出仓数据
            string sql103 = "select SUM(COL_20) as COL_20,COL_58 from UT_117 where ROW_SID >= 0 and COL_58 is not null  and COL_37 = '103' group by COL_58";

            SModelList sms103 = decipher.GetSModelList(sql103);

            //这是获取退货数据
            string sql105 = "select SUM(COL_20) as COL_20,COL_58 from UT_117 where ROW_SID >= 0 and COL_58 is not null  and COL_37 = '105' group by COL_58";

            SModelList sms105 = decipher.GetSModelList(sql105);


            //这是获取所有库存的
            string sqlTotal = "select SUM(COL_20) as COL_20,COL_58 from UT_117 where ROW_SID >= 0 and COL_58 is not null and COL_37 in ('105','103','121') group by COL_58";

            SModelList smsTotal = decipher.GetSModelList(sqlTotal);

            int i = 0;

            CreateDataBaseFields(sms121, cols, "COL_58", ref i);
            CreateDataBaseFields(sms103, cols, "COL_58", ref i);
            CreateDataBaseFields(sms105, cols, "COL_58", ref i);

            CreateDataBaseFields(smsTotal, cols, "COL_58", ref i);


            #endregion

            DataTable dt = CreateDataTable(cols);

            SetDataTable(dt, "进仓", sms121, cols, "COL_58");

            SetDataTable(dt, "出仓", sms103, cols, "COL_58");

            SetDataTable(dt, "退货", sms105, cols, "COL_58");

            SetDataTable(dt, "库存", smsTotal, cols, "COL_58");


            decipher = DbDecipherManager.GetDecipherOpen("ReportBuffer");

            try
            {
                LModelElement modelElem = BufferTableHelper.CreateReportBuffer(decipher, dt);

                this.hTableName3.Value = modelElem.DBTableName;

                BufferTableHelper.FillReportBuffer(decipher, dt, modelElem.DBTableName);


                this.store3.DataBind();

            }
            catch (Exception ex)
            {
                log.Error("创建收入统计数据临时表出错了！", ex);
                throw ex;
            }
            finally
            {
                decipher.Dispose();
            }


            Dictionary<string, ViewTableColumnTree> view_fields = CreateViewColumn(cols);

            BufferTableHelper.UpdateTableCellByFields(table3, view_fields);

            BufferTableHelper.CreateViewTableCell(table3);

        }


        void InsertStore4()
        {
            Dictionary<string, ColType> cols = new Dictionary<string, ColType>();

            DbDecipher decipher = ModelAction.OpenDecipher();

            #region  这是创建有多少个列

            //这是获取进仓数据
            string sql121 = "select SUM(COL_20) as COL_20,COL_59 from UT_117 where ROW_SID >= 0 and COL_59 is not null  and COL_37 = '121' group by COL_59";

            SModelList sms121 = decipher.GetSModelList(sql121);

            //这是获取出仓数据
            string sql103 = "select SUM(COL_20) as COL_20,COL_59 from UT_117 where ROW_SID >= 0 and COL_59 is not null  and COL_37 = '103' group by COL_59";

            SModelList sms103 = decipher.GetSModelList(sql103);

            //这是获取退货数据
            string sql105 = "select SUM(COL_20) as COL_20,COL_59 from UT_117 where ROW_SID >= 0 and COL_59 is not null  and COL_37 = '105' group by COL_59";

            SModelList sms105 = decipher.GetSModelList(sql105);

            //这是获取所有库存的
            string sqlTotal = "select SUM(COL_20) as COL_20,COL_59 from UT_117 where ROW_SID >= 0 and COL_59 is not null and COL_37 in ('105','103','121') group by COL_59";

            SModelList smsTotal = decipher.GetSModelList(sqlTotal);

            int i = 0;

            CreateDataBaseFields(sms121, cols, "COL_59", ref i);
            CreateDataBaseFields(sms103, cols, "COL_59", ref i);
            CreateDataBaseFields(sms105, cols, "COL_59", ref i);
            CreateDataBaseFields(smsTotal, cols, "COL_59", ref i);

            #endregion

            DataTable dt = CreateDataTable(cols);

            SetDataTable(dt, "进仓", sms121, cols, "COL_59");

            SetDataTable(dt, "出仓", sms103, cols, "COL_59");

            SetDataTable(dt, "退货", sms105, cols, "COL_59");

            SetDataTable(dt, "库存", smsTotal, cols, "COL_59");


            decipher = DbDecipherManager.GetDecipherOpen("ReportBuffer");

            try
            {
                LModelElement modelElem = BufferTableHelper.CreateReportBuffer(decipher, dt);

                this.hTableName4.Value = modelElem.DBTableName;

                BufferTableHelper.FillReportBuffer(decipher, dt, modelElem.DBTableName);


                this.store4.DataBind();

            }
            catch (Exception ex)
            {
                log.Error("创建收入统计数据临时表出错了！", ex);
                throw ex;
            }
            finally
            {
                decipher.Dispose();
            }


            Dictionary<string, ViewTableColumnTree> view_fields = CreateViewColumn(cols);

            BufferTableHelper.UpdateTableCellByFields(table4, view_fields);

            BufferTableHelper.CreateViewTableCell(table4);

        }


        decimal ToDecimal(object o)
        {
            try
            {
                return Convert.ToDecimal(o);
            }
            catch (Exception ex)
            {
                log.Error("转换成数字格式出错了！", ex);
            }

            return 0;

        }

        class ColType
        {
            public string Field { get; set; }

            public string Text { get; set; }

            public LMFieldDBTypes DbType { get; set; }
        }



        /// <summary>
        /// 创建内存数据表
        /// </summary>
        /// <returns></returns>
        DataTable CreateDataTable(Dictionary<string, ColType> cols)
        {


            DataTable dt = new DataTable("NEW_TABLE");

           // dt.Columns.Add("REP_ROW_IDENTITY", typeof(int));

            dt.Columns.Add("ROW_DATE_CREATE", typeof(DateTime));

            //库存类型  进仓、出仓、退货、库存
            dt.Columns.Add("STOCK_TYPE", typeof(string));

                
            foreach (var item in cols)
            {

                ColType ct = item.Value;

                dt.Columns.Add(ct.Field, HWQ.Entity.ModelConvert.ToType(ct.DbType));

            }

            dt.Columns.Add("TOTAL", typeof(decimal));

            dt.Columns.Add("REP_ROW_TYPE");

            return dt;


        }

        /// <summary>
        /// 根据传进来的字段集合来创建界面上的table列集合
        /// </summary>
        /// <param name="cols">字段集合</param>
        /// <returns></returns>
        Dictionary<string, ViewTableColumnTree> CreateViewColumn(Dictionary<string, ColType> cols)
        {
            Dictionary<string, ViewTableColumnTree> view_fileds = new Dictionary<string, ViewTableColumnTree>();

            AddViewFieldsByText(view_fileds, "类型", "STOCK_TYPE");

            foreach (var item in cols)
            {

                ColType ct = item.Value;


                AddViewFieldsByText(view_fileds, ct.Text, ct.Field);

            }


            AddViewFieldsByNum(view_fileds, "合计", "TOTAL");


            return view_fileds;
        }


        /// <summary>
        /// 添加界面的字段，字符串类型
        /// </summary>
        /// <param name="view_fileds">界面用的字段集合</param>
        /// <param name="field_text">字段说明</param>
        /// <param name="field">字段</param>
        void AddViewFieldsByText(Dictionary<string, ViewTableColumnTree> view_fileds, string field_text, string field)
        {
            ViewTableColumnTree col_tree = new ViewTableColumnTree(field_text, new ViewTableColumnType(field, field_text, LMFieldDBTypes.String));

            view_fileds.Add(field_text, col_tree);
        }

        /// <summary>
        /// 添加界面的字段，数值类型
        /// </summary>
        ///<param name="view_fileds">界面用的字段集合</param>
        /// <param name="field_text">字段说明</param>
        /// <param name="field">字段</param>
        /// <param name="is_sum">是否显示合计，默认为 false </param>
        void AddViewFieldsByNum(Dictionary<string, ViewTableColumnTree> view_fileds,string field_text, string field, bool is_sum = false)
        {

            ViewTableColumnType ct = new ViewTableColumnType(field, field_text, LMFieldDBTypes.Decimal)
            {
                Is_Total = is_sum,
                Format = "0.00"
            };

            ViewTableColumnTree col_tree = new ViewTableColumnTree(field_text, ct);

            view_fileds.Add(field_text, col_tree);

        }

        /// <summary>
        /// 给内存数据表赋值
        /// </summary>
        /// <param name="dt">内存数据表</param>
        /// <param name="stockType">类别</param>
        /// <param name="sms">数据集合</param>
        /// <param name="cols">字段集合</param>
        void SetDataTable(DataTable dt,string stockType,SModelList sms, Dictionary<string, ColType> cols)
        {

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            //dr["REP_ROW_IDENTITY"] = 3;
            dr["STOCK_TYPE"] = stockType;
            dr["ROW_DATE_CREATE"] = DateTime.Now;

            decimal total = 0;

            foreach (SModel sm in sms)
            {
                string key = sm.Get<string>("COL_3") + "-" + sm.Get<string>("COL_4");

                decimal col_20 = sm.Get<decimal>("COL_20");

                total += col_20;


                if (cols.ContainsKey(key))
                {
                    ColType ct = cols[key];

                    dr[ct.Field] = ToDecimal(dr[ct.Field]) + col_20;
                }
            }

            dr["TOTAL"] = total;


        }

        /// <summary>
        /// 给内存数据表赋值
        /// </summary>
        /// <param name="dt">内存数据表</param>
        /// <param name="stockType">类别</param>
        /// <param name="sms">数据集合</param>
        /// <param name="cols">字段集合</param>
        void SetDataTable(DataTable dt, string stockType, SModelList sms, Dictionary<string, ColType> cols,string field)
        {

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            //dr["REP_ROW_IDENTITY"] = 3;
            dr["STOCK_TYPE"] = stockType;
            dr["ROW_DATE_CREATE"] = DateTime.Now;

            decimal total = 0;

            foreach (SModel sm in sms)
            {
                string key = sm.Get<string>(field);

                decimal col_20 = sm.Get<decimal>("COL_20");

                total += col_20;


                if (cols.ContainsKey(key))
                {
                    ColType ct = cols[key];

                    dr[ct.Field] = ToDecimal(dr[ct.Field]) + col_20;
                }
            }

            dr["TOTAL"] = total;


        }


        /// <summary>
        /// 这是创建数据库里面的字段
        /// </summary>
        /// <param name="sms">数据集合</param>
        /// <param name="cols">字段集合</param>
        /// <param name="i">字段索引</param>
        void CreateDataBaseFields(SModelList sms, Dictionary<string, ColType> cols,ref int i)
        {

            foreach (var item in sms)
            {
                string col_3 = item.Get<string>("COL_3");

                string col_4 = item.Get<string>("COL_4");

                string key = col_3 + "-" + col_4;

                if (cols.ContainsKey(key))
                {
                    continue;
                }

                i++;

                cols.Add(key, new ColType()
                {
                    DbType = LMFieldDBTypes.Decimal,
                    Field = "COL_" + i,
                    Text = key
                });
            }

        }


        /// <summary>
        /// 这是创建数据库里面的字段
        /// </summary>
        /// <param name="sms">数据集合</param>
        /// <param name="cols">字段集合</param>
        /// <param name="i">字段索引</param>
        void CreateDataBaseFields(SModelList sms, Dictionary<string, ColType> cols,string filed, ref int i)
        {

            foreach (var item in sms)
            {
                string key = item.Get<string>(filed);

                if (cols.ContainsKey(key))
                {
                    continue;
                }

                i++;

                cols.Add(key, new ColType()
                {
                    DbType = LMFieldDBTypes.Decimal,
                    Field = "COL_" + i,
                    Text = key
                });
            }

        }


        public string smlisttitle = "";
        public string smlistji = "";

        /// <summary>
        /// 客户销售额分析
        /// </summary>
        public void GetBX()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select sum(COL_22) as NEW_ROW,COL_38 from UT_181 where  dateadd(month, -12, getdate())<COL_2 and  COl_2<getdate() and COL_1='收' GROUP by COL_38";

            LModelList<LModel> modellist = decipher.GetModelList(sql);

            SModelList smList = new SModelList();

            foreach (LModel model in modellist)
            {
                SModel sm = new SModel();

                sm["value"] = model["NEW_ROW"];

                sm["name"] = model["COL_38"];

                if (string.IsNullOrWhiteSpace(sm["name"]))
                {
                    continue;
                }
                smList.Add(sm);

                if (smlisttitle.Length != 0)
                {

                    smlisttitle += ",";
                }
                smlisttitle += "'" + sm["name"] + "'";

            }
            smlistji = smList.ToJson();


        }


        public string yingsk = "";
        public string yingsklist = "";


        public void ProductSale()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            string sql = "select sum(COL_22) as NWE_ROW,COL_33,COL_35 from UT_181 where dateadd(month, -12, getdate())<COL_2 and  COl_2<getdate() and COL_1='收' GROUP by  COL_33,COL_35";

            LModelList<LModel> modellist = decipher.GetModelList(sql);

            SModelList smList = new SModelList();


            StringBuilder sb = new StringBuilder();
            foreach (LModel model in modellist)
            {

                SModel sm = new SModel();

                sm["value"] = model["NWE_ROW"];

                sm["name"] = model["COL_33"] + "-" + model["COL_35"];

                if (string.IsNullOrWhiteSpace(sm["name"]))
                {
                    continue;
                }
                smList.Add(sm);


                if (yingsk.Length != 0)
                {
                    yingsk += ",";
                }
                yingsk += "'" + sm["name"] + "'";

            }

            yingsklist = smList.ToJson();

        }




        /// <summary>
        /// 显示彬哥弄好的交叉报表
        /// </summary>
        public void showReport1()
        {

            EcView.Show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id=2056", "昨日产品进出统计表");


        }

        /// <summary>
        /// 显示彬哥弄好的交叉报表
        /// </summary>
        public void showReport2()
        {

            EcView.Show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id=2059", "昨日产品小类统计表");

        }

        /// <summary>
        /// 显示彬哥弄好的交叉报表
        /// </summary>
        public void showReport3()
        {

            EcView.Show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id=2057", "昨日产品品牌统计表");

        }



        /// <summary>
        /// 显示彬哥弄好的交叉报表
        /// </summary>
        public void showReport4()
        {

            EcView.Show("/App/InfoGrid2/View/ReportBuilder/ReportPreviewV2.aspx?id=2058", "昨日产品大类统计表");

        }



    }
}