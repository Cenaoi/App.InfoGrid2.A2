using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{
    [DefaultProperty("Value")]
    [ParseChildren(true, "Value")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:HtmlTextarea  runat=\"server\" />")]
    public class HtmlTextarea:Textarea
    {
        public HtmlTextarea()
            : base()
        {
            this.SetValueScript = "CKEDITOR.instances.{0}.setData(\"{1}\")";
        }

        string m_UpdateDir;

        string m_CKFinderPath = "/Core/Scripts/CKFinder_2.1.1/";
        
        string m_CustomConfig;

        string m_ToolbarName;

        string m_FileBrowserUploadUrl = "/app/EShop/Bll/UploadFile.aspx?command=QuickUpload&type=Images";

        /// <summary>
        /// 文件上传地址
        /// </summary>
        [DefaultValue("")]
        public string FileBrowserUploadUrl
        {
            get { return m_FileBrowserUploadUrl; }
            set { m_FileBrowserUploadUrl = value; }
        }

        [DefaultValue("")]
        public string ToolbarName
        {
            get { return m_ToolbarName; }
            set { m_ToolbarName = value; }
        }

        /// <summary>
        /// 自定义配置文件的路径
        /// </summary>
        [DefaultValue("")]
        [Description("自定义配置文件的路径")]
        public string CustomConfig
        {
            get { return m_CustomConfig; }
            set { m_CustomConfig = value; }
        }

        /// <summary>
        /// CKFinder 编辑器路径
        /// </summary>
        public string CKFinderPath
        {
            get { return m_CKFinderPath; }
            set { m_CKFinderPath = value; }
        }

        /// <summary>
        /// 上传的目录路径
        /// </summary>
        [DefaultValue("")]
        [Description("上传的目录路径")]
        public string UpdateDir
        {
            get { return m_UpdateDir; }
            set { m_UpdateDir = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            CKEditor_Render(writer);
        }



        private void CKEditor_Render(HtmlTextWriter writer)
        {
            if (!m_HtmlAttrs.ContainsAttr("SubmitBufore"))
            {
                string jsCode = string.Format("$('#{0}').val(CKEDITOR.instances.{0}.getData())", GetClientID());
                m_HtmlAttrs.SetAttribute("SubmitBufore", jsCode);
            }

            string ckFinderPath = m_CKFinderPath;   // "/Core/Scripts/CKFinder_2.1.1/";

            string webDir = "&basePath=" + HttpUtility.UrlEncode(m_UpdateDir);

            RenderMiniControl(writer);

            //writer.WriteLine("<script type=\"text/javascript\">");
            //writer.WriteLine("  CKEDITOR.replace(\"{0}\",", GetClientID());
            //writer.WriteLine("{");
            //writer.WriteLine("skin : 'v2',");
            //writer.WriteLine("filebrowserBrowseUrl: '{0}ckfinder.html',", ckFinderPath);
            //writer.WriteLine("filebrowserImageBrowseUrl: '{0}ckfinder.html?Type=Images{1}',", ckFinderPath, webDir);
            //writer.WriteLine("filebrowserFlashBrowseUrl: '{0}ckfinder.html?Type=Flash{1}',", ckFinderPath, webDir);
            //writer.WriteLine("filebrowserUploadUrl: '{0}core/connector/aspx/connector.aspx?command=QuickUpload&type=Files{1}',", ckFinderPath, webDir);
            //writer.WriteLine("filebrowserImageUploadUrl: '{0}core/connector/aspx/connector.aspx?command=QuickUpload&type=Images{1}',", ckFinderPath, webDir);
            //writer.WriteLine("filebrowserFlashUploadUrl: '{0}core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash{1}',", ckFinderPath, webDir);
            //writer.WriteLine("});");
            //writer.WriteLine("</script>");

            writer.WriteLine("<script type=\"text/javascript\">");
            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }



            if (jsMode == "InJs" || jsMode == "MInJs")
            {
                writer.WriteLine("In.ready(\"ckeditor\",function(){");


                writer.WriteLine("    function LocalQuery(){ return location.href.substring(location.href.indexOf('?') + 1, location.href.length); }");


                writer.WriteLine("  CKEDITOR.replace(\"{0}\",", GetClientID());
                writer.WriteLine("  {");

                if (!string.IsNullOrEmpty(m_CustomConfig))
                {
                    writer.WriteLine("    customConfig:'{0}',", m_CustomConfig);
                }

                if (!string.IsNullOrEmpty(m_ToolbarName))
                {
                    writer.WriteLine("    toolbar:'{0}',", m_ToolbarName);
                }

                writer.WriteLine("    skin : 'v2',");
                writer.WriteLine("    filebrowserUploadUrl: '" + m_FileBrowserUploadUrl + "&' + LocalQuery()");
                writer.WriteLine("  });");


                writer.WriteLine("});");
            }
            else
            {
                writer.WriteLine("    function LocalQuery(){ return location.href.substring(location.href.indexOf('?') + 1, location.href.length); }");


                writer.WriteLine("  CKEDITOR.replace(\"{0}\",", GetClientID());
                writer.WriteLine("  {");

                if (!string.IsNullOrEmpty(m_CustomConfig))
                {
                    writer.WriteLine("    customConfig:'{0}',", m_CustomConfig);
                }

                if (!string.IsNullOrEmpty(m_ToolbarName))
                {
                    writer.WriteLine("    toolbar:'{0}',", m_ToolbarName);
                }

                writer.WriteLine("    skin : 'v2',");
                writer.WriteLine("    filebrowserUploadUrl: '" + m_FileBrowserUploadUrl + "&' + LocalQuery()");
                writer.WriteLine("  });");
            }

            writer.WriteLine("</script>");
        }

        private void KindEditor_Render(HtmlTextWriter writer)
        {
            if (!m_HtmlAttrs.ContainsAttr("SubmitBufore"))
            {
                string jsCode = string.Format("$('#{0}').val(CKEDITOR.instances.{0}.getData())", GetClientID());
                m_HtmlAttrs.SetAttribute("SubmitBufore", jsCode);
            }

            RenderMiniControl(writer);

            writer.WriteLine("<script type=\"text/javascript\">");

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }

            if (jsMode == "InJs")
            {
                writer.WriteLine("In.ready('kindeditor',function(){");
            }
            else
            {
                writer.WriteLine("$(document).ready(function(){");
            }

            writer.WriteLine("    var editor = KindEditor.create('#{0}');", GetClientID());
            writer.WriteLine("});");
            writer.WriteLine("</script>");
        }


    }
}
