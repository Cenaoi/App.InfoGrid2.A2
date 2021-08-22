using EC5.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;

namespace EasyClick.Web.Mini2
{


    /// <summary>
    /// 下拉框
    /// </summary>
    [Description("下拉选择框")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [System.Web.UI.ParseChildren(true, "Items"), System.Web.UI.PersistChildren(false)]
    public class SelectColumn : BoundField
    {
        /// <summary>
        /// 下拉框(构造函数)
        /// </summary>
        public SelectColumn()
        {
            this.DefaultEditorType = "combobox";
        }

        /// <summary>
        /// 下拉框(构造函数)
        /// </summary>
        /// <param name="dataField"></param>
        public SelectColumn(string dataField)
        {
            this.DefaultEditorType = "combobox";

            this.DataField = dataField;
        }

        /// <summary>
        /// 下拉框(构造函数)
        /// </summary>
        /// <param name="dataField"></param>
        /// <param name="headerText"></param>
        public SelectColumn(string dataField,string headerText)
        {
            this.DefaultEditorType = "combobox";

            this.DataField = dataField;
            this.HeaderText = headerText;
        }



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



        int m_DropDownWidth = 0;

        int m_DropDownHeight = 0;

        ListItemCollection m_Items;

        TriggerMode m_TriggerMode = TriggerMode.UserInput;

        /// <summary>
        /// 专门用于显示的字段名
        /// </summary>
        string m_DisplayField;


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
                if (m_RemoteParams == null)
                {
                    m_RemoteParams = new ParamCollection();
                }
                return m_RemoteParams;
            }
        }

        /// <summary>
        /// 是否有远程参数
        /// </summary>
        /// <returns></returns>
        public bool HasRemoteParams()
        {
            return m_RemoteParams != null && m_RemoteParams.Count > 0;
        }

        #endregion


        /// <summary>
        /// 值集合, 以分号';'分隔开
        /// </summary>
        [Description("值集合, 以分号';'分隔开")]
        public string StringItems { get; set; }

        /// <summary>
        /// 专门用于显示的字段名
        /// </summary>
        [DefaultValue("")]
        [Description("专门用于显示的字段名")]
        public string DisplayField
        {
            get { return m_DisplayField; }
            set { m_DisplayField = value; }
        }


        /// <summary>
        /// 下拉框显示前触发事件
        /// </summary>
        string m_OnDropDown;

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


        string m_OnMapping;

        /// <summary>
        /// 映射事件
        /// </summary>
        public string OnMapping
        {
            get { return m_OnMapping; }
            set { m_OnMapping = value; }
        }

        MapItemCollection m_MapItems;


        /// <summary>
        /// 映射集合
        /// </summary>
        [Description("映射集合")]
        [DefaultValue(null)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        public MapItemCollection MapItems
        {
            get
            {
                if (m_MapItems == null)
                {
                    m_MapItems = new MapItemCollection();
                }
                return m_MapItems;
            }
        }

        /// <summary>
        /// 下拉框的宽度
        /// </summary>
        [DefaultValue(0)]
        [Description("下拉框的宽度")]
        public int DropDownWidth
        {
            get { return m_DropDownWidth; }
            set { m_DropDownWidth = value; }
        }

        /// <summary>
        /// 下拉框的高度
        /// </summary>
        [DefaultValue(0)]
        [Description("下拉框的高度")]
        public int DropDwonHeight
        {
            get { return m_DropDownHeight; }
            set { m_DropDownHeight = value; }
        }

        /// <summary>
        /// 下拉框类型
        /// </summary>
        [DefaultValue(TriggerMode.UserInput)]
        [Description("获取或设置下拉框类型")]
        public TriggerMode TriggerMode
        {
            get { return m_TriggerMode; }
            set { m_TriggerMode = value; }
        }

        /// <summary>
        /// 记录条目集合
        /// </summary>
        [Description("记录条目集合")]
        [DefaultValue(null)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        [MergableProperty(false)]
        public virtual ListItemCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new ListItemCollection(this);
                }

                return m_Items;
            }

        }


        public override string CreateHtmlTemplate(Mini.MiniDataControlCellType cellType, Mini.MiniDataControlRowState rowState)
        {
            

            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            if (!string.IsNullOrEmpty(this.ID))
            {
                sb.AppendFormat("id:'{0}', ", this.ID);
            }

            if (!String.IsNullOrEmpty(this.ClientID))
            {
                sb.AppendFormat("clientId:'{0}', ", this.ClientID);
            }


            sb.AppendFormat("miType:'{0}', ", this.MiType);

            sb.AppendFormat("dataIndex:'{0}', ", this.DataField);
            sb.AppendFormat("width:{0}, ", this.Width);
            sb.AppendFormat("headerText:'{0}', ", this.HeaderText);

            if (!string.IsNullOrEmpty(m_DisplayField))
            {
                sb.AppendFormat("displayField:'{0}',", m_DisplayField);
            }

            if (this.SortMode != Mini.SortMode.Default)
            {
                sb.AppendFormat("sortable:'{0}', ", this.SortMode.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(this.SortExpression))
            {
                sb.AppendFormat("sortExpression:'{0}', ", this.SortExpression);
            }

            if (!this.Resizable)
            {
                sb.AppendFormat("resizable:{0}, ", this.Resizable.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(this.Renderer))
            {
                sb.AppendFormat("renderer: {0},", this.Renderer);
            }

            CreateStyleRule(sb);



            sb.AppendFormat("editorMode: '{0}', ", this.EditorMode.ToString().ToLower());


            sb.Append(CreateEditor());

            sb.AppendFormat("          align:'{0}'\n        }}", this.ItemAlign.ToString().ToLower());

            return sb.ToString();
        }

        

        protected override string CreateEditor()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.Append("          ");
            sb.Append("editor:{");
            sb.AppendFormat("xtype:'{0}', ", this.DefaultEditorType);


            sb.AppendFormat("mode: '{0}',", ((this.TriggerMode == TriggerMode.None) ? "none" : "user_input"));

            if (this.DropDownWidth > 0)
            {
                sb.AppendFormat("dropDownWidth:{0},", this.DropDownWidth);
            }

            sb.AppendFormat("  emptyEnabled:{0}, ", this.EmptyEnabled ? "true" : "false");
            sb.AppendFormat("  emptyValue: '{0}', ", this.EmptyValue);
            sb.AppendFormat("  emptyText: '{0}', ", JsonUtil.ToJson(this.EmptyText, JsonQuotationMark.SingleQuotes));


            sb.Append("fields: ['value', 'text', 'textEx'],").AppendLine();
            
            sb.Append("          ");

            if (m_MapItems != null && m_MapItems.Count > 0)
            {
                sb.Append("mapItems:[");

                for (int i = 0; i < m_MapItems.Count; i++)
                {
                    MapItem item = m_MapItems[i];

                    if (i > 0) { sb.Append(",\n"); }

                    sb.AppendFormat("{{ srcField:'{0}', targetField:'{1}'}}", item.SrcField, item.TargetField);
                }


                sb.AppendLine("],");
            }

            if (!string.IsNullOrEmpty(m_OnMapping))
            {
                sb.AppendLine("    mapping:function(owner){");
                sb.AppendLine("        " + m_OnMapping + ";");
                sb.AppendLine("    },");
            }

            if (!string.IsNullOrEmpty(m_OnDropDown))
            {
                sb.AppendLine("    dropDown:function(owner){");

                sb.AppendLine("        " + m_OnDropDown + ";");

                sb.AppendLine("    },");
            }




            //远程数据
            sb.AppendFormat("  remoteEnabled:{0}, ", this.RemoteEnabled ? "true" : "false");
            sb.AppendFormat("  remoteUrl:'{0}', ", this.RemoteUrl);

            if (this.HasRemoteParams())
            {
                sb.Append("    remoteParams: ").Append(this.RemoteParams.ToJson()).AppendLine(",");
            }


            //输入自动提示
            if (this.HasTypeahead())
            {
                sb.AppendFormat("    typeahead: {0},", this.Typeahead.ToJson());

                if (this.TypeaheadParams.Count > 0)
                {
                    sb.AppendFormat("    typeaheadParams:{0},", this.TypeaheadParams.ToJson());
                }
            }


            sb.Append("store: [");

            int itemIndex = 0;

            // 下来集合
            if (!StringUtil.IsBlank(this.StringItems))
            {
                string[] sp = StringUtil.Split(this.StringItems, ";");

                foreach (string sItem in sp)
                {
                    KeyValueString kv = KeyValueString.Parse(sItem);

                    if (itemIndex++ > 0) { sb.Append(","); }

                    sb.AppendFormat("{{value:'{0}', text:'{1}'}}", kv.Key, StringUtil.NoBlank(kv.Value, kv.Key));
                }
            }

            if (this.Items.Count > 0)
            {
                sb.AppendLine();

                foreach (ListItem item in this.Items)
                {
                    if (itemIndex++ > 0) { sb.Append(","); }

                    sb.Append(item.GetJson());
                }             

            }

            sb.Append("]");

            sb.AppendLine("\n          },");

            return sb.ToString();
        }
    }
}
