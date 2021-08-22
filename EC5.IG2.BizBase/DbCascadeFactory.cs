using App.BizCommon;
using EasyClick.Web.Mini2;
using EC5.DbCascade;
using EC5.DbCascade.V2;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;

namespace EC5.IG2.BizBase
{

    /// <summary>
    /// 执行结束
    /// </summary>
    public class DbCascadeEventArges : EventArgs
    {
        BizDbStep[] m_Steps;

        string m_OpText;

        string m_Remark;

        public DbCascadeEventArges()
        {
        }

        public DbCascadeEventArges(BizDbStep[] steps, string opText, string remark)
        {
            m_Steps = steps;
            m_OpText = opText;
            m_Remark = remark;
        }

        public BizDbStep[] Steps
        {
            get { return m_Steps; }
        }

        public string OpText
        {
            get { return m_OpText; }
        }

        public string Remark
        {
            get { return m_Remark; }
        }
    }

    /// <summary>
    /// 数据库联动操作
    /// </summary>
    public class DbCascadeFactory
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 执行结束触发的事件
        /// </summary>
        public event EventHandler<DbCascadeEventArges> ExecEnd;

        /// <summary>
        /// 采用新联动引擎 V2
        /// </summary>
        bool m_IsV2 = true;

        public DbDecipher Decipher { get; set; }

        public DbCascadeFactory()
        {

        }

        public DbCascadeFactory(DbDecipher decipher)
        {
            this.Decipher = decipher;
        }

        /// <summary>
        /// 触发执行结束
        /// </summary>
        /// <param name="e"></param>
        private void OnExecEnd(DbCascadeEventArges e)
        {
            if (ExecEnd != null) { ExecEnd(this, e); }
        }


        /// <summary>
        /// 绑定数据仓库
        /// </summary>
        /// <param name="store"></param>
        public void BindStore(Store store)
        {
            store.TranEnabled = true;

            store.Updated += store_Updated;
            store.Deleted += store_Deleted;
            store.Inserted += store_Inserted;

        }

        private void ShowErrorMsg(BizDbStepPath stepPath)
        {
            if (stepPath.Errors.Count == 0)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            foreach (var item in stepPath.Errors)
            {

                sb.Append(item).Append("<br/>");
            }

            if (sb != null)
            {
                MessageBox.Alert(sb.ToString());
            }

        }

        void store_Inserted(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            Store store = (Store)sender;

            LModel model = e.Object as LModel;


            DbDecipher storeDecipher = e.Items["decipher"] as DbDecipher;
            this.Decipher = storeDecipher;

            BizDbStepPath items = Inserted(store, model);

            ShowErrorMsg(items);

            if (items.Errors.Count > 0)
            {
                e.Exception = new Exception("阻止联动执行:" + items.Errors[0]);
            }

            e.ExceptionHandled = true;
        }

        void store_Deleted(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            Store store = (Store)sender;

            DbDecipher storeDecipher = e.Items["decipher"] as DbDecipher;
            this.Decipher = storeDecipher;


            LModel model = e.Object as LModel;


            BizDbStepPath items;

            if (e.DeleteRecycle)
            {
                items = this.Updated(store, model);
            }
            else
            {
                items = this.Deleted(store, model);
            }

            ShowErrorMsg(items);


            if (items.Errors.Count > 0)
            {
                e.Exception = new Exception("阻止联动执行:" + items.Errors[0]);
            }

            e.ExceptionHandled = true;
        }

        void store_Updated(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            LModel model = e.Object as LModel;

            //联动操作，临时只针对 LModel 对象进行处理
            if (model == null)
            {
                return;
            }

            DbDecipher storeDecipher = e.Items["decipher"] as DbDecipher;
            this.Decipher = storeDecipher;

            BizDbStepPath items = Updated((Store)sender, model);

            ShowErrorMsg(items);


            if (items.Errors.Count > 0)
            {
                e.Exception = new Exception("阻止联动执行:" + items.Errors[0]);
            }

            e.ExceptionHandled = true;
        }


        /// <summary>
        /// (注：事务又外围控制，不要在这代码里面加事务.)
        /// </summary>
        /// <param name="store"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BizDbStepPath Inserted(Store store, LModel model)
        {
            if (model == null)
            {
                return null;
            }

            DbDecipher decipher = this.Decipher ?? ModelAction.OpenDecipher();
            BizDbStep step = null;
            BizDbStepPath stepPath = null;

            if (m_IsV2)
            {
                BizDbCascadeV2 bizDb = new BizDbCascadeV2();


                BizDbAction act = new BizDbAction(DbOperate.Insert, model);
                act.Shelling = false;

                bizDb.Push(act);

                try
                {
                    stepPath = bizDb.StartCascade(decipher);
                }
                catch (Exception ex)
                {
                    throw new Exception("执行“创建”联动发生错误。", ex);
                }

                step = stepPath.Root;
            }
            else
            {
                BizDbCascade bizDb = new BizDbCascade();

                step = bizDb.StartCascade(decipher, BizDbCascade.INSERT, model);
                step.Table = model.GetModelElement().DBTableName;
            }


            BizDbStep[] items = step.ToArray();

            if (stepPath.Errors.Count == 0)
            {
                SyncStoreData(store, items);
            }

            DbCascadeEventArges ea = new DbCascadeEventArges(items, "新建操作", "");
            OnExecEnd(ea);


            return stepPath;
        }




        /// <summary>
        /// (注：事务由外围控制，不要在这代码里面加事务.)
        /// </summary>
        /// <param name="store"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BizDbStepPath Deleted(Store store, LModel model)
        {
            if (model == null)
            {
                return null;
            }



            DbDecipher decipher = this.Decipher ?? ModelAction.OpenDecipher();


            BizDbStep step = null;
            BizDbStepPath stepPath = null;


            if (m_IsV2)
            {
                BizDbCascadeV2 bizDb = new BizDbCascadeV2();


                if (store.DeleteRecycle)
                {
                    BizDbAction act = new BizDbAction(DbOperate.Update, model);
                    act.Shelling = false;

                    bizDb.Push(act);
                }
                else
                {

                    BizDbAction act = new BizDbAction(DbOperate.Delete, model);
                    act.Shelling = false;

                    bizDb.Push(act);
                }

                try
                {
                    stepPath = bizDb.StartCascade(decipher);
                }
                catch (Exception ex)
                {
                    throw new Exception("执行“删除”联动发生错误。", ex);
                }

                step = stepPath.Root;
            }
            else
            {
                BizDbCascade bizDb = new BizDbCascade();

                if (store.DeleteRecycle)
                {
                    step = bizDb.StartCascade(decipher, BizDbCascade.UPDATE, model);
                }
                else
                {
                    step = bizDb.StartCascade(decipher, BizDbCascade.DELETE, model);
                }
            }


            step.Table = model.GetModelElement().DBTableName;

            BizDbStep[] items = step.ToArray();

            if (stepPath.Errors.Count == 0)
            {
                SyncStoreData(store, items);
            }

            DbCascadeEventArges ea = new DbCascadeEventArges(items, "删除操作", "");
            OnExecEnd(ea);

            return stepPath;
        }


        /// <summary>
        /// (注：事务又外围控制，不要在这代码里面加事务.)
        /// </summary>
        /// <param name="store"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BizDbStepPath Updated(Store store, LModel model)
        {
            if (model == null)
            {
                return null;
            }

            DbDecipher decipher = this.Decipher ?? ModelAction.OpenDecipher();

            BizDbStepPath stepPath = null;
            BizDbStep step = null;


            #region 联动引擎 v2

            BizDbCascadeV2 bizDb = new BizDbCascadeV2();
            bizDb.Updating += bizDb_Updating;

            BizDbAction act = new BizDbAction(DbOperate.Update, model);
            act.Shelling = false;

            bizDb.Push(act);



            bizDb.RootModel = model;

            try
            {
                stepPath = bizDb.StartCascade(decipher);
            }
            catch (Exception ex)
            {
                throw new Exception("执行“更新”联动发生错误。", ex);
            }

            step = stepPath.Root;

            #endregion


            step.Table = model.GetModelElement().DBTableName;

            BizDbStep[] items = step.ToArray();

            if (stepPath.Errors.Count == 0)
            {
                SyncStoreData(store, items);
            }

            DbCascadeEventArges ea = new DbCascadeEventArges(items, "更新操作", "");
            OnExecEnd(ea);

            return stepPath;
        }

        void bizDb_Updating(object sender, EC5.DbCascade.ObjectEventArgs e)
        {
            LModel model = e.Object as LModel;

            if (model != null)
            {
                LCodeFactory lCodeFactory = new LCodeFactory();

                string[] updateFields = lCodeFactory.ExecLCode(null, null, model);

                LCodeValueFactory lcvFactiry = new LCodeValueFactory();
                lcvFactiry.ExecLCode(null, null, model);



                #region 二次联动操作

                //DbDecipher decipher = ModelAction.OpenDecipher();

                //BizDbCascade bizDb = new BizDbCascade();
                //bizDb.Updating += new EC5.DbCascade.ObjectEventHandler(bizDb_Updating);

                //BizDbStep step = bizDb.StartCascade(decipher, BizDbCascade.UPDATE, model);

                #endregion

            }
        }



        /// <summary>
        /// 同步当前页面的数据仓库数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="items"></param>
        private void SyncStoreData(Store sender, BizDbStep[] stepList)
        {
            if (sender == null)
            {
                return;
            }

            //获取当前页面的所有 store 

            WidgetControl owner = GetParent(sender);

            List<Store> stores = FindStores(owner);

            //删除触发源的
            //stores.Remove(sender);

            StoreGroup storeGroup = ToGroup(stores);

            StoreList sList = null;

            foreach (BizDbStep step in stepList)
            {

                if (string.IsNullOrEmpty(step.Table))
                {
                    continue;
                }

                if (!storeGroup.TryGetValue(step.Table, out sList))
                {
                    continue;
                }


                foreach (Store curStore in sList)
                {
                    StoreUpdateValue(curStore, step);
                }
            }

        }


        private void StoreUpdateValue(Store store, BizDbStep step)
        {
            string pk;
            string[] fields;

            string[] updateFields = new string[0];

            foreach (LModel model in step.Models)
            {
                pk = model.GetPk().ToString();

                fields = LightModel.GetBlemishPropNames(model);

                if (fields == null)
                {
                    continue;
                }

                SModel values = new SModel();

                foreach (string field in fields)
                {
                    if (StringUtil.IsBlank(field)) { continue; }

                    object value = model[field];

                    values[field] = value;

                    store.SetRecordValue(pk, field, value);
                }


                updateFields = ArrayUtil.Union(updateFields, fields);
            }


            if (store.HasSummaryField() && updateFields.Length > 0)
            {
                IStoreEngine sEngine = store.StoreEngine;
                string dbField;

                foreach (SummaryField sField in store.SummaryFields)
                {
                    dbField = sField.DataField;

                    if (!ArrayUtil.Exist(updateFields, dbField))
                    {
                        continue;
                    }

                    decimal value = sEngine.GetSummary(dbField, sField.SummaryType, sField.Filter);

                    store.SetSummary(dbField, value);
                }
            }


        }





        private StoreGroup ToGroup(List<Store> items)
        {
            StoreGroup groups = new StoreGroup();

            StoreList list = null;

            foreach (Store store in items)
            {
                if (!groups.TryGetValue(store.Model, out list))
                {
                    list = new StoreList();
                    groups.Add(store.Model, list);
                }

                list.Add(store);
            }

            return groups;
        }

        /// <summary>
        /// 数据仓库的分组类
        /// </summary>
        class StoreGroup : SortedList<string, StoreList>
        {

        }

        /// <summary>
        /// 数据仓库的集合类
        /// </summary>
        class StoreList : List<Store>
        {

        }


        /// <summary>
        /// 查找全部 Store 
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        private List<Store> FindStores(WidgetControl owner)
        {
            List<Store> items = new List<Store>();

            FindStore(owner, items);

            return items;
        }

        private void FindStore(Control parent, List<Store> items)
        {
            if (!parent.HasControls())
            {
                return;
            }

            foreach (Control con in parent.Controls)
            {
                if (con is Store)
                {
                    items.Add((Store)con);
                    continue;
                }

                FindStore(con, items);
            }

        }


        private WidgetControl GetParent(Store store)
        {
            WidgetControl wCon = null;

            System.Web.UI.Control con = store;

            for (int i = 0; i < 99; i++)
            {
                con = con.Parent;

                if (con is WidgetControl)
                {
                    wCon = (WidgetControl)con;
                    break;
                }
            }

            return wCon;
        }

    }




}
