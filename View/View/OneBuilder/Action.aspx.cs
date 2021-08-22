using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Interfaces;
using System.IO;
using HWQ.Entity.LightModels;
using EC5.Utility.Web;
using App.InfoGrid2.Bll.Builder;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using HWQ.Entity;

namespace App.InfoGrid2.View.OneBuilder
{
    public partial class Action : System.Web.UI.Page,IView
    {

        

        protected override void OnInit(EventArgs e)
        {


            string method = WebUtil.Query("method");    //执行的函数名称

            method = method.ToUpper();

            switch (method)
            {
                case "LOAD": LoadJson(); break;
                case "LOAD_PARAMS": LoadParams(); break;
                case "SAVE": SaveJson(); break;
            }

        }


        protected void Page_Load(object sender, EventArgs e)
        {


        }

        /// <summary>
        /// 加载 Json 数据
        /// </summary>
        private void LoadJson()
        {
            
            int tmpId = WebUtil.QueryInt("id"); //模板ID

            string pageType = WebUtil.Query("page_type");

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE model = decipher.SelectModelByPk<IG2_TABLE>(tmpId);


            string content;    // File.ReadAllText(path);

            if (string.IsNullOrEmpty(model.PAGE_TEMPLATE))
            {
                //string path = MapPath("/View/OneBuilder/page-template.json");
                //content = File.ReadAllText(path);

                string xPath;

                if (pageType == "ONE_FORM")
                {
                    xPath = MapPath("/View/OneBuilder/DEFAULT_OneForm.xml");
                }
                else
                {
                    xPath = MapPath("/View/OneBuilder/XMLFile1.xml");
                }

                string xml = File.ReadAllText(xPath);

                content = TemplateConvert.XmlToJson(xml);

            }
            else
            {
                content = TemplateConvert.XmlToJson(model.PAGE_TEMPLATE);
            }

            Response.Clear();
            Response.Write(content);
            Response.End();
        }

        /// <summary>
        /// 保存 Json 数据
        /// </summary>
        private void SaveJson()
        {
            int tmpId = WebUtil.QueryInt("id");

            string json = WebUtil.Form("data");

            string serClass = WebUtil.Form("ser_class");



            string xml = TemplateConvert.JsonToXml(json);


            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("IG2_TABLE_ID", tmpId);
            filter.And("TABLE_TYPE_ID", "PAGE");

            IG2_TABLE model = decipher.SelectToOneModel<IG2_TABLE>(filter);

            if (model == null)
            {
                return;
            }

            model.SERVER_CLASS = serClass;

            model.PAGE_TEMPLATE = xml;

            decipher.UpdateModelProps(model, "SERVER_CLASS", "PAGE_TEMPLATE");
        }

        /// <summary>
        /// 加载类名
        /// </summary>
        private void LoadParams()
        {
            int tmpId = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("IG2_TABLE_ID", tmpId);
            filter.And("TABLE_TYPE_ID", "PAGE");
            filter.Fields = new string[] {"SERVER_CLASS" };

            LModel model = decipher.GetModel(filter);

            string json = ModelHelper.ToJson(model);

            Response.Clear();
            Response.Write(json);
            Response.End(); 

        }

    }
}