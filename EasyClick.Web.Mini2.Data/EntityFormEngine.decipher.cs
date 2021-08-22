using System;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.Decipher.LightDecipher;
using System.Data;
using System.Web;
using EC5.SystemBoard;
using EC5.Utility;

namespace EasyClick.Web.Mini2.Data
{
    partial class EntityFormEngine
    {
        DbDecipher m_Decipher = null;

        private DbDecipher Decipher
        {
            get
            {
                return OpenDecipher();
            }
        }

        /// <summary>
        /// 打开数据源
        /// </summary>
        /// <returns></returns>
        private DbDecipher OpenDecipher()
        {

            if (m_Decipher != null && m_Decipher.State == ConnectionState.Open)
            {
                return m_Decipher;
            }

            string commonDecipher = "APP.COMMON_DECIPHER";

            HttpContext context = HttpContext.Current;

            DbDecipher decipher = context.Items[commonDecipher] as DbDecipher;

            if (decipher == null)
            {

                EcUserState userState = EcContext.Current.User;

                if (userState.ExpandPropertys.ContainsKey("DbDecipherName"))
                {
                    userState.DbDecipherName = userState.ExpandPropertys["DbDecipherName"];
                }

                if (StringUtil.IsBlank(userState.DbDecipherName))
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
                if (decipher.State == ConnectionState.Closed)
                {
                    EcUserState userState = EcContext.Current.User;

                    if (userState.ExpandPropertys.ContainsKey("DbDecipherName"))
                    {
                        userState.DbDecipherName = userState.ExpandPropertys["DbDecipherName"];
                    }

                    if (StringUtil.IsBlank(userState.DbDecipherName))
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

            m_Decipher = decipher;

            return decipher;
        }
    }
}
