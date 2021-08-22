using App.BizCommon;
using EasyClick.Web.Mini2;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;


namespace App.InfoGrid2.View.Biz.Rosin2
{
    public partial class UserDataSetup : WidgetControl, IView
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


        public void GoResetDict()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            int count = 0;

            count += decipher.DeleteModelAll("UT_003");

            count += decipher.DeleteModelAll("UT_004");
            count += decipher.DeleteModelAll("UT_005");
            count += decipher.DeleteModelAll("UT_006");
            count += decipher.DeleteModelAll("UT_007");
            count += decipher.DeleteModelAll("UT_010");

            MessageBox.Alert("数据库复位完成! 清理得干干净净.");
        }

        public void GoResetPlan()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            int count = 0;

            count += decipher.DeleteModelAll("UT_015_PLAN");            
            count += decipher.DeleteModelAll("UT_020");

            MessageBox.Alert("数据库复位完成! 清理得干干净净." );
        }

        public void GoResetOther()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            int count = 0;
            
            count += decipher.DeleteModelAll("UT_016");
            count += decipher.DeleteModelAll("UT_017_PROD");
            count += decipher.DeleteModelAll("UT_018");
            count += decipher.DeleteModelAll("UT_019");

            MessageBox.Alert("数据库复位完成! 清理得干干净净." );
        }
    }
}