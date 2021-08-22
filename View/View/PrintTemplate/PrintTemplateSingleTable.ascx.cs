using App.BizCommon;
using App.InfoGrid2.Bll.Sec;
using App.InfoGrid2.Excel_Template;
using App.InfoGrid2.Excel_Template.V1;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model.SecModels;
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
    /// 这是单表选择打印机和模板界面   
    /// </summary>
    public partial class PrintTemplateSingleTable : System.Web.UI.UserControl
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        /// <summary>
        /// 主表ID
        /// </summary>
        public int m_mainTableID;

        /// <summary>
        /// 主表表名
        /// </summary>
        public string m_mainTable;


        /// <summary>
        /// 主表主键
        /// </summary>
        public string m_mainPK;


        protected void Page_Load(object sender, EventArgs e)
        {

            m_mainTableID = WebUtil.QueryInt("mainTableID");
            m_mainPK = WebUtil.Query("mainPK");
            m_mainTable = WebUtil.Query("mainTable");

            InitData();

            if (!IsPostBack)
            {

                this.store1.DataBind();
                this.store2.DataBind();

            }
        }


        void InitData()
        {

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

            int pageID = WebUtil.QueryInt("pageID");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(BIZ_PRINT_TEMPLATE));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("PAGE_ID", pageID);
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

            int pageID = WebUtil.QueryInt("pageID");

            DbDecipher decipher = ModelAction.OpenDecipher();

            

            //主表列信息
            TableSet ts = TableSet.SelectSID_0_5(decipher, pageID);



            BIZ_PRINT_TEMPLATE bpt = new BIZ_PRINT_TEMPLATE()
            {
                MAIN_TABLE_ID = m_mainTableID,
                PAGE_ID = pageID,
                PAGE_TEXT = ts.Table.TABLE_NAME,
                ROW_DATE_CREATE = DateTime.Now,
                ROW_DATE_UPDATE = DateTime.Now,
                ROW_SID = 0,
                SUB_TABLE_NAME = "子表名称",
                MAIN_TABLE_NAME = m_mainTable,
                TABLE_NUMBER = 1,
                TEMPLATE_TYPE = "EXCEL",
                SUB_F_FIELD = "ROW_IDENTITY_ID",
                TEMPLATE_NAME = "默认模板"
            };


            string name = string.Format(
                    "{0}_{1}_{2}_{3}_{4}.xls",
                    ts.Table.TABLE_NAME,
                    pageID,
                    bpt.BIZ_PRINT_TEMPLATE_ID,
                    "主表名称",
                    "子表名称"
                    );

            //生成文件名称 和 路径 和相对路径
            WebFileInfo wFile = new WebFileInfo("/PrintTemplate", name);
            wFile.CreateDir();

            TemplateUtilV1.CreateSingleTableTemp(wFile.PhysicalPath, ts);

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



            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok',ids:'"+PrintID+"|"+TemplateID+"'});");


        }




        /// <summary>
        /// 显示管理模板界面
        /// </summary>
        public void showManage()
        {

            int pageID = WebUtil.QueryInt("pageID");

            string url = $"/App/InfoGrid2/View/PrintTemplate/ManageSingleTableTemlate.aspx?pageID={pageID}&mainTableID={m_mainTableID}&mainTable={m_mainTable}";

            EasyClick.Web.Mini.MiniHelper.EvalFormat("ShowUrl('{0}')", url);


        }




    }
}