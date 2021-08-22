using App.BizCommon;
using EasyClick.Web.Mini2;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Workflow
{


    /// <summary>
    /// 流程信息对象
    /// </summary>
    public class FlowInfo
    {
        /// <summary>
        /// 状态集合
        /// </summary>
        private List<int> m_states;

        
        /// <summary>
        /// 提示消息集合
        /// </summary>
        public string tooltipText { get; set; }

        /// <summary>
        /// 状态集合
        /// </summary>
        public List<int> states {
            get {

                if(m_states == null)
                {
                    m_states = new List<int>();
                }

                return m_states;

            }
            set
            {
                m_states = value;
            }
        }


    }


    public partial class FlowShow : System.Web.UI.Page
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 用户数据
        /// </summary>
        public string m_userData = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                InitData();

            }

        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitData()
        {

            //主表ID
            int id = WebUtil.QueryInt("id");


            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter lmFilter = new LightModelFilter("UT_238");
            lmFilter.And("ROW_SID",0, Logic.GreaterThanOrEqual);
            lmFilter.And("COL_1", id);


            List<LModel> lmList = decipher.GetModelList(lmFilter);

            if(lmList.Count == 0)
            {
                MessageBox.Alert("找不到数据了！");
                return;
            }


            Dictionary<int, FlowInfo> flowDic = new Dictionary<int, FlowInfo>();

            
            foreach(var lm in lmList)
            {

                GetFlowDic(lm, flowDic);

            }


            GetSubItem(flowDic);


        }


        /// <summary>
        /// 把一个节点的多条记录和在一起
        /// </summary>
        /// <param name="lm">数据对象</param>
        /// <param name="flowDic">键值对</param>
        void GetFlowDic(LModel lm, Dictionary<int, FlowInfo> flowDic)
        {
            int nodeId = lm.Get<int>("COL_2");

            //单号
            string col_8 = lm.Get<string>("COL_8");

            //产品编码
            string col_4 = lm.Get<string>("COL_4");

            //产品名称
            string col_5 = lm.Get<string>("COL_5");

            string col_9 = lm.Get<decimal>("COL_9").ToString("0.########");
            string col_10 = lm.Get<decimal>("COL_10").ToString("0.########");
            string col_11 = lm.Get<decimal>("COL_11").ToString("0.########");

            string tooltipText = $"{col_8}  {col_4}   {col_5}  计划量:{col_9}   完成量:{col_10}    剩余量:{col_11}";

            int COL_12 = lm.Get<int>("COL_12");


            if (flowDic.ContainsKey(nodeId))
            {
                FlowInfo fi = flowDic[nodeId];

                fi.tooltipText += "\n" + tooltipText;

                fi.states.Add(COL_12);
                return;
            }


            FlowInfo fi2 = new FlowInfo();
            fi2.states.Add(COL_12);
            fi2.tooltipText = tooltipText;

            flowDic.Add(nodeId,fi2);


        }


        /// <summary>
        /// 一个节点多个数据就获取一个状态值
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        int GetFlowState(FlowInfo fi)
        {

            foreach(int i in fi.states)
            {
                if(i == 1)
                {
                    return i;
                }
            }


            foreach( int i in fi.states)
            {

                if(i == 0)
                {
                    return i;
                }

            }


            
            foreach(int i in fi.states)
            {
                if(i == 2)
                {
                    return i;
                }
            }


            foreach(int i in fi.states)
            {
                if(i == 99)
                {
                    return i;
                }
            }


            return 0;


        }



        /// <summary>
        /// 获取子节点的信息
        /// </summary>
        /// <param name="flowDic">每个节点只能有一个键的</param>
        /// <returns></returns>
        void GetSubItem(Dictionary<int, FlowInfo> flowDic)
        {


            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            int i = 0;

            foreach (var item in flowDic)
            {
                if (i++ > 0)
                {
                    sb.Append(",");
                }

                string text = string.Empty;

                switch (item.Key)
                {
                    case 1:
                        text = "下订单";
                        break;
                    case 2:
                        text = "下生产单";
                        break;
                    case 3:
                        text = "下采购单";
                        break;
                    case 4:
                        text = "采购到货";
                        break;
                    case 5:
                        text = "车间生产";
                        break;
                    case 6:
                        text = "生产入库";
                        break;
                    case 7:
                        text = "销售出货";
                        break;
                    case 8:
                        text = "下外发单";
                        break;
                    case 9:
                        text = "外发收货";
                        break;
                    case 10:
                        text = "采购需求";
                        break;
                    case 11:
                        text = "外发需求";
                        break;

                }

                string tooltipText = item.Value.tooltipText;

                int col_12 = GetFlowState(item.Value);


               string json =  $"{{text: \"{text}\",tooltipText: \"{tooltipText}\",state: {col_12}}}";


                sb.Append(json);


            }

            sb.Append("]");


            m_userData = sb.ToString().Replace("\n", "\\n");

        }

    }


   

}