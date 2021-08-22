using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility;
using EC5.Utility.Web;
using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EasyClick.Web.Mini;
using HWQ.Entity;
using App.InfoGrid2.Model.SecModels;

namespace App.InfoGrid2.Sec.FunDefine
{
    public partial class ModuleNew : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Save()
        {
            int pId = WebUtil.QueryInt("pId");
            string type = WebUtil.Query("type", "module");



            SEC_FUN_DEF m = new SEC_FUN_DEF();
            m.CODE = CODE_TB.Value;
            m.TEXT = TEXT_TB.Value;
            m.PARENT_ID = pId;

            if (type == "module")
            {
                m.FUN_TYPE_ID = 0;
            }
            else if (type == "fun")
            {
                m.FUN_TYPE_ID = 2;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                decipher.InsertModel(m);

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