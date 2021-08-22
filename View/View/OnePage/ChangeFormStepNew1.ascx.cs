using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using EasyClick.Web.Mini2;
using System.Xml;
using System.IO;
using System.Text;

namespace App.InfoGrid2.View.OnePage
{
    public partial class ChangeFormStepNew1 : WidgetControl, IView
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
                this.InitData();

            }
        }

        void InitData()
        {

        }

        IG2_TABLE m_PageModel;

        public void GoNext()
        {
            int pageId = WebUtil.QueryInt("id");



            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE model = m_PageModel = decipher.SelectModelByPk<IG2_TABLE>(pageId);

            if (model == null)
            {
                throw new Exception(string.Format("复杂表页面的数据 IG2_TABLE 不存在, IG2_TABLE_ID={0}", pageId));
            }
            
         
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(model.PAGE_TEMPLATE);
            }
            catch
            {
                throw new Exception(string.Format("加载复杂表模板错误. IG2_TABLE_ID={0}", pageId));
            }

            XmlNode root = doc.DocumentElement;

            XmlNode headNode = root.SelectSingleNode("head");

            XmlNode bodyNode = root.SelectSingleNode("body");


            XmlNode searchNode = GetNode(bodyNode, "SEARCH");
            XmlNode boxNode = GetNode(bodyNode, "BOX");
            XmlNode tabsNode = GetNode(bodyNode, "TABS");


            int searchId = XmlUtil.GetAttrValue<int>(searchNode, "ec-main-view");
            int boxId = XmlUtil.GetAttrValue<int>(boxNode, "ec-main-view");

            TableSet tsSearch = GetTaablSet(searchId);
            TableSet tsBox = GetTaablSet(boxId);

            List<TableSet> tsTabs = new List<TableSet>();

            List<string> tabsText = new List<string>(); //标签 

            foreach (XmlNode tabNode in tabsNode.ChildNodes)
            {
                int tabId = XmlUtil.GetAttrValue<int>(tabNode, "ec-main-view");


                TableSet tsTab = GetTaablSet(tabId);

                tabsText.Add(tsTab.Table.TAB_TEXT);

                tsTabs.Add(tsTab);
            }

            int formTemplateId = 0;
            CreateForm(tsBox, tsTabs, tabsText,pageId, out formTemplateId);

            int srcIndex = 0;

            foreach (IG2_TABLE_COL tc in tsSearch.Cols)
            {
                IG2_TABLE_COL itc = tsBox.Find(tc.DB_FIELD,out srcIndex);

                tc.IS_SEARCH_VISIBLE = itc.IS_SEARCH_VISIBLE;
            }

            
            //DEFAULT_OneForm.xml

            
            IG2_TABLE tt = tsBox.Table;
            tt.DISPLAY += " (表单列表)";
            tt.TABLE_TYPE_ID = "VIEW";
            tt.TABLE_SUB_TYPE_ID = "USER";
            tt.IG2_CATALOG_ID = 102;


            tt.PAGE_AREA_ID = string.Empty;
            tt.DISPLAY_MODE = string.Empty;
            tt.VISIBLE_BTN_EDIT = true;

            tt.FORM_EDIT_PAGEID = formTemplateId;
            tt.FORM_EDIT_TYPE = "ONE_FORM";
            tt.FORM_NEW_PAGEID = formTemplateId;
            tt.FORM_NEW_TYPE = "ONE_FORM";

            tt.ROW_DATE_CREATE = tt.ROW_DATE_UPDATE = DateTime.Now;

            tt.COPY_FROM_TABLE_ID = pageId;

            tsBox.Insert(decipher);
            
           
        }


        private void CreateForm(TableSet tsBox, List<TableSet> tsTabs,List<string> tabsText,int pageId, out int formTemplateId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            string xPath = MapPath("/View/OneBuilder/DEFAULT_OneForm.xml");

            XmlDocument doc = new XmlDocument();
            doc.Load(xPath);


            tsBox.Insert(decipher);

            foreach (TableSet ts in tsTabs)
            {
                ts.Insert(decipher);
            }



            XmlNode rootNode = doc.DocumentElement;

            XmlNode bodyNode = rootNode.SelectSingleNode("body");

            XmlNode headNode = GetNodeForId(bodyNode, "HanderForm1");
            XmlUtil.SetAttrValue(headNode, "ec-main-view", tsBox.Table.IG2_TABLE_ID);
            XmlUtil.SetAttrValue(headNode, "ec-main-name", tsBox.Table.TABLE_NAME);

            XmlNode tabsNode = GetNodeForId(bodyNode, "detailTabPanel");

            foreach (XmlNode xn in tabsNode.ChildNodes)
            {
                tabsNode.RemoveChild(xn);
            }


            int tabIndex = 0;

            foreach (TableSet ts in tsTabs)
            {
                //<div ec-type="TAB" title="明细" id="tab1" ec-main-view="" ec-main-type="VIEW" ec-main-name="" />
                XmlNode node = doc.CreateElement("div");

                XmlUtil.SetAttrValue(node, "ec-type", "TAB");
                XmlUtil.SetAttrValue(node, "title", tabsText[tabIndex]);

                XmlUtil.SetAttrValue(node, "id", "tab" + (++tabIndex));
                XmlUtil.SetAttrValue(node, "ec-main-view", ts.Table.IG2_TABLE_ID);
                XmlUtil.SetAttrValue(node, "ec-main-type", "VIEW");
                XmlUtil.SetAttrValue(node, "ec-main-name", ts.Table.TABLE_NAME);

                tabsNode.AppendChild(node);
            }
            
            IG2_TABLE template = new IG2_TABLE();
            template.TABLE_TYPE_ID = "PAGE";
            template.TABLE_SUB_TYPE_ID = "ONE_FORM";
            template.DISPLAY = tsBox.Table.DISPLAY;
            template.IG2_CATALOG_ID = 103;

            template.PAGE_TEMPLATE = XmlUtil.ToString(doc);

            template.COPY_FROM_TABLE_ID = pageId;
            template.IS_BIG_TITLE_VISIBLE = true;

            decipher.InsertModel(template);

            formTemplateId = template.IG2_TABLE_ID;
        }

        private TableSet GetTaablSet(int tableId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet ts = TableSet.Select(decipher, tableId);

            return ts;
        } 

        private XmlNode GetNode(XmlNode node, string ecType)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if(XmlUtil.GetAttrValue(item,"ec-type") == ecType)
                {
                    return item;
                }
            }

            return null;
        }


        private XmlNode GetNodeForId(XmlNode node,  string id)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (XmlUtil.GetAttrValue(item,"id") == id)
                {
                    return item;
                }
            }

            return null;
        }

    }
}