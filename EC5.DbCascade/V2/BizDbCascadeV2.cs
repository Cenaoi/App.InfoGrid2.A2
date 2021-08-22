using System;
using System.Collections.Generic;
using System.Text;
using EC5.DbCascade.DbCascadeEngine;
using HWQ.Entity.LightModels;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity;
using EC5.Utility;
using System.ComponentModel;
using EC5.LcValueEngine;
using HWQ.Entity.LightModels.MemoryData;


namespace EC5.DbCascade.V2
{

    /// <summary>
    /// 业务联动数据操作
    /// </summary>
    public class BizDbCascadeV2:IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 动作名称

        /// <summary>
        /// 数据库删除操作
        /// </summary>
        public const string DELETE = "DELETE";

        /// <summary>
        /// 数据库更新操作
        /// </summary>
        public const string UPDATE = "UPDATE";

        /// <summary>
        /// 数据库插入操作
        /// </summary>
        public const string INSERT = "INSERT";

        /// <summary>
        /// 数据库全部操作
        /// </summary>
        public const string ALL = "ALL";

        #endregion

        /// <summary>
        /// 更新事件
        /// </summary>
        public event ObjectEventHandler Updating;

        /// <summary>
        /// 准备处理的对象.后进先出
        /// </summary>
        List<BizDbAction> m_ActionList = new List<BizDbAction>();

        int m_CurIndex = -1;


        /// <summary>
        /// 步骤路径
        /// </summary>
        BizDbStepPath m_StepPath = new BizDbStepPath();

        /// <summary>
        /// 根实体
        /// </summary>
        LModel m_RootModel = null;

        string m_RootTable;

        object m_RootPk = null;


        /// <summary>
        /// 根实体
        /// </summary>
        public LModel RootModel
        {
            get { return m_RootModel; }
            set
            {
                m_RootModel = value;

                if (value == null)
                {
                    m_RootTable = null;
                    m_RootPk = null;
                }
                else
                {
                    m_RootTable = m_RootModel.GetModelElement().DBTableName;
                    m_RootPk = m_RootModel.GetPk();
                }

            }
        }
        


        private DbOperate Convert(string opName)
        {
            DbOperate op;

            switch (opName)
            {
                case INSERT: op = DbOperate.Insert; break;
                case DELETE: op = DbOperate.Delete; break;
                case UPDATE: op = DbOperate.Update; break;
                default: op = DbOperate.None; break;
            }

            return op;
        }

        /// <summary>
        /// 触发更新事件
        /// </summary>
        /// <param name="model">实体</param>
        protected void OnUpdating(object model)
        {
            if (Updating != null)
            {
                Updating(this, new ObjectEventArgs(model));
            }
        }


        public void Push(BizDbAction act)
        {
            m_ActionList.Add(act);

            string opType = act.Op.ToString().ToUpper();


            foreach (var aItem in act.Items)
            {

                LModel model = aItem.Data as LModel;
                LModelElement modelElem = model.GetModelElement();

                BizDbStep step = new BizDbStep(opType, model);
                step.Table = modelElem.DBTableName;

                step.CreateLog();


                aItem.Step = step;

                m_StepPath.Root.Childs.Add(step);
            }


        }

        int m_MaxStep = 10000 * 100;

        /// <summary>
        /// 最大执行步数, 如果超过, 就当做死循环处理.
        /// </summary>
        public int MaxStep
        {
            get { return m_MaxStep; }
            set { m_MaxStep = value; }
        }

        /// <summary>
        /// 开始关联操作
        /// </summary>
        /// <param name="decipher">数据操作</param>
        public BizDbStepPath StartCascade(DbDecipher decipher)
        {
            bool isRead = true;

            


            while (isRead)
            {
                //如果发生错误，直接跳出
                if (m_StepPath.Errors.Count > 0)
                {
                    break;
                }

                

                try
                {
                    isRead = Read(decipher, m_StepPath);
                }
                catch (Exception ex)
                {
                    throw new Exception("联动发生过程发生错误。", ex);
                }


                if (m_LogActItems.Count >= m_MaxStep)
                {
                    throw new Exception(string.Format("可能联动发生死循环。执行步骤超过 {0} 个。", m_MaxStep));
                }
            }

            return m_StepPath;
        }

        /// <summary>
        /// 处理的联动数量
        /// </summary>
        List<BizDbActionItem> m_LogActItems = new List<BizDbActionItem>();

        /// <summary>
        /// 开始运动执行
        /// </summary>
        /// <returns></returns>
        public bool Read(DbDecipher decipher, BizDbStepPath stepPath)
        {

            if (m_ActionList.Count == 0)
            {
                return false;
            }

            if (m_ActionList.Count > m_MaxStep)
            {
                throw new Exception("发生死循环,嵌套层次太多。 层次:" + m_MaxStep);
            }


            if (m_CurIndex == -1)
            {
                m_CurIndex = 0;
            }

            BizDbAction act = m_ActionList[m_CurIndex];
            
            if (act.Items.Count == 0)
            {
                m_ActionList.RemoveAt(m_CurIndex);
                m_CurIndex = -1;

                return true;
            }
            
            LModelElement modelElem = null;
            
            BizDbActionItem item = act.Items.Dequeue();
            act.History.Enqueue(item);

            m_LogActItems.Add(item);

            if (act.Op == DbOperate.Update && item.DbccModel != null)
            {
                DbccModel bdcc = item.DbccModel;
                LModel srcModel = item.SrcModel as LModel;

                #region 处理左边条件

                BizDC_Left dcLeft = new BizDC_Left();

                BizDbAction opAction;

                bool success = false;


                try
                {
                    success = dcLeft.Update(decipher, bdcc, srcModel, stepPath, out opAction);
                }
                catch (Exception ex)
                {
                    throw new Exception("第" + stepPath.Cur.Depth + "层," +
                         string.Format("执行联动规则错误：规则ID={0}, 备注={1}, \n 左表={2}:{3}, 描述={4}, \n右表={5}:{6}, 描述={7}",
                         bdcc.ID, bdcc.Remark,
                         bdcc.L_ActCode, bdcc.L_Table, bdcc.L_Display,
                         bdcc.R_ActCode, bdcc.R_Table, bdcc.R_Display), ex);
                }

                #endregion

                //如果发生错误，直接跳出
                if (stepPath.Errors.Count > 0)
                {
                    return true;
                }

                if (!success || opAction == null || opAction.Items.Count == 0)
                {
                    return true;
                }


                //如果有数据，就放入队列
                m_ActionList.Insert(0, opAction);

                //创建日志
                LogStep(bdcc, opAction);


                return true;
            }
 


            object data = item.Data;


            if (data is LModel)
            {

                LModel model = (LModel)data;
                modelElem = model.GetModelElement();

                m_StepPath.Cur = item.Step;

                if (act.Op == DbOperate.Insert)
                {
                    ProModel_Insert(decipher, model, act);

                    if (act.DbccModel == null || act.DbccModel.AutoContinue)
                    {
                        ProDBccList(decipher, INSERT, modelElem, model, stepPath);
                    }
                    else
                    {
                        log.Debug($"联动 ID={act.DbccModel?.ID}, INSERT 到此为止, 不再触发联动.");
                    }
                }
                else if (act.Op == DbOperate.Update)
                {
                    ProModel_Update(decipher, model, act);

                    if (act.DbccModel == null || act.DbccModel.AutoContinue)
                    {
                        ProDBccList(decipher, UPDATE, modelElem, model, stepPath);
                    }
                    else
                    {
                        log.Debug($"联动 ID={act.DbccModel?.ID}, UPDATE 到此为止, 不再触发联动.");
                    }
                }
                else if (act.Op == DbOperate.Delete)
                {
                    if (act.Shelling)
                    {
                        decipher.DeleteModel(model);
                    }


                    if (act.DbccModel == null || act.DbccModel.AutoContinue)
                    {
                        ProDBccList(decipher, DELETE, modelElem, model, stepPath);
                    }
                    else
                    {
                        log.Debug($"联动 ID={act.DbccModel?.ID}, DELETE 到此为止, 不再触发联动.");
                    }
                }

            }
            else
            {
                log.Error("联动肯定有问题，不然不会有这东东");
            }



            return true;
        }

        /// <summary>
        /// 填充更新的数据
        /// </summary>
        protected void FullForUpdate(LModel model)
        {
            if (model == null)
            {
                return;
            }

            LModelElement modelElem = model.GetModelElement();

            LModelFieldElement rowUpdate;

            if (modelElem.TryGetField("ROW_DATE_UPDATE", out rowUpdate))
            {
                if (!model.GetBlemish("ROW_DATE_UPDATE"))
                {
                    model.SetValue(rowUpdate, DateTime.Now);
                }
            }



        }


        private void ProModel_Insert(DbDecipher decipher, LModel model, BizDbAction act)
        {
            if (act.Shelling)
            {
                try
                {
                    decipher.InsertModel(model);

                }
                catch (Exception ex)
                {
                    Debug_Model(model, act);

                    throw new Exception("联动插入记录失败。", ex);
                }
            }
        }

        private void ProModel_Update(DbDecipher decipher, LModel model, BizDbAction act)
        {
            if (act.Shelling)
            {
                try
                {
                    FullForUpdate(model);

                    decipher.UpdateModel(model, true);

                    #region 增加修改主实体

                    string curTable = model.GetModelElement().DBTableName;
                    object curPk = model.GetPk();

                    if (m_RootTable == curTable && BizDbMath.LogicAB(m_RootPk, Logic.Equality, curPk))
                    {
                        string[] cFields = LightModel.GetBlemishPropNames(model);

                        foreach (var cField in cFields)
                        {
                            m_RootModel[cField] = model[cField];
                        }
                    }


                    #endregion
                }
                catch (Exception ex)
                {
                    Debug_Model(model, act);

                    throw new Exception("联动更新记录失败。", ex);
                }
            }
        }


        /// <summary>
        /// 处理被监听的对象集 
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="actCode"></param>
        /// <param name="modelElem"></param>
        /// <param name="rightModel"></param>
        /// <param name="stepPath"></param>
        private void ProDBccList(DbDecipher decipher, 
            string actCode, LModelElement modelElem, LModel rightModel, 
            BizDbStepPath stepPath)
        {
            string tableName = modelElem.DBTableName;

            DbccModelCollection items = DbccManager.Acts.GetModels(tableName, actCode);

            if (items == null)
            {
                return;
            }


            int curIndex = m_CurIndex;

            bool proSuccess = false;

            foreach (DbccModel item in items)
            {
                #region 处理右边条件

                BizDC_Right dcRight = new BizDC_Right();

                try
                {
                    proSuccess = dcRight.ProDBccItem(decipher, item, rightModel, stepPath);
                }
                catch (Exception ex)
                {
                    throw new Exception("右边错误，第" + stepPath.Cur.Depth + "层," + 
                        string.Format("执行联动规则错误：规则ID={0}, 备注={1}, \n 左表={2}:{3}, 描述={4}, \n右表={5}:{6}, 描述={7}",
                        item.ID, item.Remark, 
                        item.L_ActCode, item.L_Table, item.L_Display,
                        item.R_ActCode, item.R_Table, item.R_Display),ex); 
                }

                if (!proSuccess)
                {
                    continue;
                }

                #endregion

                #region 处理左边条件

                BizDC_Left dcLeft = new BizDC_Left();

                BizDbAction opAction;

                bool success = false;

                try
                {
                    success = dcLeft.ProDBccItem_Left(decipher, item, rightModel, stepPath, out opAction);
                }
                catch (Exception ex)
                {
                    throw new Exception("第" + stepPath.Cur.Depth + "层," +
                        string.Format("执行联动规则错误：规则ID={0}, 备注={1}, \n 左表={2}:{3}, 描述={4}, \n右表={5}:{6}, 描述={7}",
                        item.ID, item.Remark,
                        item.L_ActCode, item.L_Table, item.L_Display,
                        item.R_ActCode, item.R_Table, item.R_Display), ex);
                }

                #endregion

                //如果发生错误，直接跳出
                if (stepPath.Errors.Count > 0)
                {
                    break;
                }

                if (!success || opAction == null || opAction.Items.Count == 0)
                {
                    continue;
                }
                

                //如果有数据，就放入队列
                m_ActionList.Insert(++curIndex,opAction);

                string leftActCode = item.L_ActCode.ToUpper();

                if (leftActCode != BizDbCascade.UPDATE)
                {
                    //创建日志
                    LogStep(item, opAction);
                }

                
            }
        }




        /// <summary>
        /// 创建日志
        /// </summary>
        /// <param name="item"></param>
        /// <param name="opAction"></param>
        private void LogStep(DbccModel item,BizDbAction opAction )
        {
            if (opAction == null)
            {
                return;
            }

            string opType = opAction.Op.ToString().ToUpper();

            foreach (var aItem in opAction.Items)
            {

                LModel model = aItem.Data as LModel;

                BizDbStep step = new BizDbStep(opType, model);

                LModelElement modelElem2 = model.GetModelElement();

                step.Table = modelElem2.DBTableName;
                

                step.ActionId = item.ID;

                step.CreateLog();

                aItem.Step = step;

                m_StepPath.Cur.Childs.Add(step);
            }
        }

        private void ProDbccItem()
        {

        }


        private void Debug_Models(List<LModel> models,BizDbAction act)
        {

            foreach (var model in models)
            {
                Debug_Model(model,act);
            }
        }

        private void Debug_Model(LModel model, BizDbAction act)
        {
            LModelElement modelElem = model.GetModelElement();

            log.DebugFormat("联动ID:{0}, 操作:{1}", act.ActionID, act.Op);
            log.DebugFormat("数据表：{0}", modelElem.DBTableName);

            foreach (var fieldElem in modelElem.Fields)
            {
                object value = model[fieldElem];

                if (value == null)
                {
                    continue;
                }

                LMFieldDBTypes dbType = ModelConvert.ToDbType(value.GetType());

                if (dbType != fieldElem.DBType)
                {
                    log.DebugFormat("错误类型的字段: {0}={3}, 数据类型:{1}, 要求类型:{2}",
                        fieldElem.DBField, dbType, fieldElem.DBType, value);
                }

            }

        }

        public void Dispose()
        {
            if (m_ActionList != null)
            {
                m_ActionList.Clear();
                m_ActionList = null;
            }


            if (m_StepPath != null)
            {
                m_StepPath = null;
            }

            GC.SuppressFinalize(this);
        }
    }


}
