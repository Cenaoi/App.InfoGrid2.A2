using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    [Obsolete]
    public class CKFinderFile:TextBox
    {
        bool m_ShowPreview = false;

        string m_UpdateDir;

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

        /// <summary>
        /// 显示预览
        /// </summary>
        [DefaultValue(false)]
        [Description("显示预览")]
        public bool ShowPreview
        {
            get { return m_ShowPreview; }
            set { m_ShowPreview = value; }
        }

        protected override void RenderMiniControl(System.Web.UI.HtmlTextWriter writer)
        {
            base.RenderMiniControl(writer);

            //writer.WriteLine("<input type='text' id='{0}' name='{1}' />",GetClientID(),ClientID);
            writer.WriteLine("<input type='button' onclick='{0}_BrowseServer()' value='浏览...' />",GetClientID());

            if (m_ShowPreview)
            {
                writer.WriteLine("<a id=\"{0}_ImgPreview\" href=\"{1}\" onclick=\"return false;\">预览</a>", GetClientID(), this.Value);

                writer.WriteLine("<script>");
                writer.WriteLine("$(document).ready(function(){");
                writer.WriteLine("  try{ ");
                writer.WriteLine("    $('#{0}_ImgPreview').imgPreview(); ",GetClientID() );
                writer.WriteLine("  } catch(){ } ");
                writer.WriteLine("});");
                writer.WriteLine("</script>");
            }

            writer.WriteLine("<script>");
            writer.WriteLine("    function {0}_BrowseServer() {{",GetClientID());
            writer.WriteLine("        var finder = new CKFinder();");
            writer.WriteLine("        finder.basePath = '/Core/Scripts/CKFinder_2.1.1/'; //导入CKFinder的路径");
            writer.WriteLine("        finder.selectActionFunction = function (fileUrl, data) {");
            writer.WriteLine("            $('#{0}').val( fileUrl);", GetClientID());
            writer.WriteLine("        };");
            writer.WriteLine("        finder.selectActionData = '{0}'; //接收地址的input ID  ",GetClientID());
            writer.WriteLine("        finder.popup();");
            writer.WriteLine("    }");
            writer.WriteLine("</script>");


        }

    }
}
