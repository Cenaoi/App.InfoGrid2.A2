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
            string sql = $"select o.*, c.Code as CustomerCode, c.Name as CustomerName  from SaleOrder  o left join Customer c on c.ID = o.CustomerID";

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

            //log.Debug($"获取到销售订单，数量：{list.Count}");

            LModelList orderList = new LModelList();

            SModelList uList = new SModelList();

            LModelList orderItemList = new LModelList();

            SModelList orderItemUpList = new SModelList();

            int orderCount = 0;
            int orderUpCount = 0;
            int orderItemCount = 0;
            int orderItemUpCount = 0;

            Dictionary<int, SModel> ut71List = new Dictionary<int, SModel>();

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    foreach (var item in list)
                    {
                        int pkid = TryGetInt(item, "ID");
                        string billNo = item.GetString("BillNo");
                        int state = TryGetInt(item, "State");

                        if (string.IsNullOrWhiteSpace(billNo))
                        {
                            continue;
                        }

                        int customerID = TryGetInt(item, "CustomerID");
                        if (!ut71List.TryGetValue(customerID, out SModel ut71))
                        {
                            ut71 = GetUT71(decipher, customerID);
                            ut71List.Add(customerID, ut71);
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

                            lmut90["COL_116"] = item.GetString("OutTypeID");
                            lmut90["COL_115"] = item.GetString("FrameworkID");
                            lmut90["COL_114"] = TryGetFloat(item, "TaxRate");
                            lmut90["COL_113"] = TryGetInt(item, "IsPart");
                            lmut90["COL_81"] = TryGetInt(item, "NewOrderType");

                            lmut90["COL_12"] = item.GetString("CustomerCode");
                            lmut90["COL_3"] = item.GetString("CustomerName");

                            if (state == 0)
                            {
                                lmut90["BIZ_SID"] = 0;
                            }
                            else if (state == 1)
                            {
                                lmut90["BIZ_SID"] = 2;
                            }
                            else if (state == 3)
                            {
                                lmut90["BIZ_SID"] = 4;
                            }

                            if (ut71 != null)
                            {
                                lmut90["COL_8"] = TryGetInt(ut71, "ROW_IDENTITY_ID");
                            }

                            if (lmut90.Get<int>("COL_113") == 1) 
                            {
                                lmut90["COL_86"] = 103;
                                lmut90["COL_87"] = "散件部";
                                lmut90["COL_53"] = 101;
                                lmut90["COL_54"] = "散件订单";
                            }
                            else
                            {
                                lmut90["COL_86"] = 104;
                                lmut90["COL_87"] = "整梯配货部";
                                lmut90["COL_53"] = 102;
                                lmut90["COL_54"] = "整梯订单";
                            }

                            decipher.InsertModel(lmut90);
                            orderId = lmut90.Get<int>("ROW_IDENTITY_ID");
                            orderCount += 1;
                            //orderList.Add(lmut90);

                            ut90 = new SModel();
                            ut90["COL_8"] = lmut90["COL_8"];
                            ut90["COL_107"] = lmut90["COL_107"];
                            ut90["COL_12"] = lmut90["COL_12"];
                            ut90["COL_3"] = lmut90["COL_3"];

                            ut90["COL_53"] = lmut90["COL_53"];
                            ut90["COL_54"] = lmut90["COL_54"];
                            ut90["COL_86"] = lmut90["COL_86"];
                            ut90["COL_87"] = lmut90["COL_87"];
                            ut90["COL_2"] = lmut90["COL_2"];
                            ut90["COL_84"] = lmut90["COL_84"];
                            ut90["COL_10"] = lmut90["COL_10"];
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

                            ut90["COL_116"] = item.GetString("OutTypeID");
                            ut90["COL_115"] = item.GetString("FrameworkID");
                            ut90["COL_114"] = TryGetFloat (item, "TaxRate");
                            ut90["COL_113"] = TryGetInt(item, "IsPart");
                            ut90["COL_81"] = TryGetInt(item, "NewOrderType");

                            ut90["COL_12"] = item.GetString("CustomerCode");
                            ut90["COL_3"] = item.GetString("CustomerName");

                            if (state == 0)
                            {
                                ut90["BIZ_SID"] = 0;
                            }
                            else if (state == 1)
                            {
                                ut90["BIZ_SID"] = 2;
                            }
                            else if (state == 3)
                            {
                                ut90["BIZ_SID"] = 4;
                            }

                            if (ut71 != null)
                            {
                                ut90["COL_8"] = TryGetInt(ut71, "ROW_IDENTITY_ID");
                            }

                            if (TryGetInt(ut90, "COL_113") == 1)
                            {
                                ut90["COL_86"] = 103;
                                ut90["COL_87"] = "散件部";
                                ut90["COL_53"] = 101;
                                ut90["COL_54"] = "散件订单";
                            }
                            else
                            {
                                ut90["COL_86"] = 104;
                                ut90["COL_87"] = "整梯配货部";
                                ut90["COL_53"] = 102;
                                ut90["COL_54"] = "整梯订单";
                            }

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

                                ut91["COL_34"] = ut90["COL_8"];
                                ut91["COL_137"] = ut90["COL_107"];
                                ut91["COL_35"] = ut90["COL_12"];
                                ut91["COL_36"] = ut90["COL_3"];

                                ut91["COL_116"] = ut90["COL_53"];
                                ut91["COL_117"] = ut90["COL_54"];
                                ut91["COL_152"] = ut90["COL_86"];
                                ut91["COL_153"] = ut90["COL_87"];
                                ut91["COL_64"] = ut90["COL_2"];
                                ut91["COL_63"] = ut90["COL_84"];
                                ut91["COL_113"] = ut90["COL_10"];

                                ut91["ROW_DATE_CREATE"] = DateTime.Now;
                                decipher.InsertModel(ut91);
                                orderItemCount += 1;
                            }
                            else
                            {
                                TransitionUT_091(itemData, utsm91);
                                utsm91["ROW_SID"] = 0;
                                utsm91["COL_12"] = orderId;

                                utsm91["COL_34"] = ut90["COL_8"];
                                utsm91["COL_137"] = ut90["COL_107"];
                                utsm91["COL_35"] = ut90["COL_12"];
                                utsm91["COL_36"] = ut90["COL_3"];

                                utsm91["COL_116"] = ut90["COL_53"];
                                utsm91["COL_117"] = ut90["COL_54"];
                                utsm91["COL_152"] = ut90["COL_86"];
                                utsm91["COL_153"] = ut90["COL_87"];
                                utsm91["COL_64"] = ut90["COL_2"];
                                utsm91["COL_63"] = ut90["COL_84"];
                                utsm91["COL_113"] = ut90["COL_10"];

                                utsm91["ROW_DATE_UPDATE"] = DateTime.Now;
                                decipher.UpdateSModel(utsm91, "UT_091", $" ROW_IDENTITY_ID = {utsm91["ROW_IDENTITY_ID"]} ");
                                orderItemUpCount += 1;
                            }           
                        }
                    }

                    //log.Debug($"新增销售订单（ut90），数量：{orderCount}");
                    //log.Debug($"更新销售订单（ut90），数量：{orderUpCount}");
                    //log.Debug($"新增销售订单明细（ut91），数量：{orderItemCount}");
                    //log.Debug($"更新销售订单明细（ut91），数量：{orderItemUpCount}");

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
            string sql = $"select o.*,  " +
            $"g.Code as GoodsCode, " +
            $"g.Name as GoodsName, " +
            $"d.Code as DepotCode, " +
            $"d.Name as DepotName,  " +
            $"g.Property as GoodsProperty " +
            $" from SaleOrderList o " +
            $" left join Goods g on g.ID = o.GoodsID" +
            $" left join Depot d on d.TreeID = o.DepotID";

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
        /// 同步销售订单明细表（没用）
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

                ["COL_105"] = TryGetFloat(item, "WeightRate"),
                ["SZ_0"] = TryGetFloat(item, "Remainder1"),
                ["COL_63"] = item.GetString("FindMan"),
                ["COL_104"] = item.GetString("OrderNo"),
                ["COL_142"] = item.GetString("CSR_pn"),
                ["COL_143"] = item.GetString("CSR_pr"),
                ["COL_144"] = item.GetString("CSR_psn"),
                ["COL_145"] = item.GetString("CSR_dwg"),
                ["COL_146"] = item.GetString("CSR_ps"),
                ["COL_147"] = item.GetString("CSR_ctn"),
                ["COL_148"] = item.GetString("CSR_ID"),
                ["COL_149"] = item.GetString("CSR_ut"),
                ["COL_150"] = item.GetString("CSR_Qty"),
                ["COL_151"] = item.GetString("CSR_NO"),

                ["COL_24"] = item.GetString("GoodsProperty")
            };

            lmut91["COL_242"] = lmut91.Get<float>("COL_7") * lmut91.Get<float>("COL_105");
            lmut91["COL_95"] = 1;
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

            ut91["COL_105"] = TryGetFloat(item, "WeightRate");
            ut91["SZ_0"] = TryGetFloat(item, "Remainder1");
            ut91["COL_63"] = item.GetString("FindMan");
            ut91["COL_104"] = item.GetString("OrderNo");
            ut91["COL_142"] = item.GetString("CSR_pn");
            ut91["COL_143"] = item.GetString("CSR_pr");
            ut91["COL_144"] = item.GetString("CSR_psn");
            ut91["COL_145"] = item.GetString("CSR_dwg");
            ut91["COL_146"] = item.GetString("CSR_ps");
            ut91["COL_147"] = item.GetString("CSR_ctn");
            ut91["COL_148"] = item.GetString("CSR_ID");
            ut91["COL_149"] = item.GetString("CSR_ut");
            ut91["COL_150"] = item.GetString("CSR_Qty");
            ut91["COL_151"] = item.GetString("CSR_NO");
            ut91["COL_24"] = item.GetString("GoodsProperty");

            ut91["COL_242"] = ut91.Get<float>("COL_7") * ut91.Get<float>("COL_105");
            ut91["COL_95"] = 1;
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
        /// <param name="goodId"></param>
        /// <returns></returns>
        public static SModel GetUT83(DbDecipher decipher, int goodId)
        {
            LightModelFilter filter = new LightModelFilter("UT_083");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_146", goodId);

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


        /// <summary>
        /// 尝试获取浮点类型的值
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static float TryGetFloat(SModel sm,  string field)
        {
            string str = sm.GetString(field);

            float res = 0;

            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }

            if (!float.TryParse(str, out res))
            {
                return 0;
            }

            return res;
        }


        #region 同步拣货订单表v2

        /// <summary>
        /// 获取销售订单源数据
        /// </summary>
        /// <returns></returns>
        public static SModelList GetSaleOrderListV2()
        {
            string sql = $"select s.*, c.Code as CustomerCode, c.Name as CustomerName from SaleOrder s left join Customer c on c.ID = s.CustomerID";

            SModelList list = new SModelList();

            using (DbDecipher decipher = new SqlServer2005Decipher(SDbConn))
            {
                decipher.Open();

                list = decipher.GetSModelList(sql);
            }

            return list;
        }


        /// <summary>
        /// 同步质检订单
        /// </summary>
        /// <returns></returns>
        public static bool SyncQcOrder()
        {
            SModelList list = GetSaleOrderListV2();

            if (list.Count == 0)
            {
                return true;
            }

            log.Debug($"准备同步质检数据，数量：{list.Count}");

            int orderAddCount = 0;
            int orderUpCount = 0;
            int orderItemAddCount = 0;
            int orderItemUpCount = 0;

            Dictionary<int, SModel> ut71List = new Dictionary<int, SModel>();
            Dictionary<int, SModel> ut83List = new Dictionary<int, SModel>();

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    foreach (var item in list)
                    {
                        int pkid = TryGetInt(item, "ID");
                        string billNo = item.GetString("BillNo");
                        int state = TryGetInt(item, "State");

                        if (string.IsNullOrWhiteSpace(billNo))
                        {
                            continue;
                        }

                        int customerID = TryGetInt(item, "CustomerID");
                        if (!ut71List.TryGetValue(customerID, out SModel ut71))
                        {
                            ut71 = GetUT71(decipher, customerID);
                            ut71List.Add(customerID, ut71);
                        }

                        SModel ut101 = ExistsPickingOrderV2(decipher, pkid);
                        int orderId = 0;
                        if (ut101 == null)
                        {
                            //新增
                            LModel lm = new LModel("UT_101")
                            {
                                ["COL_30"] = TryGetInt(item, "ID"),
                                ["COL_27"] = item.GetString("BillNo"),
                                ["COL_1"] = item.GetString("BillNo"),
                                ["COL_4"] = TryGetDateTime(item, "FillDate"),
                                ["COL_132"] = item.GetString("OutTypeID"),
                                //[""] = item.GetString("DepotID"),
                                ["COL_75"] = TryGetDateTime(item, "PlanDeliverDate"),
                                ["COL_59"] = TryGetDateTime(item, "PlanDeliverDate"),
                                ["COL_104"] = TryGetDateTime(item, "EditDate"),
                                ["BIZ_SID"] = TryGetInt(item, "State"),
                                ["COL_92"] = TryGetInt(item, "CustomerID"),
                                ["COL_32"] = item.GetString("CustomerCode"),
                                ["COL_33"] = item.GetString("CustomerName"),
                                ["COL_133"] = item.GetString("LinkMan"),
                                ["COL_134"] = item.GetString("Address"),
                                ["COL_135"] = item.GetString("Tel"),
                                ["COL_11"] = item.GetString("TabMan"),
                                ["COL_136"] = TryGetDateTime(item, "ConfirmDate"),
                                //[""] = item.GetString("EndCaseDate"),
                                //[""] = item.GetString("SaleMan"),
                                //[""] = item.GetString("PayTypeID"),
                                ["COL_138"] = item.GetString("TransTypeName"),
                                ["COL_139"] = item.GetString("FeeTypeName"),
                                //[""] = item.GetString("FeeType"),
                                //[""] = item.GetString("TranFeeTotal"),
                                ["COL_137"] = TryGetFloat(item, "MustReceiveT"),
                                //[""] = item.GetString("PreReceiveTotal"),
                                ["COL_140"] = item.GetString("CoinName"),
                                //[""] = item.GetString("CoinRate"),
                                //[""] = item.GetString("ContainTax"),
                                ["COL_141"] = TryGetFloat(item, "TaxRate"),
                                ["COL_6"] = item.GetString("Des"),
                                ["COL_109"] = item.GetString("SourceDes"),
                                ["COL_143"] = TryGetInt(item, "NewOrderType"),
                                ["COL_144"] = item.GetString("FrameworkID"),
                                ["COL_142"] = TryGetInt(item, "IsPart"),
                                ["COL_99"] = item.GetString("LiftNo"),

                                ["COL_131"] = item.GetString("BillNo"),
                                ["COL_25"] = item.GetString("SaleMan"),

                                ["ROW_DATE_CREATE"] = DateTime.Now
                            };

                            if (state == 0)
                            {
                                lm["BIZ_SID"] = 0;
                            }
                            else if (state == 1)
                            {
                                lm["BIZ_SID"] = 2;
                            }
                            else if (state == 3)
                            {
                                lm["BIZ_SID"] = 4;
                            }

                            if (ut71 != null)
                            {
                                lm["COL_31"] = TryGetInt(ut71, "ROW_IDENTITY_ID");
                            }

                            if (lm.Get<int>("COL_142") == 1)
                            {
                                lm["COL_94"] = 103;
                                lm["COL_16"] = "散件部";
                                lm["COL_83"] = 101;
                                lm["COL_84"] = "散件订单";
                            }
                            else
                            {
                                lm["COL_94"] = 104;
                                lm["COL_16"] = "整梯配货部";
                                lm["COL_83"] = 102;
                                lm["COL_84"] = "整梯订单";
                            }

                            lm["COL_72"] = 101;
                            lm["COL_73"] = "未开始";
                            lm["COL_126"] = 101;
                            lm["COL_127"] = "未开始";
                            lm["COL_117"] = 0;
                            lm["COL_118"] = 0;
                            lm["COL_119"] = 0;

                            decipher.InsertModel(lm);
                            orderId = lm.Get<int>("ROW_IDENTITY_ID");
                            orderAddCount += 1;

                            ut101 = new SModel();
                            ut101["COL_31"] = lm["COL_31"];
                            ut101["COL_92"] = lm["COL_92"];
                            ut101["COL_32"] = lm["COL_32"];
                            ut101["COL_33"] = lm["COL_33"];

                            ut101["COL_94"] = lm["COL_94"];
                            ut101["COL_16"] = lm["COL_16"];
                            ut101["COL_4"] = lm["COL_4"];
                            ut101["COL_11"] = lm["COL_11"];
                            ut101["COL_6"] = lm["COL_6"];
                        }
                        else
                        {
                            //更新
                            //ut101["COL_30"] = TryGetInt(item, "ID");
                            ut101["COL_27"] = item.GetString("BillNo");
                            ut101["COL_1"] = item.GetString("BillNo");
                            ut101["COL_4"] = TryGetDateTime(item, "FillDate");
                            ut101["COL_132"] = item.GetString("OutTypeID");
                            //ut101[""] = item.GetString("DepotID");
                            ut101["COL_75"] = TryGetDateTime(item, "PlanDeliverDate");
                            ut101["COL_59"] = TryGetDateTime(item, "PlanDeliverDate");
                            ut101["COL_104"] = TryGetDateTime(item, "EditDate");
                            //////////ut101["BIZ_SID"] = TryGetInt(item, "State");
                            ut101["COL_92"] = TryGetInt(item, "CustomerID");
                            ut101["COL_32"] = item.GetString("CustomerCode");
                            ut101["COL_33"] = item.GetString("CustomerName");
                            ut101["COL_133"] = item.GetString("LinkMan");
                            ut101["COL_134"] = item.GetString("Address");
                            ut101["COL_135"] = item.GetString("Tel");
                            ut101["COL_11"] = item.GetString("TabMan");
                            ut101["COL_136"] = TryGetDateTime(item, "ConfirmDate");
                            //ut101[""] = item.GetString("EndCaseDate");
                            //ut101[""] = item.GetString("SaleMan");
                            //ut101[""] = item.GetString("PayTypeID");
                            ut101["COL_138"] = item.GetString("TransTypeName");
                            ut101["COL_139"] = item.GetString("FeeTypeName");
                            //ut101[""] = item.GetString("FeeType");
                            //ut101[""] = item.GetString("TranFeeTotal");
                            ut101["COL_137"] = TryGetFloat(item, "MustReceiveT");
                            //ut101[""] = item.GetString("PreReceiveTotal");
                            ut101["COL_140"] = item.GetString("CoinName");
                            //ut101[""] = item.GetString("CoinRate");
                            //ut101[""] = item.GetString("ContainTax");
                            ut101["COL_141"] = TryGetFloat(item, "TaxRate");
                            ut101["COL_6"] = item.GetString("Des");
                            ut101["COL_109"] = item.GetString("SourceDes");
                            ut101["COL_143"] = TryGetInt(item, "NewOrderType");
                            ut101["COL_144"] = item.GetString("FrameworkID");
                            ut101["COL_142"] = TryGetInt(item, "IsPart");
                            ut101["COL_99"] = item.GetString("LiftNo");
                            ut101["COL_131"] = item.GetString("BillNo");
                            ut101["COL_25"] = item.GetString("SaleMan");

                            if (state == 0)
                            {
                                ut101["BIZ_SID"] = 0;
                            }
                            else if (state == 1)
                            {
                                ut101["BIZ_SID"] = 2;
                            }
                            else if (state == 3)
                            {
                                ut101["BIZ_SID"] = 4;
                            }

                            if (ut71 != null)
                            {
                                ut101["COL_31"] = TryGetInt(ut71, "ROW_IDENTITY_ID");
                            }

                            if (TryGetInt(ut101, "COL_142") == 1)
                            {
                                ut101["COL_94"] = 103;
                                ut101["COL_16"] = "散件部";
                                ut101["COL_83"] = 101;
                                ut101["COL_84"] = "散件订单";
                            }
                            else
                            {
                                ut101["COL_94"] = 104;
                                ut101["COL_16"] = "整梯配货部";
                                ut101["COL_83"] = 102;
                                ut101["COL_84"] = "整梯订单";
                            }

                            ut101["ROW_DATE_UPDATE"] = DateTime.Now;
                            decipher.UpdateSModel(ut101, "UT_101", $" ROW_IDENTITY_ID = {ut101["ROW_IDENTITY_ID"]} ");
                            orderId = TryGetInt(ut101, "ROW_IDENTITY_ID");
                            orderUpCount += 1;
                        }

                        //根据单号获取最新的明细数据
                        SModelList itemList = GetSaleOrderItemListV2(billNo);
                        //根据单号删除原来明细数据
                        DeleteSaleOrderItemV2(decipher, billNo);

                        try
                        {
                            //转换新增的明细数据
                            foreach (var itemData in itemList)
                            {
                                int itemId = TryGetInt(itemData, "ID");

                                int goodId = TryGetInt(itemData, "GoodsID");
                                if (!ut83List.TryGetValue(goodId, out SModel ut83))
                                {
                                    ut83 = GetUT83(decipher, goodId);
                                    ut83List.Add(goodId, ut83);
                                }

                                SModel utsm104 = ExistsPickingOrderItemV2(decipher, itemId);
                                if (utsm104 == null)
                                {
                                    LModel ut104 = TransitionUT_104(itemData);
                                    ut104["COL_12"] = orderId;
                                    ut104["ROW_DATE_CREATE"] = DateTime.Now;

                                    ut104["COL_107"] = ut101["COL_31"];
                                    ut104["COL_180"] = ut101["COL_92"];
                                    ut104["COL_108"] = ut101["COL_32"];
                                    ut104["COL_109"] = ut101["COL_33"];

                                    ut104["COL_72"] = ut101["COL_94"];
                                    ut104["COL_73"] = ut101["COL_16"];
                                    ut104["COL_103"] = ut101["COL_4"];
                                    ut104["COL_104"] = ut101["COL_11"];
                                    ut104["COL_116"] = ut101["COL_6"];

                                    if (ut83 != null)
                                    {
                                        ut104["COL_88"] = TryGetInt(ut83, "ROW_IDENTITY_ID");

                                        ut83["COL_94"] = ut104.Get<string>("COL_31");
                                        ut83["ROW_DATE_UPDATE"] = DateTime.Now;
                                        decipher.UpdateSModel(ut83, "UT_083", $" ROW_IDENTITY_ID = {ut83["ROW_IDENTITY_ID"]} ");
                                    }

                                    decipher.InsertModel(ut104);
                                    orderItemAddCount += 1;
                                }
                                else
                                {
                                    TransitionUT_104(itemData, utsm104);
                                    utsm104["ROW_SID"] = 0;
                                    utsm104["COL_12"] = orderId;

                                    utsm104["COL_107"] = ut101["COL_31"];
                                    utsm104["COL_180"] = ut101["COL_92"];
                                    utsm104["COL_108"] = ut101["COL_32"];
                                    utsm104["COL_109"] = ut101["COL_33"];

                                    utsm104["COL_72"] = ut101["COL_94"];
                                    utsm104["COL_73"] = ut101["COL_16"];
                                    utsm104["COL_103"] = ut101["COL_4"];
                                    utsm104["COL_104"] = ut101["COL_11"];
                                    utsm104["COL_116"] = ut101["COL_6"];

                                    if (ut83 != null)
                                    {
                                        utsm104["COL_88"] = TryGetInt(ut83, "ROW_IDENTITY_ID");

                                        ut83["COL_94"] = utsm104.GetString("COL_31");
                                        ut83["ROW_DATE_UPDATE"] = DateTime.Now;
                                        decipher.UpdateSModel(ut83, "UT_083", $" ROW_IDENTITY_ID = {ut83["ROW_IDENTITY_ID"]} ");
                                    }

                                    utsm104["ROW_DATE_UPDATE"] = DateTime.Now;
                                    decipher.UpdateSModel(utsm104, "UT_104", $" ROW_IDENTITY_ID = {utsm104["ROW_IDENTITY_ID"]} ");
                                    orderItemUpCount += 1;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            log.Error("出错", e);
                        }

                    }

                    log.Debug($"新增订单（ut101），数量：{orderAddCount}");
                    log.Debug($"更新订单（ut101），数量：{orderUpCount}");
                    log.Debug($"新增订单明细（ut104），数量：{orderItemAddCount}");
                    log.Debug($"更新订单明细（ut104），数量：{orderItemUpCount}");
                }
            }
            catch (Exception ex)
            {
                log.Error($"同步质检数据出错，源数据数量：{list.Count}", ex);

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
        public static SModel ExistsPickingOrderV2(DbDecipher decipher, int id)
        {
            LightModelFilter filter = new LightModelFilter("UT_101");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_30", id);

            SModel model = decipher.GetSModel(filter);

            return model;
        }

        #endregion


        #region 同步拣货订单明细表v2


        /// <summary>
        /// 拣货订单明细是否存在
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SModel ExistsPickingOrderItemV2(DbDecipher decipher, int id)
        {
            LightModelFilter filter = new LightModelFilter("UT_104");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_179", id);

            SModel model = decipher.GetSModel(filter);

            return model;
        }


        /// <summary>
        /// 根据单号获取销售订单明细源数据
        /// </summary>
        /// <param name="billNo"></param>
        /// <returns></returns>
        public static SModelList GetSaleOrderItemListV2(string billNo)
        {
            string sql = $"select o.*,  " +
            $"g.Code as GoodsCode, " +
            $"g.Name as GoodsName, " +
            $"d.Code as DepotCode, " +
            $"d.Name as DepotName, " +
            $"g.Property as GoodsProperty " +
            $" from SaleOrderList o " +
            $" left join Goods g on g.ID = o.GoodsID" +
            $" left join Depot d on d.TreeID = o.DepotID";

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
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static LModel TransitionUT_104(SModel item)
        {
            LModel lm104 = new LModel("UT_104")
            {
                ["BIZ_SID"] = 2,
                ["COL_179"] = TryGetInt(item, "ID"),
                ["COL_11"] = item.GetString("BillNo"),
                ["COL_16"] = item.GetString("BillNo"),
                ["COL_278"] = TryGetInt(item, "GoodsID"),
                ["COL_90"] = item.GetString("Model"),
                ["COL_91"] = item.GetString("GoodsCode"),
                ["COL_92"] = item.GetString("GoodsName"),
                /////["COL_88"] = TryGetInt(item, ""),
                ["COL_48"] = TryGetDecimal(item, "PackQty1"),
                ["COL_47"] = item.GetString("PackUnit1"),
                ["COL_4"] = item.GetString("Unit"),
                ["COL_49"] = TryGetDecimal(item, "Piece1"),
                ["COL_257"] = TryGetFloat(item, "Remainder1"),
                ["COL_55"] = TryGetDecimal(item, "Qty1"),
                ["COL_7"] = TryGetDecimal(item, "Qty1"),
                ["COL_9"] = TryGetDecimal(item, "Qty1"),
                ["COL_259"] = TryGetDecimal(item, "PiecePrice1"),
                ["COL_258"] = TryGetDecimal(item, "Price"),
                ["COL_260"] = TryGetDecimal(item, "OutPrice"),
                ["COL_261"] = TryGetDecimal(item, "CostPrice"),
                ["COL_262"] = item.GetString("Discount"),
                ["COL_263"] = TryGetDecimal(item, "Total"),
                ["COL_264"] = item.GetString("DepotID"),
                ["COL_10"] = item.GetString("Des"),
                ["COL_276"] = item.GetString("SourceBillNo"),
                ["COL_241"] = TryGetFloat(item, "WeightRate"),
                ["COL_266"] = item.GetString("FindMan"),
                ["COL_240"] = item.GetString("OrderNo"),
                ["COL_267"] = item.GetString("CSR_pn"),
                ["COL_268"] = item.GetString("CSR_pr"),
                ["COL_269"] = item.GetString("CSR_psn"),
                ["COL_270"] = item.GetString("CSR_dwg"),
                ["COL_271"] = item.GetString("CSR_ps"),
                ["COL_272"] = item.GetString("CSR_ctn"),
                ["COL_277"] = item.GetString("CSR_ID"),
                ["COL_275"] = item.GetString("CSR_ut"),
                ["COL_273"] = item.GetString("CSR_Qty"),
                ["COL_274"] = item.GetString("CSR_NO"),

                ["COL_31"] = item.GetString("GoodsProperty"),
                ["COL_89"] = item.GetString("GoodsCode"),
                ["COL_3"] = item.GetString("GoodsName")
            };


            lm104["COL_242"] = TryGetDecimal(item, "WeightRate") * TryGetDecimal(item, "Qty1");
            lm104["COL_245"] = item.GetString("OrderNo") + item.GetString("BillNo");

            lm104["COL_195"] = 101;
            lm104["COL_32"] = "未开始";
            lm104["COL_243"] = 101;
            lm104["COL_244"] = "未开始";
            lm104["COL_159"] = 0;
            lm104["COL_160"] = "0";
            lm104["COL_161"] = 0;
            lm104["COL_44"] = 1;
            lm104["COL_76"] = 1;
            lm104["COL_46"] = 100;
            lm104["COL_77"] = "100%";
            lm104["COL_255"] = 1;

            return lm104;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ut104"></param>
        public static void TransitionUT_104(SModel item, SModel ut104)
        {
            //更新
            //ut91.SetTakeChange(true);
            ////ut91["BIZ_SID"] = 2;
            ut104["COL_11"] = item.GetString("BillNo");
            ut104["COL_16"] = item.GetString("BillNo");
            ut104["COL_278"] = TryGetInt(item, "GoodsID");
            ut104["COL_90"] = item.GetString("Model");
            ut104["COL_91"] = item.GetString("GoodsCode");
            ut104["COL_92"] = item.GetString("GoodsName");
            /////ut104["COL_88"] = TryGetInt(item, "");
            ut104["COL_48"] = TryGetDecimal(item, "PackQty1");
            ut104["COL_47"] = item.GetString("PackUnit1");
            ut104["COL_4"] = item.GetString("Unit");
            ut104["COL_49"] = TryGetDecimal(item, "Piece1");
            ut104["COL_257"] = TryGetFloat(item, "Remainder1");
            ut104["COL_55"] = TryGetDecimal(item, "Qty1");
            ut104["COL_7"] = TryGetDecimal(item, "Qty1");
            ut104["COL_9"] = TryGetDecimal(item, "Qty1");
            ut104["COL_259"] = TryGetDecimal(item, "PiecePrice1");
            ut104["COL_258"] = TryGetDecimal(item, "Price");
            ut104["COL_260"] = TryGetDecimal(item, "OutPrice");
            ut104["COL_261"] = TryGetDecimal(item, "CostPrice");
            ut104["COL_262"] = item.GetString("Discount");
            ut104["COL_263"] = TryGetDecimal(item, "Total");
            ut104["COL_264"] = item.GetString("DepotID");
            ut104["COL_10"] = item.GetString("Des");
            ut104["COL_276"] = item.GetString("SourceBillNo");
            ut104["COL_241"] = TryGetFloat(item, "WeightRate");
            ut104["COL_266"] = item.GetString("FindMan");
            ut104["COL_240"] = item.GetString("OrderNo");
            ut104["COL_267"] = item.GetString("CSR_pn");
            ut104["COL_268"] = item.GetString("CSR_pr");
            ut104["COL_269"] = item.GetString("CSR_psn");
            ut104["COL_270"] = item.GetString("CSR_dwg");
            ut104["COL_271"] = item.GetString("CSR_ps");
            ut104["COL_272"] = item.GetString("CSR_ctn");
            ut104["COL_277"] = item.GetString("CSR_ID");
            ut104["COL_275"] = item.GetString("CSR_ut");
            ut104["COL_273"] = item.GetString("CSR_Qty");
            ut104["COL_274"] = item.GetString("CSR_NO");

            ut104["COL_242"] = TryGetDecimal(item, "WeightRate") * TryGetDecimal(item, "Qty1");
            ut104["COL_245"] = item.GetString("OrderNo") + item.GetString("BillNo");

            ut104["COL_31"] = item.GetString("GoodsProperty");
            ut104["COL_255"] = 1;
            ut104["COL_89"] = item.GetString("GoodsCode");
            ut104["COL_3"] = item.GetString("GoodsName");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="billNo"></param>
        public static void DeleteSaleOrderItemV2(DbDecipher decipher, string billNo)
        {
            //LightModelFilter filterDel = new LightModelFilter("UT_091");
            //filterDel.And("COL_19", billNo);

            SModel upValue = new SModel()
            {
                ["ROW_SID"] = -3,
                ["ROW_DATE_DELETE"] = DateTime.Now
            };

            decipher.UpdateSModel(upValue, "UT_104", $" COL_11 = '{billNo}' ");
        }

        #endregion


        #region 同步Bom

        public static SModelList GetBomList()
        {
            string sql = $"select b1.ID,b1.ParentID, g1.Name as bjName, g1.WeightRate as bjWeightRate, g1.Code as bjCode, g2.Name as cpName, g2.WeightRate as cpWeightRate, g2.Code as cpCode  from bom b1 " +
                $"left join bom b2 on b2.ID = b1.ParentID " +
                $"left join Goods g1 on g1.ID = b1.GoodsID " +
                $"left join Goods g2 on g2.ID = b2.GoodsID ";

            string sqlStr = "select b1.ID AS COL_81, b1.ParentID AS COL_46, " +
                "b2.GoodsID as COL_1, g2.Code as COL_2, g2.Name as COL_3, " +
                "b1.GoodsID as COL_44, b1.Model as COL_9, b1.DepotID as COL_22, " +
                "b1.UseQty as COL_11, b1.Origin as COL_35, b1.Wastage as COL_12, " +
                "b1.Des as COL_16, b1.ManHaur as COL_59, g1.WeightRate as COL_136, " +
                "g1.Code as COL_7, g1.Name as COL_8 from bom b1 " +
                "left join bom b2 on b2.ID = b1.ParentID " +
                "left join Goods g1 on g1.ID = b1.GoodsID " +
                "left join Goods g2 on g2.ID = b2.GoodsID ";

            SModelList list = new SModelList();

            using (DbDecipher decipher = new SqlServer2005Decipher(SDbConn))
            {
                decipher.Open();

                list = decipher.GetSModelList(sqlStr);
            }

            return list;
        }


        /// <summary>
        /// 拣货订单明细是否存在
        /// </summary>
        /// <param name="decipher"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SModel ExistsUT181(DbDecipher decipher, int id)
        {
            LightModelFilter filter = new LightModelFilter("UT_191");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_81", id);

            SModel model = decipher.GetSModel(filter);

            return model;
        }


        /// <summary>
        /// 同步Bom到UT_191
        /// </summary>
        /// <returns></returns>
        public static bool SyncBom()
        {
            SModelList bomList = GetBomList();

            if (bomList.Count == 0) 
            {
                return true;
            }

            log.Debug($"准备同步Bom表，数据量：{bomList.Count}");

            Dictionary<int, SModel> ut83List = new Dictionary<int, SModel>();

            try
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen()) 
                {
                    foreach (var bom in bomList)
                    {
                        int goodId = TryGetInt(bom, "COL_44");

                        if (!ut83List.TryGetValue(goodId, out SModel ut83))
                        {
                            ut83 = GetUT83(decipher, goodId);
                            ut83List.Add(goodId, ut83);
                        }

                        if (ut83 != null) 
                        {
                            bom["COL_6"] = TryGetInt(ut83, "ROW_IDENTITY_ID");
                        }

                        SModel sm191 = ExistsUT181(decipher, TryGetInt(bom, "COL_81"));

                        if (sm191 == null)
                        {
                            LModel lm = new LModel("UT_191")
                            {
                                ["ROW_DATE_CREATE"] = DateTime.Now,
                                ["ROW_SID"] = 0
                            };

                            foreach (var item in bom.GetFields())
                            {
                                lm[item] = bom[item];
                            }

                            decipher.InsertModel(lm);
                        }
                        else 
                        {
                            foreach (var item in bom.GetFields())
                            {
                                sm191[item] = bom[item];
                            }

                            sm191["ROW_DATE_UPDATE"] = DateTime.Now;
                            decipher.UpdateSModel(sm191, "UT_191", $" ROW_IDENTITY_ID = {sm191["ROW_IDENTITY_ID"]} ");
                        }
                    }
                }

                log.Debug("SyncBom完成...");

                return true;
            }
            catch (Exception e)
            {
                log.Error("同步Bom表出错", e);

                return false;
            }
        }

        #endregion


    }
}