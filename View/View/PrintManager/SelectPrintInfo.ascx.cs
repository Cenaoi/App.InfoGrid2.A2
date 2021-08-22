using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HWQ.Entity.LightModels;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.SecModels;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility;
using EC5.SystemBoard;
using EC5.IG2.Core;

namespace App.InfoGrid2.View.PrintManager
{
    public partial class SelectPrintInfo : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            this.store1.CurrentChanged += new EasyClick.Web.Mini2.ObjectEventHandler(store1_CurrentChanged);

            if(!IsPostBack)
            {
                this.store1.DataBind();

                InitData();

            }
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {

            EcUserState user = EcContext.Current.User;

            if (user.Roles.Exist(IG2Param.Role.BUILDER))
            {

                var table_struce = table1.Columns.FindByDataField("ROW_STRUCE_CODE");

                table_struce.Visible = true;

            }



        }


        void store1_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            if (e.SrcRecord == null)
            {
                return;
            }

            LModel lm = (LModel)e.Object;

            if(lm == null)
            {
                return;
            }

            int id = lm.Get<int>("BIZ_PRINT_ID");

            this.store2.RemoveAll();

            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                BIZ_PRINT info = decipher.SelectModelByPk<BIZ_PRINT>(id);

                if (info == null || StringUtil.IsBlank(info.PRINT_CODE))
                {
                    return;
                }

                List<BIZ_PRINT_NAME> nameList = decipher.SelectModels<BIZ_PRINT_NAME>("PRINT_CODE='{0}'", info.PRINT_CODE);

                List<BIZ_PRINT_CLIENT> clientList = new List<BIZ_PRINT_CLIENT>();

                foreach (BIZ_PRINT_NAME item in nameList)
                {
                    BIZ_PRINT_CLIENT client = decipher.SelectToOneModel<BIZ_PRINT_CLIENT>("PCLIENT_GUID='{0}'", item.PCLIENT_GUID);

                    if (client  == null)
                    {
                        continue;
                    }

                        clientList.Add(client);
                    
                }





                this.store2.AddRange(clientList);
                
            }
            catch (Exception ex) 
            {
                log.Error("查询客户端信息出错了！",ex);
                MessageBox.Alert("查询客户端信息出错了！");
            }

        }
    }
}