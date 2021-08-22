using System;
using System.Collections.Generic;
using System.Text;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity.Decipher.LightDecipher;
using System.Data;
using HWQ.Entity.LightModels;
using EC5.SystemBoard;
using System.Web;

namespace App.BizCommon
{



    public class ModelAction : IBllAction, IDisposable
    {
        /// <summary>
        /// 数据操作的公共库
        /// </summary>
        internal DbDecipher m_Decipher;


        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {

        }


        public static DbDecipher OpenDecipher()
        {
            if (!App.Register.RegHelp.IsRegister()) throw new Exception("调用限制：未注册"); 

            string commonDecipher = "APP.COMMON_DECIPHER";

            HttpContext context = HttpContext.Current;

            DbDecipher decipher  = null;



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

            return decipher;
        }

        public virtual DbDecipher Decipher
        {
            get
            {
                if (!App.Register.RegHelp.IsRegister()) throw new Exception("调用限制：未注册"); 

                if (m_Decipher == null)
                {
                    string commonDecipher = "APP.COMMON_DECIPHER";

                    if (this.Context.Items[commonDecipher] == null)
                    {
                        EcUserState userState = EcContext.Current.User;

                        if (userState.ExpandPropertys.ContainsKey("DbDecipherName"))
                        {
                            userState.DbDecipherName = userState.ExpandPropertys["DbDecipherName"];
                        }

                        if (string.IsNullOrEmpty(userState.DbDecipherName))
                        {
                            m_Decipher = DbDecipherManager.GetDecipher();
                        }
                        else
                        {
                            m_Decipher = DbDecipherManager.GetDecipher(userState.DbDecipherName);
                        }

                        m_Decipher.Open();

                        this.Context.Items[commonDecipher] = m_Decipher;
                    }
                    else
                    {
                        m_Decipher = (DbDecipher)this.Context.Items[commonDecipher];
                    }
                }

                return m_Decipher;
            }
            set { m_Decipher = value; }
        }

        public virtual void Dispose()
        {
            m_Decipher = null;
        }

        /// <summary>
        /// 当前用户进程的上下文管理
        /// </summary>
        public virtual EcContext Context
        {
            get { return EcContext.Current; }
        }
    }
}
