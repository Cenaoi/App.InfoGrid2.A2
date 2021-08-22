using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model.XmlModel;

namespace App.InfoGrid2.View.OnePage
{
    public partial class AreaLinkStepEdit2 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);


            InitData();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.join_table1.ChangedCallback += Join_table1_ChangedCallback;

        }

        public void Join_table1_ChangedCallback(object sender, string data)
        {
            if(StringUtil.IsBlank(data))
            {
                return;
            }

            SModel sm = SModel.ParseJson(data);

            string joinTable = (string)sm["value"];

            ProInitJsonTable(joinTable);
        }

        IG2_TABLE m_PView = null;

        List<IG2_TABLE_COL> m_PViewCols = null;

        private void InitData()
        {
            int pageId = WebUtil.QueryInt("page_id");   //页面的 ID

            int viewId = WebUtil.QueryInt("view_Id");   //版块的 ID


            if (viewId <= 0)
            {
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            m_PView = decipher.SelectModelByPk<IG2_TABLE>(viewId);

            m_PViewCols = decipher.SelectModels<IG2_TABLE_COL>($"ROW_SID >= 0 and IG2_TABLE_ID = {viewId}");

            if (!this.IsPostBack)
            {
                tabName1.Value = m_PView.TAB_TEXT;
                joinVersion1.Value = m_PView.JOIN_VERSION.ToString();
                linkEnabledCB.Checked = m_PView.JOIN_ENABLED;

                InitTables();

                InitJoinFields();

                OnStore1_Refresh();
            }
        }


        private void InitTables()
        {

            string self_table = WebUtil.Query("self_table");

            string tables = WebUtil.Query("tables");

            string[] tableList = StringUtil.Split(tables, ",");


            tableList = ArrayUtil.Remove(self_table, tableList);

            


            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("SEC_LEVEL", 6, Logic.LessThan);
            filter.And("TABLE_TYPE_ID", "TABLE");
            filter.And("TABLE_NAME", tableList, Logic.In);

            filter.Fields = new string[] { "IG2_TABLE_ID", "TABLE_NAME", "DISPLAY" };


            DbDecipher decipher = ModelAction.OpenDecipher();
            var models = decipher.GetModelList(filter);

            foreach (var item in models)
            {
                string text = $"{item["TABLE_NAME"]} ({item["DISPLAY"]})";
                join_table1.Items.Add((string)item["TABLE_NAME"], text);
            }



        }






        /// <summary>
        /// 初始化关联字段名称
        /// </summary>
        private void InitJoinFields()
        {
            int viewId = WebUtil.QueryInt("view_Id");
            int pageId = WebUtil.QueryInt("page_id");

            string self_table = WebUtil.Query("self_table");

            if (viewId <= 0)
            {
                return;
            }

            string xmlStr = m_PView.JOIN_V2_CONFIG;

            TableJoinConfig cfg = null;

            if (!StringUtil.IsBlank(xmlStr))
            {
                cfg = XmlUtil.Deserialize<TableJoinConfig>(xmlStr);

                join_table1.Value = cfg.join_table;

                store2.AddRange(cfg.items);
            }


            //自己的
            SelectColumn cb = this.table1.Columns.FindByDataField("field") as SelectColumn;

            foreach (var item in m_PViewCols)
            {
                ListItem li = new ListItem(item.DB_FIELD, item.DISPLAY);
                li.TextEx = $"{item.DB_FIELD} ({item.DISPLAY})";

                cb.Items.Add(li);
            }


            //
            //自己的

            if (cfg != null && !StringUtil.IsBlank(cfg.join_table))
            {
                ProInitJsonTable(cfg.join_table);
            }
        }

        private void ProInitJsonTable(string jsonTable)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();
            var joinTable = decipher.SelectToOneModel<IG2_TABLE>($"ROW_SID >=0 and TABLE_TYPE_ID='TABLE' and TABLE_NAME='{jsonTable}'");

            var joinCols = decipher.SelectModels<IG2_TABLE_COL>(LOrder.By("FIELD_SEQ DESC"),
                $"ROW_SID >= 0 and IG2_TABLE_ID = {joinTable.IG2_TABLE_ID}");

            SelectColumn cb2 = this.table1.Columns.FindByDataField("join_field") as SelectColumn;

            cb2.Items.Clear();

            foreach (var item in joinCols)
            {
                ListItem li = new ListItem(item.DB_FIELD, item.DISPLAY);
                li.TextEx = $"{item.DB_FIELD} ({item.DISPLAY})";

                cb2.Items.Add(li);
            }


        }



        public void OnStore1_Add()
        {
            this.store2.Add(new join_item());
        }



        public void OnStore1_Refresh()
        {
            string xmlStr = m_PView.JOIN_V2_CONFIG;

            TableJoinConfig cfg = null;

            if (!StringUtil.IsBlank(xmlStr))
            {
                cfg = XmlUtil.Deserialize<TableJoinConfig>(xmlStr);
            }
            else
            {
                cfg = new TableJoinConfig();
            }

            this.store2.RemoveAll();
            this.store2.AddRange(cfg.items);
        }

        private void RemoveItems<ItemT>(List<ItemT> srcArray, int[] indexs)
        {
            Array.Sort<int>(indexs);

            for (int i = 0; i < indexs.Length; i++)
            {
                int newIndex = indexs[i] - i;

                srcArray.RemoveAt(newIndex);
            }

        }


        private TableJoinConfig GetJoinConfig()
        {
            DataBatch batch = store2.GetDataChangedBatch();

            TableJoinConfig cfg = new TableJoinConfig();

            cfg.join_table = this.join_table1.Value;

            foreach (var rect in batch.Records)
            {
                join_item ji = new join_item();
                ji.field = rect["field"]?.ToString();
                ji.field_text = rect["field_text"]?.ToString();
                ji.join_field = rect["join_field"]?.ToString();
                ji.join_field_text = rect["join_field_text"]?.ToString();

                cfg.items.Add(ji);
            }

            return cfg;
        }

        public void OnStore1_Delete()
        {

            DataRecordCollection rectList = this.table1.CheckedRows;

            string xmlStr = m_PView.JOIN_V2_CONFIG;

            TableJoinConfig cfg = GetJoinConfig();


            List<int> idxs = new List<int>();
            foreach (var rect in rectList)
            {
                idxs.Add(rect.Index);
            }

            RemoveItems(cfg.items, idxs.ToArray());



            string nextXmlStr = XmlUtil.Serialize(cfg, true);
            m_PView.JOIN_V2_CONFIG = nextXmlStr;

            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.UpdateModelProps(m_PView, "JOIN_V2_CONFIG");

            this.store2.RemoveAll();
            this.store2.AddRange(cfg.items);

            Toast.Show("删除成功.");
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void GoLast()
        {
            int version = int.Parse(this.joinVersion1.Value);

            TableJoinConfig cfg = GetJoinConfig();


            string nextXmlStr = XmlUtil.Serialize(cfg, true);
            m_PView.JOIN_V2_CONFIG = nextXmlStr;
            m_PView.JOIN_VERSION = version;
            m_PView.JOIN_ENABLED = this.linkEnabledCB.Checked;
            m_PView.TAB_TEXT = this.tabName1.Value;

            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.UpdateModelProps(m_PView, "TAB_TEXT", "JOIN_ENABLED","JOIN_VERSION", "JOIN_V2_CONFIG");

            Toast.Show("保存成功.");
        }

        /// <summary>
        /// 修改, 批量修复为 v2 版本
        /// </summary>
        public void GoRepairToV2()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            List<IG2_TABLE> tabs = decipher.SelectModels<IG2_TABLE>("ROW_SID >= 0 and JOIN_ENABLED = 1 and JOIN_V2_CONFIG = '' ");

            int n = 0;

            foreach (var tab in tabs)
            {
                try
                {
                    TableJoinConfig cfg = new TableJoinConfig();
                    cfg.join_table = tab.JOIN_TAB_NAME;
                    cfg.items.Add(new join_item()
                    {
                        field = tab.ME_COL_NAME,
                        join_field = tab.JOIN_COL_NAME
                    });

                    tab.JOIN_V2_CONFIG = XmlUtil.Serialize(cfg,true);

                    decipher.UpdateModelProps(tab, "JOIN_V2_CONFIG");

                    n++;
                }
                catch (Exception ex)
                {
                    log.Error($"更新关联连接错误.IG2_TABLE_ID = {tab.IG2_TABLE_ID}", ex);
                    MessageBox.Alert("更新过程错误.");
                }
            }
            

            Toast.Show($"更新了 {n} 条记录.");

        }

    }
}