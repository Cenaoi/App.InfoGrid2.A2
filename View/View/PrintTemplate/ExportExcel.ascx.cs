using App.BizCommon;
using App.InfoGrid2.Excel_Template.V1;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.PrintTemplate
{
    public partial class ExportExcel : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


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

            m_fFiled = WebUtil.Query("fFiled");
            m_mainPK = WebUtil.Query("mainPK");
            m_mainTabel = WebUtil.Query("mainTable");
            m_pageText = WebUtil.Query("pageText");
            m_subTable = WebUtil.Query("subTable");

            if (m_subTableID == 0 || m_pageID == 0 || m_mainID == 0 || m_mainTableID == 0)
            {
                Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
            }

            if (StringUtil.IsBlank(m_fFiled, m_mainPK, m_mainTabel, m_subTable) || m_mainID == 0)
            {
                Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
            }


            if (!IsPostBack)
            {
                InitData();
            }
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {

                List<BIZ_PRINT_TEMPLATE> bptList = decipher.SelectModels<BIZ_PRINT_TEMPLATE>("PAGE_ID={0} and ROW_SID >=0", m_pageID);


                foreach (BIZ_PRINT_TEMPLATE item in bptList)
                {
                    this.cbxTemplate.Items.Add(item.TEMPLATE_URL, item.TEMPLATE_NAME);
                }

                if (bptList.Count > 0)
                {
                    this.cbxTemplate.Value = bptList[0].TEMPLATE_URL;
                    return;
                }

                CreateDefTemp();


            }
            catch (Exception ex)
            {
                log.Error("初始化数据失败了！", ex);
                Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
            }


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
            TableSet tsSub = TableSet.SelectSID_0_5(decipher, m_subTableID);



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

            this.cbxTemplate.Items.Add(bpt.TEMPLATE_URL, bpt.TEMPLATE_NAME);
            this.cbxTemplate.Value = bpt.TEMPLATE_URL;



        }


        /// <summary>
        /// 跳转到管理页面
        /// </summary>
        public void GoEdit()
        {
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

            MiniPager.Redirect(url);

        }

        /// <summary>
        /// 导出Excel文件
        /// </summary>
        public void InputOut()
        {

            string url = cbxTemplate.Value;

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Alert("请选择模板！");
                return;
            }


            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok',url:'"+url+"'});");


        }



     }
}