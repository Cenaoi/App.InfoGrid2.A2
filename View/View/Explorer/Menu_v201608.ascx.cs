using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.IG2.Core;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.IO;
using System.Text;

namespace App.InfoGrid2.View.Explorer
{
    public partial class Menu_v201608 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 获取普通菜单
        /// </summary>
        /// <returns></returns>
        private string GetCommonMenuJson()
        {

            int parentId = WebUtil.QueryInt("p_id", 100);

            int menu_level = WebUtil.QueryInt("menu_level");    //菜单的层次

            EcContext context = EcContext.Current;
            EcUserState userState = context.User;



            int secLevel = userState.LoginID.Equals(IG2Param.Role.ADMIN, StringComparison.Ordinal) ? 6 : 0;

            if (userState.Roles.Exist(IG2Param.Role.BUILDER))
            {
                secLevel = 20;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();


            LModelList<BIZ_C_MENU> models;

            if (context.User.Roles.Exist(IG2Param.Role.BUILDER))
            {

                models = decipher.SelectModels<BIZ_C_MENU>(
                    LOrder.By("PARENT_ID,SEQ,BIZ_C_MENU_ID"),
                    "ROW_SID >= 0 AND MENU_ENABLED=1 AND SEC_FUN_ID <={0} ", secLevel);

            }
            else
            {

                int[] secFunIds = M2Helper.GetUserMenuId();


                LightModelFilter filter = new LightModelFilter(typeof(BIZ_C_MENU));
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter.And("MENU_ENABLED", true);
                filter.And("BIZ_C_MENU_ID", secFunIds, Logic.In);
                filter.And("SEC_FUN_ID", secLevel, Logic.LessThanOrEqual);
                filter.TSqlOrderBy = "PARENT_ID,SEQ,BIZ_C_MENU_ID";

                models = decipher.SelectModels<BIZ_C_MENU>(filter);

            }

            if (models == null || models.Count == 0)
            {
                return "[]";
            }


            LModelGroup<BIZ_C_MENU, int> groups = models.ToGroup<int>("PARENT_ID");

            LModelList<BIZ_C_MENU> root0;

            if (!groups.TryGetValue(parentId, out root0))
            {
                return "[]";
            }



            StringBuilder sb = new StringBuilder();

            //root0.Sort("SEQ","BIZ_C_MENU_ID");

            int n = 0;

            sb.Append("[");

            foreach (BIZ_C_MENU item in root0)
            {
                if (n++ > 0) { sb.Append(", "); }


                string url = ConvertUrl(item, menu_level);  // item.URI;




                url = StringUtil.NoBlank(url, "#");
                string text = item.NAME;

                //if (menu_level >= 0)
                //{
                //    LModelList<BIZ_C_MENU> tmpSubList;

                //    if (groups.TryGetValue(item.BIZ_C_MENU_ID, out tmpSubList))
                //    {
                //        text += $" ({tmpSubList.Count})";
                //    }
                //}

                SModel t = new SModel();
                t["ICON_CHAT"] = item.ICON_CHAT;
                t["ICON_COLOR"] = item.ICON_COLOR;
                t["ICON_BG_COLOR"] = item.ICON_BG_COLOR;
                t["URL"] = url;
                t["TEXT"] = text;
                t["ID"] = item.BIZ_C_MENU_ID;
                t["MENU_LEVEL"] = menu_level + 1;


                if(menu_level == 1)
                {

                    LModelList<BIZ_C_MENU> menu2;

                    if (groups.TryGetValue(item.BIZ_C_MENU_ID, out menu2))
                    {
                        SModelList newItems = new SModelList();
                        t["ITEMS"] = newItems;

                        foreach (var item2 in menu2)
                        {

                            SModel t2 = new SModel();
                            t2["URL"] = ConvertUrl(item2, menu_level);  // item.URI;

                            t2["TEXT"] = item2.NAME;
                            t2["ID"] = item2.BIZ_C_MENU_ID;

                            newItems.Add(t2);
                        }


                    }
                }


                sb.Append(t.ToJson());
            }

            sb.Append("]");


            return sb.ToString();

        }

        private string ConvertUrl( BIZ_C_MENU item ,int menu_level)
        {
            string url = item.URI;

            if (url.StartsWith("http:") || url.StartsWith("https:"))
            {

            }
            else
            {
                if (!string.IsNullOrEmpty(url))
                {
                    UriInfo uri = new UriInfo(url);


                    //权限标签机
                    if (!string.IsNullOrEmpty(item.SEC_PAGE_TAG))
                    {
                        uri.QueryItems.Add("sec_tag", item.SEC_PAGE_TAG);
                    }


                    //别名称
                    if (!string.IsNullOrEmpty(item.ALIAS_TITLE))
                    {
                        uri.QueryItems.Add("alias_title", item.ALIAS_TITLE);
                    }

                    uri.QueryItems.Add("menu_id", item.BIZ_C_MENU_ID);
                    uri.QueryItems.Add("menu_level", (menu_level + 1));


                    url = uri.ToString();
                }
            }

            return url;
        }


        private string GetBuilderMenuJson()
        {
            WebFileInfo wfi = new WebFileInfo("/App_Biz/App_data/Builder_menu.json");

            if (!File.Exists(wfi.PhysicalPath))
            {
                return string.Empty;
            }

            string json = string.Empty;
            string srcJson = File.ReadAllText(wfi.PhysicalPath);
            SModel model = SModel.ParseJson(srcJson);

            SModelList items = model["items"] as SModelList;


            SModelList menuList = new SModelList();
             
            foreach (var item in items)
            {

                SModel t = new SModel();
                t["URL"] = item["url"];
                t["TEXT"] = item["text"];
                t["ID"] = item["text"];

                if (item.HasField("items"))
                {
                    SModelList srcItems = item["items"] as SModelList;


                    SModelList newItems = new SModelList();
                    t["ITEMS"] = newItems;

                    foreach (var item2 in srcItems)
                    {

                        SModel t2 = new SModel();
                        t2["URL"] = item2["url"];
                        t2["TEXT"] = item2["text"];
                        t2["ID"] = item2["text"];

                        newItems.Add(t2);
                    }


                }

                menuList.Add(t);
            }

            json = menuList.ToJson();

            return json;
        }

        public string GetMenuJson()
        {
            string menu_type = WebUtil.Query("menu_type");

            string json = null;

            if(menu_type == "builder")
            {
                json = GetBuilderMenuJson();
            }
            else
            {
                json = GetCommonMenuJson();
            }

            return json;
        }
    }
}