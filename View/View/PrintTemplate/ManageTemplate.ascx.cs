using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.Utility.Web;
using EasyClick.Web.Mini2.Data;
using App.InfoGrid2.Excel_Template;
using HWQ.Entity.LightModels;
using System.IO;
using EC5.Utility;
using EC5.SystemBoard;
using EC5.IG2.Core;

namespace App.InfoGrid2.View.PrintTemplate
{
    /// <summary>
    /// 这是上下表的模板管理界面
    /// </summary>
    public partial class ManageTemplate : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        #region ///页面传过来的属性

        /// <summary>
        /// 页面ID
        /// </summary>
        public int m_pageID;
        /// <summary>
        /// 页面名称
        /// </summary>
        public string m_pageText;

        /// <summary>
        /// 主表tableID
        /// </summary>
        public int m_mainTableID;

        /// <summary>
        /// 子表tableID
        /// </summary>
        public int m_subTableID;

        /// <summary>
        /// 主表本身ID
        /// </summary>
        public int m_mainID;
        /// <summary>
        /// 主表主键
        /// </summary>
        public string m_mainPK;
        /// <summary>
        /// 主表名称
        /// </summary>
        public string m_mainTabel;
        /// <summary>
        /// 子表名称
        /// </summary>
        public string m_subTable;
        /// <summary>
        /// 子表对应主表列
        /// </summary>
        public string m_fFiled;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            m_mainID = WebUtil.QueryInt("mainID");
            m_mainTableID = WebUtil.QueryInt("mainTableID");
            m_pageID = WebUtil.QueryInt("pageID");
            m_subTableID = WebUtil.QueryInt("subTableID");

            if (m_subTableID == 0 || m_pageID == 0 || m_mainID == 0 || m_mainTableID == 0)
            {
                Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
            }


            m_fFiled = WebUtil.Query("fFiled");
            m_mainPK = WebUtil.Query("mainPK");
            m_mainTabel = WebUtil.Query("mainTable");
            m_pageText = WebUtil.Query("pageText");
            m_subTable = WebUtil.Query("subTable");

            if (string.IsNullOrEmpty(m_fFiled) || string.IsNullOrEmpty(m_mainPK) || string.IsNullOrEmpty(m_mainTabel) || string.IsNullOrEmpty(m_subTable))
            {
                Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
            }


            this.store1.Deleting += new ObjectCancelEventHandler(store1_Deleting);

       

            if (!IsPostBack)
            {
                this.store1.DataBind();

                InitData();

            }


           


        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {


            EcUserState user = EcContext.Current.User;

            //判断是否是设计师
            if (user.Roles.Exist(IG2Param.Role.BUILDER))
            {
                var table_remaks = table1.Columns.FindByDataField("REMARKS");

                table_remaks.Visible = true;

            }





        }



        /// <summary>
        /// 删除模板数据顺便也把模板文件给删除了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void store1_Deleting(object sender, ObjectCancelEventArgs e)
        {
            try
            {
                LModel lm = (LModel)e.Object;

                string url = lm["TEMPLATE_URL"].ToString();

                string path = Server.MapPath(url);

                if(File.Exists(path))
                {
                    File.Delete(path);
                }

            }
            catch (Exception ex) 
            {
                log.Error("删除模板文件出错了！",ex);
            }


        }




        /// <summary>
        /// 显示上传界面
        /// </summary>
        public void InputTemplate() 
        {

            BIZ_PRINT_TEMPLATE bpt = new BIZ_PRINT_TEMPLATE()
            {
                MAIN_TABLE_ID = m_mainTableID,
                PAGE_ID = m_pageID,
                PAGE_TEXT = m_pageText,
                ROW_DATE_CREATE = DateTime.Now,
                ROW_DATE_UPDATE = DateTime.Now,
                ROW_SID = -1,
                SUB_TABLE_NAME = m_subTable,
                MAIN_TABLE_NAME = m_mainTabel,
                TABLE_NUMBER = 1,
                TEMPLATE_TYPE = "EXCEL",
                SUB_F_FIELD = m_fFiled
            };

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                decipher.InsertModel(bpt);

                string url = "/App/InfoGrid2/View/PrintTemplate/TemplateUpload.aspx?id="+bpt.BIZ_PRINT_TEMPLATE_ID;

                EasyClick.Web.Mini.MiniHelper.EvalFormat("ShowUrl('{0}')", url);


            }
            catch (Exception ex) 
            {
                log.Error("插入新模板数据出错了！",ex);
                MessageBox.Alert("插入新模板数据出错了！");
            }



        }

        /// <summary>
        /// 更新模板数据
        /// </summary>
        /// <param name="id"></param>
        public void UpdateData(string id) 
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                BIZ_PRINT_TEMPLATE bpt = decipher.SelectModelByPk<BIZ_PRINT_TEMPLATE>(id);

                if(bpt == null)
                {
                    MessageBox.Alert("找不到上传模板数据！");
                    return;
                }

                bpt.ROW_SID = 0;

                decipher.UpdateModelProps(bpt, "ROW_SID");


                this.store1.DataBind();


            }
            catch (Exception ex) 
            {
                log.Error("找不到上传模板数据！",ex);
                MessageBox.Alert("找不到上传模板数据！");
                
            }

        }


        /// <summary>
        /// 下载模板
        /// </summary>
        public void DowTemplate() 
        {
            string id = this.store1.CurDataId;

            if(string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择要下载的模板！");
                return;
            }

            DataRecord dr = this.store1.GetDataCurrent();

            string url = dr.Fields["TEMPLATE_URL"].Value;


            EasyClick.Web.Mini.MiniHelper.EvalFormat("DonwloadShow('{0}','{1}')", "下载Excle模板", url);

        }


        /// <summary>
        /// 导出Excel文件
        /// </summary>
        public void InputOut()
        {

            string id = this.store1.CurDataId;

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择要下载的模板！");
                return;
            }

            DataRecord dr = this.store1.GetDataCurrent();

            string url = dr.Fields["TEMPLATE_URL"].Value;




            DataSet ds = new DataSet();

            try
            {

                DbDecipher decipher = ModelAction.OpenDecipher();

                LightModelFilter mainFilter = new LightModelFilter(m_mainTabel);
                mainFilter.And(m_mainPK, m_mainID);
                mainFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                ///拿到主表数据
                ds.Head = decipher.GetModel(mainFilter);

                LightModelFilter subFilter = new LightModelFilter(m_subTable);
                subFilter.And(m_fFiled, m_mainID);
                subFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                ///拿到子表数据
                ds.Items = decipher.GetModelList(subFilter);

            }
            catch (Exception ex)
            {
                log.Error("查询数据出错了！", ex);
                MessageBox.Alert("查询数据出错了！");
                return;
            }

            try
            {

                string path = Server.MapPath(url);

                if(!File.Exists(path))
                {
                    MessageBox.Alert("模板路径不正确！");
                    return;
                }


                NOPIHandler handler = new NOPIHandler();

                SheetPro sp = handler.ReadExcel(path);

                handler.InsertSubData(sp, ds);


                WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel", FileUtil.NewFielname(".xls"));
                wFile.CreateDir();
                
                //保存Excel文件在服务器中
                handler.WriteExcel(sp, wFile.PhysicalPath);

                DownloadWindow.Show(wFile.Filename, wFile.RelativePath);

                //EasyClick.Web.Mini.MiniHelper.EvalFormat("DonwloadShow('{0}','{1}')", fileName + ".xls", "/_Temporary/Excel/" + fileName + ".xls");

            }
            catch (Exception ex)
            {
                log.Error("导出Excel文件出错了！", ex);
            }

        }



    }
}