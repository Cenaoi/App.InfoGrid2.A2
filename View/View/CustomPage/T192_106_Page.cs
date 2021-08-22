using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.IG2.Core;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using App.InfoGrid2.View.InputExcel;
using App.InfoGrid2.Bll;
using EC5.IG2.BizBase;

namespace App.InfoGrid2.View.CustomPage
{
    /// <summary>
    /// 销售管理/销售出货单
    /// </summary>
    public class T192_106_Page : ExPage
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


                //商品明细工具栏
                Toolbar tbar = this.FindControl("toolbarT1220") as Toolbar;

                ToolBarButton btn = new ToolBarButton("导入");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);


            }
            else
            {


                //商品明细的数据源
                Store st097 = this.FindControl("Store_UT_097") as Store;

                st097.CurrentChanged += st097_CurrentChanged;



                //商品明细工具栏
                Toolbar tbar = this.FindControl("toolbarT195") as Toolbar;




                ToolBarButton btn = new ToolBarButton("导入");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);



                //显示库存按钮
                btnMessage = new ToolBarButton("");
                btnMessage.ID = "tbMessage";
                tbar.Items.Add(btnMessage);

            }
            
        }

        void st097_CurrentChanged(object sender, ObjectEventArgs e)
        {
            LModel lm = (LModel)e.Object;
            if (lm == null) { return; }
            
            //产品ID
            int COL_21 = lm.Get<int>("COL_21");
            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilter = new LightModelFilter("UT_118");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_15", COL_21);

            List<LModel> lmList118 = decipher.GetModelList(lmFilter);

            StringBuilder sb = new StringBuilder();
            
            //库存合计 
            decimal sum = 0;


            sb.Append("合计：{0}    ");

            foreach (var item in lmList118)
            {

                decimal col9 = item.Get<decimal>("COL_9");

                sum += col9;

                sb.Append(item.Get<string>("COL_11") + ":" + col9.ToString("0.######") + "     ");

            }



            string numStr = sum.ToString("0.######");

            string message = string.Format(sb.ToString(), numStr);

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

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择项目！");
                return;
            }



            var  item  =  this.MainStore.GetDataCurrent();   //拿到焦点行数据

            string COL_10 = item.Fields["COL_10"].Value;      //客户ID


            Window win = new Window("导入");
            win.State = WindowState.Max;


            string tsqlParam = string.Concat("[{",
                @"p_type:'TSQL_WHERE', value:'UT_091.COL_34 = \'", COL_10, @"\''",
            "}]");


            string filter2 = Base64Util.ToString(tsqlParam, Base64Mode.Http);


            win.ContentPath = string.Format("/App/InfoGrid2/View/MoreView/DataImportDialog.aspx?id=677&filter2={0}", filter2);


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


            int mapId = 52;             //这是映射规则ID
            int dialogId = 677;         //弹出界面ID
            string pField = "COL_1";    //上级字段名
            
            int parentId = StringUtil.ToInt(this.MainStore.CurDataId);


            



            try
            {

                //BizHelper.ImportData(dialogId, mapId, ids, pField, parentId,true);


                IList<LModel> models = BizHelper.ImportData(dialogId, mapId, ids, pField, parentId, false);


                DbCascadeRule.Insert(models);


                this.MainStore.SetCurrntForId(this.MainStore.CurDataId);

                MessageBox.Alert("导入成功！");
            }
            catch (Exception ex)
            {
                log.Error("导入数据失败。",ex);
                MessageBox.Alert("导入失败。", ex.Message);
            }


        }


    }
}