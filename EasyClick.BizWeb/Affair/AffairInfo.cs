using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using HWQ.Entity.Decipher.LightDecipher;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Clouds;

namespace EasyClick.BizWeb.Affair
{
    /// <summary>
    /// 事务信息
    /// </summary>
    public class AffairInfo
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        string m_Name;

        string m_Description;

        string m_Uri;

        public string Uri
        {
            get { return m_Uri; }
            set { m_Uri = value; }
        }

        /// <summary>
        /// 事务名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }


        /// <summary>
        /// 执行命令
        /// </summary>
        public void Exe()
        {
            
            ICloudMethod method = CloudManager.GetMethod(m_Uri);

            if (method == null)
            {
                return;
            }

            try
            {
                method.Invoke();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

    }


}
