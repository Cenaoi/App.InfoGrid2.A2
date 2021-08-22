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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Rosin.Transform_V1
{
    public partial class WarehouseList : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            store1.Searching += Store1_Searching;
            store2.Deleting += Store2_Deleting;


            if (!IsPostBack)
            {
                InitData();
                store1.DataBind();
                store2.DataBind();
            }


        }

        private void Store2_Deleting(object sender, ObjectCancelEventArgs e)
        {

            LModel lm = e.Object as LModel;


            if(lm == null)
            {
                return;
            }

            if(lm.Get<int>("BIZ_SID") != 0)
            {
                e.Cancel = true;
                Toast.Show("不是草稿状态不能删除哦！");
                return;
            }




        }

        private void Store1_Searching(object sender, System.ComponentModel.CancelEventArgs e)
        {

            //e.Cancel = true;


            string cust_value = cb_cust_name.Value;

            string[] cust_values = cust_value.Split('|');

            store1.FilterParams.Add("CLIENT_CODE", cust_values[0]);

            if (!StringUtil.IsBlank(this.bill_no_tb.Value))
            {
                Param p = new Param("IN_BILL_NO", this.bill_no_tb.Value);
                p.Logic = "like";

                this.store1.FilterParams.Add(p);
            }

        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {

            headLab.Value = "<span class='page-head' >过户单</span>";


            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_005");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID", 4);
            lmFilter.And("CLIENT_TEXT", "", Logic.Inequality);
            lmFilter.Fields = new string[] { "CLIENT_TEXT", "CLIENT_CODE" };

            List<LModel> lm005s = decipher.GetModelList(lmFilter);

            cb_cust_name.Items.Clear();
            cb_cust_name.Value = "";

            cb_cust_name_2.Items.Clear();
            cb_cust_name_2.Value = "";

            foreach (LModel lm005 in lm005s)
            {
                string cust_name = lm005.Get<string>("CLIENT_TEXT");
                string cust_code = lm005.Get<string>("CLIENT_CODE");


                cb_cust_name.Items.Add(cust_code+ "|"+cust_name,cust_name);
                cb_cust_name_2.Items.Add(cust_code + "|" + cust_name, cust_name);

            }

            int row_id = WebUtil.QueryInt("row_id");

            LModel lm018 = decipher.GetModelByPk("UT_018", row_id);

            UpdataViewData(lm018);

        }


        /// <summary>
        /// 选到转移表中去
        /// </summary>
        public void GoCheckedUT_002()
        {

            int row_id = WebUtil.QueryInt("row_id");


            DataRecordCollection drc = table1.CheckedRows;

            if (drc.Count == 0)
            {
                MessageBox.Alert("请选择数据！");
                return;
            }


            List<LModel> lm017s = new List<LModel>();


            DbDecipher decipher = ModelAction.OpenDecipher();



            LModel lm018 = decipher.GetModelByPk("UT_018",row_id);

            if(lm018 == null || lm018.Get<int>("BIZ_SID") != 0)
            {
                MessageBox.Alert("只有草稿状态下才能转移！");
                return;
            }






            foreach (DataRecord dr in drc)
            {

                LModel lm017 = decipher.GetModelByPk("UT_017_PROD", dr.Id);

                LightModelFilter lmFilter019 = new LightModelFilter("UT_019");
                lmFilter019.And("ROW_SID",0, Logic.GreaterThanOrEqual);
                lmFilter019.And("BIZ_SID",2);
                lmFilter019.And("PROD_CODE", lm017["PROD_CODE"]);

                LModel lm019 = decipher.GetModel(lmFilter019);

                if(lm019 != null)
                {
                    MessageBox.Alert($"已经过户过，不能再过户了！编码：{lm017["PROD_CODE"]}");
                    return;
                }

                if(lm017.Get<int>("LOCK_SID") == 2)
                {
                    MessageBox.Alert("不能选择已经锁上的数据！");
                    return;
                }


                lm017s.Add(lm017);

            }


            List<LModel> lm019s = new List<LModel>();

            foreach(LModel lm017 in lm017s)
            {

                LModel lm019 = new LModel("UT_019");
                lm017.CopyTo(lm019, true);

                lm019["BIZ_SID"] = 0;
                lm019["ROW_DATE_CREATE"] = lm019["ROW_DATE_UPDATE"] = DateTime.Now;
                lm019["DOC_PARENT_ID"] = row_id;


                lm019s.Add(lm019);

            }

            lm018["ROW_DATE_UPDATE"] = DateTime.Now;
            decipher.UpdateModelProps(lm018, "ROW_DATE_UPDATE");

            decipher.BeginTransaction();

            try
            {

                decipher.InsertModels<LModel>(lm019s);

                decipher.TransactionCommit();

                store2.DataBind();

                Toast.Show("移下去了！");

            }catch(Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("创建转移数据出错了！",ex);

                return;
            }

        }


        /// <summary>
        /// 转移按钮点击事件
        /// </summary>
        public void GoTransform()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            int row_id = WebUtil.QueryInt("row_id");

            LModel lm018 = decipher.GetModelByPk("UT_018",row_id);

            if(lm018 == null || lm018.Get<int>("BIZ_SID") != 2)
            {
                MessageBox.Alert("不是审核状态中，不能直接审核！");
                return;
            }

            LightModelFilter lmFilter019 = new LightModelFilter("UT_019");
            lmFilter019.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter019.And("DOC_PARENT_ID", row_id);

            List<LModel> lm019s = decipher.GetModelList(lmFilter019);


            if(lm019s.Count == 0)
            {
                MessageBox.Alert("请选择转移数据啊！");
                return;
            }


            #region 创建出库单

            LModel lm016O = new LModel("UT_016");
            lm016O["ROW_DATE_CREATE"] = lm016O["ROW_DATE_UPDATE"] = DateTime.Now;
            lm016O["BIZ_SID"] = 4;
            lm016O["PROD_NUM_UNIT"] = "桶";
            lm016O["IO_TAG"] = "O";
            lm016O["SUB_IO_TAG"] = "O_T";
            lm016O["CLIENT_CODE"] = lm019s[0]["SRC_CLIENT_CODE"];
            lm016O["CLIENT_TEXT"] = lm019s[0]["SRC_CLIENT_TEXT"];

            string newBillNo_o = BillIdentityMgr.NewCodeForDay("BILL_TO", "O", 3);

            lm016O["BILL_NO"] = newBillNo_o;
            lm016O["DOC_CREATE_USER_TEXT"] = BizServer.LoginName;
            lm016O["DOC_CREATE_DATE"] = DateTime.Now;

            lm016O["DOC_CHECK_USER_TEXT"] = BizServer.LoginName;
            lm016O["DOC_CHECK_DATE"] = DateTime.Now;


            lm016O["REMARK"] = "过户单";

            #endregion


            #region 创建入库单

            LModel lm016I = new LModel("UT_016");
            lm016I["IO_TAG"] = "I";
            lm016I["PROD_NUM_UNIT"] = "桶";
            lm016I["SUB_IO_TAG"] = "I_T";
            lm016I["ROW_DATE_CREATE"] = lm016I["ROW_DATE_UPDATE"] = DateTime.Now;
            lm016I["BIZ_SID"] = 4;
            lm016I["CLIENT_CODE"] = lm019s[0]["CLIENT_CODE"];
            lm016I["CLIENT_TEXT"] = lm019s[0]["CLIENT_TEXT"];

            string newBillNo_i = BillIdentityMgr.NewCodeForDay("BILL_TI", "I", 3);

            lm016I["BILL_NO"] = newBillNo_i;
            lm016I["DOC_CREATE_USER_TEXT"] = BizServer.LoginName;
            lm016I["DOC_CREATE_DATE"] = DateTime.Now;

            lm016O["DOC_CHECK_USER_TEXT"] = BizServer.LoginName;
            lm016O["DOC_CHECK_DATE"] = DateTime.Now;


            lm016I["REMARK"] = "过户单";

            #endregion


            decipher.InsertModel(lm016O);

            decipher.InsertModel(lm016I);


            foreach (LModel lm019 in lm019s)
            {
                #region 出库明细

                LModel lm017O = new LModel("UT_017_PROD");

                lm019.CopyTo(lm017O, true);

                lm017O["ROW_DATE_UPDATE"] = DateTime.Now;
                lm017O["IO_TAG"] = "O";
                lm017O["BIZ_SID"] = 4;
                lm017O["DOC_PARENT_ID"] = lm016O.GetPk();

                decipher.InsertModel(lm017O);

                #endregion


                #region  入库明细

                LModel lm017I = new LModel("UT_017_PROD");

                lm019.CopyTo(lm017I, true);

                lm017I["BIZ_SID"] = 4;
                lm017I["ROW_DATE_UPDATE"] = DateTime.Now;
                lm017I["DOC_PARENT_ID"] = lm016I.GetPk();
                

                decipher.InsertModel(lm017I);

                #endregion


                UpdateInData(lm019.Get<string>("PROD_CODE"));

                lm019.SetTakeChange(true);


                lm019["BIZ_SID"] = 999;

                decipher.UpdateModel(lm019,true);

                UpdataLm017ByCode(lm019.Get<string>("PROD_CODE"), 0);

            }

           

            lm016I["PROD_NUM"] = lm019s.Count;
            lm016I["F_NUM"] = lm019s.Count;
            lm016O["PROD_NUM"] = lm019s.Count;
            lm016O["F_NUM"] = -lm019s.Count;

            decipher.UpdateModelProps(lm016I, "PROD_NUM", "F_NUM");
            decipher.UpdateModelProps(lm016O, "PROD_NUM", "F_NUM");

           
            lm018["BIZ_SID"] = 999;

            decipher.UpdateModelProps(lm018, "BIZ_SID");

            UpdataViewData(lm018);
            store2.Refresh();
            store1.Refresh();

            Toast.Show("转移数据成功了！");


        }


        /// <summary>
        /// 更新UT_017实体Lock_SID 的状态 数据
        /// </summary>
        /// <param name="prod_code">条码</param>
        /// <param name="biz_sid">0--不锁行 2--锁行</param>
        void UpdataLm017ByCode(string prod_code,int biz_sid)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter017 = new LightModelFilter("UT_017_PROD");
            lmFilter017.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter017.And("PROD_CODE", prod_code);

            LModel lm017 = decipher.GetModel(lmFilter017);

            lm017["LOCK_SID"] = biz_sid;
            lm017["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.UpdateModelProps(lm017, "LOCK_SID", "ROW_DATE_UPDATE");
        }


        /// <summary>
        /// 提交按钮点击事件
        /// </summary>
        public void GoChangeBizsid0_2()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            int row_id = WebUtil.QueryInt("row_id");

            string cust_value = cb_cust_name_2.Value;

            // 0 是客户编码  1是客户名称
            string[] cust_values = cust_value.Split('|');

            if (cust_values.Length < 2)
            {
                MessageBox.Alert("请选择客户！");
                return;
            }

            LModel lm018 = decipher.GetModelByPk("UT_018", row_id);

            if(lm018 == null || lm018.Get<int>("BIZ_SID") != 0)
            {
                MessageBox.Alert("不是草稿状态不能提交！");
                return;
            }

            LightModelFilter lmFilter019 = new LightModelFilter("UT_019");
            lmFilter019.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter019.And("DOC_PARENT_ID", row_id);

            List<LModel> lm019s = decipher.GetModelList(lmFilter019);


            if(lm019s.Count == 0)
            {
                MessageBox.Alert("没有数据，不能提交！");
                return;
            }

            #region 判断是否自己转给自己



            if(cust_values[0] == lm019s[0].Get<string>("CLIENT_CODE"))
            {
                MessageBox.Alert("不能自己转给自己！");
                return;
            }



            #endregion


            List<LModel> lm017s = new List<LModel>();


            foreach(LModel lm019 in lm019s)
            {

                string prod_code = lm019.Get<string>("PROD_CODE");

                LightModelFilter lmFilter017 = new LightModelFilter("UT_017_PROD");
                lmFilter017.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter017.And("PROD_CODE", prod_code);

                LModel lm017 = decipher.GetModel(lmFilter017);

                int lock_sid = lm017.Get<int>("LOCK_SID");

                if(lock_sid == 2)
                {
                    MessageBox.Alert($"这个条码【{prod_code}】已经被锁住了，不能提交，请删除掉！");
                    return;
                }

                lm017s.Add(lm017);
            }



            #region 更新转移主表数据

            lm018.SetTakeChange(true);

            lm018["BIZ_SID"] = 2;
            lm018["CLIENT_CODE"] = cust_values[0];
            lm018["CLIENT_TEXT"] = cust_values[1];
            lm018["CHANGE_BILL_NO"] = BillIdentityMgr.NewCodeForDay("BILL_T", "T-", 3);


            lm018["SRC_CLIENT_CODE"] = lm019s[0]["CLIENT_CODE"];
            lm018["SRC_CLIENT_TEXT"] = lm019s[0]["CLIENT_TEXT"];
            lm018["ROW_DATE_UPDATE"] = DateTime.Now;


            lm018["F_NUM"] = lm019s.Count;

            decipher.UpdateModel(lm018, true);

            #endregion


            decipher.UpdateModelProps(lm018,"BIZ_SID");

            decipher.UpdateProps(lmFilter019, 
                new object[] {
                    "BIZ_SID", 2, "ROW_DATE_UPDATE", DateTime.Now, "SRC_CLIENT_CODE", lm018["SRC_CLIENT_CODE"], "SRC_CLIENT_TEXT", lm018["SRC_CLIENT_TEXT"],
                    "CLIENT_CODE",cust_values[0],"CLIENT_TEXT", cust_values[1]
                });

            foreach(LModel lm017 in lm017s)
            {
                lm017["LOCK_SID"] = 2;
                lm017["ROW_DATE_UPDATE"] = DateTime.Now;

                decipher.UpdateModelProps(lm017, "LOCK_SID", "ROW_DATE_UPDATE");

            }


            UpdataViewData(lm018);

            store1.Refresh();
            store2.Refresh();

            Toast.Show("提交成功了！");

        }


        /// <summary>
        /// 更新界面上的一些主表数据
        /// </summary>
        /// <param name="lm018">主表</param>
        void UpdataViewData(LModel lm018)
        {

            cb_ut_018_biz_sid.Value = lm018.Get<string>("BIZ_SID");

            tbx_change_bill_no.Value = lm018.Get<string>("CHANGE_BILL_NO");

        }



        /// <summary>
        /// update入库单的数据
        /// </summary>
        /// <param name="qr_code">条码</param>
        void UpdateInData(string qr_code)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter017 = new LightModelFilter("UT_017_PROD");
            lmFilter017.And("ROW_SID",0, Logic.GreaterThanOrEqual);
            lmFilter017.And("PROD_CODE", qr_code);
            lmFilter017.And("OUT_SID", 0);
            lmFilter017.And("IO_TAG", "I");
            lmFilter017.Top = 1;


            LModel lm017 = decipher.GetModel(lmFilter017);

            if(lm017 == null)
            {
                throw new Exception("找不到UT_017的数据");
            }

            lm017["OUT_SID"] = 2;
            lm017["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.UpdateModelProps(lm017, "OUT_SID", "ROW_DATE_UPDATE");


            LModel lm016 = decipher.GetModelByPk("UT_016", lm017["DOC_PARENT_ID"]);


            LightModelFilter lmFilterNew017 = new LightModelFilter("UT_017_PROD");
            lmFilterNew017.And("ROW_SID",0, Logic.GreaterThanOrEqual);
            lmFilterNew017.And("DOC_PARENT_ID", lm016.GetPk());
            

            List<LModel> lmNew017s = decipher.GetModelList(lmFilterNew017);

            //进数量
            decimal i_num = 0;
            //出数量
            decimal o_num = 0;


            foreach(LModel lmNew017 in lmNew017s)
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

            lm016["SURPLUS_WEIGHT"] =0;
            lm016["OUT_NUM"] = o_num;
            lm016["OUT_WEIGHT"] = 0;


            decipher.UpdateModel(lm016,true);


        }


        /// <summary>
        /// 作废按钮点击事件
        /// </summary>
        public void GoChangeBizSID_0_F3()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();


            int row_id = WebUtil.QueryInt("row_id");

            

            LModel lm018 = decipher.GetModelByPk("UT_018", row_id);

            if (lm018 == null || lm018.Get<int>("BIZ_SID") != 0)
            {
                Toast.Show("不是草稿状态下不能作废！");
                return;
            }


            lm018["ROW_SID"] = -3;
            lm018["ROW_DATE_DELETE"] = DateTime.Now;

            decipher.UpdateModelProps(lm018, "ROW_SID", "ROW_DATE_DELETE");


            LightModelFilter lmFilter019 = new LightModelFilter("UT_019");
            lmFilter019.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter019.And("DOC_PARENT_ID", row_id);

            decipher.UpdateProps(lmFilter019, new object[] {"ROW_SID",-3, "ROW_DATE_DELETE", DateTime.Now  });

            Toast.Show("作废成功了！");


        }




    }
}