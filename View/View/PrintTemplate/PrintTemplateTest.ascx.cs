using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Excel_Template;
using HWQ.Entity.LightModels;
using System.IO;
using EC5.Utility.Web;
using EC5.Utility;
using HWQ.Entity.Filter;
using EC5.SystemBoard;
using EC5.IG2.Core;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.Bll.Sec;

namespace App.InfoGrid2.View.PrintTemplate
{
    /// <summary>
    /// 这是上下表的打印界面啦
    /// </summary>
    public partial class PrintTemplateTest : WidgetControl, IView
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

                TemplateUtil.CreateDefaultTemplateByOneTable(m_mainTableID, m_subTableID, m_pageID, m_pageText, m_subTable, m_mainTabel, m_fFiled);

                return;

            }


            string path = Server.MapPath(bpt.TEMPLATE_URL);

            if (File.Exists(path))
            {
                return;
            }

            decipher.DeleteModel(bpt);

            TemplateUtil.CreateDefaultTemplateByOneTable(m_mainTableID, m_subTableID, m_pageID, m_pageText, m_subTable, m_mainTabel, m_fFiled);

        }


        /// <summary>
        /// 打印Excel模板
        /// </summary>
        public void btnPrint()
        {

            string PrintID = this.store1.CurDataId;
            string TemplateID = this.store2.CurDataId;

            if(string.IsNullOrEmpty(PrintID))
            {
                MessageBox.Alert("请选择打印机！");
                return;
            }

            if(string.IsNullOrEmpty(TemplateID))
            {
                MessageBox.Alert("请选择Excel模板！");
                return;
            }

          
          

          DataRecord printDr =  this.store1.GetDataCurrent();
          DataRecord templateDr = this.store2.GetDataCurrent();

          string url = templateDr.Fields["TEMPLATE_URL"].Value;

          string pathUrl = string.Empty;

          try
          {

              pathUrl = CreateExcelData(url);
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
                  PRINT_CODE = printDr.Fields["PRINT_CODE"].Value,
                  PRINT_NAME = printDr.Fields["PRINT_TEXT"].Value,
                  ROW_DATE_CREATE = DateTime.Now,
                  ROW_SID = 0
              };

              DbDecipher decipher = ModelAction.OpenDecipher();

              decipher.InsertModel(bpf);


              EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'yes'});");

          }
          catch (Exception ex) 
          {
              log.Error("插入打印数据出错了！",ex);
              MessageBox.Alert("打印出错了！");
          }


        }


        /// <summary>
        /// 生成打印Excel文件
        /// </summary>
        public string CreateExcelData( string url)
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


                if (ds.Head == null && ds.Items.Count > 0)
                {
                    ds.Head = ds.Items[0];
                }


            }
            catch (Exception ex)
            {
                
                throw new Exception("查询数据出错了！",ex);
                
            }

            try
            {



                NOPIHandlerEX handler = new NOPIHandlerEX();

                SheetPro sp = handler.ReadExcel(path);

                handler.InsertSubData(sp, ds);

                ///导出路径
                string mapath = MapPath("/_Temporary/Excel");

                ///判断文件夹是否存在
                if (!Directory.Exists(mapath))
                {
                    Directory.CreateDirectory(mapath);
                }

                ///文件名为当前时间时分秒都有
                string fileName = BillIdentityMgr.NewCodeForDay("PRINT","P",4);

                ///这是绝对物理路径
                string filePath = string.Format("{0}/{1}.xls", mapath, fileName);

                ///保存Excel文件在服务器中
                handler.WriteExcel(sp, filePath);
                handler.Dispose();

                url = "/_Temporary/Excel/" + fileName + ".xls";

                return url;

            }
            catch (Exception ex)
            {
                throw new Exception("生成打印 Excel 文件出错了！", ex);
            }

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