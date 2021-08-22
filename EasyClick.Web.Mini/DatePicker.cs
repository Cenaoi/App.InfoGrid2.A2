using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 日期选择框
    /// </summary>
    [Description("日期选择框")]
    [DefaultProperty("Value")]
    [ToolboxData("<{0}:DatePicker  runat=\"server\" />")]
    [ParseChildren(true), PersistChildren(false)]
    public class DatePicker:TextBox,IValueFormat
    {

        DateTime m_MinDate = DateTime.MinValue;

        string m_ValueFormat;
        bool m_ButtonImageOnly = false;

        string m_ButtonImage;

        [DefaultValue("")]
        public string ButtonImage
        {
            get { return m_ButtonImage;}
            set { m_ButtonImage = value;}
        }


        [DefaultValue(false)]
        public bool ButtonImageOnly
        {
            get { return m_ButtonImageOnly;}
            set { m_ButtonImageOnly = value;}
        }



        public string MinDate
        {
            get
            {
                if (m_MinDate == DateTime.MinValue)
                {
                    return string.Empty;
                }
                return m_MinDate.ToString();
            }
            set
            {
                if (!DateTime.TryParse(value, out m_MinDate))
                {
                    m_MinDate = DateTime.MinValue;
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                base.Render(writer);

                writer.Write("<button >...</button>");

                return;
            }

            base.Render(writer);

            DateTime minDate = DateTime.Today;

            writer.WriteLine("<script type=\"text/javascript\">");
            writer.WriteLine("<!--");

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }


            if (jsMode == "InJs")
            {
                writer.WriteLine("In.ready('jq.ui',function(){");
            }
            else
            {
                writer.WriteLine("$(document).ready(function(){");
            }

            //writer.WriteLine("$(document).ready(function() {");
            //writer.WriteLine("  $('#{0}').datepicker( 'option', $.datepicker.regional[ 'zh-CN' ] );", this.GetClientID());
            writer.WriteLine("  $('#{0}').datepicker({{", this.GetClientID());
            writer.WriteLine("      showButtonPanel: true ,");
            writer.WriteLine("      showAnim:'',changeMonth: true,changeYear: true ,");
		    writer.WriteLine("      showOn: 'button',");
		    writer.WriteLine("	    buttonImage: '{0}',",this.m_ButtonImage);
		    writer.WriteLine("	    buttonImageOnly: {0},", this.ButtonImageOnly.ToString().ToLower());

            if (m_MinDate != DateTime.MinValue)
            {
                writer.WriteLine("      minDate: new Date({0}, {1}, {2}),", m_MinDate.Year, m_MinDate.Month, m_MinDate.Day);
            }

            writer.WriteLine("      dateFormat:'yy-mm-dd'");
		    writer.WriteLine("  });");

            writer.WriteLine("  try{");
            writer.WriteLine("    $('#{0}').datepicker('show');", this.GetClientID());
            writer.WriteLine("  } catch(ex){ }");

            writer.WriteLine("  try{");
            writer.WriteLine("    $('#{0}').datepicker('hide');", this.GetClientID());
            writer.WriteLine("  }catch(ex){ }");

            writer.WriteLine("});");

            writer.WriteLine("-->");
            writer.WriteLine("</script>");


        }

        [DefaultValue("")]
        public string ValueFormat
        {
            get
            {
                return m_ValueFormat;
            }
            set
            {
                m_ValueFormat = value;
            }
        }

        
        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
            }
        }

    }
}
