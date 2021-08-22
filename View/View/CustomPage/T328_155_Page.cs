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
using App.InfoGrid2.View.V2;
using EC5.IG2.BizBase;

namespace App.InfoGrid2.View.CustomPage
{   
    /// <summary>
    /// 采购管理/采购订单
    /// </summary>
    public class T328_155_Page : ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override void OnInit()
        {
            //TabPanel tabPanel = this.FindControl("tabs0") as TabPanel;
            //tabPanel.ButtonVisible = false;

        }

        /// <summary>
        /// 明细数据源
        /// </summary>
        Store st133 = null;

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

           

                Toolbar tbar = this.FindControl("toolbarT616") as Toolbar;

                st133 = this.FindControl("Store_UT_133") as Store;
                st133.CurrentChanged += st133_CurrentChanged;
                st133.Updating += st133_Updating;

                ToolBarButton btn = new ToolBarButton("导入");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);




                //显示库存按钮
                btnMessage = new ToolBarButton("");
                btnMessage.ID = "tbMessage";
                tbar.Items.Add(btnMessage);
            

        }

        /// <summary>
        /// 明细tag 的 更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void st133_Updating(object sender, ObjectCancelEventArgs e)
        {
            LModel lm = (LModel)e.Object;

            if (lm == null)
            {
                return;
            }


            //板长
            decimal COL_29 = lm.Get<decimal>("COL_29");
            //长度
            decimal COL_30 = lm.Get<decimal>("COL_30");
            //计量单位
            string COL_5 = lm.Get<string>("COL_5");
            //需求量单位
            string COL_47 = lm.Get<string>("COL_47");
            //需求量
            decimal COL_46 = lm.Get<decimal>("COL_46");
            //数量
            decimal COL_7 = lm.Get<decimal>("COL_7");
            //密度
            decimal COL_27 = lm.Get<decimal>("COL_27");
            //厚度
            decimal COL_21 = lm.Get<decimal>("COL_21");
            //宽度
            decimal COL_28 = lm.Get<decimal>("COL_28");
            //主单位
            string COL_33 = lm.Get<string>("COL_33");
            //主数量
            decimal COL_20 = lm.Get<decimal>("COL_20");


            //计量单位值改变 就执行 需求量 换算 数量 公式 
            bool b = lm.GetBlemish("COL_5");

            MyJosn json = null;

            if (b)
            {


                //这是 需求量 换算 数量 
                json = UnitUtil.UnitConversion(lm, COL_5, COL_7, COL_30, COL_27, COL_21, COL_28, COL_29, COL_47, COL_46, "COL_7", "COL_30");


                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);

                    return;
                }

                COL_7 = lm.Get<decimal>("COL_7");
            }


            if ((lm.GetBlemish("COL_7") || b) && COL_7 !=0)
            {

                // 这是 数量 换算 主数量
                json = UnitUtil.UnitConversion2(lm, COL_5, COL_33, COL_7, COL_30, COL_20, COL_27, COL_21, COL_28, COL_29, "COL_20", "COL_30");


                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);
                }
            }
            string id = lm.GetPk().ToString();

            st133.SetRecordValue(id, "COL_20", lm["COL_20"]);
            st133.SetRecordValue(id, "COL_30", lm["COL_30"]);
            st133.SetRecordValue(id, "COL_7", lm["COL_7"]);




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

            LightModelFilter lmFilter = new LightModelFilter("UT_216");
            lmFilter.And("ROW_IDENTITY_ID", ids, Logic.In);

            string id = this.MainStore.CurDataId;

            List<LModel> lm216List = decipher.GetModelList(lmFilter);


            List<LModel> lm133List = new List<LModel>();


            try
            {
                foreach (var lm216 in lm216List)
                {

                    var lm133 = new LModel("UT_133");

                    M2MapHelper.MapTable(lm216, 79, lm133);

                    lm133["COL_12"] = id;

                    lm133List.Add(lm133);

                }




                #region 联动对象

                DbCascadeFactory dbCascadeFactory = new DbCascadeFactory();
                dbCascadeFactory.ExecEnd += delegate(object dcSnder, DbCascadeEventArges dcE)
                {
                    EC5.BizLogger.LogStepMgr.Insert(dcE.Steps[0], dcE.OpText, dcE.Remark);
                };

                #endregion

                Store st133 = this.FindControl("Store_UT_133") as Store;


                lm133List.ForEach(m =>
                    {
                        m.SetTakeChange(true);
                        m.SetBlemishAll(true);

                        //单元格公式
                        LCodeFactory lcFactory = new LCodeFactory();
                        lcFactory.ExecLCode(st133, m);

                        //简单流程
                        LCodeValueFactory lcvFactiry = new LCodeValueFactory();
                        lcvFactiry.ExecLCode(st133, m);

                        decipher.InsertModel(m);

                        dbCascadeFactory.Inserted(st133, m);
                    });



               

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