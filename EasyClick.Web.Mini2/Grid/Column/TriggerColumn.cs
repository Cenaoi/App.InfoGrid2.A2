using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Security.Permissions;
using EasyClick.Web.Mini;
using System.Web.UI;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 下拉框按钮类型
    /// </summary>
    public enum TriggerButtonType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        
        /// <summary>
        /// 更多
        /// </summary>
        More
    }

    /// <summary>
    /// 下拉框
    /// </summary>
    [Description("下拉选择框")]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class TriggerColumn : BoundField
    {
        public TriggerColumn()
        {
            this.DefaultEditorType = "trigger";
        }

        public TriggerColumn(string dataField) : base(dataField)
        {
            this.DefaultEditorType = "trigger";
        }

        public TriggerColumn(string dataField, string headerText) : base(dataField, headerText)
        {
            this.DefaultEditorType = "trigger";
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

        /// <summary>
        /// 按钮类型
        /// </summary>
        TriggerButtonType m_ButtonType = TriggerButtonType.Default;

        string m_Content;

        /// <summary>
        /// 按钮点击的事件名称
        /// </summary>
        string m_OnButtonClick;

        /// <summary>
        /// 执行字段映射函数
        /// </summary>
        string m_OnMapping;

        TriggerMode m_TriggerMode = TriggerMode.UserInput;

        string m_ButtonClass;

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
        /// 映射事件
        /// </summary>
        public string OnMaping
        {
            get { return m_OnMapping; }
            set { m_OnMapping = value; }
        }


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


        //MapItemCollection m_MapItems;

        ///// <summary>
        ///// 映射集合
        ///// </summary>
        //[Description("映射集合")]
        //[DefaultValue(null)]
        //[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        //[MergableProperty(false)]
        //public override MapItemCollection MapItems
        //{
        //    get
        //    {
        //        if (m_MapItems == null)
        //        {
        //            m_MapItems = new MapItemCollection();
        //        }
        //        return m_MapItems;
        //    }
        //}


        /// <summary>
        /// 按钮样式
        /// </summary>
        [Description("按钮样式"), DefaultValue("")]
        public string ButtonClass
        {
            get { return m_ButtonClass; }
            set { m_ButtonClass = value; }
        }


        /// <summary>
        /// 点击按钮触发的事件
        /// </summary>
        [DefaultValue("点击按钮触发的事件")]
        public string OnButtonClick
        {
            get { return m_OnButtonClick; }
            set { m_OnButtonClick = value; }
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
        /// 下来框显示的内容。例如: "#id"、 ".class" 、 "对象名" 
        /// </summary>
        [DefaultValue("")]
        public string Content
        {
            get { return m_Content; }
            set { m_Content = value; }
        }

        public override string CreateHtmlTemplate(Mini.MiniDataControlCellType cellType, Mini.MiniDataControlRowState rowState)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("miType:'{0}', ", this.MiType);

            if (!string.IsNullOrEmpty(this.Tag))
            {
                sb.AppendFormat("tag:'{0}',", MiniHelper.GetItemJson(this.Tag));
            }

            if (this.SortMode != Mini.SortMode.Default)
            {
                sb.AppendFormat("sortable:'{0}', ", this.SortMode.ToString().ToLower());
            }

            sb.AppendFormat("dataIndex:'{0}', ", this.DataField);
            sb.AppendFormat("width:{0}, ", this.Width);
            sb.AppendFormat("headerText:'{0}', ", this.HeaderText);

            sb.AppendFormat("  emptyEnabled:{0}, ", this.EmptyEnabled ? "true" : "false");
            sb.AppendFormat("  emptyValue: '{0}', ", this.EmptyValue);
            sb.AppendFormat("  emptyText: '{0}', ", JsonUtil.ToJson(this.EmptyText, JsonQuotationMark.SingleQuotes));


            if (!string.IsNullOrEmpty(this.SummaryType))
            {
                sb.AppendFormat("summaryType:'{0}',", this.SummaryType);
            }

            if (!this.Resizable)
            {
                sb.AppendFormat("resizable:{0}, ", this.Resizable.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(this.SortExpression))
            {
                sb.AppendFormat("sortExpression:'{0}', ", this.SortExpression);
            }


            if (!string.IsNullOrEmpty(this.Renderer))
            {
                sb.AppendFormat("renderer: {0},", this.Renderer);
            }

            sb.AppendFormat("editorMode: '{0}', ", this.EditorMode.ToString().ToLower());


            sb.AppendFormat("triggerMode: '{0}',", ((this.TriggerMode == TriggerMode.None) ? "none" : "user_input"));

            sb.AppendFormat("dropDownWidth:{0},", this.DropDownWidth);
            sb.AppendFormat("dropDownHeight:{0},", this.DropDwonHeight);

            sb.Append(CreateEditor());

            sb.AppendFormat("align:'{0}'}}", this.ItemAlign.ToString().ToLower());

            return sb.ToString();
        }



        protected override string CreateEditor()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.Append("editor:{").AppendLine();
            sb.AppendFormat("  xtype:'{0}', ", this.DefaultEditorType).AppendLine();


            if (!string.IsNullOrEmpty(this.Tag))
            {
                sb.AppendFormat("  tag: '{0}', ", MiniHelper.GetItemJson(this.Tag)).AppendLine();
            }



            sb.AppendFormat("  mode:'{0}',", ((this.TriggerMode == TriggerMode.None) ? "none" : "user_input")).AppendLine();
            //sb.AppendFormat("mode: '{0}', ", this.EditorMode.ToString().ToLower());
            
            if (this.DropDownWidth > 0)
            {
                sb.AppendFormat("  dropDownWidth:{0},", this.DropDownWidth).AppendLine();
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


            //远程数据
            sb.AppendFormat("  remoteEnabled:{0}, ", this.RemoteEnabled?"true":"false");
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
            }


            if (!string.IsNullOrEmpty(m_OnButtonClick))
            {
                sb.AppendLine("    buttonClick:function(owner){");
                sb.AppendLine("        " + OnButtonClick + ";");
                sb.AppendLine("    },");
            }


            if (!string.IsNullOrEmpty(m_ButtonClass))
            {
                sb.AppendFormat("    buttonCls:'{0}',", m_ButtonClass).AppendLine();
            }
            else
            {
                if (m_ButtonType == TriggerButtonType.More)
                {
                    sb.Append("    buttonCls:'mi-icon-more',").AppendLine();
                }
            }


            sb.AppendFormat("content: '{0}'", this.m_Content).AppendLine();


            sb.AppendLine("},");

            return sb.ToString();
        }
    }
}
