using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;

namespace App.InfoGrid2.View.OneValue
{
    public partial class EditValue : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.store2.CurrentChanged += store2_CurrentChanged;
            if(!IsPostBack)
            {
                this.store1.DataBind();
                this.store2.DataBind();
            }
        }

        void store2_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            this.store3.DataBind();
            this.store4.DataBind();
        }



        /// <summary>
        /// 删除数据
        /// </summary>
        public void DeleteAll() 
        {
            DataRecordCollection  drc = this.table2.CheckedRows;

            if(drc.Count == 0)
            {
                MessageBox.Alert("请选择记录！");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                foreach (var item in drc)
                {
                    decipher.DeleteModelByPk<IG2_VALUE_IF>(item.Id);
                    decipher.DeleteModels<IG2_VALUE_THEN>("IG2_VALUE_IF_ID = {0}", item.Id);

                    this.store2.Remove(item);
                }
                                

                //MessageBox.Alert("删除成功了！");
            }
            catch (Exception ex) 
            {
                log.Error("删除数据出错了！",ex);
                MessageBox.Alert("删除数据出错了！");
            }



        }


        public void GoLast()
        {
            
        }


    }
}