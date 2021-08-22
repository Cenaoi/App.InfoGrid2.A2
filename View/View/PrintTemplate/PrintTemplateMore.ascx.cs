using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Excel_Template;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.Bll.Sec;
using EC5.SystemBoard;
using EC5.IG2.Core;

namespace App.InfoGrid2.View.PrintTemplate
{
    public partial class PrintTemplateMore : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        #region  ///页面传过来的属性

        /// <summary>
        /// 主表表名
        /// </summary>
        public string m_mainTabel;

        /// <summary>
        /// 主表ID
        /// </summary>
        public int m_mainID;

        /// <summary>
        /// 主表主键
        /// </summary>
        public string m_mainPK;


        /// <summary>
        /// 子表表名
        /// </summary>
        public string m_subTable;

        /// <summary>
        /// 子表对应主表列字段
        /// </summary>
        public string m_fFiled;

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {


            InitData();


            if (!IsPostBack)
            {
                this.store1.DataBind();
                this.store2.DataBind();
            }
        }



        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {

            EcUserState user = EcContext.Current.User;

            //看看是否是设计师
            if (!user.Roles.Exist(IG2Param.Role.BUILDER))
            {

                //这是过滤权限的问题  等字段添加完了就把注释去掉
                UserSecritySet userSec = SecFunMgr.GetUserSecuritySet();

                string[] codes = userSec.ArrStructCode;


                Param p = new Param("ROW_STRUCE_CODE");

                p.SetInnerValue(codes);
                p.Logic = "in";


                store1.FilterParams.Add(p);
                store2.FilterParams.Add(p);

            }



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




            DataRecord printDr = this.store1.GetDataCurrent();
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
                log.Error("插入打印数据出错了！", ex);
                MessageBox.Alert("打印出错了！");
            }


        }


        /// <summary>
        /// 生成打印Excel文件
        /// </summary>
        public string CreateExcelData(string url)
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

            DataSet ds = new DataSet();

            try
            {

                #region  构造关联表的 sql 语句

                int id = WebUtil.QueryInt("id");

                ViewSet m_ViewSet = ViewSet.Select(decipher, id);

                List<string> fields = new List<string>();


                StringBuilder sb = new StringBuilder();

                sb.Append("select ");

                sb.Append(ViewMgr.GetTSqlSelect(m_ViewSet, ref fields));

                sb.Append(" from ");

                sb.Append(ViewMgr.GetTSqlForm(m_ViewSet));

                sb.Append(" where ");

                sb.Append(ViewMgr.GetTSqlWhere(m_ViewSet));

                sb.Append(" order by ");

                sb.Append(ViewMgr.GetTSqlOrder(m_ViewSet, fields));



                string sql = sb.ToString();

                #endregion

                ds.Items = decipher.GetModelList(sql);
 


                if (ds.Head == null && ds.Items.Count > 0)
                {
                    ds.Head = ds.Items[0];
                }


            }
            catch (Exception ex)
            {

                throw new Exception("查询数据出错了！", ex);

            }

            try
            {



                NOPIHandlerEX handler = new NOPIHandlerEX();

                SheetPro sp = handler.ReadExcel(path);

                handler.InsertSubData(sp, ds);



                //文件名为当前时间时分秒都有
                string fileName = BillIdentityMgr.NewCodeForDay("PRINT", "P", 4) + ".xls";

                WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel/", fileName);

                wFile.CreateDir();

                //保存Excel文件在服务器中
                handler.WriteExcel(sp, wFile.PhysicalPath);
                handler.Dispose();


                return wFile.RelativePath;

            }
            catch (Exception ex)
            {
                throw new Exception("生成打印 Excel 文件出错了！", ex);
            }

        }
    }
}