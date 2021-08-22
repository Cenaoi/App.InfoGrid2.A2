using EC5.BizLogger.Model;
using EC5.BizLogger.ModelXml;
using EC5.DbCascade;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace EC5.BizLogger
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public static class LogStepMgr
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Insert(BizDbStep step, string opText, string remark)
        {
            if (step == null)
            {
                return;
            }

            LOG_ACT actModel = new LOG_ACT();
            actModel.OP_TEXT = opText;
            //actModel.TIME_EXEC = timeExec;
            actModel.DATE_EXEC_FROM = DateTime.Now;
            //actModel.DATE_EXEC_TO = dateExecTo;
            actModel.REMARK = remark;
            
            InsertLogAct(step,actModel);
        }


        private static void BaseInsertLogAct(BizDbStep step, LOG_ACT actModel)
        {
            DbDecipher decipher = DbDecipherManager.GetDecipherOpen();

            try
            {
                LOG_ACT_OP opModel = ConvertAct(step);
                List<LOG_ACT_OPDATA> opDataModel = GetActOpList(step);

                decipher.InsertModel(actModel);

                opModel.LOG_ACT_ID = actModel.LOG_ACT_ID;

                decipher.InsertModel(opModel);

                foreach (var item in opDataModel)
                {
                    item.LOG_ACT_ID = actModel.LOG_ACT_ID;
                    item.LOG_ACT_OP_ID = opModel.LOG_ACT_OP_ID;
                    decipher.InsertModel(item);
                }


                InsertStep(decipher, opModel, step);

            }
            catch (Exception ex)
            {
                log.Error("记录联动失败", ex);
            }
            finally
            {
                decipher.Dispose();
            }
        }

        private static void InsertLogAct(BizDbStep step,LOG_ACT actModel)
        {


            if (Transaction.Current == null)
            {
                BaseInsertLogAct(step, actModel);
            }
            else
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    BaseInsertLogAct(step, actModel);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="par"></param>
        /// <param name="parStep"></param>
        private static void InsertStep(DbDecipher decipher, LOG_ACT_OP par, BizDbStep parStep)
        {
            if (parStep == null || !parStep.HasChild())
            {
                return;
            }

            foreach (var step in parStep.Childs)
            {
                LOG_ACT_OP opModel = ConvertAct(step);
                List<LOG_ACT_OPDATA> opDataModel = GetActOpList(step);

                //opModel.LOG_ACT_ID = par.LOG_ACT_ID;
                //opModel.PARENT_ID = par.LOG_ACT_OP_ID;
                //opModel.C_TABLE = par.C_TABLE;
                //opModel.C_DISPLAY = par.C_DISPLAY;


                opModel.LOG_ACT_ID = par.LOG_ACT_ID;
                opModel.PARENT_ID = par.LOG_ACT_OP_ID;
                //opModel.C_TABLE = step.C_TABLE;
                //opModel.C_DISPLAY = step.C_DISPLAY;

                decipher.InsertModel(opModel);

                foreach (var item in opDataModel)
                {
                    item.LOG_ACT_ID = par.LOG_ACT_ID;
                    item.LOG_ACT_OP_ID = opModel.LOG_ACT_OP_ID;

                    decipher.InsertModel(item);
                }

                InsertStep(decipher, opModel, step);
            }

        }

        private static List<LOG_ACT_OPDATA> GetActOpList(BizDbStep step)
        {
            List<LOG_ACT_OPDATA> items ;

            if (step.OpDataList == null)
            {
                items = new List<LOG_ACT_OPDATA>(0);
                return items;
            }

            items = new List<LOG_ACT_OPDATA>(step.OpDataList.Count);

            foreach (var item in step.OpDataList)
            {
                LOG_ACT_OPDATA opItem = ConvertAct(step.Table, step.TableText, item);


                items.Add(opItem);
            }

            return items;
        }


        private static LOG_ACT_OP ConvertAct(BizDbStep step)
        {

            LOG_ACT_OP model = new LOG_ACT_OP();

            model.ACT_CODE = step.StepType.ToString();
            model.ACTION_ID = step.ActionId;

            model.C_DISPLAY = step.TableText;

            if (!string.IsNullOrEmpty(step.Table))
            {
                LModelElement modelEelm = LightModel.GetLModelElement(step.Table);
                model.C_TABLE = step.Table;
                model.C_DISPLAY = modelEelm.Description;
            }


            model.DEPTH = step.Depth;
            model.RESULT_MESSAGE = step.ResultMessage;

            return model;
        }

        private static LOG_ACT_OPDATA ConvertAct(string table, string tableDisplay, LogOpData opData)
        {
            LOG_ACT_OPDATA model = new LOG_ACT_OPDATA();
            model.C_PK_VALUE = opData.TablePk;
            model.C_TABLE = table;
            model.C_DISPLAY = tableDisplay;


            LOG_OPDATA odXml = new LOG_OPDATA(opData.Count);

            foreach (var odItem in opData)
            {
                LOG_SET lSet = new LOG_SET();
                lSet.FIELD = odItem.Field;
                lSet.DISPLAY = odItem.FieldText;

                lSet.OP = odItem.Op;
                lSet.OP_ID = odItem.OpId;
                lSet.SRC_VALUE = odItem.SrcValue;
                lSet.TAR_VALUE = odItem.TarValue;

                odXml.Add(lSet);
            }

            model.OPDATE_XML = EC5.Utility.XmlUtil.Serialize(odXml);

            return model;
        }


        /// <summary>
        /// 清理联动日志
        /// </summary>
        /// <param name="lastTime"></param>
        public static void Clear(DateTime lastTime)
        {
            LightModelFilter filter = new LightModelFilter(typeof(LOG_ACT));
            filter.And("ROW_DATE_CREATE", lastTime, Logic.LessThan);

            LightModelFilter filterOp = new LightModelFilter(typeof(LOG_ACT_OP));
            filterOp.And("ROW_DATE_CREATE", lastTime, Logic.LessThan);

            LightModelFilter filterOpData = new LightModelFilter(typeof(LOG_ACT_OPDATA));
            filterOpData.And("ROW_DATE_CREATE", lastTime, Logic.LessThan);

            if (Transaction.Current == null)
            {
                DbDecipher decipher = DbDecipherManager.GetDecipherOpen();

                int n = 0;

                try
                {
                    n += decipher.DeleteModels(filter);
                    n += decipher.DeleteModels(filterOp);
                    n += decipher.DeleteModels(filterOpData);

                    log.DebugFormat("共清理 {0} 条联动日志。", n);
                }
                catch (Exception ex)
                {
                    log.Error("删除联动日志失败.", ex);
                }
                finally
                {
                    decipher.Dispose();
                }
            }
            else
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Suppress))
                {

                    DbDecipher decipher = DbDecipherManager.GetDecipherOpen();

                    int n = 0;

                    try
                    {
                        n += decipher.DeleteModels(filter);
                        n += decipher.DeleteModels(filterOp);
                        n += decipher.DeleteModels(filterOpData);

                        log.DebugFormat("共清理 {0} 条联动日志。", n);
                    }
                    catch (Exception ex)
                    {
                        log.Error("删除联动日志失败.", ex);
                    }
                    finally
                    {
                        decipher.Dispose();
                    }


                }
            }

        }

    }

}
