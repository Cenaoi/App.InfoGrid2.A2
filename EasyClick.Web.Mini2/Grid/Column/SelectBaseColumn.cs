using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;
using System.Web.UI;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 下拉选择的基础框
    /// </summary>
    [Description("下拉选择的基础框")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [System.Web.UI.PersistChildren(false)]
    public class SelectBaseColumn : BoundField
    {
        /// <summary>
        /// 下拉选择的基础框
        /// </summary>
        public SelectBaseColumn()
        {

            this.DefaultEditorType = "comboboxbase";
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

        ListItemBaseCollection m_Items;

        TriggerMode m_TriggerMode = TriggerMode.UserInput;

        string m_ItemValueField;

        string m_ItemDisplayField;

        MapItemCollection m_MapItems;


        /// <summary>
        /// 专门用于显示的字段名
        /// </summary>
        string m_DisplayField;


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


        string m_OnMapping;

        /// <summary>
        /// 映射事件
        /// </summary>
        public string OnMapping
        {
            get { return m_OnMapping; }
            set { m_OnMapping = value; }
        }


        /// <summary>
        /// 有映射条目
        /// </summary>
        /// <returns></returns>
        public bool HasMapItem()
        {
            return m_MapItems != null && m_MapItems.Count > 0;
        }

        /// <summary>
        /// 字段映射条目
        /// </summary>
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
        /// 值的字段名
        /// </summary>
        [DefaultValue("")]
        [Description("值的字段名")]
        public string ItemValueField
        {
            get { return m_ItemValueField; }
            set { m_ItemValueField = value; }
        }

        /// <summary>
        /// 显示的字段名
        /// </summary>
        [DefaultValue("")]
        [Description("显示的字段名")]
        public string ItemDisplayField
        {
            get { return m_ItemDisplayField; }
            set { m_ItemDisplayField = value; }
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
        public virtual ListItemBaseCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new ListItemBaseCollection();
                }

                return m_Items;
            }

        }


        public override string CreateHtmlTemplate(Mini.MiniDataControlCellType cellType, Mini.MiniDataControlRowState rowState)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("miType:'{0}', ", this.MiType);

            sb.AppendFormat("dataIndex:'{0}', ", this.DataField);

            if (!string.IsNullOrEmpty(m_DisplayField))
            {
                sb.AppendFormat("displayField:'{0}',", m_DisplayField);
            }

            sb.AppendFormat("width:{0}, ", this.Width);
            sb.AppendFormat("headerText:'{0}', ", this.HeaderText);


            sb.AppendFormat("itemValueField:'{0}', ", this.ItemValueField);
            sb.AppendFormat("itemDisplayField:'{0}', ", this.ItemDisplayField);

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


            CreateStyleRule(sb);


            if (!string.IsNullOrEmpty(this.Renderer))
            {
                sb.AppendFormat("renderer: {0},", this.Renderer);
            }

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
            sb.AppendLine("editor:{");
            sb.AppendFormat("  xtype:'{0}', ", this.DefaultEditorType).AppendLine();


            sb.AppendFormat("  mode: '{0}',", ((this.TriggerMode == TriggerMode.None) ? "none" : "user_input")).AppendLine();

            if (this.DropDownWidth > 0)
            {
                sb.AppendFormat("  dropDownWidth:{0},", this.DropDownWidth).AppendLine();
            }


            sb.AppendFormat("  valueField:'{0}', ", this.ItemValueField).AppendLine();
            sb.AppendFormat("  displayField:'{0}', ", this.ItemDisplayField).AppendLine();

            sb.AppendFormat("  emptyEnabled:{0}, ", this.EmptyEnabled?"true":"false");
            sb.AppendFormat("  emptyValue: '{0}', ", this.EmptyValue);
            sb.AppendFormat("  emptyText: '{0}', ", JsonUtil.ToJson( this.EmptyText, JsonQuotationMark.SingleQuotes));
            //sb.Append("fields: ['value', 'text', 'textEx'],").AppendLine();

            sb.AppendLine();
            sb.Append("          ");


            //远程数据
            sb.AppendFormat("  remoteEnabled:{0}, ", this.RemoteEnabled ? "true" : "false");
            sb.AppendFormat("  remoteUrl: '{0}', ", this.RemoteUrl);

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

                if (this.HasTypeaheadMapItems())
                {
                    sb.AppendFormat("    typeaheadMapItems:{0},", this.TypeaheadMapItems.ToJson());

                }
            }


            if (this.HasMapItem())
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

                
            sb.Append("          ");

            sb.Append("store: [");

            if (this.Items.Count > 0)
            {
                sb.AppendLine();

                for (int i = 0; i < this.Items.Count; i++)
                {
                    ListItemBase item = this.Items[i];

                    if (i > 0) { sb.Append(",\n"); }

                    sb.Append(item.GetJson());
                }

            }

            sb.Append("]");

            sb.AppendLine("\n          },");

            return sb.ToString();
        }
    }
}
