using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.Utility;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini2.Data;
using HWQ.Entity.Filter;
using App.InfoGrid2.Model.SecModels;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity;

namespace App.InfoGrid2.Sec.M2User
{
    public partial class UserTreeMenuSetup1 : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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

            EcUserState ecUser = EcContext.Current.User;

            string userCode = WebUtil.Query("login_code");  // ecUser.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter roleFilter = new LightModelFilter(typeof(SEC_USER_FUN));
            roleFilter.And("LOGIN_CODE", userCode);
            roleFilter.And("VISIBLE", true);

            LModelList<SEC_USER_FUN> userModules = decipher.SelectModels<SEC_USER_FUN>(roleFilter);


            LModelList<BIZ_C_MENU> rootCatalogs = decipher.SelectModels<BIZ_C_MENU>(
                LOrder.By("SEQ ,BIZ_C_MENU_ID "),
                "MENU_ENABLED = 1 and ROW_SID >=0 ");

            SortedList<int, SEC_USER_FUN> sortedUserModules = userModules.ToSortedList<int>("SEC_FUN_DEF_ID");  //转换为字典档案

            List<string> checkIds = new List<string>();

            foreach (BIZ_C_MENU cata in rootCatalogs)
            {
                if (cata.BIZ_C_MENU_ID < 100)
                {
                    continue;
                }

                TreeNode tNode = new TreeNode(cata.NAME, cata.BIZ_C_MENU_ID);
                tNode.ParentId = cata.PARENT_ID.ToString();
                tNode.NodeType = "default";
                tNode.StatusID = 0;

                if (sortedUserModules.ContainsKey(cata.BIZ_C_MENU_ID))
                {
                    checkIds.Add(cata.BIZ_C_MENU_ID.ToString());
                    //tNode.Checked = true;
                }

                if (cata.BIZ_C_MENU_ID == 100)
                {
                    tNode.Expand();
                }


                this.TreePanel1.Add(tNode);
            }


            this.TreePanel1.CheckNodes(checkIds.ToArray());

        }

        /// <summary>
        /// 节点点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreePanel1_Selected(object sender, EventArgs e)
        {
            TreeNode node = this.TreePanel1.NodeSelected;

            int id = StringUtil.ToInt(node.Value);

            DbDecipher decipher = ModelAction.OpenDecipher();

            if (!node.ChildLoaded)
            {
                List<BIZ_C_MENU> models = decipher.SelectModels<BIZ_C_MENU>(LOrder.By("SEQ ,BIZ_C_MENU_ID "), "PARENT_ID={0} AND ROW_SID >= 0", id);

                foreach (BIZ_C_MENU model in models)
                {
                    TreeNode node2 = new TreeNode(model.NAME, model.BIZ_C_MENU_ID);

                    node2.ParentId = node.Value;
                    node2.StatusID = 0;

                    this.TreePanel1.Add(node2);
                }

                this.TreePanel1.Refresh();

                node.ChildLoaded = true;
                node.Expand();

            }

            this.TreePanel1.OpenNode(node.Value);

            if (node.Value != "ROOT")
            {
                id = int.Parse(node.Value);

                BIZ_C_MENU cata = decipher.SelectModelByPk<BIZ_C_MENU>(id);

                MiniPager.Redirect("iform1",
                    string.Format("/App/InfoGrid2/View/Biz/Core_Menu/EditMenuInfo.aspx?id={0}", id));
            }


        }


        public int[] ToIntList(string[] strList)
        {
            return ToIntList(strList, 0);
        }

        public int[] ToIntList(string[] strList,int defaultValue)
        {
            int[] ids = new int[strList.Length];

            for (int i = 0; i < strList.Length; i++)
            {
                int value;

                if (int.TryParse(strList[i], out value))
                {
                    ids[i] = value;
                }
                else
                {
                    ids[i] = defaultValue;
                }
            }

            return ids;
        }

        /// <summary>
        /// 查找全部父级
        /// </summary>
        /// <param name="models"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        private int[] GetParents(LModelList<BIZ_C_MENU> models, int nodeId)
        {
            BIZ_C_MENU model = models.FindByPk(nodeId);

            List<int> ids = new List<int>();

            int n = 0;

            while ( model != null && model.PARENT_ID > 0)
            {
                ids.Add(model.PARENT_ID);

                model = models.FindByPk(model.PARENT_ID);

                if (n++ > 20) { break; }    //避免死循环
                if (ids.Contains(model.PARENT_ID)) { break; }
            }

            return ids.ToArray();
        }

        /// <summary>
        /// 保存树节点被选中的节点
        /// </summary>
        public void GoSaveTreeChecked()
        {
            string loginCode = WebUtil.Query("login_code");

            //打钩的复选框 ID 集合
            int[] checkIds = ToIntList(this.TreePanel1.GetCheckeds());

            //不明确的复选框 ID 集合
            int[] undeIds = ToIntList(this.TreePanel1.GetUndetermineds());


            LightModelFilter filter = new LightModelFilter(typeof(BIZ_C_MENU));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("SEC_FUN_TYPE_ID", new int[] { 0, 2 }, Logic.In);


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModelList<BIZ_C_MENU> models = decipher.SelectModels<BIZ_C_MENU>(filter);

            List<int> parIds = new List<int>();

            foreach (var checkId in checkIds)
            {
                var item = models.FindByPk(checkId);

                int[] pars = GetParents(models, checkId);

                foreach (var pId in pars)
                {
                    bool exist = ArrayUtil.Exist(checkIds, pId);

                    if (!exist && !parIds.Contains(pId))
                    {
                        parIds.Add(pId);
                    }

                }
            }

            undeIds = ArrayUtil.Union(undeIds, parIds.ToArray());


            foreach (var fun in models)
            {

                LightModelFilter roleFilter = new LightModelFilter(typeof(SEC_USER_FUN));
                roleFilter.And("SEC_FUN_DEF_ID", fun.BIZ_C_MENU_ID);
                roleFilter.And("LOGIN_CODE", loginCode);
                roleFilter.And("FUN_TYPE_ID", fun.SEC_FUN_TYPE_ID);

                SEC_USER_FUN roleFun = decipher.SelectToOneModel<SEC_USER_FUN>(roleFilter);

                bool isExist = ArrayUtil.Exist(checkIds, fun.BIZ_C_MENU_ID);


                int checkStateId;
                bool visible = isExist;

                if (ArrayUtil.Exist(undeIds, fun.BIZ_C_MENU_ID))
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

                    decipher.UpdateModelProps(roleFun, "VISIBLE", "CHECK_STATE_ID");
                }
                else if (checkStateId > 0)
                {
                    roleFun = CreateModuleNode(fun,loginCode, isExist, checkStateId);
                    decipher.InsertModel(roleFun);
                }

            }

            Toast.Show("保存成功。");
        }


        private SEC_USER_FUN CreateModuleNode(BIZ_C_MENU fun, string loginCode, bool visible, int checkStateId)
        {
            int[] arrChildIds = GetFunArrChildIds(fun.BIZ_C_MENU_ID);

            SEC_USER_FUN roleFun = new SEC_USER_FUN();

            roleFun.VISIBLE = true;
            roleFun.SEC_FUN_DEF_ID = fun.BIZ_C_MENU_ID;
            roleFun.LOGIN_CODE = loginCode;
            //roleFun.LOGIN_ID = m_UserID;
            roleFun.FUN_TYPE_ID = fun.SEC_FUN_TYPE_ID;
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