using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.BizCommon;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Model.SecModels;
using HWQ.Entity.Filter;
using HWQ.Entity.Decipher.LightDecipher;
using EasyClick.Web.Mini2;

namespace App.InfoGrid2.Sec.User
{
    public partial class UserListM2 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);

        }

        protected override void OnInitCustomControls(EventArgs e)
        {

            OnBiz_InitUI();

            OnBiz_InitData();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void OnBiz_InitUI()
        {

        }

        /// <summary>
        /// 初始化界面数据
        /// </summary>
        private void OnBiz_InitData()
        {
            this.store1.Inserting += new EasyClick.Web.Mini2.ObjectCancelEventHandler(store1_Inserting);

            if (this.IsPostBack)
            {
                return;
            }
        }

        void store1_Inserting(object sender, EasyClick.Web.Mini2.ObjectCancelEventArgs e)
        {
            LModel lm = (LModel)e.Object;

            string newUserCode = BillIdentityMgr.NewCodeForNum("USER_CODE", "", 3);

            lm["BIZ_USER_CODE"] = newUserCode;
            lm["LOGIN_NAME"] = newUserCode;
            lm["LOGIN_PASS"] = "123456";


            try
            {


                CopySEC(newUserCode);

            }
            catch (Exception ex)
            {
                e.Cancel = true;
                log.Error("复制权限出错了！",ex);
                MessageBox.Alert("复制权限出错了！");
            }



        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                this.store1.DataBind();
            }
        }



        /// <summary>
        /// 拷贝权限 新增用户拷贝系统管理员的权限
        /// </summary>
        public void CopySEC(string code)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                //开启事务
                decipher.BeginTransaction();
               

                LightModelFilter lmFilter = new LightModelFilter(typeof(SEC_UI));
                lmFilter.And("SEC_USER_CODE", 001);
                lmFilter.And("SEC_MODE_ID", 2);
                lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter.Locks.Add(LockType.NoLock);
                lmFilter.TSqlOrderBy = "SEC_UI_ID desc";

                List<SEC_UI> suList = decipher.SelectModels<SEC_UI>(lmFilter);

                List<SEC_UI_TABLE> suTableList = null;

                List<SEC_UI> suNews = new List<SEC_UI>();
                List<SEC_UI_TABLE> suTableNews = new List<SEC_UI_TABLE>();

                List<SEC_UI_TABLECOL> suTabColList = null;
                List<SEC_UI_TABLECOL> suTabColNews = new List<SEC_UI_TABLECOL>();

                List<SEC_UI_TOOLBAR> suToolbarNews = new List<SEC_UI_TOOLBAR>();

                List<SEC_UI_TOOLBAR_ITEM> suToolbarItemNews = new List<SEC_UI_TOOLBAR_ITEM>();


                List<string> dict = new List<string>(); //索引


                IdentityFactory idFactory = decipher.IdentityFactory;

                foreach (var su in suList)
                {

                    string key = su.SEC_USER_CODE + "||" + su.MENU_ID + "||" + su.UI_PAGE_ID;

                    if (dict.Contains(key))
                    {
                        continue;
                    }

                    dict.Add(key);


                    int srcUI_ID = su.SEC_UI_ID;


                    suTableList = decipher.SelectModels<SEC_UI_TABLE>("SEC_UI_ID={0} and ROW_SID >= 0", su.SEC_UI_ID);

                    //suToolbarList = decipher.SelectModels<SEC_UI_TOOLBAR>("SEC_UI_ID={0} and ROW_SID >= 0", su.SEC_UI_ID);

                    //SEC_UI suNew = new SEC_UI();
                    //su.CopyTo(suNew, true);

                    su.SEC_UI_ID = idFactory.GetNewIdentity("SEC_UI");

                        su.SEC_USER_CODE = code;
 
                    su.SEC_MODE_ID = 2;

                    ResetModel(su);

                    //decipher.InsertModel(suNew);
                    suNews.Add(su);


                    foreach (var suTable in suTableList)
                    {
                        int srcTableId = suTable.SEC_UI_TABLE_ID;   //原值

                        suTabColList = decipher.SelectModels<SEC_UI_TABLECOL>("SEC_UI_ID={0} and SEC_UI_TABLE_ID={1} AND ROW_SID >= 0",
                           srcUI_ID, suTable.SEC_UI_TABLE_ID);


                        suTable.SEC_UI_TABLE_ID = idFactory.GetNewIdentity("SEC_UI_TABLE");

                        suTable.SEC_UI_ID = su.SEC_UI_ID;

                        ResetModel(suTable);

                        suTableNews.Add(suTable);


                        foreach (var suTabCol in suTabColList)
                        {
                            suTabCol.SEC_UI_TABLECOL_ID = idFactory.GetNewIdentity("SEC_UI_TABLECOL");

                            suTabCol.SEC_UI_ID = su.SEC_UI_ID;
                            suTabCol.SEC_UI_TABLE_ID = suTable.SEC_UI_TABLE_ID;

                            ResetModel(suTabCol);

                            suTabColNews.Add(suTabCol);
                        }



                        SEC_UI_TOOLBAR suToolbar = decipher.SelectToOneModel<SEC_UI_TOOLBAR>("SEC_UI_ID={0} AND SEC_UI_TABLE_ID={1} and ROW_SID >= 0",
                            srcUI_ID, srcTableId);

                        if (suToolbar == null)
                        {
                            continue;
                        }

                        List<SEC_UI_TOOLBAR_ITEM> suToolbarItems = decipher.SelectModels<SEC_UI_TOOLBAR_ITEM>("SEC_UI_ID={0} AND SEC_UI_TOOLBAR_ID={1} and ROW_SID >= 0",
                            srcUI_ID, suToolbar.SEC_UI_TOOLBAR_ID);

                        suToolbar.SEC_UI_TOOLBAR_ID = idFactory.GetNewIdentity("SEC_UI_TOOLBAR");

                        suToolbar.SEC_UI_ID = su.SEC_UI_ID;

                        suToolbar.SEC_UI_TABLE_ID = suTable.SEC_UI_TABLE_ID;


                        ResetModel(suToolbar);

                        suToolbarNews.Add(suToolbar);

                        foreach (var suToolbarItem in suToolbarItems)
                        {
                            suToolbarItem.SEC_UI_TOOLBAR_ITEM_ID = idFactory.GetNewIdentity("SEC_UI_TOOLBAR_ITEM");
                            suToolbarItem.SEC_UI_ID = suToolbar.SEC_UI_ID;
                            suToolbarItem.SEC_UI_TOOLBAR_ID = suToolbar.SEC_UI_TOOLBAR_ID;
                            suToolbarItem.SEC_UI_TABLE_ID = suTable.SEC_UI_TABLE_ID;
                            ResetModel(suToolbarItem);

                            suToolbarItemNews.Add(suToolbarItem);

                        }

                    }



                }


                decipher.IdentityStop();

                decipher.InsertModels<SEC_UI>(suNews);
                decipher.InsertModels<SEC_UI_TABLE>(suTableNews);
                decipher.InsertModels<SEC_UI_TABLECOL>(suTabColNews);
                decipher.InsertModels<SEC_UI_TOOLBAR>(suToolbarNews);
                decipher.InsertModels<SEC_UI_TOOLBAR_ITEM>(suToolbarItemNews);

                decipher.IdentityRecover();

                decipher.TransactionCommit();

            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("复制 权限出错了！", ex);

                throw ex;

            }

        }


        private void ResetModel(LightModel model)
        {
            model["ROW_DATE_CREATE"] = model["ROW_DATE_UPDATE"] = DateTime.Now;
            model["ROW_DATE_DELETE"] = null;
            model["ROW_SID"] = 0;
        }




    }
}