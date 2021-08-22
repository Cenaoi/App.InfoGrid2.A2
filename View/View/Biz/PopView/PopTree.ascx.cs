using App.BizCommon;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.Biz.PopView
{
    public partial class PopTree :  WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 树结构，允许最大显示的嵌套层次
        /// </summary>
        int m_MaxDept =20;

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            m_MaxDept = GlobelParam.GetValue<int>("PROD_META_DEPT",20,"产品构建的最大层次");

            if (!this.IsPostBack)
            {
                InitData();
            }
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData() 
        {
            int id = WebUtil.QueryInt("id");
            DbDecipher decipher = ModelAction.OpenDecipher();

            //	UT_195	BOM主表-表头
            LModel lm195 = decipher.GetModelByPk("UT_195", id);

            if (lm195 == null) 
            {
                Response.Redirect("Error.html");
                return;
            }

            //	UT_191	BOM表-构件明细表
            List<LModel> lmList191 = new List<LModel>();

            //递归获取数据
            RecursiveData(id, lmList191,0);


            TreeNode tNode = new TreeNode(lm195["COL_2"] +" " +lm195["COL_3"], id);
            tNode.ParentId = "0";
            tNode.NodeType = "default";
            tNode.Tag = "MAIN|" + id;
            tNode.StatusID = 0;
            tNode.Expand();
            this.TreePanel1.Add(tNode);

            //循环添加每个节点
            foreach (var item in lmList191) 
            {
                //节点名称
                string nodeName = string.Format("<b>{0}</b>  {1}  使用量：{2}  损耗率%：{3}",
                    item["COL_7"], item["COL_8"], item.Get<decimal>("COL_11").ToString("0.######"), item["COL_12"]);

                string col1 = item["COL_1"].ToString();      //id
                int col20 = item.Get<int>("COL_20");    //父节点

                if (col1 == "0")
                {
                    col1 += "_" + item["ROW_IDENTITY_ID"].ToString();
                }

                
                TreeNode subNode = new TreeNode(nodeName, col1 );
                subNode.ParentId = col20.ToString();
                subNode.NodeType = "default";
                subNode.Tag = "MAIN|" + id;
                subNode.StatusID = 0;
                subNode.Expand();
                this.TreePanel1.Add(subNode);

            }

        }

        /// <summary>
        /// 递归获取数据
        /// </summary>
        /// 
        bool RecursiveData(int id, List<LModel> lmList191,int dept) 
        {
            if (dept > m_MaxDept)
            {
                log.Error("产品构建出现嵌套死循环。产品ID：" + id);
                return false;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilter = new LightModelFilter("UT_191");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_20",id);


            //	UT_191	BOM表-构件明细表
            List<LModel> lmList191new = decipher.GetModelList(lmFilter);

            bool isSuccess = false;

            foreach (var item in lmList191new) 
            {
                //把查到的数据插入到集合中去
                lmList191.Add(item);

                //获取表头数据
                LightModelFilter lmFilter195 = new LightModelFilter("UT_195");
                lmFilter195.And("ROW_SID", 0,Logic.GreaterThanOrEqual);
                lmFilter195.And("COL_1", item["COL_6"]);
                lmFilter195.Fields = new string[]{"ROW_IDENTITY_ID"};

                int rowId = decipher.ExecuteScalar<int>(lmFilter195);

                //把产品ID相同的主表ID放在自身属性COL_1里面
                item["COL_1"] = rowId;

                if (rowId <= 0)
                {
                    continue;
                }


                //递归拿下面的子表数据
                isSuccess = RecursiveData(rowId, lmList191, ++dept);

                if (!isSuccess)
                {
                    return false;
                }
            }

            return true;
        }



    }
}