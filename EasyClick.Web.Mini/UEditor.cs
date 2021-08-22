using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 百度富文本框
    /// </summary>
    [DefaultProperty("Value")]
    [ParseChildren(true, "Value")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:UEditor  runat=\"server\" />")]
    [Description("百度富文本框")]
    public class UEditor : Textarea
    {

        public UEditor()
            : base()
        {
            this.SetValueScript = "UE.getEditor(\"{0}\").setContent(\"{1}\")";
        }



        #region 属性

        string m_ToolbarName;

        string m_UEditorPath;

        string m_UploadUrl = "/app/EShop/Bll/UploadFile.aspx?command=QuickUpload&type=Images";

        string m_FilePostName;


        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("")]
        public string ToolbarName
        {
            get { return m_ToolbarName; }
            set { m_ToolbarName = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("")]
        public string UploadUrl
        {
            get { return m_UploadUrl; }
            set { m_UploadUrl = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("")]
        public string FilePostName
        {
            get { return m_FilePostName; }
            set { m_FilePostName = value; }
        }

        #endregion


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


        protected override void Render(HtmlTextWriter writer)
        {
            string cID = this.GetClientID();

            if (!m_HtmlAttrs.ContainsAttr("SubmitBufore"))
            {
                string jsCode = string.Format("$('#{0}').val(UE.getEditor('{0}').getContent())", GetClientID());
                m_HtmlAttrs.SetAttribute("SubmitBufore", jsCode);
            }


            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }

            

            RenderMiniControl(writer);

            writer.WriteLine("<script type=\"text/javascript\">");

            if (jsMode == "InJs" || jsMode == "MInJs")
            {
                writer.WriteLine("In.ready(\"ueditor\",function(){");


                writer.WriteLine("  var miniUe = new Mini.ui.UEditor({");
                writer.WriteLine("      ID: '{0}'", cID);
                writer.WriteLine("     ,ASPSESSID: '{0}'", Page.Session.SessionID);
                writer.WriteLine("     ,Upload_Url: '{0}'", this.UploadUrl );
                writer.WriteLine("     ,AppRelativeVirtualPath: '{0}'", this.TemplateControl.AppRelativeVirtualPath);
                writer.WriteLine("     ,File_Post_Name : '{0}'", (string.IsNullOrEmpty(this.FilePostName) ? cID : this.FilePostName));
                writer.WriteLine("     ,SubName :'{0}'", cID);
                writer.WriteLine("     ,CID:'{0}'", GetParentUserControlID());
                writer.WriteLine("  });");


                writer.WriteLine("});");
            }
            else
            {
                writer.WriteLine("$(document).ready(function(){");

                writer.WriteLine("  var miniUe = new Mini.ui.UEditor({");
                writer.WriteLine("      ID: '{0}'", cID);
                writer.WriteLine("     ,ASPSESSID: '{0}'", Page.Session.SessionID);
                writer.WriteLine("     ,Upload_Url: '{0}'", this.UploadUrl);
                writer.WriteLine("     ,AppRelativeVirtualPath: '{0}'", this.TemplateControl.AppRelativeVirtualPath);
                writer.WriteLine("     ,File_Post_Name : '{0}'", (string.IsNullOrEmpty(this.FilePostName) ? cID : this.FilePostName));
                writer.WriteLine("     ,SubName :'{0}'", cID);
                writer.WriteLine("     ,CID:'{0}'", GetParentUserControlID());
                writer.WriteLine("  });");

                writer.WriteLine("});");
            }


            writer.WriteLine("</script>");
        }

    }
}
