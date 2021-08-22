using App.BizCommon;
using App.InfoGrid2.Model.CMS;
using App.InfoGrid2.Model.FlowModels;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.CMS
{
    public partial class CmsItemEdit : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            this.C_IMAGE_fu.Uploader += C_IMAGE_fu_Uploader;

            if (!this.IsPostBack)
            {
                
                InitData();
            }

        }

        private void InitData()
        {
            this.store1.DataBind();

            int id = WebUtil.QueryInt("id");
            string cata_code = WebUtil.Query("cata_code");

            UriInfo uri = new UriInfo("/App/InfoGrid2/View/CMS/CmsUploadHandler.aspx");

            uri.Append("method", "image_up");
            uri.Append("module", "tile");
            uri.Append("cata_code", cata_code);
            uri.Append("item_id", id);

            this.htmlEditor1.ImageUrl = uri.ToString();
        }

        private void C_IMAGE_fu_Uploader(object sender, EventArgs e)
        {
            string exFile = Path.GetExtension(this.C_IMAGE_fu.FileName);

            WebFileInfo wFile = new WebFileInfo("/UserFile/CMS/", FileUtil.NewFielname(exFile));

            this.C_IMAGE_fu.SaveAs(wFile.PhysicalPath);
            this.C_IMAGE_fu.Value = wFile.RelativePath;

        }

        public void GoSave()
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {
                CMS_ITEM ci = decipher.SelectModelByPk<CMS_ITEM>(id);

                ci.SetTakeChange(true);

                ci.C_TITLE = this.C_TITLE_tb.Value;
                ci.C_IMAGE_URL = this.C_IMAGE_fu.Value;
                ci.C_INTRO = this.C_INTRO_tb.Value;
                ci.C_CONTENT = this.htmlEditor1.Value;// this.C_CONTENT_tb2.Value;

                ci.ROW_DATE_UPDATE = DateTime.Now;

                decipher.UpdateModel(ci, true);

                Toast.Show("保存成功");
            }
            catch(Exception ex)
            {
                log.Error("保存失败", ex);

                MessageBox.Alert("保存失败:" + ex.Message);
            }

        }

    }
}