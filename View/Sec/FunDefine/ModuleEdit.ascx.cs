using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.Utility.Web;
using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using HWQ.Entity;
using EasyClick.Web.Mini;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.InfoGrid2.Model.SecModels;

namespace App.InfoGrid2.Sec.FunDefine
{
    public partial class ModuleEdit : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitData();
            }
        }

        private void InitData()
        {

            int id = WebUtil.QueryInt("Id");


            DbDecipher decipher = ModelAction.OpenDecipher();

            SEC_FUN_DEF m = decipher.SelectModelByPk<SEC_FUN_DEF>(id);

            TEXT_TB.Value = m.TEXT;
            CODE_TB.Value = m.CODE;
        }


        public void Save()
        {
            int id = WebUtil.QueryInt("Id");


            SEC_FUN_DEF m = new SEC_FUN_DEF();
            m.SEC_FUN_DEF_ID = id;
            m.TEXT = TEXT_TB.Value;
            m.CODE = CODE_TB.Value;
            m.ROW_DATE_UPDATE = DateTime.Now;

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                decipher.UpdateModelProps(m, "CODE", "TEXT", "ROW_DATE_UPDATE");

                string jsonData = ModelHelper.ToJson(m);

                EcView.close("{result:'ok',item:" + jsonData + "}");
            }
            catch (Exception ex)
            {
                log.Error(ex);

                MiniHelper.Alert("创建节点失败");
            }
        }
    }
}