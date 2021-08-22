using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5;
using EC5.AppDomainPlugin;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace App.InfoGrid2.View.Biz.Rosin2.Plan
{
    public partial class PlanList : WidgetControl, IView
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

            if (!this.IsPostBack)
            {
                OnInitData();
            }

           
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.store1.Inserted += Store1_Inserted;
            this.table1.Command += Table1_Command;
            triggerBox1.ButtonClickCallback += TriggerBox1_ButtonClickCallback;

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }

        public void TriggerBox1_ButtonClickCallback(object sender, string data)
        {

            var urlStr = $"/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={"VIEW"}&viewId={17}";

            Window win = new Window("选择", 800, 600);

            win.ContentPath = urlStr;
            win.StartPosition = WindowStartPosition.CenterScreen;

            win.WindowClosed += Win_WindowClosed2; ;

            win.ShowDialog();


        }

        public void Win_WindowClosed2(object sender, string data)
        {

            DynSModel e = DynSModel.ParseJson(data);

            if (e["result"] != "ok")
            {
                return;
            }

            SModel row = e["row"] as SModel;

            triggerBox1.Value = row.Get<string>("CLIENT_TEXT");

        }

        private void Table1_Command(object sender, TableCommandEventArgs e)
        {
            if(e.CommandName == "GoEdit")
            {
                DataRecord record = e.Record;

                object pk = record.Id;

                string ioTag = WebUtil.Query("io_tag");

                Window win = new Window((ioTag == "I" ? "入库" : "出库") + "计划");
                win.ContentPath = $"/App/InfoGrid2/View/Biz/Rosin2/Plan/PlanForm.aspx?io_tag={ioTag}&row_id={pk}";
                win.State = WindowState.Max;
                win.WindowClosed += Win_WindowClosed;
                win.ShowDialog();

            }
        }

        private void Store1_Inserted(object sender, ObjectEventArgs e)
        {
            LModel model = e.Object as LModel;

            object pk = model.GetPk();

            string ioTag = WebUtil.Query("io_tag");

            H_ID.Value = pk.ToString();

            Window win = new Window((ioTag=="I"?"入库":"出库") + "计划");
            win.ContentPath = $"/App/InfoGrid2/View/Biz/Rosin2/Plan/PlanForm.aspx?io_tag={ioTag}&row_id={pk}";
            win.State = WindowState.Max;


            win.WindowClosed += Win_WindowClosed;
            win.ShowDialog();


        }

        public void Win_WindowClosed(object sender, string data)
        {

            string pk = H_ID.Value;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm016 = decipher.GetModelByPk("UT_015_PLAN", pk);

            if(lm016 == null)
            {
                this.store1.Refresh();
                return;
            }


            //如果数据没有改变过，就自动给他删除掉
            if (lm016.Get<DateTime>("ROW_DATE_CREATE") == lm016.Get<DateTime>("ROW_DATE_UPDATE"))
            {
                lm016["ROW_SID"] = -3;
                lm016["ROW_DATE_DELETE"] = DateTime.Now;

                decipher.UpdateModelProps(lm016, "ROW_SID", "ROW_DATE_DELETE");
            }

            this.store1.Refresh();

        }

        private void OnInitData()
        {
            string io_tag = WebUtil.QueryUpper("IO_TAG");

            string title = io_tag == "I" ? "入库计划管理" : "出库计划管理";

            tbb_create_ut_001.Visible = (io_tag == "I");

            this.headLab.Value = "<span class='page-head' >" + title + "</span>";


        }


        /// <summary>
        /// 回收站
        /// </summary>
        public void GoRecycled()
        {
            string io_tag =  WebUtil.QueryUpper("io_tag");

            EasyClick.Web.Mini.EcView.show("PlanListBizF3.aspx?io_tag=" + io_tag, "回收站");
        }


        /// <summary>
        /// 导入 Excel
        /// </summary>
        public void GoFromExcel()
        {
            Window win = new Window("上传文件", 600, 400);

            win.ContentPath = "/App/InfoGrid2/View/Biz/Rosin2/Plan/FileUpdateForm.aspx";

            win.StartPosition = WindowStartPosition.CenterScreen;
            win.WindowClosed += Win_WindowClosed1;

            win.ShowDialog();
        }

        public void Win_WindowClosed1(object sender, string data)
        {
            SModel json = SModel.ParseJson(data);

            DynSModel ds = new DynSModel(json);

            if(ds["result"] != "ok")
            {
                return;
            }

            string relPath = ds["path"];

            WebFileInfo wFile = new WebFileInfo(relPath);

            List<LModel> models = null;

            try
            {
                models = Excel_Template.ImportExcelUtil.CreateLModels("UT_015_PLAN", wFile.PhysicalPath);
            }
            catch(Exception ex)
            {
                log.Error("导入失败, 转换失败.", ex);

                MessageBox.Alert("导入失败, 转换失败.");

                return;
            }

            if (models.Count == 0)
            {
                Toast.Show("这个文件没有记录, 请检查后再操作.");
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.BeginTransaction();

            try
            {
                
                foreach (LModel model in models)
                {
                    model["IO_TAG"] = "I";
                    model["ROW_DATE_CREATE"] = model["ROW_DATE_UPDATE"] =  model["DOC_CREATE_DATE"] = DateTime.Now;
                    decipher.InsertModel(model);
                }

                decipher.TransactionCommit();

                this.store1.Refresh();

                Toast.Show($"共导入 {models.Count} 条记录!");
            }
            catch(Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("导入失败: " + relPath, ex);
                MessageBox.Alert("导入失败:" + ex.Message);
            }

        }


        private void ChangeBizSID(int start, int end)
        {
            ChangeBizSID(null, null, start, end);
        }

        private void ChangeBizSID(LModel model, DataRecord record, int start, int end)
        {
            PlanMgr.ChangeBizSID(model, this.store1, record, start, end);
        }

        private void ToStoreValue(Store store, DataRecord record, LModel model)
        {

            if (record != null && store != null)
            {
                string[] cFields = model.GetBlemishFields();

                foreach (string cField in cFields)
                {
                    store.SetRecordValue(record.Id, cField, model[cField]);
                }
            }
        }


        public void GoChangeBizSID_0_2()
        {
            string io_tag = WebUtil.QueryUpper("IO_TAG");


            DataRecordCollection records = this.table1.CheckedRows;


            DbDecipher decipher = ModelAction.OpenDecipher();


            foreach (DataRecord record in records)
            {

                LModel model = decipher.SelectModelByPk("UT_015_PLAN", record.Id) as LModel;

                DynLModel dynModel = new DynLModel(model);

                if (dynModel["BIZ_SID"] != 0)
                {
                    Toast.Show("‘提交’失败");
                    return;
                }

                model.SetTakeChange(true);

                if (StringUtil.IsBlank(model["BILL_NO"]))
                {
                    string newBillNo = BillIdentityMgr.NewCodeForDay("BILL_PLAN_" + io_tag, io_tag, 3);


                    model["BILL_NO"] = newBillNo;
                    model["DOC_CREATE_USER_TEXT"] = BizServer.LoginName;
                    model["DOC_CREATE_DATE"] = DateTime.Now;

                    decipher.UpdateModel(model, true);
                }
                else
                {
                    model["DOC_UPDATE_USER_TEXT"] = BizServer.LoginName;
                    model["DOC_UPDATE_DATE"] = DateTime.Now;
                }

                ToStoreValue(this.store1, record, model);


                try
                {
                    ChangeBizSID(model, record, 0, 2);



                    Toast.Show("提交成功");
                }
                catch (Exception ex)
                {
                    log.Error("提交失败", ex);
                    MessageBox.Alert("提交失败!");

                    break;
                }
            }

        }


        /// <summary>
        /// 直接创建入库单, 并产生条形码
        /// </summary>
        public void GoCreateInBill()
        {
            string io_tag = WebUtil.QueryUpper("IO_TAG");


            DataRecordCollection records = this.table1.CheckedRows;


            DbDecipher decipher = ModelAction.OpenDecipher();

            int n = 0;

            Exception exception = null;


            foreach (DataRecord record in records)
            {

                LModel model = decipher.SelectModelByPk("UT_015_PLAN", record.Id) as LModel;

                DynLModel dynModel = new DynLModel(model);

                if (dynModel["BIZ_SID"] != 2)
                {
                    Toast.Show("‘审核’失败");
                    return;
                }

                model.SetTakeChange(true);
                
                model["DOC_CHECK_USER_TEXT"] = BizServer.LoginName;
                model["DOC_CHECK_DATE"] = DateTime.Now;
                

                ToStoreValue(this.store1, record, model);


                try
                {
                    decipher.BeginTransaction();

                    ChangeBizSID(model, record, 2, 4);
                    
                    string planBillNo = record["BILL_NO"].ToString();

                    try
                    {
                        bool created = CreateInBill(planBillNo);

                        if (created)
                        {
                            n++;
                        }

                        decipher.TransactionCommit();
                    }
                    catch (Exception ex)
                    {
                        decipher.TransactionRollback();

                        exception = ex;
                        break;
                    }

                    Toast.Show("提交成功");
                }
                catch (Exception ex)
                {
                    log.Error("提交失败", ex);
                    MessageBox.Alert("提交失败!");

                    break;
                }
            }


            

            if(n > 0)
            {
                Toast.Show($"共产生 {n} 个入库单。");
            }

            if(exception != null)
            {
                MessageBox.Alert(exception.Message);
            }
            

        }

        /// <summary>
        /// 产生入口单
        /// </summary>
        /// <param name="planBillNo">计划单号</param>
        private bool CreateInBill(string planBillNo)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_016");
            filter.AddFilter("ROW_SID >= 0");
            filter.And("PLAN_BILL_NO", planBillNo);

            bool exist = decipher.ExistsModels(filter);

            if (exist)
            {
                return false;
            }

            LightModelFilter f2 = new LightModelFilter("UT_015_PLAN");
            f2.AddFilter("ROW_SID >= 0");
            f2.AddFilter("BIZ_SID >= 4");
            f2.And("BILL_NO", planBillNo);

            LModel planModel = decipher.GetModel(f2);

            if(planModel == null)
            {
                throw new Exception($"计划单'{planBillNo}'未审核，无法产生入库单。");
            }

            LModel model = new LModel("UT_016");
            planModel.CopyTo(model, true);

            string newBillNo = BillIdentityMgr.NewCodeForDay("BILL_" + "I", "I", 3);

            model["PLAN_BILL_NO"] = planBillNo;
            model["BILL_NO"] = newBillNo;

            model["DOC_CREATE_USER_TEXT"] = BizServer.LoginName;
            model["DOC_CREATE_DATE"] = DateTime.Now;

            model["BIZ_UPDATE_USER_CODE"] = string.Empty;
            model["BIZ_DELETE_USER_CODE"] = string.Empty;

            model["DOC_CHECK_USER_TEXT"] = string.Empty;
            model["DOC_CHECK_DATE"] = null;
            model["DOC_UPDATE_USER_TEXT"] = string.Empty;
            model["DOC_UPDATE_DATE"] = null;

            model["BIZ_SID"] = 4;

            model["ROW_DATE_CREATE"] = model["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.InsertModel(model);

            int prodCount = model.Get<int>("PROD_NUM");

            

            for (int i = 0; i < prodCount; i++)
            {
                int newProdId = i + 1;

                LModel pm = new LModel("UT_017_PROD");

                pm["BIZ_SID"] = 4;

                pm["DOC_PARENT_ID"] = model.GetPk();
                pm["IN_BILL_NO"] = model["BILL_NO"];
                pm["PROD_CODE"] = string.Format("{0}-{1}", newBillNo, newProdId);

                pm["IO_TAG"] = "I";
                pm["OUT_SID"] = 0;   //未出库

                pm["ROW_DATE_CREATE"] = pm["ROW_DATE_UPDATE"] = DateTime.Now;

                pm["CLIENT_CODE"] = model["CLIENT_CODE"];
                pm["CLIENT_TEXT"] = model["CLIENT_TEXT"];

                pm["S_PROD_CODE"] = model["S_PROD_CODE"];
                pm["S_PROD_TEXT"] = model["S_PROD_TEXT"];

                decipher.InsertModel(pm);
            }

            return true;
        }
    }
}