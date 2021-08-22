using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using EasyClick.Web.Mini;
using EC5.SystemBoard;
using EC5.SystemBoard.Web;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using HWQ.Entity.WebExpand;
using EC5.Utility.Web;

namespace EasyClick.BizWeb.UI
{
    
    
    /// <summary>
    /// DataGridView 动作操作
    /// </summary>
    /// <typeparam name="ModelT"></typeparam>
    [Obsolete]
    public class DataGridViewAction<ModelT> where ModelT : class 
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// DataGridView 动作操作的构造方法
        /// </summary>
        /// <param name="grid"></param>
        public DataGridViewAction(DataGridView grid)
        {
            m_DataGridView = grid;
        }

        public DataGridViewAction()
        {

        }

        DbDecipher m_Decipher = null;

        #region 操作权限

        bool m_SecAdd = true;
        bool m_SecSave = true;
        bool m_SecDelete = true;
        bool m_SecSelect = true;

        bool m_ReadOnly = false;

        /// <summary>
        /// 权限：允许添加
        /// </summary>
        public bool SecAdd
        {
            get { return m_SecAdd; }
            set { m_SecAdd = value; }
        }

        /// <summary>
        /// 权限：允许保存
        /// </summary>
        public bool SecSave
        {
            get { return m_SecSave; }
            set { m_SecSave = value; }
        }

        /// <summary>
        /// 权限：允许删除
        /// </summary>
        public bool SecDelete
        {
            get { return m_SecDelete; }
            set { m_SecDelete = value; }
        }

        /// <summary>
        /// 权限：允许查询
        /// </summary>
        public bool SecSelect
        {
            get { return m_SecSelect; }
            set { m_SecSelect = value; }
        }

        private void HideCheckColumn()
        {
            for (int i = 0; i < 2; i++)
            {
                if (i >= m_DataGridView.Columns.Count)
                {
                    break;
                }

                if (m_DataGridView.Columns[i] is CheckBoxField)
                {
                    CheckBoxField check = (CheckBoxField)m_DataGridView.Columns[i];

                    if (check.HeaderMode == CheckBoxHeaderMode.SelectAll)
                    {
                        check.Visible = false;

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 只读
        /// </summary>
        public bool ReadOnly
        {
            set
            {
                m_ReadOnly = value;

                m_SecSave = !value;
                m_SecAdd = !value;
                m_SecDelete = !value;

                m_DataGridView.ReadOnly = value;

                if (value)
                {
                    HideCheckColumn();
                }
            }
            get
            {
                m_ReadOnly = (m_SecSave && m_SecAdd && m_SecDelete);

                return m_ReadOnly;
            }
        }

        #endregion

        /// <summary>
        /// 保存前验证
        /// </summary>
        bool m_ValidateSaveing = true;

        /// <summary>
        /// 保存前验证
        /// </summary>
        [DefaultValue(true)]
        public bool ValidateSaveing
        {
            get { return m_ValidateSaveing; }
            set { m_ValidateSaveing = value; }
        }

        #region 事件

        /// <summary>
        /// 添加过程的事件.
        /// </summary>
        public event EventHandler<ModelEventArgs<ModelT>> Adding;

        /// <summary>
        /// 触发添加过程的事件
        /// </summary>
        /// <param name="m"></param>
        protected void OnAdding(ModelT m)
        {
            if (Adding != null) Adding(this, new ModelEventArgs<ModelT>(m));
        }

        /// <summary>
        /// 添加过程的事件(可取消事件).
        /// </summary>
        public event ModelCancelEventHandler<ModelT> AddingCancel;

        /// <summary>
        /// 触发添加过程的事件(可取消事件).
        /// </summary>
        /// <param name="e"></param>
        protected void OnAddingCancel(ModelCancelEventArgs<ModelT> e)
        {
            if (AddingCancel != null) AddingCancel(this, e);
        }

        /// <summary>
        /// 添加实体结束的事件
        /// </summary>
        public event EventHandler<ModelEventArgs<ModelT>> Added;

        /// <summary>
        /// 添加实体结束触发的事件
        /// </summary>
        /// <param name="m"></param>
        protected void OnAdded(ModelT m)
        {
            if (Added != null) Added(this, new ModelEventArgs<ModelT>(m));
        }

        /// <summary>
        /// 保存过程的事件
        /// </summary>
        public event ModelCancelEventHandler<ModelT> Saving;

        /// <summary>
        /// 触发保存过程的事件
        /// </summary>
        /// <param name="e"></param>
        protected void OnSaving(ModelCancelEventArgs<ModelT> e)
        {
            if (Saving != null) Saving(this, e);
        }

        /// <summary>
        /// 保存结束的事件
        /// </summary>
        public event EventHandler<ModelEventArgs<ModelT>> Saved;

        /// <summary>
        /// 触发保存结束的事件
        /// </summary>
        /// <param name="m"></param>
        protected void OnSaved(ModelT m)
        {
            if (Saved != null) Saved(this, new ModelEventArgs<ModelT>(m));
        }

        /// <summary>
        /// 验证过程的事件
        /// </summary>
        public event ModelCancelEventHandler<ModelT> Validating;

        /// <summary>
        /// 触发验证过程的事件
        /// </summary>
        /// <param name="e"></param>
        protected void OnValidating(ModelCancelEventArgs<ModelT> e)
        {
            if (Validating != null) Validating(this, e);
        }
        /// <summary>
        /// 验证结束的事件
        /// </summary>
        public event EventHandler<ModelEventArgs<ModelT>> Validated;

        /// <summary>
        /// 触发验证结束的事件
        /// </summary>
        /// <param name="m"></param>
        protected void OnValidated(ModelT m)
        {
            if (Validated != null) Validated(this, new ModelEventArgs<ModelT>(m));
        }

        /// <summary>
        /// 删除过程的事件
        /// </summary>
        public event EventHandler<ModelEventArgs<ModelT>> Deleting;

        /// <summary>
        /// 触发删除过程的事件
        /// </summary>
        /// <param name="m"></param>
        protected void OnDeleting(ModelT m)
        {
            if (Deleting != null) Deleting(this, new ModelEventArgs<ModelT>(m));
        }

        /// <summary>
        /// 删除结束的事件
        /// </summary>
        public event EventHandler<ModelEventArgs<ModelT>> Deleted;

        /// <summary>
        /// 触发删除结束的事件
        /// </summary>
        /// <param name="m"></param>
        protected void OnDeleted(ModelT m)
        {
            if (Deleted != null) Deleted(this, new ModelEventArgs<ModelT>(m));
        }

        /// <summary>
        /// 出发删除结束的事件
        /// </summary>
        /// <param name="m"></param>
        /// <param name="decipher"></param>
        protected void OnDeleted(ModelT m, DbDecipher decipher)
        {
            if (Deleted != null) Deleted(this, new ModelEventArgs<ModelT>(m, decipher));
        }

        /// <summary>
        /// 更新过程的事件
        /// </summary>
        public event EventHandler<ModelEventArgs<ModelT>> Updating;

        /// <summary>
        /// 触发更新过程的事件
        /// </summary>
        /// <param name="m"></param>
        protected void OnUpdating(ModelT m)
        {
            if (Updating != null) Updating(this, new ModelEventArgs<ModelT>(m));
        }

        /// <summary>
        /// 更新结束的事件
        /// </summary>
        public event EventHandler<ModelEventArgs<ModelT>> Updated;

        /// <summary>
        /// 触发更新结束的事件
        /// </summary>
        /// <param name="m"></param>
        /// <param name="decipher"></param>
        protected void OnUpdated(ModelT m, DbDecipher decipher)
        {
            if (Updated != null) Updated(this, new ModelEventArgs<ModelT>(m, decipher));
        }

        /// <summary>
        /// 页面跳转过程的事件
        /// </summary>
        public event EventHandler<ModelListEventArgs<ModelT>> GoPaging;

        /// <summary>
        /// 页面跳转结束的事件
        /// </summary>
        public event EventHandler<ModelListEventArgs<ModelT>> GoPaged;

        /// <summary>
        /// 触发页面跳转过程的事件
        /// </summary>
        /// <param name="models"></param>
        protected void OnGoPageing(IList<ModelT> models)
        {
            if (GoPaging != null)
            {
                GoPaging(this, new ModelListEventArgs<ModelT>(models));
            }
        }

        /// <summary>
        /// 触发页面跳转结束的事件
        /// </summary>
        /// <param name="models"></param>
        protected void OnGoPaged(IList<ModelT> models)
        {
            if (GoPaged != null)
            {
                GoPaged(this, new ModelListEventArgs<ModelT>(models));
            }
        }

        protected void OnGoPaging(LModelList<ModelT> models)
        {

        }


        #endregion

        LModelList<ModelT> m_Models;


        public LModelList<ModelT> Models
        {
            get { return m_Models; }
        }


        DataGridView m_DataGridView;

        List<ConditionElement> m_FilterFixed = new List<ConditionElement>();

        List<ConditionElement> m_FilterSearch = new List<ConditionElement>();

        List<ConditionElement> m_FilterDelete = new List<ConditionElement>();

        List<ConditionElement> m_NewDefaultValues = new List<ConditionElement>();

        List<ConditionElement> m_FilterChangeStatus = new List<ConditionElement>();

        public List<ConditionElement> FilterSearch
        {
            get { return m_FilterSearch; }
        }

        public List<ConditionElement> NewDefaultValues
        {
            get { return m_NewDefaultValues; }
        }

        public List<ConditionElement> FilterFixed
        {
            get
            {
                return m_FilterFixed;
            }
        }

        public void AddFilterChangedStatus(string fieldName, object value, Logic logic)
        {
            m_FilterChangeStatus.Add(new ConditionElement(fieldName, value, logic));
        }

        public void AddFilterChangedStatus(string fieldName, object value)
        {
            m_FilterChangeStatus.Add(new ConditionElement(fieldName, value));
        }

        public List<ConditionElement> FilterDelete
        {
            get { return m_FilterDelete; }
        }

        public void AddNewDefaultValues(string fieldName, object value, Logic logic)
        {
            m_NewDefaultValues.Add(new ConditionElement(fieldName, value, logic));
        }

        public void AddNewDefaultValues(string fieldName, object value)
        {
            m_NewDefaultValues.Add(new ConditionElement(fieldName, value));
        }


        public void AddNewDefaultValues(object[] args)
        {
            for (int i = 0; i < args.Length; i += 2)
            {
                string fieldName = (string)args[i];
                object fieldValue = args[i + 1];

                m_NewDefaultValues.Add(new ConditionElement(fieldName, fieldValue));
            }
        }



        public void AddFilterDelete(string fieldName, object value, Logic logic)
        {
            m_FilterDelete.Add(new ConditionElement(fieldName, value, logic));
        }

        public void AddFilterDelete(string fieldName, object value)
        {
            m_FilterDelete.Add(new ConditionElement(fieldName, value));
        }

        public void AddFilterSearch(string fieldName, object value, Logic logic)
        {
            m_FilterSearch.Add(new ConditionElement(fieldName, value, logic));
        }

        public void AddFilterSearch(string table, string fieldName, object value, Logic logic)
        {
            m_FilterSearch.Add(new ConditionElement(fieldName, value, logic) { ModelName = table });
        }

        public void AddFilterSearch(string fieldName, object value)
        {
            m_FilterSearch.Add(new ConditionElement(fieldName, value, Logic.Equality));
        }

        public void AddFilterFixed(string fieldName, object value)
        {
            m_FilterFixed.Add(new ConditionElement(fieldName, value, Logic.Equality));
        }


        public void AddFilterFixed(string fieldName, object value, Logic logic)
        {
            m_FilterFixed.Add(new ConditionElement(fieldName, value, logic));
        }


        private void Modify(LightModel model, DataGridView grid)
        {

            LModelElement modelElem;

            if (model is LModel)
            {
                modelElem = ((LModel)model).GetModelElement();
            }
            else
            {
                modelElem = LightModel.GetLModelElement(model.GetType());
            }

            foreach (BoundField item in grid.Columns)
            {
                EditorTextCell editorCell = item as EditorTextCell;

                if (editorCell == null
                    || string.IsNullOrEmpty(editorCell.DefaultValue)
                    || !modelElem.Fields.ContainsField(item.DataField))
                {
                    continue;
                }

                LModelFieldElement fieldElem = modelElem.Fields[item.DataField];

                model[fieldElem.DBField] = ModelConvert.ChangeType(editorCell.DefaultValue, fieldElem.DBType);
            }

        }



        private DbDecipher OpenDecipher()
        {

            if (m_Decipher != null && m_Decipher.State == ConnectionState.Open)
            {
                return m_Decipher;
            }

            string commonDecipher = "APP.COMMON_DECIPHER";

            HttpContext context = HttpContext.Current;

            DbDecipher decipher = null;

            if (context.Items[commonDecipher] == null)
            {

                EcUserState userState = EcContext.Current.User;

                if (userState.ExpandPropertys.ContainsKey("DbDecipherName"))
                {
                    userState.DbDecipherName = userState.ExpandPropertys["DbDecipherName"];
                }

                if (string.IsNullOrEmpty(userState.DbDecipherName))
                {
                    decipher = DbDecipherManager.GetDecipher();
                }
                else
                {
                    decipher = DbDecipherManager.GetDecipher(userState.DbDecipherName);
                }

                decipher.Open();

                context.Items[commonDecipher] = decipher;
            }
            else
            {
                decipher = (DbDecipher)context.Items[commonDecipher];

                if (decipher.State == ConnectionState.Closed)
                {
                    EcUserState userState = EcContext.Current.User;

                    if (userState.ExpandPropertys.ContainsKey("DbDecipherName"))
                    {
                        userState.DbDecipherName = userState.ExpandPropertys["DbDecipherName"];
                    }

                    if (string.IsNullOrEmpty(userState.DbDecipherName))
                    {
                        decipher = DbDecipherManager.GetDecipher();
                    }
                    else
                    {
                        decipher = DbDecipherManager.GetDecipher(userState.DbDecipherName);
                    }

                    decipher.Open();

                    context.Items[commonDecipher] = decipher;
                }
            }

            m_Decipher = decipher;

            return decipher;
        }

        /// <summary>
        /// 设置焦点
        /// </summary>
        public void Focus()
        {
            if (m_Models == null || m_Models.Count == 0)
            {
                this.m_DataGridView.SetFocusedItemValue(string.Empty);
                return;
            }

            ModelT model = m_Models[0];

            //LModelElement modelElem = LightModel.GetLModelElement<ModelT>();

            object pkValue = LightModel.GetPkValue(model);

            m_DataGridView.SetFocusedItemValue(pkValue.ToString());
        }

        /// <summary>
        /// 获取没有分页的实体集合
        /// </summary>
        /// <returns></returns>
        public LModelList<ModelT> GetModelList()
        {
            LightModelFilter filter = new LightModelFilter(typeof(ModelT));

            AddFilterItems(filter, m_FilterFixed);
            AddFilterItems(filter, m_FilterSearch);

            DbDecipher decipher = OpenDecipher();

            LModelList<ModelT> models = decipher.SelectModels<ModelT>(filter);

            m_Models = models;


            return models;
        }

        public void GoPage(int pageIndex)
        {
            if (!m_SecSelect) { MiniHelper.Alert("记录已经上锁.您无权操作.!"); return; }

            DbDecipher decipher = OpenDecipher();

            GoPage(pageIndex, decipher);

        }

        private void GoPage(int pageIndex, DbDecipher decipher)
        {
            #region 构造 filter 参数

            LightModelFilter filter = new LightModelFilter(typeof(ModelT));

            if (m_JoinItems != null)
            {
                filter.Joins.AddRange(m_JoinItems);

                filter.JoinPk = m_JoinPk;

                filter.DefalutModel = 0;
            }

            AddFilterItems(filter, m_FilterFixed);
            AddFilterItems(filter, m_FilterSearch);

            int rowCount = m_DataGridView.Pagination.RowCount;
            //int curPage = m_DataGridView.Pagination.CurPage;

            m_DataGridView.Pagination.CurPage = pageIndex;
            int curPage = pageIndex;

            filter.Limit = Limit.ByPageIndex(rowCount, curPage);

            filter.TSqlOrderBy = m_DataGridView.SortExpression;

            #endregion

            LModelList<ModelT> models;

            if (m_DataGridView.PagerVisible)
            {
                models = decipher.SelectModelsByPage<ModelT>(filter);
            }
            else
            {
                models = decipher.SelectModels<ModelT>(filter);
            }

            m_Models = models;

            OnGoPageing(models);

            m_DataGridView.Items.Clear();
            m_DataGridView.Items.AddRange(models);
            m_DataGridView.Pagination.ItemTotal = models.PagesInfo.RowTotal;
            m_DataGridView.Reset();

            OnGoPaged(models);

            m_DataGridView.ClearDataStoreStatus();
            m_DataGridView.ClearSelectedStatus();

            if (models.Count > 0)
            {
                m_DataGridView.SetItemFocus(0);
            }

        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        public void Refresh()
        {
            #region 构造 filter 参数

            LightModelFilter filter = new LightModelFilter(typeof(ModelT));

            if (m_JoinItems != null)
            {
                filter.Joins.AddRange(m_JoinItems);

                filter.JoinPk = m_JoinPk;

                filter.DefalutModel = 0;
            }

            AddFilterItems(filter, m_FilterFixed);
            AddFilterItems(filter, m_FilterSearch);

            int rowCount = m_DataGridView.Pagination.RowCount;
            //int curPage = m_DataGridView.Pagination.CurPage;

            //m_DataGridView.Pagination.CurPage = pageIndex;
            //int curPage = pageIndex;

            filter.Limit = Limit.ByPageIndex(rowCount, m_DataGridView.Pagination.CurPage);

            filter.TSqlOrderBy = m_DataGridView.SortExpression;

            #endregion

            DbDecipher decipher = OpenDecipher();

            LModelList<ModelT> models = decipher.SelectModelsByPage<ModelT>(filter);

            m_Models = models;

            OnGoPageing(models);

            m_DataGridView.Items.Clear();
            m_DataGridView.Items.AddRange(models);
            m_DataGridView.Pagination.ItemTotal = models.PagesInfo.RowTotal;
            m_DataGridView.Reset();

            OnGoPaged(models);

            m_DataGridView.ClearDataStoreStatus();
            m_DataGridView.ClearSelectedStatus();

            if (models.Count > 0)
            {
                int foucsIndex = m_DataGridView.FocusedItem.Index;
                m_DataGridView.SetItemFocus(foucsIndex);
            }

        }

        public void GoPage()
        {
            if (!m_SecSelect) { MiniHelper.Alert("记录已经上锁.您无权操作.!"); return; }

            int curPage = m_DataGridView.Pagination.CurPage;

            DbDecipher decipher = OpenDecipher();

            GoPage(curPage, decipher);
        }


        /// <summary>
        /// 跳到最后页
        /// </summary>
        private void GoPageLast(DbDecipher decipher)
        {
            LightModelFilter filter = new LightModelFilter(typeof(ModelT));


            if (m_JoinItems != null)
            {
                filter.Joins.AddRange(m_JoinItems);

                filter.JoinPk = m_JoinPk;

                filter.DefalutModel = 0;
            }


            AddFilterItems(filter, m_FilterFixed);
            AddFilterItems(filter, m_FilterSearch);


            int rowCount = m_DataGridView.Pagination.RowCount;

            int totalRows = decipher.SelectCount(filter) + 1;  //加上1，表示未来新增加的记录

            int surplusNum = totalRows % rowCount;

            int pageLastIndex = (totalRows - surplusNum) / rowCount - 1;

            if (surplusNum > 0)
            {
                pageLastIndex++;
            }

            if (pageLastIndex != m_DataGridView.Pagination.CurPage)
            {
                GoPage(pageLastIndex, decipher);
            }
            else
            {
                m_DataGridView.Pagination.CurPage = pageLastIndex;
                m_DataGridView.Pagination.ItemTotal = totalRows;
                m_DataGridView.Pagination.RowCount = rowCount;
                m_DataGridView.Reset();

            }

        }

        public void New()
        {
            if (!m_SecAdd) { MiniHelper.Alert("记录已经上锁.您无权操作.!"); return; }

            LModelElement modelElem = LightModel.GetLModelElement<ModelT>();

            DbDecipher decipher = OpenDecipher();


            try
            {
                ModelT model = Activator.CreateInstance<ModelT>();

                LightModel lm = model as LightModel;

                Modify(lm, m_DataGridView);

                LModelFieldElementCollection fieldElems = modelElem.Fields;


                foreach (ConditionElement item in m_FilterFixed)
                {
                    LModelFieldElement fieldElem = fieldElems[item.FieldName];

                    object value = ModelConvert.ChangeType(item.FieldValue, fieldElem.DBType, fieldElem.Mandatory);

                    LightModel.SetFieldValue(model, item.FieldName, value);
                }

                foreach (ConditionElement item in m_NewDefaultValues)
                {
                    LModelFieldElement fieldElem = fieldElems[item.FieldName];

                    object value = ModelConvert.ChangeType(item.FieldValue, fieldElem.DBType, fieldElem.Mandatory);

                    LightModel.SetFieldValue(model, item.FieldName, value);
                }

                GoPageLast(decipher);



                OnAdding(model);

                decipher.InsertModel(model);

                OnAdded(model);


                m_DataGridView.Items.Add(model);
                m_DataGridView.Reset();

                m_DataGridView.SetItemFocus(int.MaxValue);
            }
            catch (Exception ex)
            {
                log.Error(ex);

                MiniHelper.Alert("创建新行失败!");
            }


        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            if (!m_SecSave) { MiniHelper.Alert("记录已经上锁.您无权操作.!"); return; }

            if (m_ValidateSaveing)
            {
                bool valid = ValidateGridFor(m_DataGridView);

                if (!valid)
                {
                    MiniHelper.Alert("单元格填写有错误，请检查！");
                    return;
                }
            }


            DbDecipher decipher = OpenDecipher();


            try
            {
                decipher.BeginTransaction();

                SaveGridFor(decipher, m_DataGridView);


                if (!m_SavingCancel)
                {
                    decipher.TransactionCommit();

                    MiniHelper.Tooltip("保存成功!");

                    m_DataGridView.ClearDataStoreStatus();

                    m_DataGridView.ClearModifiedTagAll();
                    m_DataGridView.ClearSelectedStatus();
                }
                else
                {
                    decipher.TransactionRollback();
                }

            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error(ex);

                MiniHelper.Alert("保存失败!");
            }


        }


        private string GetSelectLineName()
        {
            foreach (BoundField item in m_DataGridView.Columns)
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



        private LightModelFilter GetFilter(int[] ids)
        {

            LModelElement modelElem = LightModel.GetLModelElement<ModelT>();

            LightModelFilter filter = new LightModelFilter(typeof(ModelT));
            filter.And(modelElem.PrimaryKey, ids, HWQ.Entity.Filter.Logic.In);

            //foreach (ConditionElement item in m_FilterDelete)
            //{
            //    filter.And(item.FieldName, item.FieldValue, item.Logic);
            //}

            return filter;
        }

        /// <summary>
        /// 获取被选中的实体
        /// </summary>
        /// <returns></returns>
        public LModelList<ModelT> GetModelsForCheckedInt()
        {
            string checkName = GetSelectLineName();

            int[] ids = WebUtil.FormIntList(checkName);

            LModelElement modelElem = LightModel.GetLModelElement<ModelT>();

            LightModelFilter filter = new LightModelFilter(typeof(ModelT));
            filter.And(modelElem.PrimaryKey, ids, Logic.In);

            DbDecipher decipher = this.OpenDecipher();
            LModelList<ModelT> models = decipher.SelectModels<ModelT>(filter);

            return models;
        }

        /// <summary>
        /// 获取复选框打钩的值
        /// </summary>
        /// <returns></returns>
        public int[] GetCheckedIntList()
        {
            int[] ids;


            string dataKeys = this.m_DataGridView.DataKeys;
            string fixedKeys = this.m_DataGridView.FixedFields;

            int[] guids = null;

            if (dataKeys == "$P.guid")
            {
                DataSeletedStatus seletedStatsu = DataSeletedStatus.Parse(this.m_DataGridView.SelectedStatus);

                List<int> idList = new List<int>(seletedStatsu.Items.Count);
                List<int> guidList = new List<int>(seletedStatsu.Items.Count);

                foreach (DataSeletedItem item in seletedStatsu.Items)
                {
                    int pk = -1;

                    if (int.TryParse(item.Pk, out pk))
                    {
                        idList.Add(pk);
                    }

                    guidList.Add(int.Parse(item.Guid));
                }

                ids = idList.ToArray();
                guids = guidList.ToArray();

                if (idList.Count == 0 && guidList.Count == 0)
                {
                    MiniHelper.Alert("请打钩选中要操作的记录!");

                    return new int[0];
                }

                return ids;
            }


            string checkName = GetSelectLineName();

            ids = WebUtil.FormIntList(checkName);

            return ids;
        }

        public string[] GetCheckedStrList()
        {
            string[] ids;


            string dataKeys = this.m_DataGridView.DataKeys;
            string fixedKeys = this.m_DataGridView.FixedFields;

            string[] guids = null;

            if (dataKeys == "$P.guid")
            {
                DataSeletedStatus seletedStatsu = DataSeletedStatus.Parse(this.m_DataGridView.SelectedStatus);

                List<string> idList = new List<string>(seletedStatsu.Items.Count);
                List<string> guidList = new List<string>(seletedStatsu.Items.Count);

                foreach (DataSeletedItem item in seletedStatsu.Items)
                {
                    idList.Add(item.Pk);

                    guidList.Add(item.Guid);
                }

                ids = idList.ToArray();
                guids = guidList.ToArray();

                if (idList.Count == 0 && guidList.Count == 0)
                {
                    MiniHelper.Alert("请打钩选中要操作的记录!");

                    return new string[0];
                }

                return ids;
            }

            string checkName = GetSelectLineName();

            ids = WebUtil.FormStrList(checkName);

            return ids;
        }

        private bool ValidateDelete(out int[] ids, out int[] guids)
        {
            string dataKeys = this.m_DataGridView.DataKeys;
            string fixedKeys = this.m_DataGridView.FixedFields;

            guids = null;

            if (dataKeys == "$P.guid")
            {
                DataSeletedStatus seletedStatsu = DataSeletedStatus.Parse(this.m_DataGridView.SelectedStatus);

                List<int> idList = new List<int>(seletedStatsu.Items.Count);
                List<int> guidList = new List<int>(seletedStatsu.Items.Count);

                foreach (DataSeletedItem item in seletedStatsu.Items)
                {
                    int pk = -1;

                    if (int.TryParse(item.Pk, out pk))
                    {
                        idList.Add(pk);
                    }

                    guidList.Add(int.Parse(item.Guid));
                }

                ids = idList.ToArray();
                guids = guidList.ToArray();

                if (idList.Count == 0 && guidList.Count == 0)
                {
                    MiniHelper.Alert("请打钩选中要操作的记录!");
                    return false;
                }

                return true;
            }


            string checkName = GetSelectLineName();

            if (string.IsNullOrEmpty(checkName))
            {
                MiniHelper.Alert("没有指定键名称!");
                ids = null;
                return false;
            }

            ids = WebUtil.FormIntList(checkName);

            if (ids.Length == 0)
            {
                MiniHelper.Alert("请打钩选中要操作的记录!");

                return false;
            }


            return true;
        }

        #region ChangeStatus

        public virtual int ChangeStatus(string statusFieldName, int[] srcStatusId, int targetStatusId)
        {
            return ChangeStatus(statusFieldName, srcStatusId, targetStatusId, false);
        }

        public virtual int ChangeStatus(string statusFieldName, int[] srcStatusId, int targetStatusId, bool autoTooltip)
        {
            if (!m_SecSave) { MiniHelper.Alert("记录已经上锁.您无权操作.!"); return 0; }

            int[] ids;
            int[] guids;

            if (!ValidateDelete(out ids, out guids))
            {
                return 0;
            }

            DbDecipher decipher = OpenDecipher();
            LModelElement modelElem = LightModel.GetLModelElement<ModelT>();

            LightModelFilter filter = GetFilter(ids);

            filter.And(statusFieldName, srcStatusId, Logic.In);


            AddFilterItems(filter, m_FilterFixed);
            AddFilterItems(filter, m_FilterChangeStatus);


            int n = 0;

            LModelList<ModelT> models = null;

            if (Updating != null || Updated != null)
            {
                models = decipher.SelectModels<ModelT>(filter);
            }

            try
            {

                if (Updating != null)
                {
                    foreach (ModelT item in models)
                    {
                        OnUpdating(item);
                    }
                }

                n = decipher.UpdateProps(filter, new object[] { statusFieldName, targetStatusId });

                if (Updated != null)
                {
                    foreach (ModelT item in models)
                    {
                        LightModel.SetFieldValue(item, statusFieldName, targetStatusId);
                        OnUpdated(item, decipher);
                    }
                }

                if (autoTooltip)
                {
                    MiniHelper.Tooltip("共删除 {0} 条记录", n);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MiniHelper.Tooltip("删除失败!");
            }

            this.Refresh();

            return n;
        }


        private void AddFilterItems(LightModelFilter filter,List<ConditionElement> filterItems)
        {
            if (filterItems == null || filterItems.Count == 0) { return; }

            foreach (ConditionElement item in filterItems)
            {
                if (item.ExprType == ExprTypes.And)
                {
                    filter.And(item.FieldName, item.FieldValue, item.Logic);
                }
                else
                {
                    filter.Or(item.FieldName, item.FieldValue, item.Logic);
                }
            }

        }

        /// <summary>
        /// 改变字段状态值.
        /// 一般情况下修改"状态"字段 STATUS_ID 的值.
        /// </summary>
        /// <param name="statusFieldName">字段名称</param>
        /// <param name="logic">逻辑条件</param>
        /// <param name="srcStatusId">原状态值</param>
        /// <param name="targetStatusId">目标状态值</param>
        /// <param name="autoTooltip">自动提示</param>
        /// <returns>修改的记录数量</returns>
        public virtual int ChangeStatus(string statusFieldName, Logic logic, int srcStatusId, int targetStatusId, bool autoTooltip)
        {
            if (!m_SecSave) { MiniHelper.Alert("记录已经上锁.您无权操作.!"); return 0; }

            int[] ids;
            int[] guids;

            if (!ValidateDelete(out ids, out guids))
            {
                return 0;
            }

            DbDecipher decipher = OpenDecipher();

            LightModelFilter filter = GetFilter(ids);

            filter.And(statusFieldName, srcStatusId, logic);

            AddFilterItems(filter, m_FilterFixed);
            AddFilterItems(filter, m_FilterChangeStatus);


            LModelList<ModelT> models = null;

            if (Updating != null || Updated != null)
            {
                models = decipher.SelectModels<ModelT>(filter);
            }

            int n = 0;

            try
            {
                if (Updating != null)
                {
                    foreach (ModelT item in models)
                    {
                        OnUpdating(item);
                    }
                }

                n = decipher.UpdateProps(filter, new object[] { statusFieldName, targetStatusId });

                if (Updated != null)
                {
                    foreach (ModelT item in models)
                    {
                        LightModel.SetFieldValue(item, statusFieldName, targetStatusId);
                        OnUpdated(item,decipher);
                    }
                }


                if (autoTooltip) { MiniHelper.Tooltip("更新 {0} 条记录", n); }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                MiniHelper.Tooltip("更新失败!");
            }


            this.Refresh();


            return n;
        }

        /// <summary>
        /// 改变字段状态值.
        /// 一般情况下修改"状态"字段 STATUS_ID 的值.
        /// </summary>
        /// <param name="statusFieldName">字段名称</param>
        /// <param name="srcStatusId">原状态值</param>
        /// <param name="targetStatusId">目标状态值</param>
        /// <returns>修改的记录数量</returns>
        public virtual int ChangeStatus(string statusFieldName, int srcStatusId, int targetStatusId)
        {
            return ChangeStatus(statusFieldName, Logic.Equality, srcStatusId, targetStatusId, false);
        }

        /// <summary>
        /// 改变字段状态值.
        /// 一般情况下修改"状态"字段 STATUS_ID 的值.
        /// </summary>
        /// <param name="statusFieldName">字段名称</param>
        /// <param name="logic">逻辑条件</param>
        /// <param name="srcStatusId">原状态值</param>
        /// <param name="targetStatusId">目标状态值</param>
        /// <returns>修改的记录数量</returns>
        public virtual int ChangeStatus(string statusFieldName, Logic logic, int srcStatusId, int targetStatusId)
        {
            return ChangeStatus(statusFieldName, logic, srcStatusId, targetStatusId, false);
        }

        /// <summary>
        /// 改变字段状态值.
        /// 一般情况下修改"状态"字段 STATUS_ID 的值.
        /// </summary>
        /// <param name="statusFieldName">字段名称</param>
        /// <param name="srcStatusId">原状态值</param>
        /// <param name="targetStatusId">目标状态值</param>
        /// <param name="autoTooltip">自动提示</param>
        /// <returns>修改的记录数量</returns>
        public virtual int ChangeStatus(string statusFieldName, int srcStatusId, int targetStatusId, bool autoTooltip)
        {
            return ChangeStatus(statusFieldName, Logic.Equality, srcStatusId, targetStatusId, false);
        }


        #endregion

        /// <summary>
        /// 删除筛选内容
        /// </summary>
        public void DeleteForFilter()
        {
            if (!m_SecDelete) { MiniHelper.Alert("记录已经上锁.您无权操作.!"); return; }


            LModelElement modelElem = LightModel.GetLModelElement<ModelT>();

            LightModelFilter filter = new LightModelFilter(typeof(ModelT));

            AddFilterItems(filter, m_FilterFixed);
            AddFilterItems(filter, m_FilterDelete);


            DbDecipher decipher = OpenDecipher();



            LModelList<ModelT> models = null;

            if (Deleting != null || Deleted != null)
            {
                models = decipher.SelectModels<ModelT>(filter);
            }

            try
            {
                if (Deleting != null)
                {
                    foreach (ModelT item in models)
                    {
                        OnDeleting(item);
                    }
                }

                int n = decipher.DeleteModels(filter);

                MiniHelper.Tooltip("共删除 {0} 条记录", n);

                if (Deleted != null)
                {
                    foreach (ModelT item in models)
                    {
                        OnDeleted(item, decipher);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MiniHelper.Tooltip("删除失败!");
            }

            
            this.Refresh();

        }
        
        /// <summary>
        /// 删除被选中的记录
        /// </summary>
        public virtual void DeleteItems()
        {
            if (!m_SecDelete) { MiniHelper.Alert("记录已经上锁.您无权操作.!"); return; }

            int[] ids;
            int[] guids;

            if (!ValidateDelete(out ids, out guids))
            {
                return;
            }

            DbDecipher decipher = OpenDecipher();

            LightModelFilter filter = GetFilter(ids);

            AddFilterItems(filter, m_FilterFixed);
            AddFilterItems(filter, m_FilterDelete);

            LModelList<ModelT> models = null;

            if (Deleting != null || Deleted != null)
            {
                models = decipher.SelectModels<ModelT>(filter);
            }

            try
            {
                if (Deleting != null)
                {
                    foreach (ModelT item in models)
                    {
                        OnDeleting(item);
                    }
                }

                int n = decipher.DeleteModels(filter);

                MiniHelper.Tooltip("共删除 {0} 条记录", n);

                if (Deleted != null)
                {
                    foreach (ModelT item in models)
                    {
                        OnDeleted(item, decipher);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MiniHelper.Tooltip("删除失败!");
            }


            this.Refresh();
        }

        List<InputFieldError> m_ValidateErrors;

        /// <summary>
        /// 验证后,产生的错误信息
        /// </summary>
        public InputFieldError[] ValidateErrors
        {
            get
            {
                if (m_ValidateErrors == null)
                {
                    return new InputFieldError[0];
                }

                return m_ValidateErrors.ToArray();
            }
        }

        /// <summary>
        /// 验证单元格
        /// </summary>
        /// <param name="grid"></param>
        private bool ValidateGridFor(DataGridView grid)
        {
            string json = grid.DataStoreStatus;

            if (string.IsNullOrEmpty(json))
            {
                return true;
            }

            string dataKeys = grid.DataKeys;

            string fixedFields = grid.FixedFields;

            DataStoreStatus dataStaus = DataStoreStatus.Parse(json);
            LModelElement modelElem = LightModel.GetLModelElement<ModelT>();

            List<string> updateFields = new List<string>();

            List<InputFieldError> errors = new List<InputFieldError>();

            try
            {
                foreach (DataStoreRow row in dataStaus.Rows)
                {
                    if (row.State == DataStoreRowState.Modified)
                    {

                        ModelT modelInstance = Activator.CreateInstance<ModelT>();
                        LightModel model = modelInstance as LightModel;
                        SetCellValue(modelElem, model, row, updateFields);

                        LModelFieldElement fieldElem;

                        if (dataKeys == "$P.guid")
                        {

                        }
                        else
                        {
                            fieldElem = modelElem.Fields[dataKeys];
                            model[dataKeys] = ModelConvert.ChangeType(row.Pk, fieldElem.DBType);
                        }

                        ModelCancelEventArgs<ModelT> ea = new ModelCancelEventArgs<ModelT>(modelInstance);

                        OnValidating(ea);

                        if (ea.Cancel)
                        {
                            errors.Add(new InputFieldError());
                            break;
                        }

                        ValidateCellValue(modelElem, row, errors);

                        OnValidated(modelInstance);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            m_ValidateErrors = errors;

            if (errors.Count > 0)
            {
                return false;
            }

            return true;
        }

        bool m_SavingCancel = false;



        private void SaveGridFor(DbDecipher decipher, DataGridView grid)
        {
            string json = grid.DataStoreStatus;

            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            string dataKeys = grid.DataKeys;
            string fixedKeys = grid.FixedFields;

            if (string.IsNullOrEmpty(dataKeys))
            {
                throw new Exception("请为 DataGridView.DataKeys 属性指定主键字段名.");
            }

            DataStoreStatus dataStaus = DataStoreStatus.Parse(json);
            LModelElement modelElem = LightModel.GetLModelElement<ModelT>();

            LModelFieldElement pkFieldElem = modelElem.Fields[dataKeys];

            List<string> updateFields = new List<string>();

            try
            {


                foreach (DataStoreRow row in dataStaus.Rows)
                {
                    if (row.State == DataStoreRowState.Modified)
                    {
                        ModelT modelInstance ;

                        if (Saving != null)
                        {
                            object pk = ModelConvert.ChangeType(row.Pk, pkFieldElem);
                            modelInstance = decipher.SelectModelByPk<ModelT>( row.Pk);
                        }
                        else
                        {
                            modelInstance = Activator.CreateInstance<ModelT>();
                        }

                        LightModel model = modelInstance as LightModel;

                        SetCellValue(modelElem, model, row, updateFields);

                        LModelFieldElement fieldElem = null;

                        if (dataKeys == "$P.guid")
                        {
                            object fixedValue = row.FixedFields[fixedKeys].Value;
                            fieldElem = modelElem.Fields[fixedKeys];
                            model[fixedKeys] = ModelConvert.ChangeType(fixedValue, fieldElem.DBType);
                        }
                        else
                        {
                            fieldElem = modelElem.Fields[dataKeys];
                            model[dataKeys] = ModelConvert.ChangeType(row.Pk, fieldElem.DBType);
                        }


                        ModelCancelEventArgs<ModelT> saveEA = new ModelCancelEventArgs<ModelT>(modelInstance, updateFields.ToArray());

                        OnSaving(saveEA);

                        m_SavingCancel = saveEA.Cancel;

                        if (saveEA.Cancel)
                        {
                            break;
                        }


                        LightModelFilter filter = new LightModelFilter(typeof(ModelT));

                        filter.And(dataKeys, model[dataKeys]);

                        AddFilterItems(filter, m_FilterFixed);

                        

                        object[] objs = new object[updateFields.Count * 2];

                        for (int i = 0; i < updateFields.Count; i++)
                        {
                            objs[i * 2] = updateFields[i];

                            objs[i * 2 + 1] = model[updateFields[i]];
                        }

                        decipher.UpdateProps(filter, objs);
                        //decipher.UpdateModelProps(model, updateFields.ToArray());

                        OnSaved(modelInstance);
                    }
                    else if (row.State == DataStoreRowState.Added)
                    {
                        ModelT modelInstance = Activator.CreateInstance<ModelT>();

                        LightModel model = modelInstance as LightModel;

                        foreach (ConditionElement item in m_FilterFixed)
                        {
                            LModelFieldElement fElem = modelElem.Fields[item.FieldName];

                            object value = ModelConvert.ChangeType(item.FieldValue, fElem.DBType, fElem.Mandatory);

                            LightModel.SetFieldValue(model, item.FieldName, value);
                        }

                        foreach (ConditionElement item in m_NewDefaultValues)
                        {
                            LModelFieldElement fElem = modelElem.Fields[item.FieldName];

                            object value = ModelConvert.ChangeType(item.FieldValue, fElem.DBType, fElem.Mandatory);

                            LightModel.SetFieldValue(model, item.FieldName, value);
                        }

                        SetCellValue(modelElem, model, row, updateFields);

                        LModelFieldElement fieldElem = null;

                        if (dataKeys == "$P.guid")
                        {
                            //object fixedValue = row.FixedFields[fixedKeys];
                            //fieldElem = modelElem.Fields[fixedKeys];
                            //model[fixedKeys] = ModelConvert.ChangeType(fixedValue, fieldElem.DBType);
                        }
                        else
                        {
                            fieldElem = modelElem.Fields[dataKeys];
                            model[dataKeys] = ModelConvert.ChangeType(row.Pk, fieldElem.DBType);
                        }

                        //ModelCancelEventArgs<ModelT> saveEA = new ModelCancelEventArgs<ModelT>(modelInstance);


                        ModelCancelEventArgs<ModelT> saveEA = new ModelCancelEventArgs<ModelT>(modelInstance);

                        OnAddingCancel(saveEA);

                        if (saveEA.Cancel)
                        {
                            break;
                        }

                        OnAdding(modelInstance);

                        decipher.InsertModel(model);

                        OnAdded(modelInstance);

                        if (dataKeys == "$P.guid")
                        {
                            this.m_DataGridView.SetFixedValueForGuid(row.Pk, fixedKeys, model[fixedKeys].ToString());
                        }
                    }
                    else if (row.State == DataStoreRowState.Deleted)
                    {
                        LightModelFilter filter = new LightModelFilter(typeof(ModelT));
                        LModelFieldElement fieldElem = null;


                        if (dataKeys == "$P.guid")
                        {
                            object fixedValue = row.FixedFields[fixedKeys].Value;
                            fieldElem = modelElem.Fields[fixedKeys];
                            //model[fixedKeys] = ModelConvert.ChangeType(fixedValue, fieldElem.DBType);

                            fieldElem = modelElem.Fields[fixedKeys];
                            filter.And(fieldElem.DBField, ModelConvert.ChangeType(fixedValue, fieldElem.DBType));
                        }
                        else
                        {
                            fieldElem = modelElem.Fields[fixedKeys];
                            filter.And(fieldElem.DBField, row.Pk);
                        }



                        filter.Top = 1;
                        int delCount = decipher.DeleteModels(filter);


                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 验证单元格
        /// </summary>
        /// <param name="modelElem"></param>
        /// <param name="row"></param>
        private void ValidateCellValue(LModelElement modelElem, DataStoreRow row, List<InputFieldError> errors)
        {


            LModelFieldElement fieldElem;

            foreach (DataStoreCell cell in row.Cells)
            {
                fieldElem = modelElem.Fields[cell.Name];

                Type dataT = ModelConvert.ToType(fieldElem.DBType);

                bool tryChangeType = StringUtil.TryChangeType(cell.Value, dataT, fieldElem.Mandatory);

                if (!tryChangeType)
                {
                    InputFieldError err = new InputFieldError();
                    err.CellID = cell.Name;
                    err.ItemGuid = 0;

                    errors.Add(err);
                }


            }
        }


        /// <summary>
        /// 获取单元格数值
        /// </summary>
        /// <param name="modelElem">实体元素</param>
        /// <param name="model">实体实例</param>
        /// <param name="row">单元格行</param>
        /// <param name="updateFields">需要更新的字段</param>
        private void SetCellValue(LModelElement modelElem, LightModel model, DataStoreRow row, List<string> updateFields)
        {
            updateFields.Clear();

            LModelFieldElement fieldElem;

            foreach (DataStoreCell cell in row.Cells)
            {
                fieldElem = modelElem.Fields[cell.Name];

                model[cell.Name] = ModelConvert.ChangeType(cell.Value, fieldElem.DBType, fieldElem.Mandatory);

                updateFields.Add(cell.Name);
            }
        }

        private string GetDBLogin(string dbLogic)
        {
            if (dbLogic == null) return string.Empty;

            dbLogic = dbLogic.Trim();

            if (dbLogic.StartsWith("'", StringComparison.Ordinal) && dbLogic.EndsWith("'", StringComparison.Ordinal))
            {
                dbLogic = dbLogic.Substring(1, dbLogic.Length - 2);
            }

            return dbLogic;
        }

        /// <summary>
        /// 获取焦点行的值
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public LModel GetModelForFocusPk(params string[] fields)
        {
            int pk = StringUtil.ToInt(m_DataGridView.FocusedItem.Pk, -1);

            LModelElement modelElem = LightModel.GetLModelElement<ModelT>();

            LightModelFilter filter = new LightModelFilter(typeof(ModelT));
            filter.And(modelElem.PrimaryKey, pk);
            filter.Fields = fields;
            filter.Top = 1;


            DbDecipher decipher = OpenDecipher();

            LModel model = decipher.GetModel(filter);

            return model;
        }

        /// <summary>
        /// 获取焦点行的值
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public LModel GetLModelForFocusPk(params string[] fields)
        {
            int pk = StringUtil.ToInt(m_DataGridView.FocusedItem.Pk, -1);

            LModelElement modelElem = LightModel.GetLModelElement<ModelT>();

            LightModelFilter filter = new LightModelFilter(typeof(ModelT));
            filter.And(modelElem.PrimaryKey, pk);
            filter.Fields = fields;
            filter.Top = 1;


            DbDecipher decipher = OpenDecipher();

            LModel model = decipher.GetModel(filter);

            return model;
        }

        /// <summary>
        /// 初始化查询参数
        /// </summary>
        /// <param name="owner"></param>
        public void InitSearch(Control owner)
        {
            string group = "Search";

            if (owner is IAttributeAccessor)
            {
                IAttributeAccessor attr = owner as IAttributeAccessor;

                string groupStr = attr.GetAttribute("Groups");

                if (!StringUtil.IsNullOrWhiteSpace(groupStr))
                {
                    group = groupStr;
                }
            }

            InitSearch(owner, group);
        }

        /// <summary>
        /// 初始化查询参数
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="group"></param>
        public void InitSearch(Control owner, string group)
        {
            LModelElement modelElem = LightModel.GetLModelElement<ModelT>();
            LModelFieldElement fieldElem = null;

            List<LFilterItem> filterItems = LFilterHelper.GetFilter(owner, group, true);

            foreach (LFilterItem item in filterItems)
            {

                Logic logic = LFilterHelper.ConvertLogic(GetDBLogin(item.DBLogic));

                if (logic == Logic.Like)
                {
                    string v = item.Value.ToString();

                    if (!v.Contains("%"))
                    {
                        item.Value = "%" + v + "%";
                    }
                }

                if (modelElem.Fields.ContainsField(item.DBField))
                {
                    fieldElem = modelElem.Fields[item.DBField];
                }
                else
                {
                    fieldElem = null;
                }



                if ((fieldElem != null && (fieldElem.DBType == LMFieldDBTypes.Date || fieldElem.DBType == LMFieldDBTypes.DateTime)) ||
                    "DATE".Equals(item.DBType, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (logic == Logic.LessThan || logic == Logic.LessThanOrEqual)
                    {
                        DateTime v;

                        if (DateTime.TryParse((string)item.Value, out v))
                        {
                            v = new DateTime(v.Year, v.Month, v.Day, 23, 59, 59);

                            item.Value = v.ToString("yyyy-MM-dd HH:mm:ss");

                            AddFilterSearch(item.DBTable, item.DBField, v, logic);
                        }
                    }
                    else if (logic == Logic.GreaterThan || logic == Logic.GreaterThanOrEqual)
                    {
                        DateTime v;

                        if (DateTime.TryParse((string)item.Value, out v))
                        {
                            v = new DateTime(v.Year, v.Month, v.Day, 0, 0, 0);

                            item.Value = v.ToString("yyyy-MM-dd HH:mm:ss");

                            AddFilterSearch(item.DBTable, item.DBField, v, logic);
                        }
                    }
                }
                else
                {
                    AddFilterSearch(item.DBTable, item.DBField, item.Value, logic);
                }
            }
        }

        List<JoinItem> m_JoinItems ;

        string m_JoinPk;

        public string JoinPk
        {
            get { return m_JoinPk; }
            set { m_JoinPk = value; }
        }

        /// <summary>
        /// 查询关联
        /// </summary>
        /// <param name="modelT"></param>
        /// <param name="on"></param>
        public void SearchJoin(Type modelT, string on)
        {
            if (m_JoinItems == null)
            {
                m_JoinItems = new List<JoinItem>();
            }

            m_JoinItems.Add(new JoinItem(modelT, on));
        }

        /// <summary>
        /// 查询关联
        /// </summary>
        /// <param name="modelT"></param>
        /// <param name="on"></param>
        public void SearchJoin(JoinTypes joinType, Type modelT, string on)
        {
            if (m_JoinItems == null)
            {
                m_JoinItems = new List<JoinItem>();
            }

            m_JoinItems.Add(new JoinItem(joinType,modelT, on));
        }
    }
}
