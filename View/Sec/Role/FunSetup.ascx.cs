﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using EasyClick.BizWeb.UI;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using EC5.Utility.Web;
using EasyClick.Web.Mini;
using EC5.Utility;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.SecModels;

namespace App.InfoGrid2.Sec.Role
{
    public partial class FunSetup : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int m_ModuleId;
        int m_RoleId;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_ModuleId = WebUtil.QueryInt("moduleId");
            m_RoleId = WebUtil.QueryInt("roleId");

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


            LModelList<SEC_FUN_DEF> models = decipher.SelectModels<SEC_FUN_DEF>(LOrder.By("SEQ ASC"), "PARENT_ID={0} AND FUN_TYPE_ID = 4", m_ModuleId);

            SEC_ROLE_FUN roleFun = decipher.SelectToOneModel<SEC_ROLE_FUN>("SEC_FUN_DEF_ID={0} AND SEC_ROLE_ID={1}",m_ModuleId, m_RoleId);

            if (roleFun == null)
            {
                int[] ids = models.GetColumnData<int>("SEC_FUN_DEF_ID");

                roleFun = new SEC_ROLE_FUN();
                roleFun.SEC_FUN_DEF_ID = m_ModuleId;
                roleFun.SEC_ROLE_ID = m_RoleId;
                roleFun.FUN_TYPE_ID = funModule.FUN_TYPE_ID;

                roleFun.FUN_ARR_CHILD_ID = ArrayUtil.ToString(ids);


                decipher.InsertModel(roleFun);
            }



            this.TitleLab.Value = funModule.TEXT;

            this.CheckBoxList1.Items.Clear();

            foreach (var m in models)
            {
                string id = m.SEC_FUN_DEF_ID.ToString();

                ListItem item = new ListItem(id, m.TEXT);

                this.CheckBoxList1.Items.Add(item);
            }

            this.CheckBoxList1.Value = roleFun.FUN_ARR_CHILD_ID;

        }

        public void Submit()
        {
            int[] ids = StringUtil.ToIntList(this.CheckBoxList1.Value);

            DbDecipher decipher = ModelAction.OpenDecipher();

            SEC_ROLE_FUN roleFun = decipher.SelectToOneModel<SEC_ROLE_FUN>("SEC_FUN_DEF_ID={0} and SEC_ROLE_ID={1}", m_ModuleId, m_RoleId);

            roleFun.FUN_ARR_CHILD_ID = this.CheckBoxList1.Value;

            decipher.UpdateModelProps(roleFun, "FUN_ARR_CHILD_ID");


        }
    }
}