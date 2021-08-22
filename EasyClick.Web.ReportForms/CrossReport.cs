using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using EasyClick.Web.ReportForms.Data;
using System.Reflection;
using System.Data;
using EC5.Utility;
using HWQ.Entity.LightModels;
using EC5.AppDomainPlugin;

namespace EasyClick.Web.ReportForms
{
    /// <summary>
    /// 报表数据类型
    /// </summary>
    public enum ReportRowType
    {
        /// <summary>
        /// 常规数据
        /// </summary>
        Common,

        /// <summary>
        /// 期初数据
        /// </summary>
        BeginningBalance,

        /// <summary>
        /// 期末数据
        /// </summary>
        EnddingBalance
    }

    /// <summary>
    /// 交叉报表模板
    /// </summary>
    public class CrossReport
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ReportItemGroupCollection m_ColTags;

        ReportItemGroupCollection m_RowTags;

        /// <summary>
        /// 主数据区明细数据
        /// </summary>
        ReportItemGroupCollection m_DataTags;
        
        IList m_DataList = null;




        /// <summary>
        /// 数据区位置
        /// </summary>
        ReportDataLayout m_DataLayout = ReportDataLayout.Top;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }


        #region 期初/期末 

        /// <summary>
        /// 激活期初
        /// </summary>
        [DefaultValue(false)]
        public bool EnabledBeginningBalance { get; set; }

        /// <summary>
        /// 激活期末
        /// </summary>
        [DefaultValue(false)]
        public bool EnabledEndingBalance { get; set; }


        /// <summary>
        /// 期初开始时间
        /// </summary>
        public string BeginningBalanceTime { get; set; }



        /// <summary>
        /// 日期字段(用于期初/期末使用)
        /// </summary>
        public string DateField { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? DateFrom { get; set; }
        

        #endregion



        /// <summary>
        /// 
        /// </summary>
        public CrossReport()
        {
            Init();
        }

        private void Init()
        {
            m_ColTags = new ReportItemGroupCollection();
            //m_ColTags.Add(new ReportItemGroup());

            m_RowTags = new ReportItemGroupCollection();
            //m_RowTags.Add(new ReportItemGroup());


            m_DataTags = new ReportItemGroupCollection();
            //m_DataTags.Add(new ReportItemGroup());
        }


        /// <summary>
        /// 列集合
        /// </summary>
        public ReportItemGroupCollection ColGroupTags
        {
            get { return m_ColTags; }
        }

        /// <summary>
        /// 行区域
        /// </summary>
        public ReportItemGroupCollection RowGroupTags
        {
            get { return m_RowTags; }
        }

        /// <summary>
        /// 数据区域
        /// </summary>
        public ReportItemGroupCollection DataGroupTags
        {
            get { return m_DataTags; }
        }
        

        /// <summary>
        /// 数据区域
        /// </summary>
        [DefaultValue(ReportDataLayout.Bottom)]
        public ReportDataLayout DataLayout
        {
            get { return m_DataLayout; }
            set { m_DataLayout = value; }
        }



        CrossHeadTreeNode m_XTreeRoot;
        CrossRowHeadTreeNode m_YTreeRoot;


        /// <summary>
        /// X轴 期初合计
        /// </summary>
        List<CrossHeadTreeNode> m_XTotal_BB_Nodes;

        /// <summary>
        /// X轴 期末合计
        /// </summary>
        List<CrossHeadTreeNode> m_XTotal_EB_Nodes;

        /// <summary>
        /// X轴 合计
        /// </summary>
        List<CrossHeadTreeNode> m_XTotalNodes;

        /// <summary>
        /// Y轴 合计
        /// </summary>
        //List<CrossHeadTreeNode> m_YTotalNodes;

        /// <summary>
        /// Y轴的固定
        /// </summary>
        List<CrossRowHeadTreeNode> m_RowHands;

        /// <summary>
        /// Y轴的统计的行
        /// </summary>
        List<CrossRowHeadTreeNode> m_RowTotalHands;


        RTable m_Table;
        int m_CurRow = 0;

        /// <summary>
        /// 激活列小计
        /// </summary>
        bool m_EnabledColTotal = true;

        /// <summary>
        /// 激活行小计
        /// </summary>
        bool m_EnabledRowTotal = true;

        /// <summary>
        /// 激活列小计
        /// </summary>
        [DefaultValue(true)]
        public bool EnabledColTotal
        {
            get { return m_EnabledColTotal; }
            set { m_EnabledColTotal = value; }
        }

        /// <summary>
        /// 激活行小计
        /// </summary>
        [DefaultValue(true)]
        public bool EnabledRowTotal
        {
            get { return m_EnabledRowTotal; }
            set { m_EnabledRowTotal = value; }
        }


        /// <summary>
        /// 期末
        /// </summary>
        /// <param name="xTreeRoot"></param>
        private void CreateTableHead_EndingBalance(CrossHeadTreeNode xTreeRoot)
        {
            m_XTotal_EB_Nodes = new List<CrossHeadTreeNode>();

            //期初的统计节点
            CrossHeadTreeNode bbTotalNode = new CrossHeadTreeNode();
            bbTotalNode.Column = new ReportColumn("期末");
            xTreeRoot.Childs.Add(bbTotalNode);

            if (this.DataGroupTags.Count <= 1)
            {
                ReportItemGroup riGroup = this.DataGroupTags[0];
                ReportItem rItem = riGroup[0];

                bbTotalNode.Column.Name = rItem.DBField;
                bbTotalNode.Column.Text = "期末 " + rItem.Title;
                bbTotalNode.Column.Width = rItem.Width;

                bbTotalNode.NodeType = CrossHeadTreeNodeTypes.Total;

                m_XTotal_EB_Nodes.Add(bbTotalNode);
            }
            else
            {
                foreach (ReportItemGroup riGroup in this.DataGroupTags)
                {
                    ReportItem rItem = riGroup[0];

                    CrossHeadTreeNode totalAll = new CrossHeadTreeNode();

                    totalAll.Column = new ReportColumn();
                    totalAll.Column.Name = rItem.DBField;
                    totalAll.Column.Text = rItem.Title;
                    totalAll.Column.Width = rItem.Width;

                    //准备...totalAll.TotalNodes = xTreeRoot;
                    //List<CrossHeadTreeNode> dataTotals = null;
                    //if (m_DataTotalNodes.TryGetValue(rItem.DBField, out dataTotals))
                    //{
                    //    foreach (var dataTotalNode in dataTotals)
                    //    {
                    //        totalAll.TotalNodes.Add(dataTotalNode);
                    //    }
                    //}


                    totalAll.NodeType = CrossHeadTreeNodeTypes.Total;


                    bbTotalNode.Childs.Add(totalAll);
                    //totalNodes.Add(totalAll);


                    m_XTotal_EB_Nodes.Add(totalAll);
                }
            }
        }

        /// <summary>
        /// 期初
        /// </summary>
        private void CreateTableHead_BeginningBalanc(CrossHeadTreeNode xTreeRoot)
        {
            m_XTotal_BB_Nodes = new List<CrossHeadTreeNode>();  //期初-合计

            //期初的统计节点
            CrossHeadTreeNode bbTotalNode = new CrossHeadTreeNode();
            bbTotalNode.Column = new ReportColumn("期初");
            xTreeRoot.Childs.Insert(0, bbTotalNode);


            if (this.DataGroupTags.Count <= 1)
            {
                ReportItemGroup riGroup = this.DataGroupTags[0];
                ReportItem rItem = riGroup[0];

                //bbTotalNode.Column.Name = "(TOTAL_BB)";
                bbTotalNode.Column.Text = "期初 " + rItem.Title;
                bbTotalNode.Column.Name = rItem.DBField;
                bbTotalNode.Column.Width = rItem.Width;

                bbTotalNode.NodeType = CrossHeadTreeNodeTypes.Total;


                m_XTotal_BB_Nodes.Add(bbTotalNode);
            }
            else
            {
                foreach (ReportItemGroup riGroup in this.DataGroupTags)
                {
                    ReportItem rItem = riGroup[0];

                    CrossHeadTreeNode totalAll = new CrossHeadTreeNode();

                    totalAll.Column = new ReportColumn();
                    totalAll.Column.Name = rItem.DBField;
                    totalAll.Column.Text = rItem.Title;
                    totalAll.Column.Width = rItem.Width;
                    totalAll.Column.Code = rItem.Code;
                    totalAll.Column.ValueMode = rItem.ValueMode;

                    //准备...totalAll.TotalNodes = xTreeRoot;
                    //List<CrossHeadTreeNode> dataTotals = null;
                    //if (m_DataTotalNodes.TryGetValue(rItem.DBField, out dataTotals))
                    //{
                    //    foreach (var dataTotalNode in dataTotals)
                    //    {
                    //        totalAll.TotalNodes.Add(dataTotalNode);
                    //    }
                    //}


                    totalAll.NodeType = CrossHeadTreeNodeTypes.Total;


                    bbTotalNode.Childs.Add(totalAll);
                    //totalNodes.Add(totalAll);


                    m_XTotal_BB_Nodes.Add(totalAll);
                }
            }
        }

        /// <summary>
        /// 创建表格头部
        /// </summary>
        public void CreateTableHead()
        {
            CrossHeadTreeNode xTreeRoot = new CrossHeadTreeNode();
            xTreeRoot.Column = null;


            
            List<CrossHeadTreeNode> totalNodes = new List<CrossHeadTreeNode>(); // 统计类型的节点

            RTable table = new RTable();


            CreatHeadTreeNode2(m_RootCol_ValueIndex, xTreeRoot, m_ColTags, totalNodes, 0);



            //如果开启"期初"
            if (this.EnabledBeginningBalance)
            {
                CreateTableHead_BeginningBalanc(xTreeRoot);
            }


            //多项数据区的统计
            if (this.DataGroupTags.Count > 1)
            {


                foreach( ReportItemGroup riGroup in this.DataGroupTags)
                {
                    ReportItem rItem = riGroup[0];

                    CrossHeadTreeNode totalAll = new CrossHeadTreeNode();

                    totalAll.Column = new ReportColumn();
                    totalAll.Column.Name = "(TOTAL)";
                    totalAll.Column.Text = rItem.Title;
                    totalAll.Column.Width = rItem.Width;

                    totalAll.Column.Code = rItem.Code;
                    totalAll.Column.ValueMode = rItem.ValueMode;

                    totalAll.Column.Format = rItem.Format;
                    totalAll.Column.FormatType = rItem.FormatType;
                    totalAll.Column.Value = rItem.DBField;

                    //准备...totalAll.TotalNodes = xTreeRoot;
                    List<CrossHeadTreeNode> dataTotals = null;
                    if (m_DataTotalNodes.TryGetValue(rItem.DBField, out dataTotals))
                    {
                        foreach (var dataTotalNode in dataTotals)
                        {
                            totalAll.TotalNodes.Add(dataTotalNode);
                        }
                    }


                    totalAll.NodeType = CrossHeadTreeNodeTypes.Total;


                    xTreeRoot.Childs.Add(totalAll);
                    totalNodes.Add(totalAll);
                }
            }            
            else if (m_EnabledColTotal) //单项数据区的统计
            {

                CrossHeadTreeNode totalAll = new CrossHeadTreeNode();
                totalAll.Column = new ReportColumn();
                totalAll.Column.Name = "(TOTAL)";
                totalAll.Column.Text = "总计";
                totalAll.TotalNodes.Add(xTreeRoot);
                totalAll.NodeType = CrossHeadTreeNodeTypes.Total;

                //totalAll.Column.Format = rItem.Format;
                //totalAll.Column.FormatType = rItem.FormatType;

                xTreeRoot.Childs.Add(totalAll);

                totalNodes.Add(totalAll);
            }




            //如果开启"期初"
            if (this.EnabledEndingBalance)
            {
                CreateTableHead_EndingBalance(xTreeRoot);
            }

            


            try
            {
                Tree2TableHand(table.Head, xTreeRoot, table, 0, 0);   //Tree 转换为 Table 表格对象
            }
            catch (Exception ex)
            {
                throw new Exception("Tree 结构转换为 Table 表格头错误.", ex);
            }

            RHead hand = table.Head;

            hand.PadRight();
            hand.ProColSpan();
            hand.ProRowSpan();

            SetNodeIndex(hand);
            
            m_XTreeRoot = xTreeRoot;
            m_XTotalNodes = totalNodes;

            m_Table = table;
        }

        /// <summary>
        /// 
        /// </summary>
        public CrossHeadTreeNode HeadTreeRoot
        {
            get { return m_XTreeRoot; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CrossRowHeadTreeNode RowTreeRoot
        {
            get { return m_YTreeRoot; }
        }


        /// <summary>
        /// 
        /// </summary>
        public void CreateTableRowHead()
        {
            CrossRowHeadTreeNode yTreeRoot = new CrossRowHeadTreeNode();
            yTreeRoot.Column = null;

            List<CrossHeadTreeNode> totalNodes = new List<CrossHeadTreeNode>();

            //创建树结构
            CreatHeadTreeNode2(m_RootRow_ValueIndex, yTreeRoot, m_RowTags, totalNodes, 0);    //创建 Tree 树节点结构




            // 1.构造 Table 的树结构
            // 2.负责 数据
            List<CrossRowHeadTreeNode> rowHeads = new List<CrossRowHeadTreeNode>();
            List<CrossRowHeadTreeNode> rowTotalHands = new List<CrossRowHeadTreeNode>();


            RRowCollection rows = m_Table.Body;

            //'树结构'转换为'表格结构'
            TreeToRowHand(yTreeRoot, rows, rowHeads, rowTotalHands, 0);

            if (m_EnabledRowTotal)
            {
                CrossRowHeadTreeNode totalAll = CreateTotalNode(yTreeRoot, yTreeRoot, "总计", 0);
                totalAll.Column.Text = "总计";

                rowTotalHands.Add(totalAll);
            }


            m_RowHands = rowHeads;
            m_RowTotalHands = rowTotalHands;
            m_YTreeRoot = yTreeRoot;
        }


        /// <summary>
        /// 设置树节点的索引
        /// </summary>
        /// <param name="rows"></param>
        private void SetNodeIndex(RRowCollection rows)
        {
            int rowCount = rows.Count;
            RRow row = rows.GetRow(rowCount - 1);

            for (int i = 0; i < row.Count; i++)
            {
                RCell cell = row.GetCell(i);

                if (cell.IsMergeChild)
                {
                    cell = rows.GetCell(cell.MergeOwnerY, cell.MergeOwnerX);


                }

                CrossHeadTreeNode treeNode = cell.TreeNode;

                treeNode.X = i;
            }

        }





        ValueIndexNode m_RootRow_ValueIndex = new ValueIndexNode();

        ValueIndexNode m_RootCol_ValueIndex = new ValueIndexNode();
        

        /// <summary>
        /// 初始化固定值
        /// </summary>
        /// <param name="rItem"></param>
        /// <param name="parent"></param>
        private void InitFixedValue(ReportItem rItem, ValueIndexNode parent)
        {
            //创建索引
            if (parent.IsInit)
            {
                return;
            }

            foreach (ItemFixedValue fValue in rItem.FixedValues)
            {
                parent.AddChild(fValue.Value,fValue.Text);
            }

            parent.IsInit = true;

        }

        /// <summary>
        /// 获取实体的字段值
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dbField"></param>
        /// <returns></returns>
        private object GetFieldValue(object model,string dbField)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (string.IsNullOrEmpty(dbField)) throw new ArgumentNullException("dbField");

            object value = null;

            if (model is HWQ.Entity.LightModels.LModel)
            {
                value = ((HWQ.Entity.LightModels.LModel)model)[dbField];
            }
            else
            {
                value = ObjectUtil.GetPropertyValue(model, dbField);
            }

            return value;
        }

        /// <summary>
        /// 格式化显示
        /// </summary>
        /// <param name="col"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string FormatValue(ReportColumn col, object value)
        {
            if (string.IsNullOrEmpty(col.FormatType) || "DEFAULT" == col.FormatType)
            {
                return FormatValue(col.Format, value);
            }

            if (value == null)
            {
                return string.Empty;
            }


            if ("QUARTER" == col.FormatType)
            {
                DateTime date = (DateTime)value;

                int qu = QuarterUtil.GetIndex(date.Month) + 1;

                if (string.IsNullOrEmpty(col.Format))
                {
                    return string.Format($"{date.Year}年{qu}季度");
                }
                else
                {
                    return string.Format(col.Format, date, qu);
                }
            }
            else
            {
                // if("WEEK" == item.FormatType)
                return FormatValue(col.Format, value);
            }
        }

        /// <summary>
        /// 格式化显示
        /// </summary>
        /// <param name="item"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string FormatValue(ReportItem item, object value)
        {
            if(string.IsNullOrEmpty(item.FormatType) || "DEFAULT" == item.FormatType)
            {
                return FormatValue(item.Format, value);
            }

            if (value == null)
            {
                return string.Empty;
            }


            if ("QUARTER" == item.FormatType)
            {
                DateTime date = (DateTime)value;

                int qu = QuarterUtil.GetIndex(date.Month) + 1;

                if (string.IsNullOrEmpty(item.Format))
                {
                    return string.Format($"{date.Year}年{qu}季度");
                }
                else
                {
                    return string.Format(item.Format, date, qu);
                }
                //return string.Format($"{date.Year}年{qu}季度");
            }
            else
            {
                // if("WEEK" == item.FormatType)
                return FormatValue(item.Format, value);
            }
        }

        /// <summary>
        /// 格式化值
        /// </summary>
        /// <param name="format"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string FormatValue( string format,object value)
        {
            string valueStr;

            if (!string.IsNullOrEmpty(format))
            {
                valueStr = string.Format(format, value);
            }
            else
            {
                valueStr = (value == null) ? string.Empty : value.ToString();
            }

            return valueStr;
        }

        
        private void ProValueIndex_Fixed(ValueIndexNode parent,ReportItem rItem)
        {                    
            if (parent.IsInit)
            {
                return;
            }

            //创建索引
            foreach (ItemFixedValue fValue in rItem.FixedValues)
            {
                ValueIndexNode iNode = parent.AddChild(fValue.Value, fValue.Text);

                iNode.InnerDBField = fValue.Value;
                iNode.Width = fValue.Width;

                iNode.Code = fValue.Code;
                iNode.ValueMode = fValue.ValueMode;
                iNode.Format = fValue.Format;
                iNode.FormatType = fValue.FormatType;
            }

            parent.IsInit = true;
        }


        private ValueIndexNode ProValueIndex_Default(ValueIndexNode parent, ReportItem rItem,ReportRowType rowType,object model)
        {
            if (parent.OneChild && parent.HasChild())
            {
                return parent;
            }
            
            object value = GetFieldValue(model, rItem.DBField);

            string valueStr = FormatValue(rItem, value);

            ValueIndexNode newNode = null;

            if (rItem.ValueMode == RFieldValueMode.FixedValue)
            {
                InitFixedValue(rItem, parent);

                //没有固定的节点，就取消这条记录
                if (!parent.Exist(valueStr))
                {
                    return parent;
                }

                newNode = parent.GetChild(valueStr);
                newNode.Width = rItem.Width;

                newNode.Format = rItem.Format;
                newNode.FormatType = rItem.FormatType;
            }
            else if (rItem.ValueMode == RFieldValueMode.DBValue)
            {
                newNode = parent.AddChild(valueStr);

                newNode.OneChild = rItem.OneChild;
                newNode.Width = rItem.Width;

                
                newNode.Format =  rItem.Format;
                newNode.FormatType = rItem.FormatType;

                ReportItem ri = m_DataTags[0][0];
                newNode.ValueFormat = ri.Format;

            }

            return newNode;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="itemGroup"></param>
        /// <param name="rowType">报表行的数据类型</param>
        /// <param name="model"></param>
        private void ProValueIndex(ValueIndexNode parent, List<ReportItemGroup> itemGroup, object model)
        {


            foreach (ReportItemGroup group in itemGroup)
            {
                ReportItem rItem = group[0];


                if (rItem.ValueMode == RFieldValueMode.InnerCode)
                {
                    ProValueIndex_Fixed(parent, rItem);
                }
                else
                {
                    ProValueIndex_Default(parent, rItem, ReportRowType.Common, model);
                }

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="itemGroup"></param>
        /// <param name="rowType">报表行的数据类型</param>
        /// <param name="model"></param>
        private void ProValueIndex(ValueIndexNode parent,List<ReportItemGroup> itemGroup, ReportRowType rowType, object model)
        {
            ValueIndexNode curNode = parent;

            foreach (ReportItemGroup group in itemGroup)
            {
                ReportItem rItem = group[0];


                if (rItem.ValueMode == RFieldValueMode.InnerCode)
                {
                    ProValueIndex_Fixed(curNode, rItem);
                }
                else
                {
                    curNode = ProValueIndex_Default(curNode, rItem,rowType, model);
                }

            }
        }



        private void ProValueIndex2(ValueIndexNode parent, List<ReportItemGroup> itemGroup, object model)
        {
            ValueIndexNode curNode = parent;

            bool existIncludeCode = false;  //是否存在内部编码

            foreach (ReportItemGroup group in itemGroup)
            {
                ReportItem rItem = group[0];
                
                object value;

                if (rItem.ValueMode == RFieldValueMode.InnerCode)
                {
                    existIncludeCode = true;

                    ProValueIndex_Fixed(parent, rItem);

                }
                else
                {
                    if (curNode.OneChild && curNode.HasChild())
                    {

                        break;
                    }

                    value = GetFieldValue(model, rItem.DBField);

                    string valueStr = FormatValue(rItem, value);


                    if (existIncludeCode)
                    {
                        if(rItem.ValueMode == RFieldValueMode.FixedValue)
                        {

                        }
                        else if(rItem.ValueMode == RFieldValueMode.DBValue)
                        {

                            foreach (ValueIndexNode node in parent.Childs.Values)
                            {
                                ValueIndexNode tmpNode = node.AddChild(valueStr);

                                tmpNode.OneChild = rItem.OneChild;
                                tmpNode.Width = rItem.Width;
                            }

                        }

                    }
                    else
                    {
                        curNode = ProValueIndex_Default(curNode, rItem, ReportRowType.Common, model);
                    }

                }


                parent = curNode;


            }
        }

        /// <summary>
        /// 期初数据
        /// </summary>
        List<object> m_BBDataList;

        /// <summary>
        /// 分析数据，构造树节点
        /// </summary>
        /// <param name="dataList"></param>
        private IList ProDataSource(IList dataList)
        {
            //从第一层开始构造。

            if (dataList == null)
            {
                return null;
            }

            m_RootRow_ValueIndex = new ValueIndexNode();

            m_RootCol_ValueIndex = new ValueIndexNode();

            

            if (this.EnabledBeginningBalance && this.DateFrom != null)
            {
                string dateField = this.DateField;

                m_BBDataList = new List<object>();

                List<object> thremDataList = new List<object>();

                foreach (object m in dataList)
                {
                    if(m is LightModel)
                    {
                        LightModel model = (LightModel)m;

                        if (model.IsNull(dateField))
                        {
                            continue;
                        }
                    }


                    DateTime curDate = LightModel.GetFieldValue<DateTime>(m, dateField);

                    //产生行的树结构
                    ProValueIndex(m_RootRow_ValueIndex, m_RowTags, ReportRowType.Common, m);

                    if (curDate < this.DateFrom)
                    {
                        m_BBDataList.Add(m);
                    }
                    else
                    {
                        thremDataList.Add(m);


                        //产生列的树结构
                        ProValueIndex2(m_RootCol_ValueIndex, m_ColTags, m);
                    }
                }

                return thremDataList;
            }
            else
            {

                foreach (object m in dataList)
                {
                    //产生行的树结构
                    ProValueIndex(m_RootRow_ValueIndex, m_RowTags, ReportRowType.Common, m);

                    //产生列的树结构
                    ProValueIndex2(m_RootCol_ValueIndex, m_ColTags, m);
                }

                return dataList;
            }



        }



        /// <summary>
        /// 初始化数据区, 多数据项
        /// </summary>
        private void InitMoreDataItem()
        {
            if (this.DataGroupTags.Count <= 1)
            {
                return;
            }


            ReportItem rItem = new ReportItem();

            rItem.ValueMode = RFieldValueMode.InnerCode;
            rItem.EnabledTotal = false;

            foreach (var c in this.DataGroupTags)
            {
                ReportItem cItem = c[0];
                
                ItemFixedValue fv = new ItemFixedValue();
                fv.EnabledTotal = cItem.EnabledTotal;

                fv.Value = cItem.DBField;
                fv.Text = cItem.Title;
                fv.Code = cItem.Code;
                fv.ValueMode = cItem.ValueMode;
                fv.Width = cItem.Width;
                fv.Format = cItem.Format;
                fv.FormatType = cItem.FormatType;

                rItem.FixedValues.Add(fv);
            }

            if(m_DataLayout == ReportDataLayout.Top)
            {
                this.ColGroupTags.Add(rItem);
            }
            else if(m_DataLayout == ReportDataLayout.Bottom)
            {
                this.ColGroupTags.Insert(0, rItem);
            }


        }


        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="dataList"></param>
        public void SetDataSource(IList dataList)
        {
            if (dataList == null)
            {
                return;
            }

            InitMoreDataItem(); //初始化数据区, 多数据项


            m_DataList = ProDataSource(dataList);   //分析数据,构造树节点

            this.CreateTableHead();
            
            this.CreateTableRowHead();


            RBody body = m_Table.Body;



            foreach (object model in dataList)
            {
                //取行的节点
                CrossRowHeadTreeNode rowNode = GetNodeForLeaf(m_YTreeRoot, null, model) as CrossRowHeadTreeNode;

                if (rowNode == null) { continue; }
                

                foreach (ReportItemGroup dataGroupTag in this.DataGroupTags)
                {
                    ReportItem repItem = dataGroupTag[0];

                    //取列的节点
                    CrossHeadTreeNode node = GetNodeForLeaf(m_XTreeRoot, repItem, model);

                    if (node == null) { continue; }

                    if (rowNode.Data == null)
                    {
                        rowNode.Data = new RRow();
                    }

                    //填充表格
                    FillCellValue(node, rowNode.Data, repItem, model);

                    //if(repItem.ValueMode == RFieldValueMode.Code)
                    //{
                    //    //RCell cell = rowNode.Data.GetCell(node.X);
                    //    //cell.Value = 123.456;
                    //}
                    
                }


            }

            

            //处理 "期初" 数据
            if (this.EnabledBeginningBalance && m_BBDataList != null)
            {
                foreach (object model in m_BBDataList)
                {
                    //取行的节点
                    CrossRowHeadTreeNode rowNode = GetNodeForLeaf(m_YTreeRoot, null, model) as CrossRowHeadTreeNode;

                    if (rowNode == null) { continue; }

                    if (rowNode.Data == null)
                    {
                        rowNode.Data = new RRow();
                    }

                    RRow row = rowNode.Data;

                    foreach (CrossHeadTreeNode xNode in m_XTotal_BB_Nodes)
                    {

                        int x = xNode.X;

                        string field = xNode.Column.Name;

                        object valueObj = LightModel.GetFieldValue(model, field);

                        if (valueObj == null)
                        {
                            continue;
                        }

                        decimal srcValue = Convert.ToDecimal(row[x].Value);
                        decimal value = Convert.ToDecimal(valueObj);

                        row[x].Value = srcValue + value;
                    }
                }
            }




            //计算 x轴小计
            ForEach_XTreeNode(this.m_YTreeRoot);
            

            //计算 y轴小计
            ForEach_YTreeNode();

        }


        /// <summary>
        /// 计算 Y 轴的小计
        /// </summary>
        private void ForEach_YTreeNode()
        {
            RRow row = m_Table.Head[0];

            int colCount = row.Count;

            foreach (CrossRowHeadTreeNode node in m_RowTotalHands)
            {
                //CrossRowHeadTreeNode node2 = node.TotalNodes as CrossRowHeadTreeNode;

                if (node.Data == null)
                {
                    node.Data = new RRow();
                }



                RRow tarRow = node.Data;

                for (int x = 0; x < colCount; x++)
                {
                    decimal value = 0;

                    foreach (var tNode in node.TotalNodes)
                    {
                        CrossRowHeadTreeNode node2 = tNode as CrossRowHeadTreeNode;
                        value += GetRowTotalValue(node2, x);
                    }

                    tarRow[x].Value = value;
                }



                ProXScript(tarRow, m_XTreeRoot);




                //从二维表获取数据
                foreach (CrossHeadTreeNode treeNode in m_XTotalNodes)
                {
                    decimal subValue = 0;

                    if (treeNode.Column.ValueMode == RFieldValueMode.Code)
                    {
                        SModel sm = new SModel();

                        ProTotalSModel(sm, row, m_XTreeRoot);

                        string code = treeNode.Column.Code;

                        if (!code.Contains("return "))
                        {
                            code = treeNode.Column.Code = "return " + code + ";";
                        }

                        object resultValue = null;

                        ScriptInstance inst = ScriptFactory.Create(code);


                        try
                        {
                            resultValue = inst.Exec(sm);

                            if (TypeUtil.IsNumberType(resultValue.GetType()))
                            {
                                subValue = Convert.ToDecimal(resultValue);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("报表计算动态代码错误.", ex);
                        }
                    }
                    else
                    {
                        foreach (var tNode in treeNode.TotalNodes)
                        {
                            subValue += GetTotalValue(tNode, row);
                        }
                    }

                    RCell cell = row[treeNode.X];
                    if (cell.TreeNode == null)
                    {
                        cell.TreeNode = treeNode;
                    }

                    cell.Value = subValue;
                }


            }

            

            //激活 "期末" 
            if (this.EnabledEndingBalance)
            {
                int nX = 0;

                foreach (CrossHeadTreeNode handNode in m_XTotal_EB_Nodes)
                {
                    RCell xCell = row[m_XTotalNodes[nX].X];

                    RCell eeCell = row[nX++];

                    RCell ebCell = row[handNode.X];

                    try
                    {
                        ebCell.Value = Convert.ToDecimal(xCell.Value) + Convert.ToDecimal(eeCell.Value);
                    }
                    catch(Exception ex)
                    {
                        log.Error("期末错误", ex);
                    }
                }
            }

        }


        /// <summary>
        /// 处理脚本
        /// </summary>
        private void ProXScript(RRow row, CrossHeadTreeNode parent)
        {

            foreach (CrossHeadTreeNode treeNode in parent.Childs)
            {
                if (treeNode.HasChild())
                {
                    ProXScript(row, treeNode);

                    continue;
                }

                if(treeNode.Column.ValueMode != RFieldValueMode.Code)
                {
                    continue;
                }
                
                //if (treeNode is CrossRowHeadTreeNode)
                //{
                    //CrossRowHeadTreeNode rh = (CrossRowHeadTreeNode)treeNode;

                    CrossHeadTreeNodeCollection childs = treeNode.Owner.Childs;

                    SModel sm = new SModel();

                    foreach (var c in childs)
                    {
                        if (c.Column.ValueMode == RFieldValueMode.DBValue)
                        {
                            object value = row[c.X].Value;
                            sm[c.Column.Value] = value;
                        }
                    }


                    string code = treeNode.Column.Code;

                    if (!code.Contains("return "))
                    {
                        code = treeNode.Column.Code = "return " + code + ";";
                    }

                    object resultValue = null;

                    ScriptInstance inst = ScriptFactory.Create(code);


                    try
                    {
                        resultValue = inst.Exec(sm);

                        row[treeNode.X].Value = resultValue;
                    }
                    catch(Exception ex)
                    {
                        log.Error("报表计算动态代码错误.", ex);
                        row[treeNode.X].Value = null;
                    }

                //}
                //else
                //{
                //    log.Debug("xxxxxxxxxxx");
                //}

            }
        }


        private void ProTotalSModel(SModel sm, RRow row, CrossHeadTreeNode parent)
        {

            foreach (CrossHeadTreeNode treeNode in parent.Childs)
            {
                if (treeNode.HasChild())
                {
                    ProTotalSModel(sm,row, treeNode);

                    continue;
                }

                if(treeNode.NodeType == CrossHeadTreeNodeTypes.Total)
                {
                    continue;
                }

                if (treeNode.Column.ValueMode == RFieldValueMode.Code)
                {
                    continue;
                }

                CrossHeadTreeNode c = treeNode;

                ReportColumn col = treeNode.Column;

                decimal srcDec = 0;
                decimal valueDec = 0;

                if (col.ValueMode == RFieldValueMode.DBValue)
                {
                    object srcV = sm[col.Value];
                    object value = row[c.X].Value;
                            
                    if (value == null)
                    {
                        continue;
                    }

                    if (TypeUtil.IsNumberType(value.GetType()))
                    {
                        valueDec = Convert.ToDecimal(value);
                    }

                    if(srcV == null)
                    {
                        srcDec = 0;
                    }
                    else
                    {
                        srcDec = Convert.ToDecimal(srcV);
                    }

                    sm[col.Value] = srcDec + valueDec;
                }


            }
        }


        /// <summary>
        /// 计算 X 轴(横轴) 的小计.
        /// </summary>
        /// <param name="parent"></param>
        /// <remarks>
        /// 算法：沿着 Y 轴，往下循环计算没一行 RRow 里面的小计。
        /// </remarks>
        private void ForEach_XTreeNode(CrossRowHeadTreeNode parent)
        {
            foreach (CrossRowHeadTreeNode node in parent.Childs)
            {
                if (node.HasChild())
                {
                    ForEach_XTreeNode(node);

                    continue;
                }


                if (node.Data == null)
                {
                    node.Data = new RRow();
                }

                RRow row = node.Data;

                ProXScript(row, m_XTreeRoot);




                //从二维表获取数据
                foreach (CrossHeadTreeNode treeNode in m_XTotalNodes)
                {
                    decimal subValue = 0;

                    if (treeNode.Column.ValueMode == RFieldValueMode.Code)
                    {
                        SModel sm = new SModel();

                        ProTotalSModel(sm, row, m_XTreeRoot);

                        string code = treeNode.Column.Code;

                        if (!code.Contains("return "))
                        {
                            code = treeNode.Column.Code = "return " + code + ";";
                        }

                        object resultValue = null;

                        ScriptInstance inst = ScriptFactory.Create(code);


                        try
                        {
                            resultValue = inst.Exec(sm);

                            if (TypeUtil.IsNumberType(resultValue.GetType()))
                            {
                                subValue = Convert.ToDecimal(resultValue);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("报表计算动态代码错误.", ex);
                        }
                    }
                    else
                    {
                        foreach (var tNode in treeNode.TotalNodes)
                        {
                            subValue += GetTotalValue(tNode, row);
                        }
                    }

                    RCell cell = row[treeNode.X];
                    if (cell.TreeNode == null)
                    {
                        cell.TreeNode = treeNode;
                    }

                    cell.Value = subValue;
                }

                

                //激活 "期末" 
                if (this.EnabledEndingBalance)
                {
                    int nX = 0;

                    foreach (CrossHeadTreeNode handNode in m_XTotal_EB_Nodes)
                    {
                        RCell xCell = row[m_XTotalNodes[nX].X];

                        RCell eeCell = row[nX++];

                        RCell ebCell = row[handNode.X];

                        ebCell.Value = Convert.ToDecimal(xCell.Value) +  Convert.ToDecimal( eeCell.Value);
                    }
                }


            }
        }


                
        /// <summary>
        /// 获取节点,包括子节点的全部值合计
        /// </summary>
        /// <param name="node"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private decimal GetRowTotalValue(CrossRowHeadTreeNode node, int x)
        {
            decimal v = 0;

            if (node.NodeType == CrossHeadTreeNodeTypes.Value)
            {

                if (node.Data == null)
                {
                    node.Data = new RRow();
                }

                if (x >= 0)
                {
                    RRow row = node.Data;

                    RCell cell = row[x];
                    if (cell.TreeNode == null)
                    {
                        cell.TreeNode = node;
                    }

                    if (!node.HasChild())
                    {
                        v += Convert.ToDecimal(cell.Value);
                    }
                }
            }

            v += GetRowChildTotalValue(node, x);
            

            return v;
        }


        private decimal GetRowChildTotalValue(CrossRowHeadTreeNode parent, int x)
        {
            decimal v = 0;
            
            
            foreach (CrossRowHeadTreeNode node in parent.Childs)
            {
                if (node.NodeType != CrossHeadTreeNodeTypes.Value)
                {
                    continue;
                }

                if (node.Data == null)
                {
                    node.Data = new RRow();
                }

                RRow row = node.Data;

                RCell cell = row[x];
                if (cell.TreeNode == null)
                {
                    cell.TreeNode = node;
                }

                if (!node.HasChild())
                {
                    v += Convert.ToDecimal(cell.Value);
                }
                else
                {
                    v += GetRowChildTotalValue(node, x);
                }
            }

            return v;
        }


        /// <summary>
        /// 包括自身的
        /// </summary>
        /// <param name="node"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private decimal GetTotalValue(CrossHeadTreeNode node, RRow row)
        {
            decimal v = 0;

            if(node.NodeType == CrossHeadTreeNodeTypes.Value)
            {
                if (node.X >= 0)
                {
                    RCell cell = row[node.X];
                    if (cell.TreeNode == null)
                    {
                        cell.TreeNode = node;
                    }

                    if (!node.HasChild())
                    {
                        object cellV = cell.Value;

                        if (cellV != null && TypeUtil.IsNumberType(cellV.GetType()) )
                        {   
                            v += Convert.ToDecimal(cell.Value);
                        }
                    }
                }
            }

            v += GetChildTotalValue(node, row);

            return v;
        }

        /// <summary>
        /// 获取子节点的值
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private decimal GetChildTotalValue(CrossHeadTreeNode parent,RRow row)
        {
            if (parent.NodeType == CrossHeadTreeNodeTypes.Total)
            {
                return 0;
            }

            decimal v = 0;

            foreach (CrossHeadTreeNode node in parent.Childs)
            {
                RCell cell = row[node.X];
                
                if(cell.TreeNode == null)
                {
                    cell.TreeNode = node;
                } 

                if (!node.HasChild())
                {
                    if (node.NodeType == CrossHeadTreeNodeTypes.Value && cell.Value != null)
                    {
                        try
                        {
                            v += Convert.ToDecimal(cell.Value);
                        }
                        catch(Exception ex)
                        {
                            log.Error($"报表单元格转换->数值错误. value={cell.Value}");
                        }
                    }
                }
                else
                {
                    v += GetChildTotalValue(node, row);
                }
            }

            return v;
        }



        /// <summary>
        /// 转换为表格模式
        /// </summary>
        /// <returns></returns>
        public RTable ToRTable()
        {
            int offsetX = m_RowTags.Count;

            RTable table = new RTable();
            table.RowHeaderCount = m_RowTags.Count;

            m_Table.Head.CopyTo(table.Head, offsetX, 0);


            //数 转-> 表格头
            TreeToRowHand(m_YTreeRoot, table.Body, null,null, 0);

            //树 转-> 为表格
            TreeToBodyData(m_YTreeRoot, table.Body,0 , m_RowTags.Count);

            //合并单元格，两行代码顺序不能颠倒(先合并 row, 后合并 col)
            ProRowSpan(table.Body, m_RowTags.Count);
            ProColSpan(table.Body, m_RowTags.Count);


            RCell cell = table.Head[0][0];
            cell.RowSpan = m_RowTags.Count;
            cell.ColSpan = m_ColTags.Count;

            MergeCell(cell, table.Head);


            
            return table;
        }




        /// <summary>
        /// 合并单元格
        /// </summary>
        private void MergeCell(RCell cell,RRowCollection rows)
        {
            int x1 = cell.X;
            int x2 = cell.X + cell.ColSpan;

            int y1 = cell.Y;
            int y2 = cell.Y + cell.RowSpan;

            for (int x = x1; x < x2; x++)
            {
                for (int y = y1; y < y2; y++)
                {
                    if (x == x1 && y == y1)
                    {
                        continue;
                    }


                    RCell mCell = rows[y][x];
                    
                    mCell.IsMergeChild = true;
                    mCell.MergeOwnerX = cell.X;
                    mCell.MergeOwnerY = cell.Y;
                    mCell.Visible = false;
                }
            }
        }


        /// <summary>
        /// 数节点 转换-> 表格模式
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="rows"></param>
        /// <param name="rowStart"></param>
        /// <param name="xOffice"></param>
        /// <returns></returns>
        private int TreeToBodyData(CrossRowHeadTreeNode parent, RRowCollection rows, int rowStart,int xOffice)
        {
            int n = rowStart;
            int n2 = 0;

            string rowTagA ; 

            foreach (CrossRowHeadTreeNode node in parent.Childs)
            {
                if (node.Data == null)
                {
                    node.Data = new RRow();
                }

                if (node.HasChild())
                {
                    int count = TreeToBodyData(node, rows, n, xOffice);

                    n += count;
                    n2 += count;
                }
                else
                {
                    RRow srcRow = node.Data;

                    RRow tarRow = rows[n];

                    srcRow.CopyTo(tarRow, xOffice);
                    

                    n++;
                    n2++;
                }

            }

            return n2;
        }


        private void ProColSpan(RRowCollection rows, int colCount)
        {
            int colIndex = colCount - 1 ;

            for (int i = 0; i < rows.Count; i++)
            {
                RRow row = rows[i];
                RCell cell = row[colIndex];

                if (!cell.IsNullValue())
                {
                    continue;
                }

                RCell prvCell = null;
                int colSpan = 1;

                for (int j  = colIndex - 1; j  >= 0; j--)
                {
                    prvCell = row[j];

                    colSpan++;

                    if (!prvCell.IsNullValue())
                    {
                        break;
                    }

                }

                if (prvCell != null)
                {
                    prvCell.ColSpan = colSpan;

                    this.MergeCell(prvCell, rows);
                }

            }
        }

        private void ProRowSpan(RRowCollection rows, int colCount)
        {
            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rows.Count; j++)
                {
                    RRow row = rows[j];
                    RCell cell = row[i];

                    if (cell.RowSpan <= 1)
                    {
                        continue;
                    }

                    this.MergeCell(cell, rows);

                    j += cell.RowSpan - 1;
                }
            }
        }

        /// <summary>
        /// 拷贝行的固定值
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="rows"></param>
        /// <param name="rowHeads"></param>
        /// <param name="rowStart"></param>
        /// <returns></returns>
        private int TreeToRowHand(CrossRowHeadTreeNode parentNode, RRowCollection rows,
            List<CrossRowHeadTreeNode> rowHeads, List<CrossRowHeadTreeNode> rowTotals, int rowStart)
        {

            if (!parentNode.HasChild())
            {
                if (rowHeads != null && parentNode.NodeType == CrossHeadTreeNodeTypes.Value )
                {
                    rowHeads.Add(parentNode);
                }
                else if (rowTotals != null && parentNode.NodeType == CrossHeadTreeNodeTypes.Total)
                {
                    rowTotals.Add(parentNode);
                }

                return 1;
            }

            int rowIndex = rowStart;

            int rowCount2 = 0;

            foreach (CrossRowHeadTreeNode node in parentNode.Childs)
            {
                int x = node.GetDepth() - 1;
                int span = node.GetSpan();

                RRow row = rows[rowIndex];
                RCell cell = row[x];

                if (cell.TreeNode == null)
                {
                    cell.TreeNode = node;
                }

                cell.RowSpan = span;

                cell.Value = node.Column.Text;

                int rowCount = TreeToRowHand(node, rows, rowHeads, rowTotals, rowIndex);

                rowIndex += rowCount;
                rowCount2 += rowCount;
            }

            return rowCount2;
        }





        /// <summary>
        /// 获取行的节点
        /// </summary>
        /// <param name="treeRoot"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private CrossHeadTreeNode GetNodeForLeaf(CrossHeadTreeNode treeRoot, ReportItem repItem, object model)
        {
            if (!treeRoot.HasChild())
            {
                return treeRoot;
            }

            CrossHeadTreeNode leaf = null;
            object value;

            foreach (CrossHeadTreeNode node in treeRoot.Childs)
            {
                ReportColumn rCol = node.Column;
                

                if (rCol == null  || node.NodeType == CrossHeadTreeNodeTypes.Total)
                {
                    continue;
                }
                
                if(repItem != null && !string.IsNullOrEmpty( rCol.InnerDBField) &&  repItem.DBField != rCol.InnerDBField)
                {
                    continue;
                }


                if( string.IsNullOrEmpty(rCol.Name))
                {
                    if (!string.IsNullOrEmpty(rCol.InnerDBField))
                    {

                    }
                    else
                    {
                        continue;
                    }
                }

                
                value = GetFieldValue(model, StringUtil.NoBlank(rCol.InnerDBField, rCol.Name));

                bool isEquale = false;

                if (treeRoot.OneChild && treeRoot.Childs.Count >= 1)
                {
                    isEquale = true;
                }
                else
                {
                    if (rCol.FormatType == "DEFAULT"  && !string.IsNullOrEmpty(rCol.Format))
                    {
                        string valueStr = FormatValue(rCol,value);

                        isEquale = rCol.ProValue(valueStr);
                    }
                    else if(rCol.FormatType == "QUARTER")
                    {
                        string valueStr = FormatValue(rCol, value);

                        isEquale = rCol.ProValue(valueStr);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(rCol.InnerDBField))
                        {
                            if (repItem.DBField == rCol.InnerDBField)
                            {
                                isEquale = true;
                            }
                        }
                        else
                        {
                            isEquale = rCol.ProValue(value);
                        }
                    }
                }

                if (isEquale)
                {
                    CrossHeadTreeNode tNode = GetNodeForLeaf(node, repItem, model);
                    leaf = tNode;

                    break;
                }
            }

            return leaf;
        }

        private decimal FillCellValue_Sum(ReportItem rItem, RCell cell, object item,decimal srcValue)
        {

            object curValue = GetFieldValue(item, rItem.DBField);

            decimal curDecimal = 0;

            if (cell.Value == null)
            {
                curDecimal = Convert.ToDecimal( curValue);
            }
            else
            {
                try
                {
                    curDecimal = Convert.ToDecimal(curValue);
                }
                catch (Exception ex)
                {
                    log.Debug(string.Format("字段“{0}”无法转化为数值类型。", rItem.DBField), ex);
                }
            }

            return curDecimal;
        }

        /// <summary>
        /// 填充表格
        /// </summary>
        /// <param name="node"></param>
        /// <param name="row"></param>
        /// <param name="item"></param>
        private void FillCellValue(CrossHeadTreeNode node,RRow row,ReportItem repItem, object item)
        {            
            RCell cell = row.GetCell(node.X);

            if (cell.TreeNode == null)
            {
                cell.TreeNode = node;
            }

            decimal srcValue = 0;

            try
            {
                if (cell.Value != null)
                {
                    srcValue = Convert.ToDecimal(cell.Value);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("填充表格错误", ex);
            }

            if (repItem.ValueMode == RFieldValueMode.Code)
            {

            }
            else
            {
                switch (repItem.FunName)
                {
                    case FunTypes.COUNT:
                        {
                            srcValue++;
                            break;
                        }
                    case FunTypes.SUM:
                        {
                            srcValue += FillCellValue_Sum(repItem, cell, item, srcValue);
                            break;
                        }
                }

                cell.Value = srcValue;
            }
        }

        
        /// <summary>
        /// 创建 Table Hand 对象
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="parentNode"></param>
        /// <param name="table"></param>
        /// <param name="depth"></param>
        /// <param name="cellStart"></param>
        private int Tree2TableHand(RRowCollection rows, CrossHeadTreeNode parentNode, RTable table, int depth, int cellStart)
        {

            if (!parentNode.HasChild())
            {
                return 1;
            }

            int nextCellIndex = cellStart;

            int nextCellX = 0;

            foreach (CrossHeadTreeNode node in parentNode.Childs)
            {
                //int len = node.GetSpan();

                int childCount = Tree2TableHand(rows, node, table, depth + 1, nextCellIndex);

                try {
                    RCell cell = rows.GetCell(depth, nextCellIndex);
                    cell.ID = node.Column.Name;
                    cell.Value = node.Column.Text;
                    
                    cell.ColSpan = childCount;

                    cell.TreeNode = node;

                }
                catch(Exception ex)
                {
                    throw new Exception($"node 转为 cell 单元格错误.", ex);
                }

                


                nextCellIndex += childCount;
                nextCellX += childCount;

                node.X = nextCellIndex;

            }

            return nextCellX;
        }


        /// <summary>
        /// 数据区:多数据项的统计
        /// </summary>
        SortedList<string, List<CrossHeadTreeNode>> m_DataTotalNodes = new SortedList<string, List<CrossHeadTreeNode>>();

        /// <summary>
        /// 转换为树节点
        /// </summary>
        /// <param name="parentValueIndexNode"></param>
        /// <param name="treeNodeParent"></param>
        /// <param name="tags">组属性集合</param>
        /// <param name="totalTreeNodes">统计节点集合</param>
        /// <param name="colGroupIndex"></param>
        private void CreatHeadTreeNode2(ValueIndexNode parentValueIndexNode, CrossHeadTreeNode treeNodeParent,
             List<ReportItemGroup> tags, List<CrossHeadTreeNode> totalTreeNodes, int colGroupIndex)
        {
            ReportItemGroup itemGroup = tags[colGroupIndex];

            ReportItem rItem = itemGroup[0];

            IList<ValueIndexNode> kvList = null;

            if (rItem.ValueMode == RFieldValueMode.FixedValue || rItem.ValueMode == RFieldValueMode.InnerCode)
            {
                kvList = parentValueIndexNode.Childs.ToList;
            }
            else
            {
                kvList = parentValueIndexNode.Childs.Values;

                if(rItem.OrderType == ReportOrderTypes.DESC)
                {
                    List<ValueIndexNode> descTmp = new List<ValueIndexNode>(kvList);

                    descTmp.Reverse();

                    kvList = descTmp;
                }
            }


            
            foreach (ValueIndexNode kv in kvList)
            {
                CrossHeadTreeNode treeNode = new CrossRowHeadTreeNode();
                treeNodeParent.Childs.Add(treeNode);

                ReportColumn col = new ReportColumn(kv.Text);
                col.Name = rItem.DBField;
                col.Value = kv.Value;
                col.Operator = OperatorTypes.Equals;
                col.Depth = colGroupIndex;
                col.Format = kv.Format;
                col.ValueFormat = kv.ValueFormat;
                col.FormatType = kv.FormatType;
                col.Width = kv.Width;

                col.Code = kv.Code;
                col.ValueMode = kv.ValueMode;
                 




                treeNode.OneChild = rItem.OneChild;
                treeNode.Column = col;

                if (rItem.ValueMode == RFieldValueMode.FixedValue)
                {
                    foreach (ItemFixedValue item in rItem.FixedValues)
                    {
                        if (item.Value != kv.Value)
                        {
                            continue;
                        }

                        col.Operator = item.Operator;
                        col.Text = item.Text;
                    }
                }
                else if (rItem.ValueMode == RFieldValueMode.InnerCode)
                {
                    foreach (ItemFixedValue item in rItem.FixedValues)
                    {
                        if (item.Value != kv.Value)
                        {
                            continue;
                        }

                        col.Operator = item.Operator;
                        col.Text = item.Text;
                        col.InnerDBField = item.Value;
                        col.EnabledTotal = item.EnabledTotal;

                        break;
                    }

                    #region 处理合计的节点集合

                    List<CrossHeadTreeNode> dataTotals = null;


                    if (!m_DataTotalNodes.TryGetValue(col.InnerDBField, out dataTotals))
                    {
                        dataTotals = new List<CrossHeadTreeNode>();
                        m_DataTotalNodes.Add(col.InnerDBField, dataTotals);
                    }

                    dataTotals.Add(treeNode);

                    #endregion

                }


                //创建统计列
                if (rItem.EnabledTotal && colGroupIndex < m_ColTags.Count - 1)
                {
                    CrossHeadTreeNode totalNode = CreateTotalNode(treeNode, treeNodeParent, col.Text, colGroupIndex);

                    totalTreeNodes.Add(totalNode);

                }

                if (colGroupIndex + 1 < tags.Count)
                {
                    CreatHeadTreeNode2(kv, treeNode, tags, totalTreeNodes, colGroupIndex + 1);
                }
            }



        }

        /// <summary>
        /// 创建统计的树节点
        /// </summary>
        /// <param name="totalSubNode"></param>
        /// <param name="parent"></param>
        /// <param name="value"></param>
        /// <param name="colGroupIndex"></param>
        /// <returns></returns>
        private CrossRowHeadTreeNode CreateTotalNode(CrossHeadTreeNode totalSubNode, CrossHeadTreeNode parent, string value, int colGroupIndex)
        {
            CrossRowHeadTreeNode totalNode = new CrossRowHeadTreeNode();
            totalNode.NodeType = CrossHeadTreeNodeTypes.Total;
            totalNode.TotalNodes.Add(totalSubNode);

            parent.Childs.Add(totalNode);

            ReportColumn totalCol = new ReportColumn(value + " 汇总");
            totalCol.Name = "(TOTAL)";
            totalCol.Depth = colGroupIndex;

            totalNode.Column = totalCol;

            //totalNodes.Add(totalNode);

            return totalNode;
        }






        /// <summary>
        /// 创建表格行
        /// </summary>
        public void CreateTableRow()
        {

        }



    }
}
