using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.Web.UI;
using System.ComponentModel;
using EasyClick.Web.Mini;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 布局方向
    /// </summary>
    public enum RepeatDirection
    {
        /// <summary>
        /// 水平方向
        /// </summary>
        Vertical,
        /// <summary>
        /// 垂直方向
        /// </summary>
        Horizontal
    }

    /// <summary>
    /// 布局类型
    /// </summary>
    public enum RepeatLayout
    {
        /// <summary>
        /// 表格类型
        /// </summary>
        Table,
        /// <summary>
        /// 流模式
        /// </summary>
        Flow,
        UnorderedList,
        OrderedList
    }

    /// <summary>
    /// 复选框
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Items"), PersistChildren(false)]
    [DefaultProperty("Value")]
    public class CheckboxGroup : FieldBase
    {
        public CheckboxGroup()
        {
            this.InReady = "Mini2.ui.form.CheckboxGroup";

            this.JsNamespace = "Mini2.ui.form.CheckboxGroup";
        }


        ListItemCollection m_Items;


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

        /// <summary>
        /// 用于布局项的列数
        /// </summary>
        [DefaultValue(0)]
        public int RepeatColumns { get; set; } = 0;

        /// <summary>
        /// 布局方向
        /// </summary>
        [DefaultValue(RepeatDirection.Vertical)]
        public RepeatDirection RepeatDirection { get; set; } = RepeatDirection.Vertical;


        /// <summary>
        /// 布局类型
        /// </summary>
        [DefaultValue(RepeatLayout.Table)]
        public RepeatLayout RepeatLayout { get; set; } = RepeatLayout.Table;


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
            JsParam(sb, "height", this.Height);

            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "maxWidth", this.MaxWidth);


            JsParam(sb, "fieldLabel", lab.FieldLabel);
            JsParam(sb, "labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
            JsParam(sb, "hideLabel", lab.HideLabel, false);
            JsParam(sb, "labelWidth", lab.LabelWidth, 100);

            JsParam(sb, "required", this.Required, false);

            JsParam(sb, "tabStop", this.TabStop, true);

            JsParam(sb, "dock", this.Dock, DockStyle.None, TextTransform.Lower);
            JsParam(sb, "visible", this.Visible, true);
            JsParam(sb, "dirty", this.Dirty, false);
            JsParam(sb, "readOnly", this.ReadOnly, false);

            JsParam(sb, "repeatColumns", this.RepeatColumns, 0);
            JsParam(sb, "repeatDirection", this.RepeatDirection, RepeatDirection.Vertical, TextTransform.Lower);
            JsParam(sb, "repeatLayout", this.RepeatLayout, RepeatLayout.Table, TextTransform.Lower);


            JsParam(sb, "value", StringUtil.NoBlank(this.Value, this.DefaultValue));

            JsParam(sb,"secFunCode", this.SecFunCode);   //权限编码
            JsParam(sb, "secReadonly", this.SecReadonly);   //只读权限
            

            sb.AppendLine("    items:[");

            if (this.Items.Count > 0)
            {
                ListItem item;

                bool check = false;

                string[] values = StringUtil.Split(this.DefaultValue, ",");

                for (int i = 0; i < this.Items.Count; i++)
                {
                    item = this.Items[i];

                    if (i > 0) { sb.Append(","); }

                    check = ArrayUtil.Exist(values, StringUtil.NoBlank(item.Value, item.Text));

                    sb.Append(GetItemJson(item,check));
                }

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

        /// <summary>
        /// 获取 json 格式
        /// </summary>
        /// <returns></returns>
        private string GetItemJson(ListItem item, bool check)
        {
            string checkStr = BoolUtil.ToJson(check);

            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("inputValue: '{0}', boxLabel: '{1}', checked:{2}", 
                StringUtil.NoBlank(item.Value, item.Text), 
                StringUtil.NoBlank(item.Text, item.Value), checkStr);


            if (!StringUtil.IsBlank(item.TextEx))
            {
                sb.AppendFormat(", textEx:'{0}'", item.TextEx);
            }

            if (!StringUtil.IsBlank(item.MinWidth))
            {
                sb.AppendFormat(", minWidth:'{0}'", item.MinWidth);
            }

            if (!StringUtil.IsBlank(item.MaxWidth))
            {
                sb.AppendFormat(", maxWidth:'{0'", item.MaxWidth);
            }

            sb.Append("}");

            return sb.ToString();
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
                

                sb.AppendFormat("<div data-id=\"{0}\" style=\"display:none;\"></div>", this.ClientID);

                writer.Write(sb.ToString());

                StringBuilder sbJs = new StringBuilder();

                FullScript(sbJs);

                
                this.SubScript.Add(sbJs.ToString());

                return;
            }

            sb.AppendFormat("<div id=\"{0}\" style=\"display:none;\"></div>", this.ClientID);


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
