using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.LcValueEngine;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.IG2.Core.LcValue
{
    public class LCodeValueLoader
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void Init()
        {
            using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
            {
                try
                {
                    InitData(decipher);
                }
                catch (Exception ex)
                {
                    log.Error("加载简单流程失败", ex);
                }
            }
        }

        public void InitData(DbDecipher decipher)
        {
            LightModelFilter filter = new LightModelFilter(typeof(IG2_VALUE_TABLE));
            filter.And("ENABLED", true);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            var tables = decipher.SelectModels<IG2_VALUE_TABLE>(filter);

            List<LcValueTable> vTableList = new List<LcValueTable>();

            foreach (var table in tables)
            {
                if (StringUtil.IsBlank(table.TABLE_NAME))
                {
                    continue;
                }


                LcValueTable vTable = Convent_Table(table);

                var ifList = decipher.SelectModels<IG2_VALUE_IF>("IG2_VALUE_TABLE_ID={0} AND ROW_SID>=0", table.IG2_VALUE_TABLE_ID);

                foreach (var ifItem in ifList)
                {
                    if (StringUtil.IsBlank(ifItem.F_NAME))
                    {
                        continue;
                    }


                    var thenList = decipher.SelectModels<IG2_VALUE_THEN>("IG2_VALUE_TABLE_ID={0} AND IG2_VALUE_IF_ID={1} AND ROW_SID>=0",
                        table.IG2_VALUE_TABLE_ID,ifItem.IG2_VALUE_IF_ID);

                    LcValueIf vIF = Convent_If(ifItem);

                    foreach (var thenItem in thenList)
                    {
                        if (StringUtil.IsBlank(thenItem.F_NAME))
                        {
                            continue;
                        }


                        LcValueThen vThen = Convent_Then(thenItem);

                        vIF.ThenList.Add(vThen);
                    }

                    vTable.IfList.Add(vIF);

                }

                vTableList.Add(vTable);
    
            }




            LcValueManager.Table.Clear();
            
            foreach (var item in vTableList)
            {
                LcValueManager.Table.Add(item);
            }



        }


        private LcValueThen Convent_Then(IG2_VALUE_THEN model)
        {
            LcValueThen item = new LcValueThen();

            item.ID = model.IG2_VALUE_TABLE_ID;
            item.Field = model.F_NAME;
            item.Display = model.F_DISPAY;
            item.ValueTo = model.VALUE_TO;

            if ("IF_NULL".Equals(model.F_LOGIC, StringComparison.OrdinalIgnoreCase))
            {
                item.Logic = LcValueThenLogic.IF_NULL;
            }
            else
            {
                item.Logic = LcValueThenLogic.EVERY;
            }

            return item;
        }


        private LcValueIf Convent_If(IG2_VALUE_IF model)
        {
            LcValueIf item = new LcValueIf();
            item.ID = model.IG2_VALUE_IF_ID;
            item.Field = model.F_NAME;
            item.Display = model.F_DISPAY;
            item.Logic = ToLogic(model.F_LOGIC);
            item.ValueTo = model.VALUE_TO;
            item.ValueFrom = model.VALUE_FROM;

            return item;
        }


        private LcValueTable Convent_Table(IG2_VALUE_TABLE model)
        {
            LcValueTable item = new LcValueTable();

            item.ID = model.IG2_VALUE_TABLE_ID;
            item.Table = model.TABLE_NAME;
            item.Display = model.TABLE_DISPAY;
            item.Enabled = model.ENABLED;
            item.Remark = model.REMARKS;

            return item;

        }

        /// <summary>
        /// 转换逻辑运算符号
        /// </summary>
        /// <param name="logicStr"></param>
        /// <returns></returns>
        public Logic ToLogic(string logicStr)
        {
            if (String.IsNullOrEmpty(logicStr))
            {
                return Logic.Equality;
            }

            logicStr = logicStr.Trim().ToLower();

            Logic c;

            switch (logicStr)
            {
                case "==":
                case "=": c = Logic.Equality; break;
                case ">": c = Logic.GreaterThan; break;
                case ">=": c = Logic.GreaterThanOrEqual; break;
                case "<": c = Logic.LessThan; break;
                case "<=": c = Logic.LessThanOrEqual; break;
                case "!=":
                case "<>": c = Logic.Inequality; break;
                case "in": c = Logic.In; break;
                case "like": c = Logic.Like; break;
                case "noin": c = Logic.NotIn; break;
                case "nolike": c = Logic.NotLike; break;
                case "leftlike": c = Logic.LeftLike; break;
                case "rightlike": c = Logic.RightLike; break;

                default: c = Logic.Equality; break;
            }

            return c;
        }




    }




}
