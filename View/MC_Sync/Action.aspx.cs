using System;
using System.Collections.Generic;
using App.BizCommon;
using EC5.IO;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System.Xml.Serialization;
using System.ComponentModel;
using EC5.Utility;
using System.Data.SqlClient;
using EC5.AppDomainPlugin;
using System.Text;
using System.Transactions;
using EC5.Entity.Expanding.ExpandV1;
using HWQ.Entity;

namespace App.InfoGrid2.MC_Sync
{
    /// <summary>
    /// 同步程序
    /// </summary>
    public partial class Action : EC5.SystemBoard.Web.UI.Page, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            string sync_type = WebUtil.QueryTrimLower("sync_type");


            HttpResult result = null;

            try
            {
                result = ProAction(sync_type);
            }
            catch(Exception ex)
            {
                log.Error("处理动作错误.", ex);
                result = HttpResult.Error("501", "处理同步错误:" + ex.Message);
            }

            if(result == null)
            {
                result = HttpResult.Error("500","未知错误");
            }
            
            Response.Clear();
            Response.Write(result);
            Response.End();
        }

        private HttpResult ProAction(string sync_type)
        {
            HttpResult result = null;

            switch (sync_type)
            {
                case "table":

                    using (TransactionScope tsCope = new TransactionScope())
                    {
                        using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                        {
                            result = ProTabel(decipher);

                            tsCope.Complete();
                        }
                    }

                    break;
                case "form":

                    try
                    {
                        result = ProForm();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("处理表单异常.", ex);
                    }
                    

                    break;
                case "text":

                    using (TransactionScope tsCope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        using (DbDecipher decipehr = DbDecipherManager.GetDecipherOpen())
                        {

                            ProProrr(decipehr);

                            tsCope.Complete();
                        }
                    }

                    break;

                default: result = HttpResult.Error("502", "非法指令"); break;
            }

            return result;
        }

        /// <summary>
        /// 处理同步单表
        /// </summary>
        /// <returns></returns>
        private HttpResult ProTabel(DbDecipher decipher)
        {
            HttpResult result = null;

            string table = WebUtil.FormTrimUpper("table");
            string idfield = WebUtil.FormTrimUpper("id_field");
            string row_id = WebUtil.FormTrim("row_id");

            string c_state_code = WebUtil.FormTrimUpper("c_state_code");
            string c_state = WebUtil.FormTrim("c_state");

            string data = WebUtil.Form("data");


            log.Debug("接收到[单表]数据:\n\n " + data);
            log.Debug("-------------------------");


            SModel sm = SModel.ParseJson(data);


            BizDecipher bizDecipher = new BizDecipher(decipher);

            var f3 = new BizFilter(table);
            f3.And(idfield, row_id);

            var curModel = bizDecipher.GetModel(f3);


            if (c_state_code == "A" || c_state_code == "E")
            {
                if (curModel != null)
                {
                    curModel.SetTakeChange(true);
                    curModel.TrySetValues(sm);

                    bizDecipher.BizUpdateModel(curModel, true);
                }
                else
                {
                    curModel = new LModel(table);

                    curModel.TrySetValues(sm);

                    decipher.InsertModel(curModel);
                }

                result = HttpResult.Success();
            }
            else if (c_state_code == "D")
            {
                if (curModel == null)
                {
                    result = HttpResult.SuccessMsg("记录已经删除,当前命令忽略");
                }
                else
                {
                    bizDecipher.BizDeleteModel(curModel);
                }
            }


            return result;
        }


        public class Join
        {
            public string to_join_table { get; set; }

            public string join_table { get; set; }

            List<JoinItem> m_Items = new List<JoinItem>();

            public List<JoinItem> items
            {
                get
                {
                    if(m_Items == null)
                    {
                        m_Items = new List<JoinItem>();
                    }
                    return m_Items;
                }
                set { m_Items = value; }
            }

        }

        public class JoinItem
        {
            public string to_field { get; set; }

            public string to_join_field { get; set; }
        }




        private HttpResult ProForm_A( DbDecipher decipher, SModel cfg, SModel hand, SModelList sub_list)
        {
            HttpResult result = null;

            dynamic handCfg = cfg["hand"];
            SModelList subListCfg = cfg["sub_list"];

            SModel sub0Cfg = subListCfg[0];



            SModelList joinsCfg = sub0Cfg["joins"];

            Join curJoin = null;

            if (joinsCfg != null)
            {
                SModel joinCfg = joinsCfg[0];

                curJoin = joinCfg.DeserializeObject<Join>();
            }

            dynamic subCfg0 = subListCfg[0];


            #region 构造本地的新记录实体

            LModel hand_lm = CC(decipher, handCfg.to_table, handCfg, hand);


            List<LModel> sub_list_lm = new List<LModel>();

            foreach (var sub in sub_list)
            {
                LModel sub_lm = CC(decipher, subCfg0.to_table, subCfg0, sub); // new LModel(sub0["to_table"]);

                sub_lm["BIZ_SID"] = 0;

                sub_list_lm.Add(sub_lm);
            }

            #endregion


            BizFilter curFilter = new BizFilter(handCfg.to_table);
            curFilter.And("SRC_ID", hand_lm["SRC_ID"]);

            bool exist = decipher.ExistsModels(curFilter);

            if (exist)
            {
                return HttpResult.SuccessMsg("记录已经存在, 不能再添加了..");
            }

            decipher.BeginTransaction();

            try
            {
                decipher.InsertModel(hand_lm);

                if (curJoin != null)
                {
                    foreach (var sub in sub_list_lm)
                    {
                        foreach (var joinItem in curJoin.items)
                        {
                            sub[joinItem.to_field] = hand_lm[joinItem.to_join_field];
                        }
                    }
                }


                decipher.InsertModels(sub_list_lm);

                decipher.TransactionCommit();

            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                throw new Exception("插入记录失败", ex);
            }

            if (hand_lm.GetModelName() == "UT_112")
            {
                #region 求数量

                object col_12_v = hand_lm["COL_12"];

                BizFilter bizFilter = new BizFilter("UT_112");
                bizFilter.And("COL_12", col_12_v);
                bizFilter.Fields = new string[] { "ROW_IDENTITY_ID" };

                LModelReader reader = decipher.GetModelReader(bizFilter);

                int[] mainIds = ModelHelper.GetColumnData<int>(reader);



                bizFilter = new BizFilter("UT_113");
                bizFilter.And("COL_1", mainIds, Logic.In);
                bizFilter.Fields = new string[] { "sum(COL_27)" };

                decimal col27 = decipher.ExecuteScalar<decimal>(bizFilter);


                bizFilter = new BizFilter("UT_113");
                bizFilter.And("COL_1", hand_lm.GetPk());

                int uCount = decipher.UpdateProps(bizFilter, new object[] { "COL_73", col27 });


                #endregion


            }

            hand_lm = Init_BizSid_0to2( decipher, hand_lm);

            //触发联动
            hand_lm["BIZ_SID"] = 2;
            hand_lm.SetTakeChange(true);
            hand_lm["BIZ_SID"] = 4;

            try
            {
                EC5.IG2.BizBase.DbCascadeRule.Update(decipher, null, hand_lm);

            }
            catch (Exception ex)
            {
                throw new Exception($"执行联动.", ex);
            }
            

            #region  这个先删除掉先

            ////这个是更新库存的类来的
            //View.CustomPage.T1267_546_Page page = new View.CustomPage.T1267_546_Page();
            ////这是执行更新库存函数
            //page.ChangeBizSidCurrency(new string[] { hand_lm.GetPk().ToString()});

            #endregion

            result = HttpResult.Success();

            return result;
        }

        private LModel Init_BizSid_0to2(DbDecipher decipher, LModel hand_lm)
        {

            string modelName = hand_lm.GetModelName();

            if (modelName == "UT_112" || modelName == "UT_110")
            {

                //触发联动
                hand_lm["BIZ_SID"] = 0;
                hand_lm.SetTakeChange(true);
                hand_lm["BIZ_SID"] = 2;
                
                try
                {
                    using (DbDecipher decipher2 = DbDecipherManager.GetDecipherOpen())
                    {
                        EC5.IG2.BizBase.DbCascadeRule.Update(decipher, null, hand_lm);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("UT_112 业务状态 0->2 导致的错误.", ex);
                }
                

                LModelElement modelElemX = LightModel.GetLModelElement(hand_lm.GetModelName());

                LightModelFilter filterX = new LightModelFilter(hand_lm.GetModelName());
                filterX.And(modelElemX.PrimaryKey, hand_lm.GetPk());
                filterX.And("BIZ_SID", 2);

                hand_lm = decipher.GetModel(filterX);

            }

            return hand_lm;
        }

        private HttpResult ProForm_E(DbDecipher decipher, SModel cfg, SModel hand, SModelList sub_list)
        {
            BizDecipher bizDecipher = new BizDecipher(decipher);

            HttpResult result = null;

            dynamic handCfg = cfg["hand"];
            SModelList subListCfg = cfg["sub_list"];

            SModel sub0Cfg = subListCfg[0];
            
            SModelList joinsCfg = sub0Cfg["joins"];


            Join curJoin = null;

            if (joinsCfg != null)
            {
                SModel joinCfg = joinsCfg[0];

                curJoin = joinCfg.DeserializeObject<Join>();
            }

            dynamic sub0 = subListCfg[0];

            

            #region 构造本地的新记录实体

            var filter = new BizFilter(handCfg.to_table);
            filter.And("SRC_ID", hand["SRC_ID"]);

            LModel hand_lm = bizDecipher.GetModel(filter);   // CC(handCfg.to_table, handCfg, hand);

            if(hand_lm == null)
            {
                //记录丢失, 重新创建一条

                return ProForm_A(decipher, cfg, hand, sub_list);
            }


            int bizSid = hand_lm.Get<int>("BIZ_SID");

            if (bizSid == 4)
            {
                hand_lm.SetTakeChange(true);

                //触发联动

                hand_lm["BIZ_SID"] = 2;


                try
                {


                    log.Debug($"联动-准备开始. {handCfg.to_table}.BIZ_SID = 4->2 ");


                    EC5.IG2.BizBase.DbCascadeRule.Update(decipher, null, hand_lm);
                    

                    log.Debug($"联动-成功. {handCfg.to_table}.BIZ_SID = 4->2 ");


                }
                catch (Exception ex)
                {
                    throw new Exception($"执行联动, 业务状态 {handCfg.to_table}.BIZ_SID = 4->2 发生错误. \r\n\r\n", ex);
                }
                
            }



            // 重新拿回实体

            filter = new BizFilter(handCfg.to_table);
            filter.And("SRC_ID", hand["SRC_ID"]);
            filter.And("BIZ_SID", 2);

            try
            {
                hand_lm = bizDecipher.GetModel(filter);   // CC(handCfg.to_table, handCfg, hand);
            }
            catch(Exception ex)
            {
                throw new Exception("如果这里出错, 估计数据库死锁了.", ex);
            }

            hand_lm.SetTakeChange(true);

            hand_lm.TrySetValues(hand);

            CCEditor(decipher, hand_lm, handCfg.to_table, handCfg, hand);



            List<LModel> sub_list_new = new List<LModel>();
            List<LModel> sub_list_del = new List<LModel>();
            List<LModel> sub_list_edit = new List<LModel>();



            foreach (var sub in sub_list)
            {
                var subFilter = new BizFilter(sub0.to_table);
                subFilter.And("SRC_ID", sub["SRC_ID"]);

                LModel sub_lm = bizDecipher.GetModel(subFilter);   // CC(sub0.to_table, sub0, sub); // new LModel(sub0["to_table"]);

                string ec6StateCode = sub["EC6_STAET_CODE"];

                if (ec6StateCode == "D")
                {
                    sub_list_del.Add(sub_lm);
                }
                else
                {
                    if (sub_lm != null)
                    {
                        sub_lm.SetTakeChange(true);
                        sub_lm.TrySetValues(sub);

                        CCEditor(decipher, sub_lm, sub0.to_table, sub0Cfg, sub);

                        sub_list_edit.Add(sub_lm);
                    }
                    else
                    {
                        sub_lm = new LModel(sub0.to_table);
                        sub_lm.TrySetValues(sub);

                        CCEditor(decipher, sub_lm, sub0.to_table, sub0Cfg, sub);

                        if (curJoin != null)
                        {
                            foreach (var joinItem in curJoin.items)
                            {
                                sub_lm[joinItem.to_field] = hand_lm[joinItem.to_join_field];
                            }
                        }

                        sub_list_new.Add(sub_lm);
                    }

                    sub["ROW_SID"] = 0;
                }

            }

            #endregion


            decipher.BeginTransaction();

            try
            {
                bizDecipher.BizUpdateModel(hand_lm,true);      //更新主表

                bizDecipher.BizDeleteModels(sub_list_del);  //删除子表

                bizDecipher.BizInsertModels(sub_list_new);    //插入子表

                bizDecipher.BizUpdateModels(sub_list_edit,true);    //更新子表
               

                decipher.TransactionCommit();

            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                throw new Exception("插入记录失败", ex);
            }



            log.Debug($"修改修噶.........[{handCfg.to_table}]");



            if (handCfg.to_table == "UT_112")
            {
                log.Debug("开始统计..............");

                object col_12_v = hand_lm["COL_12"];

                log.Debug($"col_12 = {col_12_v}");

                BizFilter bizFilter = new BizFilter("UT_112");
                bizFilter.And("COL_12", col_12_v);
                bizFilter.Fields = new string[] { "ROW_IDENTITY_ID" };

                LModelReader reader = decipher.GetModelReader(bizFilter);

                int[] mainIds = ModelHelper.GetColumnData<int>(reader);


                log.Debug($"mainIds = {ArrayUtil.ToString(mainIds)}");

                bizFilter = new BizFilter("UT_113");
                bizFilter.And("COL_1", mainIds, Logic.In);
                bizFilter.Fields = new string[] { "sum(COL_27)" };

                decimal col27 = decipher.ExecuteScalar<decimal>(bizFilter);


                log.Debug($"sum(col_27) = {col27}");

                bizFilter = new BizFilter("UT_113");
                bizFilter.And("COL_1", hand_lm.GetPk());

                int uCount = decipher.UpdateProps(bizFilter, new object[] { "COL_73", col27 });

                log.Debug($"共更新记录 {uCount} 条,字段 UT_113.COL_73 = {col27}");


                log.Debug("结束统计...............................");
            }


            hand_lm = Init_BizSid_0to2(decipher, hand_lm);

            //触发联动

            hand_lm["BIZ_SID"] = 2;

            hand_lm.SetTakeChange(true);

            hand_lm["BIZ_SID"] = 4;


            try
            {
                log.Debug($"联动-准备开始  {handCfg.to_table}BIZ_SID = 2->4");

                EC5.IG2.BizBase.DbCascadeRule.Update(decipher, null, hand_lm);
                
                log.Debug($"联动-结束  {handCfg.to_table}BIZ_SID = 2->4");

            }
            catch (Exception ex)
            {
                throw new Exception($"联动-错了啊, {handCfg.to_table}.BIZ_SID = 2->4 \n\n", ex);
            }



            ////这个是更新库存的类来的
            //View.CustomPage.T1267_546_Page page = new View.CustomPage.T1267_546_Page();
            ////这是执行更新库存函数
            //page.ChangeBizSidCurrency(new string[] { hand_lm.GetPk().ToString() });



            result = HttpResult.Success();

            return result;
        }

        private void ProProrr(DbDecipher decipher)
        {
            int id = WebUtil.QueryInt("id");

            

            LModel hand_lm = decipher.GetModelByPk("UT_112",id);


            BizFilter bizFilter = new BizFilter("UT_113");
            bizFilter.And("COL_1", hand_lm.GetPk());
            bizFilter.Fields = new string[] { "sum(COL_27)" };

            decimal col27 = decipher.ExecuteScalar<decimal>(bizFilter);

            bizFilter = new BizFilter("UT_113");
            bizFilter.And("COL_1", hand_lm.GetPk());

            int uCount = decipher.UpdateProps(bizFilter, new object[] { "COL_73", col27 });
        }

        private HttpResult ProForm_D(DbDecipher decipher,SModel cfg, SModel hand, SModelList sub_list)
        {
            BizDecipher bizDecipher = new BizDecipher(decipher);

            HttpResult result = null;


            dynamic handCfg = cfg["hand"];
            SModelList subListCfg = cfg["sub_list"];

            SModel sub0Cfg = subListCfg[0];



            SModelList joinsCfg = sub0Cfg["joins"];

            Join curJoin = null;

            if (joinsCfg != null)
            {
                SModel joinCfg = joinsCfg[0];

                curJoin = joinCfg.DeserializeObject<Join>();
                
            }

            dynamic sub0 = subListCfg[0];



            #region 构造本地的新记录实体

            BizFilter bizfilter = new BizFilter(handCfg.to_table);


            var filter = new BizFilter(handCfg.to_table);
            filter.And("SRC_ID", hand["SRC_ID"]);
            filter.And("BIZ_SID", 4);

            LModel hand_lm = bizDecipher.GetModel(filter);   // CC(handCfg.to_table, handCfg, hand);

            hand_lm.SetTakeChange(true);

            //触发联动

            hand_lm["BIZ_SID"] = 2;

            

            using (TransactionScope tsCope = new TransactionScope())
            {
                EC5.IG2.BizBase.DbCascadeRule.Update(hand_lm);
                tsCope.Complete();
            }


            ModelTreeFilter mff = new ModelTreeFilter("主表名", ModelTeeeNodeAssc.Join("子表", "SRC_PARENT_ID", "SRC_ID"));
            mff.And("SRC_ID", hand["SRC_ID"]);
            mff.And("BIZ_SID", 2);
            
            

            //ModelFormFilter mff = new ModelFormFilter("主表名", );
            //mff.And("SRC_ID", hand["SRC_ID"]);
            //mff.And("BIZ_SID", 2);


            filter = new BizFilter(handCfg.to_table);
            filter.And("SRC_ID", hand["SRC_ID"]);
            filter.And("BIZ_SID", 2);
                        
            var subFilter = new BizFilter(sub0.to_table);
            subFilter.And("SRC_PARENT_ID", hand["SRC_ID"]);


            hand_lm = bizDecipher.GetModel(filter);
            List<LModel> sub_list_lm = bizDecipher.GetModelList(subFilter);
            
            #endregion


            decipher.BeginTransaction();

            try
            {
                bizDecipher.BizDeleteModel(hand_lm);
                bizDecipher.BizDeleteModels(sub_list_lm);
                                
                decipher.TransactionCommit();

            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                throw new Exception("删除记录失败", ex);
            }
            


            result = HttpResult.Success();

            return result;

        }


        /// <summary>
        /// 处理表单
        /// </summary>
        /// <returns></returns>
        private HttpResult ProForm()
        {
            HttpResult result = null;

            string c_state_code = WebUtil.FormTrimUpper("c_state_code");
            string c_state = WebUtil.FormTrim("c_state");

            string data = WebUtil.Form("data");

            log.Debug("接收到表单数据:\n\n " + data);
            log.Debug("-------------------------");

            SModel sm = SModel.ParseJson(data);

            SModel cfg = sm["_config"];

            

            SModel hand = sm["hand"];
            SModelList sub_list = sm["sub_list"];



            TransactionOptions tOpt = new TransactionOptions();
            tOpt.IsolationLevel = IsolationLevel.ReadCommitted;
            tOpt.Timeout = new TimeSpan(0, 5, 0);

            using (TransactionScope tsCope = new TransactionScope(TransactionScopeOption.RequiresNew, tOpt))
            {
                using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen())
                {
                    if (c_state_code == "A")
                    {
                        result = ProForm_A(decipher, cfg, hand, sub_list);
                    }
                    else if (c_state_code == "E")
                    {
                        result = ProForm_E(decipher, cfg, hand, sub_list);

                    }
                    else if (c_state_code == "D")
                    {
                        result = ProForm_D(decipher, cfg, hand, sub_list);
                    }
                }

                tsCope.Complete();

            }


            return result;

        }



        private void CCEditor(DbDecipher decipher, LModel model, string table, SModel cfg, SModel sm)
        {

            if (cfg.HasField("fields"))
            {
                SModel handFieldsCfg = cfg["fields"];

                foreach (var fieldName in handFieldsCfg.GetFields())
                {
                    string fieldXml = handFieldsCfg[fieldName];

                    MapField mapField = XmlUtil.Deserialize<MapField>(fieldXml);

                    object result = null;

                    if (mapField.FromCode != null)
                    {
                        MapFieldFromCode fc = mapField.FromCode;

                        if (fc.Lang == "sql")
                        {
                            result = ExecSQL(decipher, fc.SQL, model, sm);
                        }
                        else if (fc.Lang == "cs-script")
                        {

                        }

                    }
                    else if (mapField.IsNull != null)
                    {

                    }

                    if (result == null)
                    {
                        model[mapField.To] = mapField.Default;
                    }
                    else
                    {
                        model[mapField.To] = result;
                    }
                }
            }
        }

        private LModel CC(DbDecipher decipher, string table, SModel cfg, SModel sm)
        {

            LModel model = new LModel(table);
            model.TrySetValues(sm);

            CCEditor(decipher, model, table, cfg, sm);

            return model;
        }


        private object ExecSQL(DbDecipher decipher, string sqlTemplate, LModel toModel, SModel fromModel)
        {

            SqlTemplate st = SqlTemplate.Parse(sqlTemplate);

            SqlCommand cmd = st.Exec(toModel, fromModel);

            cmd.Connection = (SqlConnection)decipher.Connection;

            object result = null;

            try
            {
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("执行语句错误 : " + cmd.CommandText, ex);
            }

            if (DBNull.Value.Equals(result))
            {
                result = null;
            }

            return result;
        }

        


    }
}