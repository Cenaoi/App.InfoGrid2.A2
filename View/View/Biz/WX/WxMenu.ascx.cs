using App.BizCommon;
using App.InfoGrid2.Model.WeChat;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using EasyClick.Web.Mini2;
using System.Collections.Specialized;

namespace App.InfoGrid2.View.Biz.WX
{
    public partial class WxMenu : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            store1.CurrentChanged += Store1_CurrentChanged;

            store1.Inserting += Store1_Inserting;

            store2.Inserting += Store2_Inserting;



            if (!IsPostBack)
            {
                store1.DataBind();

            }
        }

        private void Store2_Inserting(object sender, EasyClick.Web.Mini2.ObjectCancelEventArgs e)
        {
            LModel lm = e.Object as LModel;

            string code = BillIdentityMgr.NewCodeForYear("WX_MENU", "");


            lm["PK_MENU_LA_CODE"] = code;


        }

        private void Store1_Inserting(object sender, EasyClick.Web.Mini2.ObjectCancelEventArgs e)
        {

            LModel lm = e.Object as LModel;

            string code = BillIdentityMgr.NewCodeForYear("WX_MENU", "");


            lm["PK_MENU_CODE"] = code;
            


        }

        private void Store1_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            store2.DataBind();
        }


        /// <summary>
        /// 更新微信自定义菜单按钮事件
        /// </summary>
        public void GoUpdataWxMenu()
        {


            DbDecipher decipher = ModelAction.OpenDecipher();

            List<LModel> menus = store1.GetList() as List<LModel>;




            SModel sm_menu = new SModel();

            SModelList sm_buttons = new SModelList();

            sm_menu["button"] = sm_buttons;

            foreach (LModel menu in menus)
            {

                if (menu["MENU_TYPE"] != "有子菜单")
                {
                    SModel sm_button = new SModel();
                    sm_button["name"] = menu["MENU_TEXT"];
                    if (menu["CLICK_TYPE"] == "click")
                    {
                        sm_button["key"] = menu["MENU_KEY"];
                        sm_button["type"] = "click";
                    }
                    else
                    {
                        sm_button["type"] = "view";
                        sm_button["url"] = menu["MENU_URL"];
                    }

                    sm_buttons.Add(sm_button);

                }
                else
                {

                    SModelList sm_sub_buttons = new SModelList();

                    SModel sm_button = new SModel();

                    sm_button["name"] = menu["MENU_TEXT"];
                    sm_button["sub_button"] = sm_sub_buttons;


                    string code = menu["PK_MENU_CODE"];
                    LightModelFilter lmFilter = new LightModelFilter(typeof(WX_MENU_LA));
                    lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                    lmFilter.And("FK_MENU_CODE", code);


                    List<WX_MENU_LA> menu_las = decipher.SelectModels<WX_MENU_LA>(lmFilter);

                    foreach(var item in menu_las)
                    {

                        SModel sm_sub_button = new SModel();
                        sm_sub_button["name"] = item.MENU_TEXT;


                        if (item.CLICK_TYPE == "click")
                        {
                            sm_sub_button["key"] = item.MENU_KEY;
                            sm_sub_button["type"] = "click";
                        }
                        else
                        {
                            sm_sub_button["type"] = "view";
                            sm_sub_button["url"] = item.MENU_URL;
                        }

                        sm_sub_buttons.Add(sm_sub_button);


                    }

                    sm_buttons.Add(sm_button);

                }
                


            }


            string url = GlobelParam.GetValue<string>("WX_MENU_API", @"http://yq.gzbeih.com/API", "微信公共API地址");

            string key = GlobelParam.GetValue<string>("WX_MENU_API_KEY", "WXHB", "在微信公共API中的关键字");


            url += "/Menu.ashx";

            string result = string.Empty;

            NameValueCollection nv = new NameValueCollection();

            try
            {

                nv["post_data"] = sm_menu.ToJson();
                nv["key"] = key;

                using (WebClient wc = new WebClient())
                {


                    byte[] recData = wc.UploadValues(url, "POST", nv);

                    result = Encoding.UTF8.GetString(recData);

                }

                dynamic sm_result = SModel.ParseJson(result);

                if (!sm_result.success)
                {
                    MessageBox.Alert(sm_result.error_msg);

                    return;

                }

                Toast.Show(sm_result.msg);
                
            }catch(Exception ex)
            {
                log.Error(ex);
                MessageBox.Alert("哦噢，出错了！");

            }

        }

    }
}