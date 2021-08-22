using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.UI;
using EasyClick.Web.Mini2.Data;

namespace EasyClick.Web.Mini2
{



    partial class DataView
    {
        /// <summary>
        /// 复选框选择模式
        /// </summary>
        CheckedMode m_CheckedMode = CheckedMode.Multi;

        /// <summary>
        /// 分页显示
        /// </summary>
        bool m_PagerVisible = true;

        /// <summary>
        /// 隐藏分页的数据量选择
        /// </summary>
        bool m_HidePagerRowCountSelect = false;

        Hidden m_Checked;

        Pagination m_Pager;

        string m_StoreID;
        Store m_Store;

        string m_Width;

        string m_Height;

        bool m_AutoSize = false;


        bool m_InitCreateChild = false;

        RegionType m_Region;

        string m_ItemWidth = "100";
        string m_ItemHeight = "100";

        string m_ItemMargin = "4";


        /// <summary>
        /// 记录选择
        /// </summary>
        SortedList<string, bool> m_RecordCheckList;

        ITemplate m_ItemTemplate;

        /// <summary>
        /// json 模式, 默认简单模式
        /// </summary>
        DataViewJsonMode m_JsonMode = DataViewJsonMode.Simple;


        ScrollMode m_Scroll = ScrollMode.Auto;

        bool m_Visible = true;

        #region 事件


        /// <summary>
        /// 命令事件
        /// </summary>
        public event EventHandler<DataViewCommandEventArgs> Command;

        /// <summary>
        /// 触发命令事件
        /// </summary>
        /// <param name="cmdName"></param>
        /// <param name="cmdParam"></param>
        /// <param name="record"></param>
        protected void OnCommand(string cmdName, string cmdParam, DataRecord record)
        {
            Command?.Invoke(this, new DataViewCommandEventArgs(cmdName, cmdParam, record));
        }

        /// <summary>
        /// 给与内部调用
        /// </summary>
        public void PreCommand(string cmdName, string cmdParam, string record)
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

        #endregion


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
        /// json 模式
        /// </summary>
        [DefaultValue(DataViewJsonMode.Simple)]
        [Description("json 模式")]
        public  DataViewJsonMode JsonMode
        {
            get { return m_JsonMode; }
            set { m_JsonMode = value; }
        }

        /// <summary>
        /// 选择模式
        /// </summary>
        [DefaultValue(CheckedMode.Multi)]
        [Description("选择模式")]
        public CheckedMode CheckedMode
        {
            get { return m_CheckedMode; }
            set { m_CheckedMode = value; }
        }


        /// <summary>
        /// 分页可视
        /// </summary>
        [DefaultValue(true)]
        public bool PagerVisible
        {
            get { return m_PagerVisible; }
            set { m_PagerVisible = value; }
        }

        /// <summary>
        /// 隐藏数量选择
        /// </summary>
        [DefaultValue(false)]
        public bool HidePagerRowCountSelect
        {
            get { return m_HidePagerRowCountSelect; }
            set { m_HidePagerRowCountSelect = false; }
        }


        /// <summary>
        /// 自动尺寸
        /// </summary>
        [DefaultValue(false)]
        public bool AutoSize
        {
            get { return m_AutoSize; }
            set { m_AutoSize = value; }
        }

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
        /// 项目模板
        /// </summary>
        [Description("项目模板")]
        [Browsable(false)]
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(System.Web.UI.WebControls.DataListItem))]
        public ITemplate ItemTemplate
        {
            get
            {
                if(m_ItemTemplate == null)
                {
                    m_ItemTemplate = new CompiledTemplateBuilder(new BuildTemplateMethod(ItemTemplate_Control));
                }
                return m_ItemTemplate;
            }
            set { m_ItemTemplate = value; }
        }

        [DebuggerNonUserCode]
        private void ItemTemplate_Control(Control con)
        {
            Console.WriteLine("啥情况???");
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
        /// 项目宽度
        /// </summary>
        [Description("项目宽度")]
        public string ItemWidth
        {
            get { return m_ItemWidth; }
            set { m_ItemWidth = value; }
        }

        /// <summary>
        /// 项目高度
        /// </summary>
        [Description("项目高度")]
        public string ItemHeight
        {
            get { return m_ItemHeight; }
            set { m_ItemHeight = value; }
        }


        /// <summary>
        /// item 项目的样式
        /// </summary>
        [Description("item 项目的样式")]
        public string ItemClass { get; set; }

        /// <summary>
        /// 选中的样式
        /// </summary>
        public string SelectClass { get; set; }

        /// <summary>
        /// 焦点的样式
        /// </summary>
        public string FocusClass { get; set; }

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



        #region Attribute

        internal Mini.MiniHtmlAttrCollection m_HtmlAttrs = new Mini.MiniHtmlAttrCollection();

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

    }
}
