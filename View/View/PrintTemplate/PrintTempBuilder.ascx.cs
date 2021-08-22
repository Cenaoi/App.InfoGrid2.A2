using App.InfoGrid2.Bll.Sec;
using App.InfoGrid2.Model.SecModels;
using EasyClick.Web.Mini2;
using EC5.IG2.Core;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using System;
using App.InfoGrid2.Excel_Template.V1;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model;
using HWQ.Entity.Filter;
using App.InfoGrid2.Model.DataSet;
using System.IO;
using EasyClick.Web.Mini2.Data;

namespace App.InfoGrid2.View.PrintTemplate
{
    /// <summary>
    /// 这是专门为复杂表选择的打印界面  
    /// </summary>
    public partial class PrintTempBuilder : WidgetControl, IView
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

            m_mainID = WebUtil.QueryInt("mainId");
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


            if (StringUtil.IsBlank(m_fFiled, m_mainPK, m_mainTabel, m_subTable) || m_mainID == 0)
            {
                Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
            }



            if (!IsPostBack)
            {


                InitData();

                this.store1.DataBind();
                this.store2.DataBind();


            }
        }


        /// <summary>
        /// 初始化，如果没有默认模板就生成一个默认模板，如果默认模板路径不存在，就重新生成一个新的默认模板
        /// </summary>
        void InitData()
        {


            #region  这个是手动权限的问题

            EcUserState user = EcContext.Current.User;

            //看看是否是设计师
            if (!user.Roles.Exist(IG2Param.Role.BUILDER))
            {

                //模板管理按钮隐藏
                btn1.Visible = false;

                //这是过滤权限的问题  等字段添加完了就把注释去掉
                UserSecritySet userSec = SecFunMgr.GetUserSecuritySet();

                string[] codes = userSec.ArrStructCode;


                Param p = new Param("ROW_STRUCE_CODE");

                p.SetInnerValue(codes);
                p.Logic = "in";


                store1.FilterParams.Add(p);
                store2.FilterParams.Add(p);

            }


            #endregion



            int m_pageID = WebUtil.QueryInt("pageID");


            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(BIZ_PRINT_TEMPLATE));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("PAGE_ID", m_pageID);
            lmFilter.And("TEMPLATE_NAME", "默认模板");

            BIZ_PRINT_TEMPLATE bpt = decipher.SelectToOneModel<BIZ_PRINT_TEMPLATE>(lmFilter);

            if (bpt == null)
            {

                CreateDefTemp();

                return;

            }

            string path = Server.MapPath(bpt.TEMPLATE_URL);

            if (File.Exists(path))
            {
                return;
            }

            decipher.DeleteModel(bpt);

            CreateDefTemp();

        }

        /// <summary>
        /// 创建默认模板
        /// </summary>
        void CreateDefTemp()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            

            //主表列信息
            TableSet ts = TableSet.SelectSID_0_5(decipher, m_mainTableID);

            //子表列信息
            TableSet tsSub =TableSet.SelectSID_0_5(decipher, m_subTableID);

  

            BIZ_PRINT_TEMPLATE bpt = new BIZ_PRINT_TEMPLATE()
            {
                MAIN_TABLE_ID = m_mainTableID,
                PAGE_ID = m_pageID,
                PAGE_TEXT = m_pageText,
                ROW_DATE_CREATE = DateTime.Now,
                ROW_DATE_UPDATE = DateTime.Now,
                ROW_SID = 0,
                SUB_TABLE_NAME = m_subTable,
                MAIN_TABLE_NAME = m_mainTabel,
                TABLE_NUMBER = 1,
                TEMPLATE_TYPE = "EXCEL",
                SUB_F_FIELD = m_fFiled,
                TEMPLATE_NAME = "默认模板"
            };

            string name = string.Format(
                "{0}_{1}_{2}_{3}_{4}.xls",
                m_pageText,
                m_pageID,
                bpt.BIZ_PRINT_TEMPLATE_ID,
                m_mainTabel,
                m_subTable
                );

            //生成文件名称 和 路径 和相对路径
            WebFileInfo wFile = new WebFileInfo("/PrintTemplate", name);
            wFile.CreateDir();

            TemplateUtilV1.CreateSingleTableTemp(wFile.PhysicalPath, tsSub, ts);

            bpt.TEMPLATE_URL = "/PrintTemplate/" + name;

            decipher.InsertModel(bpt);



        }



        /// <summary>
        /// 打印Excel模板
        /// </summary>
        public void btnPrint()
        {

            string PrintID = this.store1.CurDataId;
            string TemplateID = this.store2.CurDataId;

            if (string.IsNullOrEmpty(PrintID))
            {
                MessageBox.Alert("请选择打印机！");
                return;
            }

            if (string.IsNullOrEmpty(TemplateID))
            {
                MessageBox.Alert("请选择Excel模板！");
                return;
            }


            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok',ids:'" + PrintID + "|" + TemplateID + "',sub_table_name:'"+ m_subTable + "'});");

        }



        /// <summary>
        /// 显示管理模板界面
        /// </summary>
        public void showManage()
        {

            int pageID = WebUtil.QueryInt("pageID");


            string url = string.Format("/App/InfoGrid2/View/PrintTemplate/ManageTemplate.aspx?mainID={0}&pageID={1}&fFiled={2}&mainTableID={3}&subTableID={4}&mainPK={5}&mainTable={6}&subTable={7}&pageText={8}",
                m_mainID,
                m_pageID,
                m_fFiled,
                m_mainTableID,
                m_subTableID,
                m_mainPK,
                m_mainTabel,
                m_subTable,
                m_pageText
                );

            EasyClick.Web.Mini.MiniHelper.EvalFormat("ShowUrl('{0}')", url);


        }
    }
}