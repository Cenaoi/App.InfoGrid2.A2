using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using EasyClick.Web.Mini2.Data;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 数据仓库事件名称
    /// </summary>
    public enum StoreEvent
    {
        Searching,

        CurrentChanged,

        Selecting,
        Selected,

        Updating,
        Updated,


        /// <summary>
        /// 批更新前
        /// </summary>
        BatchUpdating,
        /// <summary>
        /// 批更新
        /// </summary>
        BatchUpdated,

        Deleting,
        Deleted,

        /// <summary>
        /// 批删除前
        /// </summary>
        BatchDeleting,

        /// <summary>
        /// 批删除后
        /// </summary>
        BatchDeleted,

        Inserting,
        Inserted,

        SavingAll,
        SavedAll,

        PageChanged,

        PageLoaded,
        PageLoading




    }

    /// <summary>
    /// 可以取消翻一页
    /// </summary>
    public class CancelPageEventArags : CancelEventArgs
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; internal set; }

        /// <summary>
        /// T-SQL 的排序
        /// </summary>
        public string TSqlSort { get; internal set; }

        /// <summary>
        /// 可以取消翻一页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="tSqlSort">T-SQL 的排序</param>
        public CancelPageEventArags(int page, string tSqlSort)
        {
            this.Page = page;
            this.TSqlSort = tSqlSort;
        }

    }




    partial class Store
    {

        #region 事件定义

        /// <summary>
        /// 选择记录前触发的事件
        /// </summary>
        [Description("选择记录前触发的事件")]
        public event ObjectListCancelEventHandler Selecting;

        /// <summary>
        /// 选择记录后触发的事件
        /// </summary>
        [Description("选择记录后触发的事件")]
        public event ObjectListEventHandler Selected;

        /// <summary>
        /// 过滤前触发的事件
        /// </summary>
        [Description("过滤前触发的事件.只有在 Select 执行中,才有效果")]
        public event ObjectCancelEventHandler Filtering;

        /// <summary>
        /// 更新记录前触发的事件
        /// </summary>
        [Description("更新记录前触发的事件")]
        public event ObjectCancelEventHandler Updating;

        /// <summary>
        /// 更新记录后触发的事件
        /// </summary>
        [Description("更新记录后触发的事件")]
        public event ObjectEventHandler Updated;

        /// <summary>
        /// 选择记录后触发的事件
        /// </summary>
        [Description("批量更新记录前触发的事件")]
        public event ObjectListCancelEventHandler BatchUpdating;

        /// <summary>
        /// 选择记录后触发的事件
        /// </summary>
        [Description("批量更新记录后触发的事件")]
        public event ObjectListEventHandler BatchUpdated;

        /// <summary>
        /// 删除记录前触发的事件
        /// </summary>
        [Description("删除记录前触发的事件")]
        public event ObjectCancelEventHandler Deleting;

        /// <summary>
        /// 删除记录后触发的事件
        /// </summary>
        [Description("删除记录后触发的事件")]
        public event ObjectEventHandler Deleted;

        /// <summary>
        /// 选择记录后触发的事件
        /// </summary>
        [Description("批量删除记录前触发的事件")]
        public event ObjectListCancelEventHandler BatchDeleting;

        /// <summary>
        /// 选择记录后触发的事件
        /// </summary>
        [Description("批量删除记录后触发的事件")]
        public event ObjectListEventHandler BatchDeleted;

        /// <summary>
        /// 插入记录前触发的事件
        /// </summary>
        [Description("插入记录前触发的事件")]
        public event ObjectCancelEventHandler Inserting;
        /// <summary>
        /// 插入记录后触发的事件
        /// </summary>
        [Description("插入记录后触发的事件")]
        public event ObjectEventHandler Inserted;

        /// <summary>
        /// 保存全部记录前触发的事件
        /// </summary>
        [Description("保存全部记录前触发的事件")]
        public event ObjectListCancelEventHandler SavingAll;
        /// <summary>
        /// 保存全部记录后触发的事件
        /// </summary>
        [Description("保存全部记录后触发的事件")]
        public event ObjectListEventHandler SavedAll;

        /// <summary>
        /// 焦点记录发生变化
        /// </summary>
        [Description("焦点记录发生变化")]
        public event ObjectEventHandler CurrentChanged;

        /// <summary>
        /// 查询页触发的事件
        /// </summary>
        [Description("分页触发的事件")]
        public event EventHandler PageChanged;

        /// <summary>
        /// 页面加载数据后
        /// </summary>
        [Description("页面加载数据后")]
        public event ObjectListEventHandler PageLoaded;

        /// <summary>
        /// 页面加载数据前
        /// </summary>
        [Description("页面加载数据前")]
        public event CancelPageEventHandler PageLoading;


        /// <summary>
        /// 开始查询事件
        /// </summary>
        [Description("查询事件")]
        public event CancelEventHandler Searching;

        #endregion


        /// <summary>
        /// 判断事件是否有监听
        /// </summary>
        /// <param name="storeEvent">事件名</param>
        /// <returns></returns>
        public bool Has(StoreEvent storeEvent)
        {
            bool has = false;

            switch (storeEvent)
            {
                case StoreEvent.Selecting: has = (Selecting != null); break;
                case StoreEvent.Selected: has = (Selected != null); break;

                case StoreEvent.Updating: has = (Updating != null); break;
                case StoreEvent.Updated: has = (Updated != null); break;

                case StoreEvent.BatchUpdating: has = (BatchUpdating != null); break;
                case StoreEvent.BatchUpdated: has = (BatchUpdated != null); break;

                case StoreEvent.Deleting: has = (Deleting != null); break;
                case StoreEvent.Deleted: has = (Deleted != null); break;

                case StoreEvent.BatchDeleting: has = (BatchDeleting != null); break;
                case StoreEvent.BatchDeleted: has = (BatchDeleted != null); break;

                case StoreEvent.Inserting: has = (Inserting != null); break;
                case StoreEvent.Inserted: has = (Inserted != null); break;

                case StoreEvent.SavingAll: has = (SavingAll != null); break;
                case StoreEvent.SavedAll: has = (SavedAll != null); break;

                case StoreEvent.CurrentChanged: has = (CurrentChanged != null); break;

                case StoreEvent.PageLoaded: has = (PageLoaded != null); break;
                case StoreEvent.PageLoading: has = (PageLoading != null); break;
            }

            return has;
        }

        public bool OnSearching()
        {
            if (Searching == null) { return false; }

            CancelEventArgs ea = new CancelEventArgs(false);


            Searching(this, ea);

            return ea.Cancel;
        }

        /// <summary>
        /// 触发页面加载前事件
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="tSqlSort">排序字段</param>
        public bool OnPageLoading(int page, string tSqlSort)
        {
            if (PageLoading == null)
            {
                return false;
            }

            CancelPageEventArags ea = new CancelPageEventArags(page, tSqlSort);

            PageLoading(this, ea);

            return ea.Cancel;
        }

        /// <summary>
        /// 触发页面加载后事件
        /// </summary>
        /// <param name="objList"></param>
        public void OnPageLoaded(IList objList)
        {
            PageLoaded?.Invoke(this, new ObjectListEventArgs(objList));
        }


        /// <summary>
        /// 查询页触发的事件
        /// </summary>
        public void OnPageChanged()
        {
            PageChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 触发过滤条件
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public bool OnFiltering(object obj, ObjectEventItemCollection items)
        {
            if (Filtering == null) { return false; }

            ObjectCancelEventArgs ea = new ObjectCancelEventArgs(obj, this.DeleteRecycle);
            ea.Items = items;

            Filtering(this, ea);

            return ea.Cancel;
        }

        /// <summary>
        /// 触发当前焦点行事件
        /// </summary>
        public void PreCurrentChanged()
        {
            if (CurrentChanged == null) { return; }

            DataRecord record = this.GetDataCurrent();

            StoreEngine_OnInit();

            if (m_StoreEngine != null)
            {
                //继续往下传
                m_StoreEngine.PreCurrentChanged(record, null);
            }
            else
            {
                OnCurrentChanged(record, null, null);
            }

            //SetCurRecord(record);
        }

        /// <summary>
        /// 焦点行发生改变
        /// </summary>
        /// <param name="srcRecord">源数据记录</param>
        /// <param name="obj"></param>
        public void OnCurrentChanged(DataRecord srcRecord, object obj, ObjectEventItemCollection items)
        {
            if (CurrentChanged == null) { return; }

            ObjectEventArgs ea = new ObjectEventArgs(obj);
            ea.SrcRecord = srcRecord;
            ea.DeleteRecycle = this.DeleteRecycle;
            ea.Items = items;

            CurrentChanged(this, ea);
        }





        public void OnSelected(IList objList, ObjectEventItemCollection items)
        {
            if (Selected == null) { return; }

            ObjectListEventArgs ea = new ObjectListEventArgs(objList);
            ea.Items = items;

            Selected(this, ea);
        }


        public bool OnSelecting(IList objList, ObjectEventItemCollection items)
        {
            if (Selecting == null) { return false; }

            ObjectListCancelEventArgs e = new ObjectListCancelEventArgs(objList);
            e.Items = items;

            Selecting(this, e);

            return e.Cancel;
        }


        /// <summary>
        /// 插入数据过程中,触发的事件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool OnInserting(object obj, ObjectEventItemCollection items)
        {
            if (Inserting == null) { return false; }

            ObjectCancelEventArgs ea = new ObjectCancelEventArgs(obj, this.DeleteRecycle);
            ea.Items = items;

            Inserting(this, ea);

            return ea.Cancel;
        }

        /// <summary>
        /// 触发 OnInserting(...)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool OnPreInserting(object obj, ObjectEventItemCollection items)
        {
            return OnInserting(obj, items);
        }


        /// <summary>
        /// 插入数据后,触发的事件
        /// </summary>
        /// <param name="obj"></param>
        public ObjectEventArgs OnInserted(object obj, ObjectEventItemCollection items)
        {
            if (Inserted != null)
            {
                ObjectEventArgs ea = new ObjectEventArgs(obj);
                ea.Items = items;

                try
                {
                    Inserted(this, ea);
                }
                catch (Exception ex)
                {
                    ea.Exception = ex;
                }
            }
            return null;
        }


        /// <summary>
        /// 删除前,触发的事件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool OnDeleting(object obj, ObjectEventItemCollection items)
        {
            if (Deleting == null) { return false; }


            ObjectCancelEventArgs ea = new ObjectCancelEventArgs(obj, this.DeleteRecycle);
            ea.Items = items;

            Deleting(this, ea);

            return ea.Cancel;
        }


        /// <summary>
        /// 删除后,触发的事件
        /// </summary>
        /// <param name="data"></param>
        public ObjectEventArgs OnDeleted(object obj, bool deleteRecycle, ObjectEventItemCollection items)
        {
            if (Deleted != null)
            {
                ObjectEventArgs ea = new ObjectEventArgs(obj);
                ea.DeleteRecycle = deleteRecycle;
                ea.Items = items;

                try
                {
                    Deleted(this, ea);
                }
                catch (Exception ex)
                {
                    ea.Exception = ex;
                }

                return ea;
            }

            return null;
        }



        /// <summary>
        /// 更新过程中,触发的事件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool OnUpdating(DataRecord srcRecord, object obj, ObjectEventItemCollection items)
        {
            if (Updating == null) { return false; }

            ObjectCancelEventArgs ea = new ObjectCancelEventArgs(obj, this.DeleteRecycle);
            ea.SrcRecord = srcRecord;
            ea.Items = items;

            Updating(this, ea);

            return ea.Cancel;
        }


        /// <summary>
        /// 更新后,触发的事件
        /// </summary>
        /// <param name="data"></param>
        public ObjectEventArgs OnUpdated(DataRecord srcRecord, object obj, ObjectEventItemCollection items)
        {
            if (Updated != null)
            {

                ObjectEventArgs ea = new ObjectEventArgs(obj);
                ea.SrcRecord = srcRecord;
                ea.DeleteRecycle = this.DeleteRecycle;
                ea.Items = items;

                try
                {
                    Updated(this, ea);
                }
                catch (Exception ex)
                {
                    ea.Exception = ex;
                }

                return ea;
            }

            return null;
        }


        /// <summary>
        /// 触发数据后的事件
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public void OnBatchUpdated(IList objList, ObjectEventItemCollection items)
        {
            if (BatchUpdated == null) { return; }

            ObjectListEventArgs ea = new ObjectListEventArgs(objList);
            ea.Items = items;

            BatchUpdated(this, ea);
        }


        /// <summary>
        /// 触发数据后的事件
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public bool OnBatchUpdating(IList objList, ObjectEventItemCollection items)
        {
            if (BatchUpdating == null) { return false; }

            ObjectListCancelEventArgs ea = new ObjectListCancelEventArgs(objList);
            ea.Items = items;

            BatchUpdating(this, ea);

            return ea.Cancel;
        }


        /// <summary>
        /// 触发数据后的事件
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public void OnBatchDeleted(IList objList, ObjectEventItemCollection items)
        {
            if (BatchDeleted == null) { return; }

            ObjectListEventArgs ea = new ObjectListEventArgs(objList);
            ea.Items = items;

            BatchDeleted(this, ea);
        }


        /// <summary>
        /// 触发数据后的事件
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public bool OnBatchDeleting(IList objList, ObjectEventItemCollection items)
        {
            if (BatchDeleting == null) { return false; }

            ObjectListCancelEventArgs ea = new ObjectListCancelEventArgs(objList);
            ea.Items = items;

            BatchDeleting(this, ea);

            return ea.Cancel;
        }




        /// <summary>
        /// 触发数据后的事件
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public bool OnSavingAll(IList objList, ObjectEventItemCollection items)
        {
            if (SavingAll == null) { return false; }

            ObjectListCancelEventArgs ea = new ObjectListCancelEventArgs(objList);
            ea.Items = items;

            SavingAll(this, ea);

            return ea.Cancel;
        }



        /// <summary>
        /// 触发数据后的事件
        /// </summary>
        /// <param name="objList"></param>
        public void OnSavedAll(IList objList, string[] blemishFieldAll, ObjectEventItemCollection items)
        {
            if (SavedAll != null) { SavedAll(this, new ObjectListEventArgs(objList, blemishFieldAll) { Items = items }); }
        }


        /// <summary>
        /// 触发插入事件
        /// </summary>
        /// <param name="obj"></param>
        public void OnProInserted(object obj, ObjectEventItemCollection items)
        {
            ObjectEventArgs ea = OnInserted(obj, items);

            if (ea != null && ea.Exception != null && !ea.ExceptionHandled)
            {
                throw ea.Exception;
            }
        }

        /// <summary>
        /// 触发删除事件
        /// </summary>
        /// <param name="obj"></param>
        public void OnProDeleted(object obj, bool deleteRecycle, ObjectEventItemCollection items)
        {
            ObjectEventArgs ea = OnDeleted(obj, deleteRecycle, items);


            if (ea != null && ea.Exception != null && !ea.ExceptionHandled)
            {
                throw ea.Exception;
            }
        }

        /// <summary>
        /// 触发更新事件
        /// </summary>
        /// <param name="srcRecord"></param>
        /// <param name="obj"></param>
        public ObjectEventArgs OnProUpdated(DataRecord srcRecord, object obj, ObjectEventItemCollection items)
        {

            ObjectEventArgs ea = OnUpdated(srcRecord, obj, items);

            if (ea != null && ea.Exception != null && !ea.ExceptionHandled)
            {
                throw ea.Exception;
            }

            return ea;

        }

        public bool OnProUpdateing(DataRecord srcRecord, object obj, ObjectEventItemCollection items)
        {
            bool cancel = OnUpdating(srcRecord, obj, items);

            return cancel;
        }



    }
}
