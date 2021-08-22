using App.BizCommon;
using App.InfoGrid2.Model.AC3;
using App.InfoGrid2.Model.FlowModels;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.MoreActionBuilder
{
    public partial class DwgSetupListen : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const string DWG_CODE_FORMAT = @"^DWG-\d{6}-\d{1,6}$";

        const string ITEM_CODE_FORMAT = @"^\b\w*$";


        protected void Page_Load(object sender, EventArgs e)
        {
            this.store2.Inserting += Store2_Inserting;

            if (!this.IsPostBack)
            {
                this.store3.DataBind();

                this.store1.DataBind();
                this.store2.DataBind();
            }
        }

        private void InitData()
        {

        }


        private string WebForm(string key, string regex)
        {
            string value = WebUtil.FormTrim(key);

            bool valid = ValidateUtil.Regex(value, regex);

            if (!valid)
            {
                throw new Exception($"表单参数错误: key={key}, value={value}");
            }

            return value;
        }


        private void Store2_Inserting(object sender, ObjectCancelEventArgs e)
        {

        }
    }
}