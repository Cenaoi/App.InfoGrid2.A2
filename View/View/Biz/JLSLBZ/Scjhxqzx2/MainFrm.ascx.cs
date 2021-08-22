using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini2;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using HWQ.Entity.Filter;

namespace App.InfoGrid2.View.Biz.JLSLBZ.Scjhxqzx2
{
    public partial class MainFrm : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Store1.CurrentChanged += new EasyClick.Web.Mini2.ObjectEventHandler(Store1_CurrentChanged);

            if (!this.IsPostBack)
            {
                this.headLab.Value = "<span class='page-head' >工艺研发管理</span>";
                this.DataBind();
            }
        }

        void Store1_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            LModel model = e.Object as LModel;

            if (model != null)
            {
                string clientCode = model.Get<string>("COL_2");

                MiniPager.Redirect("iform1",
                    string.Format("FormScjhxqzx.aspx?client_name={0}", clientCode));
            }
        
        }

        /// <summary>
        /// 根据客户名称查询客户
        /// </summary>
        public void SelectUT071() 
        {
            string name = this.tbxName.Value;
           
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_071");
            filter.Or("COL_2", "%" + name + "%", Logic.Like);
            filter.Or("COL_1", "%" + name + "%", Logic.Like);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Top = 50;

            filter.Fields = new string[] { "ROW_IDENTITY_ID", "COL_1", "COL_2" };
            filter.TSqlOrderBy = "COL_14";

            List<LModel> lmList = decipher.GetModelList(filter);

            this.Store1.RemoveAll();
            this.Store1.AddRange(lmList);

        }

        /// <summary>
        /// 生产需求分析中心  研发管理   
        /// 增加查询功能   混合排版   
        /// 查询后可以显示所有 UT_102 的 COL_56为 多客户 的记录
        /// </summary>
        public void SelectUT071_MC()
        {
            string name = this.tbxName.Value;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_071");
            filter.Or("COL_2", "%" + name + "%", Logic.Like);
            filter.Or("COL_1", "%" + name + "%", Logic.Like);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.Top = 50;

            filter.Fields = new string[] { "ROW_IDENTITY_ID", "COL_1", "COL_2" };
            filter.TSqlOrderBy = "COL_14";

            List<LModel> lmList = decipher.GetModelList(filter);

            this.Store1.RemoveAll();
            this.Store1.AddRange(lmList);

            //参数 c56_filter = COL_56 过滤
            MiniPager.Redirect("iform1",
                string.Format("FormScjhxqzx.aspx?c56_filter=mc", ""));

        }

    }
}