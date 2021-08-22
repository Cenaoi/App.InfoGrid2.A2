using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 下拉表格
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false)]
    [DefaultProperty("Value")]
    public class ComboGrid :TriggerBox
    {
        /// <summary>
        /// 下拉表格
        /// </summary>
        public ComboGrid()
        {
            this.InReady = "Mini2.ui.form.field.ComboGrid";

            this.JsNamespace = "Mini2.ui.form.field.ComboGrid";
        }

        string m_ValueField;

        string m_DisplayField;

        CheckedMode m_CheckedMode = CheckedMode.Single;

        /// <summary>
        /// 数据列集合
        /// </summary>
        TableColumnCollection m_Columns;

        string m_CustomData;

        ArrayList m_Data;


        /// <summary>
        /// 数据列集合
        /// </summary>
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("数据列集合")]
        public virtual TableColumnCollection Columns
        {
            get
            {
                if (m_Columns == null)
                {
                    m_Columns = new TableColumnCollection(this);
                }

                return m_Columns;
            }
        }

               

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

        public ArrayList Data
        {
            get
            {
                if(m_Data == null)
                {
                    m_Data = new ArrayList();
                }
                return m_Data;
            }
            set { m_Data = value; }
        }
        

        private void FullScript(StringBuilder sb)
        {

            string clientId = this.ClientID;
            Labelable lab = this.Labelable;

            ScriptTextWriter st = new ScriptTextWriter(sb, QuotationMarkConvertor.SingleQuotes);

            st.RetractBengin("var field = Mini2.create('" + this.JsNamespace + "', {");
            {
                st.WriteParam("id", this.ID);
                st.WriteParam("clientId", clientId);
                st.WriteParam("name", StringUtil.NoBlank(this.Name, clientId));

                st.WriteParam("applyTo", "#" + this.ClientID);

                st.WriteParam("dataField", this.DataField);
                st.WriteParam("value", this.Value);

                st.WriteParam("valueField", this.ValueField);
                st.WriteParam("displayField", this.DisplayField);


                //空项
                st.WriteParam("emptyEnabled", this.EmptyEnabled, true);
                st.WriteParam("emptyValue", this.EmptyValue);
                st.WriteParam("emptyText", this.EmptyText);


                st.WriteParam("width", this.Width);
                st.WriteParam("minWidth", this.MinWidth);
                st.WriteParam("maxWidth", this.MaxWidth);

                st.WriteParam("mode", this.TriggerMode,TriggerMode.UserInput, TextTransform.Lower);

                st.WriteParam("fieldLabel", this.FieldLabel);
                st.WriteParam("labelAlign", lab.LabelAlign, TextAlign.Left, TextTransform.Lower);
                st.WriteParam("required", this.Required, false);
                st.WriteParam("placeholder", this.Placeholder);
                st.WriteParam("tabStop", this.TabStop, true);

                //帮助
                st.WriteParam("hideHelper", this.HideHelper, false);
                st.WriteParam("helperText", this.HelperText);
                st.WriteParam("helperLayout", this.HelperLayout, HelperLayouts.Rigth, TextTransform.Lower);
                st.WriteParam("helperStyle", this.HelperStyle, HelperStyles.Icon, TextTransform.Lower);

                st.WriteParam("checkedMode", this.CheckedMode, CheckedMode.Single, TextTransform.Lower);
                st.WriteParam("dock", this.Dock, DockStyle.None, TextTransform.Lower);
                st.WriteParam("visible", this.Visible, true);
                st.WriteParam("dirty", this.Dirty);
                st.WriteParam("readOnly", this.ReadOnly);

                st.WriteParam("secFunCode", this.SecFunCode);   //权限编码
                st.WriteParam("secReadonly", this.SecReadonly);   //只读权限

                if (this.Columns.Count > 0)
                {
                    st.WriteLine(",");
                    st.RetractBengin("columns:[");


                    string colJs;

                    int i = 0;

                    foreach (BoundField field in this.Columns)
                    {
                        if (!field.Visible)
                        {
                            continue;
                        }

                        colJs = field.CreateHtmlTemplate(Mini.MiniDataControlCellType.DataCell, Mini.MiniDataControlRowState.Normal);

                        if(i++ > 0) { st.WriteLine(","); }

                        st.Write(colJs);

                    }

                    st.RetractEnd("]");
                    
                }



                if (!this.Page.IsPostBack)
                {
                    //sb.AppendLine();
                    //JsParam(sb, "current", m_CurrentIndex,-1);
                    st.Write(",");

                    if (!string.IsNullOrEmpty(m_CustomData))
                    {
                        st.WriteCodeLine($"data:{m_CustomData}");
                    }
                    else if (m_Data != null && m_Data.Count > 0)
                    {
                        StringBuilder dataSb = new StringBuilder();

                        ArrayList dataList = m_Data;

                        string fieldJson;

                        for (int i = 0; i < dataList.Count; i++)
                        {
                            if (i > 0) { dataSb.AppendLine(","); };

                            var data = dataList[i];

                            if (data is string)
                            {
                                dataSb.Append(data);
                            }
                            else
                            {
                                fieldJson = Mini.MiniConfiguration.JsonFactory.GetItemJson(data);

                                dataSb.Append(fieldJson);
                            }
                        }
                        
                        st.WriteCodeLine($"data:[ {dataSb.ToString()} ]");
                    }
                }


                if (!StringUtil.IsBlank(this.OnChanged))
                {
                    st.WriteFunction("changed", $"function(){{ {this.OnChanged} }}");
                }

                if (!StringUtil.IsBlank(this.OnDropDown))
                {
                    st.WriteFunction("dropDown", $"function(owner) {{ {this.OnDropDown}; }}");
                }



            }
            st.RetractEnd("});");


            st.WriteCodeLine("field.render();");
            st.WriteCodeLine($"window.{this.ClientID} = field;");

            st.WriteCodeLine($"Mini2.onwerPage.controls['{this.ID}'] = field;");
            
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
