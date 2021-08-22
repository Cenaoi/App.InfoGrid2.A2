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
using EC5.IG2.Core;

namespace App.InfoGrid2.View.CustomPage.v2
{
    /// <summary>
    /// 采购管理 / 采购退货单 副本
    /// </summary>
    public class T515_182_Page_V2 : ExPage
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

           

                Toolbar tbar = this.FindControl("toolbarT518") as Toolbar;

                //	UT_164	采购退货单-产品明细
                Store st164 = this.FindControl("Store_UT_164") as Store;


                ToolBarButton btn = new ToolBarButton("导入");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);
            

            
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

            string COL_11 = item.Fields["COL_11"].Value;      //客户代码


            Window win = new Window("导入");
            win.State = WindowState.Max;


            string filter2 = Base64Util.ToString("[{field:'UT_161.COL_11',logic:'=',value:'" + COL_11 + "'}]", Base64Mode.Http);   ///把过滤条件传过去


            win.ContentPath = string.Format("/App/InfoGrid2/View/MoreView/DataImportDialog.aspx?id=680&filter2={0}", filter2);

            //win.ContentPath = "/App/InfoGrid2/View/Biz/JLSLBZ/WWJGFLD/FormDcgqd.aspx";
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


            int mapId = 55;         //这是映射规则ID
            int dialogId = 680;   //弹出界面ID


            string pField = "COL_1";
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