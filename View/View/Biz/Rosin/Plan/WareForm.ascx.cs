using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
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

namespace App.InfoGrid2.View.Biz.Rosin.Plan
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
            this.storeProd1.Inserted += storeProd1_Inserted;

            this.tableProd1.Command += TableProd1_Command;

            if (!this.IsPostBack)
            {

                string io_tag = WebUtil.Query("io_tag");

                //是否是出口
                bool is_o = (io_tag == "O");

                tbb_import_excel.Visible = !is_o;

                DbDecipher decipher = ModelAction.OpenDecipher();

                LightModelFilter lmFilter = new LightModelFilter("UT_010");
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

                List<LModel> lm010s = decipher.GetModelList(lmFilter);

                cb_store.Items.Clear();
                cb_store.Value = "";

                foreach (LModel lm010 in lm010s)
                {

                    string store_text = lm010.Get<string>("COL_1");


                    cb_store.Items.Add(store_text, store_text);


                }





                this.storeMain1.DataBind();

                this.storeProd1.DataBind();
            }

            OnInitData();
        }




        private void TableProd1_Command(object sender, TableCommandEventArgs e)
        {
            if (e.CommandName == "GoEdit")
            {
                DataRecord record = e.Record;

                object pk = record.Id;

                DbDecipher decipher = ModelAction.OpenDecipher();

                LModel model = decipher.GetModelByPk("UT_009", pk);


                int parentId = model.Get<int>("COL_1");
                string ioTag = (string)model["IO_TAG"];

                string url = $"/App/InfoGrid2/View/Biz/Rosin/Plan/WareProdForm.aspx?";

                url += "&io_tag=" + ioTag.ToUpper();
                url += "&parent_id=" + parentId;
                url += "&row_id=" + pk;


                string title = ioTag == "I" ? "入库计划货物" : "出库计划货物";

                Window win = new Window(title);
                win.ContentPath = url;
                win.State = WindowState.Max;
                win.ShowDialog();

            }
        }

        private void storeProd1_Inserted(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            LModel model = e.Object as LModel;

            object pk = model.GetPk();

            int parentId = model.Get<int>("COL_1");
            string ioTag = model.Get<string>("IO_TAG");

            string url = $"/App/InfoGrid2/View/Biz/Rosin/Plan/WareProdForm.aspx?";

            url += "&io_tag=" + ioTag.ToUpper();
            url += "&parent_id=" + parentId;
            url += "&row_id=" + pk;

            string title = ioTag == "I" ? "入库计划货物" : "入库计划货物";

            Window win = new Window(title);
            win.ContentPath = url;
            win.State = WindowState.Max;
            win.ShowDialog();


        }

        private void OnInitData()
        {
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            string title = io_tag == "I" ? "入库计划" : "出库计划";

            this.headLab.Value = "<span class='page-head' >" + title + "</span>";


        }

        public void GoSave()
        {
            int row_id = WebUtil.QueryInt("row_id");
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModelByPk("UT_008", row_id);

            bool success = ChangeBizSID(0, 2);

            if (!success)
            {
                Toast.Show("提交失败！可能已提交。", Toast.STYLE_WARNING);
                return;
            }

            if (StringUtil.IsBlank(model["BILL_NO"]))
            {
                string newBillNo = BillIdentityMgr.NewCodeForDay("BILL", io_tag, 4);

                decipher.UpdateModelByPk("UT_008", row_id, new object[] {
                    "BILL_NO", newBillNo,
                    "COL_3", DateTime.Now,
                    "COL_1", BizServer.LoginName
                });

                textBox1.Value = newBillNo;
                textBox6.Value = BizServer.LoginName;   //开单人
                textBox8.Value = DateTime.Now.ToString();  //开单时间
            }
            else
            {
                string upCode = BillIdentityMgr.NewCodeForDay("UPDATE_BILL", io_tag + "UP-", 4);

                decipher.UpdateModelByPk("UT_008", row_id, new object[] {
                    "COL_4",upCode,
                    "COL_5", BizServer.LoginName,
                    "COL_6", DateTime.Now
                });

                this.textBox9.Value = upCode;
                this.textBox10.Value = BizServer.LoginName;
                this.textBox11.Value = DateTime.Now.ToString();
            }

            Toast.Show("提交成功!");


        }

        public void GoChangeBizSID_2_4()
        {
            ChangeBizSID(2, 4);

            int row_id = WebUtil.QueryInt("row_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModelByPk("UT_001", row_id);
            this.textBox7.Value = BizServer.LoginName;

            decipher.UpdateModelByPk("UT_001", row_id, new object[] {
                "COL_2", BizServer.LoginName
            });

            Toast.Show("审核成功!");
        }

        /// <summary>
        /// 撤销审核
        /// </summary>
        public void GoChangeBizSID_2_0()
        {
            ChangeBizSID(2, 0);


            Toast.Show("已撤销提交!");
        }

        public void GoChangeBizSID_4_2()
        {
            ChangeBizSID(4, 2);


            Toast.Show("已撤销审核!");
        }

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="startSID"></param>
        /// <param name="endSID"></param>
        /// <returns>改变状态，</returns>
        private bool ChangeBizSID(int startSID, int endSID)
        {
            int row_id = WebUtil.QueryInt("row_id");
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModelByPk("UT_008", row_id);

            if (model.Get<int>("BIZ_SID") == startSID)
            {
                decipher.UpdateModelByPk("UT_008", row_id, new object[] {
                    "BIZ_SID",endSID
                });


                this.bizSID_ci.Value = endSID.ToString();
                bizSID_cb.Value = endSID.ToString();

                if (startSID < endSID)
                {
                    AutoAdd_UT001(model);
                }

                return true;
            }

            return false;
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

            if (modelElem.DBTableName == "UT_008" && modelElem.HasField("CLIENT_NAME"))
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

            if (modelElem.DBTableName == "UT_009" && modelElem.HasField("COL_3"))
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
        void UpdataNum(string id)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm001 = decipher.GetModelByPk("UT_008", id);


            LightModelFilter lmFilter = new LightModelFilter("UT_009");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_1", id);
            lmFilter.And("IO_TAG", lm001["IO_TAG"]);

            List<LModel> lm002s = decipher.GetModelList(lmFilter);

            lm001["NUM_TOTAL"] = LModelMath.Sum(lm002s, "COL_21");
            lm001["WEIGHT_TOTAL"] = LModelMath.Sum(lm002s, "COL_23");

            decipher.UpdateModelProps(lm001, "NUM_TOTAL", "WEIGHT_TOTAL");
        }


        /// <summary>
        /// 显示导入Excel数据界面
        /// </summary>
        public void GoShowImportDataView()
        {

            int row_id = WebUtil.QueryInt("row_id");


            string url = $"/App/InfoGrid2/View/Biz/Rosin/ImportExcel/SelectExcel2.aspx?row_id={row_id}";

            Window win = new Window("Excel 文件上传");
            win.ContentPath = url;

            win.Height = 200;
            win.Width = 500;
            win.StartPosition = WindowStartPosition.CenterScreen;
            win.FormClosedForJS = "importExcelViewClose";
            win.ShowDialog();


        }

        /// <summary>
        /// 导入Excel数据界面关闭事件
        /// </summary>
        public void GoImportExcelViewClose()
        {
            storeProd1.Refresh();

            MessageBox.Alert("导入数据成功了！");

        }

    }
}