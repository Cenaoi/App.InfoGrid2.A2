using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.Utility.Web;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

namespace App.EC52Demo.View.ViewSetup
{
    public partial class MViewStepEdit1 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 正式视图表ID
        /// </summary>
        public int m_id;

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
             m_id = WebUtil.QueryInt("id");

            if (!IsPostBack)
            {
                InitData();
            }

        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData() 
        {
           

            DbDecipher decipher = ModelAction.OpenDecipher();
            try
            {

                IG2_VIEW view = decipher.SelectModelByPk<IG2_VIEW>(m_id);

                if (view == null)
                {
                    Response.Redirect("/App/EC52Demo/View/ViewSetup/Error.aspx");
                }

                List<IG2_VIEW_TABLE> tableList = decipher.SelectModels<IG2_VIEW_TABLE>("IG2_VIEW_ID={0}", m_id);

                List<IG2_VIEW_FIELD> fieldList = decipher.SelectModels<IG2_VIEW_FIELD>("IG2_VIEW_ID={0}", m_id);

                ///删除之前的新增或编辑留下的数据
                decipher.DeleteModels<IG2_TMP_VIEW_TABLE>("IG2_VIEW_ID={0}", m_id);
                ///删除之前的新增或编辑留下的数据
                decipher.DeleteModels<IG2_TMP_VIEW_FIELD>("IG2_VIEW_ID={0}", m_id);

                IG2_TMP_VIEW tmpView = new IG2_TMP_VIEW();
                view.CopyTo(tmpView, true);
                tmpView.TMP_GUID = Guid.NewGuid();

                decipher.DeleteModels<IG2_TMP_VIEW>("IG2_VIEW_ID ={0}", m_id);

                decipher.InsertModel(tmpView);

                foreach(IG2_VIEW_TABLE item in tableList)
                {
                    IG2_TMP_VIEW_TABLE tmp = new IG2_TMP_VIEW_TABLE();

                    item.CopyTo(tmp,true);

                    tmp.TMP_GUID = tmpView.TMP_GUID;

                    decipher.InsertModel(tmp);


                }


                foreach (IG2_VIEW_FIELD item in fieldList)
                {
                    IG2_TMP_VIEW_FIELD tmp = new IG2_TMP_VIEW_FIELD();

                    item.CopyTo(tmp,true);
                    tmp.TMP_GUID = tmpView.TMP_GUID;

                    decipher.InsertModel(tmp);

                }


                this.tbxViewName.Value = tmpView.DISPLAY;

          

            }
            catch (Exception ex) 
            {
                log.Error("拷贝数据出错！",ex);
            }


        }


        /// <summary>
        /// 下一步
        /// </summary>
        public void GoNext()
        {
            ///拿到视图名称
            string name = this.tbxViewName.Value;

            if (string.IsNullOrEmpty(name))
            {
                EasyClick.Web.Mini.MiniHelper.Alert("视图名称不能为空");
                return;
            }
            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                IG2_TMP_VIEW view = decipher.SelectModelByPk<IG2_TMP_VIEW>(m_id); ;

                view.DISPLAY = name;

                decipher.UpdateModelProps(view, "DISPLAY");

                MiniPager.Redirect("MViewStepEdit2.aspx?id="+m_id);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Alert("下一步出错了！");
            }
        }
    }
}