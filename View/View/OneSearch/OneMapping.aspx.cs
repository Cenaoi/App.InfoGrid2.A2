using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;

namespace App.InfoGrid2.View.OneSearch
{
    /// <summary>
    /// 获取映射数据
    /// </summary>
    public partial class OneMapping : Page, IView
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            InitData();
        }

        TableSet m_tSet;

        private void InitData()
        {
            int viewId = WebUtil.QueryInt("viewId");

            DbDecipher decipher = ModelAction.OpenDecipher();

            m_tSet = TableSet.Select(decipher, viewId);

            //初始化元素,怕没有实体元素.
            TableMgr.GetModelElem(m_tSet);

            IG2_TABLE table = m_tSet.Table;


        }
    }
}