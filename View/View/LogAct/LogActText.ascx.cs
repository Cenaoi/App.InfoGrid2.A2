using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using EC5.BizLogger.Model;
using System.Text;
using EC5.DbCascade.Model;
using HWQ.Entity.LightModels;
using System.Collections.Specialized;

namespace App.InfoGrid2.View.LogAct
{
    public partial class LogActText : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {

            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }

        /// <summary>
        /// 显示的操作
        /// </summary>
        public string m_text;


        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                InitData();
            }
        }

        NameValueCollection m_ActCodes = new NameValueCollection()
            {
                {"INSERT","新建"},
                {"UPDATE","更新"},
                {"DELETE","删除"},
                {"ALL","全部"},
                {"NONE","阻止"}
            };

        private void InitData() 
        {
            int id = WebUtil.QueryInt("id");

            StringBuilder sb = new StringBuilder();
            



            DbDecipher decipher = ModelAction.OpenDecipher();


            LOG_ACT_OP op = decipher.SelectModelByPk<LOG_ACT_OP>(id);
            
            IG2_ACTION act = decipher.SelectModelByPk<IG2_ACTION>(op.ACTION_ID);

            List<LOG_ACT_OPDATA> opdata = decipher.SelectModels<LOG_ACT_OPDATA>("LOG_ACT_OP_ID={0}", id);

            foreach(var item in opdata)
            {
                sb.AppendLine(op.RESULT_MESSAGE).AppendLine();

                sb.AppendFormat("数据表={0}, 主键={1}, 描述={2}", op.C_TABLE, item.C_PK_VALUE, op.C_DISPLAY).AppendLine();

                if (act != null)
                {
                    sb.AppendFormat("联动ID={0}, 描述={1}", act.IG2_ACTION_ID, act.REMARK).AppendLine();
                    sb.AppendFormat("    左（{0}:{1} {2}）", m_ActCodes[act.L_ACT_CODE], act.L_TABLE, act.L_TABLE_TEXT).AppendLine();
                    sb.AppendFormat("    右（{0}:{1} {2}）", m_ActCodes[act.R_ACT_CODE], act.R_TABLE, act.R_TABLE_TEXT).AppendLine();
                }

                sb.AppendLine();

                sb.AppendLine(item.OPDATE_XML);
                sb.AppendLine();
                sb.AppendLine();
            }


            m_text = sb.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'","&#39;").ToString();

        }

        //private string GetActRemark(DbDecipher decipher, int id)
        //{
        //    LightModelFilter filter = new LightModelFilter(typeof(IG2_ACTION));
        //    filter.And("", id);
        //    filter.Fields = new string[] { "L_ACT_CODE", "L_TABLE, L_TABLE_TEXT, R_TABLE, R_TABLE_TEXT, R_ACT_CODE, REMARK };

        //}



    }
}