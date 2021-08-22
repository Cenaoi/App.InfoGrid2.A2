using App.BizCommon;
using App.InfoGrid2.Model.CMS;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.DJK
{
    public partial class SinglePage : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 标题
        /// </summary>
        public string m_title = string.Empty;

        /// <summary>
        /// 内容
        /// </summary>
        public string m_content = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                //InitData();

            }
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {

            string menu_code = WebUtil.Query("MENU_CODE");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(CMS_TILE));
            lmFilter.And("ROW_SID", 0,Logic.GreaterThanOrEqual);
            lmFilter.And("FK_MENU_CODE", menu_code);

            CMS_TILE tile = decipher.SelectToOneModel<CMS_TILE>(lmFilter);


            if(tile == null)
            {
                throw new Exception("哦噢，出错了！");
            }

            m_title = tile.MENU_TEXT;
            m_content = tile.TILE_CONTEXE;


        }

    }
}