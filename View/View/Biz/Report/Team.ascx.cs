using App.BizCommon;
using App.InfoGrid2.Bll;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.Biz.Report
{
    public partial class Team : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        protected override void OnInitCustomControls(EventArgs e)
        {
            this.store1.PageLoading += Store1_PageLoading;

        }

        private void Store1_PageLoading(object sender, CancelPageEventArags e)
        {
            e.Cancel = true;


            string tableName = this.bufferTableHid.Value;

            if (string.IsNullOrEmpty(tableName))
            {
                log.Error($"缓冲表 '{tableName}'不存在 . ");
                return;
            }


            BufferTableHelper.PageLoading(store1, tableName, e.Page, e.TSqlSort);



        }


        protected void Page_Load(object sender, EventArgs e)
        {
            InitData();
        }

        /// <summary>
        /// 树型节点
        /// </summary>
        class TreeNode
        {

            /// <summary>
            /// 上级节点数据
            /// </summary>
            public TreeNode ParentNode { get; set; }

            /// <summary>
            /// 数据
            /// </summary>
            public SModel Node { get; set; }

            /// <summary>
            /// 子节点数量
            /// </summary>
            public int Num { get; set; }

            /// <summary>
            /// 节点名称
            /// </summary>
            public string NodeText { get; set; }

            /// <summary>
            /// 节点key或者唯一标识
            /// </summary>
            public string NodeKey { get; set; }

            /// <summary>
            /// 节点类型
            /// </summary>
            public string NodeType { get; set; }


            /// <summary>
            /// 子节点集合
            /// </summary>
            public List<TreeNode> ChildNodes { get; set; }


            /// <summary>
            /// 检查是否已经存在这个key的子节点了
            /// </summary>
            /// <param name="key">唯一标识</param>
            /// <returns></returns>
            public bool HasChildNode(string key)
            {

                if (ChildNodes == null)
                {
                    return false;
                }

                foreach (var item in ChildNodes)
                {
                    if (item.NodeKey == key)
                    {
                        return true;
                    }

                }

                return false;
            }

            /// <summary>
            /// 根据key获取到对应的节点
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public TreeNode GetNodeByKey(string key)
            {

                foreach (var item in ChildNodes)
                {
                    if (item.NodeKey == key)
                    {
                        return item;
                    }


                }

                return null;
            }



        }




        void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            SModelList sms = decipher.GetSModelList("select * from UT_404 where ROW_SID >= 0 and COL_1 > 0"); 

            TreeNode root1 = new TreeNode()
            {

                NodeText = "第一棵树",
                Num = 1
            };


            CreateTree1(root1, sms);
            UpdataRowNum(root1);

            TreeNode root2 = new TreeNode()
            {

                NodeText = "第二颗树",
                Num = 1

            };

            CreateTree2(root2, sms);
            UpdataRowNum(root2);

            TreeNode root3 = new TreeNode()
            {
                NodeText = "第三棵树",
                Num = 1
            };

            CreateTree3(root3, sms);
            UpdataRowNum(root3);


            DataTable dt = CreateTable();

            int maxNum = 0;

            foreach (var item in root1.ChildNodes)
            {

               var item2 = root2.ChildNodes.Find(r => r.NodeKey == item.NodeKey);

               var item3 = root3.ChildNodes.Find(r => r.NodeKey == item.NodeKey);


                maxNum = Math.Max(item.Num, item2.Num);
                maxNum = Math.Max(maxNum, item3.Num);

                item2.Num = item3.Num = item.Num = maxNum;

                for (int i = 0; i < maxNum; i++)
                {


                    DataRow row = dt.NewRow();

                    SetDataRowValue(row, item.Node, "ROW_DATE_CREATE");
                    SetDataRowValue(row, item.Node, "COL_94");
                    SetDataRowValue(row, item.Node, "COL_41");
                    SetDataRowValue(row, item.Node, "COL_42");
                    SetDataRowValue(row, item.Node, "COL_43");
                    SetDataRowValue(row, item.Node, "COL_9");
                    SetDataRowValue(row, item.Node, "COL_11");

                    dt.Rows.Add(row);

                }


                //这是统计行
                DataRow rowTotal = dt.NewRow();

                rowTotal["COL_40"] = "合计";

                dt.Rows.Add(rowTotal);
                

            }

            SetData(root1, dt, 0);

            SetData(root2, dt, 0);


            SetData(root3, dt, 0);



            decipher = DbDecipherManager.GetDecipherOpen("ReportBuffer");

            try
            {
                LModelElement modelElem = BufferTableHelper.CreateReportBuffer(decipher, dt);

                this.bufferTableHid.Value = modelElem.DBTableName;

                BufferTableHelper.FillReportBuffer(decipher, dt, modelElem.DBTableName);


                this.store1.DataBind();

            }
            catch (Exception ex)
            {
                log.Error("创建统计数据临时表出错了！", ex);
                throw ex;
            }
            finally
            {
                decipher.Dispose();
            }


        }

        /// <summary>
        /// 更新行数量，以最大的行数量为主
        /// </summary>
        /// <param name="root">树节点</param>
        /// <returns></returns>
        int UpdataRowNum(TreeNode root)
        {
            int rowNum = 0;

            foreach (var item in root.ChildNodes)
            {

                int childRowNum = UpdataRowNum(item);

                item.Num = Math.Max(item.Num, childRowNum);

                rowNum += item.Num;

            }
            return rowNum;

        }

        /// <summary>
        /// 创建第一颗树
        /// </summary>
        /// <param name="root">树根节点</param>
        /// <param name="sms">数据集合</param>
        void CreateTree1(TreeNode root,SModelList sms)
        {

            AddChildNode(sms, root,"COL_1", "项目ID","801");

            foreach (var item in root.ChildNodes)
            {
                AddChildNode(sms, item, "COL_56", "开团订单ID","802");

                foreach (var sub_item in item.ChildNodes)
                {
                    AddChildNode(sms, sub_item, "COL_90", "发票记录ID","156");
       
                }

            }        
        }

        /// <summary>
        /// 创建第二颗树
        /// </summary>
        /// <param name="root">树根节点</param>
        /// <param name="sms">数据集合</param>
        void CreateTree2(TreeNode root, SModelList sms)
        {


            AddChildNode(sms, root, "COL_1", "项目ID", "801");

            foreach (var item in root.ChildNodes)
            {

                AddChildNode(sms, item, "COL_56", "开团订单ID", "802");

                foreach (var sub_item in item.ChildNodes)
                {

                    AddChildNode(sms, sub_item, "COL_91", "发票记录ID", "124");

                }

            }

        }

        /// <summary>
        /// 创建第三颗树
        /// </summary>
        /// <param name="root">树根节点</param>
        /// <param name="sms">数据集合</param>
        void CreateTree3(TreeNode root, SModelList sms)
        {
           
            AddChildNode(sms, root, "COL_1", "项目ID", "801");

            foreach (var item in root.ChildNodes)
            {
                AddChildNode(sms, item, "COL_92", "开团订单ID", "143");

                foreach (var sub_item in item.ChildNodes)
                {

                    AddChildNode(sms, sub_item, "COL_93", "发票记录ID", "126");

                }
            }


        }

        /// <summary>
        /// 创建内存表
        /// </summary>
        /// <returns></returns>
        DataTable CreateTable()
        {

            DataTable dt = new DataTable("NEW_TABLE");

            dt.Columns.Add("ROW_IDENTITY_ID", typeof(int));
            dt.Columns.Add("ROW_DATE_CREATE", typeof(DateTime));
            dt.Columns.Add("COL_94", typeof(int));
            dt.Columns.Add("COL_1", typeof(int));
            dt.Columns.Add("COL_45", typeof(string));
            dt.Columns.Add("COL_7", typeof(string));
            dt.Columns.Add("COL_8", typeof(string));
            dt.Columns.Add("COL_9", typeof(string));
            dt.Columns.Add("COL_11", typeof(string));
            dt.Columns.Add("COL_56", typeof(int));
            dt.Columns.Add("COL_10", typeof(string));
            dt.Columns.Add("COL_87", typeof(string));
            dt.Columns.Add("COL_107", typeof(string));
            dt.Columns.Add("COL_14", typeof(string));
            dt.Columns.Add("COL_15", typeof(string));
            dt.Columns.Add("COL_16", typeof(int));

            dt.Columns.Add("COL_17", typeof(decimal));
            dt.Columns.Add("COL_18", typeof(decimal));
            dt.Columns.Add("COL_19", typeof(decimal));
            dt.Columns.Add("COL_20", typeof(decimal));
            dt.Columns.Add("COL_21", typeof(decimal));
            dt.Columns.Add("COL_22", typeof(decimal));
            dt.Columns.Add("COL_23", typeof(decimal));
            dt.Columns.Add("COL_24", typeof(decimal));
            dt.Columns.Add("COL_25", typeof(decimal));
            dt.Columns.Add("COL_27", typeof(decimal));
            dt.Columns.Add("COL_31", typeof(decimal));
            dt.Columns.Add("COL_91", typeof(int));
            dt.Columns.Add("COL_26", typeof(decimal));
            dt.Columns.Add("COL_28", typeof(DateTime));

            dt.Columns.Add("COL_29", typeof(string));

            BufferDataColumn bdc = new BufferDataColumn
            {
                IsRemark = true,
                ColumnName = "COL_44",
                DataType = typeof(string)
            };

            dt.Columns.Add(bdc);

            dt.Columns.Add("COL_90", typeof(int));
            dt.Columns.Add("COL_12", typeof(string));
            dt.Columns.Add("COL_13", typeof(DateTime));
            dt.Columns.Add("COL_78", typeof(decimal));
            dt.Columns.Add("COL_106", typeof(decimal));

            BufferDataColumn bdc1 = new BufferDataColumn
            {
                IsRemark = true,
                ColumnName = "COL_79",
                DataType = typeof(string)
            };

            dt.Columns.Add(bdc1);

            dt.Columns.Add("COL_92", typeof(int));
            dt.Columns.Add("COL_32", typeof(string));
            dt.Columns.Add("COL_33", typeof(int));
            dt.Columns.Add("COL_34", typeof(decimal));
            dt.Columns.Add("COL_30", typeof(decimal));
            dt.Columns.Add("COL_35", typeof(decimal));
            dt.Columns.Add("COL_93", typeof(int));
            dt.Columns.Add("COL_36", typeof(string));
            dt.Columns.Add("COL_37", typeof(DateTime));
            dt.Columns.Add("COL_38", typeof(string));
            dt.Columns.Add("COL_124", typeof(string));

            dt.Columns.Add("COL_100", typeof(decimal));
            dt.Columns.Add("COL_88", typeof(decimal));
            dt.Columns.Add("COL_89", typeof(decimal));
            dt.Columns.Add("COL_39", typeof(string));

            BufferDataColumn bdc2 = new BufferDataColumn
            {
                IsRemark = true,
                ColumnName = "COL_40",
                DataType = typeof(string)
            };

            dt.Columns.Add(bdc2);

            dt.Columns.Add("COL_41", typeof(decimal));

            dt.Columns.Add("COL_42", typeof(decimal));

            dt.Columns.Add("COL_43", typeof(decimal));

            dt.Columns.Add("REP_ROW_TYPE");

            return dt;

        }

        /// <summary>
        /// 每个子节点对应的字段集合
        /// </summary>
        Dictionary<string, string[]> _fields = new Dictionary<string, string[]>()
        {
            {"COL_1",new string[]{
                "COL_1", "ROW_IDENTITY_ID", "ROW_DATE_CREATE", "COL_94", "COL_45", "COL_7", "COL_8",
                "COL_9", "COL_11","COL_41","COL_42","COL_43"}},
            {"COL_56",new string[]{
                    "COL_56","COL_10","COL_87","COL_107","COL_14","COL_15",
                    "COL_16","COL_17","COL_18","COL_19","COL_20","COL_21",
                    "COL_22","COL_23","COL_24","COL_25","COL_27","COL_31" } },
            {"COL_91",new string[]{"COL_91","COL_26","COL_28","COL_29","COL_44" } },
            {"COL_90",new string[]{ "COL_90", "COL_12", "COL_13", "COL_78", "COL_106" , "COL_79" } },
            {"COL_92",new string[]{"COL_92","COL_32", "COL_33", "COL_34", "COL_30", "COL_35"}},
            {"COL_93",new string[]{ "COL_93", "COL_36", "COL_37", "COL_38", "COL_124", "COL_100", "COL_88", "COL_89", "COL_39" , "COL_40" } },
            {"TOTAL",new string[]{
                "COL_16", "COL_17", "COL_18", "COL_19", "COL_20", "COL_21", "COL_22", "COL_23",
                "COL_24","COL_25","COL_27","COL_31","COL_26","COL_78","COL_106","COL_33",
                "COL_30","COL_35","COL_88","COL_89","COL_42","COL_43","COL_41"
            } }
        };

        /// <summary>
        /// 把树结构里面的数据赋值给DataTable
        /// </summary>
        /// <param name="node">树节点</param>
        /// <param name="dt">DataTable</param>
        /// <param name="r_index">开始行索引</param>
        void SetData(TreeNode node, DataTable dt,int r_index)
        {
            foreach (var item in node.ChildNodes)
            {

                SetData(item, dt, r_index);

                if (string.IsNullOrWhiteSpace(item.NodeType))
                {
                    return;
                }

                string[] fields = _fields[item.NodeType];

                foreach (var field in fields)
                {
                    DataRow dr = dt.Rows[r_index];

                    SetDataRowValue(dr, item.Node, field);

                }

                r_index += item.Num;

                if(item.NodeType == "COL_1")
                {
                    DataRow dr = dt.Rows[r_index];

                    SetTotalRow(dt, dr, r_index - item.Num, r_index - 1);

                    r_index++;
                }

            }


        }

        /// <summary>
        /// 设置单元格的值，有些可能是空的值
        /// </summary>
        /// <param name="dr">内存行对象</param>
        /// <param name="sm">数据库数据对象</param>
        /// <param name="field">内存表字段名称</param>
        /// <param name="smField">数据库表字段名称 默认是和 内存表字段名称一样的</param>
        void SetDataRowValue(DataRow dr, SModel sm, string field,string smField = "")
        {

            if (string.IsNullOrWhiteSpace(smField))
            {
                smField = field;
            }

            if (sm.IsNull(smField))
            {

                dr[field] = DBNull.Value;

            }
            else
            {
                dr[field] = sm[smField];
            }

        }
        
        /// <summary>
        /// 设置合计行数据
        /// </summary>
        /// <param name="dt">内存表对象</param>
        /// <param name="dr">内存行对象</param>
        /// <param name="f_index">开始统计行索引</param>
        /// <param name="l_index">结束统计行索引</param>
        void SetTotalRow(DataTable dt,DataRow dr,int f_index,int l_index)
        {


            string[] fields = _fields["TOTAL"];

            foreach (var item in fields)
            {

                decimal total = 0;

                for (int i = f_index; i <= l_index; i++)
                {

                    DataRow dr1 = dt.Rows[i];

                    total += GetDec(dr1[item]);

                }

                dr[item] = total;
            }

        }

        /// <summary>
        /// 把内存表中的字段值转成数字类型
        /// </summary>
        /// <param name="obj">objecj类型值</param>
        /// <returns></returns>
        decimal GetDec(object obj)
        {

            try
            {
                return Convert.ToDecimal(obj);

            }
            catch (Exception ex)
            {
                log.Debug(ex);
                return 0;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="allSms"></param>
        /// <param name="p_node"></param>
        /// <param name="field"></param>
        /// <param name="field_text"></param>
        /// <param name="col_127"></param>
        void AddChildNode(SModelList allSms,TreeNode p_node, string field, string field_text,string col_127)
        {

            if (p_node.ChildNodes == null)
            {
                p_node.ChildNodes = new List<TreeNode>();
            }

            TreeNode node;


            SModelList sms = new SModelList();

            foreach(var item in allSms)
            {
                if (p_node.Node == null)
                {
                   
                    if(item.Get<string>("COL_127") == col_127)
                    {

                        sms.Add(item);
                    }

                    continue;

                }


                if(item.Get<string>("COL_127") == col_127 && item.Get<int>(p_node.NodeType) == p_node.Node[p_node.NodeType])
                {
                    
                    if(!item.IsNull(field) && item.Get<int>(field) > 0)
                    {
                        sms.Add(item);
                    }

                }

            }


            //这是是每个ID来排序
            sms.Sort((s1, s2) =>
            {

                if(s1.Get<int>(field) > s2.Get<int>(field))
                {
                    return 1;
                }
                else if(s1.Get<int>(field) == s2.Get<int>(field))
                {
                    return 0;
                }
                else
                {
                    return -1;
                }

            });

        

            foreach (var item in sms)
            {
                string value = string.Empty;

                if (!item.IsNull(field))
                {

                    value = item.Get<string>(field);

                }

                string key = field + "-" + value;


                node = p_node.GetNodeByKey(key);


                if (node == null)
                {

                    node = new TreeNode()
                    {

                        ParentNode = p_node,
                        ChildNodes = new List<TreeNode>(),
                        Node = item,
                        NodeKey = key,
                        NodeType = field,
                        NodeText = field_text

                };

                    p_node.ChildNodes.Add(node);

                }
              

                node.Num++;

            }

        }

    }
}