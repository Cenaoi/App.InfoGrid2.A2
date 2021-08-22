using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.CMS;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.CMS
{
    public partial class CmsHandler : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            MessageModel msg = null;

            string action = WebUtil.Query("action");

            try
            {
                msg = ProMethod(action);
            }
            catch (Exception ex)
            {
                msg = new MessageModel("error");

                log.Error(ex);
            }

            Response.Clear();
            Response.Write(msg.ToJson());
            Response.End();
        }


        public class MessageModel : SModel
        {
            public MessageModel()
            {

            }

            public MessageModel(string result)
            {
                this.Result = result;
            }

            public MessageModel(string result, string message)
            {
                this.Result = result;
                this.Message = message;
            }

            public string Result
            {
                get { return this.Get<string>("result"); }
                set { this["result"] = value; }
            }

            public string Message
            {
                get { return this.Get<string>("msg"); }
                set { this["msg"] = value; }
            }

            public object Data
            {
                get { return this["data"]; }
                set { this["data"] = value; }
            }


        }


        private MessageModel ProMethod(string action)
        {

            if (!ValidateUtil.SqlInput(action))
            {
                throw new Exception("参数异常.method=" + action);
            }


            MessageModel msg = null;


            switch (action)
            {
                case "get_list":
                    msg = ProGetList(string.Empty);
                    break;
            }

            return msg;
        }

        private MessageModel ProGetList(string code)
        {
            MessageModel msg = null;


            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter fitler = new LightModelFilter(typeof(CMS_ITEM));
            fitler.AddFilter("ROW_SID >= 0");
            fitler.Fields = new string[] { "CMS_ITEM_ID","C_TITLE","C_IMAGE_URL","C_INTRO" };


            LModelList<LModel> models = decipher.GetModelList(fitler);

            msg = new MessageModel("ok");
            msg.Data = models;

            return msg;
        }



    }
}