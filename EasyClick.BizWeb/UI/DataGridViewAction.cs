using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using EasyClick.Web.Mini;
using EC5;
using EC5.SystemBoard;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using HWQ.Entity.WebExpand;

namespace EasyClick.BizWeb.UI
{

    /// <summary>
    /// DataGridView 动作操作
    /// </summary>
    [Obsolete]
    public class DataGridViewAction 
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Type m_ModelT;
        string m_ModelName;

        /// <summary>
        /// DataGridView 动作操作的构造方法
        /// </summary>
        /// <param name="grid"></param>
        public DataGridViewAction(DataGridView grid)
        {
            m_DataGridView = grid;
        }


        /// <summary>
        /// DataGridView 动作操作的构造方法
        /// </summary>
        /// <param name="modelT"></param>
        /// <param name="grid"></param>
        public DataGridViewAction(Type modelT, DataGridView grid)
        {
            m_ModelT = modelT;
            m_DataGridView = grid;
        }


        /// <summary>
        /// DataGridView 动作操作的构造方法
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="grid"></param>
        public DataGridViewAction(string modelName, DataGridView grid)
        {
            m_ModelName = modelName;

            LModelElement modelElem = LightModel.GetLModelElement(modelName);
            m_ModelT = modelElem.ModelT;

            m_DataGridView = grid;
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

        public event EventHandler<ModelEventArgs> Adding;

        protected void OnAdding(object m)
        {
            if (Adding != null) Adding(this, new ModelEventArgs(m));
        }

        public event ModelCancelEventHandler AddingCancel;

        protected void OnAddingCancel(ModelCancelEventArgs e)
        {
            if (AddingCancel != null) AddingCancel(this, e);
        }


        public event EventHandler<ModelEventArgs> Added;

        protected void OnAdded(object m)
        {
            if (Added != null) Added(this, new ModelEventArgs(m));
        }


        public event ModelCancelEventHandler Saving;

        protected void OnSaving(ModelCancelEventArgs e)
        {
            if (Saving != null) Saving(this, e);
        }

        public event EventHandler<ModelEventArgs> Saved;

        protected void OnSaved(object m)
        {
            if (Saved != null) Saved(this, new ModelEventArgs(m));
        }


        public event ModelCancelEventHandler Validating;

        protected void OnValidating(ModelCancelEventArgs e)
        {
            if (Validating != null) Validating(this, e);
        }

        public event EventHandler<ModelEventArgs> Validated;

        protected void OnValidated(object m)
        {
            if (Validated != null) Validated(this, new ModelEventArgs(m));
        }


        public event EventHandler<ModelEventArgs> Deleting;

        protected void OnDeleting(object m)
        {
            if (Deleting != null) Deleting(this, new ModelEventArgs(m));
        }

        protected void OnItemsDeleting(IList models)
        {
            if (Deleting == null) return;

            foreach (object item in models)
            {
                Deleting(this, new ModelEventArgs(item));
            }
        }

        protected void OnItemsDeleted(IList models)
        {
            if (Deleted == null) return;

            foreach (object item in models)
            {
                Deleted(this, new ModelEventArgs(item));
            }
        }

        public event EventHandler<ModelEventArgs> Deleted;

        protected void OnDeleted(object m)
        {
            if (Deleted != null) Deleted(this, new ModelEventArgs(m));
        }

        protected void OnDeleted(object m, DbDecipher decipher)
        {
            if (Deleted != null) Deleted(this, new ModelEventArgs(m, decipher));
        }


        #endregion

        LModelList m_Models;

        /// <summary>
        /// 
        /// </summary>
        public LModelList Models
        {
            get { return m_Models; }
        }

        public Type ModelT
        {
            get { return m_ModelT; }
            set { m_ModelT = value; }
        }




        DataGridView m_DataGridView;


        List<ConditionElement> m_FilterSearch = new List<ConditionElement>();

        List<ConditionElement> m_FilterDelete = new List<ConditionElement>();

        List<ConditionElement> m_NewDefaultValues = new List<ConditionElement>();

        List<ConditionElement> m_FilterChangeStatus = new List<ConditionElement>();


        public void FilterChangedStatus(string fieldName, object value, Logic logic)
        {
            m_FilterChangeStatus.Add(new ConditionElement(fieldName, value, logic));
        }

        public void FilterChangedStatus(string fieldName, object value)
        {
            m_FilterChangeStatus.Add(new ConditionElement(fieldName, value));
        }


        public void NewDefaultValues(string fieldName, object value, Logic logic)
        {
            m_NewDefaultValues.Add(new ConditionElement(fieldName, value, logic));
        }

        public void NewDefaultValues(string fieldName, object value)
        {
            m_NewDefaultValues.Add(new ConditionElement(fieldName, value));
        }


        public void FilterDelete(string fieldName, object value, Logic logic)
        {
            m_FilterDelete.Add(new ConditionElement(fieldName, value, logic));
        }

        public void FilterDelete(string fieldName, object value)
        {
            m_FilterDelete.Add(new ConditionElement(fieldName, value));
        }

        public void FilterSearch(string fieldName, object value, Logic logic)
        {
            m_FilterSearch.Add(new ConditionElement(fieldName, value, logic));
        }

        public void FilterSearch(string fieldName, object value)
        {
            m_FilterSearch.Add(new ConditionElement(fieldName, value, Logic.Equality));
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

        public void Focus()
        {
            if (m_Models == null || m_Models.Count == 0)
            {
                this.m_DataGridView.SetFocusedItemValue(string.Empty);
                return;
            }

            object model = m_Models[0];

            //LModelElement modelElem = LightModel.GetLModelElement<ModelT>();

            object pkValue = LightModel.GetPkValue(model);

            m_DataGridView.SetFocusedItemValue(pkValue.ToString());
        }

        public event EventHandler<ModelListEventArgs> GoPaging;

        public event EventHandler<ModelListEventArgs> GoPaged;

        protected void OnGoPageing(IList models)
        {
            if (GoPaging != null)
            {
                GoPaging(this, new ModelListEventArgs(models));
            }
        }

        protected void OnGoPaged(IList models)
        {
            if (GoPaged != null)
            {
                GoPaged(this, new ModelListEventArgs(models));
            }
        }

        protected void OnGoPaging(LModelList models)
        {

        }

        public LModelList GetModelList()
        {
            LightModelFilter filter = new LightModelFilter(m_ModelT);


            foreach (ConditionElement item in m_FilterSearch)
            {
                filter.And(item.FieldName, item.FieldValue, item.Logic);
            }

            DbDecipher decipher = OpenDecipher();

            LModelList models = (LModelList)decipher.SelectModels(filter);

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

            LightModelFilter filter = new LightModelFilter(m_ModelT);


            foreach (ConditionElement item in m_FilterSearch)
            {
                filter.And(item.FieldName, item.FieldValue, item.Logic);
            }

            int rowCount = m_DataGridView.Pagination.RowCount;
            //int curPage = m_DataGridView.Pagination.CurPage;

            m_DataGridView.Pagination.CurPage = pageIndex;
            int curPage = pageIndex;

            filter.Limit = Limit.ByPageIndex(rowCount, curPage);

            filter.TSqlOrderBy = m_DataGridView.SortExpression;

            #endregion

            LModelList models = (LModelList)decipher.SelectModelsByPage(filter);

            m_Models = models;

            OnGoPageing(models);

            m_DataGridView.Items.Clear();
            m_DataGridView.Items.AddRange(models);
            m_DataGridView.Pagination.ItemTotal = models.PagesInfo.RowTotal;
            m_DataGridView.Reset();

            OnGoPaged(models);

            m_DataGridView.ClearDataStoreStatus();
            m_DataGridView.ClearSelectedStatus();

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
            LightModelFilter filter = new LightModelFilter(m_ModelT);

            foreach (ConditionElement item in m_FilterSearch)
            {
                filter.And(item.FieldName, item.FieldValue, item.Logic);
            }



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

            LModelElement modelElem = LightModel.GetLModelElement(m_ModelT);

            DbDecipher decipher = OpenDecipher();


            try
            {
                object model = Activator.CreateInstance(m_ModelT);

                LightModel lm = model as LightModel;

                Modify(lm, m_DataGridView);

                foreach (ConditionElement item in m_NewDefaultValues)
                {
                    LModelFieldElement fieldElem = modelElem.Fields[item.FieldName];

                    object value = ModelConvert.ChangeType(item.FieldValue, fieldElem.DBType, fieldElem.Mandatory);

                    LightModel.SetFieldValue(model, item.FieldName, value);
                }

                GoPageLast(decipher);



                OnAdding(model);

                decipher.InsertModel(model);

                OnAdded(model);


                m_DataGridView.Items.Add(model);
                m_DataGridView.Reset();

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

                return checkBox.Name;
            }

            return string.Empty;
        }



        private LightModelFilter GetFilter(int[] ids)
        {

            LModelElement modelElem = LightModel.GetLModelElement(m_ModelT);

            LightModelFilter filter = new LightModelFilter(m_ModelT);
            filter.And(modelElem.PrimaryKey, ids, HWQ.Entity.Filter.Logic.In);

            //foreach (ConditionElement item in m_FilterDelete)
            //{
            //    filter.And(item.FieldName, item.FieldValue, item.Logic);
            //}

            return filter;
        }

        public int[] GetCheckedIntList()
        {
            int[] ids;


            string dataKeys = this.m_DataGridView.DataKeys;
            string fixedKeys = this.m_DataGridView.FixedFields;

            int[] guids = null;

            if ("$P.guid" == dataKeys)
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
                DataSeletedStatus status = DataSeletedStatus.Parse(this.m_DataGridView.SelectedStatus);

                StringList idList = new StringList(status.Items.Count);
                StringList guidList = new StringList(status.Items.Count);

                foreach (DataSeletedItem item in status.Items)
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

        /// <summary>
        /// 验证删除的记录
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="guids"></param>
        /// <returns></returns>
        private bool ValidateDelete(out int[] ids, out int[] guids)
        {
            string dataKeys = this.m_DataGridView.DataKeys;
            string fixedKeys = this.m_DataGridView.FixedFields;

            guids = null;

            if (dataKeys == "$P.guid")
            {
                DataSeletedStatus seletedStatsu = DataSeletedStatus.Parse(this.m_DataGridView.SelectedStatus);

                IntList idList = new IntList(seletedStatsu.Items.Count);
                IntList guidList = new IntList(seletedStatsu.Items.Count);

                

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
            LModelElement modelElem = LightModel.GetLModelElement(m_ModelT);

            LightModelFilter filter = GetFilter(ids);

            filter.And(statusFieldName, srcStatusId, Logic.In);

            foreach (ConditionElement item in m_FilterChangeStatus)
            {
                filter.And(item.FieldName, item.FieldValue, item.Logic);
            }

            int n = 0;

            try
            {
                n = decipher.UpdateProps(filter, new object[] { statusFieldName, targetStatusId });

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

            GoPage();

            return n;
        }





        public virtual int ChangeStatus(string statusFieldName, Logic logic, int srcStatusId, int targetStatusId, bool autoTooltip)
        {
            if (!m_SecSave) { MiniHelper.Alert("记录已经上锁.您无权操作.!"); return 0; }

            int[] ids;
            int[] guids;

            bool valid = ValidateDelete(out ids, out guids);

            if (!valid)
            {
                return 0;
            }

            DbDecipher decipher = OpenDecipher();

            LightModelFilter filter = GetFilter(ids);

            filter.And(statusFieldName, srcStatusId, logic);

            foreach (ConditionElement item in m_FilterChangeStatus)
            {
                filter.And(item.FieldName, item.FieldValue, item.Logic);
            }

            int n = 0;

            try
            {
                n = decipher.UpdateProps(filter, new object[] { statusFieldName, targetStatusId });

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

            GoPage();

            return n;
        }

        public virtual int ChangeStatus(string statusFieldName, int srcStatusId, int targetStatusId)
        {
            return ChangeStatus(statusFieldName, Logic.Equality, srcStatusId, targetStatusId, false);
        }

        public virtual int ChangeStatus(string statusFieldName, Logic logic, int srcStatusId, int targetStatusId)
        {
            return ChangeStatus(statusFieldName, logic, srcStatusId, targetStatusId, false);
        }

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


            LModelElement modelElem = LightModel.GetLModelElement(m_ModelT);

            LightModelFilter filter = new LightModelFilter(m_ModelT);

            foreach (ConditionElement item in m_FilterDelete)
            {
                filter.And(item.FieldName, item.FieldValue, item.Logic);
            }



            DbDecipher decipher = OpenDecipher();

            int n;

            try
            {
                if (Deleting != null || Deleted != null)
                {
                    IList models = decipher.SelectModels(filter);

                    OnItemsDeleting(models);
                    n = decipher.DeleteModels(filter);
                    OnItemsDeleted(models);
                }
                else
                {
                    n = decipher.DeleteModels(filter);
                }

                MiniHelper.Tooltip("共删除 {0} 条记录", n);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MiniHelper.Tooltip("删除失败!");
            }

            GoPage();
        }

        public virtual void DeleteItems()
        {
            if (!m_SecDelete) { MiniHelper.Alert("记录已经上锁.您无权操作.!"); return; }

            int[] ids;
            int[] guids;

            bool valid = ValidateDelete(out ids, out guids);

            if (!valid) { return; }

            DbDecipher decipher = OpenDecipher();

            LightModelFilter filter = GetFilter(ids);

            foreach (ConditionElement item in m_FilterDelete)
            {
                filter.And(item.FieldName, item.FieldValue, item.Logic);
            }

            int n;

            try
            {
                if (Deleting != null || Deleted != null)
                {
                    IList models = decipher.SelectModels(filter);

                    OnItemsDeleting(models);
                    n = decipher.DeleteModels(filter);
                    OnItemsDeleted(models);
                }
                else
                {
                    n = decipher.DeleteModels(filter);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MiniHelper.Tooltip("删除失败!");
            }


            GoPage();
        }

        List<InputFieldError> m_ValidateErrors;

        /// <summary>
        /// 验证错误信息
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
        /// 验证单元格填写内容
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
            LModelElement modelElem = LightModel.GetLModelElement(m_ModelT);

            List<string> updateFields = new List<string>();

            List<InputFieldError> errors = new List<InputFieldError>();

            try
            {
                foreach (DataStoreRow row in dataStaus.Rows)
                {
                    if (row.State == DataStoreRowState.Modified)
                    {

                        object modelInstance = Activator.CreateInstance(m_ModelT);
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

                        ModelCancelEventArgs ea = new ModelCancelEventArgs(modelInstance);

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

            DataStoreStatus dataStaus = DataStoreStatus.Parse(json);
            LModelElement modelElem = LightModel.GetLModelElement(m_ModelT);

            List<string> updateFields = new List<string>();

            try
            {


                foreach (DataStoreRow row in dataStaus.Rows)
                {
                    if (row.State == DataStoreRowState.Modified)
                    {
                        object modelInstance = Activator.CreateInstance(m_ModelT);

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


                        ModelCancelEventArgs saveEA = new ModelCancelEventArgs(modelInstance, updateFields.ToArray());

                        OnSaving(saveEA);

                        m_SavingCancel = saveEA.Cancel;

                        if (saveEA.Cancel)
                        {
                            break;
                        }

                        decipher.UpdateModelProps(model, updateFields.ToArray());

                        OnSaved(modelInstance);
                    }
                    else if (row.State == DataStoreRowState.Added)
                    {
                        object modelInstance = Activator.CreateInstance(m_ModelT);

                        LightModel model = modelInstance as LightModel;

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


                        ModelCancelEventArgs saveEA = new ModelCancelEventArgs(modelInstance);

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
                        LightModelFilter filter = new LightModelFilter(m_ModelT);
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
        /// 初始化查询参数
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="group"></param>
        public void InitSearch(Control owner, string group)
        {
            LModelElement modelElem = LightModel.GetLModelElement(m_ModelT);
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
                            v = DateUtil.EndDay((string)item.Value);
                            item.Value = v;
                            FilterSearch(item.DBField, v, logic);
                        }
                    }
                    else if (logic == Logic.GreaterThan || logic == Logic.GreaterThanOrEqual)
                    {
                        DateTime v;

                        if (DateTime.TryParse((string)item.Value, out v))
                        {
                            v = DateUtil.StartDay((string)item.Value);
                            item.Value = v;
                            FilterSearch(item.DBField, v, logic);
                        }
                    }
                }
                else
                {
                    FilterSearch(item.DBField, item.Value, logic);
                }
            }
        }
    }
}
