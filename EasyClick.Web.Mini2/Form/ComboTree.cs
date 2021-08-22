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
    /// 下拉树
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false)]
    [DefaultProperty("Value")]
    public class ComboTree :TriggerBox
    { 
        
        /// <summary>
        /// 下拉表格
        /// </summary>
        public ComboTree()
        {
            this.InReady = "Mini2.ui.form.field.ComboTree";

            this.JsNamespace = "Mini2.ui.form.field.ComboTree";
        }

        bool m_RemoteEnabled = false;

        string m_RemoteUrl; //="/api/InfoGrid2/View/Sample/Examples/WebAPI/Doc001Form?action=query_search" 

        string m_IdField;
        string m_ParentField;
        
        string m_ValueField;
        string m_DisplayField;
        
        string m_RootValue;

        /// <summary>
        /// 实体名称,或表名
        /// </summary>
        string m_Model;

        string m_SortText;
        string m_SortField;

        CheckedMode m_CheckedMode = CheckedMode.Single;

        ArrayList m_Data;

        string m_CustomData;

        TreeNodeTypeCollection m_Types;

        /// <summary>
        /// 实体名称.表名称
        /// </summary>
        public string Model
        {
            get { return m_Model; }
            set { m_Model = value; }
        }


        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField
        {
            get { return m_SortField; }
            set { m_SortField = value; }
        }

        /// <summary>
        /// 值的字段
        /// </summary>
        public string IdField
        {
            get { return m_IdField; }
            set { m_IdField = value; }
        }

        /// <summary>
        /// 排序字符串
        /// </summary>
        public string SortText
        {
            get { return m_SortText; }
            set { m_SortText = value; }
        }

        /// <summary>
        /// 远程数据激活
        /// </summary>
        [Description("远程数据激活")]
        [DefaultValue(false)]
        public bool RemoteEnabled
        {
            get { return m_RemoteEnabled; }
            set { m_RemoteEnabled = value; }
        }

        /// <summary>
        /// 远程地址
        /// </summary>
        [Description("远程地址")]
        public string RemoteUrl
        {
            get { return m_RemoteUrl; }
            set { m_RemoteUrl = value; }
        }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData
        {
            get { return m_CustomData; }
            set { m_CustomData = value; }
        }

        public ArrayList Data
        {
            get
            {
                if (m_Data == null)
                {
                    m_Data = new ArrayList();
                }
                return m_Data;
            }
            set { m_Data = value; }
        }

        /// <summary>
        /// 根节点 id
        /// </summary>
        public string RootValue
        {
            get { return m_RootValue; }
            set { m_RootValue = value; }
        }

        /// <summary>
        /// 上级字段
        /// </summary>
        public string ParentField
        {
            get { return m_ParentField; }
            set { m_ParentField = value; }
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

        /// <summary>
        /// 节点类型的集合,一般用于图标
        /// </summary>
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("节点类型的集合,一般用于图标")]
        public TreeNodeTypeCollection Types
        {
            get
            {
                if (m_Types == null)
                {
                    m_Types = new TreeNodeTypeCollection();
                }

                return m_Types;
            }
        }

        /// <summary>
        /// 处理自定义数据
        /// </summary>
        private void ProCustomData(ScriptTextWriter st)
        {

            if (this.Page.IsPostBack)
            {
                return;
            }


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

                st.WriteParam("idField", this.IdField);
                st.WriteParam("valueField", this.ValueField);
                st.WriteParam("displayField", this.DisplayField);
                
                st.WriteParam("rootValue", this.RootValue);
                st.WriteParam("parField", this.ParentField);

                st.WriteParam("model", this.Model);
                st.WriteParam("sortText", this.SortText);
                st.WriteParam("sortField", this.SortField);



                st.WriteParam("remoteEnabled", this.RemoteEnabled);
                st.WriteParam("remoteUrl", this.RemoteUrl);


                st.WriteParam("width", this.Width);
                st.WriteParam("minWidth", this.MinWidth);
                st.WriteParam("maxWidth", this.MaxWidth);

                st.WriteParam("mode", this.TriggerMode, TriggerMode.UserInput, TextTransform.Lower);

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

                ProCustomData(st);

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
