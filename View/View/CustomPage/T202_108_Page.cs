using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Bll;
using EC5.IG2.Core;

namespace App.InfoGrid2.View.CustomPage
{
    public class T202_108_Page : ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override void OnInit()
        {
            //TabPanel tabPanel = this.FindControl("tabs0") as TabPanel;
            //tabPanel.ButtonVisible = false;

        }

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


            if (PageType == "FORM")
            {




                Toolbar tbar = this.FindControl("toolbarT1224") as Toolbar;

                ToolBarButton btn = new ToolBarButton("导入");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);






            }else
            {
              




                Toolbar tbar = this.FindControl("toolbarT205") as Toolbar;

                ToolBarButton btn = new ToolBarButton("导入");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);

            }



      


               
            

        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="name"></param>
        public void DaoRu(string name)
        {
            //MessageBox.Alert("你好:" + name);

            //弹出导入界面

            string id = this.MainStore.CurDataId;

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择项目！");
                return;
            }



            var item = this.MainStore.GetDataCurrent();       ///拿到焦点行数据

            string COL_5 = item.Fields["COL_5"].Value;      //客户代码


            Window win = new Window("导入");
            win.State = WindowState.Max;


            string filter2 = Base64Util.ToString("[{field:'UT_096.COL_11',logic:'=',value:'" + COL_5 + "'}]", Base64Mode.Http);   ///把过滤条件传过去


            win.ContentPath = string.Format("/App/InfoGrid2/View/MoreView/DataImportDialog.aspx?id=678&filter2={0}", filter2);


            win.FormClosedForJS = "tijiao"; //导入界面关闭后，执行的脚本函数名

            win.ShowDialog();

        }

        /// <summary>
        /// 关闭导入窗口后，执行的语句
        /// </summary>
        /// <param name="checkeIds"></param>
        public void DaoRu_Step2(string checkeIds)
        {

            int[] ids = StringUtil.ToIntList(checkeIds);


            int mapId = 53;       //这是映射规则ID
            int dialogId = 678;   //弹出界面ID

            string pField = "COL_10";
            int parentId = StringUtil.ToInt(this.MainStore.CurDataId);

            try
            {
                BizHelper.ImportData(dialogId, mapId, ids, pField, parentId,true);

                this.MainStore.SetCurrntForId(this.MainStore.CurDataId);

                MessageBox.Alert("导入成功！");
            }
            catch (Exception ex)
            {
                log.Error("导入数据失败。", ex);
                MessageBox.Alert("导入失败。", ex.Message);
            }

        }

    }
}