using App.BizCommon;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace App.InfoGrid2.View.Biz.Rosin.Forms
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

            if (!this.IsPostBack)
            {
                this.store1.DataBind();

                OnInitData();
            }
        }



        private void Store1_Inserted(object sender, ObjectEventArgs e)
        {
            LModel model = e.Object as LModel;

            object pk = model.GetPk();

            string ioTag = model.Get<string>("IO_TAG");

            string url = $"/App/InfoGrid2/View/Biz/Rosin/Forms/WareForm.aspx?";

            url += "&io_tag=" + ioTag.ToUpper();
            url += "&row_id=" + pk;

            string title = ioTag == "I" ? "入库单" : "出库单";

            EasyClick.Web.Mini.MiniHelper.Eval("EcView.show('" + url + "','" + title + "');");
        }

        private void Table1_Command(object sender, TableCommandEventArgs e)
        {
            if(e.CommandName == "GoEdit")
            {
                DataRecord record = e.Record;
                
                object pk = record.Id;

                DbDecipher decipher = ModelAction.OpenDecipher();

                LModel model = decipher.GetModelByPk("UT_001", pk);


                string ioTag =(string)model["IO_TAG"];

                string url = $"/App/InfoGrid2/View/Biz/Rosin/Forms/WareForm.aspx?";

                url += "&io_tag=" + ioTag.ToUpper();
                url += "&row_id=" + pk;


                string title = ioTag == "I" ? "入库单" : "出库单";

                EasyClick.Web.Mini.MiniHelper.Eval("EcView.show('" + url + "','" + title + "');");

            }    
        }



        private void OnInitData()
        {
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            string title = io_tag == "I" ? "入库单管理" : "出库单管理";

            this.headLab.Value = "<span class='page-head' >" + title + "</span>";


        }

        
        public void GoToExcel()
        {
            
        }


        /// <summary>
        /// 提交
        /// </summary>
        public void GoChangeBizSID_0_2()
        {

            DataRecordCollection records = this.table1.CheckedRows;

            if (records.Count == 0)
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
                    LModel model = decipher.GetModelByPk("UT_001", record.Id);

                    bool success = Plan.PlanMgr.ChangeBizSID_0_2(model, this.store1, record, io_tag);

                    if (success)
                    {
                        n++;
                    }
                }

                if (n > 0)
                {
                    Toast.Show("提交成功!");
                }
            }
            catch (Exception ex)
            {
                log.Error("提交失败", ex);
                MessageBox.Alert("提交失败!");
            }


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
    }
}