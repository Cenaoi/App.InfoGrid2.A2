using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using System.Reflection;
using EasyClick.Web.Mini.Utility;
using System.Data;
using System.IO;

namespace EasyClick.Web.Mini
{
 
    /// <summary>
    /// 表格
    /// </summary>
    [ToolboxData("<{0}:DataGrid runat=\"server\" border=\"1\" cellpadding=\"3\" cellspacing=\"0\" style=\"font-size: 12px;\" rules=\"all\">" +
    "<Columns>" +
    "    <{0}:ButtonField HeaderText=\"功能\" CommandName=\"Select\" CommandParam=\"ID\" ButtonType=\"Link\" Text=\"选择\" />" +
    "    <{0}:CheckBoxField HeaderText=\"选择\" DataField=\"SelectID\" />" +
    "    <{0}:BoundField DataField=\"Field1\" HeaderText=\"字段1\" />" +
    "    <{0}:BoundField DataField=\"Field2\" HeaderText=\"字段2\" />" +
    "</Columns>" +
    "<EmptyDataText>没有找到记录! 请您重新输入查询条件,进行查询!</EmptyDataText>" +
    "</mi:DataGrid>")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public class DataGrid : Control, IAttributeAccessor,IMiniControl
    {
        /// <summary>
        /// 设计模式
        /// </summary>
        bool m_MiniDesignMode = false;

        /// <summary>
        /// 设计模式
        /// </summary>
        public void SetDesignMode(bool value)
        {
            m_MiniDesignMode = value;
        }
        
        /// <summary>
        /// 设计模式
        /// </summary>
        /// <returns></returns>
        public bool GetDesignMode()
        {
            return m_MiniDesignMode ;
        }

        public new bool DesignMode
        {
            get
            {
                if (m_MiniDesignMode)
                {
                    return true;
                }

                return base.DesignMode;
            }
        }

        DataGridColumnCollection m_Columns;

        ClientIDMode m_ClientIDMode = ClientIDMode.AutoID;

        /// <summary>
        /// 分页
        /// </summary>
        Pagination m_Pagination ;
        Template m_Template;

        ControlCollection m_HeaderTemplate;

        /// <summary>
        /// 焦点参数
        /// </summary>
        HiddenField m_FocusRow;
        HiddenField m_FocusRowIndex;

        HiddenField m_FixedValues;

        HiddenField m_Sort;
        HiddenField m_SortMode;
        HiddenField m_SortFIndex;
        HiddenField m_SortFIndexSrc;

        ControlCollection m_FooterTemplate;

        /// <summary>
        /// 数据仓库 ID
        /// </summary>
        string m_StoreId;


        /// <summary>
        /// 隔行显示的样式
        /// </summary>
        string m_OddClass = "odd";

        /// <summary>
        /// 隔行显示的样式
        /// </summary>
        [DefaultValue("odd")]
        public string OddClass
        {
            get { return m_OddClass; }
            set { m_OddClass = value; }
        }

        /// <summary>
        /// 展开的模板
        /// </summary>
        string m_ExpandStringTemplate;

        bool m_PagerVisible = true;

        /// <summary>
        /// 显示页脚
        /// </summary>
        bool m_ShowFooter = false;

        bool m_ShowHeader = true;

        string m_Title;

        DataGridRowExpandTemplate m_RowExpandTemplate = DataGridRowExpandTemplate.None;

        DataGridRowExpandMode m_RowExpandMode = DataGridRowExpandMode.User;

        ScrollBars m_ScrollBars = ScrollBars.None;

        /// <summary>
        /// 允许排序
        /// </summary>
        bool m_AllowSorting = false;

        #region 事件

        /// <summary>
        /// 排序的事件
        /// </summary>
        [Description("排序的事件")]
        public event EventHandler Sort;

        /// <summary>
        /// 触发排序的事件
        /// </summary>
        protected void OnSort()
        {
            int index = StringUtility.ToInt(m_SortFIndex.Value,-1);
            int indexSrc = StringUtility.ToInt(m_SortFIndexSrc.Value, -1);

            if (index >= 0 && index < this.Columns.Count)
            {
                BoundField field = this.Columns[index];

                string sortMode;

                if (indexSrc != index)
                {
                    sortMode = "ASC";
                }
                else
                {
                    sortMode = ( "ASC".Equals(m_SortMode.Value, StringComparison.CurrentCultureIgnoreCase) ) ? "DESC" : "ASC";
                }

                m_SortMode.Value = sortMode;
                m_SortFIndexSrc.Value = index.ToString();

                if (field.SortMode == SortMode.Default)
                {
                    if (!string.IsNullOrEmpty( field.SortExpression))
                    {
                        if (field.SortExpression.EndsWith(" ASC", StringComparison.CurrentCultureIgnoreCase))
                        {
                            this.SortExpression = field.SortExpression;
                        }
                        else if (field.SortExpression.EndsWith(" DESC", StringComparison.CurrentCultureIgnoreCase))
                        {
                            this.SortExpression = field.SortExpression;
                        }
                        else
                        {
                            this.SortExpression = field.SortExpression + " " + sortMode;
                        }
                    }
                    else if (!string.IsNullOrEmpty(field.DataField))
                    {
                        this.SortExpression = field.DataField + " " + sortMode;
                    }
                }
                else if (field.SortMode == SortMode.User)
                {
                    if (sortMode == "ASC")
                    {
                        this.SortExpression = field.SortAscExpression;
                    }
                    else
                    {
                        this.SortExpression = field.SortDescExpression;
                    }
                }
               
            }

            if (Sort != null) { Sort(this, EventArgs.Empty); }
        }

        #endregion


        /// <summary>
        /// 数据仓库ID
        /// </summary>
        public string StoreId
        {
            get { return m_StoreId; }
            set { m_StoreId = value; }
        }


        /// <summary>
        /// 允许排序
        /// </summary>
        [DefaultValue(false),Description("允许排序")]
        public bool AllowSorting
        {
            get { return m_AllowSorting; }
            set { m_AllowSorting = value; }
        }

        string m_SortExpression;

        /// <summary>
        /// 排序表达式
        /// </summary>
        [DefaultValue(""), Description("排序表达式")]
        public string SortExpression
        {
            get
            {
                if (!this.DesignMode && m_Sort != null)
                {
                    return m_Sort.Value;
                }
                else
                {
                    return m_SortExpression;
                }
            }
            set
            {
                m_SortExpression = value;

                if (!this.DesignMode && m_Sort != null)
                {
                    m_Sort.Value = value;
                }
            }
        }


        /// <summary>
        /// 滚动条类型
        /// </summary>
        [DefaultValue(ScrollBars.None), Description("滚动条类型")]
        public ScrollBars ScrollBars
        {
            get { return m_ScrollBars; }
            set { m_ScrollBars = value; }
        }

        /// <summary>
        /// 展开操作模式
        /// </summary>
        [DefaultValue(DataGridRowExpandMode.User), Description("展开操作模式")]
        public DataGridRowExpandMode RowExpandMode
        {
            get { return m_RowExpandMode; }
            set { m_RowExpandMode = value; }
        }

        /// <summary>
        /// 记录展开的模板类型
        /// </summary>
        [DefaultValue(DataGridRowExpandTemplate.None)]
        [Description("记录展开的模板类型")]
        public DataGridRowExpandTemplate RowExpandTemplate
        {
            get { return m_RowExpandTemplate; }
            set { m_RowExpandTemplate = value; }
        }

        /// <summary>
        /// 展开记录的模板
        /// </summary>
        [Browsable(false)]
        [Description("展开记录的模板")]
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string ExpandStringTemplate
        {
            get { return m_ExpandStringTemplate; }
            set { m_ExpandStringTemplate = value; }
        }

        [Browsable(false)]
        [Description("标题模板")]
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ControlCollection HeaderTemplate
        {
            get
            {
                if (m_HeaderTemplate == null)
                {
                    m_HeaderTemplate = new ControlCollection(this);
                }
                return m_HeaderTemplate;
            }
        }

        public DataGrid()
        {

            m_Pagination = new Pagination();

            m_Template = new Template();

            m_FocusRow = new HiddenField();
            m_FocusRowIndex = new HiddenField();
            m_FixedValues = new HiddenField();

            m_Sort = new HiddenField();
            m_SortMode = new HiddenField();
            m_SortFIndex = new HiddenField();
            m_SortFIndexSrc = new HiddenField();


        }

                


        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        /// <summary>
        /// 表格标题
        /// </summary>
        [Description("表格标题")]
        [DefaultValue("")]
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        /// <summary>
        /// 表格模板核心
        /// </summary>
        internal Template Template
        {
            get { return m_Template; }
        }


        /// <summary>
        /// 焦点行
        /// </summary>
        internal HiddenField FocusRow
        {
            get { return m_FocusRow; }
        }

        

        /// <summary>
        /// 获取焦点项目的值
        /// </summary>
        /// <returns></returns>
        public string GetFocusedItemValue()
        {
            return m_FocusRow.Value;
        }

        /// <summary>
        /// 获取焦点行索引
        /// </summary>
        /// <returns></returns>
        public int GetFocusedItemIndex()
        {

            int eq = -1;

            if (int.TryParse(m_FocusRowIndex.Value, out eq))
            {
                return eq;
            }

            return -1;
        }

        /// <summary>
        /// 设置焦点项目的值
        /// </summary>
        /// <param name="value"></param>
        public void SetFocusedItemValue(string value)
        {
            m_FocusRow.Value = value;
        }

        public string GetFocusedItemFixedValue()
        {
            return m_FixedValues.Value;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CreateChildControls();

        }


        /// <summary>
        /// 筛选字段
        /// </summary>
        [Description("筛选字段")]
        [DefaultValue(true)]
        public bool FilterField
        {
            get { return m_Template.FilterField; }
            set { m_Template.FilterField = value; }
        }

        [Description("页脚显示")]
        [DefaultValue(false)]
        public bool ShowFooter
        {
            get { return m_ShowFooter; }
            set { m_ShowFooter = value; }
        }

        [Description("表头显示")]
        [DefaultValue(true)]
        public bool ShowHeader
        {
            get { return m_ShowHeader; }
            set { m_ShowHeader = value; }
        }

        [Browsable(false)]
        [Description("页脚模板")]
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ControlCollection FooterTemplate
        {
            get
            {
                if (m_FooterTemplate == null)
                {
                    m_FooterTemplate = new ControlCollection(this);
                }
                return m_FooterTemplate;
            }
        }

        [Description("分页数字显示")]
        [DefaultValue(true)]
        public bool PagerVisible
        {
            get { return m_PagerVisible; }
            set { m_PagerVisible = value; }
        }

        [DefaultValue(ClientIDMode.AutoID)]
        public new ClientIDMode ClientIDMode
        {
            get { return m_ClientIDMode; }
            set { m_ClientIDMode = value; }
        }

        public string GetClientID()
        {
            string cId;

            switch (m_ClientIDMode)
            {
                case ClientIDMode.Static:
                    cId = this.ID;
                    break;
                default:
                    cId = this.ClientID;
                    break;
            }

            return cId;
        }



        /// <summary>
        /// 数据列集合
        /// </summary>
        [MergableProperty(false), DefaultValue((string)null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("数据列集合")]
        public virtual DataGridColumnCollection Columns
        {
            get
            {
                if (m_Columns == null)
                {
                    m_Columns = new DataGridColumnCollection(this);
                    m_Columns.Added += new DataGridColumnAddedDelegate(m_Columns_Added);
                }

                return m_Columns;
            }
        }

        void m_Columns_Added(object sender, BoundField field)
        {
            if (!string.IsNullOrEmpty(field.DataFormatString))
            {
                m_Template.DataFormats.Add(field.DataField, field.DataFormatString);
            }
        }

        string m_EmptyDataText;

        /// <summary>
        /// 没有数据记录的时候，显示的文字
        /// </summary>
        [Description("没有数据记录的时候，显示的文字")]
        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string EmptyDataText
        {
            get { return m_EmptyDataText; }
            set { m_EmptyDataText = value; }
        }

        bool m_InitCreateChild = false;

        protected override void CreateChildControls()
        {
            if (m_InitCreateChild)
            {
                return;
            }

            m_InitCreateChild = true;

            //this.Controls.Clear();


            //分页栏
            m_Pagination.ID = this.ID + "_Page";
            m_Pagination.IsAjax = true;
            m_Pagination.ClassName = "flickr";

            this.Controls.Add(m_Pagination);

            //拥有焦点行的主键值
            m_FocusRow.ID = this.ID + "_FocusRow";
            this.Controls.Add(m_FocusRow);

            //拥有焦点行的索引
            m_FocusRowIndex.ID = this.ID + "_FocusRowIndex";
            this.Controls.Add(m_FocusRowIndex);

            //拥有焦点行的固定字段值
            m_FixedValues.ID = this.ID + "_FixedRow";
            this.Controls.Add(m_FixedValues);


            m_Sort.ID = this.ID + "_Sort";
            this.Controls.Add(m_Sort);

            if (m_AllowSorting)
            {
                m_SortMode.ID = this.ID + "_SortMode";
                this.Controls.Add(m_SortMode);

                m_SortFIndex.ID = this.ID + "_SortFIndex";
                this.Controls.Add(m_SortFIndex);

                m_SortFIndexSrc.ID = this.ID + "_SortFIndexSrc";
                this.Controls.Add(m_SortFIndexSrc);
            }

            m_Template.DataFormats.Clear();

            foreach (BoundField col in this.Columns)
            {
                if (!string.IsNullOrEmpty(col.DataFormatString))
                {
                    m_Template.DataFormats.Add(col.DataField, col.DataFormatString);
                }
            }


            m_Template.ID =  this.ID + "_ItemTemplate";

            this.Controls.Add(m_Template);


            if (m_ShowFooter && !this.DesignMode)
            {
                ResetFoot();
            }



        }


        protected UserControl FindWidget()
        {
            Control con = this.Parent;

            for (int i = 0; i < 9; i++)
            {
                if (con is UserControl)
                {
                    break;
                }

                if (con.Parent == null)
                {
                    break;
                }

                con = con.Parent;
            }

            return con as UserControl;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GetStore();
        }

        internal StoreAbstract m_Store;

        public virtual StoreAbstract GetStore()
        {
            if (!string.IsNullOrEmpty(m_StoreId))
            {
                Control con = FindWidget();

                StoreAbstract store = con.FindControl(m_StoreId) as StoreAbstract;
                
                m_Store = store;

                store.Loaded += new StoreStatusDelegate(store_Loaded);
                store.Added += new StoreStatusDelegate(store_Added);
                store.Deleting += new StoreCancelDelegate(store_Deleting);
                store.Deleted += new StoreStatusDelegate(store_Deleted);
                
            }

            return m_Store;
        }

        protected void store_Deleted(object sender, StoreStatusEventArgs e)
        {
            m_Store.Load();
        }

        protected string GetSelectLineName()
        {
            foreach (BoundField item in this.Columns)
            {
                CheckBoxField checkBox = item as CheckBoxField;

                if (checkBox == null)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(checkBox.Name))
                {
                    return checkBox.Name;
                }

                return checkBox.DataField;
            }

            return string.Empty;
        }


        protected void store_Deleting(object sender, StoreCancelEventArgs e)
        {
            List<object> ids = e.Data as List<object>;

            string checkName = GetSelectLineName();

            if (!string.IsNullOrEmpty(checkName))
            {
                string[] checkIds = WebUtil.FormStrList(checkName);

                ids.AddRange(checkIds);
            }
        }

        protected void store_Added(object sender, StoreStatusEventArgs e)
        {
            this.Items.Add(e.ReturnValue);
            this.Reset();
        }

        protected void store_Loaded(object sender, StoreStatusEventArgs e)
        {
            this.Items.Clear();
            this.Items.AddRange(e.ReturnValue as ICollection);
            this.Reset();
        }


        private void FindMathControls(ControlCollection parents, List<Control> footCons)
        {
            foreach (Control con in parents)
            {
                if (con.HasControls())
                {
                    FindMathControls(con.Controls, footCons);
                }

                IAttributeAccessor attr = con as IAttributeAccessor;

                if (attr == null) { continue; }

                string math = attr.GetAttribute("Math");

                if (string.IsNullOrEmpty(math)) { continue; }

                footCons.Add(con);
            }
        }


        /// <summary>
        /// 数据条目集合
        /// </summary>
        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TemplateItemCollection Items
        {
            get
            {
                return m_Template.Items;
            }
        }

        /// <summary>
        /// 分页组件
        /// </summary>
        [Description("分页组件")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Pagination Pagination
        {
            get { return m_Pagination; }
        }

        public virtual void Reset()
        {
            m_Template.OnItemsChanged();
            m_Pagination.Reset();

            ResetFoot();
        }

        protected void ResetFoot()
        {
            if (!m_ShowFooter)
            {
                return;
            }

            List<Control> footCons = new List<Control>(2);
            FindMathControls(this.FooterTemplate, footCons);

            foreach (Control con in footCons)
            {
                IAttributeAccessor attr = (IAttributeAccessor)con;
                string math = attr.GetAttribute("Math");

                if ("sum".Equals(math, StringComparison.OrdinalIgnoreCase))
                {
                    SumField(con);
                }
            }

        }

        /// <summary>
        /// 输出页脚
        /// </summary>
        /// <param name="writer"></param>
        private void RenderFoot(System.Web.UI.HtmlTextWriter writer)
        {
            if (!m_ShowFooter)
            {
                return;
            }

            writer.WriteLine("<tfoot>");

            foreach (Control con in this.FooterTemplate)
            {
                con.RenderControl(writer);
            }
            
            writer.WriteLine("</tfoot>");
        }

        /// <summary>
        /// 合计字段值
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        private void SumField(Control con)
        {
            IAttributeAccessor attr = con as IAttributeAccessor;

            if (attr == null)
            {
                return;
            }

            bool auotMath = StringUtility.ToBool(attr.GetAttribute("AutoMath"));    //自动计算

            if (!auotMath)
            {
                return;
            }

            string math = attr.GetAttribute("Math");    //计算方法 sum,
             
            if (string.IsNullOrEmpty(math))
            {
                return;
            }

            string dataField = attr.GetAttribute("DataField");  //需要计算的字段

            if (string.IsNullOrEmpty(dataField))
            {
                return;
            }

            string formatStr = attr.GetAttribute("DataFormatString");   //获取格式化字符串

            decimal sumValue = 0;

            // 循环计算
            // 当计算错误的时候,退出计算
            foreach (object item in this.Items)
            {
                try
                {
                    object tmpValue = GetObjectValue(item, dataField);

                    if (tmpValue != null)
                    {
                        decimal value = Convert.ToDecimal(tmpValue);

                        sumValue += value;
                    }
                }
                catch
                {
                    sumValue = 0;
                    break;
                }
            }

            string sumValueStr;

            if (string.IsNullOrEmpty(formatStr))
            {
                sumValueStr = sumValue.ToString();
                MiniHelper.SetValue(con, sumValueStr);
            }
            else
            {
                sumValueStr = string.Format(formatStr, sumValue);
                MiniHelper.SetValue(con, sumValueStr);
            }

            

        }

        private object GetObjectValue(object obj, string dataField)
        {
            object value = null;

            if (obj is ICustomTypeDescriptor)
            {
                ICustomTypeDescriptor customTs = (ICustomTypeDescriptor)obj;
                PropertyDescriptorCollection propList = customTs.GetProperties();
                
                PropertyDescriptor prop = propList[dataField];

                value = prop.GetValue(obj);
            }
            else
            {
                Type objT = obj.GetType();

                PropertyInfo prop = objT.GetProperty(dataField);

                value = prop.GetValue(obj, null);
            }

            return value;
        }

        /// <summary>
        /// 输出空行
        /// </summary>
        /// <param name="writer"></param>
        private void RenderEmptyRow(System.Web.UI.HtmlTextWriter writer)
        {
            int visibleCount = 0;

            foreach (BoundField col in this.Columns)
            {
                if (col.Visible) { visibleCount++; }
            }

            writer.WriteLine("<tr class='empty_row' style='height:40px;'>");
            writer.WriteLine("<td colspan='{0}'>", visibleCount);

            writer.WriteLine(this.EmptyDataText);

            writer.WriteLine("</td>");
            writer.WriteLine("</tr>");
        }

        /// <summary>
        /// 输出分页对象
        /// </summary>
        /// <param name="writer"></param>
        private void RenderPager(System.Web.UI.HtmlTextWriter writer)
        {


            m_Pagination.Visible = m_PagerVisible;
            m_Pagination.RenderControl(writer);
        }

        string m_Width = "100%";
        string m_Height;

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
        /// （JS）刷新客户端的表格头部
        /// </summary>
        public string RefreshHeader()
        {
            if (!m_ShowHeader)
            {
                return string.Empty;
            }

            StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(sw); 

            if (m_HeaderTemplate != null && m_HeaderTemplate.Count > 0)
            {
                foreach (Control con in m_HeaderTemplate)
                {
                    con.RenderControl(writer);
                }
            }
            else
            {
                writer.WriteLine("<tr >");

                foreach (BoundField col in this.Columns)
                {
                    string colHtml = col.CreateHtmlTemplate(MiniDataControlCellType.Header, MiniDataControlRowState.Normal);

                    writer.WriteLine(colHtml);
                }

                writer.WriteLine("</tr>");
            }

            string txt = JsonUtility.ToJson(sw.ToString());

            MiniHelper.EvalFormat("$(\"#{0} thead\").html(\"{1}\");",
                this.GetClientID(),
                txt);

            return sw.ToString();
        }

        /// <summary>
        /// （JS）刷新客户端 body 的定义
        /// </summary>
        /// <returns></returns>
        public string RefreshBodyDefine()
        {
            string text = GetBodyDefine();
            m_Template.ItemTemplate = text;

            return text;
        }

        private void RenderHeader(System.Web.UI.HtmlTextWriter writer)
        {
            if (!m_ShowHeader)
            {
                return;
            }

            if (m_HeaderTemplate != null && m_HeaderTemplate.Count > 0)
            {
                writer.WriteLine("<thead>");
                foreach (Control con in m_HeaderTemplate)
                {
                    con.RenderControl(writer);
                }
                writer.WriteLine("</thead>");
            }
            else
            {
                writer.WriteLine("<thead>");
                writer.WriteLine("<tr >");

                foreach (BoundField col in this.Columns)
                {
                    string colHtml = col.CreateHtmlTemplate(MiniDataControlCellType.Header, MiniDataControlRowState.Normal);

                    writer.WriteLine(colHtml);
                }

                writer.WriteLine("</tr>");
                writer.WriteLine("</thead>");
            }
        }

        /// <summary>
        /// 获取 body 的定义
        /// </summary>
        /// <returns></returns>
        protected string GetBodyDefine()
        {
            StringBuilder bodyFormat = new StringBuilder();

            bodyFormat.Append("<tr>");

            if (this.DesignMode)
            {
                foreach (BoundField col in this.Columns)
                {
                    if (!col.Visible) { continue; }

                    string colHtml = col.DebugModeCreateHtml(MiniDataControlCellType.DataCell, MiniDataControlRowState.Normal);
                    bodyFormat.Append(colHtml);
                }
            }
            else
            {
                foreach (BoundField col in this.Columns)
                {
                    string colHtml = col.CreateHtmlTemplate(MiniDataControlCellType.DataCell, MiniDataControlRowState.Normal);
                    bodyFormat.Append(colHtml);
                }
            }

            bodyFormat.Append("</tr>");

            if (m_RowExpandTemplate == DataGridRowExpandTemplate.StringTemplate)
            {
                bodyFormat.Append("<tr expandpanel='true' class='expand' ");

                if (m_RowExpandMode == DataGridRowExpandMode.User || m_RowExpandMode == DataGridRowExpandMode.Closed)
                {
                    bodyFormat.Append("style='display:none;' ");
                }

                bodyFormat.Append(">");
                bodyFormat.AppendFormat("<td colspan='{0}' >", this.Columns.Count);
                bodyFormat.Append(m_ExpandStringTemplate);
                bodyFormat.Append("</td>");
                bodyFormat.Append("</tr>");
            }

            return bodyFormat.ToString();
        }

        protected virtual void RenderBody(System.Web.UI.HtmlTextWriter writer)
        {
            m_Template.ItemTemplate = GetBodyDefine();

            writer.WriteLine("<tbody>");
            m_Template.RenderControl(writer);
            writer.WriteLine("</tbody>");
        }


        private void RenderDesignMode(System.Web.UI.HtmlTextWriter writer)
        {
            foreach (BoundField item in this.Columns)
            {
                item.OnInit();
            }

            foreach (BoundField item in this.Columns)
            {
                item.OnLoad();
            }
        }

        private void RenderPage_DesignMode(System.Web.UI.HtmlTextWriter writer)
        {
            // 分页
            if (m_PagerVisible)
            {
                writer.WriteLine("<tr>");
                writer.WriteLine("<td colspan='{0}'>", this.Columns.Count);
                m_Pagination.RenderControl(writer);
                writer.WriteLine("</td>");
                writer.WriteLine("</tr>");
            }

        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            EnsureChildControls();


            m_FocusRow.RenderControl(writer);
            m_FocusRowIndex.RenderControl(writer);

            m_FixedValues.RenderControl(writer);

            m_Sort.RenderControl(writer);

            if (m_AllowSorting)
            {
                m_SortMode.RenderControl(writer);
                m_SortFIndex.RenderControl(writer);
                m_SortFIndexSrc.RenderControl(writer);
            }

            if (!this.DesignMode)
            {
                RenderDesignMode(writer);
            }




            writer.Write("<div class=\"DataGridPanel\" ");
            writer.Write("style=\"width:{0};height:{1};", m_Width, m_Height);

            switch (m_ScrollBars)
            {
                case ScrollBars.Horizontal: writer.Write("overflow-x:scroll;"); break;
                case ScrollBars.Vertical: writer.Write("overflow-y:scroll;"); break;
                case ScrollBars.Both: writer.Write("overflow:scroll;"); break;
            }

            writer.Write("\" >");

            writer.WriteLine("<table id='{0}' ", GetClientID());

            foreach (MiniHtmlAttr attr in m_HtmlAttrs.Values)
            {
                writer.Write("{0}=\"{1}\" ", attr.Key, attr.Value);
            }

            writer.WriteLine(">");

            //输出标题
            RenderHeader(writer);

            //输出body
            RenderBody(writer);


            //输出空行
            RenderEmptyRow(writer);

            //输出页脚
            RenderFoot(writer);

            writer.WriteLine("</table>");

            writer.Write("</div>");


            if (this.DesignMode)
            {
                RenderPage_DesignMode(writer);
            }
            else
            {
                // 分页
                RenderPager(writer);
            }

            if (this.DesignMode)
            {
                return;
            }

            RenderClientScript(writer);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void RenderClientScript(HtmlTextWriter writer)
        {
            writer.WriteLine("<script type=\"text/javascript\">");

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }

            if (jsMode == "InJs")
            {
                writer.WriteLine("In.ready('mi.DataGrid',function () {");
            }
            else
            {
                writer.WriteLine("$(document).ready(function(){");
            }

            writer.WriteLine("        {0}.itemsChanged(function () {{", m_Template.ClientID);

            writer.WriteLine("            try{");

            writer.WriteLine("              var grid = {0};", this.m_Template.ClientID);
            writer.WriteLine("              var emptyRow = $('#{0} .empty_row:first');", this.GetClientID());

            writer.WriteLine("              if (grid.itemCount() == 0) {");
            writer.WriteLine("                  $(emptyRow).show();");
            writer.WriteLine("              }");
            writer.WriteLine("              else {");
            writer.WriteLine("                  $(emptyRow).hide();");
            writer.WriteLine("              }");
            writer.WriteLine("            } catch(ex) { alert(ex.Message);}");
            writer.WriteLine("        });");

            writer.WriteLine("        {0}.onItemsChanged();", m_Template.ClientID);


            writer.WriteLine("        window.{0} = new Mini.ui.DataGrid({{id:'{0}', oddClass:'{1}'}});", 
                this.GetClientID(),
                m_OddClass);

            writer.WriteLine("    });");


            writer.WriteLine("</script>");
        }

        /// <summary>
        /// 获取行的Guid 集合（客户端 js，不支持服务器）
        /// </summary>
        /// <returns></returns>
        public int[] GetItemGuids()
        {
            return m_Template.GetItemGuids();
        }
        
        /// <summary>
        /// 显示单元格错误
        /// </summary>
        /// <param name="itemGuid">行错误</param>
        /// <param name="cellId">单元格 id</param>
        /// <param name="message">显示的错误消息</param>
        public void ShowCellError(int itemGuid, string cellId, string message)
        {
            BoundField boundField = this.Columns.GetInputItemByID(cellId);

            if (!( boundField is IMiniInputField))
            {
                throw new Exception(string.Format( "这个列 \"{0}\" 非可输入字段!",cellId) );
            }

            string cellGuid = string.Format("{0}_{1}${2}", this.GetClientID(), itemGuid, cellId);

            string js = string.Format("{0}.showCellError('{1}',\"{2}\")", this.GetClientID(), cellGuid, JsonUtility.ToJson(message) );

            MiniHelper.Eval(js);
        }

        /// <summary>
        /// 显示单元格错误信息
        /// </summary>
        /// <param name="error"></param>
        public void ShowCellErrors(IEnumerable<InputFieldError> error)
        {
            foreach (InputFieldError err in error)
            {
                ShowCellError(err.ItemGuid, err.CellID, err.Message);
            }
        }

        /// <summary>
        /// 验证行
        /// </summary>
        /// <param name="customObj"></param>
        /// <param name="rowGuid"></param>
        /// <returns></returns>
        public InputFieldError[] ValidateRow(object obj, int rowGuid)
        {
            List<InputFieldError> errors = new List<InputFieldError>();

            Type objT = obj.GetType();

            PropertyInfo[] propList = objT.GetProperties();

            PropertyInfo prop = null;

            for (int i = 0; i < propList.Length; i++)
            {
                prop = propList[i];

                if (!prop.CanWrite)
                {
                    continue;
                }

                string name = prop.Name;

                BoundField boundField = this.Columns.GetInputItemByID(name);

                if (boundField == null)
                {
                    continue;
                }

                IMiniInputField inputField = (IMiniInputField)boundField;

                Type valueT = prop.PropertyType;

                string valueStr = GetCellValue(rowGuid, inputField.ID);

                bool tryChangeType = StringUtility.TryChangeType(valueStr, valueT);

                if (!tryChangeType)
                {
                    InputFieldError err = new InputFieldError();
                    err.CellID = name;
                    err.ItemGuid = rowGuid;

                    errors.Add(err);
                }


            }

            return errors.ToArray();
        }

        /// <summary>
        /// 验证行
        /// </summary>
        /// <param name="customObj"></param>
        /// <param name="rowGuid"></param>
        /// <returns></returns>
        public InputFieldError[] ValidateRow(ICustomTypeDescriptor customObj, int rowGuid)
        {
            List<InputFieldError> errors = new List<InputFieldError>();

            ICustomTypeDescriptor customTs = (ICustomTypeDescriptor)customObj;
            PropertyDescriptorCollection propList = customTs.GetProperties();
            PropertyDescriptor prop = null;

            for (int i = 0; i < propList.Count; i++)
            {
                prop = propList[i];

                if (prop.IsReadOnly)
                {
                    continue;
                }

                string name = prop.Name;

                BoundField boundField = this.Columns.GetInputItemByID(name);

                if (boundField == null)
                {
                    continue;
                }

                IMiniInputField inputField = (IMiniInputField)boundField;

                Type valueT = prop.PropertyType;

                string valueStr = GetCellValue(rowGuid, inputField.ID);

                bool tryChangeType = StringUtility.TryChangeType(valueStr, valueT);

                if (!tryChangeType)
                {
                    InputFieldError err = new InputFieldError();
                    err.CellID = name;
                    err.ItemGuid = rowGuid;

                    errors.Add(err);
                }


            }

            return errors.ToArray();
        }

        /// <summary>
        /// 行数据拷贝至对象
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="rowGuid"></param>
        public void RowCopyTo(DataRow dataRow, int rowGuid)
        {
            DataTable table = dataRow.Table;

            for (int i = 0; i < table.Columns.Count; i++)
            {
                DataColumn col = table.Columns[i];

                if (col.ReadOnly)
                {
                    continue;
                }

                string name = col.ColumnName;

                BoundField boundField = this.Columns.GetInputItemByID(name);

                if (boundField == null)
                {
                    continue;
                }

                IMiniInputField inputField = (IMiniInputField)boundField;

                Type valueT = col.DataType;

                string valueStr = GetCellValue(rowGuid, inputField.ID);

                object value = StringUtility.ChangeType(valueStr, valueT);

                dataRow[i] = value;

            }
        }

        /// <summary>
        /// 行数据拷贝至对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="rowGuid"></param>
        public void RowCopyTo(object obj, int rowGuid)
        {
            Type objT = obj.GetType();

            PropertyInfo[] propList = objT.GetProperties();
            PropertyInfo prop = null;

            for (int i = 0; i < propList.Length; i++)
            {
                prop = propList[i];

                if (!prop.CanWrite)
                {
                    continue;
                }

                string name = prop.Name;

                BoundField boundField = this.Columns.GetInputItemByID(name);

                if (boundField == null)
                {
                    continue;
                }

                IMiniInputField inputField = (IMiniInputField)boundField;

                Type valueT = prop.PropertyType;

                string valueStr = GetCellValue(rowGuid, inputField.ID);

                object value = StringUtility.ChangeType(valueStr, valueT);

                prop.SetValue(obj, value,null);


            }
        }

        /// <summary>
        /// 行数据拷贝至对象
        /// </summary>
        /// <param name="customObj"></param>
        /// <param name="rowGuid"></param>
        public void RowCopyTo(ICustomTypeDescriptor customObj, int rowGuid)
        {
            ICustomTypeDescriptor customTs = (ICustomTypeDescriptor)customObj;
            PropertyDescriptorCollection propList = customTs.GetProperties();
            PropertyDescriptor prop = null;

            for (int i = 0; i < propList.Count; i++)
            {
                prop = propList[i];

                if (prop.IsReadOnly)
                {
                    continue;
                }

                string name = prop.Name;

                BoundField boundField = this.Columns.GetInputItemByID(name);

                if (boundField == null)
                {
                    continue;
                }

                IMiniInputField inputField = (IMiniInputField)boundField;

                Type valueT = prop.PropertyType;

                string valueStr = GetCellValue(rowGuid, inputField.ID);

                object value = StringUtility.ChangeType(valueStr, valueT);

                prop.SetValue(customObj, value);

                
            }
        }

        

        

        /// <summary>
        /// 获取单元格值（客户端 js,不支持服务器）
        /// </summary>
        /// <param name="rowGuid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCellValue(int rowGuid, string id)
        {
            string key = string.Format("{0}_{1}${2}", this.ClientID, rowGuid, id);

            HttpContext content = HttpContext.Current;

            string value = content.Request.Form[key];

            return value;
        }

        
        /// <summary>
        /// 获取单元格 CELL 值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rowGuid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetCellValue<T>(int rowGuid, string id) where T : struct
        {
            string key = string.Format("{0}_{1}${2}", this.ClientID, rowGuid, id);

            HttpContext content = HttpContext.Current;

            string value = content.Request.Form[key];

            if (value == null)
            {
                return default(T);
            }


            T valueT = (T)Convert.ChangeType(value, typeof(T));

            return valueT;
        }

        public DataGridRow GetRow(int rowGuid)
        {
            DataGridRow row = new DataGridRow(this, rowGuid);

            return row;
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


        #region IMiniControl 成员

        public void LoadPostData()
        {
            //m_Template.LoadPostData();
        }

        #endregion
    }






}
