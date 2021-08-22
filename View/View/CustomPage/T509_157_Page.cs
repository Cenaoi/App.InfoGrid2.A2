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
using HWQ.Entity.Filter;
using App.InfoGrid2.Bll;
using HWQ.Entity;
using EC5.IG2.Core;
using App.InfoGrid2.View.V2;
using EC5.IG2.BizBase;

namespace App.InfoGrid2.View.CustomPage
{
    /// <summary>
    /// 采购管理/采购到货单
    /// </summary>
    public class T509_157_Page : ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override void OnInit()
        {
            //TabPanel tabPanel = this.FindControl("tabs0") as TabPanel;
            //tabPanel.ButtonVisible = false;

        }

        /// <summary>
        /// 	UT_162	采购到货单-产品明细
        /// </summary>
        Store st162 = null;

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

           

                Toolbar tbar = this.FindControl("toolbarT511") as Toolbar;


                st162 = this.FindControl("Store_UT_162") as Store;
                st162.CurrentChanged += st162_CurrentChanged;
                st162.Updating += st162_Updating;


                ToolBarButton btn = new ToolBarButton("导入");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);




                ///显示库存按钮
                btnMessage = new ToolBarButton("");
                btnMessage.ID = "tbMessage";
                tbar.Items.Add(btnMessage);
          
        }

        /// <summary>
        /// 明细更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void st162_Updating(object sender, ObjectCancelEventArgs e)
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
            string COL_53 = lm.Get<string>("COL_53");
            //需求量
            decimal COL_52 = lm.Get<decimal>("COL_52");
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

                // 这是 需求量 换算 数量 
                json = UnitUtil.UnitConversion(lm, COL_5, COL_7, COL_30, COL_27, COL_21, COL_28, COL_29, COL_53, COL_52, "COL_7", "COL_30");

                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);

                    return;
                }

                COL_7 = lm.Get<decimal>("COL_7");
            }

            if ((lm.GetBlemish("COL_7") || b) && COL_7 != 0)
            {

                //这是 数量 换算 主数量
                json = UnitUtil.UnitConversion2(lm, COL_5, COL_33, COL_7, COL_30, COL_20, COL_27, COL_21, COL_28, COL_29, "COL_20", "COL_30");


                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);

                }
            }

            string id = lm.GetPk().ToString();

            st162.SetRecordValue(id, "COL_7", lm["COL_7"]);
            st162.SetRecordValue(id, "COL_30", lm["COL_30"]);
            st162.SetRecordValue(id, "COL_20", lm["COL_20"]);


        }

        void st162_CurrentChanged(object sender, ObjectEventArgs e)
        {
            LModel lm = (LModel)e.Object;
            if (lm == null) { return; }
            ///产品ID
            string col_52 = lm.Get<string>("COL_40");
            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilter = new LightModelFilter("UT_118");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_15", col_52);

            List<LModel> lmList118 = decipher.GetModelList(lmFilter);

            StringBuilder sb = new StringBuilder();
            ///库存合计 
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

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择项目！");
                return;
            }



            var item = this.MainStore.GetDataCurrent();       ///拿到焦点行数据

            string COL_10 = item.Fields["COL_10"].Value;      //供应商ID


            Window win = new Window("导入");
            win.State = WindowState.Max;


            string filter2 = Base64Util.ToString("[{field:'UT_132.COL_10',logic:'=',value:'" + COL_10 + "'}]", Base64Mode.Http);   ///把过滤条件传过去


            win.ContentPath = string.Format("/App/InfoGrid2/View/MoreView/DataImportDialog.aspx?id=679&filter2={0}", filter2);

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

            int mapId = 54;       //这是映射规则ID
            int dialogId = 679;   //弹出界面ID
            string pField = "COL_12";


            int parentId = StringUtil.ToInt(this.MainStore.CurDataId);


            try
            {
               IList<LModel> lmList = BizHelper.ImportData(dialogId, mapId, ids, pField, parentId,false);


                #region 联动对象

                DbCascadeFactory dbCascadeFactory = new DbCascadeFactory();
                dbCascadeFactory.ExecEnd += delegate(object dcSnder, DbCascadeEventArges dcE)
                {
                    EC5.BizLogger.LogStepMgr.Insert(dcE.Steps[0], dcE.OpText, dcE.Remark);
                };

                #endregion


                foreach (var item in lmList)
                {
                    item.SetTakeChange(true);
                    item.SetBlemishAll(true);

                    //单元格公式
                    LCodeFactory lcFactory = new LCodeFactory();
                    lcFactory.ExecLCode(null, item);

                    //简单流程
                    LCodeValueFactory lcvFactiry = new LCodeValueFactory();
                    lcvFactiry.ExecLCode(null, item);

                    decipher.InsertModel(item);

                    dbCascadeFactory.Inserted(null, item);
                }

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