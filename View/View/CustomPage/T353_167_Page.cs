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
using App.InfoGrid2.View.V2;
using EC5.IG2.BizBase;

namespace App.InfoGrid2.View.CustomPage
{
    /// <summary>
    /// 委外加工管理/委外加工收货单
    /// </summary>
    public class T353_167_Page : ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit()
        {
            //TabPanel tabPanel = this.FindControl("tabs0") as TabPanel;
            //tabPanel.ButtonVisible = false;

        }

        /// <summary>
        /// 	UT_143	委外加工收货单-主材料
        /// </summary>
        Store st143 = null;

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

            

            if(PageType == "FORM")
            {
                Toolbar tbar = this.FindControl("toolbarT1250") as Toolbar;

                ToolBarButton btn = new ToolBarButton("导入");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);

                
            }else
            {
                Toolbar tbar = this.FindControl("toolbarT659") as Toolbar;

                st143 = this.FindControl("Store_UT_143") as Store;

                st143.Updating += st143_Updating;



                ToolBarButton btn = new ToolBarButton("导入");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);
            }




            
        }

        void st143_Updating(object sender, ObjectCancelEventArgs e)
        {

            LModel lm = (LModel)e.Object;

            if (lm == null)
            {
                return;
            }



            //板长
            decimal COL_6 = lm.Get<decimal>("COL_6");
            //长度
            decimal COL_36 = lm.Get<decimal>("COL_36");
            //计量单位
            string COL_43 = lm.Get<string>("COL_43");
            //需求量单位
            string COL_48 = lm.Get<string>("COL_48");
            //需求量
            decimal COL_47 = lm.Get<decimal>("COL_47");
            //数量
            decimal COL_35 = lm.Get<decimal>("COL_35");
            //密度
            decimal COL_5 = lm.Get<decimal>("COL_5");
            //厚度
            decimal COL_17 = lm.Get<decimal>("COL_17");
            //宽度
            decimal COL_4 = lm.Get<decimal>("COL_4");
            //主单位
            string COL_9 = lm.Get<string>("COL_9");
            //主数量
            decimal COL_12 = lm.Get<decimal>("COL_12");


            //计量单位值改变 就执行 需求量 换算 数量 公式 
            bool b = lm.GetBlemish("COL_43");

            MyJosn json = null;

            if (b)
            {

                //这是 需求量 换算 数量
                json = UnitUtil.UnitConversion(lm, COL_43, COL_35, COL_36, COL_5, COL_17, COL_4, COL_6, COL_48, COL_47, "COL_35", "COL_36");

                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);

                    return;
                }

                COL_35 = lm.Get<decimal>("COL_35");
            }


            if ((lm.GetBlemish("COL_35") || b) && COL_35 !=0)
            {


                //这是 数量 换算 主数量
                json = UnitUtil.UnitConversion2(lm, COL_43, COL_9, COL_35, COL_36, COL_12, COL_5, COL_17, COL_4, COL_6, "COL_12", "COL_36");

                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);
                }
            }
            string id = lm.GetPk().ToString();

            st143.SetRecordValue(id, "COL_35", lm["COL_35"]);
            st143.SetRecordValue(id, "COL_36", lm["COL_36"]);
            st143.SetRecordValue(id, "COL_12", lm["COL_12"]);


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

            var item = this.MainStore.GetDataCurrent();

            string COL_11 = item.Fields["COL_11"].Value;





            Window win = new Window("导入");
            win.State = WindowState.Max;


            string tsqlParam = string.Concat("[{",
               @"p_type:'TSQL_WHERE', value:'UT_242.COL_56.COL_34 = \'", COL_11, @"\''",
           "}]");

            string filter2 = Base64Util.ToString(tsqlParam, Base64Mode.Http);


            win.ContentPath = string.Format("/App/InfoGrid2/View/MoreView/DataImportDialog.aspx?id=954&filter2={0}", filter2);

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
            LightModelFilter lmFilter = new LightModelFilter("UT_242");
            lmFilter.And("ROW_IDENTITY_ID", ids, HWQ.Entity.Filter.Logic.In);
            string id = this.MainStore.CurDataId;
            List<LModel> lm242List = decipher.GetModelList(lmFilter);
            List<LModel> Lm137List = new List<LModel>();
            List<LModel> Lm139List = new List<LModel>();


            List<LModel> lm141List = new List<LModel>();
            List<LModel> lm143List = new List<LModel>();
            List<LModel> lm142List = new List<LModel>();


            try
            {

                foreach (var lm242 in lm242List)
                {
                    LightModelFilter lmFilter137 = new LightModelFilter("UT_137");
                    lmFilter137.And("COL_11", lm242["COL_16"]);
                    lmFilter137.And("COL_22", lm242["COL_60"]);
                    lmFilter137.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                    var lm137 = decipher.GetModelList(lmFilter137);


                    LightModelFilter lmFilter139 = new LightModelFilter("UT_139");
                    lmFilter139.And("COL_34", lm242["COL_60"]);
                    lmFilter139.And("COL_17", lm242["COL_16"]);
                    lmFilter139.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                    var lm139 = decipher.GetModelList(lmFilter139);

                    Lm137List.AddRange(lm137);
                    Lm139List.AddRange(lm139);

                    var lm143 = new LModel("UT_143");

                    M2MapHelper.MapTable(lm242, 97, lm143);

                    lm143["COL_15"] = id;

                    lm141List.Add(lm143);
                }


                foreach (var lm137 in Lm137List)
                {
                    var lm141 = new LModel("UT_141");
                    M2MapHelper.MapTable(lm137, 47, lm141);

                    lm141["COL_12"] = id;

                    lm143List.Add(lm141);
                }

                foreach (var lm139 in Lm139List)
                {
                    var lm142 = new LModel("UT_142");

                    M2MapHelper.MapTable(lm139, 49, lm142);

                    lm142["COL_13"] = id;

                    lm142List.Add(lm142);

                }

                Store st141 = this.FindControl("Store_UT_141") as Store;
                Store st143 = this.FindControl("Store_UT_143") as Store;
                Store st142 = this.FindControl("Store_UT_142") as Store;



                #region 联动对象

                DbCascadeFactory dbCascadeFactory = new DbCascadeFactory();
                dbCascadeFactory.ExecEnd += delegate(object dcSnder, DbCascadeEventArges dcE)
                {
                    EC5.BizLogger.LogStepMgr.Insert(dcE.Steps[0], dcE.OpText, dcE.Remark);
                };

                #endregion


                lm141List.ForEach(m =>
                {
                    m.SetTakeChange(true);
                    m.SetBlemishAll(true);

                    //单元格公式
                    LCodeFactory lcFactory = new LCodeFactory();
                    lcFactory.ExecLCode(st141, m);

                    //简单流程
                    LCodeValueFactory lcvFactiry = new LCodeValueFactory();
                    lcvFactiry.ExecLCode(st141, m);


                    decipher.InsertModel(m);

                    dbCascadeFactory.Inserted(st141, m);
                });

                lm142List.ForEach(m =>
                {
                    m.SetTakeChange(true);
                    m.SetBlemishAll(true);

                    //单元格公式
                    LCodeFactory lcFactory = new LCodeFactory();
                    lcFactory.ExecLCode(st143, m);

                    //简单流程
                    LCodeValueFactory lcvFactiry = new LCodeValueFactory();
                    lcvFactiry.ExecLCode(st143, m);


                    decipher.InsertModel(m);

                    dbCascadeFactory.Inserted(st143, m);
                });

                lm143List.ForEach(m =>
                {
                    m.SetTakeChange(true);
                    m.SetBlemishAll(true);

                    //单元格公式
                    LCodeFactory lcFactory = new LCodeFactory();
                    lcFactory.ExecLCode(st142, m);

                    //简单流程
                    LCodeValueFactory lcvFactiry = new LCodeValueFactory();
                    lcvFactiry.ExecLCode(st142, m);
                    decipher.InsertModel(m);

                    dbCascadeFactory.Inserted(st142, m);
                });

                st141.Refresh();
                st143.Refresh();
                st142.Refresh();

                MessageBox.Alert("导入成功！");

            }
            catch (Exception ex) 
            {
                log.Error("导入数据失败了！",ex);
                MessageBox.Alert("导入数据出错了！");
            }

        }

    }
}