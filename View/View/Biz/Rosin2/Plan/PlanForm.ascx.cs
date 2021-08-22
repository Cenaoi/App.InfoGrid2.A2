using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.AppDomainPlugin;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace App.InfoGrid2.View.Biz.Rosin2.Plan
{
    public partial class PlanForm : WidgetControl, IView
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

        protected override void OnLoad(EventArgs e)
        {

            this.triggerBox1.ButtonClickCallback += TriggerBox1_ButtonClickCallback;
            this.triggerBox2.ButtonClickCallback += TriggerBox2_ButtonClickCallback;

            if (!this.IsPostBack)
            {
                BizHelper.Full(cb_store, "UT_010", "COL_1", "COL_1");    //仓库信息填充到下拉框

                this.storeMain1.DataBind();
                storeProd1.DataBind();
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

            if (e["result"] != "ok")
            {
                return;
            }

            SModel row = e["row"] as SModel;

            DataRecord record = this.storeMain1.GetDataCurrent();


            foreach (MapItem map in this.triggerBox1.MapItems)
            {
                string value = Convert.ToString(row[map.SrcField]);

                this.storeMain1.SetRecordValue(true, record.Id, map.TargetField, value);
            }

        }


        protected void TriggerBox2_ButtonClickCallback(object sender, string data)
        {
            var urlStr = $"/App/InfoGrid2/view/OneSearch/SelectPreview.aspx?type={"VIEW"}&viewId={39}";

            Window win = new Window("选择", 800, 600);

            win.ContentPath = urlStr;
            win.StartPosition = WindowStartPosition.CenterScreen;

            win.WindowClosed += Win_WindowClosed2;

            win.ShowDialog();
        }


        protected void Win_WindowClosed2(object sender, string data)
        {
            DynSModel e = DynSModel.ParseJson(data);

            if (e["result"] != "ok")
            {
                return;
            }

            SModel row = e["row"] as SModel;

            DataRecord record = this.storeMain1.GetDataCurrent();


            foreach (MapItem map in this.triggerBox2.MapItems)
            {
                string value = Convert.ToString(row[map.SrcField]);

                this.storeMain1.SetRecordValue(true, record.Id, map.TargetField, value);
            }

        }



        private void OnInitData()
        {
            string io_tag = WebUtil.QueryUpper("IO_TAG");

            string title = io_tag == "I" ? "入库计划单" : "出库计划单";

            this.headLab.Value = "<span class='page-head' >" + title + "</span>";


            //只有出库计划才用到这个表格
            panel2.Visible = io_tag == "O";


            LModel model = this.storeMain1.GetFirstData() as LModel;

            

            if (model != null)
            {
                if(model.Get<int>("BIZ_SID") > 0)
                {
                    prodToolbar1.Visible = false;
                }

                BizSid_Changed(model.Get<int>("BIZ_SID"));
            }

            if(io_tag == "O")
            {
                COL_21_nb.ReadOnly = true;
                textBox1.FieldLabel = "出库计划单号";

                DbDecipher decipher = ModelAction.OpenDecipher();


                SModelList smList = decipher.GetSModelList("select * from UT_012 where ROW_SID >= 0 and V_TEXT <> ''");

                SelectColumn selectColumn = table1.Columns.FindByDataField("CHECK_CODE") as SelectColumn;


                foreach (SModel sm in smList)
                {
                    selectColumn.Items.Add(sm.Get<string>("V_TEXT"), sm.Get<string>("V_TEXT"));

                }


            }
            else
            {
                textBox1.FieldLabel = "入库计划单号";
            }
        }


        /// <summary>
        /// 删除子表数据
        /// </summary>
        void DeleteSubData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();


            List<LModel> lm020s = storeProd1.GetList() as List<LModel>;


            foreach (LModel lm020 in lm020s)
            {

                lm020.SetTakeChange(true);

                lm020["ROW_SID"] = -3;
                lm020["ROW_DATE_DELETE"] = DateTime.Now;

            }


            decipher.BeginTransaction();

            try
            {
                decipher.UpdateModels(lm020s, true);

                decipher.TransactionCommit();


            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();
                throw new Exception("删除子表数据出错了！", ex);
            }


        }


        private bool ChangeBizSID(int start,int end)
        {
            return ChangeBizSID(null,null, start, end);
        }

        private bool ChangeBizSID(LModel model, DataRecord record, int start,int end)
        {
            if (record == null)
            {
                record = this.storeMain1.GetDataCurrent();
            }

            if (model == null)
            {
                
                DbDecipher decipher = ModelAction.OpenDecipher();

                model = this.storeMain1.GetFirstData() as LModel;
            }

           return  PlanMgr.ChangeBizSID(model, this.storeMain1, record, start, end);
        }

        private void BizSid_Changed(int bizSid)
        {
            bool readOnly = (bizSid > 0);

            foreach (var item in HanderForm1.Controls)
            {
                if(item is FieldBase)
                {
                    FieldBase fb = (FieldBase)item;

                    if (fb.DataField == "DOC_UPDATE_USER_TEXT" || fb.DataField == "DOC_UPDATE_DATE")
                    {
                        continue;
                    }

                    fb.ReadOnly = readOnly;
                }
            }

            foreach (var item in formLayout2.Controls)
            {
                if (item is FieldBase)
                {
                    FieldBase fb = (FieldBase)item;

                    if (fb.DataField == "DOC_UPDATE_USER_TEXT" || fb.DataField == "DOC_UPDATE_DATE")
                    {
                        continue;
                    }

                    fb.ReadOnly = readOnly;
                }
            }

        }

        /// <summary>
        /// 更新界面
        /// </summary>
        /// <param name="store"></param>
        /// <param name="record"></param>
        /// <param name="model"></param>
        private void UpdateStore(Store store, DataRecord record, LModel model)
        {
            if (record == null || store == null)
            {
                return;
            }

            string[] cFields = model.GetBlemishFields();

            if (cFields != null && cFields.Length > 0)
            {
                foreach (string cField in cFields)
                {
                    store.SetRecordValue(record.Id, cField, model[cField]);
                }
            }
        }

        /// <summary>
        /// 获取货物数量
        /// </summary>
        /// <returns></returns>
        private int GetProdCount(int billPk)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter countFilter = new LightModelFilter("UT_020");
            //countFilter.AddFilter("BIZ_SID >= 0");
            countFilter.AddFilter("ROW_SID >= 0");
            countFilter.And("DOC_PARENT_ID", billPk);

            int prodCount = decipher.SelectCount(countFilter);

            return prodCount;
        }


        /// <summary>
        /// 提交
        /// </summary>
        public void GoChangeBizSID_0_2()
        {
            string io_tag = WebUtil.QueryUpper("IO_TAG");

            DataRecord record = this.storeMain1.GetDataCurrent();

            if(record == null)
            {
                MessageBox.Alert("记录无效.");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = this.storeMain1.GetFirstData() as LModel;

            DynLModel dynModel = new DynLModel(model);

            if(dynModel["BIZ_SID"] != 0)
            {
                Toast.Show("‘提交’失败");
                return;
            }

            model.SetTakeChange(true);

            #region 验证数据

            bool isValid = true;

            if(StringUtil.IsBlank(dynModel["CLIENT_CODE"]))
            {
                this.storeMain1.MarkInvalid(record,"error","CLIENT_CODE", "必填");
                isValid = false;
            }

            if (StringUtil.IsBlank(dynModel["CLIENT_TEXT"]))
            {
                this.storeMain1.MarkInvalid(record,"error","CLIENT_TEXT", "必填");
                isValid = false;
            }

            if (io_tag == "I")
            {
                if (dynModel["PROD_NUM"] <= 0)
                {
                    this.storeMain1.MarkInvalid(record, "error", "PROD_NUM", "不能小于 0");
                    isValid = false;
                }
            }

            if (!isValid)
            {
                return;
            }

            #endregion


            if (io_tag == "O")
            {

                int prodCount = GetProdCount((int)model.GetPk());


                if (prodCount <= 0)
                {
                    Toast.Show("没有货物,不能提交.");
                    return;
                }

                dynModel["PROD_NUM"] = prodCount;
            }
            else
            {
                //入库计划,不用算数量
                //int prodCount = GetProdCount((int)model.GetPk());
                //dynModel["PROD_NUM"] = prodCount;
            }
            

            if (StringUtil.IsBlank(model["BILL_NO"]))
            {
                string newBillNo = BillIdentityMgr.NewCodeForDay("BILL_PLAN_" + io_tag , io_tag, 3);
                
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


            
            UpdateStore(this.storeMain1, record, model);


            try
            {
                ChangeBizSID(0, 2);

                BizSid_Changed(2);

                UpdataUT_020(2);


                #region 隐藏工具栏

                prodToolbar1.Visible = false;


                #endregion

                Toast.Show("提交成功");
            }
            catch(Exception ex)
            {
                log.Error("提交失败", ex);
                MessageBox.Alert("提交失败!");
            }
        }

        /// <summary>
        /// 改变UT_020的锁状态
        /// </summary>
        /// <param name="lock_sid">0--不锁，2--上锁</param>
        void UpdataUT_020(int lock_sid)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel mainModel = this.storeMain1.GetFirstData() as LModel;


            //LightModelFilter lmFilter020 = new LightModelFilter("UT_020");
            //lmFilter020.AddFilter("ROW_SID >= 0");
            //lmFilter020.And("DOC_PARENT_ID", mainModel.GetPk());

            //decipher.UpdateProps(lmFilter020, new object[] { "LOCK_SID", lock_sid, "ROW_DATE_UPDATE", DateTime.Now});

            decipher.UpdateProps("UT_020", $"ROW_SID >=0 AND DOC_PARENT_ID={ mainModel.GetPk()}", new object[] {
                "LOCK_SID", lock_sid,
                "ROW_DATE_UPDATE", DateTime.Now
            });

            storeProd1.Refresh();

        }


        /// <summary>
        /// 审核
        /// </summary>
        public void GoChangeBizSID_2_4()
        {
            string io_tag = WebUtil.QueryUpper("IO_TAG");


            DataRecord record = this.storeMain1.GetDataCurrent();

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = this.storeMain1.GetFirstData() as LModel;

            DynLModel dynModel = new DynLModel(model);

            if (dynModel["BIZ_SID"] != 2)
            {
                Toast.Show("‘审核’失败");
                return;
            }


            model.SetTakeChange(true);
                
            model["DOC_CHECK_USER_TEXT"] = BizServer.LoginName;
            model["DOC_CHECK_DATE"] = DateTime.Now;




            decipher.UpdateModel(model, true);
            
            
            UpdateStore(this.storeMain1, record, model);



            decipher.BeginTransaction();

            try
            {
                ChangeBizSID(2, 4);

                if (io_tag == "I")
                {

                    CreateInBill((string)model["BILL_NO"]);
                }
                else
                {


                    UpdataUT_020(0);

                    CreateOutOrder();
                }

                decipher.TransactionCommit();

                BizSid_Changed(2);

 
                Toast.Show("审核成功");
            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("审核失败", ex);
                MessageBox.Alert("审核失败!");
            }
        }


        /// <summary>
        /// 创建一份出库单
        /// </summary>
        void CreateOutOrder()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            int row_id = WebUtil.QueryInt("row_id");

            LModel lm015 = decipher.GetModelByPk("UT_015_PLAN", row_id);

            //出库主表
            LModel lm016 = new LModel("UT_016");
            lm015.CopyTo(lm016);

            lm016["BIZ_SID"] = 4;
            lm016["F_NUM"] = -lm016.Get<decimal>("PROD_NUM");
            lm016["ROW_DATE_CREATE"] = lm016["ROW_DATE_UPDATE"] = DateTime.Now;


            lm016["DOC_CREATE_USER_TEXT"] = BizServer.LoginName;
            lm016["DOC_CREATE_DATE"] = DateTime.Now;

            lm016["DOC_CHECK_USER_TEXT"] = BizServer.LoginName;
            lm016["DOC_CHECK_DATE"] = DateTime.Now;



            string bill_no = BillIdentityMgr.NewCodeForDay("BILL_TO", "O", 3);

            lm016["BILL_NO"] = bill_no;

            lm016["PLAN_BILL_NO"] = lm015["BILL_NO"];

            decipher.InsertModel(lm016);


            LightModelFilter lmFilter020 = new LightModelFilter("UT_020");
            lmFilter020.AddFilter("ROW_SID >= 0");
            lmFilter020.And("DOC_PARENT_ID", lm015.GetPk());




            //decipher.SelectChildModels(lm015, "id", "DOC_PARENT_ID");

            List<LModel> lm020s = decipher.GetModelList(lmFilter020);

            List<LModel> lm017s = new List<LModel>();


            foreach(LModel lm020 in lm020s)
            {

                LModel lm017 = new LModel("UT_017_PROD");

                lm020.CopyTo(lm017);

                lm017["ROW_DATE_CREATE"] = lm017["ROW_DATE_UPDATE"] = DateTime.Now;

                lm017["DOC_PARENT_ID"] = lm016.GetPk();

                lm017s.Add(lm017);

                decipher.InsertModel(lm017);

            }


            foreach(LModel lm017 in lm017s)
            {

                //条码
                string prod_code = lm017.Get<string>("PROD_CODE");

                UpdateInData(prod_code);

            }



        }


        /// <summary>
        /// 更新入库单的数据 根据条码
        /// </summary>
        /// <param name="qr_code">条码</param>
        void UpdateInData(string qr_code)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter017 = new LightModelFilter("UT_017_PROD");
            lmFilter017.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter017.And("PROD_CODE", qr_code);
            lmFilter017.And("OUT_SID", 0);
            lmFilter017.And("IO_TAG", "I");
            lmFilter017.Top = 1;


            LModel lm017 = decipher.GetModel(lmFilter017);

            if (lm017 == null)
            {
                throw new Exception("找不到UT_017的数据");
            }

            lm017["OUT_SID"] = 2;
            lm017["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.UpdateModelProps(lm017, "OUT_SID", "ROW_DATE_UPDATE");


            LModel lm016 = decipher.GetModelByPk("UT_016", lm017["DOC_PARENT_ID"]);


            LightModelFilter lmFilterNew017 = new LightModelFilter("UT_017_PROD");
            lmFilterNew017.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterNew017.And("DOC_PARENT_ID", lm016.GetPk());


            List<LModel> lmNew017s = decipher.GetModelList(lmFilterNew017);

            //进数量
            decimal i_num = 0;
            //出数量
            decimal o_num = 0;


            foreach (LModel lmNew017 in lmNew017s)
            {
                DynLModel mm = new DynLModel(lmNew017);

                if (mm["OUT_SID"] == 2)
                {

                    o_num++;

                    continue;
                }

                i_num++;

            }

            lm016.SetTakeChange(true);


            lm016["SURPLUS_NUM"] = i_num;

            lm016["SURPLUS_WEIGHT"] = 0;
            lm016["OUT_NUM"] = o_num;
            lm016["OUT_WEIGHT"] = 0;


            decipher.UpdateModel(lm016, true);

            





        }



        /// <summary>
        /// 从提交->草稿
        /// </summary>
        public void GoChangeBizSID_2_0()
        {
            try
            {

                LModel lm015 = storeMain1.GetFirstData() as LModel;

                int biz_sid = lm015.Get<int>("BIZ_SID");

                //这有这两个才能执行撤销动作
                if(biz_sid != 2 && biz_sid != 1)
                {
                    Toast.Show("撤销失败！");

                    return;
                }

                bool flag = ChangeBizSID(biz_sid, 0);

                if (!flag)
                {
                    Toast.Show("撤销失败了！");
                    return;
                }

                BizSid_Changed(0);

                Toast.Show("撤销成功");
            }
            catch (Exception ex)
            {
                log.Error("审核失败", ex);
                MessageBox.Alert("审核失败!");
            }
        }

        /// <summary>
        /// 直接作废
        /// </summary>
        public void GoChangeBizSID_4_F3()
        {
            try
            {
               bool flag = ChangeBizSID(0, -3);


                if (flag)
                {

                    Toast.Show("‘作废操作’成功");

                    DeleteSubData();
                }
                else
                {
                    Toast.Show("'作废操作'失败了！");
                }
            }
            catch (Exception ex)
            {
                log.Error("作废操作失败", ex);
                MessageBox.Alert("审核失败!");
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

            if (planModel == null)
            {
                throw new Exception($"计划单'{planBillNo}'未审核，无法产生入库单。");
            }

            LModel model = new LModel("UT_016");
            planModel.CopyTo(model, true);

            string newBillNo = BillIdentityMgr.NewCodeForDay("BILL_" + "I", "I", 3);

            model["PLAN_BILL_NO"] = planBillNo;
            model["BILL_NO"] = newBillNo;

            model["F_NUM"] = model["PROD_NUM"]; //F_NUM 特殊字段


            model["DOC_CREATE_USER_TEXT"] = BizServer.LoginName;
            model["DOC_CREATE_DATE"] = DateTime.Now;

            model["BIZ_UPDATE_USER_CODE"] = string.Empty;
            model["BIZ_DELETE_USER_CODE"] = string.Empty;

            model["DOC_CHECK_USER_TEXT"] = BizServer.LoginName;
            model["DOC_CHECK_DATE"] = DateTime.Now;
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


        /// <summary>
        /// 出库啥搜毛
        /// </summary>
        public void GoOutWin()
        {
            int row_id = WebUtil.QueryInt("row_id");

            DbDecipher decipher = ModelAction.OpenDecipher();
            LModel model = decipher.GetModelByPk("UT_015_PLAN", row_id);

            if (StringUtil.IsBlank(model["CLIENT_CODE"]))
            {
                MessageBox.Alert("请先选中客户编码");
                return;
            }


            TextWindow tw = new TextWindow("扫码出库");
            tw.StartPosition = WindowStartPosition.CenterScreen;
            tw.WindowClosed += Tw_WindowClosed;
            tw.ContentFontSize = "24px";
            tw.ShowDialog();
        }

        protected void Tw_WindowClosed(object sender, string data)
        {
            SModel json = SModel.ParseJson(data);

            DynSModel e = new DynSModel(json);

            if (e["result"] != "ok")
            {
                return;
            }

            string io_tag = WebUtil.QueryUpper("io_tag");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel mainModel = this.storeMain1.GetFirstData() as LModel;

            string[] barCodeList = StringUtil.Split(e["value"], "\n", "\r\n");

            StringBuilder sb = new StringBuilder();

            foreach (string barCode in barCodeList)
            {
                LModel modelIn = GetProdForIn(barCode); //在场的货物

                if (modelIn == null)
                {
                    Toast.Show($"这个 {barCode} 条码无效.");
                    continue;
                }

                LightModelFilter filter = new LightModelFilter("UT_020");
                filter.AddFilter("ROW_SID >= 0");
                filter.And("DOC_PARENT_ID", mainModel.GetPk());
                filter.And("PROD_CODE", barCode);
                filter.And("IO_TAG", "O");

                if (decipher.ExistsModels(filter))
                {
                    Toast.Show($"此 {barCode} 已经存在记录中,重复.");
                    continue;
                }


                LModel model = new LModel("UT_020");
                modelIn.CopyTo(model,true);

                model["ROW_SID"] = 0;
                model["DOC_PARENT_ID"] = mainModel.GetPk();
                model["IO_TAG"] = "O";
                model["OUT_SID"] = 0;



                decipher.InsertModel(model);

                this.storeProd1.Add(model);

                Toast.Show($"添加 {barCode} 成功");
            }


            int prodCount = GetProdCount((int)mainModel.GetPk());
            
            LModel u015_PLAN = mainModel;// decipher.GetModelByPk("UT_015_PLAN",row_id); int prodCount = GetProdCount(row_id);

            u015_PLAN["PROD_NUM"] = prodCount;


            decipher.UpdateModelProps(u015_PLAN, "PROD_NUM");

            this.storeMain1.SetRecordValue(mainModel.GetPk(), "PROD_NUM", prodCount);

        }


        /// <summary>
        /// 获取入口单的条码
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        private LModel GetProdForIn(string barCode)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_017_PROD");
            filter.AddFilter("ROW_SID >= 0");
            filter.And("IO_TAG", "I");
            filter.AddFilter("OUT_SID = 0");
            filter.And("PROD_CODE", barCode);

            LModel model = decipher.GetModel(filter);

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        public void GoDeleteProd()
        {
            int row_id = WebUtil.QueryInt("row_id");
            string io_tag = WebUtil.QueryUpper("IO_TAG");

            if (io_tag != "O")
            {
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModelByPk("UT_015_PLAN", row_id);

            if (model.Get<int>("BIZ_SID") > 0)
            {
                MessageBox.Alert("此订单已经提交或审核，无法删除.");
                return;
            }


            foreach (DataRecord record in this.table1.CheckedRows)
            {
                LModel pp = decipher.GetModelByPk("UT_020", record.Id);
                pp.SetTakeChange(true);

                pp["ROW_SID"] = -3;
                pp["ROW_DATE_DELETE"] = DateTime.Now;

                decipher.UpdateModel(pp, true);

                this.storeProd1.RemoveByClientId(record.ClientId);
            }


            LModel u015_PLAN = decipher.GetModelByPk("UT_015_PLAN",row_id);

            int prodCount = GetProdCount(row_id);
            
            u015_PLAN["PROD_NUM"] = prodCount;
            

            decipher.UpdateModelProps(u015_PLAN, "PROD_NUM");

            this.storeMain1.SetRecordValue(row_id, "PROD_NUM", prodCount);

        }

        /// <summary>
        /// 从入库单选择按钮点击事件
        /// </summary>
        public void GoShowInOrder()
        {

            Window win = new Window("入库单货物数据");


            LModel lm = storeMain1.GetFirstData() as LModel;

            string client_code = lm.Get<string>("CLIENT_CODE");

            string client_text = lm.Get<string>("CLIENT_TEXT");

            win.ContentPath = $"/App/InfoGrid2/View/Biz/Rosin2/Actual/SelectInOrder.aspx?client_code={client_code}&client_text={client_text}";
            win.State = WindowState.Max;
            win.WindowClosed += Win_WindowClosed1;

            win.ShowDialog();


        }

        public void Win_WindowClosed1(object sender, string data)
        {


            int row_id = WebUtil.QueryInt("row_id");


            if (string.IsNullOrWhiteSpace(data))
            {
                return;
            }

            SModel json = SModel.ParseJson(data);

            DynSModel e = new DynSModel(json);

            if (e["result"] != "ok")
            {
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            string[] ids = StringUtil.Split(e["ids"]);


            List<LModel> lm017s = new List<LModel>();

            List<LModel> lm020s = new List<LModel>();

            foreach (string id in ids)
            {

                LModel lm017 = decipher.GetModelByPk("UT_017_PROD", id);

                LightModelFilter filter = new LightModelFilter("UT_017_PROD");
                filter.AddFilter("ROW_SID >= 0");
                filter.And("DOC_PARENT_ID", row_id);
                filter.And("PROD_CODE", lm017["PROD_CODE"]);
                filter.And("IO_TAG", "O");

                if (decipher.ExistsModels(filter))
                {
                    Toast.Show($"此 【{lm017["PROD_CODE"]}】 已经存在记录中,重复.");
                    return;
                }

                lm017s.Add(lm017);
            }


            foreach (LModel lm017 in lm017s)
            {

                LModel lmNew020 = new LModel("UT_020");

                lm017.CopyTo(lmNew020,true);

                lmNew020["IO_TAG"] = "O";
                lmNew020["BIZ_SID"] = 0;
                lmNew020["ROW_DATE_CREATE"] = lmNew020["ROW_DATE_UPDATE"] = DateTime.Now;
                lmNew020["LOCK_SID"] = 0;
                lmNew020["DOC_PARENT_ID"] = row_id;

                lm020s.Add(lmNew020);

            }

            LModel u015_PLAN = decipher.GetModelByPk("UT_015_PLAN",row_id);


            decipher.BeginTransaction();

            try
            {
                decipher.InsertModels<LModel>(lm020s);


                int prodCount = GetProdCount(row_id);

                u015_PLAN["PROD_NUM"] = prodCount;

                decipher.UpdateModelProps(u015_PLAN, "PROD_NUM");

                

                decipher.TransactionCommit();

                this.storeMain1.SetRecordValue(row_id, "PROD_NUM", prodCount);
                storeProd1.Refresh();
            }
            catch (Exception ex)
            {

                decipher.TransactionRollback();
                log.Error(ex);

                MessageBox.Alert("哦噢，有问题了！");


            }

        }


        /// <summary>
        /// 打回按钮
        /// </summary>
        public void GoChangeBizSID_2_1()
        {

            try
            {
                ChangeBizSID(2, 1);

                BizSid_Changed(1);

                Toast.Show("打回成功");
            }
            catch (Exception ex)
            {
                log.Error("打回失败", ex);
                MessageBox.Alert("打回失败!");
            }


        }


    }
}