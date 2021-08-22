using System;
using System.Collections.Generic;
using System.Web;
using EasyClick.Web.Mini2;
using App.InfoGrid2.Model.DataSet;
using App.InfoGrid2.Model;
using EC5.Utility;
using App.InfoGrid2.Model.JsonModel;
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using EC5.SystemBoard;
using App.InfoGrid2.Bll.Sec;
using System.Data;
using App.InfoGrid2.Model.SecModels;
using System.Text;

namespace EC5.IG2.Core.UI
{
    /// <summary>
    /// 权限数据
    /// </summary>
    public class M2SecurityDataFactory
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);





        /// <summary>
        /// 设置归属权限
        /// </summary>
        /// <param name="uiStore"></param>
        public void BindStore(Store uiStore)
        {
            UserSecritySet userSecSet = SecFunMgr.GetUserSecuritySet();

            
            EcContext context = EcContext.Current;
            EcUserState userState = context.User;

            if (userState == null)
            {
                log.Debug("EcUserState 用户对象不存在");   
                return;
            }


            if ( userState.Roles.Exist(IG2Param.Role.BUILDER))
            {
                return;
            }



            LModelElement modelElem = LightModel.GetLModelElement(uiStore.Model);

            if (modelElem == null)
            {
                log.ErrorFormat("实体“{0}”对象不存在", uiStore.Model);
                return;
            }

            if (userSecSet == null)
            {
                log.Debug("UserSecritySet 安全数据集对象不存在.可能是'设计师'模式" );
            }


            //如果是虚拟用户，没有登陆的用户，给予不存在的数据
            if (userSecSet == null || userState.IsVirtual)
            {
                if (modelElem.HasField("BIZ_USER_CODE"))
                {
                    Param bizUserCode_Filter = new Param("BIZ_USER_CODE");
                    bizUserCode_Filter.DbType = DbType.String;
                    bizUserCode_Filter.DefaultValue = "0000000000";
                    bizUserCode_Filter.Logic = "in";

                    uiStore.FilterParams.Add(bizUserCode_Filter);
                }


                if (modelElem.HasField("BIZ_ORG_CODE"))
                {
                    Param bizOrgCode_Filter = new Param("BIZ_ORG_CODE");
                    bizOrgCode_Filter.DbType = DbType.String;
                    bizOrgCode_Filter.DefaultValue = "0000000000";
                    bizOrgCode_Filter.Logic = "in";

                    uiStore.FilterParams.Add(bizOrgCode_Filter);
                }

                return;
            }


            //ARR 权限控制模式.0-没有，1-角色，2-用户

            if (userSecSet.ArrModeId == 1)
            {
                //1-角色模式

                if (!modelElem.HasField("BIZ_ORG_CODE"))
                {
                    return;
                }

                bool isAll = ArrayUtil.Exist(userSecSet.ArrRoleCode, "*");

                if (isAll)
                {
                    return;
                }

                Param bizOrgCode_Filter = new Param("BIZ_ORG_CODE");
                bizOrgCode_Filter.DbType = DbType.String;
                bizOrgCode_Filter.SetInnerValue(userSecSet.ArrRoleCode);
                bizOrgCode_Filter.Logic = "in";

                uiStore.FilterParams.Add(bizOrgCode_Filter);
            }
            else if (userSecSet.ArrModeId == 2)
            {
                //1-用户模式

                if (!modelElem.HasField("BIZ_USER_CODE"))
                {
                    return;
                }

                bool isAll = ArrayUtil.Exist(userSecSet.ArrUserCode, "*");

                if (isAll)
                {
                    return;
                }

                Param bizUserCode_Filter = new Param("BIZ_USER_CODE");
                bizUserCode_Filter.DbType = DbType.String;
                bizUserCode_Filter.SetInnerValue(userSecSet.ArrUserCode);
                bizUserCode_Filter.Logic = "in";

                uiStore.FilterParams.Add(bizUserCode_Filter);
            }

        }


        private static string GetCataStrList(UserSecritySet uss)
        {
            StringBuilder valueSb = new StringBuilder();

            if (uss.ArrCatalogCode == null || uss.ArrCatalogCode.Length == 0)
            {
                valueSb.Append("null");
            }
            else
            {
                int i = 0;
                foreach (var item in uss.ArrCatalogCode)
                {
                    if (i++ > 0) { valueSb.Append(", "); }

                    valueSb.Append("'").Append(item).Append("'");
                }
            }

            return valueSb.ToString();
        }

        //已经处理的类别字段名
        List<int> m_FilterCatalogFields = new List<int>();

        /// <summary>
        /// 绑定目录权限
        /// </summary>
        /// <param name="uiStore"></param>
        /// <param name="tSet"></param>
        public void BindCatalog(Store uiStore, TableSet tSet)
        {

            UserSecritySet uss = SecFunMgr.GetUserSecuritySet();

            if (uss == null)
            {
                return;
            }

            int tableId = tSet.Table.IG2_TABLE_ID;

            if (m_FilterCatalogFields.Contains(tableId))
            {
                return;
            }

            m_FilterCatalogFields.Add(tableId);


            foreach (var col in tSet.Cols)
            {
                BindCatalogForCol(col, uss, uiStore);
                
            }


        }

        public static void BindCatalogForCol(IG2_TABLE_COL col,UserSecritySet uss, Store uiStore)
        {
            if (col == null || uss == null || uiStore == null)
            {
                return;
            }

            if (StringUtil.IsBlank(col.FILTER_CATA_TABLE) ||
                    StringUtil.IsBlank(col.FILTER_CATA_FIELD))
            {
                return;
            }


            LModelElement srcMElem = LightModel.GetLModelElement(col.FILTER_CATA_TABLE);

            if (srcMElem == null)
            {
                return;
            }

            LModelFieldElement srcFElem = null;

            if (!srcMElem.TryGetField(col.FILTER_CATA_FIELD, out srcFElem))
            {
                throw new Exception(string.Format("“{0}”表，字段不存在“{1}”.", col.FILTER_CATA_TABLE, col.FILTER_CATA_FIELD));
                //return;
            }


            string valueSb = GetCataStrList(uss);

            TSqlWhereParam p = new TSqlWhereParam();
            p.Name = col.DB_FIELD;

            p.Where = string.Format("({0} is null or {0} IN (SELECT {1} FROM {2} WHERE ROW_SID >=0 AND BIZ_CATA_CODE in ({3})))",
                col.DB_FIELD,
                col.FILTER_CATA_FIELD,
                srcMElem.DBTableName, valueSb);

            uiStore.SelectQuery.Add(p);
        }

    }
}