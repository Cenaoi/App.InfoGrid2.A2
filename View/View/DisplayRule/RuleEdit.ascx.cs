using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;


namespace App.InfoGrid2.View.DisplayRule
{
    public partial class RuleEdit : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///外面显示的js
        /// </summary>
        public string m_js;

        /// <summary>
        /// 外面显示的css
        /// </summary>
        public string m_css;

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);


        }

        protected void Page_Load(object sender, EventArgs e)
        {

            this.Store1.CurrentChanged += Store1_CurrentChanged;

            if (!IsPostBack) 
            {
                this.Store1.DataBind();
            }
        }

        void Store1_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            LModel lm = e.Object as LModel;

            if (lm == null ) { return; }

            DbDecipher decipher = ModelAction.OpenDecipher();  


            int id = lm.Get<int>("IG2_DISPLAY_RULE_ID");

            var rule = decipher.SelectModelByPk<IG2_DISPLAY_RULE>(id);




            //界面js
            m_js = JsonUtil.ToJson(rule.EX_JS, JsonQuotationMark.SingleQuotes);

            //界面样式
            m_css = JsonUtil.ToJson( rule.EX_CSS, JsonQuotationMark.SingleQuotes);
            
            //返回js函数
            string reJs = JsonUtil.ToJson(rule.EX_RETURN_JS_FUN, JsonQuotationMark.SingleQuotes);


            ScriptManager.Eval($"SetValue('{m_js}','{m_css}','{reJs}')");

        }


        /// <summary>
        /// 保存数据
        /// </summary>
        public void btnSave() 
        {
            int id = StringUtil.ToInt(this.Store1.CurDataId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            var rule = decipher.SelectModelByPk<IG2_DISPLAY_RULE>(id);


            var mCss = WebUtil.Form("widget1$I$tbxCSS");

            var mJs = WebUtil.Form("widget1$I$tbxJS");

            var RJs = WebUtil.Form("widget1$I$tbReturnFun");

            rule.EX_CSS = mCss;

            rule.EX_JS = mJs;

            rule.EX_RETURN_JS_FUN = RJs;

            rule.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(rule, "EX_CSS", "EX_JS", "EX_RETURN_JS_FUN","ROW_DATE_UPDATE");


            Toast.Show("保存成功了！");



        }

        /// <summary>
        /// 应用到系统
        /// </summary>
        public void GoApply()
        {
            DisplayRuleMgr.Clear();

            Toast.Show("应用成功！");
        }

        
    }
}