using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Model.SecModels;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using HWQ.Entity.Filter;
using System.Diagnostics;
using System.Text;

namespace App.InfoGrid2.Sec.UIFilterTU
{
    public partial class StepCopySEC : WidgetControl, IView
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
                this.cbModeID.Value = "2";
            }
        }

       


        /// <summary>
        /// 确定拷贝
        /// </summary>
        public void GoNext() 
        {

            //这是填写的类型编码
            string code = this.TB1.Value;
            
            //这是角色ID
            int modeID = int.Parse(this.cbModeID.Value);

            //这是菜单ID
            int menuID = WebUtil.QueryInt("menuId");

            // 1 == 单个界面   2 == 全部界面
            int oneOrAll = int.Parse(this.cbOneOrAll.Value);

            if(string.IsNullOrEmpty(code))
            {
                MessageBox.Alert("请填写用户或角色代码！");
                return;
            }

            string userCode = WebUtil.Query("code");

            if (string.IsNullOrEmpty(userCode))
            {
                Response.Redirect("https://www.baidu.com");
            }


            if(code == userCode)
            {
                MessageBox.Alert("要复制到用户不能和原来的相同！");
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();



            LightModelFilter lmFilterOld = new LightModelFilter(typeof(SEC_UI));
            if (modeID == 2)
            {
                lmFilterOld.And("SEC_USER_CODE", code);

                int count = decipher.SelectCount<SEC_LOGIN_ACCOUNT>("BIZ_USER_CODE='{0}'",code);

                if(count < 1)
                {
                    MessageBox.Alert("请输入正确的用户编码");
                    return;
                }
            }
            else 
            {
                lmFilterOld.And("SEC_ROLE_CODE", code);
                int count = decipher.SelectCount<SEC_ROLE>("ROLE_CODE='{0}'", code);
                if (count < 1)
                {
                    MessageBox.Alert("请输入正确的角色编码");
                    return;
                }
            }
            


            lmFilterOld.And("SEC_MODE_ID", modeID);
            lmFilterOld.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            List<SEC_UI> suListOld = decipher.SelectModels<SEC_UI>(lmFilterOld);


            if(oneOrAll == 1)
            {
                //拷贝单个界面的权限功能
                CopyOne(userCode, code, modeID, menuID);

                return;
            }



            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                Stopwatch sw2 = new Stopwatch();
                sw2.Start();


                LightModelFilter lmFilter = new LightModelFilter(typeof(SEC_UI));
                if (modeID == 2)
                {
                    lmFilter.And("SEC_USER_CODE", userCode);
                    lmFilter.And("SEC_MODE_ID", modeID);

    
                }
                else
                {
                    lmFilter.And("SEC_ROLE_CODE", userCode);
                    lmFilter.And("SEC_MODE_ID", modeID);
                   
                }
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

                    if (modeID == 2)
                    {
                        su.SEC_USER_CODE = code;
                    }
                    else
                    {
                        su.SEC_ROLE_CODE = code;
                    }

                    su.SEC_MODE_ID = modeID;
                    
                    ResetModel(su);

                    //decipher.InsertModel(suNew);
                    suNews.Add(su);

                    if(suTableList.Count == 0)
                    {
                        continue;
                    }

                    Queue<int> tabNewIDs = idFactory.GetNewBatchIdentity("SEC_UI_TABLE", "SEC_UI_TABLE_ID", suTableList.Count);

                    foreach (var suTable in suTableList)
                    {
                        int srcTableId = suTable.SEC_UI_TABLE_ID;   //原值

                        suTabColList = decipher.SelectModels<SEC_UI_TABLECOL>("SEC_UI_ID={0} and SEC_UI_TABLE_ID={1} AND ROW_SID >= 0",
                           srcUI_ID, suTable.SEC_UI_TABLE_ID);


                        suTable.SEC_UI_TABLE_ID = tabNewIDs.Dequeue();// idFactory.GetNewIdentity("SEC_UI_TABLE");

                        suTable.SEC_UI_ID = su.SEC_UI_ID;

                        ResetModel(suTable);

                        suTableNews.Add(suTable);

                        if (suTabColList.Count == 0)
                        {
                            continue;
                        }

                        sw2.Restart();

                        Queue<int> teNewIds = idFactory.GetNewBatchIdentity("SEC_UI_TABLECOL", "SEC_UI_TABLECOL_ID", suTabColList.Count);

                        foreach (var suTabCol in suTabColList)
                        {
                            suTabCol.SEC_UI_TABLECOL_ID = teNewIds.Dequeue();// idFactory.GetNewIdentity("SEC_UI_TABLECOL");

                            suTabCol.SEC_UI_ID = su.SEC_UI_ID;
                            suTabCol.SEC_UI_TABLE_ID = suTable.SEC_UI_TABLE_ID;

                            ResetModel(suTabCol);

                            suTabColNews.Add(suTabCol);
                        }

                        sw2.Stop();
                        log.Debug($"拷贝表字段, 共 {suTabColNews.Count} 个字段, 耗时: {sw2.Elapsed}");



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

                        if (suToolbarItems.Count == 0)
                        {
                            continue;
                        }

                        Queue<int> ttNewIds = idFactory.GetNewBatchIdentity("SEC_UI_TOOLBAR_ITEM", "SEC_UI_TOOLBAR_ITEM_ID", suToolbarItems.Count);

                        foreach (var suToolbarItem in suToolbarItems)
                        {
                            suToolbarItem.SEC_UI_TOOLBAR_ITEM_ID = ttNewIds.Dequeue();
                            suToolbarItem.SEC_UI_ID = suToolbar.SEC_UI_ID;
                            suToolbarItem.SEC_UI_TOOLBAR_ID = suToolbar.SEC_UI_TOOLBAR_ID;
                            suToolbarItem.SEC_UI_TABLE_ID = suTable.SEC_UI_TABLE_ID;
                            ResetModel(suToolbarItem);

                            suToolbarItemNews.Add(suToolbarItem);

                        }


                    }



                }


                StringBuilder sb = new StringBuilder();

                int j = 0;

                foreach (var item in suListOld)
                {
                    if (j++ > 0) { sb.Append(","); }

                    sb.Append(item.SEC_UI_ID);
                }

                string deleteUIIDs = sb.ToString();

                object[] updateFields = new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now };

                //事务开始
                decipher.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);

                int count = 0;


                sw2.Restart();

                if (suListOld.Count > 0)
                {
                    count += decipher.UpdateProps<SEC_UI>($"ROW_SID >=0 and SEC_UI_ID in ({deleteUIIDs})", updateFields);

                    count += decipher.UpdateProps<SEC_UI_TABLE>($"ROW_SID >=0 and SEC_UI_ID in ({deleteUIIDs})", updateFields);

                    count += decipher.UpdateProps<SEC_UI_TABLECOL>($"ROW_SID >=0 and SEC_UI_ID in ({deleteUIIDs})", updateFields);

                    count += decipher.UpdateProps<SEC_UI_TOOLBAR>($"ROW_SID >=0 and SEC_UI_ID in ({deleteUIIDs})", updateFields);

                    count += decipher.UpdateProps<SEC_UI_TOOLBAR_ITEM>($"ROW_SID >=0 and SEC_UI_ID in ({deleteUIIDs})", updateFields);
                }

                sw2.Stop();
                log.Debug($"更新记录, 总耗时: {sw2.Elapsed}");


                decipher.IdentityStop();

                sw2.Restart();

                decipher.InsertModels<SEC_UI>(suNews);

                sw2.Stop();
                log.Debug($"插入记录 SEC_UI, 共{suNews.Count}条记录, 耗时: {sw2.Elapsed}");

                sw2.Restart();

                decipher.InsertModels<SEC_UI_TABLE>(suTableNews);

                sw2.Stop();
                log.Debug($"插入记录 SEC_UI_TABLE, 共{suTableNews.Count}条记录, 耗时: {sw2.Elapsed}");

                sw2.Restart();
                decipher.InsertModels<SEC_UI_TABLECOL>(suTabColNews);
                sw2.Stop();
                log.Debug($"插入记录 SEC_UI_TABLECOL, 共{suTabColNews.Count}条记录, 耗时: {sw2.Elapsed}");

                sw2.Restart();

                decipher.InsertModels<SEC_UI_TOOLBAR>(suToolbarNews);
                sw2.Stop();
                log.Debug($"插入记录 SEC_UI_TOOLBAR, 共{suToolbarNews.Count}条记录, 耗时: {sw2.Elapsed}");

                sw2.Restart();
                decipher.InsertModels<SEC_UI_TOOLBAR_ITEM>(suToolbarItemNews);
                sw2.Stop();
                log.Debug($"插入记录 SEC_UI_TOOLBAR_ITEM, 共{suToolbarItemNews.Count}条记录, 耗时: {sw2.Elapsed}");

                sw2.Restart();

                decipher.IdentityRecover();

                decipher.TransactionCommit();

                sw.Stop();

                MessageBox.Alert("复制权限成功了！");

                log.Debug("复制权限总耗时: " + sw.Elapsed.ToString());
            }
            catch (Exception ex) 
            {
                decipher.TransactionRollback();

                log.Error("复制 权限出错了！",ex);
                MessageBox.Alert("复制权限出错了！");
            }

        }


        /// <summary>
        /// 复制单界面权限
        /// </summary>
        /// <param name="userCode">源用户代码</param>
        /// <param name="code">目标用户代码</param>
        /// <param name="modeID">类型 2-- 用户  1 -- 角色</param>
        /// <param name="menuID">菜单ID</param>
        public void CopyOne(string userCode,string code,int modeID,int menuID )
        {

            DbDecipher decipher = ModelAction.OpenDecipher();



            LightModelFilter lmFilterOld = new LightModelFilter(typeof(SEC_UI));
            if (modeID == 2)
            {
                lmFilterOld.And("SEC_USER_CODE", code);

            }
            else
            {
                lmFilterOld.And("SEC_ROLE_CODE", code);
                
            }
            lmFilterOld.And("MENU_ID", menuID);
            lmFilterOld.And("SEC_MODE_ID", modeID);
            lmFilterOld.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            SEC_UI suOld = decipher.SelectToOneModel<SEC_UI>(lmFilterOld);


            object[] updateFields = new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now };

            //事务开始
            decipher.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);

            //如果之前就没有权限的话，就不用删除权限了
            if (suOld != null)
            {

                decipher.UpdateProps<SEC_UI>(string.Format("SEC_UI_ID={0} and ROW_SID >=0", suOld.SEC_UI_ID), updateFields);

                decipher.UpdateProps<SEC_UI_TABLE>(string.Format("SEC_UI_ID={0} and ROW_SID >= 0", suOld.SEC_UI_ID), updateFields);
                decipher.UpdateProps<SEC_UI_TABLECOL>(string.Format("SEC_UI_ID={0} and ROW_SID >= 0", suOld.SEC_UI_ID), updateFields);
                decipher.UpdateProps<SEC_UI_TOOLBAR>(string.Format("SEC_UI_ID={0} and ROW_SID >= 0", suOld.SEC_UI_ID), updateFields);
                decipher.UpdateProps<SEC_UI_TOOLBAR_ITEM>(string.Format("SEC_UI_ID={0} and ROW_SID >= 0", suOld.SEC_UI_ID), updateFields);
            }

            


            LightModelFilter lmFilter = new LightModelFilter(typeof(SEC_UI));
            if (modeID == 2)
            {
                lmFilter.And("SEC_USER_CODE", userCode);
            }
            else
            {
                lmFilter.And("SEC_ROLE_CODE", userCode);
               
            }
            lmFilter.And("SEC_MODE_ID", modeID);
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("MENU_ID", menuID);
            lmFilter.Locks.Add(LockType.NoLock);
            lmFilter.TSqlOrderBy = "SEC_UI_ID desc";

            //目标源代码权限
            SEC_UI su = decipher.SelectToOneView<SEC_UI>(lmFilter);




            List<SEC_UI_TABLE> suTableList = null;

            List<SEC_UI_TABLE> suTableNews = new List<SEC_UI_TABLE>();

            List<SEC_UI_TABLECOL> suTabColList = null;
            List<SEC_UI_TABLECOL> suTabColNews = new List<SEC_UI_TABLECOL>();

            List<SEC_UI_TOOLBAR> suToolbarNews = new List<SEC_UI_TOOLBAR>();

            List<SEC_UI_TOOLBAR_ITEM> suToolbarItemNews = new List<SEC_UI_TOOLBAR_ITEM>();



            IdentityFactory idFactory = decipher.IdentityFactory;


            int srcUI_ID = su.SEC_UI_ID;

            suTableList = decipher.SelectModels<SEC_UI_TABLE>("SEC_UI_ID={0} and ROW_SID >= 0", su.SEC_UI_ID);

            //suToolbarList = decipher.SelectModels<SEC_UI_TOOLBAR>("SEC_UI_ID={0} and ROW_SID >= 0", su.SEC_UI_ID);

            //SEC_UI suNew = new SEC_UI();
            //su.CopyTo(suNew, true);

            su.SEC_UI_ID = idFactory.GetNewIdentity("SEC_UI");

            if (modeID == 2)
            {
                su.SEC_USER_CODE = code;
            }
            else
            {
                su.SEC_ROLE_CODE = code;
            }

            su.SEC_MODE_ID = modeID;

            ResetModel(su);

     
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

            decipher.IdentityStop();

            try {
                decipher.InsertModel(su);
                decipher.InsertModels<SEC_UI_TABLE>(suTableNews);
                decipher.InsertModels<SEC_UI_TABLECOL>(suTabColNews);
                decipher.InsertModels<SEC_UI_TOOLBAR>(suToolbarNews);
                decipher.InsertModels<SEC_UI_TOOLBAR_ITEM>(suToolbarItemNews);

                decipher.IdentityRecover();

                decipher.TransactionCommit();


                Toast.Show("复制权限成功了！");

            }catch(Exception ex)
            {
                log.Error($"复制单个界面出错了！数据源用户代码：{userCode}，要复制的代码：{code},要复制的类型2--用户，1--角色：{modeID},要复制的单个界面ID:{menuID}",ex);
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