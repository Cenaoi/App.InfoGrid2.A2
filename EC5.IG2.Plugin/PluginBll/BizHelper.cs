
using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.IG2.Plugin.PluginBll
{
    public class BizHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 上传字段识别表（UT_484）
        /// </summary>
        public static string ImportRuleFieldTableName { get; set; } = "UT_484";

        /// <summary>
        /// 导入规则方式表（UT_483）
        /// </summary>
        public static string ImportRuleModeTableName { get; set; } = "UT_483";

        /// <summary>
        /// 订单上传界面明细表（UT_477）
        /// </summary>
        public static string UploadOrderFileTableName { get; set; } = "UT_477";

        /// <summary>
        /// 客户产品档案名称对比表[映射表]（UT_478）
        /// </summary>
        public static string CustomerGoodsMapTableName { get; set; } = "UT_478";

        /// <summary>
        /// 老系统的产品档案表（Goods）
        /// </summary>
        public static string GoodsTableName { get; set; } = "Goods";

        /// <summary>
        /// 老系统的客户档案表（Customer）
        /// </summary>
        public static string CustomerTableName { get; set; } = "Customer";

        /// <summary>
        /// 老系统的订单表（SaleOrder）
        /// </summary>
        public static string SaleOrderTableName { get; set; } = "SaleOrder";

        /// <summary>
        /// 老系统的订单明细表（SaleOrderList）
        /// </summary>
        public static string SaleOrderListTableName { get; set; } = "SaleOrderList";

        /// <summary>
        /// 目标数据库的产品档案表（UT_083）
        /// </summary>
        public static string TargetGoodsTableName { get; set; } = "UT_083";

        /// <summary>
        /// 目标数据库的客户档案表（UT_071）
        /// </summary>
        public static string TargetCustomerTableName { get; set; } = "UT_071";

        /// <summary>
        /// 源数据库连接字符串
        /// </summary>
        public static string SDbConn { get; set; } = "Data Source=127.0.0.1;Initial Catalog=gubo2021Test;Persist Security Info=True;User ID=sa;Password=sa2014";

        /// <summary>
        /// 缓存数据毫秒，默认值：5分钟（5 * 60 * 1000）
        /// </summary>
        public static int CacheDataMilliseconds { get; set; } = 5 * 60 * 1000;

        /// <summary>
        /// 获取查询记录数据的第一个
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SModel GetGbDataBySql(string sql)
        {
            using (DbDecipher decipher = new SqlServer2005Decipher(SDbConn))
            {
                decipher.Open();

                SModel model = decipher.GetSModel(sql);

                return model;
            }
        }

        /// <summary>
        /// 缓存产品信息的key，缓存客户信息的key
        /// </summary>
        static Dictionary<string, int> DbDataKeyList = new Dictionary<string, int>();

        /// <summary>
        /// 根据对应的数据表获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SModel GetDbDataModel(string sql)
        {
            SModel res = null;

            if (sql.Contains("Goods"))
            {
                res = GetGoods(sql);
            }
            else if (sql.Contains("Customer"))
            {
                res = GetCustomer(sql);
            }
            else
            {
                res = GetDbDataBySql(sql);
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SModel GetDbDataBySql(string sql)
        {
            using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
            {
                SModel res = decipher.GetSModel(sql);

                return res;
            }
        }

        #region 同步产品档案

        /// <summary>
        /// 获取产品档案
        /// </summary>
        /// <returns></returns>
        public SModelList GetGoodsList()
        {
            SModelList list = new SModelList();

            string sql = $"select * from {GoodsTableName}";

            using (DbDecipher decipher = new SqlServer2005Decipher(SDbConn))
            {
                decipher.Open();

                list = decipher.GetSModelList(sql);
            }

            return list;
        }

        /// <summary>
        /// 导入产品档案
        /// </summary>
        public bool ImportGoods()
        {
            SModelList goods = GetGoodsList();

            if (goods.Count == 0)
            {
                return true;
            }

            log.Debug($"获取到产品，数量：{goods.Count}");

            LModelList list = new LModelList();

            SModelList uList = new SModelList();

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    foreach (var item in goods)
                    {
                        int id = item.GetInt("ID");

                        SModel ut83 = GetGoods(decipher, id);

                        if (ut83 == null)
                        {
                            LModel lm = new LModel(TargetGoodsTableName)
                            {
                                ["BIZ_SID"] = 2,
                                ["ROW_SID"] = 0,
                                ["COL_146"] = item.GetInt("ID"),
                                ["COL_2"] = item.GetString("Code"),
                                ["COL_3"] = item.GetString("Name"),
                                //[""] = item.GetString("EnglishName"),
                                //[""] = item.GetString("BarCode"),
                                ["COL_12"] = item.GetString("SortID"),
                                ["COL_4"] = item.GetString("Model"),
                                ["COL_114"] = item.GetString("Brand"),
                                ["COL_155"] = item.GetString("Origin"),
                                ["COL_77"] = item.GetString("Property"),
                                //[""] = item.GetString("PackModel"),
                                ["COL_31"] = item.GetDouble("PackQty1"),
                                ["COL_29"] = item.GetString("PackUnit1"),
                                ["COL_5"] = item.GetString("Unit"),
                                //[""] = item.GetString("Spell"),
                                //[""] = item.GetString("DepotID"),
                                //[""] = item.GetString("ABC"),
                                //[""] = item.GetDouble("TopQty"),
                                //[""] = item.GetDouble("LowQty"),
                                //[""] = item.GetDouble("OrderQty"),
                                //[""] = item.GetInt("CoinID"),
                                //[""] = item.GetString("State"),
                                ["COL_47"] = item.GetString("des"),
                                //[""] = item.GetString("Photo"),
                                ["COL_41"] = item.GetDateTime("BeginDate"),
                                ["COL_73"] = item.GetDateTime("EditDate"),
                                //[""] = item.GetInt("ProviderID"),
                                ["COL_156"] = item.GetDouble("ReferPrice"),
                                ["COL_157"] = item.GetDouble("WeightRate")
                            };

                            list.Add(lm);
                        }
                        else
                        {
                            ut83["COL_2"] = item.GetString("Code");
                            ut83["COL_3"] = item.GetString("Name");
                            //ut83[""] = item.GetString("EnglishName");
                            //ut83[""] = item.GetString("BarCode");
                            ut83["COL_12"] = item.GetString("SortID");
                            ut83["COL_4"] = item.GetString("Model");
                            ut83["COL_114"] = item.GetString("Brand");
                            ut83["COL_155"] = item.GetString("Origin");
                            ut83["COL_77"] = item.GetString("Property");
                            //ut83[""] = item.GetString("PackModel");
                            ut83["COL_31"] = item.GetDouble("PackQty1");
                            ut83["COL_29"] = item.GetString("PackUnit1");
                            ut83["COL_5"] = item.GetString("Unit");
                            //ut83[""] = item.GetString("Spell");
                            //ut83[""] = item.GetString("DepotID");
                            //ut83[""] = item.GetString("ABC");
                            //ut83[""] = item.GetDouble("TopQty");
                            //ut83[""] = item.GetDouble("LowQty");
                            //ut83[""] = item.GetDouble("OrderQty");
                            //ut83[""] = item.GetInt("CoinID");
                            //ut83[""] = item.GetString("State");
                            ut83["COL_47"] = item.GetString("des");
                            //ut83[""] = item.GetString("Photo");
                            ut83["COL_41"] = item.GetDateTime("BeginDate");
                            ut83["COL_73"] = item.GetDateTime("EditDate");
                            //ut83[""] = item.GetInt("ProviderID");
                            ut83["COL_156"] = item.GetDouble("ReferPrice");
                            ut83["COL_157"] = item.GetDouble("WeightRate");

                            uList.Add(ut83);
                        }
                    }

                    if (list.Count > 0)
                    {
                        decipher.InsertModels(list);

                        log.Debug($"新增产品（{TargetGoodsTableName}），数量：{list.Count}");
                    }

                    if (uList.Count > 0)
                    {
                        foreach (var item in uList)
                        {
                            decipher.UpdateSModel(item, TargetGoodsTableName, $" ROW_IDENTITY_ID = {item["ROW_IDENTITY_ID"]} ");
                        }

                        log.Debug($"更新产品（{TargetGoodsTableName}），数量：{uList.Count}");
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error($"同步产品档案出错，源数据数量：{list.Count}", ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SModel GetGoods(DbDecipher decipher, int id)
        {
            LightModelFilter filter = new LightModelFilter(TargetGoodsTableName);
            filter.And("COL_146", id);

            SModel model = decipher.GetSModel(filter);

            return model;
        }

        /// <summary>
        /// 缓存产品信息
        /// </summary>
        //static Cache<int, SModel> GoodsList = new Cache<int, SModel>();

        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SModel GetGoods(int id)
        {
            //if (GoodsList.TryGet(id, out SModel goods))
            //{
            //    return goods;
            //}

            string sql = $"select * from {GoodsTableName} where ID = {id}";

            SModel goods = GetGbDataBySql(sql);

            if (goods == null)
            {
                return null;
            }

            //GoodsList.Set(id, CacheDataMilliseconds, goods);

            //DbDataKeyList.Set(sql, CacheDataMilliseconds, id);

            return goods;
        }

        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SModel GetGoods(string sql)
        {
            //if (DbDataKeyList.TryGet(sql, out int goodId))
            //{
            //    return GetGoods(goodId);
            //}

            SModel goods = GetGbDataBySql(sql);

            if (goods == null)
            {
                return null;
            }

            //goodId = goods.GetInt("ID");

            //GoodsList.Set(goodId, CacheDataMilliseconds, goods);

            //DbDataKeyList.Set(sql, CacheDataMilliseconds, goodId);

            return goods;
        }

        #endregion


        #region 同步客户档案

        /// <summary>
        /// 获取客户档案
        /// </summary>
        /// <returns></returns>
        public SModelList GetCustomerList()
        {
            SModelList list = new SModelList();

            string sql = $"select * from {CustomerTableName}";

            using (DbDecipher decipher = new SqlServer2005Decipher(SDbConn))
            {
                decipher.Open();

                list = decipher.GetSModelList(sql);
            }

            return list;
        }

        /// <summary>
        /// 导入客户档案
        /// </summary>
        public bool ImportCustomer()
        {
            SModelList customers = GetCustomerList();

            if (customers.Count == 0)
            {
                return true;
            }

            log.Debug($"获取到客户，数量：{customers.Count}");

            LModelList list = new LModelList();

            SModelList uList = new SModelList();

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    foreach (var item in customers)
                    {
                        int id = item.GetInt("ID");

                        SModel ut71 = GetCustomer(decipher, id);

                        if (ut71 == null)
                        {
                            LModel lm = new LModel(TargetCustomerTableName)
                            {
                                ["BIZ_SID"] = 2,
                                ["ROW_SID"] = 0,
                                ["COL_141"] = item.GetInt("ID"),
                                ["COL_1"] = item.GetString("Code"),
                                ["COL_2"] = item.GetString("Name"),
                                ["COL_18"] = item.GetString("ShortName"),
                                ["COL_126"] = item.GetString("CustTypeID"),
                                //[""] = item.GetString("PriceType"),
                                ["COL_3"] = item.GetString("Tel"),
                                ["COL_15"] = item.GetString("LinkMan"),
                                ["COL_16"] = item.GetString("LMTel"),
                                ["COL_4"] = item.GetString("Address"),
                                //[""] = item.GetString("AreaID"),
                                ["COL_50"] = item.GetString("Bank"),
                                //[""] = item.GetString("PayTypeID"),
                                //[""] = item.GetString("TransTypeID"),
                                //[""] = item.GetInt("FeeTypeID"),
                                ["COL_25"] = item.GetString("Fax"),
                                //[""] = item.GetString("EMail"),
                                //[""] = item.GetString("Post"),
                                //[""] = item.GetString("Http"),
                                //[""] = item.GetString("Tax"),
                                ["COL_51"] = item.GetString("Accounts"),
                                //[""] = item.GetString("AccountName"),
                                //[""] = item.GetString("QualityAssess"),
                                //[""] = item.GetString("TimeAssess"),
                                //[""] = item.GetDouble("Credit"),
                                ["BIZ_CREATE_DATE"] = item.GetDateTime("BeginDate"),
                                ["BIZ_CHECK_DATE"] = item.GetDateTime("EditDate"),
                                //[""] = item.GetString("Spell"),
                                ["COL_10"] = item.GetString("des"),
                                //[""] = item.GetBool("UseCredit")
                            };

                            list.Add(lm);
                        }
                        else
                        {
                            ut71["COL_1"] = item.GetString("Code");
                            ut71["COL_2"] = item.GetString("Name");
                            ut71["COL_18"] = item.GetString("ShortName");
                            ut71["COL_126"] = item.GetString("CustTypeID");
                            //ut71[""] = item.GetString("PriceType");
                            ut71["COL_3"] = item.GetString("Tel");
                            ut71["COL_15"] = item.GetString("LinkMan");
                            ut71["COL_16"] = item.GetString("LMTel");
                            ut71["COL_4"] = item.GetString("Address");
                            //ut71[""] = item.GetString("AreaID");
                            ut71["COL_50"] = item.GetString("Bank");
                            //ut71[""] = item.GetString("PayTypeID");
                            //ut71[""] = item.GetString("TransTypeID");
                            //ut71[""] = item.GetInt("FeeTypeID");
                            ut71["COL_25"] = item.GetString("Fax");
                            //ut71[""] = item.GetString("EMail");
                            //ut71[""] = item.GetString("Post");
                            //ut71[""] = item.GetString("Http");
                            //ut71[""] = item.GetString("Tax");
                            ut71["COL_51"] = item.GetString("Accounts");
                            //ut71[""] = item.GetString("AccountName");
                            //ut71[""] = item.GetString("QualityAssess");
                            //ut71[""] = item.GetString("TimeAssess");
                            //ut71[""] = item.GetDouble("Credit");
                            ut71["BIZ_CREATE_DATE"] = item.GetDateTime("BeginDate");
                            ut71["BIZ_CHECK_DATE"] = item.GetDateTime("EditDate");
                            //ut71[""] = item.GetString("Spell");
                            ut71["COL_10"] = item.GetString("des");
                            //ut71[""] = item.GetBool("UseCredit");

                            uList.Add(ut71);
                        }
                    }

                    if (list.Count > 0)
                    {
                        decipher.InsertModels(list);

                        log.Debug($"新增客户（{TargetCustomerTableName}），数量：{list.Count}");
                    }

                    if (uList.Count > 0)
                    {
                        foreach (var item in uList)
                        {
                            decipher.UpdateSModel(item, TargetCustomerTableName, $" ROW_IDENTITY_ID = {item["ROW_IDENTITY_ID"]} ");
                        }

                        log.Debug($"更新客户（{TargetCustomerTableName}），数量：{uList.Count}");
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error($"同步客户档案出错，源数据数量：{list.Count}", ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SModel GetCustomer(DbDecipher decipher, int id)
        {
            LightModelFilter filter = new LightModelFilter(TargetCustomerTableName);
            filter.And("COL_141", id);

            SModel model = decipher.GetSModel(filter);

            return model;
        }

        /// <summary>
        /// 缓存客户信息
        /// </summary>
        //static Cache<int, SModel> CustomerList = new Cache<int, SModel>();

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SModel GetCustomer(int id)
        {
            //if (CustomerList.TryGet(id, out SModel customer))
            //{
            //    return customer;
            //}

            string sql = $"select * from {CustomerTableName} where ID = {id}";

            SModel customer = GetGbDataBySql(sql);

            if (customer == null)
            {
                return null;
            }

            //CustomerList.Set(id, CacheDataMilliseconds, customer);

            //DbDataKeyList.Set(sql, CacheDataMilliseconds, id);

            return customer;
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SModel GetCustomer(string sql)
        {
            //if (DbDataKeyList.TryGet(sql, out int customerId))
            //{
            //    return GetGoods(customerId);
            //}

            SModel customer = GetGbDataBySql(sql);

            if (customer == null)
            {
                return null;
            }

            //customerId = customer.GetInt("ID");

            //CustomerList.Set(customerId, CacheDataMilliseconds, customer);

            //DbDataKeyList.Set(sql, CacheDataMilliseconds, customerId);

            return customer;
        }

        #endregion


        /// <summary>
        /// 获取产品最近报价（准备废弃）
        /// </summary>
        /// <returns></returns>
        public static SModel GetLastQuoted(DbDecipher decipher, int customerId, int goodsId)
        {
            LightModelFilter filter = new LightModelFilter("");
            filter.And("", customerId);

            //获取客户全部报价单
            SModelList orderList = decipher.GetSModelList(filter);

            SModelList quotedItems = new SModelList();

            foreach (var order in orderList)
            {
                LightModelFilter filterItem = new LightModelFilter("");
                filterItem.And("", order.GetInt("")); //主键
                filterItem.And("", goodsId);

                //获取报价单的指定产品报价信息
                SModel item = decipher.GetSModel(filterItem);

                if (item == null)
                {
                    continue;
                }

                quotedItems.Add(item);
            }

            //排序时间字段，倒序取最近报价
            SModel res = quotedItems.OrderByDescending(p => p[""]).FirstOrDefault();

            return res;
        }


        /// <summary>
        /// 产品映射信息
        /// </summary>
        //static Cache<string, SModel> GoodsMapInfoList = new Cache<string, SModel>();


        /// <summary>
        /// 根据客户id和识别内容获取产品映射信息（UT_478）
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="ocrContent"></param>
        /// <returns></returns>
        public static SModel GetGoodsMapInfo(int customerId, string ocrContent)
        {
            string key = $"{customerId}_{ocrContent}";

            //if (GoodsMapInfoList.TryGet(key, out SModel res))
            //{
            //    return res;
            //}

            SModel res = null;

            LightModelFilter filter = new LightModelFilter(CustomerGoodsMapTableName);
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("BIZ_SID", 2);
            filter.And("COL_158", customerId);
            filter.And("COL_166", ocrContent);
            filter.And("COL_168", 1);

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    res = decipher.GetSModel(filter);
                }

                if (res == null)
                {
                    return null;
                }

                //GoodsMapInfoList.Set(key, CacheDataMilliseconds, res);

                return res;
            }
            catch (Exception ex)
            {
                log.Error($"获取导入规则信息出错", ex);

                return null;
            }
        }


        /// <summary>
        /// 获取导入规则附加信息
        /// </summary>
        /// <returns></returns>
        public static SModelList GetRuleAppendInfo()
        {
            SModelList list = new SModelList();

            LightModelFilter filter = new LightModelFilter(ImportRuleModeTableName);
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    list = decipher.GetSModelList(filter);
                }
            }
            catch (Exception ex)
            {
                log.Error("获取导入规则附加信息出错", ex);
            }

            return list;
        }


        /// <summary>
        /// 获取导入文件列表
        /// </summary>
        /// <returns></returns>
        public static SModelList GetImportFileList()
        {
            SModelList list = new SModelList();

            LightModelFilter filter = new LightModelFilter(UploadOrderFileTableName);
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("BIZ_SID", 2);
            filter.And("COL_144", "102");  //过滤未处理状态

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    string conn = decipher.ConnectionString;

                    list = decipher.GetSModelList(filter);
                }
            }
            catch (Exception ex)
            {
                log.Error("获取导入文件列表出错", ex);
            }

            return list;
        }


        /// <summary>
        /// 获取导入规则信息
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static SModelList GetRuleInfoList(int ruleId)
        {
            SModelList list = new SModelList();

            LightModelFilter filter = new LightModelFilter(ImportRuleFieldTableName);
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("BIZ_SID", 2);
            filter.And("COL_22", ruleId);
            filter.And("COL_29", 999, HWQ.Entity.Filter.Logic.Inequality);

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    list = decipher.GetSModelList(filter);
                }
            }
            catch (Exception ex)
            {
                log.Error("获取导入规则信息出错", ex);
            }

            return list;
        }


        /// <summary>
        /// 获取导入默认客户产品档案名称对比信息（找不到对应产品信息时用）
        /// </summary>
        /// <returns></returns>
        public static SModel GetEmptyGoodsMapInfo()
        {
            SModel emptyGoodsMapInfo = new SModel()
            {
                ["COL_146"] = 12
            };

            using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
            {
                string idStr = GlobelParam.GetValue(decipher, "DEFAULT_GOODS_ID", "12", "默认产品ID");

                if (int.TryParse(idStr, out int goodsId))
                {
                    emptyGoodsMapInfo["COL_146"] = goodsId;
                }
            }

            return emptyGoodsMapInfo;
        }


        /// <summary>
        /// 获取导入默认产品信息（找不到对应产品信息时用）
        /// </summary>
        /// <returns></returns>
        public static SModel GetEmptyGoodsInfo()
        {
            SModel emptyGoodsInfo = new SModel()
            {
                ["ID"] = 12
            };

            using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
            {
                string idStr = GlobelParam.GetValue(decipher, "DEFAULT_GOODS_ID", "12", "默认产品ID");

                if (int.TryParse(idStr, out int goodsId))
                {
                    emptyGoodsInfo["ID"] = goodsId;
                }
            }

            return emptyGoodsInfo;
        }


        /// <summary>
        /// 获取导入文件信息
        /// </summary>
        /// <param name="pkId"></param>
        /// <returns></returns>
        public static LModel GetImportFileInfo(int pkId)
        {
            LightModelFilter filter = new LightModelFilter(BizHelper.UploadOrderFileTableName);
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("ROW_IDENTITY_ID", pkId);

            using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
            {
                LModel model = decipher.GetModel(filter);

                return model;
            }
        }


        /// <summary>
        /// 获取导入文件信息
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="pkId"></param>
        /// <returns></returns>
        public static LModel GetImportFileInfo(DbDecipher decipher, int pkId)
        {
            try
            {
                LightModelFilter filter = new LightModelFilter(BizHelper.UploadOrderFileTableName);
                filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                filter.And("ROW_IDENTITY_ID", pkId);

                LModel model = decipher.GetModel(filter);

                return model;
            }
            catch (Exception ex)
            {
                log.Error("获取导入文件信息出错", ex);

                return null;
            }
        }


    }
}
