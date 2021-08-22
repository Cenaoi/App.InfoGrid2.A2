using EasyClick.Web.Mini;
using EC5.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Collections;
using EasyClick.Web.Mini2.Data;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 树数据
    /// </summary>
    public class TreeModel
    {
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 存在子节点
        /// </summary>
        public bool HasChild { get; set; }

        public TreeModel()
        {

        }

        public TreeModel(bool hasChild, object data)
        {
            this.HasChild = hasChild;
            this.Data = data;
        }


        public string ToJson()
        {
            string fieldJson = MiniConfiguration.JsonFactory.GetItemJson(this.Data);

            StringBuilder sb = new StringBuilder("{");

            sb.Append("\"role\": \"tree-model\"");
            sb.Append(",\"has_child\":" + (this.HasChild ? "true" : "false"));
            sb.Append(",\"data\": " + fieldJson);

            sb.Append("}");

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToJson();
        }
    }

    /// <summary>
    /// 树节点的删除方式
    /// </summary>
    public enum TreeRemoveMode
    {
        /// <summary>
        /// 只删除自己
        /// </summary>
        Oneself,
        /// <summary>
        /// 递归删除子节点
        /// </summary>
        Recursive,

        /// <summary>
        /// 禁止删除包含子节点的
        /// </summary>
        Pause
    }

    /// <summary>
    /// 树仓库
    /// </summary>
    public class TreeStore : Store
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 树仓库
        /// </summary>
        public TreeStore()
        {
            this.PageSize = 0;
            this.EngineName = "tree-default";
        }


        string m_TextField;

        /// <summary>
        /// 根节点ID
        /// </summary>
        string m_RootValue = "0";


        /// <summary>
        /// 显示根节点
        /// </summary>
        [Description("显示根节点"), DefaultValue(true)]
        public bool ShowRoot { get; set; } = true;

        /// <summary>
        /// 根节点描述, 
        /// </summary>
        /// <remarks>如果根节点找不到, 就采用这个名称</remarks>
        public string RootText { get; set; } = "类别目录";


        /// <summary>
        /// 删除节点的方式
        /// </summary>
        [Description("删除节点的方式")]
        [DefaultValue(TreeRemoveMode.Pause)]
        public TreeRemoveMode TreeRemoveMode { get; set; } = TreeRemoveMode.Pause;


        /// <summary>
        /// 显示的字段名
        /// </summary>
        public string TextField
        {
            get { return m_TextField; }
            set { m_TextField = value; }
        }

        /// <summary>
        /// 根节点的值
        /// </summary>
        public string RootValue
        {
            get { return m_RootValue; }
            set { m_RootValue = value; }
        }

        protected override void FullScript(StringBuilder sb)
        {

            sb.AppendLine("  var store = Mini2.create('Mini2.data.TreeStore', {");
            JsParam(sb, "storeId", this.ClientID);
            JsParam(sb, "id", this.ID);

            //JsParam(sb, "pageSize", this.PageSize);         //每页显示的数量
            //JsParam(sb, "totalCount", this.TotalCount);   //记录总量，分页用.

            JsParam(sb, "parentField", this.ParentField);

            JsParam(sb, "textField", this.TextField);
            JsParam(sb, "rootValue", this.RootValue);

            JsParam(sb, "rootText", this.RootText);

            JsParam(sb, "dymLoad", this.DymLoad, true);
            JsParam(sb, "dymHasChild", this.DymHasChild, true);
            JsParam(sb, "showRoot", this.ShowRoot, true);

            JsParam(sb, "sortText", this.SortText);    //默认排序字符串

            JsParam(sb, "autoSave", this.AutoSave, true);      //自动保存

            JsParam(sb, "idField", this.IdField);              //主键字段
            JsParam(sb, "lockedField", this.LockedField);      //锁字段

            JsParam(sb, "autoFocus", this.AutoFocus, true);    //自动触焦点行事件

            if (!StringUtil.IsBlank(this.LockedRule))
            {
                JsParam(sb, "lockedRule", this.LockedRule.Replace("'", "\'"));
            }

            JsParam(sb, "model", this.Model);

            JsParam(sb, "readOnly", this.ReadOnly, false);

            if (!StringUtil.IsBlank(this.StringFields))
            {
                string strFields = this.StringFields.Replace("\n", string.Empty)
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

                if (!string.IsNullOrEmpty(this.CustomData))
                {
                    sb.AppendLine();
                    sb.AppendFormat("    data:{0}, \n", this.CustomData);
                }
                else
                {
                    StringBuilder dataSb = new StringBuilder();

                    if (this.Data != null && this.Data.Count > 0)
                    {
                        string fieldJson;

                        for (int i = 0; i < this.Data.Count; i++)
                        {
                            object data = this.Data[i];

                            if (i > 0) { dataSb.AppendLine(",").Append("        "); };

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

            if (Has(StoreEvent.CurrentChanged))
            {
                sb.AppendLine("    currentChanged: function(){");

                sb.AppendLine("        widget1.subMethod('form:first',{");

                sb.AppendFormat("          subName:'{0}',", this.ID).AppendLine();
                sb.AppendFormat("          subMethod:'{0}'", "PreCurrentChanged").AppendLine();

                sb.AppendLine("        });");

                sb.AppendLine("    },");
            }

            if (this.SummaryItems != null && this.SummaryItems.Count > 0)
            {
                sb.AppendLine("        summary: {");

                for (int i = 0; i < this.SummaryItems.Count; i++)
                {
                    string field = this.SummaryItems.Keys[i];
                    object value = this.SummaryItems[field];

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
                }

                sb.AppendLine("        },");
            }

            sb.AppendLine("        isTreeStore:true");

            sb.AppendLine("  });");

            sb.AppendFormat("  window.{0} = store;\n", this.ClientID);


        }

        /// <summary>
        /// 动态加载过程中, 提示是否有子节点
        /// </summary>
        public bool DymHasChild { get; set; } = true;


        /// <summary>
        /// 处理是否有子节点
        /// </summary>
        private IList ProHasChilds(IList models)
        {

            if (models == null || models.Count == 0) { return null; }


            StoreEngine_OnInit();

            List<TreeModel> treeModels = new List<TreeModel>(models.Count);

            this.SelectQuery.Clear();

            foreach (var item in models)
            {
                bool hasChild = this.StoreEngine.HasChild(item);

                TreeModel tm = new TreeModel(true, item);
                tm.HasChild = hasChild;

                treeModels.Add(tm);
            }

            return treeModels;
        }


        public override IList LoadPage(int page)
        {

            //树形控件参数




            StoreEngine_OnInit();

            DataRequest dr = GetAction();

            string tSqlSort = StringUtil.NoBlank(dr.TSqlSort, this.SortText);

            bool pageCancel = OnPageLoading(page, tSqlSort);

            if (pageCancel) { return null; }


            bool cancel = OnFiltering(null, null);

            if (cancel) { return null; }

            //m_StoreEngine.PageSize = this.PageSize;
            //m_StoreEngine.CurPage = page;


            IList dataList = this.StoreEngine.LoadPage(page);

            //int itemTotal = m_StoreEngine.ItemTotal;



            OnPageLoaded(dataList);

            this.BeginLoadData();
            {
                this.RemoveAll();

                if (this.DymHasChild)
                {
                    dataList = ProHasChilds(dataList);
                }

                this.AddRange(dataList);


                //this.SetTotalCount(itemTotal);
                //this.SetCurrentPage(page);

                //if (m_AutoFocus && dataList.Count > 0)
                //{
                //    this.SetCurrent(0);

                //    if (m_AutoFocus && !Page.IsPostBack)
                //    {
                //        OnCurrentChanged(null, dataList[0]);
                //    }
                //}
                //else
                //{
                //    this.SetCurrent(this.CurIndex);

                //    if (m_AutoFocus && !Page.IsPostBack)
                //    {
                //        if (this.CurIndex >= 0 && this.CurIndex < dataList.Count)
                //        {
                //            OnCurrentChanged(null, dataList[this.CurIndex]);
                //        }
                //    }
                //}

            }
            this.EndLoadData();

            OnPageChanged();

            return dataList;
        }

        /// <summary>
        /// 将数据源绑定到被调用的服务器控件及其所有子控件。
        /// </summary>
        public override void DataBind()
        {
            if (StringUtil.IsBlank(this.IdField)) { throw new Exception($"必须指定主键字段名. IdFioeld 不能为空."); }
            if (StringUtil.IsBlank(this.ParentField)) { throw new Exception($"必须指定上级字段名. ParentField 不能为空."); }

            try
            {
                if (this.DymLoad)
                {
                    if (!this.Page.IsPostBack)
                    {
                        if (!StringUtil.IsBlank(this.RootValue))
                        {
                            string tSqlWhere = $"{this.IdField}='{this.RootValue}' or {this.ParentField}='{this.RootValue}'";

                            TSqlWhereParam param = new TSqlWhereParam(tSqlWhere);

                            this.SelectQuery.Add(param);
                        }
                    }
                    else
                    {
                        //this.SelectQuery.Add(this.ParentField, this.CurDataId);
                    }
                }

                IList models = this.LoadPage(0);
            }
            catch (Exception ex)
            {
                throw new Exception("绑定数据异常", ex);
            }
        }


        /// <summary>
        /// 加载子节点数据
        /// </summary>
        public void LoadChilds()
        {
            string parentId = this.CurDataId;

            if (StringUtil.IsBlank(parentId))
            {
                return;
            }

            this.SelectQuery.Add(this.ParentField, parentId);

            IList models = this.LoadPage(0);


        }

        public override bool OnPreInserting(object obj, ObjectEventItemCollection items)
        {
            bool result = base.OnPreInserting(obj, items);

            return result;
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <returns></returns>
        public override int Insert()
        {
            string parentId = this.CurDataId;

            if (StringUtil.IsBlank(parentId))
            {
                return 0;
            }

            this.InsertParams.Add(this.ParentField, parentId);

            return base.Insert();

        }

        /// <summary>
        /// 执行删除操作, 一般是删除当前节点
        /// </summary>
        /// <returns></returns>
        public override int Delete()
        {
            string curId = this.CurDataId;

            if (StringUtil.IsBlank(curId))
            {
                return 0;
            }

            if (this.TreeRemoveMode == TreeRemoveMode.Pause)
            {

                string tSqlWhere = $"{this.ParentField}='{curId}'";

                bool hasChild;

                try
                {
                    hasChild = this.StoreEngine.HasChild(tSqlWhere);
                }
                catch (Exception ex)
                {
                    throw new Exception("判断包含子节点错误." + tSqlWhere, ex);
                }

                if (hasChild)
                {
                    return 0;
                }

                this.DeleteQuery.Add(this.IdField, curId);

            }
            else if (this.TreeRemoveMode == TreeRemoveMode.Recursive)
            {
                throw new Exception("未实现递归删除的函数");
            }
            else if (this.TreeRemoveMode == TreeRemoveMode.Oneself)
            {
                this.DeleteQuery.Add(this.IdField, curId);
            }

            StoreEngine_OnInit();

            int count = this.StoreEngine.Delete();

            //this.Refresh();

            return count;


        }

        /// <summary>
        /// 重命名
        /// </summary>
        public bool Rename(string oldText, string text)
        {
            string curId = this.CurDataId;

            if (StringUtil.IsBlank(curId))
            {
                return false;
            }

            ITreeStoreEngine tse = this.StoreEngine as ITreeStoreEngine;

            bool result = tse.Rename(oldText, text);

            return result;

        }

    }
}
