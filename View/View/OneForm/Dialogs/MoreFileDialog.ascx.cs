using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.FlowModels;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;

using EC5.AppDomainPlugin;
using EC5.IG2.Core;
using EC5.IO;
using EC5.SystemBoard;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace App.InfoGrid2.View.OneForm.Dialogs
{
    public partial class MoreFileDialog :WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        string m_Table;

        string m_Field;

        
        string m_TagCode;

        int m_RowPk = 0;

        LModelElement m_ModelElem;

        protected override void OnInit(EventArgs e)
        {
            m_RowPk = WebUtil.QueryInt("row_pk");

            m_Table = WebUtil.Query("table");
            m_Field = WebUtil.Query("field");

            m_TagCode = WebUtil.Query("tag_code");

            m_ModelElem = LightModel.GetLModelElement(m_Table);


            base.OnInit(e);
        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            UriInfo uri = new UriInfo("/App/InfoGrid2/View/OneForm/FormHandle.aspx");
            uri.Append("method", "image_upload");
            uri.Append("table", m_Table);
            uri.Append("item_id", m_RowPk);
            uri.Append("tag_code", m_TagCode);
            uri.Append("field", m_Field);


            this.moreFileUpload1.FileUrl = uri.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {


            base.OnLoad(e);

            if (!this.IsPostBack)
            {
                DbDecipher decipher = ModelAction.OpenDecipher();
                LModel model = decipher.GetModelByPk(m_Table, m_RowPk);

                string fv = (string)model[m_Field];

                this.moreFileUpload1.Value = fv;
            }
        }


        public void GoSubmit()
        {
            string value = this.moreFileUpload1.Value;

            value =  value?.Trim('\n');

            if (StringUtil.IsBlank(value))
            {
                MessageBox.Alert("请选择文件上传.");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();
            LModel model = decipher.GetModelByPk(m_Table, m_RowPk);

            model[m_Field] = value;

            try
            {
                decipher.UpdateModelProps(model, m_Field);

                HttpResult pack = HttpResult.Success(value);
                
                ScriptManager.Eval($"ownerWindow.close({pack})");

                Toast.Show("提交成功!");
            }
            catch(Exception ex)
            {
                log.Error($"保存数据失败. Table={m_Table}, Field={m_Field}, row_pk={m_RowPk}. 数据:\r\n\r\n {value}", ex);

                MessageBox.Alert("保存失败!\r\n" + ex.Message);

            }
            

        }


    }
}