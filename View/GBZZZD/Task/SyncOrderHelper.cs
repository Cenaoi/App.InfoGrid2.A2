using App.BizCommon;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.GBZZZD.Task
{
    public class SyncOrderHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 目标数据库连接
        /// </summary>
        public static string TDbConn { get; set; }

        /// <summary>
        /// 数据源数据库连接
        /// </summary>
        public static string SDbConn { get; set; }


        #region 同步销售订单表


        /// <summary>
        /// 获取销售订单源数据
        /// </summary>
        /// <returns></returns>
        public static SModelList GetSaleOrderList()
        {
            string sql = $"select * from SaleOrder  ";

            SModelList list = new SModelList();

            using (DbDecipher decipher = new SqlServer2005Decipher(SDbConn))
            {
                decipher.Open();

                list = decipher.GetSModelList(sql);
            }

            return list;
        }


        /// <summary>
        /// 同步销售订单表
        /// </summary>
        /// <returns></returns>
        public static bool SyncSaleOrder()
        {
            SModelList list = GetSaleOrderList();

            if (list.Count == 0)
            {
                return true;
            }

            log.Debug($"获取到销售订单，数量：{list.Count}");

            LModelList orderList = new LModelList();

            SModelList uList = new SModelList();

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    foreach (var item in list)
                    {
                        int pkid = item.GetInt("ID");

                        SModel ut90 = ExistsSaleOrder(decipher, pkid);

                        if (ut90 == null)
                        {
                            //新增
                            LModel lmut90 = new LModel("UT_090")
                            {
                                ["BIZ_SID"] = item.GetInt("State"),
                                //["BIZ_CHECK_DATE"] = item.GetDateTime(""),
                                //["BIZ_CHECK_USER_TEXT"] = item.GetString(""),
                                ["COL_1"] = item.GetString("BillNo"),
                                ["COL_84"] = item.GetString("TabMan"),
                                //["COL_12"] = item.GetString(""),//Max(C.Code)
                                ["COL_4"] = item.GetString("LinkMan"),
                                //["COL_6"] = item.GetString(""),
                                ["COL_9"] = item.GetDecimal("MustReceiveTotal"),
                                ["COL_80"] = item.GetDateTime("PlanDeliverDate"),
                                ["COL_10"] = item.GetString("Des"),
                                ["COL_107"] = item.GetInt("CustomerID"),
                                ["COL_109"] = item.GetString("LiftNo"),
                                ["COL_54"] = item.GetString("SourceDes"),
                                ["COL_2"] = item.GetDateTime("FillDate"),
                                ["COL_8"] = item.GetInt("CustomerID"),
                                //["COL_3"] = item.GetString(""),//Max(C.Name)
                                ["COL_5"] = item.GetString("Tel"),
                                ["COL_7"] = item.GetString("Address"),
                                ["COL_106"] = item.GetInt("ID"),
                                ["COL_110"] = item.GetDateTime("EditDate"),
                                ["COL_111"] = item.GetString("TransTypeName"),
                                ["COL_112"] = item.GetString("FeeTypeName"),
                                ["ROW_DATE_CREATE"] = DateTime.Now
                            };

                            orderList.Add(lmut90);
                        }
                        else
                        {
                            //更新
                            //ut90.SetTakeChange(true);
                            ut90["BIZ_SID"] = item.GetInt("State");
                            //ut90["BIZ_CHECK_DATE"] = item.GetDateTime("");
                            //ut90["BIZ_CHECK_USER_TEXT"] = item.GetString("");
                            ut90["COL_1"] = item.GetString("BillNo");
                            ut90["COL_84"] = item.GetString("TabMan");
                            //ut90["COL_12"] = item.GetString("");//Max(C.Code)
                            ut90["COL_4"] = item.GetString("LinkMan");
                            //ut90["COL_6"] = item.GetString("");
                            ut90["COL_9"] = item.GetDecimal("MustReceiveTotal");
                            ut90["COL_80"] = item.GetDateTime("PlanDeliverDate");
                            ut90["COL_10"] = item.GetString("Des");
                            ut90["COL_107"] = item.GetInt("CustomerID");
                            ut90["COL_109"] = item.GetString("LiftNo");
                            ut90["COL_54"] = item.GetString("SourceDes");
                            ut90["COL_2"] = item.GetDateTime("FillDate");
                            ut90["COL_8"] = item.GetInt("CustomerID");
                            //ut90["COL_3"] = item.GetString("");//Max(C.Name)
                            ut90["COL_5"] = item.GetString("Tel");
                            ut90["COL_7"] = item.GetString("Address");
                            ut90["COL_106"] = item.GetInt("ID");
                            ut90["COL_110"] = item.GetDateTime("EditDate");
                            ut90["COL_111"] = item.GetString("TransTypeName");
                            ut90["COL_112"] = item.GetString("FeeTypeName");
                            ut90["ROW_DATE_UPDATE"] = DateTime.Now;

                            uList.Add(ut90);
                        }
                    }

                    if (orderList.Count > 0)
                    {
                        decipher.InsertModels(orderList);

                        log.Debug($"新增销售订单（ut90），数量：{orderList.Count}");
                    }

                    if (uList.Count > 0)
                    {
                        foreach (var item in uList)
                        {
                            decipher.UpdateSModel(item, "UT_090", $" ROW_IDENTITY_ID = {item["ROW_IDENTITY_ID"]} ");
                        }

                        log.Debug($"更新销售订单（ut90），数量：{uList.Count}");
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error($"同步销售订单出错，源数据数量：{list.Count}", ex);

                return false;
            }

            return true;
        }


        /// <summary>
        /// 销售订单是否存在
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SModel ExistsSaleOrder(DbDecipher decipher, int id)
        {
            LightModelFilter filter = new LightModelFilter("UT_090");
            filter.And("COL_106", id);

            SModel model = decipher.GetSModel(filter);

            return model;
        }


        #endregion


        #region 同步销售订单明细表


        /// <summary>
        /// 获取销售订单明细源数据
        /// </summary>
        /// <returns></returns>
        public static SModelList GetSaleOrderItemList()
        {
            string sql = $"select *,  " +
                $"(Select Code From Goods Where ID = GoodsID) as GoodsCode, " +
                $"(Select Name From Goods Where ID = GoodsID) as GoodsName, " +
                $"(Select Code From Depot Where TreeID = DepotID) as DepotCode, " +
                $"(Select Name From Depot Where TreeID = DepotID) as DepotName  " +
                $" from SaleOrderList  ";

            SModelList list = new SModelList();

            using (DbDecipher decipher = new SqlServer2005Decipher(SDbConn))
            {
                decipher.Open();

                list = decipher.GetSModelList(sql);
            }

            return list;
        }


        /// <summary>
        /// 同步销售订单明细表
        /// </summary>
        /// <returns></returns>
        public static bool SyncSaleOrderItem()
        {
            SModelList list = GetSaleOrderItemList();

            if (list.Count == 0)
            {
                return true;
            }

            log.Debug($"获取到销售订单明细，数量：{list.Count}");

            LModelList orderList = new LModelList();

            SModelList uList = new SModelList();

            try
            {
                using (DbDecipher decipher  = DbDecipherManager.GetDecipherOpen())
                {
                    foreach (var item in list)
                    {
                        int pkid = item.GetInt("ID");

                        SModel ut91 = ExistsSaleOrderItem(decipher, pkid);

                        if (ut91 == null)
                        {
                            //新增
                            LModel lmut91 = new LModel("UT_091")
                            {
                                ["BIZ_SID"] = 2,
                                ["COL_19"] = item.GetString("SourceBillNo"),
                                ["COL_2"] = item.GetString("GoodsCode"), //Select GoodsCode From Goods Where ID=GoodsID
                                ["COL_3"] = item.GetString("GoodsName"), //Select GoodsName From Goods Where ID=GoodsID
                                //["COL_104"] = item.GetInt("OrderNo"),
                                ["COL_105"] = item.GetDecimal("WeightRate"),
                                //["COL_115"] = item.GetDecimal(""),
                                ["COL_25"] = item.GetString("Des"),
                                ["COL_4"] = item.GetString("Model"),
                                ["COL_47"] = item.GetString("PackUnit1"),
                                ["COL_48"] = item.GetDecimal("PackQty1"),
                                ["COL_49"] = item.GetDecimal("Piece1"),
                                ["COL_5"] = item.GetString("Unit"),
                                ["COL_6"] = item.GetDecimal("Price"),
                                ["COL_82"] = item.GetDecimal("OutPrice"),
                                ["COL_7"] = item.GetDecimal("Qty1"),
                                ["COL_8"] = item.GetDecimal("Total"),
                                ["COL_11"] = item.GetString("Des"),
                                //[""] = item.GetString("DepotID"),
                                ["COL_22"] = item.GetString("DepotCode"), //Select DepotCode From Depot Where TreeID=DepotID
                                ["COL_23"] = item.GetString("DepotName"), //Select DepotName From Depot Where TreeID=DepotID
                                ["COL_29"] = item.GetInt("GoodsID"),
                                ["COL_131"] = item.GetInt("GoodsID"),
                                ["COL_129"] = item.GetInt("ID"),
                                ["COL_138"] = item.GetDecimal("CostPrice"),
                                ["ROW_DATE_CREATE"] = DateTime.Now
                            };

                            orderList.Add(lmut91);
                        }
                        else
                        {
                            //更新
                            //ut91.SetTakeChange(true);
                            ////ut91["BIZ_SID"] = 2;
                            ut91["COL_19"] = item.GetString("SourceBillNo");
                            ut91["COL_2"] = item.GetString("GoodsCode"); //Select GoodsCode From Goods Where ID=GoodsID
                            ut91["COL_3"] = item.GetString("GoodsName"); //Select GoodsName From Goods Where ID=GoodsID
                            //ut91["COL_104"] = item.GetInt("OrderNo");
                            ut91["COL_105"] = item.GetDecimal("WeightRate");
                            //ut91["COL_115"] = item.GetDecimal("");
                            ut91["COL_25"] = item.GetString("Des");
                            ut91["COL_4"] = item.GetString("Model");
                            ut91["COL_47"] = item.GetString("PackUnit1");
                            ut91["COL_48"] = item.GetDecimal("PackQty1");
                            ut91["COL_49"] = item.GetDecimal("Piece1");
                            ut91["COL_5"] = item.GetString("Unit");
                            ut91["COL_6"] = item.GetDecimal("Price");
                            ut91["COL_82"] = item.GetDecimal("OutPrice");
                            ut91["COL_7"] = item.GetDecimal("Qty1");
                            ut91["COL_8"] = item.GetDecimal("Total");
                            ut91["COL_11"] = item.GetString("Des");
                            //ut91[""] = item.GetString("DepotID");
                            ut91["COL_22"] = item.GetString("DepotCode"); //Select DepotCode From Depot Where TreeID=DepotID
                            ut91["COL_23"] = item.GetString("DepotName"); //Select DepotName From Depot Where TreeID=DepotID
                            ut91["COL_29"] = item.GetInt("GoodsID");
                            ut91["COL_131"] = item.GetInt("GoodsID");
                            ut91["COL_129"] = item.GetInt("ID");
                            ut91["COL_138"] = item.GetDecimal("CostPrice");
                            ut91["ROW_DATE_UPDATE"] = DateTime.Now;

                            uList.Add(ut91);
                        }
                    }

                    if (orderList.Count > 0)
                    {
                        decipher.InsertModels(orderList);

                        log.Debug($"新增销售订单明细（ut91），数量：{orderList.Count}");
                    }

                    if (uList.Count > 0)
                    {
                        foreach (var item in uList)
                        {
                            decipher.UpdateSModel(item, "UT_091", $" ROW_IDENTITY_ID = {item["ROW_IDENTITY_ID"]} ");
                        }

                        log.Debug($"更新销售订单明细（ut91），数量：{uList.Count}");
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error($"同步销售订单明细出错，源数据数量：{list.Count}", ex);

                return false;
            }

            return true;
        }


        /// <summary>
        /// 销售订单明细是否存在
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SModel ExistsSaleOrderItem(DbDecipher decipher, int id)
        {
            LightModelFilter filter = new LightModelFilter("UT_091");
            filter.And("COL_129", id);

            SModel model = decipher.GetSModel(filter);

            return model;
        }


        #endregion


        #region 同步拣货订单表


        /// <summary>
        /// 获取拣货订单源数据
        /// </summary>
        /// <returns></returns>
        public static SModelList GetPickingOrderList()
        {
            string sql = $"select *, GetMan as man," +
                $"(Select Code From Depot Where TreeID = DepotID) as DepotCode, " +
                $"(Select Name From Depot Where TreeID = DepotID) as DepotName  " +
                $" from Get ";

            SModelList list = new SModelList();

            using (DbDecipher decipher = new SqlServer2005Decipher(SDbConn))
            {
                decipher.Open();

                list = decipher.GetSModelList(sql);
            }

            return list;
        }


        /// <summary>
        /// 同步拣货订单表
        /// </summary>
        /// <returns></returns>
        public static bool SyncPickingOrder()
        {
            SModelList list = GetPickingOrderList();

            if (list.Count == 0)
            {
                return true;
            }

            log.Debug($"获取到拣货订单，数量：{list.Count}");

            LModelList orderList = new LModelList();

            LModelList uList = new LModelList();

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    foreach (var item in list)
                    {
                        int pkid = item.GetInt("ID");

                        string dec = item.GetString("Des");

                        if (!string.IsNullOrWhiteSpace(dec) && dec.Length > 50)
                        {
                            dec = dec.Substring(0, 50);
                        }

                        LModel ut101 = ExistsPickingOrder(decipher, pkid);

                        if (ut101 == null)
                        {
                            //新增
                            ut101 = new LModel("UT_101")
                            {
                                ["COL_33"] = "", //string
                                ["COL_71"] = item.GetString("Man"),
                                ["COL_72"] = 100,
                                ["COL_73"] = "未开始",
                                ["COL_1"] = item.GetString("BillNo"),
                                ["COL_4"] = item.GetDateTime("FillDate"),
                                ["COL_16"] = item.GetString("BranchID"),
                                ["COL_88"] = 0,
                                ["COL_6"] = dec,
                                ["BIZ_CHECK_DATE"] = item.GetDateTime("ConfirmDate"),
                                ["BIZ_CHECK_USER_TEXT"] = item.GetString("ConfirmMan"),
                                ["COL_103"] = item.GetInt("ID"),
                                ["COL_104"] = item.GetDateTime("EditDate"),
                                ["COL_105"] = item.GetInt("DepotID"),
                                ["COL_106"] = item.GetString("DepotCode"),
                                ["COL_107"] = item.GetString("DepotName"),
                                ["COL_108"] = item.GetString("PlantID"),
                                ["COL_109"] = item.GetString("SourceDes"),
                                ["COL_110"] = item.GetString("SourceForm"),
                                ["COL_31"] = item.GetInt("CustomerID"),
                                ["BIZ_SID"] = 2,
                                ["ROW_DATE_CREATE"] = DateTime.Now
                            };

                            orderList.Add(ut101);
                        }
                        else
                        {
                            //更新
                            ut101.SetTakeChange(true);
                            //ut101["COL_33"] = item.GetString("");
                            ut101["COL_71"] = item.GetString("GetMan");
                            ut101["COL_1"] = item.GetString("BillNo");
                            ut101["COL_4"] = item.GetDateTime("FillDate");
                            ut101["COL_16"] = item.GetString("BranchID");
                            //ut101["COL_88"] = item.GetInt("");
                            ut101["COL_6"] = dec;
                            ut101["BIZ_CHECK_DATE"] = item.GetDateTime("ConfirmDate");
                            ut101["BIZ_CHECK_USER_TEXT"] = item.GetString("ConfirmMan");
                            ut101["COL_103"] = item.GetInt("ID");
                            ut101["COL_104"] = item.GetDateTime("EditDate");
                            ut101["COL_105"] = item.GetInt("DepotID");
                            ut101["COL_106"] = item.GetString("DepotCode");
                            ut101["COL_107"] = item.GetString("DepotName");
                            ut101["COL_108"] = item.GetString("PlantID");
                            ut101["COL_109"] = item.GetString("SourceDes");
                            ut101["COL_110"] = item.GetString("SourceForm");
                            ut101["COL_31"] = item.GetInt("CustomerID");
                            ut101["ROW_DATE_UPDATE"] = DateTime.Now;

                            uList.Add(ut101);
                        }
                    }

                    if (orderList.Count > 0)
                    {
                        decipher.InsertModels(orderList);

                        log.Debug($"新增拣货订单（ut101），数量：{orderList.Count}");
                    }

                    if (uList.Count > 0)
                    {
                        //foreach (var item in uList)
                        //{
                        //    decipher.UpdateSModel(item, "UT_101", $" ROW_IDENTITY_ID = {item["ROW_IDENTITY_ID"]} ");
                        //}

                        foreach (var item in uList)
                        {
                            decipher.UpdateModel(item, true);
                        }

                        log.Debug($"更新拣货订单（ut101），数量：{uList.Count}");
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error($"同步拣货订单出错，源数据数量：{list.Count}", ex);

                return false;
            }

            return true;
        }


        /// <summary>
        /// 拣货订单是否存在
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static LModel ExistsPickingOrder(DbDecipher decipher, int id)
        {
            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("COL_103", id);

            LModel model = decipher.GetModel(filter);

            return model;
        }


        #endregion


        #region 同步拣货订单明细表


        /// <summary>
        /// 获取拣货订单明细源数据
        /// </summary>
        /// <returns></returns>
        public static SModelList GetPickingOrderItemList()
        {
            string sql = $"select *," +
                $"(Select Code From Goods Where ID = GoodsID) as GoodsCode, " +
                $"(Select Name From Goods Where ID = GoodsID) as GoodsName, " +
                $"(Select Code From Depot Where TreeID = DepotID) as DepotCode, " +
                $"(Select Name From Depot Where TreeID = DepotID) as DepotName  " +
                $" from GetList ";

            SModelList list = new SModelList();

            using (DbDecipher decipher = new SqlServer2005Decipher(SDbConn))
            {
                decipher.Open();

                list = decipher.GetSModelList(sql);
            }

            return list;
        }


        /// <summary>
        /// 同步拣货订单明细表
        /// </summary>
        /// <returns></returns>
        public static bool SyncPickingOrderItem()
        {
            SModelList list = GetPickingOrderItemList();

            if (list.Count == 0)
            {
                return true;
            }

            log.Debug($"获取到拣货订单明细，数量：{list.Count}");

            LModelList orderList = new LModelList();

            SModelList uList = new SModelList();

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    foreach (var item in list)
                    {
                        int pkid = item.GetInt("ID");

                        SModel ut104 = ExistsPickingOrderItem(decipher, pkid);

                        if (ut104 == null)
                        {
                            //新增
                            LModel lmut104 = new LModel("UT_104")
                            {
                                ["COL_180"] = item.GetInt("CustomerID"),
                                //["COL_181"] = item.GetDateTime(""),
                                //["COL_109"] = item.GetString(""),
                                ["COL_16"] = item.GetString("SourceBillNo"),
                                ["COL_88"] = item.GetInt("GoodsID"),
                                ["COL_89"] = item.GetString("GoodsCode"), //Select GoodsCode From Goods Where ID=GoodsID
                                ["COL_3"] = item.GetString("GoodsName"), //Select GoodsName From Goods Where ID=GoodsID
                                ["COL_90"] = item.GetString("Model"),
                                ["COL_4"] = item.GetString("Unit"),
                                ["COL_55"] = item.GetDecimal("Qty"),
                                ["COL_9"] = item.GetDecimal("Qty"),
                                //["COL_112"] = item.GetDateTime(""),
                                //["COL_110"] = item.GetDateTime(""),
                                ["COL_10"] = item.GetString("Des"),
                                ["COL_195"] = 100,
                                ["COL_32"] = "未开始",
                                ["COL_196"] = DateTime.Now,
                                ["BIZ_SID"] = 2,
                                ["COL_224"] = item.GetInt("ID"),
                                ["COL_225"] = DateTime.Now,
                                ["COL_11"] = item.GetString("BillNo"),
                                ["COL_226"] = item.GetInt("DepotID"),
                                ["COL_14"] = item.GetString("DepotCode"), //Select DepotCode From Depot Where TreeID=DepotID
                                ["COL_15"] = item.GetString("DepotName"),  //Select DepotName From Depot Where TreeID=DepotID
                                ["ROW_DATE_CREATE"] = DateTime.Now
                            };

                            orderList.Add(lmut104);
                        }
                        else
                        {
                            //更新
                            //ut104.SetTakeChange(true);
                            ut104["COL_180"] = item.GetInt("CustomerID");
                            //ut104["COL_181"] = item.GetDateTime("");
                            //ut104["COL_109"] = item.GetString("");
                            ut104["COL_16"] = item.GetString("SourceBillNo");
                            ut104["COL_88"] = item.GetInt("GoodsID");
                            //ut104["COL_89"] = item.GetString("GoodsCode"); //Select GoodsCode From Goods Where ID=GoodsID
                            //ut104["COL_3"] = item.GetString("GoodsName"); //Select GoodsName From Goods Where ID=GoodsID
                            ut104["COL_90"] = item.GetString("Model");
                            ut104["COL_4"] = item.GetString("Unit");
                            ut104["COL_55"] = item.GetDecimal("Qty");
                            ut104["COL_9"] = item.GetDecimal("Qty");
                            //ut104["COL_112"] = item.GetDateTime("");
                            //ut104["COL_110"] = item.GetDateTime("");
                            ut104["COL_10"] = item.GetString("Des");
                            ut104["COL_224"] = item.GetInt("ID");
                            ut104["COL_11"] = item.GetString("BillNo");
                            ut104["COL_226"] = item.GetInt("DepotID");
                            ut104["COL_14"] = item.GetString("DepotCode"); //Select DepotCode From Depot Where TreeID=DepotID
                            ut104["COL_15"] = item.GetString("DepotName");  //Select DepotName From Depot Where TreeID=DepotID
                            ut104["ROW_DATE_UPDATE"] = DateTime.Now;

                            uList.Add(ut104);
                        }
                    }

                    if (orderList.Count > 0)
                    {
                        decipher.InsertModels(orderList);

                        log.Debug($"新增拣货订单明细（ut104），数量：{orderList.Count}");
                    }

                    if (uList.Count > 0)
                    {
                        foreach (var item in uList)
                        {
                            decipher.UpdateSModel(item, "UT_104", $" ROW_IDENTITY_ID = {item["ROW_IDENTITY_ID"]} ");
                        }

                        log.Debug($"更新拣货订单明细（ut104），数量：{uList.Count}");
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error($"同步拣货订单明细出错，源数据数量：{list.Count}", ex);

                return false;
            }

            return true;
        }


        /// <summary>
        /// 拣货订单明细是否存在
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SModel ExistsPickingOrderItem(DbDecipher decipher, int id)
        {
            LightModelFilter filter = new LightModelFilter("UT_104");
            filter.And("COL_224", id);

            SModel model = decipher.GetSModel(filter);

            return model;
        }


        #endregion

    }
}