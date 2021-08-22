using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Sec.UIFilterTU;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;

namespace App.InfoGrid2.View.OneBuilder
{
    public partial class CopyStep1 : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitData();
            }
        }


        private void InitData()
        {
            int srcTableId = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.Select(decipher, srcTableId);

            TB1.Value = tSet.Table.DISPLAY;


        }


        public void GoNext()
        {
            int srcTableId = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();  
            

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("IG2_TABLE_ID", srcTableId);
            //filter.And("TABLE_TYPE_ID","PAGE");
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            //filter.Fields = new string[]{"PAGE_TEMPLATE"};

            IG2_TABLE mainTable = decipher.SelectToOneModel<IG2_TABLE>(filter);

            string pageTemplate = mainTable.PAGE_TEMPLATE;// decipher.ExecuteScalar<string>(filter);


            mainTable.ROW_DATE_CREATE = DateTime.Now;
            mainTable.ROW_DATE_DELETE = DateTime.Now;
            mainTable.ROW_DATE_UPDATE = DateTime.Now;

            mainTable.DISPLAY = TB1.Value;

            decipher.InsertModel(mainTable);
             

            if (StringUtil.IsBlank(pageTemplate))
            {
                return;
            }

            List<PageTable> pTables = new List<PageTable>();

            XmlDocument doc = ParseTemplate(pageTemplate, pTables);


            foreach (PageTable pTable in pTables)
            {

                

                TableSet tSet = TableSet.Select(decipher, pTable.TableId);

                tSet.Table.ROW_DATE_CREATE = DateTime.Now;
                tSet.Table.ROW_DATE_DELETE = DateTime.Now;
                tSet.Table.ROW_DATE_UPDATE = DateTime.Now;

                tSet.Insert(decipher);

                pTable.TableId = tSet.Table.IG2_TABLE_ID;
                
                foreach(var item in tSet.Cols)
                {

                    if(string.IsNullOrEmpty(item.ACT_TABLE_ITEMS))
                    {
                        continue;
                    }

                    try
                    {

                        JObject o = (JObject)JsonConvert.DeserializeObject(item.ACT_TABLE_ITEMS);

                        int tableID = o.Value<int>("view_id");

                        TableSet tSetDo = TableSet.Select(decipher, tableID);

                        tSetDo.Table.VIEW_OWNER_TABLE_ID = tSet.Table.IG2_TABLE_ID;
                        tSetDo.Table.ROW_DATE_CREATE = DateTime.Now;
                        tSetDo.Table.ROW_DATE_DELETE = DateTime.Now;
                        tSetDo.Table.ROW_DATE_UPDATE = DateTime.Now;

                        tSetDo.Insert(decipher);

                        item.ACT_TABLE_ITEMS = string.Format("{{\"type_id\":\"VIEW\",\"view_id\":{0},\"table_name\":\"{1}\",\"owner_table_id\":{2}}}", tSetDo.Table.IG2_TABLE_ID, tSetDo.Table.TABLE_NAME, tSet.Table.IG2_TABLE_ID);

                        decipher.UpdateModelProps(item, "ACT_TABLE_ITEMS");
                    }
                    catch (Exception ex) 
                    {
                        log.Error("拷贝字段出错了！",ex);
                    }
                }





            }
            

           
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            doc.Save(sw);

            sw.Close();

            string xml = sb.ToString(); //模板数据


            mainTable.PAGE_TEMPLATE = xml;

            decipher.UpdateModelProps(mainTable, "PAGE_TEMPLATE");



            MessageBox.Alert("拷贝完成！请回原来界面刷新下！");

        }

        private XmlDocument ParseTemplate(string pageTemplate, List<PageTable> pTables)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(pageTemplate);

            XmlNode root = doc.DocumentElement;
            XmlNode xBody = root.SelectSingleNode("body");


            ParseTemplateNode(xBody, pTables);

            return doc;
        }


        private void ParseTemplateNode(XmlNode xParent, List<PageTable> pTables)
        {
            foreach (XmlNode xNode in xParent.ChildNodes)
            {
                string ecType = XmlUtil.GetAttrValue(xNode, "ec-type");
                string ecMainView = XmlUtil.GetAttrValue(xNode, "ec-main-view");
                string ecMainName = XmlUtil.GetAttrValue(xNode, "ec-main-name");
                string id = XmlUtil.GetAttrValue(xNode, "id");

                if (!StringUtil.IsBlank(ecMainView) && !StringUtil.IsBlank(ecMainName))
                {
                    PageTable pt = new PageTable();
                    pt.EcType = ecType;
                    pt.ID = id;
                    pt.TableName = ecMainName;
                    pt.TableId = StringUtil.ToInt(ecMainView);

                    pt.XNode = xNode;

                    pTables.Add(pt);
                }

                ParseTemplateNode(xNode, pTables);
            }

        }


        class PageTable
        {
            /// <summary>
            /// 
            /// </summary>
            public string EcType { get; set; }


            public string TableName { get; set; }

            public string ID { get; set; }

            public int TableId
            {
                get
                {
                    if (this.XNode == null)
                    {
                        return 0;
                    }
                    
                    return XmlUtil.GetAttrValue<int>(this.XNode, "ec-main-view");
                }
                set
                {
                    if (this.XNode == null)
                    {
                        return;
                    }

                    XmlUtil.SetAttrValue(this.XNode, "ec-main-view", value);
                }
            }

            public XmlNode XNode { get; set; }
        }


       
    }



}