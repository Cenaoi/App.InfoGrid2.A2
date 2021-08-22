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
    partial class EntityStoreEngine
    {
        DbDecipher m_Decipher = null;

        protected DbDecipher Decipher
        {
            get
            {
                return OpenDecipher();
            }
        }


        const string COMMIN_DECIPHER = "APP.COMMON_DECIPHER";

        const string DB_DECIPHER_NAME = "DbDecipherName";


        protected DbDecipher GetDbDecipher()
        {
            EcUserState userState = EcContext.Current.User;

            Settings expands = userState.ExpandPropertys;
            
            DbDecipher decipher = null;

            if (expands.ContainsKey(DB_DECIPHER_NAME))
            {
                userState.DbDecipherName = expands[DB_DECIPHER_NAME];
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

            return decipher;
        }

        /// <summary>
        /// 打开数据源
        /// </summary>
        /// <returns></returns>
        protected DbDecipher OpenDecipher()
        {

            if (m_Decipher != null && m_Decipher.State == ConnectionState.Open)
            {
                return m_Decipher;
            }


            HttpContext context = HttpContext.Current;

            DbDecipher decipher = context.Items[COMMIN_DECIPHER] as DbDecipher;

            if (decipher == null)
            {
                decipher = GetDbDecipher();
                context.Items[COMMIN_DECIPHER] = decipher;
            }
            else
            {
                if (decipher.State == ConnectionState.Closed)
                {
                    decipher = GetDbDecipher();

                    context.Items[COMMIN_DECIPHER] = decipher;
                }
            }

            m_Decipher = decipher;

            return decipher;
        }
    }
}
