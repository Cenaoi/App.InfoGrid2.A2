using EC5.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 文件上传插件
    /// </summary>
    public enum FileUploadPluginType
    {
        /// <summary>
        /// 没指定类型
        /// </summary>
        None,
        /// <summary>
        /// 图像类型
        /// </summary>
        Image,
        /// <summary>
        /// 其它类型
        /// </summary>
        OtherFile
    }

    /// <summary>
    /// 上传文件表单数据
    /// </summary>
    public enum FileUploadFormType
    {
        /// <summary>
        /// 简单模式
        /// </summary>
        Sampel,

        /// <summary>
        /// 完全提交
        /// </summary>
        Full
    }

    public enum FileUploadDisplayMode
    {
        /// <summary>
        /// 无效
        /// </summary>
        None,
        /// <summary>
        /// 编辑
        /// </summary>
        Editor,
        /// <summary>
        /// 预览
        /// </summary>
        Preview
    }

    /// <summary>
    /// 文件上传
    /// </summary>
    public class FileUpload : FieldBase, Mini.IMiniControl
    {

        /// <summary>
        /// (构造函数) 文件上传
        /// </summary>
        public FileUpload()
        {
            
            this.InReady = "Mini2.ui.form.field.FileUpload";
            this.JsNamespace = "Mini2.ui.form.field.FileUpload";
        }
        
        /// <summary>
        /// 展示模式
        /// </summary>
        [DefaultValue(FileUploadDisplayMode.Editor)]
        public FileUploadDisplayMode DisplayMode { get; set; } = FileUploadDisplayMode.Editor;

        /// <summary>
        /// 表单提交类型
        /// </summary>
        [DefaultValue(FileUploadFormType.Full)]
        public FileUploadFormType FormType { get; set; } = FileUploadFormType.Full;

        /// <summary>
        /// 按钮名称
        /// </summary>
        public string Text { get; set; } = "选择文件";

        /// <summary>
        /// 上传文件的接收路径
        /// </summary>
        [Description("上传文件的接收路径")]
        public string FileUrl { get; set; }

        /// <summary>
        /// 上传完成后，触发的事件
        /// </summary>
        [Description("上传完成后，触发的事件")]
        public event EventHandler Uploader;

        /// <summary>
        /// 上传完成。触发事件
        /// </summary>
        public void OnUploader()
        {
            Uploader?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 插件类型，上传图片
        /// </summary>
        [Description("插件类型，上传图片")]
        [DefaultValue(FileUploadPluginType.Image)]
        public FileUploadPluginType PluginType { get; set; } = FileUploadPluginType.Image;

        /// <summary>
        /// 缩略图宽度
        /// </summary>
        [DefaultValue(100)]
        public int ThumbWidth { get; set; } = 100;


        /// <summary>
        /// 缩略图宽度
        /// </summary>
        [DefaultValue(100)]
        public int ThumbHeight { get; set; } = 100;


        string m_TempValue;


        /// <summary>
        /// 过滤,扩展名称过滤. 例如: .xml|.xls|.jpg
        /// </summary>
        public string Filter { get; set; }


        /// <summary>
        /// 上传后临时文件名,或临时存储的位置
        /// </summary>
        public string TempValue
        {
            get
            {
                return m_TempValue;
            }
            set
            {
                m_TempValue = value;

                if (this.DesignMode)
                {
                    return;
                }

                if (string.IsNullOrEmpty(value))
                {
                    ScriptManager.Eval("{0}.setTempValue(\"\");", this.ClientID);
                }
                else
                {
                    string valueStr = JsonUtil.ToJson(value, JsonQuotationMark.DoubleQuote);
                    ScriptManager.Eval("{0}.setTempValue(\"{1}\");", this.ClientID, valueStr);
                }
            }
        }

        /// <summary>
        /// 加载 Post 上传的数据
        /// </summary>
        public virtual void LoadPostData()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            System.Web.HttpRequest request = context.Request; 

            string temp_value_id = this.ClientID + "_temp_value";
            string value_id = this.ClientID + "_value";

            this.m_TempValue = request.Form[temp_value_id];

            this.Value = request.Form[value_id];
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
                string id = StringUtil.NoBlank(this.Name, this.ClientID);


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
                string id = StringUtil.NoBlank(this.Name, this.ClientID);


                System.Web.HttpPostedFile pFile = this.Page.Request.Files[id];

                byte[] bs = new byte[pFile.ContentLength];

                pFile.InputStream.Position = 0;

                int len = pFile.InputStream.Read(bs, 0, pFile.ContentLength);

                return bs;
            }
        }

        /// <summary>
        /// 文件上下文
        /// </summary>
        [Browsable(false)]
        public System.IO.Stream FileContent
        {
            get
            {
                string id = StringUtil.NoBlank(this.Name, this.ClientID);

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
            string id = StringUtil.NoBlank(this.Name, this.ClientID);

            System.Web.HttpPostedFile pFile = this.Page.Request.Files[id];

            FileInfo fi = new FileInfo(filename);

            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }

            pFile.SaveAs(filename);
        }


        string m_Tag;

        public string Tag
        {
            get { return m_Tag; }
            set
            {
                m_Tag = value;

                if (this.DesignMode)
                {
                    return;
                }

                Mini.MiniHelper.EvalFormat("{0}.setTag('{1}');", this.ClientID, JsonUtil.ToJson( value));
            }
        }



        private void FullScript(StringBuilder sb)
        {
            string clientId = this.ClientID;

            Labelable lab = this.Labelable;

            sb.AppendLine("var field = Mini2.create('" + this.JsNamespace + "', {");
            JsParam(sb, "id", this.ID);
            JsParam(sb, "clientId", clientId);
            JsParam(sb, "name", StringUtil.NoBlank(this.Name, clientId));
            JsParam(sb, "dataField", this.DataField);

            JsParam(sb, "formType", this.FormType, FileUploadFormType.Full, TextTransform.Lower);

            JsParam(sb, "server", this.TemplateControl.AppRelativeVirtualPath);
            JsParam(sb, "appRelativeVirtualPath", this.TemplateControl.AppRelativeVirtualPath);

            JsParam(sb, "fileUrl", this.FileUrl);

            JsParam(sb, "displayMode", this.DisplayMode, FileUploadDisplayMode.None, TextTransform.Lower);

            JsParam(sb, "pluginType", this.PluginType, FileUploadPluginType.Image, TextTransform.Lower);
            JsParam(sb, "thumbWidth", this.ThumbWidth);
            JsParam(sb, "thumbHeight", this.ThumbHeight);

            JsParam(sb, "width", this.Width, "100%");
            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "maxWidth", this.MaxWidth);

            JsParam(sb, "height", this.Height);
            JsParam(sb, "minHeight", this.MinHeight);
            JsParam(sb, "maxHeight", this.MaxHeight);

            JsParam(sb, "tag", this.Tag);

            JsParam(sb, "fieldLabel", lab.FieldLabel);
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);

            //帮助
            JsParam(sb, "hideHelper", this.HideHelper, false);
            JsParam(sb, "helperText", this.HelperText);
            JsParam(sb, "helperLayout", this.HelperLayout, HelperLayouts.Rigth, TextTransform.Lower);
            JsParam(sb, "helperStyle", this.HelperStyle, HelperStyles.Icon, TextTransform.Lower);


            JsParam(sb, "required", this.Required, false);

            JsParam(sb, "colspan", StringUtil.ToInt(GetAttribute("colspan")));

            JsParam(sb, "readOnly", this.ReadOnly, false);

            JsParam(sb, "text", JsonUtil.ToJson(this.Text, JsonQuotationMark.SingleQuotes));

            JsParam(sb, "temp_value", JsonUtil.ToJson(this.TempValue, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "value", JsonUtil.ToJson(this.Value, JsonQuotationMark.SingleQuotes));
            
            JsParam(sb, "visible", this.Visible, true);

            JsParam(sb,"secFunCode", this.SecFunCode);   //权限编码
            JsParam(sb, "secReadonly", this.SecReadonly);   //只读权限

            sb.AppendFormat("    applyTo: '#{0}'", clientId).AppendLine();
            sb.AppendLine("});");

            if (this.IsDelayRender)
            {
                sb.AppendLine("field.delayRender();");
            }
            else
            {
                sb.AppendLine("field.render();");
            }

            sb.AppendFormat("window.{0} = field;\n", clientId);
            sb.AppendFormat("Mini2.onwerPage.controls['{0}'] = field;\n", this.ID);

        }


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            string clientId = this.ClientID;

            sb.AppendFormat("<input type=\"text\" id=\"{0}\" style=\"width: 100%;display:none; \" />", this.ClientID);


            ScriptManager script = ScriptManager.GetManager(this.Page);

            if (script != null)
            {
                StringBuilder sbJs = new StringBuilder();

                BeginReady(sbJs);
                FullScript(sbJs);
                EndReady(sbJs);

                script.AddScript(sbJs.ToString());

                writer.Write(sb.ToString());
            }
            else
            {

                BeginScript(sb);
                BeginReady(sb);

                FullScript(sb);

                EndReady(sb);
                EndScript(sb);

                writer.Write(sb.ToString());
            }
        }


    }
}
