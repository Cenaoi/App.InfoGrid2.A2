using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 多文件上传框
    /// </summary>
    [Description("多文件上传框")]
    public class MoreFileUpload: FileUpload
    {
        public MoreFileUpload()
        {
            this.InReady = "Mini2.ui.form.field.MoreFileUpload";
            this.JsNamespace = "Mini2.ui.form.field.MoreFileUpload";


        }



        /// <summary>
        /// 加载 Post 上传的数据
        /// </summary>
        public override void LoadPostData()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            System.Web.HttpRequest request = context.Request;

            string value_id = this.ClientID;
            
            this.Value = request.Form[value_id];
        }




    }
}
