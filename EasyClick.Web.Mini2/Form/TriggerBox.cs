using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using EasyClick.Web.Mini;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 下拉框样式
    /// </summary>
    public enum TriggerMode
    {
        /// <summary>
        /// 不可编辑的下拉框
        /// </summary>
        None,
        /// <summary>
        /// 可编辑的下拉框
        /// </summary>
        UserInput,
    }

    /// <summary>
    /// 下拉框
    /// </summary>
    public class TriggerBox:TextBox
    {
        /// <summary>
        /// 带按钮的文本框。下拉框、弹出框的基类。
        /// </summary>
        public TriggerBox()
        {
            this.InReady = "Mini2.ui.form.field.Trigger";

            this.JsNamespace = "Mini2.ui.form.field.Trigger";
        }

        TriggerMode m_TriggerMode = TriggerMode.UserInput;

        string m_Content;

        string m_OnButtonClick;

        string m_ButtonClass;


        /// <summary>
        /// 按钮类型
        /// </summary>
        TriggerButtonType m_ButtonType = TriggerButtonType.Default;


        /// <summary>
        /// 下拉项目发生变化产生的事件
        /// </summary>
        string m_OnChanged;

        /// <summary>
        /// 下拉框显示前触发事件
        /// </summary>
        string m_OnDropDown;


        /// <summary>
        /// 执行字段映射函数
        /// </summary>
        string m_OnMapping;


        /// <summary>
        /// 空的项目激活
        /// </summary>
        public bool EmptyEnabled { get; set; } = true;

        /// <summary>
        /// 空的时候显示的内容
        /// </summary>
        public string EmptyValue { get; set; }

        /// <summary>
        /// 空的时候显示的文字
        /// </summary>
        public string EmptyText { get; set; } = "--请选择--";


        /// <summary>
        /// 按钮的回调事件
        /// </summary>
        public event CallbackEventHandler ButtonClickCallback;

        /// <summary>
        /// 按钮的回调数据类型
        /// </summary>
        public CallbackDataType ButtonClickCallbackDType { get; set; } = CallbackDataType.Full;


        /// <summary>
        /// 映射事件
        /// </summary>
        public string OnMaping
        {
            get { return m_OnMapping; }
            set { m_OnMapping = value; }
        }


        /// <summary>
        /// 按钮类型
        /// </summary>
        [Description("按钮类型")]
        [DefaultValue(TriggerButtonType.Default)]
        public TriggerButtonType ButtonType
        {
            get { return m_ButtonType; }
            set { m_ButtonType = value; }
        }


        /// <summary>
        /// 下拉框显示前触发事件
        /// </summary>
        [DefaultValue("")]
        [Description("下拉框显示前触发事件")]
        public string OnDropDown
        {
            get { return m_OnDropDown; }
            set { m_OnDropDown = value; }
        }

        /// <summary>
        /// 按钮点击的事件名
        /// </summary>
        [DefaultValue("")]
        [Description("下拉项目发生变化产生的事件")]
        public string OnChanged
        {
            get { return m_OnChanged; }
            set { m_OnChanged = value; }
        }

        /// <summary>
        /// 按钮样式
        /// </summary>
        [Description("按钮样式"),DefaultValue("")]
        public string ButtonClass
        {
            get { return m_ButtonClass; }
            set { m_ButtonClass = value; }
        }

        /// <summary>
        /// 按钮点击的事件名
        /// </summary>
        [DefaultValue("")]
        [Description("按钮点击的事件")]
        public string OnButtonClick
        {
            get { return m_OnButtonClick; }
            set { m_OnButtonClick = value; }
        }

        /// <summary>
        /// 下拉框模式.可编辑或不可编辑
        /// </summary>
        [Description("下拉框模式.可编辑或不可编辑")]
        [DefaultValue(TriggerMode.UserInput)]
        public TriggerMode TriggerMode
        {
            get { return m_TriggerMode; }
            set { m_TriggerMode = value; }
        }

        /// <summary>
        /// 下拉框内容。
        /// 例:  '#abcd', '.abcd'
        /// </summary>
        [DefaultValue("")]
        [Description("下拉框内容。例:  '#abcd', '.abcd'")]
        public string Content
        {
            get { return m_Content; }
            set { m_Content = value; }
        }

        

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

            JsParam(sb, "width", this.Width, "100%");
            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "maxWidth", this.MaxWidth);


            sb.AppendFormat("mode: '{0}',", ((this.TriggerMode == TriggerMode.None)?"none":"user_input"));

            JsParam(sb, "fieldLabel",  JsonUtil.ToJson(lab.FieldLabel, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);


            //帮助
            JsParam(sb, "hideHelper", this.HideHelper, false);
            JsParam(sb, "helperText", this.HelperText);
            JsParam(sb, "helperLayout", this.HelperLayout, HelperLayouts.Rigth, TextTransform.Lower);
            JsParam(sb, "helperStyle", this.HelperStyle, HelperStyles.Icon, TextTransform.Lower);


            JsParam(sb, "required", this.Required, false);

            JsParam(sb, "placeholder", this.Placeholder);
            JsParam(sb, "dirty", this.Dirty, false);
            JsParam(sb, "readOnly", this.ReadOnly, false);

            JsParam(sb, "value",  JsonUtil.ToJson(this.Value, JsonQuotationMark.SingleQuotes));

            JsParam(sb, "dock", this.Dock, DockStyle.None, TextTransform.Lower);

            JsParam(sb, "tabStop", this.TabStop, true);

            JsParam(sb,"secFunCode", this.SecFunCode);   //权限编码
            JsParam(sb, "secReadonly", this.SecReadonly);   //只读权限

            if (!string.IsNullOrEmpty(this.Tag))
            {
                JsParam(sb,"tag", MiniHelper.GetItemJson(this.Tag));
            }


            if (!string.IsNullOrEmpty(m_ButtonClass))
            {
                sb.AppendFormat("    buttonCls:'{0}',", m_ButtonClass);
            }
            else
            {
                if (m_ButtonType == TriggerButtonType.More)
                {
                    sb.Append("    buttonCls:'mi-icon-more',");
                }
            }

            JsParam(sb, "content", this.Content);

            if (!string.IsNullOrEmpty(m_OnDropDown))
            {
                sb.AppendLine("    dropDown:function(owner){");

                sb.AppendLine("        " + m_OnDropDown + ";");

                sb.AppendLine("    },");
            }

            //输入自动提示
            if (this.HasTypeahead())
            {
                sb.AppendFormat("typeahead: {0},", this.Typeahead.ToJson());

                if (this.TypeaheadParams.Count > 0)
                {
                    sb.Append("    typeaheadParams: ").Append(this.TypeaheadParams.ToJson()).AppendLine(",");
                }

                if (this.HasTypeaheadMapItems())
                {
                    sb.AppendFormat("typeaheadMapItems: {0},", this.TypeaheadMapItems.ToJson());
                }
            }

            //映射
            if (this.HasMapItems())
            {
                sb.AppendFormat("mapItems: {0},", this.MapItems.ToJson());
            }

            if (!string.IsNullOrEmpty(m_OnMapping))
            {
                sb.AppendLine("    mapping:function(owner){");
                sb.AppendLine("        " + m_OnMapping + ";");
                sb.AppendLine("    },");

            }

            if(this.ButtonClickCallback != null)
            {
                StringBuilder code = new StringBuilder();

                string method = this.ButtonClickCallback.Method.Name;

                code.AppendLine("buttonClick_Callback:function(owner){");

                // code.AppendLine("    alert( Mini2.Json.toJson(e) ); ");
                // code.AppendLine("    if( e.result != 'ok') return false;

                code.AppendLine("    widget1.submit('form', {");
                code.AppendLine("        RMode:'callback', ");
                code.AppendLine($"        callback_data: '{this.ButtonClickCallbackDType.ToString().ToLower()}', ");    //回调的数据类型
                code.AppendLine($"        action: '{method}', ");
                code.AppendLine($"        actionPs: owner.getValue() ");

                code.AppendLine("    });");

                code.AppendLine("},");

                sb.AppendLine(code.ToString());
            }

            if (!string.IsNullOrEmpty(m_OnButtonClick))
            {
                sb.AppendLine("    buttonClick:function(owner){");
                sb.AppendLine("        " + OnButtonClick + ";");
                sb.AppendLine("    },");
            }

            JsParam(sb, "visible", this.Visible, true);


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


                sb.AppendFormat("<input type=\"text\" data-id=\"{0}\" style=\"width: 100%;display:none; \" />", this.ClientID);

                writer.Write(sb.ToString());

                StringBuilder sbJs = new StringBuilder();

                FullScript(sbJs);


                this.SubScript.Add(sbJs.ToString());

                return;
            }


            sb.AppendFormat("<input type=\"text\" id=\"{0}\" style=\"width: 100%;display:none; \">", this.ClientID);

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
