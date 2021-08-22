using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.AppDomainPlugin;
using EC5.IG2.BizBase;
using EC5.IG2.Core;
using EC5.IG2.Plugin;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace App.InfoGrid2.View.Biz.Rosin.Forms
{
    public partial class WareForm : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 用户信息
        /// </summary>
        public EC5.SystemBoard.EcUserState EcUser
        {
            get { return EC5.SystemBoard.EcContext.Current.User; }
        }


        /// <summary>
        /// 是否为设计师
        /// </summary>
        /// <returns></returns>
        public bool IsBuilder()
        {
            return this.EcUser.Roles.Exist(IG2Param.Role.BUILDER);
        }

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

        }

        protected override void OnLoad(EventArgs e)
        {
            DbCascadeRule.Bind(this.storeMain1);

            this.storeMain1.Updating += StoreMain1_Updating;
            this.storeProd1.Inserting += StoreProd1_Inserting;
            this.storeProd1.Inserted += storeProd1_Inserted;

            this.tableProd1.Command += TableProd1_Command;

            triggerBox1.ButtonClickCallback += TriggerBox1_ButtonClickCallback;


            if (!this.IsPostBack)
            {

                string io_tag = WebUtil.Query("io_tag");

                //是否是出口
                bool is_o = (io_tag == "O");

                tbb_002_to_002.Visible = is_o;
                tbx_COL_7.Visible = is_o;
                tbx_COL_8.Visible = is_o;
                tbx_COL_9.Visible = is_o;
                dp_COL_10.Visible = is_o;

                textBox12.Visible = is_o;
                button1.Visible = is_o;

                //快速修改'等级'按钮
                QuickLevelEditBtn.Visible = !is_o;

                PrintCode1Btn.Visible = !is_o;


                BizHelper.Full(cb_store, "UT_010", "COL_1","COL_1");    //仓库信息填充到下拉框

                OnInitData();


                this.storeMain1.DataBind();

                this.storeProd1.DataBind();
            }

        }

        protected void TriggerBox1_ButtonClickCallback(object sender, string data)
        {
            var urlStr = $"/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={"VIEW"}&viewId={17}";

            Window win = new Window("选择", 800, 600);

            win.ContentPath = urlStr;
            win.StartPosition = WindowStartPosition.CenterScreen;

            win.WindowClosed += Win_WindowClosed;

            win.ShowDialog();
        }

        protected void Win_WindowClosed(object sender, string data)
        {
            DynSModel e = DynSModel.ParseJson(data);

            if(e["result"] != "ok")
            {
                return;
            }

            SModel row = e["row"] as SModel;

            DataRecord record = this.storeMain1.GetDataCurrent();


            foreach (MapItem map in this.triggerBox1.MapItems)
            {
                string value = Convert.ToString(row[map.SrcField]);

                this.storeMain1.SetRecordValue(true,record.Id, map.TargetField, value);
            }
            

        }



        private void StoreProd1_Inserting(object sender, ObjectCancelEventArgs e)
        {
            
        }

        private void StoreMain1_Updating(object sender, ObjectCancelEventArgs e)
        {
            BizHelper.FullForUpdate(e.Object as LModel);
        }

        private void TableProd1_Command(object sender, TableCommandEventArgs e)
        {
            if (e.CommandName == "GoEdit")
            {
                DataRecord record = e.Record;

                object pk = record.Id;

                DbDecipher decipher = ModelAction.OpenDecipher();

                LModel model = decipher.GetModelByPk("UT_002", pk);


                int parentId = model.Get<int>("COL_1");
                string ioTag = (string)model["IO_TAG"];

                string url = $"/App/InfoGrid2/View/Biz/Rosin/Forms/WareProdForm.aspx?";

                url += "&io_tag=" + ioTag.ToUpper();
                url += "&parent_id=" + parentId;
                url += "&row_id=" + pk;


                string title = ioTag == "I" ? "入库货物" : "入库货物";

                Window win = new Window(title);
                win.ContentPath = url;
                win.State = WindowState.Max;
                win.WindowClosed += Win_WindowClosed1;
                win.ShowDialog();

            }
        }

        protected void Win_WindowClosed1(object sender, string data)
        {
            this.storeProd1.Refresh();
        }

        private void storeProd1_Inserted(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            LModel model = e.Object as LModel;

            object pk = model.GetPk();

            int parentId = model.Get<int>("COL_1");
            string ioTag = model.Get<string>("IO_TAG");

            string url = $"/App/InfoGrid2/View/Biz/Rosin/Forms/WareProdForm.aspx?";

            url += "&io_tag=" + ioTag.ToUpper();
            url += "&parent_id=" + parentId;
            url += "&row_id=" + pk;

            string title = ioTag == "I" ? "入库货物" : "入库货物";

            Window win = new Window(title);
            win.ContentPath = url;
            win.State = WindowState.Max;
            win.WindowClosed += Win_WindowClosed1;
            win.ShowDialog();
        }


        private void OnInitData()
        {
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            string title = io_tag == "I" ? "入库单" : "出库单";

            this.headLab.Value = "<span class='page-head' >" + title + "</span>";


        }

        public void GoSave()
        {
            int row_id = WebUtil.QueryInt("row_id");
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModelByPk("UT_001", row_id);




            try
            {
                DataRecord record = this.storeMain1.GetDataCurrent();

                bool success = WareMsg.ChangeBizSID_0_2(model, this.storeMain1, record, io_tag);

                if (success)
                {
                    Toast.Show("提交成功!");
                }
            }
            catch (Exception ex)
            {
                log.Error("提交失败", ex);
                MessageBox.Alert("提交失败!");
            }




            LModelList<LModel> prods = this.storeProd1.GetList() as LModelList<LModel>;

            if (io_tag == "I")
            {
                foreach (LModel prod in prods)
                {
                    prod["YSSL_NUM"] = prod.Get<decimal>("COL_18");
                    prod["YSZL_NUM"] = prod.Get<decimal>("COL_19");
                    prod["SL_NUM"] = prod.Get<decimal>("COL_21");
                    prod["ZL_NUM"] = prod.Get<decimal>("COL_23");

                    decipher.UpdateModelProps(prod, "YSSL_NUM", "YSZL_NUM", "SL_NUM", "ZL_NUM");
                }
            }
            else if (io_tag == "O")
            {
                foreach (LModel prod in prods)
                {
                    prod["YSSL_NUM"] = -prod.Get<decimal>("COL_18");
                    prod["YSZL_NUM"] = -prod.Get<decimal>("COL_19");
                    prod["SL_NUM"] = -prod.Get<decimal>("COL_21");
                    prod["ZL_NUM"] = -prod.Get<decimal>("COL_23");

                    decipher.UpdateModelProps(prod, "YSSL_NUM", "YSZL_NUM", "SL_NUM", "ZL_NUM");
                }
            }


            if(io_tag == "I")
            {
                LModelList<LModel> prods2 = this.storeProd1.GetList() as LModelList<LModel>;


                foreach (LModel prod in prods2)
                {
                    LModel total = GetTotal(prod.Get<string>("BIZ_ROW_CODE"));

                    prod["OUT_NUM"] = total["TOTAL_NUM"];   //出库数量
                    prod["OUT_NUM"] = total["TOTAL_WEIGHT"];   //出库重量

                    prod["SURPLUS_NUM"] = prod.Get<decimal>("COL_21") - prod.Get<decimal>("OUT_NUM");   //数量

                    prod["SURPLUS_WEIGHT"] = prod.Get<decimal>("COL_23") - prod.Get<decimal>("OUT_WEIGHT"); //重量

                    decipher.UpdateModelProps(prod, "SURPLUS_NUM", "SURPLUS_WEIGHT");
                }
            }
            else if(io_tag == "O")
            {
                LModelList<LModel> prods2 = this.storeProd1.GetList() as LModelList<LModel>;


                foreach (LModel prod in prods2)
                {
                    UpdateTotal(prod.Get<string>("SRC_PROD_ITEM_CODE"));
                }
            }


            UpdataUT002(model, row_id, io_tag);

            UpdateBizRowCode(model, row_id, model.Get<string>("BILL_NO"));

            WareMsg.UpdataStockAll();
        }


        private LModel GetTotal(string inBillCode)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_002");
            filter.AddFilter("ROW_SID >= 0");
            //filter.And("COL_1", row_id);
            //filter.AddFilter("IO_TAG = 'O'");
            filter.And("SRC_PROD_ITEM_CODE", inBillCode);
            filter.Fields = new string[] {"SUM(COL_21) as TOTAL_NUM" ,"SUM(COL_23) as TOTAL_WEIGHT"};

            LModel model = decipher.GetModel(filter);

            return model;
        }


        /// <summary>
        /// 产生业务代码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="row_id"></param>
        /// <param name="billCode">单据代码</param>
        private void UpdateBizRowCode(LModel model ,int row_id, string billCode)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<LModel> prods = this.storeProd1.GetList() as LModelList<LModel>;

            foreach (LModel prod in prods)
            {
                string BIZ_ROW_CODE = prod.Get<string>("BIZ_ROW_CODE");

                if (!StringUtil.IsBlank(BIZ_ROW_CODE))
                {
                    continue;
                }

                LightModelFilter filter = new LightModelFilter("UT_001");
                filter.AddFilter("ROW_SID >= 0");
                filter.And("ROW_IDENTITY_ID", row_id);
                filter.Fields = new string[] { "BIZ_SUB_IDENTTIY" };


                int newId = decipher.ExecuteScalar<int>(filter);


                prod["BIZ_ROW_CODE"] = string.Format("{0}-{1}-{2}", billCode, model["CLIENT_CODE"] , newId);
                prod["BIZ_ROW_ID"] = newId;

                decipher.UpdateModelProps(prod, "BIZ_ROW_CODE", "BIZ_ROW_ID");
                

                newId++;

                decipher.UpdateProps(filter, new object[] { "BIZ_SUB_IDENTTIY", newId });

            }

        }


        /// <summary>
        /// 提交的时候把UT_001的信息放到下面去
        /// </summary>
        /// <param name="lm001">UT_001对象</param>
        /// <param name="row_id">主表ID</param>
        /// <param name="io_tag">进出库类型</param>
        void UpdataUT002(LModel lm001,int row_id,string io_tag)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_002");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_1", lm001.GetPk());
            lmFilter.And("IO_TAG", io_tag);

            decipher.UpdateProps(lmFilter, new object[]{ "ROW_DATE_UPDATE", DateTime.Now, "P_BILL_CODE",lm001["BILL_NO"], "P_CLIENT_TEXT", lm001["CLIENT_NAME"] });
            
        }


        public void GoChangeBizSID_2_4()
        {
            bool isSuccess = ChangeBizSID(2, 4);



            int row_id = WebUtil.QueryInt("row_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModelByPk("UT_001", row_id);

            DataRecord record = this.storeMain1.GetDataCurrent();

            try
            {
                decipher.UpdateModelByPk("UT_001", row_id, new object[] {
                    "COL_2", BizServer.LoginName
                });

                this.storeMain1.SetRecordValue(record.Id, "COL_2", BizServer.LoginName);

                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }

        public void GoChangeBizSID_2_0()
        {
            ChangeBizSID(2, 0);
        }

        public void GoChangeBizSID_4_2()
        {
            ChangeBizSID(4, 2);
        }

        private bool ChangeBizSID(int startSID, int endSID)
        {

            int row_id = WebUtil.QueryInt("row_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModelByPk("UT_001", row_id);

            DataRecord record = this.storeMain1.GetDataCurrent();

            bool result = true;

            try
            {
                result = WareMsg.ChangeBizSID(model, this.storeMain1, record, startSID, endSID);

                if (!result)
                {
                    Toast.Show("操作无效");
                    return result;
                }

                Toast.Show("操作完成!");
            }
            catch (Exception ex)
            {
                log.Error(ex);

                MessageBox.Alert(ex.Message);
            }

            return result;
        }


        /// <summary>
        /// 自动插入数据
        /// </summary>
        private void AutoAdd_UT001(LModel ownerModel)
        {
            bool isRosinSystem = GlobelParam.GetValue<bool>("IS_ROSIN_SYSTEM", false);

            if (!isRosinSystem)
            {
                return;
            }

            LModelElement modelElem = ownerModel.GetModelElement();

            if (modelElem.DBTableName == "UT_001" && modelElem.HasField("CLIENT_NAME"))
            {
                string clientName = ownerModel.Get<string>("CLIENT_NAME").Trim();

                DbDecipher decipher = ModelAction.OpenDecipher();

                LightModelFilter filter = new LightModelFilter("UT_005");
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter.And("CLIENT_TEXT", clientName);

                if (!decipher.ExistsModels(filter))
                {
                    LModel model = new LModel("UT_005");
                    model["CLIENT_TEXT"] = clientName;

                    decipher.InsertModel(model);
                }

            }
        }

        /// <summary>
        /// 自动插入数据
        /// </summary>
        private void AutoAdd_UT002(LModel ownerModel)
        {
            bool isRosinSystem = GlobelParam.GetValue<bool>("IS_ROSIN_SYSTEM", false);

            if (!isRosinSystem)
            {
                return;
            }

            LModelElement modelElem = ownerModel.GetModelElement();

            if (modelElem.DBTableName == "UT_002" && modelElem.HasField("COL_3"))
            {
                string clientName = ownerModel.Get<string>("COL_3").Trim();

                DbDecipher decipher = ModelAction.OpenDecipher();

                LightModelFilter filter = new LightModelFilter("UT_006");
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter.And("PROD_TEXT", clientName);

                if (!decipher.ExistsModels(filter))
                {
                    LModel model = new LModel("UT_006");
                    model["PROD_TEXT"] = clientName;

                    decipher.InsertModel(model);
                }

            }
        }


        /// <summary>
        /// 更新主表的数量和金额
        /// </summary>
        /// <param name="id"></param>
        void UpdataNum(int id)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm001 = decipher.GetModelByPk("UT_001", id);


            LightModelFilter lmFilter = new LightModelFilter("UT_002");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_1", id);
            lmFilter.And("IO_TAG", lm001["IO_TAG"]);

            List<LModel> lm002s = decipher.GetModelList(lmFilter);

            lm001["NUM_TOTAL"] = LModelMath.Sum(lm002s, "COL_21");
            lm001["WEIGHT_TOTAL"] = LModelMath.Sum(lm002s, "COL_23");

            decipher.UpdateModelProps(lm001, "NUM_TOTAL", "WEIGHT_TOTAL");
        }


        /// <summary>
        /// 导入数据
        /// </summary>
        public void GoImportData(string ids)
        {

            string[] idList = StringUtil.Split(ids, ",");

            if (idList.Length == 0)
            {
                MessageBox.Alert("没有选择记录！");
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm009 = decipher.GetModelByPk("UT_009", idList[0]);

            if (lm009 == null)
            {
                MessageBox.Alert("有问题了，请联系系统管理员！");
                return;
            }


            int row_id = WebUtil.QueryInt("row_id");

            LModel lm001 = decipher.GetModelByPk("UT_001", row_id);

            LModel lm008 = decipher.GetModelByPk("UT_008", lm009["COL_1"]);

            List<LModel> lm002List = new List<LModel>();

            foreach (string id_009 in idList)
            {
                LModel lmNew009 = decipher.GetModelByPk("UT_009", id_009);


                LModel lm002 = new LModel("UT_002");

                lmNew009.CopyTo(lm002, true);


                lm002["ROW_DATE_CREATE"] = lm002["ROW_DATE_UPDATE"] = DateTime.Now;
                lm002["COL_1"] = row_id;
                lm002["BIZ_SID"] = 0;


                lm002["YSSL_NUM"] = lm002["COL_18"];
                lm002["YSZL_NUM"] = lm002["COL_19"];
                lm002["SL_NUM"] = lm002["COL_21"];
                lm002["ZL_NUM"] = lm002["COL_23"];


                if (lm002.Get<string>("IO_TAG") == "O")
                {
                    lm002["YSSL_NUM"] = -lm002.Get<decimal>("COL_18");
                    lm002["YSZL_NUM"] = -lm002.Get<decimal>("COL_19");
                    lm002["SL_NUM"] = -lm002.Get<decimal>("COL_21");
                    lm002["ZL_NUM"] = -lm002.Get<decimal>("COL_23");
                }

                lm002List.Add(lm002);


            }


            decipher.InsertModels<LModel>(lm002List);


            UpdataNum(row_id);


            Toast.Show("导入成功了！");

            storeProd1.Refresh();



        }

        public void GoImportData_2(string ids)
        {
            string[] idList = StringUtil.Split(ids, ",");

            if (idList.Length == 0)
            {
                MessageBox.Alert("没有选择记录！");
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm009 = decipher.GetModelByPk("UT_002", idList[0]);

            if (lm009 == null)
            {
                MessageBox.Alert("有问题了，请联系系统管理员！");
                return;
            }


            int row_id = WebUtil.QueryInt("row_id");

            LModel lm001 = decipher.GetModelByPk("UT_001", row_id);

            List<LModel> lm002List = new List<LModel>();

            foreach (string id_009 in idList)
            {
                LModel lmNew009 = decipher.GetModelByPk("UT_002", id_009);


                LModel lm002 = new LModel("UT_002");

                lmNew009.CopyTo(lm002, true);


                lm002["ROW_DATE_CREATE"] = lm002["ROW_DATE_UPDATE"] = DateTime.Now;
                lm002["COL_1"] = row_id;
                lm002["BIZ_SID"] = 0;
                lm002["IO_TAG"] = "O";

                lm002["YSSL_NUM"] = lm002["COL_18"];
                lm002["YSZL_NUM"] = lm002["COL_19"];
                lm002["SL_NUM"] = lm002["COL_21"];
                lm002["ZL_NUM"] = lm002["COL_23"];


                if (lm002.Get<string>("IO_TAG") == "O")
                {
                    lm002["YSSL_NUM"] = -lm002.Get<decimal>("COL_18");
                    lm002["YSZL_NUM"] = -lm002.Get<decimal>("COL_19");
                    lm002["SL_NUM"] = -lm002.Get<decimal>("COL_21");
                    lm002["ZL_NUM"] = -lm002.Get<decimal>("COL_23");

                    lm002["SRC_PROD_ITEM_CODE"] = lmNew009["BIZ_ROW_CODE"];
                    lm002["SRC_PROD_ITEM_ID"] = lmNew009["ROW_IDENTITY_ID"];

                    lm002["BIZ_ROW_CODE"] = string.Empty;
                }

                lm002List.Add(lm002);
            }


            decipher.InsertModels<LModel>(lm002List);

            UpdataNum(row_id);


            Toast.Show("导入成功了！");

            storeProd1.Refresh();


        }

        /// <summary>
        /// 商品信息导出到 Excel
        /// </summary>
        public void GoProdToExcel()
        {
            WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel", FileUtil.NewFielname(".xls"));
            wFile.CreateDir();

            try
            {   
                EC5.IG2.Plugin.Custom.InputOutExcelPlugin ePlugin = new EC5.IG2.Plugin.Custom.InputOutExcelPlugin();

                LModelList<LModel> models = (LModelList<LModel>)this.storeProd1.GetList();

                ePlugin.InputOutExcelFile(41, models, wFile.PhysicalPath);


                DownloadWindow.Show("下载 Excel 文件", wFile.RelativePath);
                                
                
            }
            catch (Exception ex)
            {
                log.Error("导出Excel文件出错了！", ex);

                MessageBox.Alert("导出 Excel 文件错误");

            }

        }


        public void GoShowQuickkLevel()
        {
            MessageBox.Prompt("货物等级 - 修改", "请输入等级值:", new CallbackEventHandler( QuickLevelEdit));

        }


        /// <summary>
        /// 批量修改“等级”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        protected void QuickLevelEdit(object sender, string data)
        {
            string levelStr = data;

            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = WebUtil.QueryInt("row_id");

            LModelList<LModel> models = this.storeProd1.GetList() as LModelList<LModel>;

            foreach (LModel model in models)
            {
                model["COL_28"] = levelStr;

                decipher.UpdateModelProps(model, "COL_28");
            }

            this.storeProd1.Refresh();
        }
        

        /// <summary>
        /// 打印一维码
        /// </summary>
        public void GoPrintCode1()
        {
            int row_id = WebUtil.QueryInt("row_id");


            Window win = new Window("打印条码");
            win.ContentPath = $"/App/InfoGrid2/view/biz/rosin/prints/FormPrint.aspx?id={row_id}";

            win.ShowDialog();

        }

        /// <summary>
        /// 打印二维码
        /// </summary>
        public void GoPrintCode2()
        {
            Window win = new Window("打印条码");
            win.ContentPath = "/App/InfoGrid2/view/biz/Rosin/Prints/FormPrint2.aspx";

            win.ShowDialog();
        }


        /// <summary>
        /// 扫码框的提交事件或者回车事件
        /// </summary>
        public void GoScanCode()
        {

            string biz_row_code = textBox12.Value;

            if (string.IsNullOrWhiteSpace(biz_row_code))
            {
                MessageBox.Alert("扫码框里面的值不能为空！");
                return;
            }

            int row_id = WebUtil.QueryInt("row_id");


            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_002");
            lmFilter.And("ROW_SID",0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_ROW_CODE", biz_row_code);

            LModel lm002 = decipher.GetModel(lmFilter);


            LModel lm001 = decipher.GetModelByPk("UT_001", row_id);

            LModel lmNew002 = new LModel("UT_002");

            lm002.CopyTo(lmNew002);
            lmNew002["ROW_DATE_CREATE"] = lmNew002["ROW_DATE_UPDATE"] = DateTime.Now;

            lmNew002["COL_1"] = row_id;
            lmNew002["P_BILL_CODE"] = "";
            lmNew002["P_CLIENT_TEXT"] = "";

            lmNew002["IO_TAG"] = "O";


            lmNew002["SRC_PROD_ITEM_CODE"] = lm002["BIZ_ROW_CODE"];
            lmNew002["SRC_PROD_ITEM_ID"] = lm002["ROW_IDENTITY_ID"];


            lmNew002["BIZ_ROW_CODE"] = string.Empty;

            decipher.InsertModel(lmNew002);

            

            storeProd1.Refresh();


        }


        private void UpdateTotal(string bizRowCode)
        {
            if (StringUtil.IsBlank(bizRowCode))
            {
                return;
            }

            LightModelFilter filter = new LightModelFilter("UT_002");
            filter.AddFilter("ROW_SID >=0 ");
            filter.And("BIZ_ROW_CODE", bizRowCode);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<LModel> prods = decipher.GetModelList(filter);

            foreach (LModel prod in prods)
            {

                LModel total = GetTotal(prod.Get<string>("BIZ_ROW_CODE"));

                prod["OUT_NUM"] = total["TOTAL_NUM"];   //出库数量
                prod["OUT_WEIGHT"] = total["TOTAL_WEIGHT"];   //出库重量

                prod["SURPLUS_NUM"] = prod.Get<decimal>("COL_21") - prod.Get<decimal>("OUT_NUM");   //数量

                prod["SURPLUS_WEIGHT"] = prod.Get<decimal>("COL_23") - prod.Get<decimal>("OUT_WEIGHT"); //重量

                decipher.UpdateModelProps(prod, "OUT_NUM", "OUT_WEIGHT", "SURPLUS_NUM", "SURPLUS_WEIGHT");
            }
        }




    }
}