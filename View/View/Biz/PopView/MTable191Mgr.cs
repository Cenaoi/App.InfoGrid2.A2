using App.BizCommon;
using App.InfoGrid2.Bll;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;

namespace App.InfoGrid2.View.Biz.PopView
{
    /// <summary>
    /// 自定义树结构节点
    /// </summary>
    public class UserTreeNode
    {

        UserTreeNodeCollection m_Childs;

        public UserTreeNodeCollection Childs
        {
            get
            {
                if (m_Childs == null)
                {
                    m_Childs = new UserTreeNodeCollection(this);
                }
                return m_Childs;
            }
        }

        public UserTreeNode()
        {
        }

        public UserTreeNode(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// 是否有子节点
        /// </summary>
        /// <returns></returns>
        public bool HasChild()
        {
            return (m_Childs != null && m_Childs.Count > 0);
        }


        /// <summary>
        /// 节点名称
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 获取所有子节点包括孙节点集合
        /// </summary>
        /// <returns></returns>
        public List<UserTreeNode> GetChildAll()
        {
            List<UserTreeNode> nodes = new List<UserTreeNode>();

            _ChildsAll(this, nodes);

            return nodes;
        }

        private void _ChildsAll(UserTreeNode node, List<UserTreeNode> nodes)
        {
            if (node.m_Childs == null)
            {
                return;
            }

            foreach (var item in node.m_Childs)
            {
                nodes.Add(item);

                _ChildsAll(item, nodes);
            }
        }
    }

    /// <summary>
    /// 自定义的树节点集合
    /// </summary>
    public class UserTreeNodeCollection : List<UserTreeNode>
    {
        UserTreeNode m_Owner;

        public UserTreeNodeCollection(UserTreeNode owner)
        {
            m_Owner = owner;
        }

        /// <summary>
        /// 所属的上级节点
        /// </summary>
        public UserTreeNode Owner
        {
            get { return m_Owner; }
        }
    }



    public class MTable191Mgr
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int m_MaxDept = 20;

        /// <summary>
        /// 目标表名
        /// </summary>
        public string TargetTable { get; set; }

        public int GetUT_195_ID(string code)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            //获取表头数据
            LightModelFilter lmFilter195 = new LightModelFilter("UT_195");
            lmFilter195.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter195.And("COL_1", code);
            lmFilter195.Fields = new string[] { "ROW_IDENTITY_ID" };

            int rowId = decipher.ExecuteScalar<int>(lmFilter195);

            return rowId;
        }

        /// <summary>
        /// 获取 UT_196 数据
        /// </summary>
        /// <param name="mapId">映射ID</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<LModel> GetModels196(int mapId,int id)
        {
            UserTreeNode srcRoot191 = new UserTreeNode("原根节点191");
            UserTreeNode tarRoot196 = new UserTreeNode("目标节点196");


            bool isSuccess = RecursiveSrcTree(id, srcRoot191, 0);    //获取原始表的树结构


            ChangeTree(mapId, srcRoot191, tarRoot196);


            //获取所有孙节点集合
            List<UserTreeNode> tarNodes = tarRoot196.GetChildAll();


            List<LModel> models = new List<LModel>();

            //插入数据库
            foreach (var node in tarNodes)
            {
                LModel ut196 = node.Data as LModel;

                models.Add(ut196);
            }

            return models;
        }

        /// <summary>
        /// 两个树转换数据
        /// </summary>
        /// <param name="mapId">两个表的映射id</param>
        /// <param name="srcParentNode191">原数据</param>
        /// <param name="tarParentNode196">新数据</param>
        /// <returns></returns>
        public void ChangeTree(int mapId, UserTreeNode srcParentNode191, UserTreeNode tarParentNode196)
        {
            if (!srcParentNode191.HasChild())
            {
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel tarModel = tarParentNode196.Data as LModel;  //上级对象

            //上级主键,如果是根节点，那么就是 0
            int tarRowId = (tarModel != null ? tarModel.Get<int>("ROW_IDENTITY_ID") : 0);

            foreach (UserTreeNode srcNode in srcParentNode191.Childs)
            {
                LModel m196 = new LModel(TargetTable);

                //拷贝数据

                MapMgr.MapData(mapId, (LModel)srcNode.Data, m196);

                //直接获取记录的主键
                int newId = decipher.IdentityFactory.GetNewIdentity(TargetTable);

                m196["ROW_IDENTITY_ID"] = newId;

                //上级ID
                m196["BIZ_PARENT_ID"] = tarRowId;


                UserTreeNode tarNode = new UserTreeNode();
                tarNode.Data = m196;

                tarParentNode196.Childs.Add(tarNode);

                ChangeTree(mapId,srcNode, tarNode);
            }

            
        }

        /// <summary>
        /// UT_191 转换为 UT_196
        /// </summary>
        /// <param name="ut191"></param>
        /// <param name="ut196"></param>
        private void UT191_To_UT196( LModel ut191, LModel ut196)
        {


        }


        /// <summary>
        /// 递归获取树
        /// </summary>
        /// <param name="parentID">上级ID</param>
        /// <param name="pNode">上级树节点</param>
        /// <param name="dept">深度</param>
        /// <returns></returns>
        public bool RecursiveSrcTree(int parentID, UserTreeNode pNode, int dept)
        {
            if (dept > m_MaxDept)
            {
                log.Error("产品构建出现嵌套死循环。产品ID：" + parentID);
                return false;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter("UT_191");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_20", parentID);


            //	UT_191	BOM表-构件明细表
            List<LModel> lmList191new = decipher.GetModelList(lmFilter);

            bool isSuccess = false;

            foreach (var item in lmList191new)
            {
                //把查到的数据插入到集合中去
                UserTreeNode node = new UserTreeNode();
                node.Data = item;

                pNode.Childs.Add(node);

                string col6 = item["COL_6"].ToString();
                int mainPk = GetUT_195_ID(col6);


                if (mainPk <= 0)
                {
                    continue;
                }


                //递归拿下面的子表数据
                isSuccess = RecursiveSrcTree(mainPk, node, ++dept);

                if (!isSuccess)
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 获取表数据
        /// </summary>
        /// <param name="mapId">映射规则ID</param>
        /// <param name="id">根节点ID</param>
        /// <param name="name">源数据表</param>
        /// <param name="parentCol">源数据表中存放父ID的字段</param>
        /// <returns></returns>
        public List<LModel> GetModelsByName(int mapId,int id,string name,string parentCol) 
        {
            UserTreeNode srcRoot191 = new UserTreeNode("原根节点");
            UserTreeNode tarRoot196 = new UserTreeNode("目标节点");


            bool isSuccess = RecursiveSrcTreeByName(id, srcRoot191, 0,name,parentCol);    //获取原始表的树结构

            ChangeTree(mapId, srcRoot191, tarRoot196);


            //获取所有孙节点集合
            List<UserTreeNode> tarNodes = tarRoot196.GetChildAll();


            List<LModel> models = new List<LModel>();

            //插入数据库
            foreach (var node in tarNodes)
            {
                LModel ut196 = node.Data as LModel;

                models.Add(ut196);
            }

            return models;

        }


        /// <summary>
        /// 递归获取树
        /// </summary>
        /// <param name="parentID">上级ID</param>
        /// <param name="pNode">上级树节点</param>
        /// <param name="dept">深度</param>
        /// <param name="name">源数据表名</param>
        /// <returns></returns>
        public bool RecursiveSrcTreeByName(int parentID, UserTreeNode pNode, int dept, string name, string parentCol)
        {
            if (dept > m_MaxDept)
            {
                log.Error("产品构建出现嵌套死循环。产品ID：" + parentID);
                return false;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(name);
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And(parentCol, parentID);


            //	UT_191	BOM表-构件明细表
            List<LModel> lmList191new = decipher.GetModelList(lmFilter);

            bool isSuccess = false;

            foreach (var item in lmList191new)
            {
                //把查到的数据插入到集合中去
                UserTreeNode node = new UserTreeNode();
                node.Data = item;

                pNode.Childs.Add(node);


                //递归拿下面的子表数据
                isSuccess = RecursiveSrcTreeByName(item.Get<int>("ROW_IDENTITY_ID"), node, ++dept, name, parentCol);

                if (!isSuccess)
                {
                    return false;
                }
            }

            return true;
        }


    }
}