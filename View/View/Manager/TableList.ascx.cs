using System;
using System.Collections.Generic;
using System.Web;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EasyClick.Web.Mini2;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.Utility.Web;
using EasyClick.Web.Mini2.Data;
using EC5.Utility;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using HWQ.Entity;
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity.Xml;
using System.Text;
using System.IO;
using App.InfoGrid2.Bll;
using System.Reflection;
using EC5.AppDomainPlugin;
using EC5.WScript;
using EC5.Action3.CodeProcessors;
using System.Diagnostics;
using EC5.Action3;

namespace App.InfoGrid2.View.Manager
{
    public partial class TableList : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }


        /// <summary>
        /// 新表格
        /// </summary>
        public void GoNewTable()
        {
            int catalogId  = WebUtil.QueryInt("catalog_id");

            DbDecipher decipher = ModelAction.OpenDecipher();
            IG2_CATALOG cata = decipher.SelectModelByPk<IG2_CATALOG>(catalogId);



            if (cata.DEFAULT_TABLE_TYPE == "PAGE")
            {
                string url = "/app/InfoGrid2/view/OnePage/StepNew1.aspx?catalog_id=" + catalogId;

                EcView.Show(url, "创建复杂表");                
            }
            else if (cata.DEFAULT_TABLE_TYPE == "VIEW")
            {
                string url = "/app/InfoGrid2/view/OneView/StepNew2.aspx?catalog_id=" + catalogId;
                EcView.Show(url, "创建视图表");
            }
            else if (cata.DEFAULT_TABLE_TYPE == "MORE_VIEW")
            {
                string url = "/app/InfoGrid2/view/MoreView/MViewStepNew1.aspx?catalog_id=" + catalogId;
                EcView.Show(url, "创建视图表");
            }
            else if (cata.DEFAULT_TABLE_TYPE == "CROSS_TABLE")
            {
                string url = "/App/InfoGrid2/View/ReportBuilder/NewStep1.aspx?catalog_id=" + catalogId;
                EcView.Show(url, "创建交叉报表");
            }
            else
            {
                string url = "/app/infogrid2/view/onetable/StepNew1.aspx?catalog_id=" + catalogId;
                EcView.Show(url, "创建工作表");
            }

        }

        /// <summary>
        /// 节点转移
        /// </summary>
        /// <param name="nodeId"></param>
        public void GoNodeMove(int nodeId)
        {
            if (this.table1.CheckedRows.Count == 0)
            {
                return;
            }

            if (nodeId <= 0)
            {
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            foreach (DataRecord item in this.table1.CheckedRows)
            {
                IG2_TABLE table = decipher.SelectModelByPk<IG2_TABLE>(StringUtil.ToInt(item.Id));
                table.IG2_CATALOG_ID = nodeId;

                decipher.UpdateModelProps(table, "IG2_CATALOG_ID");

                this.store1.Remove(item);
            }
        }

        /// <summary>
        /// 验证数据结构
        /// </summary>
        public void GoValidStruce()
        {
            try
            {
                string file =  SysValidateMgr.TableStruceValids();

                MessageBox.Alert("验证完成，报告保存在 " + file);
            }
            catch (Exception ex)
            {
                log.Error("验证过程失败", ex);

                MessageBox.Alert("验证过程失败.");
            }
        }

        /// <summary>
        /// 验证视图表的结构
        /// </summary>
        public void GoValidViewStruce()
        {
            try
            {
                string file = SysValidateMgr.ViewStruceValids();

                MessageBox.Alert("验证完成，报告保存在 " + file);
            }
            catch (Exception ex)
            {

                log.ErrorFormat("验证过程失败", ex);

                MessageBox.Alert("验证过程失败.");
            }
        }


        /// <summary>
        /// 根据数据库里面的表创建自定义表
        /// </summary>
        public void CreateTable()
        {

            int catalogId = WebUtil.QueryInt("catalog_id");

            EcView.Show("/App/InfoGrid2/View/OneTable/CreateNew1.aspx?catalog_id="+ catalogId,"创建自定义表");


        }
        

        public void GoTableMove()
        {
            Window win = new Window("选择",400,600);
            win.ContentPath = "/App/InfoGrid2/View/Manager/SelectTreeCatalog.aspx";

            win.WindowClosed += Win_WindowClosed1;

            win.ShowDialog();
        }

        protected void Win_WindowClosed1(object sender, string data)
        {
            DynSModel e = DynSModel.ParseJson(data);

            if(e["result"] != "ok")
            {
                return;
            }

            int nodeId = (int)e["node_id"];

            GoNodeMove(nodeId);

        }


        /// <summary>
        /// 批量增删改表
        /// </summary>
        public void GoOpTableBatch()
        {

            string url = $"/App/InfoGrid2/View/OneTable/OpTableBatchList.aspx";

            EcView.Show(url, "批量修改表字段");
            
        }


        /// <summary>
        /// 测试联动v3
        /// </summary>
        public void Test_Ac3()
        {

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen();



            //List<TG_ORDER> orderList = new List<TG_ORDER>();

            //for (int i = 0; i < 2; i++)
            //{
            //    TG_ORDER order = new TG_ORDER();
            //    order.TITLE = "核武器";

            //    orderList.Add(order);
            //}

            dynamic order2 = new LModel("TG_ORDER");
            //order2.ORDER_ID = 3;
            order2["CODE"] = 32;
            order2["TITLE"] = "核武器";

            dynamic dd = VBA.FIRST(order2).ORDER_ID - 2;

            //dynamic ss = new TG_ORDER();
            //ss.TITLE = "3232";



            //////////////////////////////////////////////////////////////////////////

            DrawingLibrary lib = LibraryManager.GetDefault();


            CodeContext context = new CodeContext();
            context.Library = lib;
            context.Decipher = decipher;

            context.CurParams = new SModel();
            context.CurParams["decipher"] = decipher;

            CodeProcess process = new CodeProcess(context);

            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                process.Exec(order2, OperateMethod.Insert);

                sw.Stop();

                log.Debug($"总运行时间: {sw.Elapsed.TotalMilliseconds:#,###.000} 毫秒");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                decipher.Dispose();
            }
        }

    }
}