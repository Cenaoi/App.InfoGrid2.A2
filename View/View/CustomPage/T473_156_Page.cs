using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model.DataSet;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using System;
using System.Collections.Generic;
using System.Web;

namespace App.InfoGrid2.View.CustomPage
{
    /// <summary>
    /// 采购管理 / 采购订单明细查询
    /// </summary>
    public class T473_156_Page : ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnLoad()
        {
            //StringBuilder sb = new StringBuilder();

            //foreach (var item in this.Controls)
            //{
            //    sb.AppendLine(item.ID);   
            //}


            string regJs = "function tijiao(e){ " +
                "if(e.result != 'ok'){return;};  \n" +
                "widget1.subMethod('form:first',{ subName:'ExPage', subMethod:'DaoRu_Step2', commandParam: e.ids || ''  });" +
                    "}";

            EasyClick.Web.Mini.MiniHelper.Eval(regJs);



            Toolbar tbar = this.FindControl("Toolbar1") as Toolbar;

 
            ToolBarButton btnPrint = new ToolBarButton("打印");

            btnPrint.OnClick = GetMethodJs("DaoRu", "你好");
            tbar.Items.Add(btnPrint);


            int id = WebUtil.QueryInt("id", 2);

  

            DbDecipher decipher = ModelAction.OpenDecipher();


            ViewSet m_ViewSet = ViewSet.Select(decipher, id);

            List<string> fields = new List<string>();


            StoreTSqlQuerty tSql = this.MainStore.TSqlQuery;

            tSql.Enabeld = true;
            tSql.Select = ViewMgr.GetTSqlSelect(m_ViewSet, ref fields);
            tSql.Form = ViewMgr.GetTSqlForm(m_ViewSet);
            tSql.Where = ViewMgr.GetTSqlWhere(m_ViewSet);
            tSql.OrderBy = ViewMgr.GetTSqlOrder(m_ViewSet, fields);




        }

    }
}