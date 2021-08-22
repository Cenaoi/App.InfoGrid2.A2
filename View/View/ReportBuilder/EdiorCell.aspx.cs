using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using HWQ.Entity.Xml;
using EasyClick.Web.Mini;
using App.InfoGrid2.Model;
using App.InfoGrid2.Bll;
using Newtonsoft.Json;
using EC5.Utility.Web;
using System.Linq;

namespace App.InfoGrid2.View.ReportBuilder
{
    public partial class EdiorCell : System.Web.UI.Page, EC5.SystemBoard.Interfaces.IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  

        public string m_talbeName;

        public string m_json;

        public Guid m_Id;

        public object m_dbFields;

        /// <summary>
        /// 表ID
        /// </summary>
        public int m_tableID;

        /// <summary>
        /// 编辑表格字段属性路径
        /// </summary>
        public string m_editColProUrl;

        

        /// <summary>
        /// 字段集合
        /// </summary>
        public List<IG2_TABLE_COL> m_colList;


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if(!IsPostBack)
            {
                 InitData();
                 
            }
           

        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData()
        {



            string text = "";





            m_Id = WebUtil.QueryGuid("id");

            m_editColProUrl = string.Format("/App/InfoGrid2/View/ReportBuilder/EditTableCol.aspx?id={0}",m_Id);


            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TMP_TABLE it = decipher.SelectToOneModel<IG2_TMP_TABLE>("TMP_GUID='{0}' and TMP_SESSION_ID='{1}' ", m_Id,this.Session.SessionID);

            if(it == null)
            {
                Response.Redirect("Error.aspx");
                return;
            }

            try
            {
                List<IG2_TABLE_COL> colList = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0} and ROW_SID >=0 ", it.VIEW_OWNER_TABLE_ID);

                m_colList = colList;

                m_talbeName = it.TABLE_NAME;

                //表ID
                m_tableID = it.VIEW_OWNER_TABLE_ID;

                List<object> items = new List<object>();

                foreach (IG2_TABLE_COL elem in colList)
                {
                    var item = new
                    {
                        ///真实字段
                        dbField = elem.DB_FIELD,
                        ///显示说明
                        description = elem.DISPLAY,
                        ///字段的表名
                        talbeName = elem.TABLE_NAME,
                        ///视图的原字段
                        viewFieldSrc = elem.VIEW_FIELD_SRC
                    };

                    items.Add(item);
                }

                m_dbFields = JsonConvert.SerializeObject(items);
            }
            catch (Exception ex) 
            {
                log.Error("查找字段数据出错了！",ex);
                throw new Exception("查找字段数据出错了！", ex);
            }
            try
            {


                ExcelConvert converXmlJson = new ExcelConvert();

                
                m_json = converXmlJson.XmlToJson(it.PAGE_TEMPLATE);



            }
            catch (Exception ex)
            {
                log.Error("转换xml出错了！",ex);
                log.Debug(text);
                
            }


        }

        



    }
}