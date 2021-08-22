using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using EasyClick.Web.Mini2.Data;
using EC5.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EasyClick.Web.Mini;
using System.Data;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 插入记录的位置
    /// </summary>
    public enum InsertPosition
    {
        /// <summary>
        /// 自动位置
        /// </summary>
        Auto,
        /// <summary>
        /// 第一位
        /// </summary>
        First,
        /// <summary>
        /// 最后位
        /// </summary>
        Last,

        /// <summary>
        /// 焦点行的后面
        /// </summary>
        FocusLast,
        /// <summary>
        /// 焦点行的前面
        /// </summary>
        FocusFirst
    }


    public enum StoreJsonMode
    {
        /// <summary>
        /// 简单模式
        /// </summary>
        Sample,
        /// <summary>
        /// 完全模式
        /// </summary>
        Full
    }

    /// <summary>
    /// 数据仓库
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public partial class Store : Component, IAttributeAccessor, IMiniControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 数据仓库
        /// </summary>
        public Store()
        {
            this.InReady = "Mini2.data.Store";
            this.JsNamespace = "Mini2.data.Store";
        }

        object m_Tag;

        InsertPosition m_DefaultInsertPos = InsertPosition.Auto;



        /// <summary>
        /// 上级字段名
        /// </summary>
        string m_ParentField;


        /// <summary>
        /// 增量加载数据
        /// </summary>
        [DefaultValue(false)]
        public bool IsIncr { get; set; } = false;


        /// <summary>
        /// 上级字段名
        /// </summary>
        [Description("上级字段名")]
        public string ParentField
        {
            get { return m_ParentField; }
            set { m_ParentField = value; }
        }

        /// <summary>
        /// 自动执行回调函数
        /// </summary>
        [DefaultValue(true)]
        public bool AutoCallback { get; set; } = true;


        /// <summary>
        /// 动态加载
        /// </summary>
        [Description("动态加载")]
        [DefaultValue(true)]
        public bool DymLoad { get; set; } = true;

        /// <summary>
        /// 数据仓库模式
        /// </summary>
        [DefaultValue(StoreJsonMode.Sample)]
        public StoreJsonMode JsonMode { get; set; } = StoreJsonMode.Sample;


        /// <summary>
        /// 默认插入记录的位置
        /// </summary>
        [DefaultValue(InsertPosition.Auto)]
        [Description("默认插入记录的位置")]
        public InsertPosition DefaultInsertPos
        {
            get { return m_DefaultInsertPos; }
            set { m_DefaultInsertPos = value; }
        }

        /// <summary>
        /// 数据参考名称
        /// </summary>
        string m_DbDecipherName;

        /// <summary>
        /// 数据仓库名称
        /// </summary>
        [DefaultValue("")]
        [Description("数据参考名称")]
        public string DbDecipherName
        {
            get { return m_DbDecipherName; }
            set { m_DbDecipherName = value; }
        }

        /// <summary>
        /// 用户自定义属性
        /// </summary>
        [Browsable(false)]
        public object Tag
        {
            get { return m_Tag; }
            set { m_Tag = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CreateChildControls();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            StoreEngine_OnInit();

            this.m_StoreEngine.OnLoad();

            //if (!this.Page.IsPostBack)
            //{
            //    this.DataBind();
            //}
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


        #region 数据仓库引擎

        /// <summary>
        /// 数据仓库引擎
        /// </summary>
        IStoreEngine m_StoreEngine;

        /// <summary>
        /// 数据引擎名称
        /// </summary>
        string m_EngineName;

        /// <summary>
        /// 数据仓库引擎
        /// </summary>
        public IStoreEngine StoreEngine
        {
            get { return m_StoreEngine; }
            set { m_StoreEngine = value; }
        }

        /// <summary>
        /// 数据引擎名称
        /// </summary>
        [Description("数据引擎名称")]
        public string EngineName
        {
            get { return m_EngineName; }
            set { m_EngineName = value; }
        }


        #endregion

        bool m_AutoBind = false;

        HiddenField m_StoreBatch;
        HiddenField m_ActionHid;

        string m_StringFields = null;

        string m_CustomData;

        ArrayList m_Data = new ArrayList();

        /// <summary>
        /// 实体类型
        /// </summary>
        string m_Model;

        /// <summary>
        /// 主键字段
        /// </summary>
        string m_IdField;

        /// <summary>
        /// 排序的字段
        /// </summary>
        string m_SortField;

        /// <summary>
        /// 锁的字段名
        /// </summary>
        string m_LockedField;

        int m_PageSize = 20;

        /// <summary>
        /// 自动保存
        /// </summary>
        bool m_AutoSave = true;

        /// <summary>
        /// 排序的字符串
        /// </summary>
        string m_SortText;


        /// <summary>
        /// 回收站模式
        /// </summary>
        bool m_DeleteRecycle = false;

        /// <summary>
        /// 汇总字段集合
        /// </summary>
        SummaryFieldCollection m_SummaryFields;

        /// <summary>
        /// 汇总的字段值
        /// </summary>
        SummaryItemCollection m_SummaryItems;

        /// <summary>
        /// 当前索引
        /// </summary>
        int m_CurIndex = -1;

        /// <summary>
        /// 客户端提交的数据,解析
        /// </summary>
        JObject m_SubmitJson = null;

        /// <summary>
        /// 拼接的 SQL 查询语句。这样切分，是为了照顾多数据分页的设计。
        /// </summary>
        StoreTSqlQuerty m_TSqlQuery;

        /// <summary>
        /// 只读模式
        /// </summary>
        bool m_ReadOnly = false;


        /// <summary>
        /// 锁行的 JS 规则
        /// </summary>
        string m_LockedRule;

        /// <summary>
        /// 自动触发焦点行
        /// </summary>
        bool m_AutoFocus = true;


        /// <summary>
        /// 事务模式激活
        /// </summary>
        bool m_TransactionEnabled = false;

        /// <summary>
        /// 设置锁后, 排除掉的字段...也就是不锁的字段名
        /// </summary>
        string[] m_LockedExclusionFields;

        /// <summary>
        /// 激活虚拟实体
        /// </summary>
        bool m_VirtualModelEnabled = true;

        /// <summary>
        /// 激活虚拟实体
        /// </summary>
        [DefaultValue(true)]
        public bool VirtualModelEnabled
        {
            get { return m_VirtualModelEnabled; }
            set { m_VirtualModelEnabled = value; }
        }


        /// <summary>
        /// 锁住后, 例外的字段名...也就是可以修改的字段
        /// </summary>
        public string[] LockedExclusionFields
        {
            get { return m_LockedExclusionFields; }
            set { m_LockedExclusionFields = value; }
        }

        /// <summary>
        /// 自动触发焦点行
        /// </summary>
        [Description("自动触发焦点行")]
        [DefaultValue(true)]
        public bool AutoFocus
        {
            get { return m_AutoFocus; }
            set { m_AutoFocus = value; }
        }


        /// <summary>
        /// 获取或设置数据库事务模式
        /// </summary>
        [Description("获取或设置数据库事务模式")]
        [DefaultValue(false)]
        public bool TranEnabled
        {
            get { return m_TransactionEnabled; }
            set { m_TransactionEnabled = value; }
        }




        /// <summary>
        /// 锁行规则。。暂时只支持 JS
        /// </summary>
        [Description("锁行的规则")]
        [DefaultValue("")]
        public string LockedRule
        {
            get { return m_LockedRule; }
            set { m_LockedRule = value; }
        }


        /// <summary>
        /// 只读模式
        /// </summary>
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return m_ReadOnly; }
            set
            {
                m_ReadOnly = value;

                if (!this.DesignMode)
                {
                    ScriptManager.Eval("{0}.setReadOnly({1});", this.ClientID, BoolUtil.ToJson(value));
                }
            }
        }

        /// <summary>
        /// 拼接的 SQL 查询语句
        /// </summary>
        /// <remarks>这样切分，是为了照顾多数据分页的设计。</remarks>
        [Description("拼接的 SQL 查询语句")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public StoreTSqlQuerty TSqlQuery
        {
            get
            {
                if (m_TSqlQuery == null)
                {
                    m_TSqlQuery = new StoreTSqlQuerty();
                }
                return m_TSqlQuery;
            }
        }


        /// <summary>
        /// 汇总的字段值
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public SummaryItemCollection SummaryItems
        {
            get
            {
                if (m_SummaryItems == null)
                {
                    m_SummaryItems = new SummaryItemCollection();
                }

                return m_SummaryItems;
            }
        }

        /// <summary>
        /// (支持JS)设置汇总值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public void SetSummary(string field, decimal value)
        {
            this.SummaryItems[field] = value;

            ScriptManager.Eval("{0}.setSummary('{1}', {2});", this.ClientID, field, value);
        }


        /// <summary>
        /// (支持JS)设置汇总值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public void SetSummary(string field, string value)
        {
            this.SummaryItems[field] = value;

            ScriptManager.Eval("{0}.setSummary('{1}', '{2}');", this.ClientID, field, JsonUtil.ToJson(value, JsonQuotationMark.SingleQuotes));
        }

        /// <summary>
        /// 是否有汇总字段
        /// </summary>
        /// <returns></returns>
        public bool HasSummaryField()
        {
            return (m_SummaryFields != null && m_SummaryFields.Count > 0);
        }

        /// <summary>
        /// 汇总字段集合
        /// </summary>
        [Description("汇总字段集合")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public SummaryFieldCollection SummaryFields
        {
            get
            {
                if (m_SummaryFields == null)
                {
                    m_SummaryFields = new SummaryFieldCollection();
                }
                return m_SummaryFields;
            }
        }


        /// <summary>
        /// 获取总结数据
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="summartyType">总结类型</param>
        /// <returns></returns>
        public decimal GetSummary(string field, SummaryType summartyType)
        {
            StoreEngine_OnInit();

            return this.StoreEngine.GetSummary(field, summartyType);
        }

        /// <summary>
        /// 获取总结数据
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="summartyType">总结类型</param>
        /// <param name="summartyFilter">总结过滤</param>
        /// <returns></returns>
        public decimal GetSummary(string field, SummaryType summartyType, ParamCollection summartyFilter)
        {
            StoreEngine_OnInit();

            return this.StoreEngine.GetSummary(field, summartyType, summartyFilter);
        }


        /// <summary>
        /// 自动绑定数据
        /// </summary>
        [Description("自动绑定数据")]
        [DefaultValue(false)]
        public bool AuotBind
        {
            get { return m_AutoBind; }
            set { m_AutoBind = value; }
        }

        /// <summary>
        /// 原始数据
        /// </summary>
        public ArrayList Data
        {
            get { return m_Data; }
        }


        /// <summary>
        /// 排序字段名
        /// </summary>
        [DefaultValue("")]
        [Description("排序字段名")]
        public string SortField
        {
            get { return m_SortField; }
            set { m_SortField = value; }
        }


        /// <summary>
        /// 获取客户端提交的 json 数据
        /// </summary>
        /// <returns></returns>
        private JObject ParanSbmitJson()
        {


            if (m_SubmitJson != null)
            {
                return m_SubmitJson;
            }

            string json = GetDataBatchJson();

            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    m_SubmitJson = (JObject)JsonConvert.DeserializeObject(json);
                }
                catch (Exception ex)
                {
                    log.Error("解析 json 数据错误: " + json, ex);
                }
            }

            return m_SubmitJson;
        }

        /// <summary>
        /// 焦点行索引
        /// </summary>
        [Description("焦点行索引")]
        [DefaultValue(-1)]
        public int CurIndex
        {
            get
            {
                if (m_CurIndex == -1)
                {
                    JObject o = ParanSbmitJson();

                    if (o != null)
                    {
                        JToken current = o["cur_index"];

                        if (current != null && current.HasValues)
                        {
                            m_CurIndex = current.Value<int>();
                        }
                    }
                }

                return m_CurIndex;
            }
            set
            {
                m_CurIndex = value;

                MiniScript.Add("{0}.setCurrent({1});", this.ClientID, value);
            }
        }

        /// <summary>
        /// 默认排序字符串。例： 字段名1 ASC,字段名2 DESC
        /// </summary>
        [Description("默认排序字符串。例： 字段名1 ASC,字段名2 DESC")]
        [DefaultValue("")]
        public string SortText
        {
            get { return m_SortText; }
            set
            {
                m_SortText = value;

                MiniScript.Add("{0}.sortText = \"{1}\"", this.ClientID, value);
            }
        }

        /// <summary>
        /// 激活删除回收
        /// </summary>
        [Description("激活删除回收")]
        [DefaultValue(false)]
        public bool DeleteRecycle
        {
            get { return m_DeleteRecycle; }
            set { m_DeleteRecycle = value; }
        }


        /// <summary>
        /// 自动保存
        /// </summary>
        [Description("自动保存")]
        [DefaultValue(true)]
        public bool AutoSave
        {
            get { return m_AutoSave; }
            set { m_AutoSave = value; }
        }


        /// <summary>
        /// 每页显示的记录数量
        /// </summary>
        [Description("每页显示的记录数量")]
        [DefaultValue("")]
        public int PageSize
        {
            get
            {
                DataRequest dr = GetAction();

                if (dr != null && dr.Page != null && dr.Page.End > 0)
                {
                    m_PageSize = dr.Page.Limit;
                }

                return m_PageSize;
            }
            set { m_PageSize = value; }
        }


        /// <summary>
        /// 主键字段
        /// </summary>
        [Description("主键字段")]
        [DefaultValue("")]
        public string IdField
        {
            get { return m_IdField; }
            set { m_IdField = value; }
        }


        /// <summary>
        /// 锁字段
        /// </summary>
        [Description("锁字段")]
        [DefaultValue("")]
        public string LockedField
        {
            get { return m_LockedField; }
            set { m_LockedField = value; }
        }



        /// <summary>
        /// 实体名称
        /// </summary>
        [Description("实体名称")]
        [DefaultValue("")]
        public string Model
        {
            get { return m_Model; }
            set { m_Model = value; }
        }

        /// <summary>
        /// 自定义数据.集合 
        /// </summary>
        /// <example>例: [{'F1':1,'F2':2},{...},...]</example>
        [Description("自定义数据.例:\n [{'F1':1,'F2':2},{...},...]")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string CustomData
        {
            get { return m_CustomData; }
            set { m_CustomData = value; }
        }


        /// <summary>
        /// 字符串字段
        /// </summary>
        [Description("字符串字段。例：字段1,字段2,字段3,.... ")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string StringFields
        {
            get { return m_StringFields; }
            set { m_StringFields = value; }
        }

        /// <summary>
        /// 记录总量。分页的时候使用.
        /// </summary>
        int m_TotalCount = 0;



        /// <summary>
        /// 设置记录总量。分页的时候使用
        /// </summary>
        /// <param name="value"></param>
        public void SetTotalCount(int value)
        {
            m_TotalCount = value;
            MiniScript.Add("{0}.setTotalCount({1});", this.ClientID, value);
        }

        /// <summary>
        /// 设置页码
        /// </summary>
        /// <param name="page"></param>
        public void SetCurrentPage(int page)
        {
            MiniScript.Add("{0}.setCurrentPage({1});", this.ClientID, page);
        }


        /// <summary>
        /// 按实体 id,设置焦点
        /// </summary>
        /// <param name="id"></param>
        public void SetCurrntForId(string id)
        {

            MiniScript.Add("{0}.setCurrntForId('{1}');", this.ClientID, id);
        }

        /// <summary>
        /// 设置焦点项目
        /// </summary>
        /// <param name="index"></param>
        public void SetCurrent(int index)
        {

            MiniScript.Add("{0}.setCurrent({1});", this.ClientID, index);

        }


        /// <summary>
        /// (JScript)添加数据
        /// </summary>
        /// <param name="item"></param>
        public void Add(string item)
        {
            m_Data.Add(item);

            if (MiniScriptManager.ClientScript.ReadOnly)
            {
                return;
            }


            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}.add(", this.ClientID);

            sb.Append(item);

            sb.Append(");");

            MiniScript.Add(sb.ToString());
        }

        /// <summary>
        /// (JScript)添加数据
        /// </summary>
        /// <param name="item"></param>
        public void Add(object item)
        {
            Insert(int.MaxValue, item);
        }

        /// <summary>
        /// (JScript)插入数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, object item)
        {
            int curIndex = index;

            if (index > m_Data.Count)
            {
                curIndex = m_Data.Count;
            }
            m_Data.Insert(curIndex, item);

            if (MiniScriptManager.ClientScript.ReadOnly)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}.insert({1},", this.ClientID, index);

            //string[] fields = null;

            string fieldJson = MiniConfiguration.JsonFactory.GetItemJson(item);

            sb.Append(fieldJson);

            sb.Append(");");

            MiniScript.Add(sb.ToString());
        }

        /// <summary>
        /// (JScript)插入数据集合
        /// </summary>
        /// <param name="index"></param>
        /// <param name="items"></param>
        public void InsertRange(int index, IList items)
        {
            if (items == null || items.Count == 0)
            {
                return;
            }

            int curIndex = index;

            if (index > m_Data.Count)
            {
                curIndex = m_Data.Count;
            }

            m_Data.InsertRange(curIndex, items);

            if (MiniScriptManager.ClientScript.ReadOnly)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}.insert({1}, ", this.ClientID, index);

            sb.Append("[");

            int i = 0;

            foreach (object item in items)
            {
                if (item == null) { continue; }

                if (i++ > 0) { sb.Append(","); }

                if (item is TreeModel)
                {
                    string tmJson = ((TreeModel)item).ToJson();

                    sb.Append(tmJson);
                }
                else
                {
                    string fieldJson = MiniConfiguration.JsonFactory.GetItemJson(item);

                    sb.Append(fieldJson);
                }
            }

            sb.Append("]");

            sb.Append(");");

            MiniScript.Add(sb.ToString());
        }

        /// <summary>
        /// 添加数据集合到末尾
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IList items)
        {
            InsertRange(int.MaxValue, items);
        }

        /// <summary>
        /// 添加数据集合到末尾
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(string items)
        {
            m_Data.Add(items);

            if (MiniScriptManager.ClientScript.ReadOnly)
            {
                return;
            }


            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}.add(", this.ClientID);

            sb.Append(items);

            sb.Append(");");

            MiniScript.Add(sb.ToString());
        }

        /// <summary>
        /// 删除全部数据
        /// </summary>
        public void RemoveAll()
        {
            m_Data.Clear();

            if (MiniScriptManager.ClientScript.ReadOnly)
            {
                return;
            }

            MiniScript.Add("{0}.removeAll();", this.ClientID);
        }

        /// <summary>
        /// 删除全部数据
        /// </summary>
        /// <param name="silent">静默： true=触发事件, false=静默</param>
        public void RemoveAll(bool silent)
        {
            m_Data.Clear();

            if (MiniScriptManager.ClientScript.ReadOnly)
            {
                return;
            }

            MiniScript.Add("{0}.removeAll({1});", this.ClientID, BoolUtil.ToJson(silent));
        }

        /// <summary>
        /// 按客户端的唯一 ID，删除记录
        /// </summary>
        /// <param name="clientId"></param>
        public void RemoveByClientId(string clientId)
        {
            MiniScript.Add("{0}.removeByMuid({1});", this.ClientID, clientId);
        }

        /// <summary>
        /// 按 ID 删除记录
        /// </summary>
        /// <param name="id"></param>
        public void RemoveById(string id)
        {
            MiniScript.Add($"{this.ClientID}.removeById('{id}');");
        }

        /// <summary>
        /// 按 ID 删除记录
        /// </summary>
        /// <param name="id"></param>
        public void RemoveById(int id)
        {
            MiniScript.Add($"{this.ClientID}.removeById('{id}');");
        }

        /// <summary>
        /// 按 ID 删除记录
        /// </summary>
        /// <param name="id"></param>
        public void RemoveById(long id)
        {
            MiniScript.Add($"{this.ClientID}.removeById('{id}');");
        }

        /// <summary>
        /// 按 ID 删除记录
        /// </summary>
        /// <param name="id"></param>
        public void RemoveById(object id)
        {
            MiniScript.Add($"{this.ClientID}.removeById('{id}');");
        }


        /// <summary>
        /// 按客户端的唯一 ID，删除记录
        /// </summary>
        /// <param name="record">记录行</param>
        public void Remove(DataRecord record)
        {
            MiniScript.Add("{0}.removeByMuid({1});", this.ClientID, record.ClientId);
        }

        /// <summary>
        /// 设置记录为正常状态
        /// </summary>
        public void CommitChanges()
        {
            MiniScript.Add("{0}.commitChanges();", this.ClientID);
        }

        /// <summary>
        /// 设置记录为正常状态
        /// </summary>
        /// <param name="record"></param>
        public void CommitChanges(DataRecord record)
        {
            string fieldsStr = GetJsonForRecord(record);

            MiniScript.Add("{0}.commitChangesForMuid({1},[{2}]);", this.ClientID, record.ClientId, fieldsStr);
        }

        /// <summary>
        /// 设置记录值
        /// </summary>
        /// <param name="id">字段主键值</param>
        /// <param name="field">需要更新的字段名</param>
        /// <param name="value">字段值</param>
        public void SetRecordValue(object id, string field, object value)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id", "字段主键值不能为空.");
            }

            Type idTyep = id.GetType();

            if (!idTyep.IsValueType && idTyep != typeof(string))
            {
                throw new ArgumentNullException("id", "字段主键值必须是值类型.");
            }

            SetRecordValue(false, id.ToString(), field, value);
        }


        /// <summary>
        /// 设置记录值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public void SetRecordValue(int id, string field, object value)
        {
            SetRecordValue(false, id.ToString(), field, value);
        }

        /// <summary>
        /// 设置记录值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public void SetRecordValue(string id, string field, object value)
        {
            SetRecordValue(false, id, field, value);
        }

        /// <summary>
        /// 设置记录值
        /// </summary>
        /// <param name="id">字段主键值</param>
        /// <param name="field">需要更新的字段名</param>
        /// <param name="value">字段值</param>
        /// <param name="dirty">弄脏. true=弄脏；false=正常</param>
        public void SetRecordValue(bool dirty, string id, string field, object value)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id", "字段主键值不能为空.");
            }

            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException("field", "字段名不能为空.");
            }



            string varStr = string.Empty;

            if (value == null)
            {
                varStr = "null";
            }
            else if (value is string)
            {
                varStr = "\"" + JsonUtil.ToJson(value) + "\"";
            }
            else if (value is DateTime)
            {
                varStr = "\"" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff") + "\"";
            }
            else if (value is bool)
            {
                varStr = BoolUtil.ToJson((bool)value);
            }
            else
            {
                varStr = value.ToString();
            }

            string js;

            if (dirty)
            {
                js = string.Format("{0}.setRecordValue(\"{1}\",\"{2}\", {3}, true)",
                    this.ClientID,
                    id, field, varStr);
            }
            else
            {
                js = string.Format("{0}.setRecordValue(\"{1}\",\"{2}\", {3})",
                    this.ClientID,
                    id, field, varStr);
            }

            MiniScript.Add(js);
        }

        private string GetJsonForRecord(DataRecord record)
        {
            DataFieldCollection fields = record.Fields;
            StringBuilder sb = new StringBuilder();

            if (fields != null && fields.Count > 0)
            {

                sb.Append("'").Append(fields[0].Name).Append("'");

                for (int i = 1; i < fields.Count; i++)
                {
                    sb.Append(",'").Append(fields[i].Name).Append("'");
                }
            }

            return sb.ToString();
        }

        protected string GetJson(string[] items)
        {
            StringBuilder sb = new StringBuilder();

            //string[] fs = StringUtil.Split(m_Fields);

            if (items != null && items.Length > 0)
            {
                string item = items[0].Trim();
                sb.Append("'").Append(item).Append("'");

                for (int i = 1; i < items.Length; i++)
                {
                    item = items[i].Trim();
                    sb.Append(",'").Append(item).Append("'");
                }
            }

            return sb.ToString();
        }

        bool m_InitCreateChild = false;

        protected override void CreateChildControls()
        {
            if (m_InitCreateChild)
            {
                return;
            }

            m_InitCreateChild = true;

            base.CreateChildControls();

            m_StoreBatch = new HiddenField();
            m_StoreBatch.ID = this.ID + "_Batch";
            this.Controls.Add(m_StoreBatch);

            m_ActionHid = new HiddenField();
            m_ActionHid.ID = this.ID + "_Action";
            this.Controls.Add(m_ActionHid);

        }


        /// <summary>
        /// 获取数据记录批次  Json 数据
        /// </summary>
        /// <returns></returns>
        public string GetDataBatchJson()
        {
            if (m_StoreBatch == null)
            {
                return string.Empty;
            }

            return m_StoreBatch.Value;
        }




        /// <summary>
        /// 获取当前焦点的记录
        /// </summary>
        /// <returns></returns>
        public DataRecord GetDataCurrent()
        {
            string json = GetDataBatchJson();

            if (string.IsNullOrEmpty(json))
            {
                return null;
            }


            JObject o = ParanSbmitJson();

            if (o == null)
            {
                return null;
            }

            JToken current = o["current"];

            if (current == null)
            {
                return null;
            }

            DataRecord record = DataRecord.Parse(current);

            return record;
        }

        /// <summary>
        /// 获取当前焦点的id值。一般是主键值
        /// </summary>
        [Description("获取当前焦点的id值。一般是主键值")]
        public string CurDataId
        {
            get
            {
                DataRecord record = GetDataCurrent();

                if (record == null)
                {
                    return string.Empty;
                }


                return record.Id;
            }
        }


        /// <summary>
        /// 获取数据更改的记录批次
        /// </summary>
        /// <returns></returns>
        public DataBatch GetDataChangedBatch()
        {
            string json = GetDataBatchJson();

            DataBatch ds = null;

            try
            {
                ds = DataBatch.Parse(json);
            }
            catch (Exception ex)
            {
                log.Error("解析 Json 格式错误: " + json, ex);
            }

            return ds;
        }


        /// <summary>
        /// 获取动作Json
        /// </summary>
        /// <returns></returns>
        public string GetActionJson()
        {
            if (m_ActionHid == null)
            {
                return string.Empty;
            }
            return m_ActionHid.Value;
        }

        DataRequest m_DataRequest = null;

        /// <summary>
        /// 获取提交的数据
        /// </summary>
        /// <returns></returns>
        public DataRequest GetAction()
        {
            if (m_DataRequest != null)
            {
                return m_DataRequest;
            }

            string json = GetActionJson();

            DataRequest dr = DataRequest.Parse(json);

            m_DataRequest = dr;

            return dr;

        }



        private void RenderStoreBatch(System.Web.UI.HtmlTextWriter writer)
        {
            string jsCode = string.Format("$('#{0}').val({1}.getUploadJson())", m_StoreBatch.ClientID, this.ClientID);
            m_StoreBatch.SetAttribute("SubmitBufore", jsCode);
            m_StoreBatch.RenderControl(writer);
        }

        private void RenderAction(System.Web.UI.HtmlTextWriter writer)
        {
            string jsActionCode = string.Format("$('#{0}').val({1}.getRequestJson())", m_ActionHid.ClientID, this.ClientID);
            m_ActionHid.SetAttribute("SubmitBufore", jsActionCode);
            m_ActionHid.RenderControl(writer);


        }

        /// <summary>
        /// 填充数据仓库
        /// </summary>
        /// <param name="sb"></param>
        private void FullStoreBatch(StringBuilder sb)
        {
            string jsCode = string.Format("$('#{0}').val({1}.getUploadJson())", m_StoreBatch.ClientID, this.ClientID);
            m_StoreBatch.SetAttribute("SubmitBufore", jsCode);
            m_StoreBatch.FullScript(sb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        private void FullAction(StringBuilder sb)
        {
            string jsActionCode = string.Format("$('#{0}').val({1}.getRequestJson())", m_ActionHid.ClientID, this.ClientID);
            m_ActionHid.SetAttribute("SubmitBufore", jsActionCode);
            m_ActionHid.FullScript(sb);
        }


        protected virtual void FullScript(StringBuilder sb)
        {


            sb.AppendLine("  var store = Mini2.create('Mini2.data.Store', {");
            JsParam(sb, "storeId", this.ClientID);
            JsParam(sb, "id", this.ID);
            JsParam(sb, "pageSize", this.PageSize);         //每页显示的数量
            JsParam(sb, "totalCount", this.m_TotalCount);   //记录总量，分页用.

            JsParam(sb, "sortText", m_SortText);    //默认排序字符串

            JsParam(sb, "autoSave", m_AutoSave, true);      //自动保存

            JsParam(sb, "jsonMode", this.JsonMode, StoreJsonMode.Sample, TextTransform.Lower);

            JsParam(sb, "idField", m_IdField);              //主键字段
            JsParam(sb, "lockedField", m_LockedField);      //锁字段

            JsParam(sb, "autoCallback", this.AutoCallback, true);//自动触发回调函数
            JsParam(sb, "autoFocus", m_AutoFocus, true);    //自动触焦点行事件

            JsParam(sb, "isIncr", this.IsIncr, false);  //增量模式

            if (LockedExclusionFields != null && LockedExclusionFields.Length > 0)
            {

                JsParam(sb, "lockedExclusionFields", LockedExclusionFields);

            }

            if (!String.IsNullOrEmpty(m_LockedRule))
            {
                JsParam(sb, "lockedRule", m_LockedRule.Replace("'", "\'"));
            }

            JsParam(sb, "model", m_Model);

            JsParam(sb, "readOnly", this.ReadOnly, false);

            if (!string.IsNullOrEmpty(m_StringFields))
            {
                string strFields = m_StringFields.Replace("\n", string.Empty)
                    .Replace("\r", string.Empty)
                    .Replace("\t", string.Empty);

                string[] fs = StringUtil.Split(strFields);
                string fieldsStr = GetJson(fs);

                sb.AppendFormat("    fields:[{0}],", fieldsStr);
            }


            if (!this.Page.IsPostBack)
            {
                //sb.AppendLine();
                //JsParam(sb, "current", m_CurrentIndex,-1);

                if (!string.IsNullOrEmpty(m_CustomData))
                {
                    sb.AppendLine();
                    sb.AppendFormat("    data:{0}, \n", m_CustomData);
                }
                else
                {
                    StringBuilder dataSb = new StringBuilder();

                    if (m_Data != null && m_Data.Count > 0)
                    {
                        string fieldJson;

                        for (int i = 0; i < this.m_Data.Count; i++)
                        {
                            if (i > 0) { dataSb.AppendLine(",").Append("        "); };

                            var data = m_Data[i];

                            if (data is TreeModel)
                            {
                                string tmJson = ((TreeModel)data).ToJson();

                                dataSb.Append(tmJson);
                            }
                            else if (data is string)
                            {
                                dataSb.Append(data);
                            }
                            else
                            {
                                fieldJson = MiniConfiguration.JsonFactory.GetItemJson(data);

                                dataSb.Append(fieldJson);
                            }
                        }
                    }

                    sb.AppendLine();
                    sb.AppendFormat("    data:[ {0} ],\n", dataSb.ToString());
                }
            }

            if (this.AutoCallback && this.AutoFocus && Has(StoreEvent.CurrentChanged))
            {
                sb.AppendLine("    currentChanged: function(){");

                sb.AppendLine("        widget1.subMethod('form:first',{");

                sb.AppendFormat("          subName:'{0}',", this.ID).AppendLine();
                sb.AppendFormat("          subMethod:'{0}'", "PreCurrentChanged").AppendLine();

                sb.AppendLine("        });");

                sb.AppendLine("    },");
            }

            if (m_SummaryItems != null && m_SummaryItems.Count > 0)
            {
                sb.AppendLine("        summary: {");

                for (int i = 0; i < m_SummaryItems.Count; i++)
                {
                    string field = m_SummaryItems.Keys[i];

                    object value = m_SummaryItems[field];

                    if (i > 0) { sb.Append(","); }


                    if (value == null)
                    {
                        sb.AppendFormat("{0}: null", field);
                    }
                    else if (TypeUtil.IsNumberType(value.GetType()))
                    {
                        sb.AppendFormat("{0}:{1}", field, value);
                    }
                    else
                    {
                        sb.AppendFormat("{0}: '{1}'", field, JsonUtil.ToJson(value.ToString(), JsonQuotationMark.SingleQuotes));
                    }

                    //if (i > 0) { sb.Append(","); }

                    //sb.AppendFormat("{0}:{1}", field, m_SummaryItems[field]);
                }

                sb.AppendLine("        },");
            }

            sb.AppendLine("        isStore:true");

            sb.AppendLine("  });");

            sb.AppendFormat("  window.{0} = store;\n", this.ClientID);
            sb.AppendFormat("Mini2.onwerPage.controls['{0}'] = store;\n", this.ID);
        }


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {

                return;
            }


            ScriptManager script = ScriptManager.GetManager(this.Page);

            StringBuilder sb = new StringBuilder();

            EnsureChildControls();

            RenderStoreBatch(writer);
            RenderAction(writer);

            if (script != null)
            {
                StringBuilder jsSb = new StringBuilder();

                //FullStoreBatch(jsSb);
                //FullAction(jsSb);

                BeginReady(jsSb);

                FullScript(jsSb);

                EndReady(jsSb);

                script.AddScript(jsSb.ToString());
            }
            else
            {
                BeginScript(sb);
                BeginReady(sb);

                FullScript(sb);

                EndReady(sb);
                EndScript(sb);

            }


            writer.Write(sb.ToString());

        }



        public void LoadPostData()
        {

            //HttpContext context = HttpContext.Current;

            //HttpRequest request = context.Request;

            //m_StoreHField.Value = request.Form[m_StoreHField.ClientID];
        }


        #region IStoreEngine

        /// <summary>
        /// 实体引擎
        /// </summary>
        protected virtual void StoreEngine_OnInit()
        {
            if (m_StoreEngine != null || this.DesignMode)
            {
                return;
            }


            if (StringUtil.IsBlank(m_EngineName))
            {
                m_StoreEngine = StoreEngineManager.GetDefaultStoreEngine();
            }
            else
            {
                m_StoreEngine = StoreEngineManager.GetStoreEngine(m_EngineName);
            }

            if (m_StoreEngine == null)
            {
                throw new Exception(string.Format("'{0}' 数据库引擎不存在.", m_EngineName));
            }


            m_StoreEngine.Store = this;
        }


        int m_CurPage = 0;

        /// <summary>
        /// 获取或设置当前页面索引
        /// </summary>
        [Description("获取或设置当前页面索引")]
        public int CurPage
        {
            get
            {
                if (this.DesignMode)
                {
                    return m_CurPage;
                }

                StoreEngine_OnInit();
                return m_StoreEngine.CurPage;
            }
            set
            {
                if (this.DesignMode)
                {
                    m_CurPage = value;
                    return;
                }

                StoreEngine_OnInit();

                m_StoreEngine.CurPage = value;
            }
        }


        /// <summary>
        /// 将数据源绑定到被调用的服务器控件及其所有子控件。
        /// </summary>
        public override void DataBind()
        {
            try
            {
                if (this.IsIncr)
                {
                    this.RemoveAll();

                    this.CurIndex = 0;
                    this.CurPage = 0;
                }

                this.LoadPage(this.CurPage);
            }
            catch (Exception ex)
            {
                log.Error("绑定数据异常。", ex);
                throw new Exception("绑定数据异常", ex);
            }
        }

        private void InitAction()
        {
            //DataRequest dr = GetAction();

            //this.m_s
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh()
        {
            if (this.IsIncr)
            {
                this.RemoveAll();

                this.CurIndex = 0;
                this.CurPage = 0;
            }


            DataRequest dr = GetAction();

            StoreEngine_OnInit();

            int curPage = dr.Page.CurrentPage;
            m_StoreEngine.CurPage = curPage;

            this.LoadPage(curPage);
        }

        /// <summary>
        /// 刷新汇总信息
        /// </summary>
        public void RefreshSummary()
        {
            DataRequest dr = GetAction();

            StoreEngine_OnInit();

            m_StoreEngine.LoadSummary();
        }


        /// <summary>
        /// 在加载数据时关闭通知、索引维护和约束。
        /// </summary>
        public void BeginLoadData()
        {
            MiniScript.Add("{0}.beginLoadData();", this.ClientID);
        }

        /// <summary>
        /// 在加载数据后打开通知、索引维护和约束。
        /// </summary>
        public void EndLoadData()
        {
            MiniScript.Add("{0}.endLoadData();", this.ClientID);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public IList Search()
        {
            bool cancel = OnSearching();

            if (cancel)
            {
                return null;
            }

            return LoadPage(0);
        }

        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public virtual IList LoadPage(int page)
        {

            StoreEngine_OnInit();

            DataRequest dr = GetAction();

            string tSqlSort = StringUtil.NoBlank(dr.TSqlSort, this.SortText);

            bool pageCancel = OnPageLoading(page, tSqlSort);

            if (pageCancel) { return null; }


            bool cancel = OnFiltering(null, null);

            if (cancel) { return null; }

            m_StoreEngine.PageSize = this.PageSize;
            m_StoreEngine.CurPage = page;


            IList dataList = m_StoreEngine.LoadPage(page);

            int itemTotal = m_StoreEngine.ItemTotal;

            OnPageLoaded(dataList);

            this.BeginLoadData();
            {
                if (!this.IsIncr)
                {
                    this.RemoveAll();
                }

                this.AddRange(dataList);

                this.SetTotalCount(itemTotal);
                this.SetCurrentPage(page);

                if (m_AutoFocus && dataList.Count > 0)
                {
                    this.SetCurrent(0);

                    if (m_AutoFocus && !Page.IsPostBack)
                    {
                        OnCurrentChanged(null, dataList[0], null);
                    }
                }
                else
                {
                    this.SetCurrent(this.CurIndex);

                    if (m_AutoFocus && !Page.IsPostBack)
                    {
                        if (this.CurIndex >= 0 && this.CurIndex < dataList.Count)
                        {
                            OnCurrentChanged(null, dataList[this.CurIndex], null);
                        }
                    }
                }

            }
            this.EndLoadData();

            OnPageChanged();

            return dataList;
        }


        public void SetCurRecord(DataRecord record)
        {
            StoreEngine_OnInit();

            m_StoreEngine.SetCurRecord(record);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList Select()
        {
            StoreEngine_OnInit();

            bool cancel = OnFiltering(null, null);

            if (cancel) { return null; }

            IList dataList = m_StoreEngine.Select();
            return dataList;
        }


        /// <summary>
        /// 全部保存
        /// </summary>
        /// <returns></returns>
        public int SaveAll()
        {
            StoreEngine_OnInit();

            int count = m_StoreEngine.SaveAll();

            return count;

        }


        /// <summary>
        /// 插入记录
        /// </summary>
        /// <returns></returns>
        public virtual int Insert()
        {
            StoreEngine_OnInit();

            int count = m_StoreEngine.Insert();
            return count;
        }


        /// <summary>
        /// 删除记录
        /// </summary>
        /// <returns></returns>
        public virtual int Delete()
        {
            StoreEngine_OnInit();

            int count = m_StoreEngine.Delete();

            if (count > 0)
            {
                this.Refresh();
            }

            return count;
        }

        /// <summary>
        /// 焦点行排序-上移
        /// </summary>
        public bool MoveUp()
        {
            StoreEngine_OnInit();

            bool result = m_StoreEngine.MoveUp();

            return result;
        }

        /// <summary>
        /// 焦点行排序-下移
        /// </summary>
        public bool MoveDown()
        {
            StoreEngine_OnInit();

            bool result = m_StoreEngine.MoveDown();

            return result;
        }

        /// <summary>
        /// 重建排序索引
        /// </summary>
        /// <returns></returns>
        public bool SortReset()
        {
            StoreEngine_OnInit();

            bool result = m_StoreEngine.SortReset();

            return result;
        }


        [Browsable(false)]
        public bool ContainsListCollection
        {
            get
            {
                StoreEngine_OnInit();

                bool cc = m_StoreEngine.ContainsListCollection;
                return cc;
            }
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        /// <returns></returns>
        public IList GetList()
        {
            StoreEngine_OnInit();

            bool cancel = OnFiltering(null, null);

            if (cancel) { return null; }

            IList dataList = m_StoreEngine.GetList();

            return dataList;
        }


        public DataTable GetDataTable()
        {
            StoreEngine_OnInit();

            bool cancel = OnFiltering(null, null);

            if (cancel) { return null; }

            DataTable dataList = m_StoreEngine.GetDataTable();

            return dataList;
        }


        /// <summary>
        /// 获取第一条记录
        /// </summary>
        /// <returns></returns>
        public object GetFirstData()
        {
            IList datas = GetList();

            if (datas.Count > 0)
            {
                return datas[0];
            }

            return null;
        }


        /// <summary>
        /// 清理异常
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="field"></param>
        public void ClearInvalid(object recordId, string field)
        {

        }


        /// <summary>
        /// 创建异常信息
        /// </summary>
        /// <param name="recordId">数据记录的 ID</param>
        /// <param name="invalidType">异常类型</param>
        /// <param name="field">字段名</param>
        /// <param name="message">消息</param>
        public void MarkInvalid(object recordId, string invalidType, string field, string message)
        {
            string error = string.Format("{{ type:\"{0}\", field:\"{1}\", message:\"{2}\"}}", invalidType, field, JsonUtil.ToJson(message));


            ScriptManager.Eval("{0}.markInvalid_ByRecordId('{1}', {2});", this.ClientID, recordId, error);
        }

        /// <summary>
        /// 创建异常信息
        /// </summary>
        /// <param name="record">数据记录</param>
        /// <param name="invalidType">异常类型</param>
        /// <param name="field">字段名</param>
        /// <param name="message">消息</param>
        public void MarkInvalid(DataRecord record, string invalidType, string field, string message)
        {
            MarkInvalid(record.Id, invalidType, field, message);
        }

        #endregion

    }


}
