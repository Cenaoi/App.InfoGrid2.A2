using EC5.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 下拉框
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    [DefaultProperty("Value")]
    public class ClipImage : FieldBase
    {

        public ClipImage()
        {
            this.InReady = "Mini2.ui.form.field.ClipImage";

            this.JsNamespace = "Mini2.ui.form.field.ClipImage";
        }


        ListItemCollection m_Items;

        /// <summary>
        /// 值的字段名
        /// </summary>
        public string ValueField { get; set; }

        /// <summary>
        /// 显示的字段名
        /// </summary>
        [DefaultValue("")]
        public string DisplayField { get; set; }
        
        /// <summary>
        /// 条目集合
        /// </summary>
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [MergableProperty(false)]
        public virtual ListItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    if (this.DesignMode)
                    {
                        m_Items = new ListItemCollection();
                    }
                    else
                    {
                        m_Items = new ListItemCollection(this);
                    }
                }

                return m_Items;
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

            if (this.SubItemMode)
            {
                JsParam(sb, "subItemMode", this.SubItemMode, false);

                sb.AppendLine("    parentEl: e.itemEl ,");
            }

            JsParam(sb, "dataField", this.DataField);
            JsParam(sb, "value", EC5.Utility.JsonUtil.ToJson(this.Value, JsonQuotationMark.SingleQuotes));

            JsParam(sb, "left", this.Left);
            JsParam(sb, "top", this.Top);


            JsParam(sb, "position", this.Position, StylePosition.Static, TextTransform.Lower);

            if (!StringUtil.IsBlank(this.DataSource))
            {
                string globelStoreId = GetDataSourceIdByPage(this.DataSource);
                
                JsParam(sb, "dataSource", globelStoreId);
            }

            JsParam(sb, "height", this.Height);
            JsParam(sb, "minHeight", this.MinHeight);
            JsParam(sb, "maxHeight", this.MaxHeight);

            JsParam(sb, "width", this.Width, "100%");
            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "maxWidth", this.MaxWidth);
            
            JsParam(sb, "fieldLabel", EC5.Utility.JsonUtil.ToJson(lab.FieldLabel, JsonQuotationMark.SingleQuotes));
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);

            JsParam(sb, "required", this.Required, false);
            
            JsParam(sb, "valueField", this.ValueField);
            JsParam(sb, "displayField", this.DisplayField);
            JsParam(sb, "tabStop", this.TabStop, true);

            
            JsParam(sb, "dock", this.Dock, DockStyle.None, TextTransform.Lower);
            JsParam(sb, "visible", this.Visible, true);

            JsParam(sb,"secFunCode", this.SecFunCode);   //权限编码
            JsParam(sb, "secReadonly", this.SecReadonly);   //只读权限

            sb.AppendFormat("    fields: ['{0}', '{1}'],", this.ValueField, this.DisplayField);
            sb.AppendLine("    store:[");

            ListItem item;

            for (int i = 0; i < this.Items.Count; i++)
            {
                if (i > 0) { sb.Append(", "); }

                item = this.Items[i];
                sb.Append(item.GetJson());
            }

            sb.AppendLine("    ],");


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



        protected override void Render(HtmlTextWriter writer)
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
                

                sb.AppendFormat("<select data-id=\"{0}\" style=\"width: 100%; display:none;\"></select>", this.ClientID);

                writer.Write(sb.ToString());

                StringBuilder sbJs = new StringBuilder();

                FullScript(sbJs);

                
                this.SubScript.Add(sbJs.ToString());

                return;
            }


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
