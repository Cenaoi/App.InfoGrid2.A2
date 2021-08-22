using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.PrintTemplate
{
    /// <summary>
    /// 单表模板管理界面
    /// </summary>
    public partial class ManageSingleTableTemlate : System.Web.UI.UserControl
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 界面ID
        /// </summary>
        public int m_pageID = 0;

        /// <summary>
        /// 主表ID
        /// </summary>
        public int m_mainTableID = 0;

        /// <summary>
        /// 主表名称
        /// </summary>
        public string m_mainTable = string.Empty;


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            m_pageID = WebUtil.QueryInt("pageID");

            m_mainTableID = WebUtil.QueryInt("mainTableID");

            m_mainTable = WebUtil.Query("mainTable");

            this.store1.Deleting += new ObjectCancelEventHandler(store1_Deleting);

            if (!IsPostBack)
            {

                InitData();

                this.store1.DataBind();
            }
        }


        /// <summary>
        /// 初始化，如果没有默认模板就生成一个默认模板，如果默认模板路径不存在，就重新生成一个新的默认模板
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




            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(BIZ_PRINT_TEMPLATE));
            lmFilter.And("ROW_SID",0,Logic.GreaterThanOrEqual);
            lmFilter.And("PAGE_ID", m_pageID);
            lmFilter.And("TEMPLATE_NAME","默认模板");

            BIZ_PRINT_TEMPLATE bpt = decipher.SelectToOneModel<BIZ_PRINT_TEMPLATE>(lmFilter);

            if(bpt == null)
            {

                TemplateUtil.CreateDefaultTemplateBySingleTable(m_mainTableID, m_pageID, m_mainTable);

                return;

            }


            string path = Server.MapPath(bpt.TEMPLATE_URL);

            if (File.Exists(path))
            {
                return;
            }

            decipher.DeleteModel(bpt);

            TemplateUtil.CreateDefaultTemplateBySingleTable(m_mainTableID, m_pageID, m_mainTable);



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

                if(lm == null)
                {
                    return;
                }

                string url = lm["TEMPLATE_URL"].ToString();

                string path = Server.MapPath(url);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

            }
            catch (Exception ex)
            {
                log.Error("删除模板文件出错了！", ex);
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
                PAGE_TEXT = "单表界面",
                ROW_DATE_CREATE = DateTime.Now,
                ROW_DATE_UPDATE = DateTime.Now,
                ROW_SID = 0,
                SUB_TABLE_NAME = "子表名称",
                MAIN_TABLE_NAME = m_mainTable,
                TABLE_NUMBER = 0,
                TEMPLATE_TYPE = "EXCEL",
                SUB_F_FIELD = "ROW_IDENTITY_ID",
                TEMPLATE_NAME = "默认模板"
            };

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                decipher.InsertModel(bpt);

                string url = "/App/InfoGrid2/View/PrintTemplate/TemplateUpload.aspx?id=" + bpt.BIZ_PRINT_TEMPLATE_ID;

                EasyClick.Web.Mini.MiniHelper.EvalFormat("ShowUrl('{0}')", url);


            }
            catch (Exception ex)
            {
                log.Error("插入新模板数据出错了！", ex);
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

                if (bpt == null)
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
                log.Error("找不到上传模板数据！", ex);
                MessageBox.Alert("找不到上传模板数据！");

            }

        }

        /// <summary>
        /// 下载模板
        /// </summary>
        public void DowTemplate()
        {
            string id = this.store1.CurDataId;

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择要下载的模板！");
                return;
            }

            DataRecord dr = this.store1.GetDataCurrent();

            string url = dr.Fields["TEMPLATE_URL"].Value;


            EasyClick.Web.Mini.MiniHelper.EvalFormat("DonwloadShow('{0}','{1}')", "下载Excle模板", url);

        }


    }
}