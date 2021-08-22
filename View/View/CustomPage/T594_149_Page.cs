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
using EC5.IG2.BizBase;

namespace App.InfoGrid2.View.CustomPage
{
    /// <summary>
    /// 应收应付管理/付款单
    /// </summary>
    public class T594_149_Page : ExPage
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

            if (!IsPostBack)
            {




                string regJs = "function tijiao(e){ " +
                    "if(e.result != 'ok'){return;};  \n" +
                    "widget1.subMethod('form:first',{ subName:'ExPage', subMethod:'DaoRu_Step2', commandParam: e.ids || ''  });" +
                        "}";

                ScriptManager scr = ScriptManager.GetManager(this.Page);

                scr.AddScript(regJs);



            }

            
                Toolbar tbar = this.FindControl("toolbarT2577") as Toolbar;

                ToolBarButton btn = new ToolBarButton("勾兑应付款单据");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);

                ToolBarButton atuoMoney = new ToolBarButton("自动付款");
                //atuoMoney.OnClick = GetMethodJs("AtuoMoney");

                atuoMoney.OnClick = " var money = prompt('请输入金额');" +
                                     "widget1.subMethod('form:first',{ subName:'ExPage', subMethod:'AutoReceivables', commandParam: money || 0  });";

                tbar.Items.Add(atuoMoney);
            
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

            string COL_12 = item.Fields["COL_12"].Value;      //客户代码


            Window win = new Window("导入");
            win.State = WindowState.Max;


            string filter2 = Base64Util.ToString("[{field:'UT_185.COL_15',logic:'=',value:'" + COL_12 + "'}]", Base64Mode.Http);   //把过滤条件传过去


            win.ContentPath = string.Format("/App/InfoGrid2/View/MoreView/DataImportDialog.aspx?id=684&filter2={0}", filter2);

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


            int mapId = 51;       //这是映射规则ID
            int dialogId = 684;   //弹出界面ID

            string pField = "COL_42";

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

        /// <summary>
        /// 自动收款
        /// </summary>
        public void AutoReceivables(string moneyStr)
        {
            decimal money;

            if (!decimal.TryParse(moneyStr, out money))
            {
                MessageBox.Alert("请输入正确的金额格式！");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            //	UT_125	收款单-表头  ID
            int id125 = int.Parse(this.MainStore.CurDataId);

            LightModelFilter lmFilter = new LightModelFilter("UT_126");
            lmFilter.And("COL_42", id125);
            lmFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            //COL_21  未收金额    COL_24  现收金额
            lmFilter.TSqlOrderBy = "COL_41 asc";

            List<LModel> lm126List = decipher.GetModelList(lmFilter);


            decimal num = 0;

            lm126List.ForEach(m => num += m.Get<decimal>("COL_29"));

            if (money > num)
            {

                string popJs = "Mini2.Msg.confirm('提示','" + string.Format("现收金额大于应付款！点击确定，将核销{0}元。多出{1}元可在预付款单手工录入！", num, money - num) + "'," +
                    "function(){ widget1.subMethod('form:first',{ subName:'ExPage', subMethod:'AoutOk', commandParam: " + money + " });" +
                    "})";

                EasyClick.Web.Mini.MiniHelper.Eval(popJs);


                //MessageBox.Alert(string.Format("现收金额大于应收款！点击确定，将核销{0}元。多出{1}元可在预收款单手工录入！",num,money - num));
                return;
            }

            AoutOk(money);

        }

        /// <summary>
        /// 点击确定执行
        /// </summary>
        /// <param name="money"></param>
        public void AoutOk(decimal money)
        {


            DbDecipher decipher = ModelAction.OpenDecipher();

            //	UT_125	收款单-表头  ID
            int id125 = int.Parse(this.MainStore.CurDataId);

            LightModelFilter lmFilter = new LightModelFilter("UT_126");
            lmFilter.And("COL_42", id125);
            lmFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            //COL_21  未收金额    COL_24  现收金额
            lmFilter.TSqlOrderBy = "COL_41 asc";

            List<LModel> lm126List = decipher.GetModelList(lmFilter);

            decipher.BeginTransaction();


            try
            {




                lm126List.ForEach(m =>
                {
                    m.SetTakeChange(true);

                    m["COL_31"] = 0;
                    DbCascadeRule.Update(m);

                });


                foreach (var lm in lm126List)
                {
                    if (money - lm.Get<decimal>("COL_29") < 0)
                    {

                        lm["COL_31"] = money;


                        DbCascadeRule.Update(lm);
                        //decipher.UpdateModelProps(lm, "COL_24");

                        break;
                    }


                    lm["COL_31"] = lm["COL_29"];

                    money -= lm.Get<decimal>("COL_29");


                    DbCascadeRule.Update(lm);

                }

                decipher.TransactionCommit();

            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("自动收款出错了！", ex);
                MessageBox.Alert("出错了！");
                return;
            }


            this.MainStore.SetCurrntForId(this.MainStore.CurDataId);

            MessageBox.Alert("收款完成了！");
        }


    }
}