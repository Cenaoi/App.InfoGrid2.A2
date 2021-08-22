using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 下拉框基类
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    [DefaultProperty("Value")]
    public class ComboBoxBase : TriggerBox
    {
        /// <summary>
        /// 
        /// </summary>
        public ComboBoxBase()
        {
            this.InReady = "Mini2.ui.form.field.ComboBoxBase";

            this.JsNamespace = "Mini2.ui.form.field.ComboBoxBase";
        }

        string m_ValueField;

        string m_DisplayField;

        /// <summary>
        /// 远程数据的地址
        /// </summary>
        string m_RemoteUrl { get; set; }

        /// <summary>
        /// 激活远程数据
        /// </summary>
        bool m_RemoteEnabeld { get; set; } = false;




        CheckedMode m_CheckedMode = CheckedMode.Single;


        ListItemBaseCollection m_Items;

        #region 调用远程数据过来

        /// <summary>
        /// 数据源的远程地址
        /// </summary>
        public string RemoteUrl { get; set; }

        /// <summary>
        /// 远程数据激活
        /// </summary>
        public bool RemoteEnabled { get; set; }

        /// <summary>
        /// 调用远程数据用到的参数
        /// </summary>
        ParamCollection m_RemoteParams;

        /// <summary>
        /// 调用远程数据用到的参数
        /// </summary>
        [Description("调用远程数据用到的参数")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ParamCollection RemoteParams
        {
            get
            {
                if(m_RemoteParams == null)
                {
                    m_RemoteParams = new ParamCollection();
                }
                return m_RemoteParams;
            }
        }

        #endregion

        /// <summary>
        /// 值的字段名
        /// </summary>
        public string ValueField
        {
            get { return m_ValueField; }
            set { m_ValueField = value; }
        }

        /// <summary>
        /// 显示的字段名
        /// </summary>
        [DefaultValue("")]
        public string DisplayField
        {
            get { return m_DisplayField; }
            set { m_DisplayField = value; }
        }


        /// <summary>
        /// 选择的模式: 单选,多选
        /// </summary>
        [DefaultValue(CheckedMode.Single)]
        public CheckedMode CheckedMode
        {
            get { return m_CheckedMode; }
            set { m_CheckedMode = value; }
        }



        /// <summary>
        /// 条目集合
        /// </summary>
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public virtual ListItemBaseCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    if (this.DesignMode)
                    {
                        m_Items = new ListItemBaseCollection();
                    }
                    else
                    {
                        m_Items = new ListItemBaseCollection(this);
                    }
                }

                return m_Items;
            }

        }

        /// <summary>
        /// 存在远程参数
        /// </summary>
        /// <returns></returns>
        public bool HasRemoteParams()
        {
            return m_RemoteParams != null && m_RemoteParams.Count > 0;
        }


        private void FullScript(StringBuilder sb)
        {

            string clientId = this.ClientID;
            Labelable lab = this.Labelable;

            sb.AppendLine("var field = Mini2.create('" + this.JsNamespace + "', {");

            JsParam(sb, "id", this.ID);
            JsParam(sb, "clientId", clientId);

            JsParam(sb, "name", StringUtil.NoBlank(this.Name,clientId ));
            JsParam(sb, "dataField", this.DataField);
            JsParam(sb, "value",  JsonUtil.ToJson(this.Value, JsonQuotationMark.SingleQuotes));


            //空项
            JsParam(sb, "emptyEnabled", this.EmptyEnabled, true);
            JsParam(sb, "emptyValue", this.EmptyValue);
            JsParam(sb, "emptyText", this.EmptyText);



            JsParam(sb, "width", this.Width, "100%");
            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "maxWidth", this.MaxWidth);

            JsParam(sb, "buttonType", this.ButtonType, TriggerButtonType.Default, TextTransform.Lower);
            JsParam(sb, "buttonClass", this.ButtonClass);

            sb.AppendFormat("mode: '{0}',", ((this.TriggerMode == TriggerMode.None) ? "none" : "user_input"));

            JsParam(sb, "fieldLabel", JsonUtil.ToJson(lab.FieldLabel, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);

            JsParam(sb, "required", this.Required, false);

            JsParam(sb, "placeholder", JsonUtil.ToJson(this.Placeholder, JsonQuotationMark.SingleQuotes));

            JsParam(sb, "valueField", this.ValueField);
            JsParam(sb, "displayField", this.DisplayField);
            JsParam(sb, "tabStop", this.TabStop, true);

            //帮助
            JsParam(sb, "hideHelper", this.HideHelper, false);
            JsParam(sb, "helperText", this.HelperText);
            JsParam(sb, "helperLayout", this.HelperLayout, HelperLayouts.Rigth, TextTransform.Lower);
            JsParam(sb, "helperStyle", this.HelperStyle, HelperStyles.Icon, TextTransform.Lower);

            //远程数据
            JsParam(sb, "remoteEnabled", this.RemoteEnabled, false);
            JsParam(sb, "remoteUrl", this.RemoteUrl);

            if(this.HasRemoteParams())
            {
                sb.Append("    remoteParams: ").Append(this.RemoteParams.ToJson()).AppendLine(",");
            }


            JsParam(sb, "checkedMode", m_CheckedMode, CheckedMode.Single, TextTransform.Lower);

            JsParam(sb, "dock", this.Dock, DockStyle.None, TextTransform.Lower);
            JsParam(sb, "visible", this.Visible, true);
            JsParam(sb, "dirty", this.Dirty, false);
            JsParam(sb, "readOnly", this.ReadOnly, false);

            JsParam(sb,"secFunCode", this.SecFunCode);   //权限编码
            JsParam(sb, "secReadonly", this.SecReadonly);   //只读权限

            //条目发生变化,触发的事件名
            if (!string.IsNullOrEmpty(this.OnChanged))
            {
                sb.AppendLine("changed: function(){ " + this.OnChanged + ";},");
            }

            if (!StringUtil.IsBlank(this.OnButtonClick))
            {
                sb.AppendLine("    buttonClick:function(owner){");

                sb.AppendLine("        return " + this.OnButtonClick + ";");

                sb.AppendLine("    },");
            }

            if (!string.IsNullOrEmpty(this.OnDropDown))
            {
                sb.AppendLine("    dropDown:function(owner){");

                sb.AppendLine("        return " + this.OnDropDown + ";");

                sb.AppendLine("    },");
            }


            sb.AppendFormat("    fields: ['{0}', '{1}'],", m_ValueField , m_DisplayField);
            sb.AppendLine("    store:[");

            ListItemBase item;

            for (int i = 0; i < this.Items.Count; i++)
            {
                if (i > 0) { sb.Append(", "); }

                item = this.Items[i];
                sb.Append(item.GetJson());
            }

            sb.AppendLine("    ],");


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
            

            sb.AppendFormat("  applyTo: '#{0}'", clientId).AppendLine();
            sb.AppendLine("});");

            sb.AppendLine("field.render();");


            ChangedCallbackScript(sb, "field");


            sb.AppendFormat("window.{0} = field;\n", clientId);
            sb.AppendFormat("Mini2.onwerPage.controls['{0}'] = field;\n", this.ID);
            //sb.AppendLine("});");

        }



        protected override void Render(HtmlTextWriter writer)
        {

            if (this.DesignMode)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            string clientId = this.ClientID;

            
            sb.AppendFormat("<select id=\"{0}\" style=\"width: 100%; display:none;\"></select>", this.ClientID);

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
