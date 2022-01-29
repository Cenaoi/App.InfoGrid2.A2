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
        /// 同步销售订单表（SaleOrder=UT_090，SaleOrderList=UT_091）
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

            LModelList orderItemList = new LModelList();

            SModelList orderItemUpList = new SModelList();

            int orderCount = 0;
            int orderUpCount = 0;
            int orderItemCount = 0;
            int orderItemUpCount = 0;

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    foreach (var item in list)
                    {
                        int pkid = TryGetInt(item, "ID");
                        string billNo = item.GetString("BillNo");

                        if (string.IsNullOrWhiteSpace(billNo))
                        {
                            continue;
                        }

                        SModel ut90 = ExistsSaleOrder(decipher, billNo);

                        int orderId = 0;

                        if (ut90 == null)
                        {
                            //新增
                            LModel lmut90 = new LModel("UT_090")
                            {
                                ["BIZ_SID"] = TryGetInt(item, "State"),
                                //["BIZ_CHECK_DATE"] = TryGetDateTime(item, ""),
                                //["BIZ_CHECK_USER_TEXT"] = item.GetString(""),
                                ["COL_1"] = billNo,
                                ["COL_84"] = item.GetString("TabMan"),
                                //["COL_12"] = item.GetString(""),//Max(C.Code)
                                ["COL_4"] = item.GetString("LinkMan"),
                                //["COL_6"] = item.GetString(""),
                                ["COL_9"] = TryGetDecimal(item, "MustReceiveTotal"),
                                ["COL_80"] = TryGetDateTime(item, "PlanDeliverDate"),
                                ["COL_10"] = item.GetString("Des"),
                                ["COL_107"] = TryGetInt(item, "CustomerID"),
                                ["COL_109"] = item.GetString("LiftNo"),
                                ["COL_54"] = item.GetString("SourceDes"),
                                ["COL_2"] = TryGetDateTime(item, "FillDate"),
                                ["COL_8"] = TryGetInt(item, "CustomerID"),
                                //["COL_3"] = item.GetString(""),//Max(C.Name)
                                ["COL_5"] = item.GetString("Tel"),
                                ["COL_7"] = item.GetString("Address"),
                                ["COL_106"] = TryGetInt(item, "ID"),
                                ["COL_110"] = TryGetDateTime(item, "EditDate"),
                                ["COL_111"] = item.GetString("TransTypeName"),
                                ["COL_112"] = item.GetString("FeeTypeName"),
                                ["ROW_DATE_CREATE"] = DateTime.Now
                            };

                            decipher.InsertModel(lmut90);

                            orderId = lmut90.Get<int>("ROW_IDENTITY_ID");

                            orderCount += 1;

                            //orderList.Add(lmut90);
                        }
                        else
                        {
                            //更新
                            //ut90.SetTakeChange(true);
                            ut90["BIZ_SID"] = TryGetInt(item, "State");
                            //ut90["BIZ_CHECK_DATE"] = TryGetDateTime(item, "");
                            //ut90["BIZ_CHECK_USER_TEXT"] = item.GetString("");
                            ut90["COL_1"] = item.GetString("BillNo");
                            ut90["COL_84"] = item.GetString("TabMan");
                            //ut90["COL_12"] = item.GetString("");//Max(C.Code)
                            ut90["COL_4"] = item.GetString("LinkMan");
                            //ut90["COL_6"] = item.GetString("");
                            ut90["COL_9"] = TryGetDecimal(item, "MustReceiveTotal");
                            ut90["COL_80"] = TryGetDateTime(item, "PlanDeliverDate");
                            ut90["COL_10"] = item.GetString("Des");
                            ut90["COL_107"] = TryGetInt(item, "CustomerID");
                            ut90["COL_109"] = item.GetString("LiftNo");
                            ut90["COL_54"] = item.GetString("SourceDes");
                            ut90["COL_2"] = TryGetDateTime(item, "FillDate");
                            ut90["COL_8"] = TryGetInt(item, "CustomerID");
                            //ut90["COL_3"] = item.GetString("");//Max(C.Name)
                            ut90["COL_5"] = item.GetString("Tel");
                            ut90["COL_7"] = item.GetString("Address");
                            ut90["COL_106"] = TryGetInt(item, "ID");
                            ut90["COL_110"] = TryGetDateTime(item, "EditDate");
                            ut90["COL_111"] = item.GetString("TransTypeName");
                            ut90["COL_112"] = item.GetString("FeeTypeName");
                            ut90["ROW_DATE_UPDATE"] = DateTime.Now;
                            ut90["ROW_SID"] = 0;

                            //uList.Add(ut90);

                            decipher.UpdateSModel(ut90, "UT_090", $" ROW_IDENTITY_ID = {ut90["ROW_IDENTITY_ID"]} ");

                            orderId = ut90.GetInt("ROW_IDENTITY_ID");

                            orderUpCount += 1;
                        }

                        //根据单号获取最新的明细数据
                        SModelList itemList = GetSaleOrderItemList(billNo);
                        //根据单号删除原来明细数据
                        DeleteSaleOrderItem(decipher, billNo);
                        //转换新增的明细数据
                        foreach (var itemData in itemList)
                        {
                            int itemId = TryGetInt(itemData, "ID");

                            SModel utsm91 = ExistsSaleOrderItem(decipher, itemId);

                            if (utsm91 == null)
                            {
                                LModel ut91 = TransitionUT_091(itemData);
                                ut91["COL_12"] = orderId;
                                //orderItemList.Add(ut91);

                                decipher.InsertModel(ut91);

                                orderItemCount += 1;
                            }
                            else
                            {
                                TransitionUT_091(itemData, utsm91);
                                utsm91["ROW_SID"] = 0;
                                utsm91["ROW_DATE_UPDATE"] = DateTime.Now;
                                utsm91["COL_12"] = orderId;
                                //orderItemUpList.Add(utsm91);

                                decipher.UpdateSModel(utsm91, "UT_091", $" ROW_IDENTITY_ID = {utsm91["ROW_IDENTITY_ID"]} ");

                                orderItemUpCount += 1;
                            }           
                        }
                    }

                    log.Debug($"新增销售订单（ut90），数量：{orderCount}");
                    log.Debug($"更新销售订单（ut90），数量：{orderUpCount}");
                    log.Debug($"新增销售订单明细（ut91），数量：{orderItemCount}");
                    log.Debug($"更新销售订单明细（ut91），数量：{orderItemUpCount}");

                    //if (orderList.Count > 0)
                    //{
                    //    decipher.InsertModels(orderList);

                    //    log.Debug($"新增销售订单（ut90），数量：{orderList.Count}");
                    //}

                    //if (uList.Count > 0)
                    //{
                    //    foreach (var item in uList)
                    //    {
                    //        decipher.UpdateSModel(item, "UT_090", $" ROW_IDENTITY_ID = {item["ROW_IDENTITY_ID"]} ");
                    //    }

                    //    log.Debug($"更新销售订单（ut90），数量：{uList.Count}");
                    //}

                    //if (orderItemList.Count > 0)
                    //{
                    //    decipher.InsertModels(orderItemList);

                    //    log.Debug($"新增销售订单明细（ut91），数量：{orderItemList.Count}");
                    //}

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
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_106", id);

            SModel model = decipher.GetSModel(filter);

            return model;
        }


        /// <summary>
        /// 销售订单是否存在
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="billNo"></param>
        /// <returns></returns>
        public static SModel ExistsSaleOrder(DbDecipher decipher, string billNo)
        {
            LightModelFilter filter = new LightModelFilter("UT_090");
            //filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_1", billNo);

            SModel model = decipher.GetSModel(filter);

            return model;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="decipher"></param>
        public static void DeleteSaleOrder(DbDecipher decipher)
        {
            SModel upValue = new SModel()
            {
                ["ROW_SID"] = -3,
                ["ROW_DATE_DELETE"] = DateTime.Now
            };

            decipher.UpdateSModel(upValue, "UT_090", "");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="billNo"></param>
        public static void DeleteSaleOrderItem(DbDecipher decipher, string billNo)
        {
            //LightModelFilter filterDel = new LightModelFilter("UT_091");
            //filterDel.And("COL_19", billNo);

            SModel upValue = new SModel()
            {
                ["ROW_SID"] = -3,
                ["ROW_DATE_DELETE"] = DateTime.Now
            };

            decipher.UpdateSModel(upValue, "UT_091", $" COL_19 = '{billNo}' ");
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
        /// 根据单号获取销售订单明细源数据
        /// </summary>
        /// <param name="billNo"></param>
        /// <returns></returns>
        public static SModelList GetSaleOrderItemList(string billNo)
        {
            string sql = $"select *,  " +
            $"(Select Code From Goods Where ID = GoodsID) as GoodsCode, " +
            $"(Select Name From Goods Where ID = GoodsID) as GoodsName, " +
            $"(Select Code From Depot Where TreeID = DepotID) as DepotCode, " +
            $"(Select Name From Depot Where TreeID = DepotID) as DepotName  " +
            $" from SaleOrderList  ";

            if (!string.IsNullOrWhiteSpace(billNo))
            {
                sql += $" where BillNo = '{billNo}' ";
            }

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
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    foreach (var item in list)
                    {
                        int pkid = TryGetInt(item, "ID");

                        SModel ut91 = ExistsSaleOrderItem(decipher, pkid);

                        if (ut91 == null)
                        {
                            //新增
                            LModel lmut91 = TransitionUT_091(item);
                            //???
                            //orderList.Add(lmut91);
                        }
                        else
                        {
                            TransitionUT_091(item, ut91);

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
            //filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_129", id);

            SModel model = decipher.GetSModel(filter);

            return model;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static LModel TransitionUT_091(SModel item)
        {
            //新增
            LModel lmut91 = new LModel("UT_091")
            {
                ["BIZ_SID"] = 2,
                ["COL_19"] = item.GetString("BillNo"), //BillNo
                ["COL_2"] = item.GetString("GoodsCode"), //Select GoodsCode From Goods Where ID=GoodsID
                ["COL_3"] = item.GetString("GoodsName"), //Select GoodsName From Goods Where ID=GoodsID
                //["COL_104"] = TryGetInt(item, "OrderNo"),
                ["COL_105"] = TryGetDecimal(item, "WeightRate"),
                //["COL_115"] = item.GetDecimal(""),
                ["COL_25"] = item.GetString("Des"),
                ["COL_4"] = item.GetString("Model"),
                ["COL_47"] = item.GetString("PackUnit1"),
                ["COL_48"] = TryGetDecimal(item, "PackQty1"),
                ["COL_49"] = TryGetDecimal(item, "Piece1"),
                ["COL_5"] = item.GetString("Unit"),
                ["COL_6"] = TryGetDecimal(item, "Price"),
                ["COL_82"] = TryGetDecimal(item, "OutPrice"),
                ["COL_7"] = TryGetDecimal(item, "Qty1"),
                ["COL_8"] = TryGetDecimal(item, "Total"),
                ["COL_11"] = item.GetString("Des"),
                //[""] = item.GetString("DepotID"),
                ["COL_22"] = item.GetString("DepotCode"), //Select DepotCode From Depot Where TreeID=DepotID
                ["COL_23"] = item.GetString("DepotName"), //Select DepotName From Depot Where TreeID=DepotID
                ["COL_29"] = TryGetInt(item, "GoodsID"),
                ["COL_131"] = TryGetInt(item, "GoodsID"),
                ["COL_129"] = TryGetInt(item, "ID"),
                ["COL_138"] = TryGetDecimal(item, "CostPrice"),
                ["ROW_DATE_CREATE"] = DateTime.Now
            };

            return lmut91;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ut91"></param>
        public static void TransitionUT_091(SModel item,SModel ut91)
        {
            //更新
            //ut91.SetTakeChange(true);
            ////ut91["BIZ_SID"] = 2;
            ut91["COL_19"] = item.GetString("BillNo");
            ut91["COL_2"] = item.GetString("GoodsCode"); //Select GoodsCode From Goods Where ID=GoodsID
            ut91["COL_3"] = item.GetString("GoodsName"); //Select GoodsName From Goods Where ID=GoodsID
             //ut91["COL_104"] = TryGetInt(item, "OrderNo");
            ut91["COL_105"] = TryGetDecimal(item, "WeightRate");
            //ut91["COL_115"] = item.GetDecimal("");
            ut91["COL_25"] = item.GetString("Des");
            ut91["COL_4"] = item.GetString("Model");
            ut91["COL_47"] = item.GetString("PackUnit1");
            ut91["COL_48"] = TryGetDecimal(item, "PackQty1");
            ut91["COL_49"] = TryGetDecimal(item, "Piece1");
            ut91["COL_5"] = item.GetString("Unit");
            ut91["COL_6"] = TryGetDecimal(item, "Price");
            ut91["COL_82"] = TryGetDecimal(item, "OutPrice");
            ut91["COL_7"] = TryGetDecimal(item, "Qty1");
            ut91["COL_8"] = TryGetDecimal(item, "Total");
            ut91["COL_11"] = item.GetString("Des");
            //ut91[""] = item.GetString("DepotID");
            ut91["COL_22"] = item.GetString("DepotCode"); //Select DepotCode From Depot Where TreeID=DepotID
            ut91["COL_23"] = item.GetString("DepotName"); //Select DepotName From Depot Where TreeID=DepotID
            ut91["COL_29"] = TryGetInt(item, "GoodsID");
            ut91["COL_131"] = TryGetInt(item, "GoodsID");
            ut91["COL_129"] = TryGetInt(item, "ID");
            ut91["COL_138"] = TryGetDecimal(item, "CostPrice");
            ut91["ROW_DATE_UPDATE"] = DateTime.Now;
        }


        #endregion


        #region 同步拣货订单表


        /// <summary>
        /// 获取拣货订单源数据
        /// </summary>
        /// <returns></returns>
        public static SModelList GetPickingOrderList()
        {
            string sql = $"select *," +
                $"(Select sum(Qty1) From GetList a Where a.BillNo = t.BillNo) as QtyTotal," +
                $"(Select Code From Depot b Where b.TreeID = t.DepotID) as DepotCode, " +
                $"(Select Name From Depot c Where c.TreeID = t.DepotID) as DepotName  " +
                $" from Get t";

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

            Dictionary<int, SModel> ut71List = new Dictionary<int, SModel>();

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    foreach (var item in list)
                    {
                        int pkid = TryGetInt(item, "ID");

                        string dec = item.GetString("Des");

                        int customerID = TryGetInt(item, "CustomerID");

                        if (!ut71List.TryGetValue(customerID, out SModel ut71))
                        {
                            ut71 = GetUT71(decipher, customerID);

                            ut71List.Add(customerID, ut71);
                        }

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
                                //["COL_71"] = item.GetString("GetMan"),
                                ["COL_72"] = 101,
                                ["COL_73"] = "未开始",
                                ["COL_1"] = item.GetString("BillNo"),
                                ["COL_4"] = TryGetDateTime(item, "FillDate"),
                                ["COL_16"] = item.GetString("BranchID"),
                                ["COL_88"] = 0,
                                ["COL_6"] = dec,
                                ["BIZ_CHECK_DATE"] = TryGetDateTime(item, "ConfirmDate"),
                                ["BIZ_CHECK_USER_TEXT"] = item.GetString("ConfirmMan"),
                                ["COL_103"] = TryGetInt(item, "ID"),
                                ["COL_104"] = TryGetDateTime(item, "EditDate"),
                                ["COL_105"] = TryGetInt(item, "DepotID"),
                                ["COL_106"] = item.GetString("DepotCode"),
                                ["COL_107"] = item.GetString("DepotName"),
                                ["COL_108"] = item.GetString("PlantID"),
                                ["COL_109"] = item.GetString("SourceDes"),
                                ["COL_110"] = item.GetString("SourceForm"),
                                ["COL_92"] = TryGetInt(item, "CustomerID"),
                                ["BIZ_SID"] = TryGetInt(item, "State") + 2,
                                ["ROW_DATE_CREATE"] = DateTime.Now,
                                ["COL_11"] = item.GetString("TabMan"),
                                ["COL_19"] = TryGetDecimal(item, "QtyTotal"),
                                ["COL_59"] = TryGetDateTime(item, "FillDate")
                            };

                            if (ut71 != null)
                            {
                                ut101["COL_31"] = TryGetInt(ut71, "ROW_IDENTITY_ID");
                                ut101["COL_32"] = ut71.GetString("COL_1");
                                ut101["COL_33"] = ut71.GetString("COL_2");
                            }

                            orderList.Add(ut101);
                        }
                        else
                        {
                            //更新
                            ut101.SetTakeChange(true);
                            //ut101["COL_33"] = item.GetString("");
                            //ut101["COL_71"] = item.GetString("GetMan");
                            ut101["COL_1"] = item.GetString("BillNo");
                            ut101["COL_4"] = TryGetDateTime(item, "FillDate");
                            ut101["COL_16"] = item.GetString("BranchID");
                            //ut101["COL_88"] = TryGetInt(item, "");
                            ut101["COL_6"] = dec;
                            ut101["BIZ_CHECK_DATE"] = TryGetDateTime(item, "ConfirmDate");
                            ut101["BIZ_CHECK_USER_TEXT"] = item.GetString("ConfirmMan");
                            ut101["COL_103"] = TryGetInt(item, "ID");
                            ut101["COL_104"] = TryGetDateTime(item, "EditDate");
                            ut101["COL_105"] = TryGetInt(item, "DepotID");
                            ut101["COL_106"] = item.GetString("DepotCode");
                            ut101["COL_107"] = item.GetString("DepotName");
                            ut101["COL_108"] = item.GetString("PlantID");
                            ut101["COL_109"] = item.GetString("SourceDes");
                            ut101["COL_110"] = item.GetString("SourceForm");
                            ut101["COL_92"] = TryGetInt(item, "CustomerID");
                            ut101["ROW_DATE_UPDATE"] = DateTime.Now;
                            ut101["COL_11"] = item.GetString("TabMan");
                            ut101["COL_19"] = TryGetDecimal(item, "QtyTotal");
                            ut101["COL_59"] = TryGetDateTime(item, "FillDate");

                            if (ut71 != null)
                            {
                                ut101["COL_31"] = TryGetInt(ut71, "ROW_IDENTITY_ID");
                                ut101["COL_32"] = ut71.GetString("COL_1");
                                ut101["COL_33"] = ut71.GetString("COL_2");
                            }

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
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
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

            LModelList orderItemList = new LModelList();

            SModelList uList = new SModelList();

            Dictionary<string, SModel> orderList = new Dictionary<string, SModel>();

            int newCount = 0;
            int upCount = 0;

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    foreach (var item in list)
                    {
                        int pkid = TryGetInt(item, "ID");

                        string billNo = item.GetString("BillNo");

                        if (!orderList.TryGetValue(billNo, out SModel ut101))
                        {
                            ut101 = GetUT101ByBillNo(decipher, billNo);

                            orderList.Add(billNo, ut101);
                        }

                        SModel ut104 = ExistsPickingOrderItem(decipher, pkid);

                        if (ut104 == null)
                        {
                            //新增
                            LModel lmut104 = new LModel("UT_104")
                            {
                                ["COL_180"] = TryGetInt(item, "CustomerID"),
                                //["COL_181"] = TryGetDateTime(item, ""),
                                //["COL_109"] = item.GetString(""),
                                ["COL_16"] = item.GetString("SourceBillNo"),
                                ["COL_88"] = TryGetInt(item, "GoodsID"),
                                ["COL_89"] = item.GetString("GoodsCode"), //Select GoodsCode From Goods Where ID=GoodsID
                                ["COL_3"] = item.GetString("GoodsName"), //Select GoodsName From Goods Where ID=GoodsID
                                ["COL_90"] = item.GetString("Model"),
                                ["COL_4"] = item.GetString("Unit"),
                                ["COL_55"] = TryGetDecimal(item, "Qty"), 
                                ["COL_9"] = TryGetDecimal(item, "Qty"), 
                                //["COL_112"] = TryGetDateTime(item, ""),
                                //["COL_110"] = TryGetDateTime(item, ""),
                                ["COL_10"] = item.GetString("Des"),
                                ["COL_195"] = 101,
                                ["COL_32"] = "未开始",
                                ["COL_196"] = DateTime.Now,
                                ["BIZ_SID"] = 2,
                                ["COL_224"] = TryGetInt(item, "ID"),
                                ["COL_225"] = DateTime.Now,
                                ["COL_11"] = item.GetString("BillNo"),
                                ["COL_226"] = TryGetInt(item, "DepotID"),
                                ["COL_14"] = item.GetString("DepotCode"), //Select DepotCode From Depot Where TreeID=DepotID
                                ["COL_15"] = item.GetString("DepotName"),  //Select DepotName From Depot Where TreeID=DepotID
                                ["ROW_DATE_CREATE"] = DateTime.Now
                            };

                            if (ut101 != null)
                            {
                                lmut104["COL_12"] = TryGetInt(ut101, "ROW_IDENTITY_ID");
                                lmut104["COL_180"] = TryGetInt(ut101, "COL_92");
                                lmut104["COL_108"] = ut101.GetString("COL_32");
                                lmut104["COL_109"] = ut101.GetString("COL_33");
                                lmut104["COL_107"] = TryGetInt(ut101, "COL_31");
                                lmut104["COL_110"] = TryGetDateTime(ut101, "COL_59");
                                lmut104["BIZ_SID"] = TryGetInt(ut101, "BIZ_SID");
                                lmut104["COL_103"] = TryGetDateTime(ut101, "COL_59");
                            }

                            //orderItemList.Add(lmut104);

                            decipher.InsertModel(lmut104);

                            newCount++;
                        }
                        else
                        {
                            //更新
                            //ut104.SetTakeChange(true);
                            ut104["COL_180"] = TryGetInt(item, "CustomerID");
                            //ut104["COL_181"] = TryGetDateTime(item, "");
                            //ut104["COL_109"] = item.GetString("");
                            ut104["COL_16"] = item.GetString("SourceBillNo");
                            ut104["COL_88"] = TryGetInt(item, "GoodsID");
                            //ut104["COL_89"] = item.GetString("GoodsCode"); //Select GoodsCode From Goods Where ID=GoodsID
                            //ut104["COL_3"] = item.GetString("GoodsName"); //Select GoodsName From Goods Where ID=GoodsID
                            ut104["COL_90"] = item.GetString("Model");
                            ut104["COL_4"] = item.GetString("Unit");
                            ut104["COL_55"] = TryGetDecimal(item, "Qty");
                            ut104["COL_9"] = TryGetDecimal(item, "Qty");
                            //ut104["COL_112"] = TryGetDateTime(item, "");
                            //ut104["COL_110"] = TryGetDateTime(item, "");
                            ut104["COL_10"] = item.GetString("Des");
                            ut104["COL_224"] = TryGetInt(item, "ID");
                            ut104["COL_11"] = item.GetString("BillNo");
                            ut104["COL_226"] = TryGetInt(item, "DepotID");
                            ut104["COL_14"] = item.GetString("DepotCode"); //Select DepotCode From Depot Where TreeID=DepotID
                            ut104["COL_15"] = item.GetString("DepotName");  //Select DepotName From Depot Where TreeID=DepotID
                            ut104["ROW_DATE_UPDATE"] = DateTime.Now;

                            if (ut101 != null)
                            {
                                ut104["COL_12"] = TryGetInt(ut101, "ROW_IDENTITY_ID");
                                ut104["COL_180"] = TryGetInt(ut101, "COL_92");
                                ut104["COL_108"] = ut101.GetString("COL_32");
                                ut104["COL_109"] = ut101.GetString("COL_33");
                                ut104["COL_107"] = TryGetInt(ut101, "COL_31");
                                ut104["COL_110"] = TryGetDateTime(ut101, "COL_59");
                                ut104["BIZ_SID"] = TryGetInt(ut101, "BIZ_SID");
                                ut104["COL_103"] = TryGetDateTime(ut101, "COL_59");
                            }

                            //uList.Add(ut104);

                            decipher.UpdateSModel(ut104, "UT_104", $" ROW_IDENTITY_ID = {ut104["ROW_IDENTITY_ID"]} ");

                            upCount++;
                        }
                    }

                    log.Debug($"新增拣货订单明细（ut104），数量：{newCount}");
                    log.Debug($"更新拣货订单明细（ut104），数量：{upCount}");

                    if (orderItemList.Count > 0)
                    {
                        decipher.InsertModels(orderItemList);

                        log.Debug($"新增拣货订单明细（ut104），数量：{orderItemList.Count}");
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
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_224", id);

            SModel model = decipher.GetSModel(filter);

            return model;
        }


        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static SModel GetUT71(DbDecipher decipher, int customerID)
        {
            LightModelFilter filter = new LightModelFilter("UT_071");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_141", customerID);

            SModel model = decipher.GetSModel(filter);

            return model;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="billNo"></param>
        /// <returns></returns>
        public static SModel GetUT101ByBillNo(DbDecipher decipher, string billNo)
        {
            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_1", billNo);

            SModel model = decipher.GetSModel(filter);

            return model;
        }


        /// <summary>
        /// 尝试获取整型类型的值
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static int TryGetInt(SModel sm, string field)
        {
            string str = sm.GetString(field);

            int v = 0;

            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }

            if (!int.TryParse(str, out v))
            {
                return 0;
            }

            return v;
        }


        /// <summary>
        /// 尝试获取数值类型的值
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static decimal TryGetDecimal(SModel sm, string field)
        {
            string str = sm.GetString(field);

            decimal res = 0;

            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }

            if (!decimal.TryParse(str, out res))
            {
                return 0;
            }

            return res;
        }


        /// <summary>
        /// 尝试获取日期时间类型的值
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static DateTime TryGetDateTime(SModel sm, string field)
        {
            string str = sm.GetString(field);

            DateTime res = DateTime.Now;

            if (string.IsNullOrWhiteSpace(str))
            {
                return DateTime.Now;
            }

            if (!DateTime.TryParse(str, out res))
            {
                return DateTime.Now;
            }

            return res;
        }


    }
}