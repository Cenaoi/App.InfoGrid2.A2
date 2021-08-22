using App.BizCommon;
using App.InfoGrid2.View.Biz.Rosin.Forms;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace App.InfoGrid2.View.Biz.Rosin.Transform
{
    public partial class WarehouseList : WidgetControl, IView
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


        protected void Page_Load(object sender, EventArgs e)
        {
            this.store1.Inserted += Store1_Inserted;

            store2.Filtering += Store2_Filtering;


            if (!this.IsPostBack)
            {



                DbDecipher decipher = ModelAction.OpenDecipher();

                LightModelFilter lmFilter = new LightModelFilter("UT_005");
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.And("CLIENT_TEXT", "", Logic.Inequality);
                lmFilter.Fields = new string[] { "CLIENT_TEXT" };

                List<LModel> lm005s = decipher.GetModelList(lmFilter);

                cb_cust_name.Items.Clear();
                cb_cust_name.Value = "";

                cb_cust_name_2.Items.Clear();
                cb_cust_name_2.Value = "";

                foreach (LModel lm005 in lm005s)
                {
                    string cust_name = lm005.Get<string>("CLIENT_TEXT");

                    cb_cust_name.Items.Add(cust_name, cust_name);
                    cb_cust_name_2.Items.Add(cust_name, cust_name);

                }

                //this.store1.DataBind();

                OnInitData();
            }
        }

        private void Store2_Filtering(object sender, ObjectCancelEventArgs e)
        {

            store2.FilterParams.Add("SESSION_ID", Session.SessionID);
        }

        private void Store1_Inserted(object sender, ObjectEventArgs e)
        {
            LModel model = e.Object as LModel;

            object pk = model.GetPk();

            string ioTag = model.Get<string>("IO_TAG");

            string url = $"/App/InfoGrid2/View/Biz/Rosin/Transform/WareForm.aspx?";

            url += "&io_tag=" + ioTag.ToUpper();
            url += "&row_id=" + pk;

            string title = ioTag == "I" ? "入库单" : "出库单";

            EasyClick.Web.Mini.MiniHelper.Eval("EcView.show('" + url + "','" + title + "');");
        }



        int CreateData(LModel model,string[] ids)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter lmFilter001 = new LightModelFilter("UT_001");
            lmFilter001.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter001.And("BIZ_SID", 0);
            lmFilter001.And("IO_TAG", "转移");
            lmFilter001.And("COL_11", model.GetPk());
            lmFilter001.And("SESSION_ID", Session.SessionID);



            LModel lmNew001 = decipher.GetModel(lmFilter001);

            //看看这个转移主表是否已经创建了！
            if (lmNew001 == null)
            {

                lmNew001 = new LModel("UT_001");

                model.CopyTo(lmNew001);
                lmNew001["BIZ_SID"] = 0;

                lmNew001["ROW_DATE_CREATE"] = lmNew001["ROW_DATE_UPDATE"] = DateTime.Now;
                lmNew001["IO_TAG"] = "转移";
                lmNew001["COL_11"] = model.GetPk();
                lmNew001["SRC_CLIENT_TEXT"] = model["CLIENT_NAME"];
                lmNew001["SESSION_ID"] = Session.SessionID;

                decipher.InsertModel(lmNew001);

            }

            LightModelFilter lmFilter002 = new LightModelFilter("UT_002");
            lmFilter002.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter002.And("IO_TAG", "I");
            lmFilter002.And("ROW_IDENTITY_ID", ids, Logic.In);

            List<LModel> lm002s = decipher.GetModelList(lmFilter002);

            List<LModel> lmNew002s = new List<LModel>();

            foreach(LModel lm002 in lm002s)
            {

                LModel lmNew002 = new LModel("UT_002");

                lm002.CopyTo(lmNew002);

                lmNew002["ROW_DATE_CREATE"] = lmNew002["ROW_DATE_UPDATE"] = DateTime.Now;
                lmNew002["IO_TAG"] = "转移";
                lmNew002["COL_1"] = lmNew001.GetPk();
                lmNew002["COL_30"] = lm002.GetPk();
                lmNew002["SESSION_ID"] = Session.SessionID;


                lmNew002s.Add(lmNew002);

            }

            decipher.BeginTransaction();

            try
            {


                decipher.InsertModels<LModel>(lmNew002s);

                decipher.TransactionCommit();


                return lmNew001.Get<int>("ROW_IDENTITY_ID");


            }catch(Exception ex)
            {
                decipher.TransactionRollback();

                log.Error(ex);
                return 0;

            }


        }


        private void OnInitData()
        {
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();


            this.headLab.Value = "<span class='page-head' >库存</span>";


        }


        public void GoToExcel()
        {

        }


        /// <summary>
        /// 把上面选中的数据插到下面去
        /// </summary>
        public void GoCheckedUT_002()
        {

            DataRecordCollection drc = table1.CheckedRows;

            if(drc.Count == 0)
            {
                MessageBox.Alert("请选择数据！");
                return;
            }


            List<string> id_002s = new List<string>();


            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter lmFilterOld002 = new LightModelFilter("UT_002");
            lmFilterOld002.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterOld002.And("BIZ_SID", 0);
            lmFilterOld002.And("IO_TAG", "转移");
            lmFilterOld002.And("SESSION_ID", Session.SessionID);
            lmFilterOld002.Top = 1;

            LModel lmOld002 = decipher.GetModel(lmFilterOld002);

            //单号
            string order_no = string.Empty;

            if(lmOld002 != null)
            {

                order_no = lmOld002.Get<string>("P_BILL_CODE");

            }



            foreach (DataRecord dr in drc)
            {
                #region 判断是否是同一张单


                LModel lmNew002 = decipher.GetModelByPk("UT_002", dr.Id);

                string p_bill_code = lmNew002.Get<string>("P_BILL_CODE");

                if (string.IsNullOrWhiteSpace(order_no))
                {
                    order_no = p_bill_code;
                }
                else
                {
                    if(order_no != p_bill_code)
                    {

                        MessageBox.Alert("不能一次转移两张不同单号的数据！");
                        return;

                    }
                }


                #endregion

                LightModelFilter lmFilter = new LightModelFilter("UT_002");
                lmFilter.And("ROW_SID",0, Logic.GreaterThanOrEqual);
                lmFilter.And("IO_TAG", "转移");
                lmFilter.And("BIZ_SID", 0);
                lmFilter.And("COL_30", dr.Id);
                lmFilter.And("SESSION_ID", Session.SessionID);


                //查看是否已有转移数据
                LModel lm002_1 = decipher.GetModel(lmFilter);

                if(lm002_1 != null)
                {
                    continue;
                }

                id_002s.Add(dr.Id);

            }

            if(id_002s.Count == 0)
            {
                MessageBox.Alert("请选择未转移的数据！");
                return;
            }

            LModel lm002 = decipher.GetModelByPk("UT_002", id_002s[0]);

            LModel lm001 = decipher.GetModelByPk("UT_001", lm002["COL_1"]);



            //创建转移数据
            int newPk = CreateData(lm001, id_002s.ToArray());

            store2.DataBind();

            Toast.Show("移下去了！");

        }


        public void GoTransform()
        {

            string cust_name = cb_cust_name_2.Value;

            if (string.IsNullOrWhiteSpace(cust_name))
            {
                MessageBox.Alert("请选择目标客户！");
                
                return;

            }



            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_002");
            lmFilter.And("ROW_SID",0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID", 0);
            lmFilter.And("IO_TAG", "转移");
            lmFilter.And("SESSION_ID", Session.SessionID);

            List<LModel> lm002s = decipher.GetModelList(lmFilter);

            if(lm002s.Count == 0)
            {
                MessageBox.Alert("没有要转移的数据！");
                return;
            }


            LModel lm001 = decipher.GetModelByPk("UT_001",lm002s[0]["COL_1"]);


            //创建入库单
            CreateIData(lm001,lm002s);
            //创建出库单
            CreatetOData(lm001,lm002s);

            UpdataNum((int)lm001.GetPk());


            lm001["BIZ_SID"] = 999;
            lm001["ROW_DATE_UPDATE"] = DateTime.Now;
            lm001["CLIENT_NAME"] = cb_cust_name_2.Value;

            decipher.UpdateModelProps(lm001, "BIZ_SID", "ROW_DATE_UPDATE", "CLIENT_NAME");


            foreach(LModel lm002 in lm002s)
            {

                lm002["BIZ_SID"] = 999;
                lm002["ROW_DATE_UPDATE"] = DateTime.Now;

                decipher.UpdateModelProps(lm002, "BIZ_SID", "ROW_DATE_UPDATE");


            }


            WareMsg.UpdataStockAll();


            store2.Refresh();



            Toast.Show("转移成功了！");

        }


        void CreateIData(LModel lm001,List<LModel> lm002s)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            //主表单号
            string bill_no = BillIdentityMgr.NewCodeForDay("BILL", "I", 4);

            //子表自增ID
            int newId = 0;


            LModel lmNew001 = new LModel("UT_001");

            lm001.CopyTo(lmNew001);
            lmNew001["ROW_DATE_CREATE"] = lmNew001["ROW_DATE_UPDATE"] = lmNew001["COL_3"] = DateTime.Now;
            lmNew001["IO_TAG"] = "I";
            lmNew001["CLIENT_NAME"] = cb_cust_name_2.Value;
            lmNew001["REMARK"] = "转移操作自动产生的单据";
            lmNew001["BILL_NO"] = bill_no;
            lmNew001["COL_1"] = BizServer.LoginName;
            lmNew001["BIZ_SUB_IDENTTIY"] = newId;



            decipher.InsertModel(lmNew001);


            List<LModel> lmNew002s = new List<LModel>();


            foreach (LModel lm002 in lm002s)
            {

                LModel lmNew002 = new LModel("UT_002");

                lm002.CopyTo(lmNew002);

                lmNew002["ROW_DATE_CREATE"] = lmNew002["ROW_DATE_UPDATE"] = DateTime.Now;
                lmNew002["IO_TAG"] = "I";
                lmNew002["COL_1"] = lmNew001.GetPk();
                lmNew002["P_CLIENT_TEXT"] = cb_cust_name_2.Value;

                lmNew002["BIZ_ROW_CODE"] = string.Format("{0}-{1}", bill_no, newId);
                lmNew002["BIZ_ROW_ID"] = newId;

                newId++;


                lmNew002s.Add(lmNew002);

            }

            lmNew001["BIZ_SUB_IDENTTIY"] = newId;

            decipher.BeginTransaction();

            try
            {


                decipher.InsertModels<LModel>(lmNew002s);

                decipher.UpdateModelProps(lmNew001, "BIZ_SUB_IDENTTIY");

                UpdataNum((int)lmNew001.GetPk());

                decipher.TransactionCommit();


            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error(ex);

            }


        }

        void CreatetOData(LModel lm001,List<LModel> lm002s)
        {


            //主表单号
            string bill_no = BillIdentityMgr.NewCodeForDay("BILL", "O", 4);

            //子表自增ID
            int newId = 0;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lmOld001 = decipher.GetModelByPk("UT_001", lm001.Get<int>("COL_11"));



            LModel lmNew001 = new LModel("UT_001");

            lm001.CopyTo(lmNew001);
            lmNew001["ROW_DATE_CREATE"] = lmNew001["ROW_DATE_UPDATE"] = lmNew001["COL_3"] = DateTime.Now;
            lmNew001["IO_TAG"] = "O";
            lmNew001["CLIENT_NAME"] = lm001["CLIENT_NAME"];
            lmNew001["REMARK"] = "转移操作自动产生的单据";
            lmNew001["BILL_NO"] = bill_no;
            lmNew001["COL_1"] = BizServer.LoginName;
            lmNew001["BIZ_SUB_IDENTTIY"] = newId;


            decipher.InsertModel(lmNew001);

            List<LModel> lmNew002s = new List<LModel>();

            foreach (LModel lm002 in lm002s)
            {

                LModel lmNew002 = new LModel("UT_002");

                lm002.CopyTo(lmNew002);

                lmNew002["ROW_DATE_CREATE"] = lmNew002["ROW_DATE_UPDATE"] = DateTime.Now;
                lmNew002["IO_TAG"] = "O";
                lmNew002["COL_1"] = lmNew001.GetPk();
                lmNew002["YSSL_NUM"] = -lm002.Get<decimal>("COL_18");
                lmNew002["YSZL_NUM"] = -lm002.Get<decimal>("COL_19");
                lmNew002["SL_NUM"] = -lm002.Get<decimal>("COL_21");
                lmNew002["ZL_NUM"] = -lm002.Get<decimal>("COL_23");
                lmNew002["BIZ_ROW_CODE"] = string.Format("{0}-{1}", bill_no, newId);
                lmNew002["BIZ_ROW_ID"] = newId;

                newId++;


                lmNew002s.Add(lmNew002);

            }

            lmNew001["BIZ_SUB_IDENTTIY"] = newId;

            decipher.BeginTransaction();

            try
            {


                decipher.InsertModels<LModel>(lmNew002s);

                decipher.UpdateModelProps(lmNew001, "BIZ_SUB_IDENTTIY");

                UpdataNum((int)lmNew001.GetPk());


                decipher.TransactionCommit();


            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error(ex);

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


    }
}