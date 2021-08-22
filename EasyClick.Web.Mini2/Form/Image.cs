using EasyClick.Web.Mini;
using EC5.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 指定图像在 ImageSizeMode 中的定位方式。
    /// </summary>
    public enum ImageSizeMode
    {
        /// <summary>
        /// 图像被置于 System.Windows.Forms.PictureBox 的左上角。如果图像比包含它的 System.Windows.Forms.PictureBox
        ///     大，则该图像将被剪裁掉。
        /// </summary>
        Normal ,
        /// <summary>
        /// System.Windows.Forms.PictureBox 中的图像被拉伸或收缩，以适合 System.Windows.Forms.PictureBox
        ///     的大小。
        /// </summary>
        StretchImage ,

        /// <summary>
        /// 调整 System.Windows.Forms.PictureBox 大小，使其等于所包含的图像大小。
        /// </summary>
        AutoSize,
        /// <summary>
        /// 如果 System.Windows.Forms.PictureBox 比图像大，则图像将居中显示。如果图像比 System.Windows.Forms.PictureBox
        ///     大，则图片将居于 System.Windows.Forms.PictureBox 中心，而外边缘将被剪裁掉。
        /// </summary>
        CenterImage,
        /// <summary>
        /// 图像大小按其原有的大小比例被增加或减小。
        /// </summary>
        Zoom
    }

    /// <summary>
    /// 标签
    /// </summary>
    [Description("标签")]
    public class Image : FieldBase, IMiniControl
    {
        /// <summary>
        /// (构造函数)标签
        /// </summary>
        public Image()
        {
            this.InReady = "Mini2.ui.form.field.Image";
            this.JsNamespace = "Mini2.ui.form.field.Image";
        }


        /// <summary>
        /// 图片尺寸模式
        /// </summary>
        [Description("图片尺寸模式")]
        [DefaultValue(ImageSizeMode.Normal)]
        public ImageSizeMode SizeMode { get; set; } = ImageSizeMode.Normal;


        private void FullScript(StringBuilder sb)
        {

            string clientId = this.ClientID;

            Labelable lab = this.Labelable;

            sb.AppendLine("var field = Mini2.create('" + this.JsNamespace + "', {");
            JsParam(sb, "id", this.ID);
            JsParam(sb, "clientId", clientId);
            JsParam(sb, "name", StringUtil.NoBlank(this.Name, clientId));

            if (this.SubItemMode)
            {
                JsParam(sb, "subItemMode", this.SubItemMode, false);

                sb.AppendLine("    parentEl: e.itemEl ,");
            }

            JsParam(sb, "dataField", this.DataField);

            JsParam(sb, "sizeMode", this.SizeMode, ImageSizeMode.Normal, TextTransform.Lower);

            JsParam(sb, "value", JsonUtil.ToJson(this.Value, JsonQuotationMark.SingleQuotes));

            JsParam(sb, "left", this.Left);
            JsParam(sb, "top", this.Top);

            JsParam(sb, "position", this.Position, StylePosition.Static, TextTransform.Lower);

            JsParam(sb, "height", this.Height);
            JsParam(sb, "width", this.Width, "100%");

            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "maxWidth", this.MaxWidth);

            JsParam(sb, "minHeight", this.MinHeight);
            JsParam(sb, "maxHeight", this.MaxHeight);

            JsParam(sb, "fieldLabel", JsonUtil.ToJson(lab.FieldLabel, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);

            JsParam(sb, "dock", this.Dock, DockStyle.None, TextTransform.Lower);
            JsParam(sb, "visible", this.Visible, true);

            JsParam(sb, "bodyAlign", this.BodyAlign, TextAlign.Left, TextTransform.Lower);

            JsParam(sb,"secFunCode", this.SecFunCode);   //权限编码
            JsParam(sb, "secReadonly", this.SecReadonly);   //只读权限


            if (this.SubItemMode)
            {
                sb.AppendFormat("    applyTo: '[data-id=\\\'{0}\\\']'", this.ID).AppendLine();
            }
            else
            {
                sb.AppendFormat("    applyTo: '#{0}'", clientId).AppendLine();
            }

            sb.AppendLine("});");

            sb.AppendLine("field.render();");

            if (this.SubItemMode)
            {

            }
            else
            {
                sb.AppendFormat("window.{0} = field;\n", clientId);
                sb.AppendFormat("Mini2.onwerPage.controls['{0}'] = field;\n", this.ID);
            }


        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            string clientId = this.ClientID;

            if (this.SubItemMode)
            {
                DataViewItem item = new DataViewItem();
                

                sb.AppendFormat("<img data-id=\"{0}\" style=\"display:none;\" />", this.ClientID);

                writer.Write(sb.ToString());

                StringBuilder sbJs = new StringBuilder();

                FullScript(sbJs);

                
                this.SubScript.Add(sbJs.ToString());

                return;
            }

            sb.AppendFormat("<img id=\"{0}\" style=\"display:none;\" />", this.ClientID);


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

        public void LoadPostData()
        {

        }
    }
}
