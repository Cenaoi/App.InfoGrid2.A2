using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.LCodeEngine
{
    /// <summary>
    /// 规则管理器
    /// </summary>
    public static class LcModelManager
    {
        static LcModelSorted m_Models = new LcModelSorted();

        public static LcModelSorted Models
        {
            get { return m_Models; }
            set { m_Models = value; }
        }


        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="code"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static object Exec(string code, HWQ.Entity.LightModels.LModel model)
        {
            
            EC5.LCodeEngine.LcFieldRule lcFRule = new LCodeEngine.LcFieldRule();
            lcFRule.Code = code;

            lcFRule.CodeParse();

            object result = null;

            Exception exception = null;

            try
            {
                result = lcFRule.Exec(model);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                lcFRule.Dispose();
            }

            if (exception != null) { throw exception; }

            return result;
        }

    }



}
