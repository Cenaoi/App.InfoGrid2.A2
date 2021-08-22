using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using App.InfoGrid2.Bll;
using HWQ.Entity.LightModels;
using App.BizCommon;
using App.InfoGrid2.Model.DataSet;
using System.Text;
using App.InfoGrid2.Model;
using EC5.Utility.Web;
using EasyClick.Web.Mini2.Data;
using EasyClick.Web.Mini2;
using EasyClick.BizWeb2;
using HWQ.Entity;
using HWQ.Entity.Filter;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.Util;
using App.InfoGrid2.Excel_Template;

using EC5.BizCoder;
using EC5.IG2.Core.UI;
using EC5.IG2.BizBase;

namespace App.InfoGrid2.View.Biz.JLSLBZ.XSSCZL
{
    public partial class FormXssczl : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);

            //M2SecurityUiFactory secUiFactory = new M2SecurityUiFactory();
            //secUiFactory.InitSecUI(0, "FIXED_PAGE", WebUtil.QueryInt("menuId"));

            //secUiFactory.Filter("", null, "mainClientTable", this.mainClientTable, this.StoreUT102);
            //secUiFactory.Filter("", null, "Table103", this.Table103, this.StoreUT103);
            //secUiFactory.Filter("", null, "Table105", this.Table105, this.StoreUT105);
            //secUiFactory.Filter("", null, "Table106", this.Table106, this.StoreUT106);
            //secUiFactory.Filter("", null, "Table104", this.Table104, this.StoreUT104);

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            

            this.StoreUT103.Inserting += new ObjectCancelEventHandler(StoreUT103_Inserting);
            this.StoreUT103.Updating += StoreUT103_Updating;

            this.StoreUT102.SavedAll += StoreUT102_SavedAll;
            this.StoreUT102.CurrentChanged += StoreUT102_CurrentChanged;
            this.StoreUT102.Inserting += StoreUT102_Inserting;

            this.StoreUT104.Updating += new ObjectCancelEventHandler(StoreUT104_Updating);

            LCodeFactory lCodeFactory = new LCodeFactory();
            lCodeFactory.BindStore(this.StoreUT103);
            lCodeFactory.BindStore(this.StoreUT104);
            lCodeFactory.BindStore(this.StoreUT102);

            LCodeValueFactory lcvFactiry = new LCodeValueFactory();
            lcvFactiry.BindStore(this.StoreUT103);
            lcvFactiry.BindStore(this.StoreUT104);
            lcvFactiry.BindStore(this.StoreUT102);

            if (!this.IsPostBack)
            {
                this.headLab.Value = "<span class='page-head' >吸塑生产指令单</span>";

                this.DataBind();
                
                InitData();
            }
        }

        void StoreUT102_CurrentChanged(object sender, ObjectEventArgs e)
        {
            LModel lm = (LModel)e.Object;
            if (lm == null) { return; }

            ///产品ID
            string col_52 = lm.Get<string>("COL_42");
            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilter = new LightModelFilter("UT_118");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_15", col_52);

            List<LModel> lmList118 = decipher.GetModelList(lmFilter);

            StringBuilder sb = new StringBuilder();
            ///库存合计 
            decimal sum = 0;


            sb.Append("合计：{0}    ");

            foreach (var item in lmList118)
            {
                sum += item.Get<decimal>("COL_9");

                sb.Append(item.Get<string>("COL_11") + ":" + item.Get<string>("COL_9") + "     ");

            }


            string message = string.Format(sb.ToString(), sum);
            this.tbMessage.Text = message;
        }

        void StoreUT102_SavedAll(object sender, ObjectListEventArgs e)
        {
            this.StoreUT102.RefreshSummary();
        }

        void StoreUT102_Inserting(object sender, ObjectCancelEventArgs e)
        {
            LModel lm = (LModel)e.Object;


            int id = WebUtil.QueryInt("id");

            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_IDENTITY_ID", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[] { "BIZ_SID" }; ///拿到表头的状态

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModel(filter);

            object sid = model["BIZ_SID"];

            lm["BIZ_SID"] = sid;
            lm["COL_13"] = id;

        }

        void StoreUT103_Updating(object sender, ObjectCancelEventArgs e)
        {
            LModel model = e.Object as LModel;

            if (model == null)
            {
                return;
            }


            int id = WebUtil.QueryInt("id");

            LightModelFilter filter = new LightModelFilter("UT_102");
            filter.And("COL_13", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[] { "COL_9", "COL_8" };

            //COL_9 = 生产数量， COL_8 = 排摸个数

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<LModel> models = decipher.GetModelList(filter);

            decimal totalCol8 = LModelMath.Sum(models, "COL_8");   //排摸个数之和
            decimal totalCol9 = LModelMath.Sum(models, "COL_9");   //生产数量之和


            if (!model.IsNull("COL_6"))
            {
                decimal COL_6 = (decimal)model["COL_6"];

                model["COL_19"] = COL_6 * 1000 + 70;

                this.StoreUT103.SetRecordValue(e.SrcRecord.Id, "COL_19", COL_6 * 1000 + 70);
            }

            if(!model.IsNull("COL_7") && !model.IsNull("COL_11"))
            {
                decimal COL_8 = model.Get<decimal>("COL_7") + model.Get<decimal>("COL_11");
                model["COL_8"] = COL_8;
                this.StoreUT103.SetRecordValue(e.SrcRecord.Id, "COL_8", COL_8);

            }

            if (!model.IsNull("COL_4") && !model.IsNull("COL_5") && !model.IsNull("COL_17") && !model.IsNull("COL_19") && !model.IsNull("COL_8") && !model.IsNull("COL_9"))
            {
                decimal col_4 = model.Get<decimal>("COL_4");
                decimal COL_5 = model.Get<decimal>("COL_5");
                decimal COL_17 = model.Get<decimal>("COL_17");
                decimal COL_19 = model.Get<decimal>("COL_19");
                decimal COL_8 = model.Get<decimal>("COL_8");
                string COL_9 = model.Get<string>("COL_9");

                if (COL_9 == "KG" || COL_9 == "公斤")
                {
                    //      （生产数量之和 +  损耗张数    * 排摸个数之和 ）
                    decimal a = (totalCol9 + model.Get<decimal>("COL_11") * totalCol8);

                    ///    实际用量 =  （50000 / 宽度  /  ( 厚度 / 100 ) / 密度   / (长度 / 1000) ）                                                     
                    decimal COL_12 = (50000m / col_4 / (COL_17 / 100m) / COL_5 / (COL_19 / 1000m));
                    //              取整后         *  排摸个数之和 * 0.97
                    decimal x = Math.Floor(COL_12) * totalCol8   *   0.97m;
                    //    （生产数量之和 +  损耗张数  * 排摸个数之和 ） /    然后再取整  * 50
                    decimal y = a / Math.Floor(x) * 50m;

                    model["COL_12"] = Math.Floor(y);
                    this.StoreUT103.SetRecordValue(e.SrcRecord.Id, "COL_12", Math.Floor(y));
                }

                if (COL_9 == "米")
                {
                    ///实际用量 = 长度   X 实需张数 /1000 
                    decimal COL_12 = COL_19 * COL_8 / 1000;
                    decimal x = Math.Floor(COL_12);
                    model["COL_12"] = x;
                    this.StoreUT103.SetRecordValue(e.SrcRecord.Id, "COL_12", x);
                }

                if (COL_9 == "张")
                {
                    ///实际用量 = 实需张数
                    decimal COL_12 = COL_8;
                    decimal x = Math.Floor(COL_12);
                    model["COL_12"] = x;
                    this.StoreUT103.SetRecordValue(e.SrcRecord.Id, "COL_12", x);
                }
            }

            if(!model.IsNull("COL_17") && !model.IsNull("COL_5") && !model.IsNull("COL_19") && !model.IsNull("COL_4"))
            {

               

                //        50KG生产个数 /             / 宽度                          (厚度 /100)                     /  密度                       / (长度/1000)                  
                decimal COL_18 = 50000 /  model.Get<decimal>("COL_4") / (model.Get<decimal>("COL_17") /100m) / model.Get<decimal>("COL_5") / (model.Get<decimal>("COL_19")/1000m)   ;
                //  取整后   *  排摸个数之和
                decimal x = Math.Floor(COL_18) * totalCol8;
                //然后再取整   *  0.97
                decimal y = Math.Floor(x) * 0.97m;

                model["COL_18"] = Math.Floor(y);

                this.StoreUT103.SetRecordValue(e.SrcRecord.Id, "COL_18",Math.Floor(y));
            }


            





        }

        /// <summary>
        /// 工艺工序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StoreUT104_Updating(object sender, ObjectCancelEventArgs e)
        {
            LModel model = e.Object as LModel;

            if (model == null)
            {
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            string znjs = (string)model["COL_43"];          //判断这个字段值是否等于 "智能运算"

            if (znjs != "智能运算")
            {
                return;
            }

            //单头ID
            int mainId = model.Get<int>("COL_12");
            LightModelFilter filter103 = new LightModelFilter("UT_103");
            filter103.And("COL_15",mainId);
            filter103.And("ROW_SID",0, Logic.GreaterThanOrEqual);
            //主材料
            LModel lm103 = decipher.GetModel(filter103);


            int id = WebUtil.QueryInt("id");

            LightModelFilter filter = new LightModelFilter("UT_102");
            filter.And("COL_13", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[] { "COL_9", "COL_8" };

            //COL_9 = 生产数量， COL_8 = 排摸个数

            

            LModelList<LModel> models = decipher.GetModelList(filter);

            decimal totalCol9 = LModelMath.Sum(models, "COL_9");
            decimal totalCol8 = LModelMath.Sum(models, "COL_8");

            string unit = model.Get<string>("COL_4");   //计量单位名称

            unit = unit.Trim();

            if (unit == "个")
            {
                model["COL_7"] = totalCol9;    //生产数量之和
            }
            else if (unit == "张")
            {
                decimal num = totalCol9 / totalCol8;     //生产数量之和   / 排摸个数之和

                model["COL_7"] = Math.Round(num);

            }else if(unit == "米")
            {
                if(lm103 != null)
                {
                    //  计划数量 =  长度 * 实需张数 / 1000
                    model["COL_7"] = lm103.Get<decimal>("COL_19") * lm103.Get<decimal>("COL_8") / 1000m;  

                }
                
            }

            if (!model.IsNull("COL_8"))
            {
                model["COL_9"] = (decimal)model["COL_7"] + (decimal)model["COL_8"];

                
            }
            Store uiStore = (Store)sender;

            uiStore.SetRecordValue(e.SrcRecord.Id, "COL_9", model["COL_9"]);
            uiStore.SetRecordValue(e.SrcRecord.Id, "COL_7", model["COL_7"]);
            
            

        }



        void StoreUT103_Inserting(object sender, ObjectCancelEventArgs e)
        {
            LModel model = e.Object as LModel;

            if (model == null)
            {
                return;
            }

            int id = WebUtil.QueryInt("id");

            LightModelFilter filter = new LightModelFilter("UT_102");
            filter.And("COL_13", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[] { "COL_9", "COL_8" ,"COL_7"};

            //COL_9 = 生产数量， COL_8 = 排摸个数 ，COL_7 = 订单数量
            
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<LModel> models = decipher.GetModelList(filter);

            decimal totalCol9 = LModelMath.Sum( models,"COL_9");
            decimal totalCol8 = LModelMath.Sum(models, "COL_8");
            decimal totalCol7 = LModelMath.Sum(models, "COL_7");
            model["COL_64"] = totalCol8;
            model["COL_65"] = totalCol9;

            if (totalCol8 != 0) 
            {
                decimal col_7 = totalCol9 / totalCol8;
                model["COL_7"] = Math.Floor(col_7);
            }

            model["COL_11"] = 0; 

            model["COL_67"] = 0;

            model["COL_33"] = ".";
            model["COL_30"] = ".";
            model["COL_47"] = "是";
            model["COL_62"] = ".";

        }




        //<mi:DatePicker runat="server" ID="TextBox12" FieldLabel="时间" />
        //<mi:TextBox  runat="server" ID="TextBox1" FieldLabel="生产单号"  />
        //<mi:TextBox  runat="server" ID="TextBox2" FieldLabel="单据类型"  />
        //<mi:TextBox  runat="server" ID="TextBox3" FieldLabel="生产模式"  />

        private void InitData()
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_IDENTITY_ID", id);

            LModel model = decipher.GetModel(filter);


            if (model.Get<decimal>("BIZ_SID") > 0)
            {
                this.Table103.ReadOnly = true;
                this.Table104.ReadOnly = true;
                this.Table106.ReadOnly = true;
                this.mainClientTable.ReadOnly = true;
                this.Table105.ReadOnly = true;
                
                this.Toolbar1.Visible = false;
                this.Toolbar2.Visible = false;
                this.Toolbar3.Visible = false;
                this.Toolbar5.Visible = false;
                this.Toolbar4.Visible = false;
            }


            TextBox12.Value = IfUtil.NotBlank(model["COL_4"], "");  //时间
            TextBox1.Value = IfUtil.NotBlank(model["COL_1"], "");   //生产单号
            this.comboBox2.Value = IfUtil.NotBlank(model["COL_2"], "正常生产单");   //单据类型
            this.comboBox1.Value = IfUtil.NotBlank(model["COL_3"], "自制");   //生产模式
            this.comboBox3.Value = IfUtil.NotBlank(model["COL_10"], "");   //是否生产
            this.comboBox4.Value = IfUtil.NotBlank(model["COL_12"], "单客户");
            remarkTb.Value = IfUtil.NotBlank(model["COL_6"], "");   //备注


        }

        /// <summary>
        /// 提交
        /// </summary>
        public void GoSubmit()
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_IDENTITY_ID", id);

            LModel model = decipher.GetModel(filter);


            int rowSid = model.Get<int>("BIZ_SID");

            if (rowSid == 2)
            {
                MessageBox.Alert("此单已经提交，无需重复提交.");
                return;
            }

            if(rowSid == 99)
            {
                MessageBox.Alert("此单已经结案，不能提交了！");
                return;
            }



            model.SetTakeChange(true);

            string billNo = IfUtil.NotBlank(model["COL_1"], string.Empty);

            if (StringUtil.IsBlank(billNo))
            {
                //billNo = BizCommon.BillIdentityMgr.NewCodeForMonth("XS_SCZLD", "SC");   //吸塑生产指令单

                billNo = BizCodeMgr.NewCode("sczl");

                model["COL_1"] = billNo;

                this.TextBox1.Value = billNo;
            }

            model["COL_4"] = ModelConvert.ChangeType(TextBox12.Value, LMFieldDBTypes.DateTime, false);

            model["COL_2"] = this.comboBox2.Value;
            model["COL_3"] = this.comboBox1.Value;
            model["COL_10"] = this.comboBox3.Value;
            model["COL_6"] = StringUtil.NoBlank( remarkTb.Value,WebUtil.Form(remarkTb.ClientID));   //注：（临时解决，控件出问题，取不出值。）
            model["COL_12"] = this.comboBox4.Value; //混合排版
            model["BIZ_SID"] = 2;

            model["COL_11"] = BizServer.LoginName;  //更新制单员


            decipher.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);

            try
            {
                decipher.UpdateModel(model, true);


                DbCascadeFactory dbccFactory = new DbCascadeFactory();
                dbccFactory.ExecEnd += dbccFactory_ExecEnd;
                dbccFactory.Updated(null, model);

                decipher.TransactionCommit();


                this.Table103.ReadOnly = true;
                this.Table104.ReadOnly = true;
                this.Table105.ReadOnly = true;
                this.Table106.ReadOnly = true;
                this.mainClientTable.ReadOnly = true;


                MessageBox.Alert("提交成功!");
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("提交失败",ex);

                MessageBox.Alert("提交失败!");
            }

        }

        void dbccFactory_ExecEnd(object sender, DbCascadeEventArges e)
        {
            
            EC5.BizLogger.LogStepMgr.Insert(e.Steps[0], e.OpText,e.Remark);
        }

        /// <summary>
        /// 撤销操作
        /// </summary>
        public void Revoke() 
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_IDENTITY_ID", id);

            LModel model = decipher.GetModel(filter);


            int rowSid = model.Get<int>("BIZ_SID");

            if (rowSid == 0)
            {
                MessageBox.Alert("此单已经能用，无需再撤销.");
                return;
            }

            if(rowSid == 99)
            {
                MessageBox.Alert("此单已经结案，不能撤销");
                return;
            }

            model.SetTakeChange(true);

            model["BIZ_SID"] = 0;

            decipher.UpdateModel(model, true);

            DbCascadeFactory dbccFactory = new DbCascadeFactory();
            dbccFactory.ExecEnd += dbccFactory_ExecEnd;
            dbccFactory.Updated(null, model);


            this.Table103.ReadOnly = false;
            this.Table104.ReadOnly = false;
            this.Table105.ReadOnly = false;
            this.Table106.ReadOnly = false;
            this.mainClientTable.ReadOnly = false;

            MessageBox.Alert("撤销成功了！");
        }

        /// <summary>
        /// 增加产品组按钮事件 导入订单数据
        /// </summary>
        /// <param name="idStr"></param>
        public void GoInBill1(string idStr)
        {

            int[] ids = StringUtil.ToIntList(idStr);

            if(ids.Length == 0)
            {
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            //ViewSet vSet = new ViewSet();
            //vSet.Select(decipher, 233);

            //string otherWhere = "UT_091.ROW_IDENTITY_ID in (" + ArrayUtil.ToString(ids) + ")";
            //string tSql = GetTSql(vSet, otherWhere);

            //LModelList<LModel> rModels = decipher.GetModelList(tSql);


            LightModelFilter filter = new LightModelFilter("UT_091");
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("ROW_IDENTITY_ID", ArrayUtil.ToString(ids), Logic.In);

            LModelList<LModel> rModels = decipher.GetModelList(filter);


            int mapId = 60; //映射表ID

            MapSet mapSet = MapSet.SelectSID_0(decipher, mapId);

            IG2_MAP map = mapSet.Map;

            int parentId = WebUtil.QueryInt("id");
            string p_id_field = "COL_13";


            List<LModel> lModels = new List<LModel>();

            foreach (LModel rModel in rModels)
            {

                LModel lModel = new LModel(map.L_TABLE);

                //映射数据
                App.InfoGrid2.Bll.MapMgr.MapData(mapSet, rModel, lModel);

                lModel[p_id_field] = parentId;
                lModel["COL_16"] = "吸塑成品";

                lModels.Add(lModel);
            }


            foreach (LModel model in lModels)
            {
                decipher.InsertModel(model);
            }

            if (this.StoreUT102 != null)
            {
                this.StoreUT102.Refresh();
            }

        }


        public static string GetTSql(ViewSet vSet,string otherWhere)
        {
            List<string> fields = new List<string>();

            string tSqlSelect = ViewMgr.GetTSqlSelect(vSet, ref fields);

            string tSqlForm = ViewMgr.GetTSqlForm(vSet);

            string tSqlWhere = ViewMgr.GetTSqlWhere(vSet);

            string tSqlOrder = ViewMgr.GetTSqlOrder(vSet, fields);

            StringBuilder tSql = new StringBuilder("SELECT ");

            tSql.Append(tSqlSelect);

            tSql.AppendLine().Append(" FROM ").Append(tSqlForm);


            if (!StringUtil.IsBlank(tSqlWhere))
            {
                tSqlWhere += " AND " + otherWhere;
            }
            else
            {
                tSqlWhere = otherWhere;
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


        /// <summary>
        /// 打印整张胶盒生产施工单
        /// </summary>
        public void Print(string PrintCode) 
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_IDENTITY_ID", id);

            LModel model101 = decipher.GetModel(filter);

            if (model101 == null)
            {
                MessageBox.Alert("找不到订单明细！");
                return;
            }

            LightModelFilter filter102 = new LightModelFilter("UT_102");
            filter102.And("COL_13", id);
            filter102.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            LModel lm102 = decipher.GetModel(filter102);

            //订单明细的订单号
            string col_2 = lm102.Get<string>("COL_2");



            try
            {

                NOPIHandlerEX nopiEx = new NOPIHandlerEX();

                string patha = Server.MapPath("/PrintTemplate/吸塑施工单.xls");

                SheetPro sp = nopiEx.ReadExcel(patha);


                //设置标题
                SetHead(sp, model101, col_2);
                
                //设置数据
                SetData(sp);

                WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel", FileUtil.NewFielname(".xls"));
                wFile.CreateDir();

                nopiEx.WriteExcel(sp, wFile.PhysicalPath);
                nopiEx.Dispose();

                BIZ_PRINT_FILE file = new BIZ_PRINT_FILE()
                {
                    FILE_URL = wFile.RelativePath,
                    PRINT_CODE = PrintCode,
                    ROW_DATE_CREATE = DateTime.Now,
                    ROW_SID = 0
                };

                decipher.InsertModel(file);


            }
            catch (Exception ex)
            {
                throw new Exception("导出 Excel文件出错了！", ex);
            }
          


        }

        /// <summary>
        /// 这是设置标题样式和值的
        /// </summary>
        /// <param name="sp">模板值</param>
        /// <param name="model">主表数据</param>
        private void SetHead(SheetPro sp, LModel model,string col_2)
        {
            string CreateDate = IfUtil.NotBlank(model["ROW_DATE_CREATE"], ""); ///制单日期
            string NO = IfUtil.NotBlank(model["COL_1"], "");///生产单号
            string ROW_AUTHOR_ORG_CODE = IfUtil.NotBlank(model["ROW_AUTHOR_ORG_CODE"], "");///创建人
            string col_6 = IfUtil.NotBlank(model["COL_6"], "");                ///生产要求

            sp.DataArea[2].cellPro[0].Value = "制单日期: " + CreateDate;

            sp.DataArea[2].cellPro[4].Value = "销售订单号:"+ col_2;    ///订单号

            sp.DataArea[2].cellPro[9].Value = "ERP单号:" + NO;

            sp.DataArea[sp.DataArea.Count - 3].cellPro[1].Value = col_6;

            sp.DataArea[sp.DataArea.Count - 1].cellPro[1].Value = "制单人员： " + ROW_AUTHOR_ORG_CODE;

            sp.DataArea[sp.DataArea.Count - 1].cellPro[6].Value = "打印日期： " + DateTime.Now;

        }

        private void SetData(SheetPro sp)
        {
            try
            {
                RowProCollection rpc1 = CreateUT102Cell(sp);
                RowProCollection rpc2 = CreateUT103Cell(sp);
                RowProCollection rpc3 = CreateUT104Cell1(sp);
                RowProCollection rpc4 = CreateUT104Cell2(sp);
                RowProCollection rpc5 = CreateUT104Cell3(sp);

                #region 这是在处理合并信息的

                List<CellPro> cpList = new List<CellPro>();
                int num = 0;
                for (int i = 0; i < rpc5.Count; i++)
                {
                    ///这是在处理合并信息的
                    foreach (CellPro cp in sp.SpanCellAndRow.cellPro)
                    {
                        if (cp.SpanFirstRow != 16)
                        {
                            continue;
                        }

                        CellPro cp1 = new CellPro()
                        {
                            SpanFirstCell = cp.SpanFirstCell,
                            SpanLastCell = cp.SpanLastCell,
                            SpanFirstRow = cp.SpanFirstRow,
                            SpanLastRow = cp.SpanLastRow
                        };

                        cp1.SpanFirstRow += rpc1.Count - 1 + rpc2.Count - 1 + rpc3.Count - 1 + rpc4.Count - 1 + i;
                        cp1.SpanLastRow += rpc1.Count - 1 + rpc2.Count - 1 + rpc3.Count - 1 + rpc4.Count - 1 + i;

                        cpList.Add(cp1);
                        
                    }


                }

                for (int i = 0; i < rpc4.Count; i++)
                {
                    ///这是在处理合并信息的
                    foreach (CellPro cp in sp.SpanCellAndRow.cellPro)
                    {
                        if (cp.SpanFirstRow != 13)
                        {
                            continue;
                        }

                        CellPro cp1 = new CellPro()
                        {
                            SpanFirstCell = cp.SpanFirstCell,
                            SpanLastCell = cp.SpanLastCell,
                            SpanFirstRow = cp.SpanFirstRow,
                            SpanLastRow = cp.SpanLastRow
                        };

                        cp1.SpanFirstRow += rpc1.Count - 1 + rpc2.Count - 1 + rpc3.Count - 1 + i;
                        cp1.SpanLastRow += rpc1.Count - 1 + rpc2.Count - 1 + rpc3.Count - 1 + i;

                        cpList.Add(cp1);
                        
                    }



                }


                for (int i = 0; i < rpc3.Count; i++)
                {
                    ///这是在处理合并信息的
                    foreach (CellPro cp in sp.SpanCellAndRow.cellPro)
                    {
                        if (cp.SpanFirstRow != 10)
                        {
                            continue;
                        }

                        CellPro cp1 = new CellPro()
                        {
                            SpanFirstCell = cp.SpanFirstCell,
                            SpanLastCell = cp.SpanLastCell,
                            SpanFirstRow = cp.SpanFirstRow,
                            SpanLastRow = cp.SpanLastRow
                        };

                        cp1.SpanFirstRow += rpc1.Count - 1 + rpc2.Count - 1 + i;
                        cp1.SpanLastRow += rpc1.Count - 1 + rpc2.Count - 1 + i;

                        cpList.Add(cp1);
                        
                    }



                }

                for (int i = 0; i < rpc2.Count; i++)
                {
                    ///这是在处理合并信息的
                    foreach (CellPro cp in sp.SpanCellAndRow.cellPro)
                    {
                        if (cp.SpanFirstRow != 7)
                        {
                            continue;
                        }

                        CellPro cp1 = new CellPro()
                        {
                            SpanFirstCell = cp.SpanFirstCell,
                            SpanLastCell = cp.SpanLastCell,
                            SpanFirstRow = cp.SpanFirstRow,
                            SpanLastRow = cp.SpanLastRow
                        };

                        cp1.SpanFirstRow += rpc1.Count - 1 + i;
                        cp1.SpanLastRow += rpc1.Count - 1 + i;

                        cpList.Add(cp1);
                        
                    }


                }

                for (int i = 0; i < rpc1.Count; i++)
                {
                    ///这是在处理合并信息的
                    foreach (CellPro cp in sp.SpanCellAndRow.cellPro)
                    {
                        if (cp.SpanFirstRow != 4)
                        {
                            continue;
                        }

                        CellPro cp1 = new CellPro()
                        {
                            SpanFirstCell = cp.SpanFirstCell,
                            SpanLastCell = cp.SpanLastCell,
                            SpanFirstRow = cp.SpanFirstRow,
                            SpanLastRow = cp.SpanLastRow
                        };

                        cp1.SpanFirstRow += i;
                        cp1.SpanLastRow += i;

                        cpList.Add(cp1);
                        
                    }



                }

                List<CellPro> cpList1 = new List<CellPro>();

                foreach (CellPro cp in sp.SpanCellAndRow.cellPro)
                {
                    if (cp.SpanFirstRow == 4 || cp.SpanFirstRow == 7 || cp.SpanFirstRow == 10 || cp.SpanFirstRow == 13 || cp.SpanFirstRow == 16)
                    {
                        cpList1.Add(cp);

                    }

                }




                foreach (CellPro cp in sp.SpanCellAndRow.cellPro)
                {
                    if (cp.SpanFirstRow > 16 && cp.Display)
                    {
                        cp.SpanFirstRow += rpc1.Count - 1 + rpc2.Count - 1 + rpc3.Count - 1 + rpc4.Count - 1 + rpc5.Count - 1;
                        cp.SpanLastRow += rpc1.Count - 1 + rpc2.Count - 1 + rpc3.Count - 1 + rpc4.Count - 1 + rpc5.Count - 1;
                        cp.Display = false;
                    }

                    if (cp.SpanFirstRow > 13 && cp.SpanFirstRow < 16 && cp.Display)
                    {
                        cp.SpanFirstRow += rpc1.Count - 1 + rpc2.Count - 1 + rpc3.Count - 1 + rpc4.Count - 1;
                        cp.SpanLastRow += rpc1.Count - 1 + rpc2.Count - 1 + rpc3.Count - 1 + rpc4.Count - 1;
                        cp.Display = false;
                    }

                    if (cp.SpanFirstRow > 10 && cp.SpanFirstRow < 13 && cp.Display)
                    {
                        cp.SpanFirstRow += rpc1.Count - 1 + rpc2.Count - 1 + rpc3.Count - 1;
                        cp.SpanLastRow += rpc1.Count - 1 + rpc2.Count - 1 + rpc3.Count - 1;
                        cp.Display = false;
                    }

                    if (cp.SpanFirstRow > 7 && cp.SpanFirstRow < 10 && cp.Display)
                    {
                        cp.SpanFirstRow += rpc1.Count - 1 + rpc2.Count - 1;
                        cp.SpanLastRow += rpc1.Count - 1 + rpc2.Count - 1;
                        cp.Display = false;
                    }

                    if (cp.SpanFirstRow > 4 && cp.SpanFirstRow < 7 && cp.Display)
                    {
                        cp.SpanFirstRow += rpc1.Count - 1;
                        cp.SpanLastRow += rpc1.Count - 1;
                        cp.Display = false;
                    }
                }


                foreach (CellPro cp in cpList1)
                {
                    sp.SpanCellAndRow.cellPro.Remove(cp);
                }
                sp.SpanCellAndRow.cellPro.AddRange(cpList);

                #endregion


                sp.DataArea.RemoveAt(16);
                sp.DataArea.InsertRange(16, rpc5);





                sp.DataArea.RemoveAt(13);
                sp.DataArea.InsertRange(13, rpc4);



                sp.DataArea.RemoveAt(10);
                sp.DataArea.InsertRange(10, rpc3);



                sp.DataArea.RemoveAt(7);
                sp.DataArea.InsertRange(7, rpc2);




                sp.DataArea.RemoveAt(4);
                sp.DataArea.InsertRange(4, rpc1);


                sp.rowProList.AddRange(sp.DataArea);

            }
            catch (Exception ex)
            {
                throw new Exception("设置数据部分出错了！", ex);
            }
        }

        /// <summary>
        /// 创建订单明细数据
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        private RowProCollection CreateUT102Cell(SheetPro sp) 
        {
            RowProCollection rpList = new RowProCollection();

            int id = WebUtil.QueryInt("id");

            LightModelFilter filter = new LightModelFilter("UT_102");
            filter.And("COL_13", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            DbDecipher decipher = ModelAction.OpenDecipher();
            
            try
            {

                LModelList<LModel> models = decipher.GetModelList(filter);

                foreach (LModel item in models)
                {
                    ///客户编码
                    string CustomerName = IfUtil.NotBlank(item["COL_22"], "");
                    ///产品名称
                    string ProName = IfUtil.NotBlank(item["COL_4"], "");
                    ///订单数量
                    string OrderNum = IfUtil.NotBlank(item["COL_7"], "");
                    ///生产数量
                    string ProductionNum = IfUtil.NotBlank(item["COL_9"], "");
                    ///模具个数
                    string MouldNum = IfUtil.NotBlank(item["COL_8"], "");
                    ///交货日期
                    string DeliveryDate = "";
                    //规格尺寸
                    string col_5 = IfUtil.NotBlank(item["COL_5"], ""); ;

                    
                    if(!item.IsNull("COL_11"))
                    {
                        DeliveryDate = ((DateTime)item["COL_11"]).ToString("yyyy-MM-dd");   //2005年11月5日;
                    }
                    

                    RowPro rp = CopyData(sp.DataArea, 4, 4);
                    rp.cellPro[0].Value = CustomerName;
                    rp.cellPro[1].Value = ProName;
                    rp.cellPro[6].Value = col_5;
                    rp.cellPro[8].Value = OrderNum;
                    rp.cellPro[9].Value = ProductionNum;
                    rp.cellPro[10].Value = MouldNum;
                    rp.cellPro[11].Value = DeliveryDate;

                    rpList.Add(rp);

                }
                return rpList;
            }
            catch (Exception ex) 
            {
                throw new Exception("创建订单明细数据出错了！",ex);
            }

        }


        /// <summary>
        /// 创建主材料数据
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        private RowProCollection CreateUT103Cell(SheetPro sp)
        {
            RowProCollection rpList = new RowProCollection();

            int id = WebUtil.QueryInt("id");

            LightModelFilter filter = new LightModelFilter("UT_103");
            filter.And("COL_15", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            DbDecipher decipher = ModelAction.OpenDecipher();
            try
            {

                LModelList<LModel> models = decipher.GetModelList(filter);

                foreach (LModel item in models)
                {
                    ///材料编号
                    string ProNO = IfUtil.NotBlank(item["COL_1"], "");
                    ///材料名称
                    string ProName = IfUtil.NotBlank(item["COL_2"], "");
                    ///板长
                    string col_6 = IfUtil.NotBlank(item["COL_6"], "");
                    ///排摸个数
                    string col_64 = IfUtil.NotBlank(item["COL_64"], "");
                    ///实际用料张数
                    string col_8 = IfUtil.NotBlank(item["COL_8"], "");
                    ///单位
                    string col_9 = IfUtil.NotBlank(item["COL_9"], "");
                    ///数量
                    string col_12 = IfUtil.NotBlank(item["COL_12"], "");
                    ///备注
                    string col_10 = IfUtil.NotBlank(item["COL_10"], "");

                    //50KG生产数
                    string col_18 = IfUtil.NotBlank(item["COL_18"], "");
                    ///是否植绒
                    string col_13 = IfUtil.NotBlank(item["COL_13"], "");
                    if (!string.IsNullOrEmpty(col_13))
                    {
                        ProName = col_13 + ProName;
                    }
                    string col_72 = IfUtil.NotBlank(item["COL_72"], "");
                    RowPro rp = CopyData(sp.DataArea, 7, 7);
                    rp.cellPro[0].Value = ProNO;
                    rp.cellPro[1].Value = ProName;
                    rp.cellPro[4].Value = col_64;
                    rp.cellPro[5].Value = col_6;
                    rp.cellPro[6].Value = col_18;
                    rp.cellPro[8].Value = col_72;
                    rp.cellPro[9].Value = col_8;
                    rp.cellPro[10].Value = col_12 + col_9;
                    rp.cellPro[11].Value = col_10;

                    rpList.Add(rp);

                }
                return rpList;
            }
            catch (Exception ex)
            {
                throw new Exception("创建主材料数据出错了！", ex);
            }

        }

        /// <summary>
        /// 创建模具数据
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        private RowProCollection CreateUT104Cell1(SheetPro sp)
        {
            RowProCollection rpList = new RowProCollection();

            int id = WebUtil.QueryInt("id");

            LightModelFilter filter = new LightModelFilter("UT_104");
            filter.And("COL_12", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("COL_29", "888");
            DbDecipher decipher = ModelAction.OpenDecipher();
            try
            {

                LModelList<LModel> models = decipher.GetModelList(filter);

                foreach (LModel item in models)
                {
                    ///模具编码
                    string col_28 = IfUtil.NotBlank(item["COL_28"], "");
                    ///模具名称
                    string col_1 = IfUtil.NotBlank(item["COL_1"], "");
                    ///新旧
                    string col_26 = IfUtil.NotBlank(item["COL_26"], "");
                    ///数量
                    string col_9 = IfUtil.NotBlank(item["COL_9"], "");
                    ///加工方式
                    string col_2 = IfUtil.NotBlank(item["COL_2"], "");
                    ///工序要求
                    string col_6 = IfUtil.NotBlank(item["COL_6"], "");
                    ///备注
                    string col_10 = IfUtil.NotBlank(item["COL_10"], "");
                    ///存放位置
                    string col_39 = IfUtil.NotBlank(item["COL_39"], "");

                    RowPro rp = CopyData(sp.DataArea, 10, 10);
                    rp.cellPro[0].Value = col_28;
                    rp.cellPro[1].Value = col_1;
                    rp.cellPro[4].Value = col_26;
                    rp.cellPro[5].Value = col_9;
                    rp.cellPro[6].Value = col_2;
                    rp.cellPro[8].Value = col_39;
                    rp.cellPro[9].Value = col_6;
                    rp.cellPro[11].Value = col_10;
 

                    rpList.Add(rp);

                }
                return rpList;
            }
            catch (Exception ex)
            {
                throw new Exception("创建模具料数据出错了！", ex);
            }

        }

        /// <summary>
        /// 创建刀具数据
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        private RowProCollection CreateUT104Cell2(SheetPro sp)
        {
            RowProCollection rpList = new RowProCollection();

            int id = WebUtil.QueryInt("id");

            LightModelFilter filter = new LightModelFilter("UT_104");
            filter.And("COL_12", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("COL_29", "777");
            DbDecipher decipher = ModelAction.OpenDecipher();
            try
            {

                LModelList<LModel> models = decipher.GetModelList(filter);

                foreach (LModel item in models)
                {
                    ///模具编码
                    string col_28 = IfUtil.NotBlank(item["COL_28"], "");
                    ///模具名称
                    string col_1 = IfUtil.NotBlank(item["COL_1"], "");
                    ///新旧
                    string col_26 = IfUtil.NotBlank(item["COL_26"], "");
                    ///数量
                    string col_9 = IfUtil.NotBlank(item["COL_9"], "");
                    ///加工方式
                    string col_2 = IfUtil.NotBlank(item["COL_2"], "");
                    ///工序要求
                    string col_6 = IfUtil.NotBlank(item["COL_6"], "");
                    ///备注
                    string col_10 = IfUtil.NotBlank(item["COL_10"], "");
                    ///存放位置
                    string col_39 = IfUtil.NotBlank(item["COL_39"], "");

                    RowPro rp = CopyData(sp.DataArea, 13, 13);
                    rp.cellPro[0].Value = col_28;
                    rp.cellPro[1].Value = col_1;
                    rp.cellPro[4].Value = col_26;
                    rp.cellPro[5].Value = col_9;
                    rp.cellPro[6].Value = col_2;
                    rp.cellPro[8].Value = col_39;
                    rp.cellPro[9].Value = col_6;
                    rp.cellPro[11].Value = col_10;


                    rpList.Add(rp);

                }
                return rpList;
            }
            catch (Exception ex)
            {
                throw new Exception("创建刀具料数据出错了！", ex);
            }

        }


        /// <summary>
        /// 创建工序数据
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        private RowProCollection CreateUT104Cell3(SheetPro sp)
        {
            RowProCollection rpList = new RowProCollection();

            int id = WebUtil.QueryInt("id");

            LightModelFilter filter = new LightModelFilter("UT_104");
            filter.And("COL_12", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("COL_29", "999");
            filter.TSqlOrderBy = "COL_GXSX asc";
            DbDecipher decipher = ModelAction.OpenDecipher();
            try
            {

                LModelList<LModel> models = decipher.GetModelList(filter);

                foreach (LModel item in models)
                {
                    ///部件名称
                    string col_3 = IfUtil.NotBlank(item["COL_3"], "");
                    ///工序名称
                    string col_1 = IfUtil.NotBlank(item["COL_1"], "");
                    ///加工方式
                    string col_2 = IfUtil.NotBlank(item["COL_2"], "");
                    ///投入数
                    string col_9 = IfUtil.NotBlank(item["COL_9"], "");
                    ///损耗
                    string col_8 = IfUtil.NotBlank(item["COL_8"], "");
                    ///工序要求
                    string col_6 = IfUtil.NotBlank(item["COL_6"], "");
               

                    RowPro rp = CopyData(sp.DataArea, 16, 16);
                    rp.cellPro[0].Value = col_3;
                    rp.cellPro[1].Value = col_1;
                    rp.cellPro[3].Value = col_2;
                    rp.cellPro[4].Value = col_6;
                    rpList.Add(rp);

                }
                return rpList;
            }
            catch (Exception ex)
            {
                throw new Exception("创建工序料数据出错了！", ex);
            }

        }

        /// <summary>
        /// 拷贝数据
        /// </summary>
        /// <returns></returns>
        private RowPro CopyData(RowProCollection item, int subFirstRow, int subLastRow)
        {
            try
            {
                RowPro rp1 = new RowPro();
        
                for (int i = subFirstRow; i <= subLastRow ; i++)
                {
                    RowPro rp = item[i];

                    foreach (CellPro cp in rp.cellPro)
                    {
                        CellPro cp1 = cp.Clone();
                        
                        rp1.cellPro.Add(cp1);

                    }

                    rp1.RowHeight = rp.RowHeight;


                }

                return rp1;

      

            }
            catch (Exception ex)
            {
                throw new Exception("这是拷贝数据发生错误！", ex);
            }

        }



        /// <summary>
        /// 自动调取产品组工艺
        /// </summary>
        public void AutoInputData() 
        {
            int id = WebUtil.QueryInt("id");

            LightModelFilter filter = new LightModelFilter("UT_102");
            filter.And("COL_13", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);


            //COL_9 = 生产数量， COL_8 = 排摸个数

            DbDecipher decipher = ModelAction.OpenDecipher();

            List<LModel> lm102List = decipher.GetModelList(filter);

            if(lm102List.Count == 0)
            {
                MessageBox.Alert("请先添加订单！");
                return;
            }

            

            ///所有的产品ID
            string col_42ID = "";
            ///生产指令单号
            string col_17 ="";
            for (int i = 0; i < lm102List.Count;i++ )
            {
                var item = lm102List[i];

                if(i != 0)
                {
                    col_42ID += ",";
                }
                col_42ID += "'" + IfUtil.NotBlank(item["COL_42"], "") + "'";
            }
            string sql = string.Format(@"select Distinct COL_17 from dbo.UT_102 where row_sid >=0 and COL_42 in({0}) and COL_19 ='模板'", col_42ID);

            List<LModel> lm102List1 = decipher.GetModelList(sql);


            string billName = "";//单号

            foreach (var item in lm102List1)
            {
                col_17 = IfUtil.NotBlank(item["COL_17"], "");

                if (string.IsNullOrEmpty(col_17))
                {
                    continue;
                }

                int num = 0;

                int count = decipher.ExecuteScalar<int>(string.Format("select count(ROW_IDENTITY_ID) from dbo.UT_102 where COL_17 = '{0}' and row_sid >=0 and COL_19 ='模板'", col_17));
                

                if (count != lm102List.Count)
                {
                    continue;
                }


                foreach (var subItem in lm102List)
                {
                    string newID = IfUtil.NotBlank(subItem["COL_42"], "");
                    int cou = decipher.ExecuteScalar<int>(string.Format("select count(ROW_IDENTITY_ID) from dbo.UT_102 where COL_17 = '{0}' and COL_42='{1}' and row_sid >=0 and COL_19 ='模板'", col_17, newID));

                    if(cou ==0)
                    {
                        break;
                    }
                    num += cou;

                }

                if (num == lm102List.Count)
                {
                    billName = col_17;
                    break;
                }

            }

            if (string.IsNullOrEmpty(billName))
            {
                MessageBox.Alert("未能找到匹配工艺信息！");
                return;
            }

            try
            {


                InputDataByNo(id, billName, lm102List);
                this.StoreUT102.DataBind();
                this.StoreUT103.DataBind();
                this.StoreUT104.DataBind();
                this.StoreUT105.DataBind();
                this.StoreUT106.DataBind();
                EasyClick.Web.Mini.MiniHelper.EvalFormat("ShowGif('" + billName + "')");


                AutoCalculation("A");



            }
            catch (Exception ex) 
            {
                log.Error("导入数据出错了！",ex);
                MessageBox.Alert("调用数据出错了！");
            }


        }

        /// <summary>
        /// 根据生产指令单找到
        /// </summary>
        /// <param name="id"></param>
        /// <param name="billName"></param>
        private void InputDataByNo(int id, string billName, List<LModel> lm102List) 
        {
            DbDecipher decipher = ModelAction.OpenDecipher();


            try
            {

                LightModelFilter filter3 = new LightModelFilter("UT_103");
                filter3.And("COL_15", id);
                ///清空之前的主材料记录
                decipher.DeleteModels(filter3);


                LightModelFilter filter4 = new LightModelFilter("UT_104");
                filter4.And("COL_12", id);
                ///清空之前的工艺记录
                decipher.DeleteModels(filter4);


                decipher.BeginTransaction();

                ///这是更新订单信息的排摸个数
                foreach (var item in lm102List)
                {
                    string col_42ID = IfUtil.NotBlank(item["COL_42"], "");
                    LightModelFilter filter = new LightModelFilter("UT_102");
                    filter.And("COL_17", billName);
                    filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    filter.And("COL_42", col_42ID);

                    LModel lm = decipher.GetModel(filter);

                    item["COL_8"] = IfUtil.NotBlank(lm["COL_8"], "");
                    decipher.UpdateModelProps(item, "COL_8");

                }


               

                LightModelFilter filter1 = new LightModelFilter("UT_103");
                filter1.And("COL_16", billName);
                filter1.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                List<LModel> lm103 = decipher.GetModelList(filter1);
                List<LModel> lm103new = new List<LModel>();
                foreach (var item in lm103)
                {
                    LModel New103 = new LModel("UT_103");
                    item.CopyTo(New103,true);
                    New103["COL_15"] = id;
                    New103["COL_7"] = 0;
                    New103["COL_8"] = 0;
                    New103["COL_10"] = 0;
                    New103["COL_16"] = "";
                    New103["BIZ_SID"] = 0;
                    New103["ROW_SID"] = 0;
                    lm103new.Add(New103);
                }


                

                LightModelFilter filter2 = new LightModelFilter("UT_104");
                filter2.And("COL_11", billName);
                filter2.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                List<LModel> lm104 = decipher.GetModelList(filter2);

                List<LModel> lm104new = new List<LModel>();

                
                foreach (var item in lm104)
                {
                    LModel New104 = new LModel("UT_104");
                    item.CopyTo(New104,true);

                    New104["COL_12"] = id;
                    New104["COL_9"] = 0;
                    New104["COL_11"] = "";
                    New104["BIZ_SID"] = 0;
                    New104["ROW_SID"] = 0;
                    New104["COL_33"] = 0;
                    lm104new.Add(New104);

                }



                foreach(var item in lm103new)
                {
                    DbCascadeRule.Insert(item);
                }

                foreach(var item in lm104new)
                {

                    DbCascadeRule.Insert(item);
                }


                decipher.TransactionCommit();

            }
            catch (Exception ex) 
            {
                decipher.TransactionRollback();
                throw new Exception("调用数据出错了！",ex);
            }
        }

        /// <summary>
        /// 结案按钮事件
        /// </summary>
        public void GoSubmitClosed() 
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_IDENTITY_ID", id);

            LModel model = decipher.GetModel(filter);


            int rowSid = model.Get<int>("BIZ_SID");

            if (rowSid != 2)
            {
                MessageBox.Alert("此单还未提交，请先提交再结案！");
                return;
            }


            model.SetTakeChange(true);

            string billNo = IfUtil.NotBlank(model["COL_1"], string.Empty);

            if (StringUtil.IsBlank(billNo))
            {
                //billNo = BizCommon.BillIdentityMgr.NewCodeForMonth("XS_SCZLD", "SC");   //吸塑生产指令单

                billNo = BizCodeMgr.NewCode("sczl");
                model["COL_1"] = billNo;

                this.TextBox1.Value = billNo;
            }

            model["BIZ_SID"] = 99;

            try
            {
                decipher.UpdateModel(model, true);


                DbCascadeFactory dbccFactory = new DbCascadeFactory();
                dbccFactory.ExecEnd += dbccFactory_ExecEnd;
                dbccFactory.Updated(null, model);


                this.Table103.ReadOnly = true;
                this.Table104.ReadOnly = true;
                this.Table105.ReadOnly = true;
                this.Table106.ReadOnly = true;
                this.mainClientTable.ReadOnly = true;




                MessageBox.Alert("结案成功!");
            }
            catch (Exception ex)
            {
                log.Error("结案失败", ex);

                MessageBox.Alert("结案失败!");
            }
        }

        /// <summary>
        /// 撤销结案按钮事件
        /// </summary>
        public void RevokeClosed() 
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_IDENTITY_ID", id);

            LModel model = decipher.GetModel(filter);


            int rowSid = model.Get<int>("BIZ_SID");

            if (rowSid == 2)
            {
                MessageBox.Alert("此单已经能用，无需再撤销.");
                return;
            }
            model.SetTakeChange(true);

            model["BIZ_SID"] = 2;

            decipher.UpdateModel(model, true);

            DbCascadeFactory dbccFactory = new DbCascadeFactory();
            dbccFactory.ExecEnd += dbccFactory_ExecEnd;
            dbccFactory.Updated(null, model);


            this.Table103.ReadOnly = false;
            this.Table104.ReadOnly = false;
            this.Table105.ReadOnly = false;
            this.Table106.ReadOnly = false;
            this.mainClientTable.ReadOnly = false;


            MessageBox.Alert("撤销成功了！");
        }


        /// <summary>
        /// 需求量计算按钮
        /// </summary>
        /// <param name="name">只有参数为空,才会有提示</param>
        public void AutoCalculation(string name) 
        {
            int id = WebUtil.QueryInt("id");

            LightModelFilter filter = new LightModelFilter("UT_102");
            filter.And("COL_13", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Fields = new string[] { "COL_9", "COL_8", "COL_7" };

            //COL_9 = 生产数量， COL_8 = 排摸个数 ，COL_7 = 订单数量

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<LModel> models = decipher.GetModelList(filter);

            decimal totalCol9 = LModelMath.Sum(models, "COL_9");
            decimal totalCol8 = LModelMath.Sum(models, "COL_8");
            decimal totalCol7 = LModelMath.Sum(models, "COL_7");


            LightModelFilter filter103 = new LightModelFilter("UT_103");
            filter103.And("COL_15", id);
            filter103.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            LModelList<LModel> lm103List = decipher.GetModelList(filter103);



            foreach (var model in lm103List)
            {

                model.SetTakeChange(true);


                model["COL_64"] = totalCol8;
                model["COL_65"] = totalCol9;

                if (totalCol8 != 0)
                {
                    decimal col_7 = totalCol9 / totalCol8;
                    decimal col_72 = totalCol7 / totalCol8;
                    model["COL_7"] = Math.Round(col_7);
                    model["COL_72"] = Math.Round(col_72);
                }

                

                if (!model.IsNull("COL_6"))
                {
                    decimal COL_6 = (decimal)model["COL_6"];

                    model["COL_19"] = COL_6 * 1000 + 70;
                }

                if (!model.IsNull("COL_7") && !model.IsNull("COL_11"))
                {
                    decimal COL_8 = model.Get<decimal>("COL_7") + model.Get<decimal>("COL_11");
                    model["COL_8"] = COL_8;

                }

                if (!model.IsNull("COL_4") && !model.IsNull("COL_5") && !model.IsNull("COL_17") && !model.IsNull("COL_19") && !model.IsNull("COL_8") && !model.IsNull("COL_9"))
                {
                    decimal col_4 = model.Get<decimal>("COL_4");
                    decimal COL_5 = model.Get<decimal>("COL_5");
                    decimal COL_17 = model.Get<decimal>("COL_17");
                    decimal COL_19 = model.Get<decimal>("COL_19");
                    decimal COL_8 = model.Get<decimal>("COL_8");
                    string COL_9 = model.Get<string>("COL_9");

                    if (COL_9 == "KG" || COL_9 == "公斤")
                    {
                        //      （生产数量之和 +  损耗张数    * 排摸个数之和 ）
                        decimal a = (totalCol9 + model.Get<decimal>("COL_11") * totalCol8);

                        ///    实际用量 =  （50000 / 宽度  /  ( 厚度 / 100 ) / 密度   / (长度 / 1000) ）                                                     
                        decimal COL_12 = (50000m / col_4 / (COL_17 / 100m) / COL_5 / (COL_19 / 1000m));
                        //              取整后         *  排摸个数之和 * 0.97
                        decimal x = Math.Floor(COL_12) * totalCol8 * 0.97m;
                        //    （生产数量之和 +  损耗张数  * 排摸个数之和 ） /    然后再取整  * 50
                        decimal y = a / Math.Floor(x) * 50m;

                        model["COL_12"] = Math.Floor(y);
                    }

                    if (COL_9 == "米")
                    {
                        ///实际用量 = 长度   X 实需张数 /1000 
                        decimal COL_12 = COL_19 * COL_8 / 1000;
                        decimal x = Math.Floor(COL_12);
                        model["COL_12"] = x;
   
                    }

                    if (COL_9 == "张")
                    {
                        ///实际用量 = 实需张数
                        decimal COL_12 = COL_8;
                        decimal x = Math.Floor(COL_12);
                        model["COL_12"] = x;
    
                    }
                }

                if (!model.IsNull("COL_17") && !model.IsNull("COL_5") && !model.IsNull("COL_19") && !model.IsNull("COL_4"))
                {



                    //        50KG生产个数 /             / 宽度                          (厚度 /100)                     /  密度                       / (长度/1000)                  
                    decimal COL_18 = 50000 / model.Get<decimal>("COL_4") / (model.Get<decimal>("COL_17") / 100m) / model.Get<decimal>("COL_5") / (model.Get<decimal>("COL_19") / 1000m);
                    //  取整后   *  排摸个数之和
                    decimal x = Math.Floor(COL_18) * totalCol8;
                    //然后再取整   *  0.97
                    decimal y = Math.Floor(x) * 0.97m;
                    model["COL_18"] = Math.Floor(y);


                }




                DbCascadeRule.Update(model);

                //decipher.UpdateModelProps(model, "COL_64", "COL_65", "COL_7", "COL_19", "COL_8", "COL_12", "COL_18","COL_72");


            }


            LightModelFilter filter104 = new LightModelFilter("UT_104");
            filter104.And("COL_12", id);
            filter104.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter104.And("COL_29",999);
            LModelList<LModel> lm104List = decipher.GetModelList(filter104);



            foreach(var model in lm104List)
            {

                model.SetTakeChange(true);



                string znjs = (string)model["COL_43"];  //判断这个字段值是否等于 "智能运算"

                if (znjs != "智能运算")
                {
                    continue;
                }

                string unit = model.Get<string>("COL_4");   //计量单位名称


                if(string.IsNullOrEmpty(unit))
                {
                    continue;
                }

                unit = unit.Trim();

                if (unit == "个")
                {
                    model["COL_7"] = totalCol9;    //生产数量之和
                }
                else if (unit == "张")
                {
                    decimal num = totalCol9 / totalCol8;     //生产数量之和   / 排摸个数之和

                    model["COL_7"] = Math.Round(num);

                }
                else if (unit == "米")
                {
                    if (lm103List.Count > 0)
                    {
                        //  计划数量 =  长度 * 实需张数 / 1000
                        model["COL_7"] = lm103List[0].Get<decimal>("COL_19") * lm103List[0].Get<decimal>("COL_8") / 1000m;

                    }

                }

                if (!model.IsNull("COL_8"))
                {

                    model["COL_9"] = (decimal)model["COL_7"] + (decimal)model["COL_8"];
                }
                else 
                {
                    model["COL_9"] = (decimal)model["COL_7"];
                }


                DbCascadeRule.Update(model);

                //decipher.UpdateModelProps(model,"COL_9","COL_7");


            }

            if(string.IsNullOrEmpty(name))
            {
                MessageBox.Alert("计算完成！");
            }
           
            this.StoreUT102.Refresh();
            this.StoreUT103.Refresh();
            this.StoreUT104.Refresh();
            this.StoreUT105.Refresh();
            this.StoreUT106.Refresh();

        }



    }
}