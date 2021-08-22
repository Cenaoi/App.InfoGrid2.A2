using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.IO;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// Flash 封装的文件上传组件
    /// </summary>
    [ToolboxData("<{0}:SWFUpload runat=\"server\" />")]
    public class SWFUpload:TextBox
    {
        public SWFUpload()
        {
            m_HideField = new HiddenField();

        }

        /// <summary>
        /// 上传文件后产生的事件
        /// </summary>
        public event EventHandler Uploader;

        /// <summary>
        /// 触发上传文件后产生的事件
        /// </summary>
        public void OnUploader()
        {
            if (Uploader == null)
            {
                return;
            }

            Uploader(this, EventArgs.Empty);
        }




        HiddenField m_HideField;

        /// <summary>
        /// 上传文件的路径
        /// </summary>
        string m_UploadUrl = "";

        string m_FilePostName = "";

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        [DefaultValue("")]
        public string FilePostName
        {
            get {return m_FilePostName;}
            set { m_FilePostName =value;}
        }

        /// <summary>
        /// 上传文件的路径
        /// </summary>
        [DefaultValue("")]
        [Description("上传文件的路径")]
        public string UploadUrl
        {
            get { return m_UploadUrl; }
            set { m_UploadUrl = value; }
        }

        private new void Init()
        {
           
        }

        protected override void CreateChildControls()
        {
            m_HideField.ID = this.ClientID + this.ClientIDSeparator + "SrcName";
        }


        /// <summary>
        /// 加载 Post 上传的数据
        /// </summary>
        public void LoadPostData()
        {
            //string name2 = this.GetClientID() + this.ClientIDSeparator + "SrcName";
            //m_HideField.Value = Page.Request.Form[name2];
        }

        /// <summary>
        /// 图片名称
        /// </summary>
        [DefaultValue("")]
        [Description("图片的原始名称")]
        [Browsable(false)]
        public string FileName
        {
            get
            {
                string id = this.GetClientID();

                System.Web.HttpPostedFile pFile = this.Page.Request.Files[id];

                return pFile.FileName;
            }
        }

        /// <summary>
        /// 文件的大小
        /// </summary>
        [Browsable(false)]
        public byte[] FileBytes
        {
            get
            {
                string id = this.GetClientID();

                System.Web.HttpPostedFile pFile = this.Page.Request.Files[id];

                byte[] bs = new byte[pFile.ContentLength];

                pFile.InputStream.Position = 0;

                int len = pFile.InputStream.Read(bs, 0, pFile.ContentLength);

                return bs;
            }
        }

        /// <summary>
        /// 文件上下
        /// </summary>
        [Browsable(false)]
        public System.IO.Stream FileContent
        {
            get
            {
                string id = this.GetClientID();

                System.Web.HttpPostedFile pFile = this.Page.Request.Files[id];

                return pFile.InputStream;
            }
        }

        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="filename">文件名</param>
        public void SaveAs(string filename)
        {
            string id = this.GetClientID();

            System.Web.HttpPostedFile pFile = this.Page.Request.Files[id];

            FileInfo fi = new FileInfo(filename);

            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }

            pFile.SaveAs(filename);
        }

        public string GetParentUserControlID()
        {
            Control parent = this.Parent;

            for (int i = 0; i < 99; i++)
            {
                if (parent is UserControl)
                {
                    break;
                }

                parent = parent.Parent;
            }

            return parent.ClientID;
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                base.RenderControl(writer);
                writer.Write("<input type='button' value='选择...' />");
                
                return;
            }

            base.RenderControl(writer);

            m_HideField.RenderControl(writer);


            string cID = this.GetClientID();


            writer.Write("<span id='{0}'>选择...</span>", cID + "_Button");
            writer.WriteLine("<div id='{0}' style='height: 75px;display:none; '></div>", cID + "_FileProgressContainer");

            writer.WriteLine("<script type=\"text/javascript\">");
            writer.WriteLine("<!--");

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }


            if (jsMode == "InJs" || jsMode == "MInJs")
            {
                writer.WriteLine("In.ready('mi.SWFUpload',function () {");
            }
            else
            {
                writer.WriteLine("$(document).ready(function(){");
            }

            writer.WriteLine("  var swfu = new Mini.ui.SWFUpload({");
            writer.WriteLine("      ID: '{0}'",cID);
            writer.WriteLine("     ,ASPSESSID: '{0}'", Page.Session.SessionID);
            writer.WriteLine("     ,Upload_Url: '{0}'", this.UploadUrl);
            writer.WriteLine("     ,AppRelativeVirtualPath: '{0}'",this.TemplateControl.AppRelativeVirtualPath);
            writer.WriteLine("     ,File_Post_Name : '{0}'", (string.IsNullOrEmpty(this.FilePostName) ? cID : this.FilePostName));
            writer.WriteLine("     ,SubName :'{0}'", cID);
            writer.WriteLine("     ,CID:'{0}'", GetParentUserControlID() );
            writer.WriteLine("  });");

            writer.WriteLine("  window.{0} = swfu;", cID);

            writer.WriteLine("});");

            writer.WriteLine("-->");
            writer.WriteLine("</script>");

        }



    }
}
