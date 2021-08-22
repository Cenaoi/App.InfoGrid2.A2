using App.InfoGrid2.Model;
using EC5.BizCoder;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.IG2.Core
{
    public class BizCodeLoader
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 加载定义
        /// </summary>
        public void Load()
        {
            BizCodeDefineList list = new BizCodeDefineList();

            LModelList<BIZ_CODE> models = null;

            DbDecipher decipher = DbDecipherManager.GetDecipherOpen();

            try
            {
                models = GetBizCodes(decipher);
            }
            catch (Exception ex)
            {
                log.Error("加载单据编码定义错误", ex);
            }
            finally
            {
                decipher.Dispose();
            }

            if (models == null)
            {
                return;
            }

            foreach (var model in models)
            {
                if (StringUtil.IsBlank(model.T_CODE))
                {
                    continue;
                }

                BizCodeDefine bc = new BizCodeDefine();
                bc.ID = model.BIZ_CODE_ID;
                bc.CodeMode = EnumUtil.Parse<BizCodeMode>(model.MODE_ID.ToString(), BizCodeMode.Auto);
                bc.CodePrefix = model.CODE_PRDFIX;
                bc.CodeSuffix = model.CODE_SUFFIX;
                bc.GroupName = model.GROUP_NAME;
                
                bc.NumStart = model.NUM_START;
                bc.NumEnd = model.NUM_END;
                bc.NumCur = model.NUM_CUR;
                bc.NumAdd = model.NUM_ADD;

                bc.TCode = model.T_CODE;
                bc.TFormat = model.T_FORMAT;

                list.Add(bc);
            }

            
            BizCodeMgr.SetCdoeDefineList(list);
            
        }

        private LModelList<BIZ_CODE> GetBizCodes(DbDecipher decipher)
        {
            LightModelFilter filter = new LightModelFilter(typeof(BIZ_CODE));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);


            LModelList<BIZ_CODE> models = decipher.SelectModels<BIZ_CODE>(filter);

            return models;

        }
    }
}
