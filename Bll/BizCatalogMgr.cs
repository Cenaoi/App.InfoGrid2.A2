using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.Utility;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.InfoGrid2.Bll
{
    /// <summary>
    /// 业务类别分类
    /// </summary>
    public static class BizCatalogMgr
    {

        /// <summary>
        /// 获取新代码
        /// </summary>
        /// <param name="pCata"></param>
        /// <returns></returns>
        public static string NewCode(BIZ_CATALOG pCata)
        {

            if (pCata == null) { throw new ArgumentNullException("pCata"); }


            DbDecipher decipher = ModelAction.OpenDecipher();
            string newCode = string.Empty;

            for (int i = 0; i < 9999; i++)
            {
                int code = ++pCata.CHILD_IDENTITY;

                if (pCata.CATA_CODE == "ROOT")
                {
                    newCode = code.ToString("00");
                }
                else
                {
                    newCode = pCata.CATA_CODE + code.ToString("00");
                }

                LightModelFilter filter = new LightModelFilter(typeof(BIZ_CATALOG));
                filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                filter.And("PARENT_ID", pCata.BIZ_CATALOG_ID);
                filter.And("CATA_CODE", newCode);

                bool exist = decipher.ExistsModels(filter);

                if (!exist)
                {
                    decipher.UpdateModelProps(pCata, "CHILD_IDENTITY");

                    break;
                }
            }

            return newCode;
        }

        /// <summary>
        /// 获取允许查看的类别代码
        /// </summary>
        /// <returns></returns>
        public static string[] GetCodes(string[] secStructCodes)
        {
            if (secStructCodes == null || secStructCodes.Length == 0)
            {
                return new string[0];
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(BIZ_CATALOG));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("SEC_STRUCT_CODE", secStructCodes, Logic.In);
            filter.Fields = new string[] { "CATA_CODE" };

            LModelReader reader = decipher.GetModelReader(filter);

            string[] codes = ModelHelper.GetColumnData<string>(reader);

            codes = ArrayUtil.RemoveDuplicate(codes);

            return codes;
        }

        /// <summary>
        /// 根据 ID ,获取代码
        /// </summary>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        public static string GetCode(int catalogId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(BIZ_CATALOG));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("BIZ_CATALOG_ID", catalogId);
            filter.Fields = new string[] { "CATA_CODE" };

            string code = decipher.ExecuteScalar<string>(filter);

            return code;
        }

        /// <summary>
        /// 根据目录id，获取子节点孙节点的全部 代码
        /// </summary>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        public static string[] GetChildCodeAll(int catalogId)
        {
            BIZ_CATALOG[] items = GetChildAll(catalogId);

            string[] codes = new string[items.Length];

            for (int i = 0; i < items.Length; i++)
            {
                codes[i] = items[i].CATA_CODE;
            }

            return codes;
        }

        /// <summary>
        /// 根据目录id，获取子节点孙节点的全部 代码
        /// </summary>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        public static BIZ_CATALOG[] GetChildAll(int catalogId)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(BIZ_CATALOG));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("BIZ_CATALOG_ID", catalogId);

            BIZ_CATALOG root = decipher.SelectToOneModel<BIZ_CATALOG>(filter);

            List<BIZ_CATALOG> items = new List<BIZ_CATALOG>() { root };

            FullChilds(decipher, catalogId, items,0);

            return items.ToArray();
        }

        /// <summary>
        /// 填充子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="items"></param>
        /// <param name="dept"></param>
        private static void FullChilds(DbDecipher decipher,int parentId, List<BIZ_CATALOG> items,int dept)
        {
            if (decipher == null) { throw new ArgumentNullException("decipher"); }

            if (items == null) { throw new ArgumentNullException("items"); }

            if (dept > 20)
            {
                throw new Exception("业务类别 BIZ_CATALOG 发现死循环节点。");
            }

            LightModelFilter filter = new LightModelFilter(typeof(BIZ_CATALOG));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("PARENT_ID", parentId);

            List<BIZ_CATALOG> models = decipher.SelectModels<BIZ_CATALOG>(filter);

            items.AddRange(models);

            foreach (var model in models)
            {
                FullChilds(decipher, model.BIZ_CATALOG_ID, items, dept + 1);
            }
        }


        /// <summary>
        /// 根据ID 获取 分类对象值  
        /// 小渔夫写的
        /// </summary>
        /// <param name="catalogId">ID</param>
        /// <returns></returns>
        public static BIZ_CATALOG GetBizCata(int catalogId)
        {
            if (catalogId == 0)
            {
                return null;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(BIZ_CATALOG));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("BIZ_CATALOG_ID", catalogId);

            BIZ_CATALOG root = decipher.SelectToOneModel<BIZ_CATALOG>(filter);

            return root;

        }

        /// <summary>
        /// 根据分类编码获取类型对象信息
        /// 小渔夫写的
        /// </summary>
        /// <param name="code">分类编码</param>
        /// <returns></returns>
        public static BIZ_CATALOG GetBizCata(string code)
        {
            if (string.IsNullOrEmpty(code)) throw new ArgumentNullException("code", "分类编码不能为空");

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(BIZ_CATALOG));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("CATA_CODE", code);

            BIZ_CATALOG root = decipher.SelectToOneModel<BIZ_CATALOG>(filter);

            return root;


        }

        /// <summary>
        /// 根据分类编码获取类型对象信息
        /// 小渔夫写的
        /// </summary>
        /// <param name="code">分类编码</param>
        /// <param name="decipher">数据库帮助类</param>
        /// <returns></returns>
        public static BIZ_CATALOG GetBizCata(string code, DbDecipher decipher)
        {
            if (string.IsNullOrEmpty(code)) throw new ArgumentNullException("code", "分类编码不能为空");

            LightModelFilter filter = new LightModelFilter(typeof(BIZ_CATALOG));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("CATA_CODE", code);

            BIZ_CATALOG root = decipher.SelectToOneModel<BIZ_CATALOG>(filter);

            return root;


        }


    }
}
