using EasyClick.Web.Mini2;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.IG2.Plugin.Custom
{
    /// <summary>
    /// 同步固铂数据插件
    /// </summary>
    public class SyncGbDataPlugin: PagePlugin
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 提交
        /// </summary>
        public void Submit()
        {
            //string paramStr = this.Params;

            //log.Debug("提交参数(20211114)：" + paramStr);

            try
            {
                ImportGoods();

                ImportCustomer();

                MessageBox.Alert("同步成功");
            }
            catch (Exception ex)
            {
                log.Error("同步数据失败", ex);

                MessageBox.Alert("同步失败");
            }    
        }

        #region 同步产品档案

        /// <summary>
        /// 获取产品档案
        /// </summary>
        /// <returns></returns>
        public SModelList GetGoodsList()
        {
            string conn = DbDecipherManager.GetConnectionString("GUBO_2021");

            SModelList list = new SModelList();

            string sql = "select * from Goods";

            using (DbDecipher decipher = new SqlServer2005Decipher(conn))
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
                            LModel lm = new LModel("UT_083")
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
                                ["COL_31"] = GetDouble(item, "PackQty1"),
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
                                ["COL_156"] = GetDouble(item, "ReferPrice"),
                                ["COL_157"] = GetDouble(item, "WeightRate")
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
                            ut83["COL_31"] = GetDouble(item, "PackQty1");
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
                            ut83["COL_156"] = GetDouble(item, "ReferPrice");
                            ut83["COL_157"] = GetDouble(item, "WeightRate");

                            uList.Add(ut83);
                        }
                    }

                    if (list.Count > 0)
                    {
                        decipher.InsertModels(list);

                        log.Debug($"新增产品（ut83），数量：{list.Count}");
                    }

                    if (uList.Count > 0)
                    {
                        foreach (var item in uList)
                        {
                            decipher.UpdateSModel(item, "UT_083", $" ROW_IDENTITY_ID = {item["ROW_IDENTITY_ID"]} ");
                        }

                        log.Debug($"更新产品（ut83），数量：{uList.Count}");
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error($"同步产品档案出错，源数据数量：{list.Count}", ex);

                throw new Exception($"同步产品档案出错，源数据数量：{list.Count}", ex);
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
            LightModelFilter filter = new LightModelFilter("UT_083");
            filter.And("COL_146", id);

            SModel model = decipher.GetSModel(filter);

            return model;
        }

        #endregion


        #region 同步客户档案

        /// <summary>
        /// 获取客户档案
        /// </summary>
        /// <returns></returns>
        public SModelList GetCustomerList()
        {
            string conn = DbDecipherManager.GetConnectionString("GUBO_2021");

            SModelList list = new SModelList();

            string sql = "select * from Customer";

            using (DbDecipher decipher = new SqlServer2005Decipher(conn))
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
                            LModel lm = new LModel("UT_071")
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
                                //[""] = item.GetBool("UseCredit"),
                                ["COL_12"] = "True"
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
                            ut71["COL_12"] = "True";

                            uList.Add(ut71);
                        }
                    }

                    if (list.Count > 0)
                    {
                        decipher.InsertModels(list);

                        log.Debug($"新增客户（ut71），数量：{list.Count}");
                    }

                    if (uList.Count > 0)
                    {
                        foreach (var item in uList)
                        {
                            decipher.UpdateSModel(item, "UT_071", $" ROW_IDENTITY_ID = {item["ROW_IDENTITY_ID"]} ");
                        }

                        log.Debug($"更新客户（ut71），数量：{uList.Count}");
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error($"同步客户档案出错，源数据数量：{list.Count}", ex);

                throw new Exception($"同步客户档案出错，源数据数量：{list.Count}", ex);
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
            LightModelFilter filter = new LightModelFilter("UT_071");
            filter.And("COL_141", id);

            SModel model = decipher.GetSModel(filter);

            return model;
        }


        #endregion


        public double GetDouble(SModel data, string field)
        {
            double res = 0;

            string vStr = data.GetString(field);

            if (!string.IsNullOrWhiteSpace(vStr))
            {
                res = data.GetDouble(field, 0);
            }

            return res;
        }


    }
}
