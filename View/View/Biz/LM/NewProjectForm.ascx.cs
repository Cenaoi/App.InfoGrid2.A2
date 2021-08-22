using App.BizCommon;
using App.InfoGrid2.Excel_Template.V1;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.Entity.Expanding.ExpandV1;
using EC5.IG2.BizBase;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.LM
{
    public partial class NewProjectForm : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

            StoreUT090.CurrentChanged += StoreUT090_CurrentChanged;

            StoreUT091_2.Inserting += StoreUT091_2_Inserting;
            StoreUT091_2.Deleting += StoreUT091_2_Deleting;

            StoreUT091_3.Inserting += StoreUT091_3_Inserting;
            StoreUT091_3.Deleting += StoreUT091_3_Deleting;

            StoreUT091_4.Inserting += StoreUT091_4_Inserting;
            StoreUT091_4.Deleting += StoreUT091_4_Deleting;

            StoreUT091_5.Inserting += StoreUT091_5_Inserting;
            StoreUT091_5.Inserting += StoreUT091_5_Inserting1;

            StoreUT091_6.Inserting += StoreUT091_6_Inserting;
            StoreUT091_6.Deleting += StoreUT091_6_Deleting;



            if (!IsPostBack)
            {
                StoreUT090.DataBind();
            }
        }

        private void StoreUT091_6_Deleting(object sender, ObjectCancelEventArgs e)
        {
            InsertBefore(e, "删除");
        }

        private void StoreUT091_5_Inserting1(object sender, ObjectCancelEventArgs e)
        {
            InsertBefore(e, "删除");
        }

        private void StoreUT091_4_Deleting(object sender, ObjectCancelEventArgs e)
        {
            InsertBefore(e, "删除");
        }

        private void StoreUT091_3_Deleting(object sender, ObjectCancelEventArgs e)
        {
            InsertBefore(e, "删除");
        }

        private void StoreUT091_2_Deleting(object sender, ObjectCancelEventArgs e)
        {
            InsertBefore(e, "删除");
        }
    

        private void StoreUT091_6_Inserting(object sender, ObjectCancelEventArgs e)
        {
            InsertBefore(e,"新增");

            LModel lm = e.Object as LModel;

            DefValue(lm, e);
        }

        private void StoreUT091_5_Inserting(object sender, ObjectCancelEventArgs e)
        {
            InsertBefore(e, "新增");

            LModel lm = e.Object as LModel;

            DefValue(lm, e);
        }

        private void StoreUT091_4_Inserting(object sender, ObjectCancelEventArgs e)
        {
            InsertBefore(e, "新增");

            LModel lm = e.Object as LModel;

            DefValue(lm, e);
        }

        private void StoreUT091_3_Inserting(object sender, ObjectCancelEventArgs e)
        {
            

            InsertBefore(e, "新增");

            LModel lm = e.Object as LModel;

            DefValue(lm, e);
        }

        private void StoreUT091_2_Inserting(object sender, ObjectCancelEventArgs e)
        {
            LModel lm = e.Object as LModel;

            InsertBefore(e, "新增");

            DefValue(lm, e);


        }

        private void StoreUT090_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {

            StoreUT091_1.DataBind();
            StoreUT091_2.DataBind();
            StoreUT091_3.DataBind();
            StoreUT091_4.DataBind();
            StoreUT091_5.DataBind();
            StoreUT091_6.DataBind();

        }


        /// <summary>
        /// 提交按钮点击事件
        /// </summary>
        public void GoChangeBizSid0_2()
        {

            ChangeBizSid(0, 2, "提交成功了");

        }


        /// <summary>
        /// 审核按钮点击事件
        /// </summary>
        public void GoChangeBizSid2_4()
        {


            ChangeBizSid(2, 4, "审核成功了");

        }

        /// <summary>
        /// 撤销提交按钮点击事件
        /// </summary>
        public void GoChangeBizSid2_0()
        {
            ChangeBizSid(2, 0, "撤销提交成功了");

        }


        /// <summary>
        /// 撤销审核按钮点击事件
        /// </summary>
        public void GoChangeBizSid4_2()
        {

            ChangeBizSid(4, 2, "撤销审核成功了");

        }


        /// <summary>
        /// 显示打印机界面  新的写法了  新的打印Excel写法，好像还不能通用
        /// 小渔夫 
        /// 1206-12-09
        /// </summary>
        public void GoPrint()
        { 



            int pkValue = WebUtil.QueryInt("id");
            int pageId = 2371;


            int mainTableId = 8718;
            string mainTable = StoreUT090.Model;
            string mainPk = StoreUT090.IdField;

            int subTableId = 2415;
            string subTable = StoreUT091_1.Model;


            string url = "/App/InfoGrid2/View/PrintTemplate/PrintTempBuilder.aspx";

            NameValueCollection nv = new NameValueCollection(){
                {"id",pageId.ToString()},
                {"mainId",pkValue.ToString()},  //主数据记录的ID
                {"pageID",pageId.ToString()},

                {"fFiled","COL_12"},

                {"mainTableID",mainTableId.ToString()},  //表定义的ID
                {"mainTable",mainTable},    //表名
                {"mainPK",mainPk},

                {"subTableID",subTableId.ToString()},
                {"subTable",subTable},
                {"pageText","测试新项目订单"}
            };


            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < nv.Count; i++)
            {
                string key = nv.Keys[i];
                string value = nv[i];

                if (i > 0) { sb.Append("&"); }

                sb.Append(key).Append("=").Append(value);
            }



            string urlStr = url + "?" + sb.ToString();

            Window win = new Window("打印");
            win.StartPosition = WindowStartPosition.CenterScreen;
            win.ContentPath = urlStr;
            win.WindowClosed += Win_WindowClosed;
            win.ShowDialog();

        }


        /// <summary>
        /// 新的选择打印界面确定按钮的回调函数
        /// 小渔夫
        /// 2016-12-12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public void Win_WindowClosed(object sender, string data)
        {

            if (string.IsNullOrWhiteSpace(data))
            {
                return;
            }

            SModel sm = SModel.ParseJson(data);

            if (sm.Get<string>("result") != "ok")
            {
                return;
            }


            string[] idList = StringUtil.Split(sm.Get<string>("ids"), "|");

            if (idList.Length < 2)
            {
                MessageBox.Alert("请选择打印机和打印模板");
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();


            BIZ_PRINT bp = decipher.SelectModelByPk<BIZ_PRINT>(idList[0]);

            BIZ_PRINT_TEMPLATE bpt = decipher.SelectModelByPk<BIZ_PRINT_TEMPLATE>(idList[1]);


            if (bp == null || bpt == null)
            {
                MessageBox.Alert("请选择打印机和打印模板");
                return;
            }


            string pathUrl = string.Empty;

            try
            {

                pathUrl = CreateExcelData_2(bpt.TEMPLATE_URL);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Alert("打印出错了！" + ex.Message);
                return;
            }

            try
            {

                BIZ_PRINT_FILE bpf = new BIZ_PRINT_FILE()
                {
                    FILE_URL = pathUrl,
                    PRINT_CODE = bp.PRINT_CODE,
                    PRINT_NAME = bp.PRINT_TEXT,
                    ROW_DATE_CREATE = DateTime.Now,
                    ROW_SID = 0
                };

                decipher.InsertModel(bpf);

            }
            catch (Exception ex)
            {
                log.Error("插入打印数据出错了！", ex);
                MessageBox.Alert("打印出错了！");
            }

        }





        /// <summary>
        ///  小渔夫写的   
        /// 生成打印Excel文件
        /// 修改于 2017-03-13   可以打印多个子表
        /// </summary>
        string CreateExcelData_2(string url)
        {

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Alert("请选择模板！");
                return url;
            }

            string path = Server.MapPath(url);

            if (!File.Exists(path))
            {
                throw new Exception("模板文件不存在。");
            }



            DbDecipher decipher = ModelAction.OpenDecipher();


            try
            {

                //文件名为当前时间时分秒都有
                string fileName = BillIdentityMgr.NewCodeForDay("PRINT", "P", 4);
                WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel", fileName + ".xls");
                wFile.CreateDir();


                SheetParam sp = TemplateUtilV1.ReadTemp(path);


                MoreSubTableDataSet ds_v1 = new MoreSubTableDataSet();


                foreach (LModel item in StoreUT090.GetList())
                {
                    if (item.GetPk<string>() == StoreUT090.CurDataId)
                    {
                        ds_v1.Head = item;
                    }
                }


                // 拿到主表数据
                ds_v1.OneItems = StoreUT091_1.GetList() as List<LModel>;


                //这个可以打多个子表的  只能顺序打 打完一个子表再打下一个子表  不能合在一起打
                TemplateUtilV1.CreateExcel(sp, ds_v1, wFile.PhysicalPath);


                return wFile.RelativePath;

            }
            catch (Exception ex)
            {
                throw new Exception("生成打印 Excel 文件出错了！", ex);
            }

        }

        /// <summary>
        /// 通用改变业务状态方法
        /// </summary>
        /// <param name="old_biz_sid">当前状态值</param>
        /// <param name="biz_sid">改变状态值</param>
        /// <param name="msg">成功提示文字</param>
        void ChangeBizSid(int old_biz_sid, int biz_sid, string msg)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = WebUtil.QueryInt("id");


            LModel lm090 = decipher.GetModelByPk("UT_090", id);



            BizFilter filter = new BizFilter("UT_091");
            filter.And("COL_12", id);
            filter.And("BIZ_SID", old_biz_sid);


            List<LModel> lm091s = decipher.GetModelList(filter);

            if (lm090.Get<int>("BIZ_SID") != old_biz_sid)
            {
                MessageBox.Alert("当前状态不正确");
                return;
            }


            try
            {


                lm090.SetTakeChange(true);

                lm090["BIZ_SID"] = biz_sid;
                lm090["ROW_DATE_UPDATE"] = DateTime.Now;

                DbCascadeRule.Update(lm090);


                foreach(var item in lm091s)
                {
                    item.SetTakeChange(true);

                    item["BIZ_SID"] = biz_sid;
                    item["ROW_DATE_UPDATE"] = DateTime.Now;

                    DbCascadeRule.Update(lm090);

                }

                StoreUT090.Refresh();
                StoreUT091_1.Refresh();
                StoreUT091_2.Refresh();
                StoreUT091_3.Refresh();
                StoreUT091_4.Refresh();
                StoreUT091_5.Refresh();
                StoreUT091_6.Refresh();
                

                MessageBox.Alert(msg);


            }
            catch (Exception ex)
            {
                log.Error("改变业务状态出错了",ex);
                MessageBox.Alert("哦噢,改变业务状态出错了喔.");
            }

        }

        /// <summary>
        /// 子表插入数据之前和删除数据之前的前置条件
        /// </summary>
        /// <param name="e"></param>
        /// <param name="msg">新增或者删除</param>
        void InsertBefore(ObjectCancelEventArgs e,string msg)
        {

            //事件如果取消了就不用执行下面的代码了
            if (e.Cancel)
            {
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = WebUtil.QueryInt("id");

            LModel lm090 = decipher.GetModelByPk("UT_090", id);

            if (lm090.Get<int>("BIZ_SID") > 0)
            {
                e.Cancel = true;
                MessageBox.Alert($"已审核就不能{msg}数据了");
                return;
            }


        }

        void DefValue(LModel lm, ObjectCancelEventArgs e)
        {

            if (e.Cancel)
            {
                return;
            }

            lm["COL_102"] = 0;
            lm["COL_103"] = 0;
            lm["COL_104"] = 0;
            lm["COL_105"] = 0;
            lm["COL_7"] = 0;
            lm["COL_6"] = 0;
            lm["COL_8"] = 0;
            lm["COL_160"] = 0;
            lm["COL_48"] = 1;
            lm["COL_49"] = 0;
            lm["COL_136"] = 0;
            lm["COL_137"] = 0;
            lm["COL_164"] = 0;
            lm["COL_165"] = 0;
            lm["COL_12"] = WebUtil.QueryInt("id");
        }



    }
}