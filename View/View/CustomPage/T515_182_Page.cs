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
using App.InfoGrid2.View.V2;

namespace App.InfoGrid2.View.CustomPage
{
    /// <summary>
    /// 采购管理 / 采购退货单
    /// </summary>
    public class T515_182_Page : ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override void OnInit()
        {
            //TabPanel tabPanel = this.FindControl("tabs0") as TabPanel;
            //tabPanel.ButtonVisible = false;

        }

        /// <summary>
        /// UT_164	采购退货单-产品明细
        /// </summary>
        Store st164 = null;


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


                Toolbar tbar = this.FindControl("toolbarT1237") as Toolbar;

                ToolBarButton btn = new ToolBarButton("导入");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);

                
            } else
            {




                Toolbar tbar = this.FindControl("toolbarT518") as Toolbar;

                st164 = this.FindControl("Store_UT_164") as Store;
                st164.Updating += st164_Updating;


                ToolBarButton btn = new ToolBarButton("导入");
                btn.OnClick = GetMethodJs("DaoRu", "你好");
                tbar.Items.Add(btn);

            }
            
        }
        /// <summary>
        /// 	UT_164	采购退货单-产品明细 更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void st164_Updating(object sender, ObjectCancelEventArgs e)
        {
            LModel lm = e.Object as LModel;

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



            if (lm.GetBlemish("COL_7"))
            {


                var json = UnitUtil.UnitConversion2(lm, COL_5, COL_33, COL_7, COL_30, COL_20, COL_27, COL_21, COL_28, COL_29, "COL_20", "COL_30");

                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);
                }
            }

            string id = lm.GetPk().ToString();

            st164.SetRecordValue(id, "COL_20", lm["COL_20"]);
            st164.SetRecordValue(id, "COL_30", lm["COL_30"]);



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


            string pField = "COL_12";  //这个是存放主表ID的字段

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