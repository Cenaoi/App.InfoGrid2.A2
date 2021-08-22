using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.BizWeb.Affair
{
    /// <summary>
    /// 事务集合
    /// </summary>
    public class AffairInfoList:List<AffairInfo>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string m_Name;

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void Exe()
        {
            for (int i = 0; i < base.Count; i++)
            {
                AffairInfo ai = base[i];

                try
                {
                    ai.Exe();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }



    }


}
