using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.InfoGrid2.Bll;
using EasyClick.Web.ReportForms.UI;
using EC5.Utility;
using EasyClick.Web.ReportForms;
using System.IO;
using EasyClick.Web.Mini;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using HWQ.Entity.LightModels;
using System.Collections;
using App.InfoGrid2.Model.DataSet;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

namespace App.InfoGrid2.View.ReportBuilder
{
    public partial class ReportPreview : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public ReportTemplateItem m_item;

        IG2_TABLE m_CrossTable = null;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            IG2_TABLE ts = decipher.SelectModelByPk<IG2_TABLE>(id);
            m_CrossTable = ts;

            ReportTemplateItem item = GetXml(ts);

            m_item = item;

            if (item == null)
            {
                return;
            }




            ReportFieldGroup group = new ReportFieldGroup();




            foreach (ReportCol c in item.col_value)
            {
                this.CrossReport1.ColTags.Add(AddItem(group, c));
            }





            foreach (ReportCol c in item.row_value)
            {
                this.CrossReport1.RowTags.Add(AddItem(group, c));
            }




            foreach (var col in item.values)
            {


                this.CrossReport1.DataTags.Add(AddItem(group, col));

            }





        }


        protected override void OnLoad(EventArgs e)
        {



            InitData(m_item.table_name);
        }



        /// <summary>
        /// 添加列、行、值标签
        /// </summary>
        public ReportFieldGroup AddItem(ReportFieldGroup group, ReportCol c)
        {
            group = new ReportFieldGroup();

            EasyClick.Web.ReportForms.UI.ReportField field = new EasyClick.Web.ReportForms.UI.ReportField();



            field.DBField = c.field;

            field.FunName = c.fun_name;

            field.DBValue = c.db_value;

            field.EnabledTotal = c.total;

            field.Format = c.format;

            if (!StringUtil.IsBlank(c.format_type))
            {
                field.FormatType = c.format_type.ToUpper() ;
            }
            else
            {
                field.FormatType = "DEFAULT";
            }


            field.Width = StringUtil.ToInt(c.width);

            field.Title = c.title;

            field.Style = c.style;



            foreach (Bll.ReportField item in c.fixed_values)
            {
                RFieldValue value = new RFieldValue();

                value.Text = item.text;

                value.Type = item.type;

                value.Value = item.value;

                value.Operator = EnumUtil.Parse<OperatorTypes>(item.operators, OperatorTypes.Equals);


                field.FixedValues.Add(value);

            }


            group.Items.Add(field);


            return group;

        }



        /// <summary>
        /// 导出Excel文件
        /// </summary>
        public void ExportExcel()
        {
            string dir = Server.MapPath("/_Temporary/CrossReport/Excel");


            if (!Directory.Exists(dir))
            {

                Directory.CreateDirectory(dir);
            }


            string name = System.Guid.NewGuid().ToString();



            string path = string.Format("{0}\\{1}.xls", dir, name);

            this.CrossReport1.SaveToExcel(path);

            DownloadWindow down = new DownloadWindow("你点我下载", 100, 200);





            string pa = Request.Path;

            down.FileUrl = string.Format("/_Temporary/CrossReport/Excel/{0}.xls", name);

            down.Show();
        }




        /// <summary>
        /// 拿到报表设置xml信息
        /// </summary>
        /// <returns></returns>
        public ReportTemplateItem GetXml(IG2_TABLE ts)
        {


            if (ts == null)
            {
                return null;
            }

            try
            {
                ReportTemplateItem item = XmlUtil.Deserialize<ReportTemplateItem>(ts.PAGE_TEMPLATE);

                return item;

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }


            return null;

        }




        /// <summary>
        /// 绑定数据
        /// </summary>
        public void InitData(string tableName)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();



            int id = WebUtil.QueryInt("id");


            try
            {

                int viewOwnerId = m_CrossTable.VIEW_OWNER_TABLE_ID;

                IG2_TABLE vTab = decipher.SelectModelByPk<IG2_TABLE>(viewOwnerId);
                
                IList models = null;

                if (vTab.TABLE_TYPE_ID == "VIEW" || vTab.TABLE_TYPE_ID == "TABLE")   //普通单表
                {
                    LightModelFilter filter = new LightModelFilter(vTab.TABLE_NAME);

                    models = decipher.GetModelList(filter);

                }
                else if (vTab.TABLE_TYPE_ID == "MORE_VIEW") //关联表
                {

                    ViewSet tSet = ViewSet.Select(decipher, viewOwnerId);

                    string tSql = ViewMgr.GetTSql(tSet);

                    models = decipher.GetModelList(tSql);
                }





                this.CrossReport1.DataSource = models;

                this.CrossReport1.Render();
            }
            catch (Exception ex)
            {
                log.Error("查找数据出错了！",ex);
                throw new Exception("查找数据出错了！",ex);
            }



        }


    }
}