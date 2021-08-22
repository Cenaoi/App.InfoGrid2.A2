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
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace App.InfoGrid2.View.Biz.Rosin2.Actual
{
    public partial class ActualForm : WidgetControl, IView
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

                this.storeProd1.DataBind();
            }
        }


        /// <summary>
        /// 获取货物数量
        /// </summary>
        /// <returns></returns>
        private int GetProdCount(int billPk)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter countFilter = new LightModelFilter("UT_017_PROD");
            //countFilter.AddFilter("BIZ_SID >= 0");
            countFilter.AddFilter("ROW_SID >= 0");
            countFilter.And("DOC_PARENT_ID", billPk);

            int prodCount = decipher.SelectCount(countFilter);

            return prodCount;
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

            string title = io_tag == "I" ? "入库单操作" : "出库单操作";

            this.headLab.Value = "<span class='page-head' >" + title + "</span>";

            DbDecipher decipher = ModelAction.OpenDecipher();


            SModelList smList = decipher.GetSModelList("select * from UT_012 where ROW_SID >= 0 and V_TEXT <> ''");

            SelectColumn selectColumn = table1.Columns.FindByDataField("CHECK_CODE") as SelectColumn;


            foreach (SModel sm in smList)
            {
                selectColumn.Items.Add(sm.Get<string>("V_TEXT"), sm.Get<string>("V_TEXT"));

            }

            if (io_tag == "I")
            {
                prodToolbar1.Visible = false;

                car_no_tb.Visible = false;
                car_date_dp.Visible = false;


                OutCodeList.Visible = false;
                SelectInOrderBtn.Visible = false;

                textBox1.FieldLabel = "入库单号";
                PrintQrCodeBtn.Visible = true;
                PrintInBtn.Visible = true;
                ComboBox_CJ_LEVE.Visible = true;





                foreach(SModel sm in smList)
                {
                    ComboBox_CJ_LEVE.Items.Add(sm.Get<string>("V_TEXT"), sm.Get<string>("V_TEXT"));

                }

            }
            else
            {
                this.table1.Columns.FindByDataField("OUT_SID").Visible = false;

                this.prodToolbar1.Visible = true;

                COL_21_nb.ReadOnly = true;
                textBox1.FieldLabel = "出库单号";
                PrintOutBtn.Visible = true;

            }

            LModel model = this.storeMain1.GetFirstData() as LModel;

            if (model != null)
            {

        
                BizSid_Changed(model.Get<int>("BIZ_SID"));
            }

        }


        private bool ChangeBizSID(int start, int end)
        {
           return  ChangeBizSID(null, null, start, end);
        }

        private bool ChangeBizSID(LModel model, DataRecord record, int start, int end)
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

           return ActualMgr.ChangeBizSID(model, this.storeMain1, record, start, end);
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

        /// <summary>
        /// 提交
        /// </summary>
        public void GoChangeBizSID_0_2()
        {
            string io_tag = WebUtil.QueryUpper("IO_TAG");


            DataRecord record = this.storeMain1.GetDataCurrent();

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = this.storeMain1.GetFirstData() as LModel;

            DynLModel dynModel = new DynLModel(model);

            if (dynModel["BIZ_SID"] != 0)
            {
                Toast.Show("‘提交’失败");
                return;
            }

            #region 验证数据

            bool isValid = true;

            if (StringUtil.IsBlank(dynModel["CLIENT_CODE"]))
            {
                this.storeMain1.MarkInvalid(record, "error", "CLIENT_CODE", "必填");
                isValid = false;
            }

            if (StringUtil.IsBlank(dynModel["CLIENT_TEXT"]))
            {
                this.storeMain1.MarkInvalid(record, "error", "CLIENT_TEXT", "必填");
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


            if (Check017IsLock())
            {
                MessageBox.Alert("入库中的数据有锁上的，不能提交！");
                return;
            }

            if(io_tag == "O")
            {
                List<LModel> lm017s = storeProd1.GetList() as List<LModel>;

                foreach(LModel lm017 in lm017s)
                {

                    string barCode = (string)lm017["PROD_CODE"];
                    LModel mIn = GetProdForIn(barCode);

                    if(mIn == null)
                    {
                        MessageBox.Alert($"入库单没有这个【{barCode}】条形码，不能出库！");
                        return;
                    }

                }

            }

            

      


            model.SetTakeChange(true);

            if (StringUtil.IsBlank(model["BILL_NO"]))
            {
                string newBillNo = BillIdentityMgr.NewCodeForDay("BILL_" + io_tag, io_tag, 3);


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

            ToStoreValue(this.storeMain1, record, model);
            

            try
            {



                ChangeBizSID(0, 2);

                
                //更新数量
                UpdataNum(record);

                UpdataSubBizSid(2);

                //上锁入库货物明细
                LockIn017Data(2);


                Toast.Show("提交成功");

                BizSid_Changed(2);
            }
            catch (Exception ex)
            {
                log.Error("提交失败", ex);
                MessageBox.Alert("提交失败!");
            }

        }


        /// <summary>
        /// 检查货物明细是否有上锁
        /// </summary>
        /// <returns>有锁上的数据返回true</returns>
        bool Check017IsLock()
        {
          

            List<LModel> lm017s = this.storeProd1.GetList() as List<LModel>;


            foreach(LModel lm017 in lm017s)
            {
               
                //看看入库那边有没有这条记录
                LModel lmI017 = Get017ByProdCode(lm017.Get<string>("PROD_CODE"),"I");

                if(lmI017 == null)
                {
                    throw new Exception($"这个【{lm017["PROD_CODE"]}】条形码在入库明细里面找不到");
                }

                if(lmI017.Get<int>("LOCK_SID") == 2)
                {
                    return true;
                }

            }

            return false;

        }


        void UpdataSubBizSid(int biz_sid)
        {

            string io_tag = WebUtil.Query("IO_TAG");

            int row_id = WebUtil.QueryInt("row_id");



            DbDecipher decipher = ModelAction.OpenDecipher();

            #region 改变子表的状态

            LightModelFilter filter2 = new LightModelFilter("UT_017_PROD");
            filter2.AddFilter("ROW_SID >= 0");
            filter2.AddFilter("BIZ_SID >= 0");
            filter2.And("DOC_PARENT_ID", row_id);
            filter2.And("IO_TAG", io_tag);


            LModelList<LModel> prodModels = decipher.GetModelList(filter2);

            foreach (var pm in prodModels)
            {
                pm.SetTakeChange(true);
                pm["BIZ_SID"] = biz_sid;

                decipher.UpdateModel(pm);
            }

            #endregion
        }

        /// <summary>
        /// 删除子表数据
        /// </summary>
        void DeleteSubData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();


            List<LModel> lm017s = storeProd1.GetList() as List<LModel>;


            foreach(LModel lm017 in lm017s)
            {

                lm017.SetTakeChange(true);

                lm017["ROW_SID"] = -3;
                lm017["ROW_DATE_DELETE"] = DateTime.Now;

            }


            decipher.BeginTransaction();

            try
            {
                decipher.UpdateModels(lm017s,true);

                decipher.TransactionCommit();


            }catch(Exception ex)
            {
                decipher.TransactionRollback();
                throw new Exception("删除子表数据出错了！",ex);
            }


        }

        void UpdataNum(DataRecord record)
        {


            int row_id = WebUtil.QueryInt("row_id");

            string io_tag = WebUtil.QueryUpper("IO_TAG");

            //入库的不拿数量
            if(io_tag == "I")
            {
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm016 = decipher.GetModelByPk("UT_016", row_id);

            LightModelFilter lmFilter = new LightModelFilter("UT_017_PROD");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("DOC_PARENT_ID", row_id);
            lmFilter.And("IO_TAG", io_tag);


            List<LModel> lm017s = decipher.GetModelList(lmFilter);

            if(io_tag == "I")
            {

                lm016["PROD_NUM"] = lm017s.Count;
                lm016["F_NUM"] = lm017s.Count;
                lm016["ROW_DATE_UPDATE"] = DateTime.Now;

                decipher.UpdateModelProps(lm016,"PROD_NUM", "F_NUM" ,"ROW_DATE_UPDATE");

            }
            else
            {
                lm016["PROD_NUM"] = lm017s.Count;
                lm016["F_NUM"] =-lm017s.Count;
                lm016["ROW_DATE_UPDATE"] = DateTime.Now;

                decipher.UpdateModelProps(lm016, "PROD_NUM", "F_NUM", "ROW_DATE_UPDATE");
            }




            storeMain1.SetRecordValue(record.Id, "PROD_NUM", lm017s.Count);
        }


        /// <summary>
        /// 锁上或解锁入库货物明细数据
        /// </summary>
        void LockIn017Data(int lock_sid)
        {

            List<LModel> lm017s = this.storeProd1.GetList() as List<LModel>;

            List<LModel> lmI017s = new List<LModel>();

            foreach(LModel lm017 in lm017s)
            {
                //看看入库那边有没有这条记录
                LModel lmI017 = Get017ByProdCode(lm017.Get<string>("PROD_CODE"), "I");

                lmI017.SetTakeChange(true);

                lmI017["LOCK_SID"] = lock_sid;
                lmI017["ROW_DATE_UPDATE"] = DateTime.Now;

                lmI017s.Add(lmI017);
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.BeginTransaction();

            try
            {
               int num =  decipher.UpdateModels(lmI017s,true);

                decipher.TransactionCommit();

            }catch(Exception ex)
            {
                decipher.TransactionRollback();
                throw new Exception("更新入库货物明细的上锁状态出问题了！", ex);
            }

        }

        /// <summary>
        /// 根据条形码和进出类型获取 货物明细对象
        /// </summary>
        /// <param name="io_tag">进出库标记</param>
        /// <param name="prod_code">货物条形码</param>
        /// <returns></returns>
        LModel Get017ByProdCode(string prod_code,string io_tag)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_017_PROD");
            lmFilter.AddFilter("ROW_SID >= 0");
            lmFilter.AddFilter("BIZ_SID >= 0");
            lmFilter.And("IO_TAG", io_tag);
            lmFilter.And("PROD_CODE", prod_code);

            LModel lm017 = decipher.GetModel(lmFilter);

            return lm017;

        }
       



        /// <summary>
        /// 审核
        /// </summary>
        public void GoChangeBizSID_2_4()
        {
            string io_tag = WebUtil.QueryUpper("IO_TAG");

            if(io_tag == "I")
            {
                ProChangeBizSID_2_4_ForIN();
            }
            else if(io_tag == "O")
            {
                ProChangeBizSID_2_4_ForOUT();
            }

        }



        private void ProChangeBizSID_2_4_ForOUT()
        {
            DataRecord record = this.storeMain1.GetDataCurrent();

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = this.storeMain1.GetFirstData() as LModel;

            DynLModel dynModel = new DynLModel(model);

            if (dynModel["BIZ_SID"] != 2)
            {
                Toast.Show("‘审核’失败");
                return;
            }


            LightModelFilter filter = new LightModelFilter("UT_017_PROD");
            filter.AddFilter("ROW_SID >= 0");
            filter.And("DOC_PARENT_ID", model.GetPk());

            LModelList<LModel> prodModels = decipher.GetModelList(filter);

            //if (dynModel["PROD_NUM"] == 0)
            //{
            //    Toast.Show("请填写数量.");
            //    return;
            //}

            string billNo = dynModel["BILL_NO"];

            decipher.BeginTransaction();

            try
            {
                foreach (var prodModel in prodModels)
                {
                    string barCode = (string)prodModel["PROD_CODE"];
                    LModel mIn = GetProdForIn(barCode);
                    
                    if(mIn == null)
                    {
                        throw new Exception($"此条码 {barCode} 不存在.");
                    }

                    mIn["OUT_SID"] = 2;
                    mIn["OUT_BILL_NO"] = billNo;

                    decipher.UpdateModelProps(mIn, "OUT_SID", "OUT_BILL_NO");
                }

                decipher.TransactionCommit();
            }
            catch(Exception ex)
            {
                decipher.TransactionRollback();

                MessageBox.Alert("提交失败.");

                return;
            }



            foreach (var prodModel in prodModels)
            {
                string barCode = (string)prodModel["PROD_CODE"];

                UpdateInData(barCode);
            }


            int prodNum = Convert.ToInt32(dynModel["PROD_NUM"]);
            
            



            model.SetTakeChange(true);

            model["PROD_NUM"] = prodModels.Count;

            model["DOC_CHECK_USER_TEXT"] = BizServer.LoginName;
            model["DOC_CHECK_DATE"] = DateTime.Now;

            model["F_NUM"] = -model.Get<decimal>("PROD_NUM");
            model["F_WEIGHT"] = -model.Get<decimal>("PROD_WEIGHT");

            model["SURPLUS_NUM"] = 0;// model["PROD_NUM"];
            model["SURPLUS_WEIGHT"] = 0;// model["PROD_WEIGHT"];

            model["OUT_NUM"] = 0;
            model["OUT_WEIGHT"] = 0;


            decipher.UpdateModel(model, true);


            ToStoreValue(this.storeMain1, record, model);


            try
            {
                ChangeBizSID(2, 4);

                //上锁入库货物明细
                LockIn017Data(0);

                UpdataSubBizSid(4);

                BizSid_Changed(4);

                Toast.Show("审核成功");
            }
            catch (Exception ex)
            {
                log.Error("审核失败", ex);
                MessageBox.Alert("审核失败!");
            }
        }



        private void ProChangeBizSID_2_4_ForIN()
        {
            DataRecord record = this.storeMain1.GetDataCurrent();

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = this.storeMain1.GetFirstData() as LModel;

            DynLModel dynModel = new DynLModel(model);

            if (dynModel["BIZ_SID"] != 2)
            {
                Toast.Show("‘审核’失败");
                return;
            }

            if (dynModel["PROD_NUM"] == 0)
            {
                Toast.Show("请填写数量.");
                return;
            }

            string billNo = dynModel["BILL_NO"];

            int prodNum = Convert.ToInt32( dynModel["PROD_NUM"]);

            for (int i = 0; i < prodNum; i++)
            {
                string newProdId = BizCommon.BillIdentityMgr.NewCodeForNum(billNo, string.Empty, 3);

                LModel ut017Prod = new LModel("UT_017_PROD");
                ut017Prod["DOC_PARENT_ID"] = model.GetPk();
                ut017Prod["IN_BILL_NO"] = model["BILL_NO"];
                ut017Prod["PROD_CODE"] = string.Format("{0}-{1}", billNo, newProdId);

                ut017Prod["IO_TAG"] = "I";
                ut017Prod["OUT_SID"] = 0;   //未出库

                ut017Prod["ROW_DATE_CREATE"] = ut017Prod["ROW_DATE_UPDATE"] = DateTime.Now;

                ut017Prod["CLIENT_CODE"] = model["CLIENT_CODE"];
                ut017Prod["CLIENT_TEXT"] = model["CLIENT_TEXT"];

                ut017Prod["S_PROD_CODE"] = model["S_PROD_CODE"];
                ut017Prod["S_PROD_TEXT"] = model["S_PROD_TEXT"];


                decipher.InsertModel(ut017Prod);

                this.storeProd1.Add(ut017Prod);
            }






            model.SetTakeChange(true);

            model["DOC_CHECK_USER_TEXT"] = BizServer.LoginName;
            model["DOC_CHECK_DATE"] = DateTime.Now;

            model["F_NUM"] = model["PROD_NUM"];
            model["F_WEIGHT"] = model["PROD_WEIGHT"];

            model["SURPLUS_NUM"] = model["PROD_NUM"];
            model["SURPLUS_WEIGHT"] = model["PROD_WEIGHT"];

            model["OUT_NUM"] =0;
            model["OUT_WEIGHT"] = 0;
                

            decipher.UpdateModel(model, true);


            ToStoreValue(this.storeMain1, record, model);


            try
            {
                ChangeBizSID(2, 4);

                Toast.Show("审核成功");

                BizSid_Changed(4);

                UpdataSubBizSid(4);
            }
            catch (Exception ex)
            {
                log.Error("审核失败", ex);
                MessageBox.Alert("审核失败!");
            }
        }


        /// <summary>
        /// 从提交->草稿
        /// </summary>
        public void GoChangeBizSID_2_0()
        {
            try
            {
                LModel lm016 = storeMain1.GetFirstData() as LModel;

                int biz_sid = lm016.Get<int>("BIZ_SID");

                //这有这两个才能执行撤销动作
                if (biz_sid != 2 && biz_sid != 1)
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

                Toast.Show("撤销提交成功");


                BizSid_Changed(0);
            }
            catch (Exception ex)
            {
                log.Error("撤销提交操作失败", ex);
                MessageBox.Alert("撤销提交操作失败!");
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
                    Toast.Show("‘作废操作’失败");
                }

            }
            catch (Exception ex)
            {
                log.Error("作废操作失败", ex);
                MessageBox.Alert("审核失败!");
            }
        }

       
        /// <summary>
        /// 出库啥搜毛
        /// </summary>
        public void GoOutWin()
        {
            int row_id = WebUtil.QueryInt("row_id");

            DbDecipher decipher = ModelAction.OpenDecipher();
            LModel model = decipher.GetModelByPk("UT_016", row_id);

            if(StringUtil.IsBlank( model["CLIENT_CODE"]))
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

            if(e["result"] != "ok")
            {
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel mainModel = this.storeMain1.GetFirstData() as LModel;

            string[] barCodeList = StringUtil.Split(e["value"], "\n", "\r\n");

            StringBuilder sb = new StringBuilder();

            foreach (string barCode in barCodeList)
            {
                LModel modelIn = GetProdForIn(barCode); //在场的货物

                if(modelIn == null)
                {
                    Toast.Show($"这个 {barCode} 条码无效.");
                    continue;
                }

                LightModelFilter filter = new LightModelFilter("UT_017_PROD");
                filter.AddFilter("ROW_SID >= 0");
                filter.And("DOC_PARENT_ID", mainModel.GetPk());
                filter.And("PROD_CODE", barCode);
                filter.And("IO_TAG", "O");

                if (decipher.ExistsModels(filter))
                {
                    Toast.Show($"此 {barCode} 已经存在记录中,重复.");
                    continue;
                }


                LModel model = new LModel("UT_017_PROD");
                modelIn.CopyTo(model);

                model["ROW_SID"] = 0;
                model["DOC_PARENT_ID"] = mainModel.GetPk();
                model["IO_TAG"] = "O";
                model["OUT_SID"] = 0;

                decipher.InsertModel(model);

                this.storeProd1.Add(model);
                
                Toast.Show($"添加 {barCode} 成功");
            }


            string io_tag = WebUtil.QueryUpper("io_tag");
            
            int count = GetProdCount((int)mainModel.GetPk());

            if (io_tag == "I")
            {
                mainModel["PROD_NUM"] = count;
                mainModel["F_NUM"] = count;
            }
            else
            {

                mainModel["PROD_NUM"] = count;
                mainModel["F_NUM"] = -count;
            }

            decipher.UpdateModelProps(mainModel, "PROD_NUM", "F_NUM");

            
            storeProd1.Refresh();

            this.storeMain1.SetRecordValue(mainModel.GetPk(), "PROD_NUM", count);
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

            if(io_tag != "O")
            {
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModelByPk("UT_016", row_id);

            if(model.Get<int>("BIZ_SID") > 0)
            {
                MessageBox.Alert("此订单已经提交或审核，无法删除.");
                return;
            }


            foreach (DataRecord record in this.table1.CheckedRows)
            {
                LModel pp = decipher.GetModelByPk("UT_017_PROD", record.Id);
                pp.SetTakeChange(true);

                pp["ROW_SID"] = -3;
                pp["ROW_DATE_DELETE"] = DateTime.Now;

                decipher.UpdateModel(pp, true);

                this.storeProd1.RemoveByClientId(record.ClientId);
            }


            
            int count = GetProdCount(row_id);

            if (io_tag == "I")
            {
                model["PROD_NUM"] = count;
                model["F_NUM"] = count;
            }
            else
            {

                model["PROD_NUM"] = count;
                model["F_NUM"] = -count;
            }

            decipher.UpdateModelProps(model, "PROD_NUM", "F_NUM");
            

            storeProd1.Refresh();

            this.storeMain1.SetRecordValue(row_id, "PROD_NUM", count);
        }


        /// <summary>
        /// update入库单的数据
        /// </summary>
        /// <param name="qr_code">条码</param>
        void UpdateInData(string qr_code)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter017 = new LightModelFilter("UT_017_PROD");
            lmFilter017.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter017.And("PROD_CODE", qr_code);
            //lmFilter017.And("OUT_SID", 0);
            lmFilter017.And("IO_TAG", "I");
            lmFilter017.Top = 1;




            LModel lm017 = decipher.GetModel(lmFilter017);

            if (lm017 == null)
            {
                throw new Exception("找不到UT_017的数据");
            }


            LModel lm016 = decipher.GetModelByPk("UT_016", lm017["DOC_PARENT_ID"]);


            LightModelFilter lmFilterNew017 = new LightModelFilter("UT_017_PROD");
            lmFilterNew017.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterNew017.And("DOC_PARENT_ID", lm016.GetPk());
            lmFilterNew017.Fields = new string[] { "OUT_SID" };

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


        private void BizSid_Changed(int bizSid)
        {
            bool readOnly = (bizSid > 0);

            string io_tag = WebUtil.QueryUpper("IO_TAG");




            string[] exFields = new string[]
            {
                "DOC_UPDATE_USER_TEXT",
                "DOC_UPDATE_DATE",
                "O_CAR_NO",
                "O_CAR_DATE",
                "PROD_NUM",
                "CJ_LEVE"
            };




            if (io_tag == "O")
            {

                prodToolbar1.Visible = (bizSid == 0);
            }

            foreach (var item in HanderForm1.Controls)
            {
                if (item is FieldBase)
                {
                    FieldBase fb = (FieldBase)item;

                    if (ArrayUtil.Exist(exFields,fb.DataField))
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

                    if (ArrayUtil.Exist(exFields, fb.DataField))
                    {
                        continue;
                    }

                    fb.ReadOnly = readOnly;
                }
            }

        }


        /// <summary>
        /// 从入库单选择按钮点击事件
        /// </summary>
        public void GoShowInOrder()
        {
            int row_id = WebUtil.QueryInt("row_id");


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
            string io_tag = WebUtil.QueryLower("io_tag");

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

            List<LModel> lmNew017s = new List<LModel>();

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

                LModel lmNew017 = new LModel("UT_017_PROD");

                lm017.CopyTo(lmNew017);

                lmNew017["IO_TAG"] = "O";
                lmNew017["ROW_DATE_CREATE"] = lmNew017["ROW_DATE_UPDATE"] = DateTime.Now;
                lmNew017["LOCK_SID"] = 0;
                lmNew017["DOC_PARENT_ID"] = row_id;


                lmNew017s.Add(lmNew017);

            }


            
            LModel u016 = decipher.GetModelByPk("UT_016",row_id);


            decipher.BeginTransaction();

            try
            {

                decipher.InsertModels<LModel>(lmNew017s);


                int count = GetProdCount(row_id);

                if (io_tag == "I")
                {
                    u016["PROD_NUM"] = count;
                    u016["F_NUM"] = count;
                }
                else
                {

                    u016["PROD_NUM"] = count;
                    u016["F_NUM"] = -count;
                }

                decipher.UpdateModelProps(u016, "PROD_NUM", "F_NUM");


                decipher.TransactionCommit();

                storeProd1.Refresh();

                this.storeMain1.SetRecordValue(row_id, "PROD_NUM", count);
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

        public void GoQrCodePrint()
        {
            int row_id = WebUtil.QueryInt("row_id");

            string io_tag = WebUtil.QueryUpper("io_tag");

            LightModelFilter filter = new LightModelFilter("UT_017_PROD");
            filter.AddFilter("ROW_SID >= 0");
            filter.And("DOC_PARENT_ID", row_id);


            DbDecipher decipher = ModelAction.OpenDecipher();

            int count = decipher.SelectCount(filter);


            Window win = new Window("打印设置", 500, 200);

            win.StartPosition = WindowStartPosition.CenterScreen;

            win.ContentPath = $"/app/InfoGrid2/View/Biz/Rosin/Prints/PrintSetup.aspx?row_id={row_id}&row_count={count}";

            win.Show();


        }

        /// <summary>
        /// 入库单打印按钮事件
        /// </summary>
        public void GoInPrint()
        {
            int row_id = WebUtil.QueryInt("row_id");
           

            Window win = new Window("打印", 800, 600);

            win.StartPosition = WindowStartPosition.CenterScreen;

            win.ContentPath = $"/app/InfoGrid2/View/Biz/Rosin/Prints/InFormPrint.aspx?row_id={row_id}";

            win.Show();

        }

        /// <summary>
        /// 出库单打印按钮事件
        /// </summary>
        public void GoOutPrint()
        {
            int row_id = WebUtil.QueryInt("row_id");

            Window win = new Window("打印", 800, 600);

            win.StartPosition = WindowStartPosition.CenterScreen;

            win.ContentPath = $"/app/InfoGrid2/View/Biz/Rosin/Prints/OutFormPrint.aspx?row_id={row_id}";

            win.Show();

        }


    }
}