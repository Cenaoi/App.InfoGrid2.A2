using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini;
using EC5.Utility;
using App.InfoGrid2.Model.SecModels;

namespace App.InfoGrid2.Sec
{
    public partial class SecFunDefList : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        int m_ModuleId;
        string m_RoleId;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_ModuleId = WebUtil.QueryInt("moduleId");
            m_RoleId = WebUtil.Query("roleId");

            if (!this.IsPostBack)
            {
                InitData();
            }

        }

        private void InitData()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            SEC_FUN_DEF funModule = decipher.SelectModelByPk<SEC_FUN_DEF>(m_ModuleId);

            if (funModule == null)
            {
                return;
            }


            LModelList<SEC_FUN_DEF> models = decipher.SelectModels<SEC_FUN_DEF>("PARENT_ID={0}", m_ModuleId);

            LModelList<SEC_ROLE_FUN> roleFuns = decipher.SelectModels<SEC_ROLE_FUN>();



            int[] funIds = roleFuns.GetColumnData<int>("SEC_FUN_DEF_ID");

            this.TitleLab.Value = funModule.TEXT;

            this.CheckBoxList1.Items.Clear();

            foreach (var m in models)
            {
                string id = m.SEC_FUN_DEF_ID.ToString();

                ListItem item = new ListItem(id, m.TEXT);              

                this.CheckBoxList1.Items.Add(item);
            }

            this.CheckBoxList1.Value = ArrayUtil.ToString(funIds);

        }

        public void Submit()
        {
            int[] ids = StringUtil.ToIntList(this.CheckBoxList1.Value);



            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<SEC_FUN_DEF> models = decipher.SelectModels<SEC_FUN_DEF>("PARENT_ID={0}", m_ModuleId);

            LModelList<SEC_ROLE_FUN> roleFuns = decipher.SelectModels<SEC_ROLE_FUN>();

        }
    }
}