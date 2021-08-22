using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
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

namespace App.InfoGrid2.View.Biz.Rosin.Transform
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
            this.storeProd1.Inserted += storeProd1_Inserted;

            this.tableProd1.Command += TableProd1_Command;


            if (!this.IsPostBack)
            {

                string io_tag = WebUtil.Query("io_tag");

                //是否是出口
                bool is_o = (io_tag == "O");



                //tbx_COL_7.Visible = is_o;
                //tbx_COL_8.Visible = is_o;
                //tbx_COL_9.Visible = is_o;
                //dp_COL_10.Visible = is_o;


                this.storeMain1.DataBind();

                this.storeProd1.DataBind();
            }

            OnInitData();
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

                string url = $"/App/InfoGrid2/View/Biz/Rosin/Transform/WareProdForm.aspx?";

                url += "&io_tag=" + ioTag.ToUpper();
                url += "&parent_id=" + parentId;
                url += "&row_id=" + pk;


                string title = ioTag == "I" ? "入库货物" : "入库货物";

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

            string url = $"/App/InfoGrid2/View/Biz/Rosin/Transform/WareProdForm.aspx?";

            url += "&io_tag=" + ioTag.ToUpper();
            url += "&parent_id=" + parentId;
            url += "&row_id=" + pk;

            string title = ioTag == "I" ? "入库货物" : "入库货物";

            Window win = new Window(title);
            win.ContentPath = url;
            win.State = WindowState.Max;
            win.ShowDialog();


        }

        private void OnInitData()
        {
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();


            this.headLab.Value = "<span class='page-head' >转移单</span>";


        }

        public void GoSave()
        {
            int row_id = WebUtil.QueryInt("row_id");
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModelByPk("UT_001", row_id);

            ChangeBizSID(0, 2);

            if (StringUtil.IsBlank(model["BILL_NO"]))
            {
                string newBillNo = BillIdentityMgr.NewCodeForDay("BILL", io_tag, 4);

                decipher.UpdateModelByPk("UT_001", row_id, new object[] {
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

                decipher.UpdateModelByPk("UT_001", row_id, new object[] {
                    "COL_4",upCode,
                    "COL_5", BizServer.LoginName,
                    "COL_6", DateTime.Now
                });

                this.textBox9.Value = upCode;
                this.textBox10.Value = BizServer.LoginName;
                this.textBox11.Value = DateTime.Now.ToString();
            }


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


        }

        public void GoChangeBizSID_2_0()
        {
            ChangeBizSID(2, 0);
        }

        public void GoChangeBizSID_4_2()
        {
            ChangeBizSID(4, 2);
        }

        private void ChangeBizSID(int startSID, int endSID)
        {
            int row_id = WebUtil.QueryInt("row_id");
            string io_tag = WebUtil.Query("IO_TAG").ToUpper();

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel model = decipher.GetModelByPk("UT_001", row_id);

            if (model.Get<int>("BIZ_SID") == startSID)
            {
                decipher.UpdateModelByPk("UT_001", row_id, new object[] {
                    "BIZ_SID",endSID
                });

                bizSID_cb.Value = endSID.ToString();

                if (startSID < endSID)
                {
                    AutoAdd_UT001(model);
                }

            }
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

            try
            {
                //   /_Temporary/Excel/g

                //导出路径
                string mapath = System.Web.HttpContext.Current.Server.MapPath("/_Temporary/Excel");

                //判断文件夹是否存在
                if (!Directory.Exists(mapath))
                {
                    Directory.CreateDirectory(mapath);
                }

                //文件名为当前时间时分秒都有
                string fileName = DateTime.Now.ToString("yyMMddHHmmss");

                //这是绝对物理路径
                string filePath = string.Format("{0}\\{1}.xls", mapath, fileName);

                //这是服务器路径
                string urlPaht = string.Format("/_Temporary/Excel/{0}.xls", fileName);



                EC5.IG2.Plugin.Custom.InputOutExcelPlugin ePlugin = new EC5.IG2.Plugin.Custom.InputOutExcelPlugin();

                LModelList<LModel> models = (LModelList<LModel>)this.storeProd1.GetList();

                ePlugin.InputOutExcelFile(41, models, filePath);


                string downloa = "Mini2.create('Mini2.ui.extend.DownloadWindow', {fileName: '下载 Excel 文件',fielUrl:'" + urlPaht + "'}).show();";

                EasyClick.Web.Mini.MiniHelper.Eval(downloa);

            }
            catch (Exception ex)
            {
                log.Error("导出Excel文件出错了！", ex);

                MessageBox.Alert("导出 Excel 文件错误");

            }




        }

        /// <summary>
        /// 转移客户数据
        /// </summary>
        public void GoTransform()
        {


            string ut_001_id = storeMain1.CurDataId;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm001 = decipher.GetModelByPk("UT_001", ut_001_id);

            //创建入库单
            CreateIData(lm001);
            //创建出库单
            CreatetOData(lm001);


            UpdataNum((int)lm001.GetPk());


            LightModelFilter lmFilter002 = new LightModelFilter("UT_002");
            lmFilter002.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter002.And("COL_1", lm001.GetPk());
            lmFilter002.And("IO_TAG", "转移");

            //decipher.DeleteModels(lmFilter002);

            //decipher.DeleteModel(lm001);


            EasyClick.Web.Mini.EcView.close();

        }





        void CreateIData(LModel lm001)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lmNew001 = new LModel("UT_001");

            lm001.CopyTo(lmNew001);
            lmNew001["ROW_DATE_CREATE"] = lmNew001["ROW_DATE_UPDATE"] = lmNew001["COL_3"] = DateTime.Now;
            lmNew001["IO_TAG"] = "I";

            decipher.InsertModel(lmNew001);


            LightModelFilter lmFilter002 = new LightModelFilter("UT_002");
            lmFilter002.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter002.And("COL_1", lm001.GetPk());
            lmFilter002.And("IO_TAG", "转移");

            List<LModel> lm002s = decipher.GetModelList(lmFilter002);

            List<LModel> lmNew002s = new List<LModel>();

            foreach (LModel lm002 in lm002s)
            {

                LModel lmNew002 = new LModel("UT_002");

                lm002.CopyTo(lmNew002);

                lmNew002["ROW_DATE_CREATE"] = lmNew002["ROW_DATE_UPDATE"] = DateTime.Now;
                lmNew002["IO_TAG"] = "I";
                lmNew002["COL_1"] = lmNew001.GetPk();



                lmNew002s.Add(lmNew002);

            }

            decipher.BeginTransaction();

            try
            {


                decipher.InsertModels<LModel>(lmNew002s);

                decipher.TransactionCommit();
    

            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error(ex);

            }


        }

        void CreatetOData(LModel lm001)
        {

      

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lmOld001 = decipher.GetModelByPk("UT_001", lm001.Get<int>("COL_11"));

            

            LModel lmNew001 = new LModel("UT_001");

            lm001.CopyTo(lmNew001);
            lmNew001["ROW_DATE_CREATE"] = lmNew001["ROW_DATE_UPDATE"] = lmNew001["COL_3"] = DateTime.Now;
            lmNew001["IO_TAG"] = "O";
            lmNew001["CLIENT_NAME"] = lmOld001["CLIENT_NAME"];

            decipher.InsertModel(lmNew001);


            LightModelFilter lmFilter002 = new LightModelFilter("UT_002");
            lmFilter002.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter002.And("COL_1", lm001.GetPk());
            lmFilter002.And("IO_TAG", "转移");

            List<LModel> lm002s = decipher.GetModelList(lmFilter002);

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


                lmNew002s.Add(lmNew002);

            }

            decipher.BeginTransaction();

            try
            {


                decipher.InsertModels<LModel>(lmNew002s);

                decipher.TransactionCommit();


            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error(ex);

            }


        }









    }
}