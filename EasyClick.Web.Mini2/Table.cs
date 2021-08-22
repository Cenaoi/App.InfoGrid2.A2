using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using EasyClick.Web.Mini;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Security.Permissions;
using EC5.Utility;
using EasyClick.Web.Mini2.Data;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 表格
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    [Description("表格")]
    public class Table : Component, IAttributeAccessor, IMiniControl, IPanel, IDelayRender
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// (构造函数)表格
        /// </summary>
        public Table()
        {
            this.JsNamespace = "Mini2.ui.panel.Table";
            this.InReady = "Mini2.ui.panel.Table";

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CreateChildControls();
        }


        TableColumnCollection m_Columns;

        /// <summary>
        /// 复选框选择模式
        /// </summary>
        CheckedMode m_CheckedMode = CheckedMode.Multi;

        /// <summary>
        /// 分页显示
        /// </summary>
        bool m_PagerVisible = true;

        /// <summary>
        /// 隐藏页数选择框
        /// </summary>
        bool m_HidePagerRowCountSelect = false;

        /// <summary>
        /// 底部备注,合计的行
        /// </summary>
        bool m_SummaryVisible = false;


        string m_Width = "100%";
        string m_Height = "400px";

        string m_StoreID;

        /// <summary>
        /// 自动选择
        /// </summary>
        bool m_AutoRowCheck = false;


        HiddenField m_Checked;

        Pagination m_Pager;

        /// <summary>
        /// 焦点边框
        /// </summary>
        bool m_FocusBorders = true;

        /// <summary>
        /// 默认编辑模式
        /// </summary>
        EditorMode m_DefaultEditorMode = EditorMode.Auto;

        bool m_ReadOnly = false;

        bool m_RowLines = true;

        bool m_ColumnLines = true;

        RegionType m_Region = RegionType.North;

        /// <summary>
        /// 是否排序
        /// </summary>
        bool m_Sortable = true;

        /// <summary>
        /// 隔行样式激活
        /// </summary>
        bool m_RowAltEnabled = true;

        /// <summary>
        /// 行重新设置以后，触发的 JS 代码
        /// </summary>
        string m_OnItemReseted = string.Empty;

        /// <summary>
        /// 行样式激活显示
        /// </summary>
        bool m_RowStyleEnabled = true;

        bool m_Visible = true;

        ScrollMode m_Scroll = ScrollMode.Auto;


        /// <summary>
        /// 显示焦点边框
        /// </summary>
        [Description("显示焦点边框")]
        [DefaultValue(true)]
        public bool FocusBorders
        {
            get { return m_FocusBorders; }
            set { m_FocusBorders = value; }
        }

        /// <summary>
        /// 滚动条
        /// </summary>
        [DefaultValue(ScrollMode.Auto)]
        public ScrollMode Scroll
        {
            get { return m_Scroll; }
            set { m_Scroll = value; }
        }

        [DefaultValue(true)]
        public new bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        /// <summary>
        /// Json 数据提交的模式
        /// </summary>
        [Description("Json 数据提交的模式")]
        [DefaultValue(TableJsonMode.Simple)]
        public TableJsonMode JsonMode { get; set; } = TableJsonMode.Simple;

        /// <summary>
        /// 命令事件
        /// </summary>
        public event EventHandler<TableCommandEventArgs> Command;

        /// <summary>
        /// 触发命令事件
        /// </summary>
        /// <param name="cmdName"></param>
        /// <param name="cmdParam"></param>
        /// <param name="record"></param>
        protected void OnCommand(string cmdName,string cmdParam, DataRecord record)
        {
            if(Command != null)
            {
                Command(this, new TableCommandEventArgs(cmdName, cmdParam, record));
            }
        }

        /// <summary>
        /// 给与内部调用
        /// </summary>
        public void PreCommand(string cmdName,string cmdParam, string record)
        {
            Newtonsoft.Json.Linq.JToken jt;
            DataRecord dr;

            try
            {
                jt = (Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(record);

                dr = DataRecord.Parse(jt);
            }
            catch (Exception ex)
            {
                log.Debug("解析 record 参数错误: record=" + record);
                return;
            }

            OnCommand(cmdName, cmdParam, dr);
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        [Description("双击事件")]
        public event EventHandler CellDbclick;

        /// <summary>
        /// 触发双击事件
        /// </summary>
        private void OnCellDbclick()
        {
            CellDbclick?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 行记录选中触发的事件
        /// </summary>
        [Description("行记录选中触发的事件")]
        public event EventHandler RowsChecked;

        /// <summary>
        /// 触发行选中的触发事件
        /// </summary>
        protected void OnRowsChecked()
        {
            RowsChecked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 触发行选中的事件
        /// </summary>
        public void PreRowsChecked()
        {
            OnRowsChecked();
        }

        /// <summary>
        /// 触发当前焦点行事件
        /// </summary>
        public void PreCellDbclick()
        {
            OnCellDbclick();
        }




        /// <summary>
        /// 行样式激活显示
        /// </summary>
        [DefaultValue(true)]
        [Description("行样式激活显示")]
        public bool RowStyleEnabled
        {
            get { return m_RowStyleEnabled; }
            set { m_RowStyleEnabled = value; }
        }

        /// <summary>
        /// 行重新设置以后，触发的 JS 代码
        /// </summary>
        [DefaultValue("")]
        [Description("行重新设置以后，触发的 JS 代码")]
        public string OnItemReseted
        {
            get { return m_OnItemReseted; }
            set { m_OnItemReseted = value; }
        }

        /// <summary>
        /// 隔行样式激活
        /// </summary>
        [Description("隔行样式激活")]
        [DefaultValue(true)]
        public bool RowAltEnabled
        {
            get { return m_RowAltEnabled; }
            set { m_RowAltEnabled = value; }
        }


        /// <summary>
        /// 概要描述,底部显示合计的行
        /// </summary>
        [DefaultValue(false)]
        [Description("概要描述,底部显示合计的行")]
        public bool SummaryVisible
        {
            get { return m_SummaryVisible; }
            set { m_SummaryVisible = value; }
        }

        /// <summary>
        /// 是否排序
        /// </summary>
        [DefaultValue(true)]
        [Description("是否排序")]
        public bool Sortable
        {
            get { return m_Sortable; }
            set { m_Sortable = value; }
        }


        /// <summary>
        /// 版面所属区域,配合 Viewport 控件使用。
        /// </summary>
        [DefaultValue(RegionType.North)]
        [Description("版面所属区域,配合 Viewport 控件使用。")]
        public RegionType Region
        {
            get { return m_Region; }
            set { m_Region = value; }
        }


        /// <summary>
        /// 分页显示
        /// </summary>
        [Description("分页显示")]
        [Category("外观")]
        [DefaultValue(true)]
        public bool PagerVisible
        {
            get { return m_PagerVisible; }
            set { m_PagerVisible = value; }
        }

        /// <summary>
        /// 隐藏页数选择框架
        /// </summary>
        [Description("隐藏页数选择框架")]
        [Category("外观")]
        [DefaultValue(false)]
        public bool HidePagerRowCountSelect
        {
            get { return m_HidePagerRowCountSelect; }
            set { m_HidePagerRowCountSelect = value; }
        }




        /// <summary>
        /// 显示行的分割线
        /// </summary>
        [Description("显示行的分割线")]
        [Category("外观")]
        [DefaultValue(true)]
        public bool RowLines
        {
            get { return m_RowLines; }
            set { m_RowLines = value; }
        }

        /// <summary>
        /// 显示列的分割线
        /// </summary>
        [Description("显示列的分割线")]
        [Category("外观")]
        [DefaultValue(true)]
        public bool ColumnLines
        {
            get { return m_ColumnLines; }
            set { m_ColumnLines = value; }
        }


        /// <summary>
        /// 选择单元格，自动行打钩
        /// </summary>
        [Description("选择单元格，自动行打钩")]
        [DefaultValue(false)]
        public bool AutoRowCheck
        {
            get { return m_AutoRowCheck; }
            set { m_AutoRowCheck = value; }
        }

        /// <summary>
        /// 复选框选择模式,Multi
        /// </summary>
        [Description("复选框选择模式.")]
        [DefaultValue(CheckedMode.Multi)]
        public CheckedMode CheckedMode
        {
            get { return m_CheckedMode; }
            set
            {
                m_CheckedMode = value;
            }
        }

        /// <summary>
        /// (支持JS)只读
        /// </summary>
        [Description("表格只读状态")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return m_ReadOnly; }
            set
            {
                m_ReadOnly = value;

                if (!this.DesignMode)
                {
                    MiniHelper.EvalFormat("{0}.setReadOnly({1});", this.ClientID, EC5.Utility.BoolUtil.ToJson(value));
                }
            }
        }

        /// <summary>
        /// 默认单元格编辑模式
        /// </summary>
        [DefaultValue(EditorMode.Auto)]
        [Description("默认单元格编辑模式")]
        public EditorMode DefaultEditorMode
        {
            get { return m_DefaultEditorMode; }
            set { m_DefaultEditorMode = value; }
        }


        /// <summary>
        /// 获取提交数据的 JSON 原始数据
        /// </summary>
        /// <returns></returns>
        public string GetCheckedJson()
        {
            if (m_Checked == null)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(m_Checked.Value))
            {
                HttpRequest request = this.Context.Request;
                string json = request.Form[m_Checked.ClientID];

                m_Checked.Value = json;
            }

            return m_Checked.Value;
        }

        /// <summary>
        /// 获取选中记录的批次
        /// </summary>
        /// <returns></returns>
        public DataBatch GetCheckedBatch()
        {
            string json = GetCheckedJson();

            if (string.IsNullOrEmpty(json))
            {
                return new DataBatch();
            }

            DataBatch db = DataBatch.Parse(json);

            return db;
        }


        /// <summary>
        /// 获取被选中的行
        /// </summary>
        [Browsable(false)]
        public DataRecordCollection CheckedRows
        {
            get
            {
                DataBatch batch = GetCheckedBatch();

                return batch.Records;
            }
        }



        /// <summary>
        /// 数据仓库ID
        /// </summary>
        [Description("数据仓库ID")]
        [DefaultValue("")]
        public string StoreID
        {
            get { return m_StoreID; }
            set { m_StoreID = value; }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        [Description("宽度")]
        [DefaultValue("100%")]
        public string Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        /// <summary>
        /// 高度
        /// </summary>
        [Description("高度")]
        [DefaultValue("")]
        public string Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

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




        #region Attribute

        internal MiniHtmlAttrCollection m_HtmlAttrs = new MiniHtmlAttrCollection();

        /// <summary>
        /// 是否存在此对应属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsAttr(string key)
        {
            return m_HtmlAttrs.ContainsAttr(key);
        }

        public string GetAttribute(string key)
        {
            return m_HtmlAttrs.GetAttribute(key);
        }

        public void SetAttribute(string key, string value)
        {
            m_HtmlAttrs.SetAttribute(key, value);
        }

        #endregion



        bool m_InitCreateChild = false;

        protected override void CreateChildControls()
        {
            if (m_InitCreateChild)
            {
                return;
            }

            m_InitCreateChild = true;

            base.CreateChildControls();


            m_Checked = new HiddenField();
            m_Checked.ID = this.ID + "_Checked";
            this.Controls.Add(m_Checked);

            m_Pager = new Pagination();
            m_Pager.ID = this.ID + "_Pager";
            this.Controls.Add(m_Pager);
        }


        private void Render_DesignMode(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddAttribute("border", "1");
            writer.AddStyleAttribute("width", "100%");

            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Thead);
            {
                foreach (BoundField item in this.Columns)
                {
                    writer.AddStyleAttribute("width", item.Width + "px");

                    writer.RenderBeginTag(HtmlTextWriterTag.Th);

                    writer.Write(item.HeaderText);

                    writer.RenderEndTag();
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Th);

                writer.Write("&nbsp;");

                writer.RenderEndTag();
            }
            writer.RenderEndTag();


            writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
            {

                foreach (BoundField item in this.Columns)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);

                    if (string.IsNullOrEmpty(item.DataFormatString))
                    {
                        writer.Write("&nbsp;");
                    }
                    else
                    {
                        writer.Write(item.DataFormatString);
                    }
                    writer.RenderEndTag();
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                writer.Write("&nbsp;");

                writer.RenderEndTag();
            }
            writer.RenderEndTag();



            writer.RenderEndTag();
        }


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                Render_DesignMode(writer);
                return;
            }

            EnsureChildControls();

            ScriptManager script = ScriptManager.GetManager(this.Page);

            m_TempBoxId = this.ClientID + "_Box" + RandomUtil.Next();
            writer.WriteLine("    <div id='{0}' style='display:none;'></div>", m_TempBoxId);

            string jsCode = string.Format("$('#{0}').val({1}.getJson())", m_Checked.ClientID, this.ClientID);
            m_Checked.SetAttribute("SubmitBufore", jsCode);

            m_Checked.RenderControl(writer);

            if (script != null)
            {

                StringBuilder jsSb = new StringBuilder();


                BeginReady(jsSb);

                FullScript(jsSb);

                EndReady(jsSb);

                script.AddScript(jsSb.ToString());
            }
            else
            {

                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);

                HtmlTextWriter htmlWriter = new HtmlTextWriter(sw);

                RenderClientScript(htmlWriter);

                writer.Write(sw.ToString());

                sw.Dispose();
            }

        }

        Store m_Store;

        /// <summary>
        /// 数据仓库
        /// </summary>
        [Browsable(false)]
        public Store Store
        {
            get { return m_Store; }
            set { m_Store = value; }
        }


        /// <summary>
        /// 获取当前控件中的 StoreID 对应的唯一ID
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        private string GetStoreIdByPage(string storeId)
        {
            if (m_Store != null)
            {
                return m_Store.ClientID;
            }

            if (string.IsNullOrEmpty(storeId))
            {
                return string.Empty;
            }

            Control con = this.Parent.FindControl(storeId);

            if (con == null)
            {
                con = this.Page.FindControl(storeId);
            }

            if (con == null)
            {
                return string.Empty;
            }

            return con.ClientID;
        }

        private void FullScript(StringBuilder sb)
        {

            sb.AppendLine("  var grid = Mini2.create('Mini2.ui.panel.Table', {");
            //writer.WriteLine("        renderTo: '#{0}',", this.ClientID + "_Box");

            JsParam(sb, "id", this.ID);
            JsParam(sb, "clientId", this.ClientID);
            JsParam(sb, "applyTo", "#" + m_TempBoxId);

            string globelStoreId = GetStoreIdByPage(m_StoreID);


            JsParam(sb, "visible", this.Visible, true);

            JsParam(sb, "store", globelStoreId);

            JsParam(sb, "defaultEditorMode", m_DefaultEditorMode, EditorMode.Auto, TextTransform.Lower);
            JsParam(sb, "readOnly", m_ReadOnly, false);
            JsParam(sb, "checkedMode", m_CheckedMode, CheckedMode.Multi, TextTransform.Upper);
            JsParam(sb, "jsonMode", this.JsonMode, TableJsonMode.Simple, TextTransform.Lower);

            JsParam(sb, "autoRowCheck", m_AutoRowCheck, false);

            JsParam(sb, "scroll", m_Scroll, ScrollMode.Auto, TextTransform.Lower);

            JsParam(sb, "focusBorders", m_FocusBorders, true);

            JsParam(sb, "rowLines", m_RowLines, true);
            JsParam(sb, "columnLines", m_ColumnLines, true);
            JsParam(sb, "rowAltEnabled", m_RowAltEnabled, true);

            JsParam(sb, "rowStyleEnabled", m_RowStyleEnabled, true);

            JsParam(sb, "width", this.Width);
            JsParam(sb, "height", this.Height);


            JsParam(sb, "minWidth", this.MinWidth);
            JsParam(sb, "minHeight", this.MinHeight);

            JsParam(sb, "maxWidth", this.MaxWidth);
            JsParam(sb, "maxHeight", this.MaxHeight);   


            JsParam(sb, "dock", this.Dock, DockStyle.Top, TextTransform.Lower);

            JsParam(sb, "region", this.Region, RegionType.North, TextTransform.Lower);

            JsParam(sb, "pagerVisible", this.PagerVisible, true);

            JsParam(sb, "hidePagerRowCountSelect", this.HidePagerRowCountSelect, false);

            JsParam(sb, "summaryVisible", this.SummaryVisible, false);

            if (!string.IsNullOrEmpty(m_OnItemReseted))
            {
                sb.AppendFormat("itemReseted: {0},", m_OnItemReseted);
            }

            JsParam(sb, "sortable", this.Sortable, true);

            JsParam(sb, "selType", "checkBoxModel");     //选择模式，checkBoxMode = 复选框选择模式
            JsParam(sb, "xtype", "cell-editing");        // 'cell-editing' 或 'row-expander-grid' 或 property-grid


            if (this.CellDbclick != null)
            {
                sb.AppendLine("    cellDbclick: function(){");

                sb.AppendLine("        widget1.subMethod('form:first',{");

                sb.AppendFormat("          subName:'{0}',", this.ID).AppendLine();
                sb.AppendFormat("          subMethod:'{0}'", "PreCellDbclick").AppendLine();

                sb.AppendLine("        });");

                sb.AppendLine("    },");
            }

            if(this.RowsChecked != null)
            {
                sb.AppendLine("    rowsChecked: function(){");

                sb.AppendLine("        widget1.subMethod('form:first',{");

                sb.AppendFormat("          subName:'{0}',", this.ID).AppendLine();
                sb.AppendFormat("          subMethod:'{0}'", "PreRowsChecked").AppendLine();

                sb.AppendLine("        });");

                sb.AppendLine("    },");
            }

            sb.Append("      columns: [");


            if (this.Columns.Count > 0)
            {
                sb.AppendLine();

                string colJs;

                int i = 0;

                foreach (BoundField field in this.Columns)
                {
                    if (!field.Visible)
                    {
                        continue;
                    }

                    colJs = field.CreateHtmlTemplate(MiniDataControlCellType.DataCell, MiniDataControlRowState.Normal);

                    if (i++ > 0) { sb.AppendLine(","); }

                    sb.Append(colJs);

                }

            }

            sb.AppendLine("      ]");


            sb.AppendLine("  });");

            if (m_IsDelayRender)
            {
                sb.AppendLine("  grid.delayRender();");
            }
            else
            {
                sb.AppendLine("  grid.render();");
            }

            if (m_RecordCheckList != null && m_RecordCheckList.Count > 0)
            {
                foreach (string recordId in m_RecordCheckList.Keys)
                {
                    bool isCheck = m_RecordCheckList[recordId];

                    sb.AppendFormat("grid.setRecordCheck('{0}',{1});", recordId, BoolUtil.ToJson(isCheck));
                    sb.AppendLine();
                }
            }

            sb.AppendFormat("  window.{0} = grid;\n", this.ClientID);
            sb.AppendFormat("Mini2.onwerPage.controls['{0}'] = grid;\n", this.ID);

        }

        string m_TempBoxId;

        protected virtual void RenderClientScript(HtmlTextWriter writer)
        {
            //m_TempBoxId = this.ClientID + "_Box" + RandomUtil.Next();
            //writer.WriteLine("    <div id='{0}' style='display:none;'></div>", m_TempBoxId);

            StringBuilder sb = new StringBuilder();

            BeginScript(sb);
            BeginReady(sb);

            FullScript(sb);

            EndReady(sb);
            EndScript(sb);

            writer.Write(sb.ToString());
        }

        /// <summary>
        /// 记录选择
        /// </summary>
        SortedList<string, bool> m_RecordCheckList;

        /// <summary>
        /// (支持 JS)设置记录选择状态
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="isCheck"></param>
        public void SetRecordCheck(string recordId, bool isCheck)
        {
            if (m_RecordCheckList == null)
            {
                m_RecordCheckList = new SortedList<string, bool>();
            }

            if (m_RecordCheckList.ContainsKey(recordId))
            {
                m_RecordCheckList[recordId] = isCheck;
            }
            else
            {
                m_RecordCheckList.Add(recordId, isCheck);
            }

            string isCheckStr = isCheck.ToString().ToLower();

            MiniScript.Add("{0}.setRecordCheck('{1}',{2})", this.ClientID, recordId, isCheckStr);
        }

        /// <summary>
        /// (支持 JS)设置记录选择状态
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="isCheck"></param>
        public void SetRecordCheck(string[] recordIds, bool isCheck)
        {
            if (m_RecordCheckList == null)
            {
                m_RecordCheckList = new SortedList<string, bool>();
            }

            foreach (var recordId in recordIds)
            {
                if (m_RecordCheckList.ContainsKey(recordId))
                {
                    m_RecordCheckList[recordId] = isCheck;
                }
                else
                {
                    m_RecordCheckList.Add(recordId, isCheck);
                }

                string isCheckStr = BoolUtil.ToJson(isCheck);

                MiniScript.Add("{0}.setRecordCheck('{1}',{2})", this.ClientID, recordId, isCheckStr);
            }
        }

        /// <summary>
        /// (支持 JS)设置记录选择状态
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="isCheck"></param>
        public void SetRecordCheck(int[] iRecordId, bool isCheck)
        {
            string[] ids = new string[iRecordId.Length];

            for (int i = 0; i < iRecordId.Length; i++)
            {
                ids[i] = iRecordId[i].ToString();
            }

            SetRecordCheck(ids, isCheck);
        }

        /// <summary>
        /// (支持 JS)设置记录选择状态
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="isCheck"></param>
        public void SetRecordCheck(long[] iRecordId, bool isCheck)
        {
            string[] ids = new string[iRecordId.Length];

            for (int i = 0; i < iRecordId.Length; i++)
            {
                ids[i] = iRecordId[i].ToString();
            }

            SetRecordCheck(ids, isCheck);
        }



        #region Render_JsParam


        private void Render_JsParam(HtmlTextWriter writer, string name, bool value, bool defaultValue)
        {
            if (value == defaultValue)
            {
                return;
            }

            writer.WriteLine("        {0}: {1},", name, value.ToString().ToLower());
        }

        private void Render_JsParam(HtmlTextWriter writer, string name, Enum value, Enum defaultValue)
        {
            Render_JsParam(writer, name, value, defaultValue, TextTransform.None);
        }

        private void Render_JsParam(HtmlTextWriter writer, string name, Enum value, Enum defaultValue, TextTransform textTransform)
        {
            if (value == defaultValue)
            {
                return;
            }

            string valueStr = value.ToString();

            switch (textTransform)
            {
                case TextTransform.Lower: valueStr = valueStr.ToLower(); break;
                case TextTransform.Upper: valueStr = valueStr.ToUpper(); break;
            }

            writer.WriteLine("        {0}: '{1}',", name, valueStr);
        }

        #endregion



        public void LoadPostData()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 延迟显示
        /// </summary>
        bool m_IsDelayRender = false;

        /// <summary>
        /// 延迟显示
        /// </summary>
        [Description("延迟显示")]
        [DefaultValue(false)]
        public bool IsDelayRender
        {
            get { return m_IsDelayRender; }
            set { m_IsDelayRender = value; }
        }

        /// <summary>
        /// 获取 Columns 的 Json 
        /// </summary>
        /// <returns></returns>
        public string GetJsonForColumns()
        {
            StringBuilder sb = new StringBuilder();


            sb.Append("[");


            if (this.Columns.Count > 0)
            {
                sb.AppendLine();

                string colJs;

                int i = 0;

                foreach (BoundField field in this.Columns)
                {
                    if (!field.Visible)
                    {
                        continue;
                    }

                    colJs = field.CreateHtmlTemplate(MiniDataControlCellType.DataCell, MiniDataControlRowState.Normal);

                    if (i++ > 0) { sb.AppendLine(","); }

                    sb.Append(colJs);

                }

            }

            sb.AppendLine("]");

            return sb.ToString();
        }
    
    }
}
