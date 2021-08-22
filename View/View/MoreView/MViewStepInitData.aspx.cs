using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model.DataSet;
using EC5.Utility.Web;

namespace App.InfoGrid2.View.MoreView
{
    public partial class MViewStepInitData : System.Web.UI.Page, IView
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            StepEdit2();

        }

        public void StepEdit2()
        {
            Guid opGuid = Guid.NewGuid();

            int id = WebUtil.QueryInt("id");

            InitData(opGuid);

            this.Response.Redirect(string.Format("MViewStepEdit3.aspx?id={0}&uid={1}", id, opGuid));
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData(Guid uid)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                int id = WebUtil.QueryInt("id");


                ViewSet vSet = ViewSet.Select(decipher, id);


                TmpViewSet tvSet = vSet.ToTmpViewSet();

                tvSet.Insert(decipher, uid, this.Session.SessionID);

            }
            catch (Exception ex)
            {
                throw new Exception("拷贝到临时表数据出错。", ex);
            }


        }
    }
}