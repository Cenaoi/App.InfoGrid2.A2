using System;
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
using EC5.Utility;
using EC5.Utility.Web;
using EasyClick.Web.Mini;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.SecModels;


namespace App.InfoGrid2.Sec.User
{
    public partial class ModuleTree : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        TreeViewAction<SEC_FUN_DEF_VIEW, int> m_TreeAction;

        int m_UserID;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_UserID = WebUtil.QueryInt("userId");

            m_TreeAction = new TreeViewAction<SEC_FUN_DEF_VIEW, int>(this.TreeView1);
            m_TreeAction.NodeTypeChange("2", "page");

            m_TreeAction.FilterSearch("FUN_TYPE_ID", new int[] { 0, 2 }, Logic.In);

            if (!this.IsPostBack)
            {
                InitDataTree();

            }
        }


        private void InitDataTree()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(SEC_FUN_DEF));
            filter.And("ROW_STATUS_ID", 0, Logic.GreaterThanOrEqual);
            filter.And("FUN_TYPE_ID", new int[] { 0, 2 }, Logic.In);

            LModelList<SEC_FUN_DEF> models = decipher.SelectModels<SEC_FUN_DEF>(filter);


            LightModelFilter roleFilter = new LightModelFilter(typeof(SEC_USER_FUN));
            roleFilter.And("LOGIN_ID", m_UserID);
            roleFilter.And("VISIBLE", true);

            LModelList<SEC_USER_FUN> roleModules = decipher.SelectModels<SEC_USER_FUN>(roleFilter);

            LModelList<SEC_FUN_DEF_VIEW> roleViews = new LModelList<SEC_FUN_DEF_VIEW>();

            foreach (var m in models)
            {
                SEC_FUN_DEF_VIEW view = new SEC_FUN_DEF_VIEW();
                m.CopyTo(view, true);
                roleViews.Add(view);
            }

            foreach (var roleFun in roleModules)
            {
                SEC_FUN_DEF_VIEW m = roleViews.FindByPk(roleFun.SEC_FUN_DEF_ID);

                if (m == null)
                {
                    continue;
                }

                m.VISIBLE = true;
            }

            m_TreeAction.ExplandNodes(roleViews);
        }


        /// <summary>
        /// 提交
        /// </summary>
        public void Submit()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            //打钩的复选框 ID 集合
            int[] checkIds = StringUtil.ToIntList(this.TreeView_CheckedIds.Value);
            
            //不明确的复选框 ID 集合
            int[] undeIds = StringUtil.ToIntList(this.TreeView_StateUndeterminedIds.Value);

            LightModelFilter filter = new LightModelFilter(typeof(SEC_FUN_DEF));
            filter.And("ROW_STATUS_ID", 0, Logic.GreaterThanOrEqual);
            filter.And("FUN_TYPE_ID", new int[] { 0, 2 }, Logic.In);

            LModelList<SEC_FUN_DEF> models = decipher.SelectModels<SEC_FUN_DEF>(filter);


            foreach (var fun in models)
            {

                LightModelFilter roleFilter = new LightModelFilter(typeof(SEC_USER_FUN));
                roleFilter.And("SEC_FUN_DEF_ID", fun.SEC_FUN_DEF_ID);
                roleFilter.And("LOGIN_ID", m_UserID);
                roleFilter.And("FUN_TYPE_ID", fun.FUN_TYPE_ID);

                SEC_USER_FUN roleFun = decipher.SelectToOneModel<SEC_USER_FUN>(roleFilter);

                bool isExist = ArrayUtil.Exist(checkIds, fun.SEC_FUN_DEF_ID);


                int checkStateId;
                bool visible = isExist;

                if (ArrayUtil.Exist(undeIds, fun.SEC_FUN_DEF_ID))
                {
                    checkStateId = 2;
                }
                else
                {
                    checkStateId = isExist ? 1 : 0;
                }


                if (roleFun != null)
                {
                    roleFun.VISIBLE = isExist;
                    roleFun.CHECK_STATE_ID = checkStateId;

                    decipher.UpdateModelProps(roleFun, "VISIBLE","CHECK_STATE_ID");
                }
                else if (checkStateId > 0)
                {
                    roleFun = CreateModuleNode(fun, isExist, checkStateId);
                    decipher.InsertModel(roleFun);
                }

            }

            MiniHelper.Tooltip("保存成功");

        }

        private SEC_USER_FUN CreateModuleNode(SEC_FUN_DEF fun, bool visible, int checkStateId)
        {
            int[] arrChildIds = GetFunArrChildIds(fun.SEC_FUN_DEF_ID);

            SEC_USER_FUN roleFun = new SEC_USER_FUN();

            roleFun.VISIBLE = true;
            roleFun.SEC_FUN_DEF_ID = fun.SEC_FUN_DEF_ID;
            roleFun.LOGIN_ID = m_UserID;
            roleFun.FUN_TYPE_ID = fun.FUN_TYPE_ID;
            roleFun.FUN_ARR_CHILD_ID = ArrayUtil.ToString(arrChildIds);
            roleFun.CHECK_STATE_ID = checkStateId;
            roleFun.VISIBLE = visible;

            return roleFun;
        }

        public int[] GetFunArrChildIds(int moduleId)
        {
            LightModelFilter filter = new LightModelFilter(typeof(SEC_FUN_DEF));
            filter.And("PARENT_ID", moduleId);
            filter.And("FUN_TYPE_ID", 4);
            filter.Fields = new string[] { "SEC_FUN_DEF_ID" };

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelReader reader = decipher.GetModelReader(filter);

            int[] ids = ModelHelper.GetColumnData<int>(reader);

            return ids;

        }

    }
}