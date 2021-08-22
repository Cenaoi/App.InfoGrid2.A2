using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Sample
{
    public partial class XSS : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);

            //this.file1.Uploader += File1_Uploader;

            //if (!this.IsPostBack)
            //{
            //    this.file1.Value = "http://cdn.play.cn/f/cms/u/cms/www/201507/23160417prjz.png";
            //}
        }

        private void File1_Uploader(object sender, EventArgs e)
        {
            //this.file1.SaveAs(@"C:\\SSS\\" + this.file1.FileName);

            //this.file1.TempValue = this.file1.FileName;
            //this.file1.Value = "http://img2.kuwo.cn/star/starheads/300/27/45/1182138792.jpg";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void GoNext()
        {
            //this.textBox11.Value = DateTime.Now.ToString();
        }
        
    }
}