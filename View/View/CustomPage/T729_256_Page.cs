using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using System;
using System.Collections.Generic;
using System.Web;

namespace App.InfoGrid2.View.CustomPage
{
    /// <summary>
    /// 研发中心 / BOM物料配方
    /// </summary>
    public class T729_256_Page : ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnLoad()
        {

            string regJs = "function tijiao(e){ " +
                "if(e.result != 'ok'){return;};  \n" +
                //"widget1.subMethod('form:first',{ subName:'ExPage', subMethod:'DaoRu_Step2', commandParam: e.ids || ''  });" +
                    "}";

            EasyClick.Web.Mini.MiniHelper.Eval(regJs);

            

                //主表的工具栏
                Toolbar tbar = this.FindControl("toolbarT736") as Toolbar;

                //主表数据仓库
                Store st195 = this.FindControl("Store_UT_195") as Store;




                ToolBarButton btn = new ToolBarButton("树形结构");
                btn.OnClick = GetMethodJs("DaoRu");
                tbar.Items.Add(btn);

            
            
        }

        /// <summary>
        /// 树形结构
        /// </summary>
        /// <param name="name"></param>
        public void DaoRu()
        {
        
            string id = this.MainStore.CurDataId;

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择项目！");
                return;
            }


            Window win = new Window("树形结构");
            win.State = WindowState.Max;
            win.ContentPath = "/App/InfoGrid2/View/Biz/PopView/PopTree.aspx?id=" + id;
            win.FormClosedForJS = "tijiao"; //导入界面关闭后，执行的脚本函数名
            win.ShowDialog();

        }


    }
}