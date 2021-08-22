using System;
using System.Collections.Generic;
using System.Web;
using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.IG2.Core;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using System.Text;

namespace App.InfoGrid2.View.CustomPage.v2
{   
    /// <summary>
    /// 采购管理/采购订单 副本
    /// </summary>
    public class T328_155_Page_V2 : ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override void OnInit()
        {
            //TabPanel tabPanel = this.FindControl("tabs0") as TabPanel;
            //tabPanel.ButtonVisible = false;

        }
        ///显示库存按钮
        ToolBarButton btnMessage;
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

                Toolbar tbar = this.FindControl("toolbarT1228") as Toolbar;

                ToolBarButton btn = new ToolBarButton("导入");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);


            }
            else
            {

                Toolbar tbar = this.FindControl("toolbarT616") as Toolbar;

                //明细数据源
                Store st133 = this.FindControl("Store_UT_133") as Store;
                st133.CurrentChanged += st133_CurrentChanged;

                ToolBarButton btn = new ToolBarButton("导入");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);

                //显示库存按钮
                btnMessage = new ToolBarButton("");
                btnMessage.ID = "tbMessage";
                tbar.Items.Add(btnMessage);
            }

        }

       

        void st133_CurrentChanged(object sender, ObjectEventArgs e)
        {
            LModel lm = (LModel)e.Object;
            if (lm == null) { return; }
            //产品ID
            string col_52 = lm.Get<string>("COL_40");
            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilter = new LightModelFilter("UT_118");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_15", col_52);

            List<LModel> lmList118 = decipher.GetModelList(lmFilter);

            StringBuilder sb = new StringBuilder();
            //库存合计 
            decimal sum = 0;


            sb.Append("合计：{0}    ");

            foreach (var item in lmList118)
            {

                decimal col_9 = item.Get<decimal>("COL_9");

                sum += col_9;

                sb.Append(item.Get<string>("COL_11") + ":" + col_9.ToString("0.######") + "     ");

            }

            string message = string.Format(sb.ToString(), sum.ToString("0.######"));
            btnMessage.Text = message;
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

            if(string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择项目！");
                return;
            }




            Window win = new Window("导入");
            win.State = WindowState.Max;
            win.ContentPath = "/App/InfoGrid2/View/Biz/JLSLBZ/WWJGFLD/FormDcgqd.aspx";
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

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_103");
            lmFilter.And("ROW_IDENTITY_ID", ids, Logic.In);

            string id = this.MainStore.CurDataId;

            List<LModel> lm104List = decipher.GetModelList(lmFilter);


            List<LModel> lm133List = new List<LModel>();


            try
            {
                foreach (var lm103 in lm104List)
                {

                    var lm133 = new LModel("UT_133");

                    M2MapHelper.MapTable(lm103, 43, lm133);

                    lm133["COL_12"] = id;

                    lm133List.Add(lm133);

                }


                decipher.InsertModels<LModel>(lm133List);

                Store st133 = this.FindControl("Store_UT_133") as Store;

                st133.Refresh();

                MessageBox.Alert("导入成功！");


            }
            catch (Exception ex) 
            {

                log.Error(ex);
            }






        }
    }
}