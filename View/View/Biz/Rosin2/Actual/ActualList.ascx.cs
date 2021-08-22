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

namespace App.InfoGrid2.View.Biz.Rosin2.Actual
{
    public partial class ActualList : WidgetControl, IView
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

            win.WindowClosed += Win_WindowClosed1;

            win.ShowDialog();
        }

        public void Win_WindowClosed1(object sender, string data)
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
            if (e.CommandName == "GoEdit")
            {
                DataRecord record = e.Record;

                object pk = record.Id;

                string ioTag = WebUtil.Query("io_tag");

                Window win = new Window((ioTag == "I" ? "入库" : "出库") + "单");
                win.ContentPath = $"/App/InfoGrid2/View/Biz/Rosin2/Actual/ActualForm.aspx?io_tag={ioTag}&row_id={pk}";
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

            Window win = new Window((ioTag == "I" ? "入库" : "出库") + "单");
            win.ContentPath = $"/App/InfoGrid2/View/Biz/Rosin2/Actual/ActualForm.aspx?io_tag={ioTag}&row_id={pk}";
            win.State = WindowState.Max;
            
            win.WindowClosed += Win_WindowClosed;
            win.ShowDialog();


        }

        public void Win_WindowClosed(object sender, string data)
        {
            //Window win = (Window)sender;

            //string tag = win.Tag;

            //先放在界面上
            string pk = H_ID.Value;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm016 = decipher.GetModelByPk("UT_016", pk);


            if (lm016 == null)
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
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            bool is_io = io_tag == "I";

            string title = is_io ? "入库管理" : "出库管理";
            
            this.headLab.Value = "<span class='page-head' >" + title + "</span>";


            if (is_io)
            {

            }else
            {


               BoundField bf =  table1.Columns.FindByDataField("CJ_LEVE");

               if(bf != null)
                {
                    bf.Visible = false;
                }


            }

        }




        private void ChangeBizSID(int start, int end)
        {
            ChangeBizSID(null, null, start, end);
        }

        private void ChangeBizSID(LModel model, DataRecord record, int start, int end)
        {
            ActualMgr.ChangeBizSID(model, this.store1, record, start, end);
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

                    #region 改变子表的状态

                    LightModelFilter filter2 = new LightModelFilter("UT_017_PROD");
                    filter2.AddFilter("ROW_SID >= 0");
                    filter2.AddFilter("BIZ_SID >= 0");

                    LModelList<LModel> prodModels  = decipher.GetModelList (filter2);

                    foreach (var pm in prodModels)
                    {
                        pm.SetTakeChange(true);
                        pm["BIZ_SID"] = 2;

                        decipher.UpdateModel(pm);
                    }

                    #endregion


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
    }
}