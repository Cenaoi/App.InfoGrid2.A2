using System;
using System.Collections.Generic;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EC5.Utility.Web;
using App.InfoGrid2.Model.DataSet;
using HWQ.Entity.LightModels;
using System.Text;
using Newtonsoft.Json;

namespace App.InfoGrid2.View.ReportBuilder
{
    public partial class SaveData : System.Web.UI.Page, EC5.SystemBoard.Interfaces.IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Guid m_Id;

        /// <summary>
        /// 表格ID，正确就返回
        /// </summary>
        int m_tableId;


        protected void Page_Load(object sender, EventArgs e)
        {

            m_Id = WebUtil.QueryGuid("id");

            //转成大写
            string action = WebUtil.Query("action").ToUpper();


            Response.Clear();

            try
            {
                if (action == "INIT_DATA")
                {
                    InitData();
                }
                else
                {
                    SaveXml();

                    Response.Write("{\"result\":\"ok\",\"id\":\"" + m_tableId + "\"}");
                }
                

               
            }
            catch (Exception ex)
            {
                Response.Write("{\"result\":\"error\",\"id\":\"" + m_Id + "\"}");
                log.Error(ex);
            }

            Response.End();

        }


        /// <summary>
        /// 把传过来的数据存放到数据库里面去
        /// </summary>
        private void SaveXml()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            ExcelConvert con = new ExcelConvert();

            string str = Request.Form["name"];


            IG2_TMP_TABLE it = decipher.SelectToOneModel<IG2_TMP_TABLE>("TMP_GUID='{0}' and TMP_SESSION_ID='{1}' ", m_Id,this.Session.SessionID);

            List<IG2_TMP_TABLECOL> itColList = decipher.SelectModels < IG2_TMP_TABLECOL>("ROW_SID >= 0 and TMP_GUID='{0}'", m_Id);


            it.PAGE_TEMPLATE = con.JsonToXml(str);
            it.ROW_SID = 0;
            it.ROW_DATE_UPDATE = DateTime.Now;

           


            try
            {
                decipher.UpdateModelProps(it, "PAGE_TEMPLATE", "ROW_SID", "ROW_DATE_UPDATE");


                if (it.TMP_OP_ID == "E")
                {

                    m_tableId = it.IG2_TABLE_ID;

                    IG2_TABLE itable = decipher.SelectModelByPk<IG2_TABLE>(it.IG2_TABLE_ID);

                    itable.PAGE_TEMPLATE = it.PAGE_TEMPLATE;



                    decipher.UpdateModelProps(itable, "PAGE_TEMPLATE");


                    LightModelFilter lmFilter = new LightModelFilter(typeof(IG2_TABLE_COL));
                    lmFilter.And("IG2_TABLE_ID", itable.IG2_TABLE_ID);
                    lmFilter.And("ROW_SID",0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);

                    //删除
                    decipher.UpdateProps(lmFilter, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });


                    foreach (var item in itColList)
                    {
                        IG2_TABLE_COL itc = new IG2_TABLE_COL();
                        item.CopyTo(itc, true);
                        itc.IG2_TABLE_ID = itable.IG2_TABLE_ID;
                        itc.ROW_DATE_CREATE = DateTime.Now;


                        decipher.InsertModel(itc);

                        //删除临时列数据
                        decipher.DeleteModel(item);


                    }

                    //删除临时表的数据
                    decipher.DeleteModel(it);



                }
                else 
                {
                    TmpTableSet ttSet = TmpTableSet.Select(decipher, m_Id);

                    TableSet tSet = ttSet.ToTableSet();

                    tSet.Insert(decipher);

                    m_tableId = tSet.Table.IG2_TABLE_ID;

                    //删除临时表数据
                    TmpTableSet.Delete(decipher, m_Id);
                }

                

                

            }
            catch (Exception ex)
            {
                log.Error(ex);

                throw new Exception("保存错误.", ex);
            }

        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {
            m_Id = WebUtil.QueryGuid("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TMP_TABLE it = decipher.SelectToOneModel<IG2_TMP_TABLE>("TMP_GUID='{0}' and TMP_SESSION_ID='{1}' ", m_Id, this.Session.SessionID);

            if (it == null)
            {
                Error404.Send("找不到数据，请和管理员联系！");
                return;
            }

            StringBuilder sb = new StringBuilder();



            try
            {
                List<IG2_TABLE_COL> colList = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0} and ROW_SID >=0 ", it.VIEW_OWNER_TABLE_ID);

                sb.AppendFormat("{{\"result\":\"ok\",\"TABLE_NAME\":\"{0}\",\"TABLE_ID\":\"{1}\"", it.TABLE_NAME, it.VIEW_OWNER_TABLE_ID);

                List<object> items = new List<object>();

                foreach (IG2_TABLE_COL elem in colList)
                {



                    var item = new
                    {
                        //真实字段
                        dbField = elem.DB_FIELD,
                        //显示说明
                        description = elem.DISPLAY,
                        //字段的表名
                        talbeName = it.TABLE_NAME,
                        //视图的原字段
                        viewFieldSrc = elem.VIEW_FIELD_SRC,
                        //数据类型
                        dbType = elem.DB_TYPE

                    };

                    items.Add(item);
                }

                var m_dbFields = JsonConvert.SerializeObject(items);

                sb.AppendFormat(",\"FIELDS\":{0}", m_dbFields);
            }
            catch (Exception ex)
            {
                log.Error("查找字段数据出错了！", ex);
                throw new Exception("查找字段数据出错了！", ex);
            }
            try
            {


                ExcelConvert converXmlJson = new ExcelConvert();


               string   m_json = converXmlJson.XmlToJson(it.PAGE_TEMPLATE);

                if (string.IsNullOrEmpty(m_json))
                {
                    sb.Append(",\"DATA\":{}}");
                }
                else
                {
                    sb.AppendFormat(",\"DATA\":{0}}}", m_json);
                }

                


                Response.Write(sb.ToString());

            }
            catch (Exception ex)
            {
                log.Error("转换xml出错了！", ex);

                throw new Exception("转换xml出错了！", ex);

            }
        }


        


    }
}