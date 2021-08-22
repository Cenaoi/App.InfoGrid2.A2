using App.BizCommon;
using App.InfoGrid2.View.Biz.PopView;
using App.InfoGrid2.View.InputExcel;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.InfoGrid2.View.CustomPage
{
    /// <summary>
    /// 销售管理/销售订单
    /// </summary>
    public partial class T136_Page : ExPage
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit()
        {
            //TabPanel tabPanel = this.FindControl("tabs0") as TabPanel;
            //tabPanel.ButtonVisible = false;


            if (!this.IsPostBack)
            {
                ScriptManager scr = ScriptManager.GetManager(this.Page);

                if (scr != null)
                {
                    string regJs = "function tijiao(e){ alert('参数 ids:' + e.ids); \n" +
                       "widget1.subMethod('form:first',{ subName:'ExPage', subMethod:'DaoRu_Step2', commandParam: e.ids || '0'  });" +
                           "}";

                    scr.AddScript(regJs);
                }
            }





            //产品明细的工具栏
            Toolbar tbar = this.FindControl("toolbarT139") as Toolbar;

            //产品明细的数据源
            Store st091 = this.FindControl("Store_UT_091") as Store;
            st091.CurrentChanged += st091_CurrentChanged;


            //ToolBarButton btn = new ToolBarButton("导入");
            //btn.OnClick = GetMethodJs("DaoRu", "你好");
            //tbar.Items.Add(btn);


            ToolBarButton tbarBom = new ToolBarButton("物料配方");
            tbarBom.OnClick = GetMethodJs("GetBomFormula");
            tbar.Items.Add(tbarBom);


            //显示库存按钮
            btnMessage = new ToolBarButton("");
            btnMessage.ID = "tbMessage";
            tbar.Items.Add(btnMessage);
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

        }

        void st091_CurrentChanged(object sender, ObjectEventArgs e)
        {
            LModel lm = e.Object as LModel;

            if (lm == null) { return; }
            
            //产品ID
            string col_52 = lm.Get<string>("COL_29");
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

                decimal col9 = item.Get<decimal>("COL_9");

                sum += col9;

                sb.Append(item.Get<string>("COL_11") + ":" + col9.ToString("0.######") + "     ");

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
            Window win = new Window("导入");

            win.FormClosedForJS = "tijiao"; //导入界面关闭后，执行的脚本函数名

            win.ShowDialog();

        }

        /// <summary>
        /// 关闭导入窗口后，执行的语句
        /// </summary>
        /// <param name="checkeIds"></param>
        public void DaoRu_Step2(string checkeIds)
        {
            MessageBox.Alert("导入成功: " + checkeIds);
        }

        /// <summary>
        /// 物料配方按钮点击事件
        /// </summary>
        public void GetBomFormula() 
        {
            ///产品明细的数据源
            Store st091 = this.FindControl("Store_UT_091") as Store;

            int id = int.Parse(st091.CurDataId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            MTable191Mgr mgr = new MTable191Mgr();

            //		UT_091	销售订单-产品明细
            LModel lm091 = decipher.GetModelByPk("UT_091", id);

            Window win = new Window("产品结构");
            win.ContentPath = "/App/InfoGrid2/View/Biz/BOM/BOM091Tree.aspx?id=" + id;
            win.State = WindowState.Max;

            if (lm091.Get<bool>("HAS_CHILD"))
            {
                win.ShowDialog();
                return;
            }

            //产品ID
            string COL_29 = lm091.Get<string>("COL_29");


            LightModelFilter lmFilter = new LightModelFilter("UT_195");
            lmFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            lmFilter.And("COL_1", COL_29); //产品ID

            //	UT_195	BOM主表-表头
            LModel lm195 = decipher.GetModel(lmFilter);

            if (lm195 == null)
            {
                MessageBox.Alert(string.Format("产品ID：{0}在BOM主表-表头中找不到！", COL_29));
                return;
            }
            //设置目标表名
            mgr.TargetTable = "UT_219";

            List<LModel> lmList096 = mgr.GetModels196(61, lm195.Get<int>("ROW_IDENTITY_ID"));


            decipher.IdentityStop();

            foreach (var item in lmList096)
            {

                item["COL_20"] = MainStore.CurDataId;
                item["UT_091_ID"] = id;


            }

            //这是拿上级信息放到自身
            lmList096.ForEach((lm) =>
            {
                lm["COL_24"] = lm091["COL_2"]; //订单号
                lm["COL_39"] = lm091["COL_42"]; //产品ID
                lm["COL_40"] = lm091["COL_3"];  //产品编号
                lm["COL_41"] = lm091["COL_4"]; //产品名称
                lm["COL_42"] = lm091["COL_5"]; //规格
                lm["COL_44"] = lm091["COL_9"]; // 生产数量
                lm["COL_43"] = lm091["COL_6"]; //单位
                lm["COL_22"] = lm091["COL_17"]; //生产单号


                if (lm.Get<int>("BIZ_PARENT_ID") == 0)
                {
                    lm["COL_1"] = lm091["COL_42"]; //产品ID
                    lm["COL_2"] = lm091["COL_3"];  //产品编号
                    lm["COL_3"] = lm091["COL_4"];  //产品名称
                    lm["COL_4"] = lm091["COL_5"]; //规格
                }
                else
                {
                    try
                    {

                        var lm196Parent = lmList096.Find(l => l.Get<int>("ROW_IDENTITY_ID") == lm.Get<int>("BIZ_PARENT_ID"));

                        lm["COL_1"] = lm196Parent["COL_1"]; //产品ID
                        lm["COL_2"] = lm196Parent["COL_2"];  //产品编号
                        lm["COL_3"] = lm196Parent["COL_3"];  //产品名称
                        lm["COL_4"] = lm196Parent["COL_4"]; //规格
                    }
                    catch (Exception ex)
                    {
                        log.Error("父节点是这个：" + lm.Get<int>("BIZ_PARENT_ID"), ex);
                    }

                }
            });



            decipher.InsertModels<LModel>(lmList096);

            lm091["HAS_CHILD"] = true;
            lm091["COL42"] = DateTime.Now;
            lm091["COL_43"] = "已调整";

            decipher.UpdateModelProps(lm091, "HAS_CHILD", "COL42", "COL43");

            decipher.IdentityRecover();

            win.ShowDialog();



        }

       



    }

}