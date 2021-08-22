using App.BizCommon;
using EC5.SystemBoard;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.Handlers
{
    /// <summary>
    /// EcVipHandler 的摘要说明
    /// </summary>
    public class EcVipHandler : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        HttpRequest m_request;

        HttpResponse m_response;


        public void ProcessRequest(HttpContext context)
        {

            m_response = context.Response;

            m_request = context.Request;

            m_response.ContentType = "text/plain";

            EcUserState user = EcContext.Current.User;

            if (!user.Roles.Exist("VIP"))
            {
                m_response.Write("没有登录，所有不能操作！");
                return;
            }

            string action = WebUtil.FormTrimUpper("action");

            switch (action)
            {

                case "GET_ADDRESS_LIST":
                    GetAddressList();
                    break;

            }

        }


        /// <summary>
        /// 获取当前用户的地址集合
        /// </summary>
        void GetAddressList()
        {

            EcUserState user = EcContext.Current.User;


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lmVip = decipher.GetModelByPk("VIP_ACCOUNT", user.Identity);

            if (lmVip == null)
            {
                m_response.Write("没有找到用户，请联系系统管理员！");
                return;
            }


            LightModelFilter lmFilter = new LightModelFilter("ES_SHIPPING_ADDRESS");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("VIP_ID",user.Identity);


            List<LModel> lmAddList = decipher.GetModelList(lmFilter);


            LModel lmDefAdd = null;
            
            foreach(LModel lm in lmAddList)
            {

               if(lm.Get<int>("ROW_IDENTITY_ID") == lmVip.Get<int>("ADDRESS_ID"))
                {
                    lmDefAdd = lm;

                    break;
                }
            }


            lmAddList.Remove(lmDefAdd);

            SModel sm = new SModel();
            sm["address_list"] = lmAddList;
            sm["def_address"] = lmDefAdd;

            m_response.Write(sm.ToJson());

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}