using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5;
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


namespace App.InfoGrid2.View.Biz.Rosin.Plan
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
            this.table1.Command += Table1_Command;

            this.table1.RowsChecked += Table1_RowsChecked;

            if (!this.IsPostBack)
            {
                this.store1.DataBind();

                OnInitData();
            }
        }

        private void Table1_RowsChecked(object sender, EventArgs e)
        {
            int count =  this.table1.CheckedRows.Count;

        }

        private void Store1_Inserted(object sender, ObjectEventArgs e)
        {
            LModel model = e.Object as LModel;

            object pk = model.GetPk();

            string ioTag = model.Get<string>("IO_TAG");

            string url = $"/App/InfoGrid2/View/Biz/Rosin/Plan/WareForm.aspx?";

            url += "&io_tag=" + ioTag.ToUpper();
            url += "&row_id=" + pk;

            string title = ioTag == "I" ? "入库计划" : "出库计划";

            EasyClick.Web.Mini.MiniHelper.Eval("EcView.show('" + url + "','" + title + "');");
        }

        private void Table1_Command(object sender, TableCommandEventArgs e)
        {
            if (e.CommandName == "GoEdit")
            {
                DataRecord record = e.Record;

                object pk = record.Id;

                DbDecipher decipher = ModelAction.OpenDecipher();

                LModel model = decipher.GetModelByPk("UT_008", pk);


                string ioTag = (string)model["IO_TAG"];

                string url = $"/App/InfoGrid2/View/Biz/Rosin/Plan/WareForm.aspx?";

                url += "&io_tag=" + ioTag.ToUpper();
                url += "&row_id=" + pk;


                string title = ioTag == "I" ? "入库计划" : "出库计划";

                EasyClick.Web.Mini.MiniHelper.Eval("EcView.show('" + url + "','" + title + "');");

            }
        }



        private void OnInitData()
        {
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            string title = io_tag == "I" ? "入库计划管理" : "出库计划管理";

            tbb_create_ut_001.Visible = io_tag == "I";

            this.headLab.Value = "<span class='page-head' >" + title + "</span>";


        }

        /// <summary>
        /// 提交
        /// </summary>
        public void GoChangeBizSID_0_2()
        {
            
            DataRecordCollection records = this.table1.CheckedRows;

            if(records.Count == 0)
            {
                return;
            }
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                int n = 0;

                foreach (DataRecord record in records)
                {
                    LModel model = decipher.GetModelByPk("UT_008", record.Id);

                    bool success = PlanMgr.ChangeBizSID_0_2(model, this.store1, record, io_tag);

                    if (success)
                    {
                        n++;
                    }
                }

                if(n > 0)
                {
                    Toast.Show("提交成功!");
                }
            }
            catch(Exception ex)
            {
                log.Error("提交失败", ex);
                MessageBox.Alert("提交失败!");
            }


        }


        public void GoToExcel()
        {

        }


        /// <summary>
        /// 改变 Biz 业务状态
        /// </summary>
        public void ChangeBizSID(string psStr)
        {
            string[] sp = StringUtil.Split(psStr, ",");


            Table tab = this.table1;
            Store store = this.store1;

            if (sp.Length == 4)
            {
                string tableId = sp[2];
                string storeId = sp[3];

                tab = FindControl(this.viewport1, tableId) as Table;
                store = FindControl(this.viewport1, storeId) as Store;

                psStr = $"{sp[0]},{sp[1]}";
            }

            try
            {
                //m_TableSet

                ResultBase result = BizHelper.ChangeBizSID(psStr, tab, store);

                Toast.Show(result.Message);
            }
            catch (Exception ex)
            {
                log.Error("改变 BIZ_SID 字段失败.", ex);
                MessageBox.Alert("错误:" + ex.Message);
            }


        }


        /// <summary>
        /// 把计划数据导入到库存表中
        /// </summary>
        public void GoCreateInBill()
        {

            DataRecordCollection drc = table1.CheckedRows;

            if(drc.Count == 0)
            {
                MessageBox.Alert("请至少选择一条数据！");
                return;
            }

            List<LModel> lm008s = new List<LModel>();

            DbDecipher decipher = ModelAction.OpenDecipher();


            foreach (DataRecord dr in drc)
            {


                LModel lm008 = decipher.GetModelByPk("UT_008", dr.Id);

                if (lm008.Get<int>("BIZ_SID") == 0)
                {
                    MessageBox.Alert("请选择不是草稿的数据！");
                    return;

                }

                string bill_no = lm008.Get<string>("BILL_NO");

                if (string.IsNullOrWhiteSpace(bill_no))
                {
                    MessageBox.Alert("提交的数据单号不能为空！");
                    return;
                }


                #region 查看是否已经在库存中存在这个计划数据了

                LightModelFilter lmFilter001 = new LightModelFilter("UT_001");
                lmFilter001.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter001.And("IO_TAG", "I");
                lmFilter001.And("FORM_PLAN_NO", lm008["BILL_NO"]);

                LModel lmOld001 = decipher.GetModel(lmFilter001);


                if (lmOld001 != null)
                {
                    MessageBox.Alert($"这个计划单号的【{lm008["BILL_NO"]}】数据已经存在入库表里了！");
                    return;
                }


                #endregion

                lm008s.Add(lm008);
            }


            foreach (LModel lm008 in lm008s)
            {

                LightModelFilter lmFilter009 = new LightModelFilter("UT_001");
                lmFilter009.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter009.And("IO_TAG", "I");
                lmFilter009.And("COL_1", lm008.GetPk());

                List<LModel> lm009s = decipher.GetModelList(lmFilter009);

                LModel lm001 = new LModel("UT_001");

                lm008.CopyTo(lm001, true);


                string bill_no = BillIdentityMgr.NewCodeForDay("BILL", "I", 4);

                int newId = 0;

                lm001["ROW_DATE_CREATE"] = lm001["ROW_DATE_UPDATE"] = lm001["COL_3"] = DateTime.Now;
                lm001["BIZ_SID"] = 0;
                lm001["BILL_NO"] = bill_no;
                lm001["COL_1"] = BizServer.LoginName;
                lm001["BIZ_SUB_IDENTTIY"] = newId;
                lm001["FORM_PLAN_NO"] = lm008["BILL_NO"];

                decipher.InsertModel(lm001);


                foreach (LModel lm009 in lm009s)
                {

                    LModel lmNew002 = new LModel("UT_002");

                    lm009.CopyTo(lmNew002, true);
                    lmNew002["BIZ_SID"] = 0;
                    lmNew002["ROW_DATE_CREATE"] = lmNew002["ROW_DATE_UPDATE"] = DateTime.Now;
                    lmNew002["IO_TAG"] = "I";
                    lmNew002["COL_1"] = lm001.GetPk();
                    lmNew002["BIZ_ROW_CODE"] = string.Format("{0}-{1}", bill_no, newId);
                    lmNew002["BIZ_ROW_ID"] = newId;

                    newId++;

                    decipher.InsertModel(lmNew002);
                }

                lm001["BIZ_SUB_IDENTTIY"] = newId;

                decipher.UpdateModelProps(lm001, "BIZ_SUB_IDENTTIY");

            }


         


            Toast.Show("产生入库单成功了！");


            
        }


    }
}