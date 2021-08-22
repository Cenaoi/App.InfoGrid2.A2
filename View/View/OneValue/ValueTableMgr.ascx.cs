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
using EC5.IG2.Core.LcValue;

namespace App.InfoGrid2.View.OneValue
{
    public partial class ValueTableMgr : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if(!IsPostBack)
            {
                this.store1.DataBind();
            }
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        public void DeleteAll() 
        {
            DataRecordCollection drc = this.table1.CheckedRows;

            if(drc.Count == 0)
            {
                Toast.Show("请选择一条记录！");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                foreach (var item in drc)
                {
                    decipher.DeleteModelByPk<IG2_VALUE_TABLE>(item.Id);
                    decipher.DeleteModels<IG2_VALUE_IF>("IG2_VALUE_TABLE_ID = {0}", item.Id);
                    decipher.DeleteModels<IG2_VALUE_THEN>("IG2_VALUE_TABLE_ID = {0}", item.Id);

                    this.store1.Remove(item);
                }                
            }
            catch (Exception ex) 
            {
                log.Error("删除数据出错了！",ex);
                MessageBox.Alert("删除数据出错了！");
            }
            

        }

        /// <summary>
        /// 重新加载
        /// </summary>
        public void GoReload()
        {
            LCodeValueLoader loader = new LCodeValueLoader();

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                loader.InitData(decipher);

                Toast.Show("重新加载成功.");
            }
            catch (Exception ex)
            {
                log.Error("加载简单流程失败", ex);

                MessageBox.Alert("重新加载失败");
            }
        }


    }
}