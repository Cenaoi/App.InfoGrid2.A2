using App.BizCommon;
using App.InfoGrid2.Model.JF;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Sysboard.Web.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View
{
    /// <summary>
    /// 分销任务类  积分商城的
    /// </summary>
    public class DistrTask:WebTask
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DistrTask()
        {
            //一个小时生成一下分销明细
            this.TaskSpan = new TimeSpan(1, 0, 0);

            this.TaskType = WebTaskTypes.Span;

            this.TaskTime = DateTime.Parse("2016-1-1");
        }


        public override void Exec()
        {
            DbDecipher decipher = null;

            try
            {
                decipher = DbDecipherManager.GetDecipherOpen();
            }
            catch (Exception ex)
            {
                log.Error("打开数据库失败", ex);
                return;
            }

            try
            {
                CreateDistr(decipher);
            }
            catch (Exception ex)
            {
                log.Error("生成分销数据出错了！",ex);
            }
            finally
            {
                if (decipher != null)
                {
                    decipher.Dispose();
                }
            }

        }



        /// <summary>
        /// 创建分销数据
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        void CreateDistr(DbDecipher decipher)
        {

            LightModelFilter lmFilterOrder = new LightModelFilter(typeof(ES_ORDER));
            lmFilterOrder.And("ROW_SID", 0,Logic.GreaterThanOrEqual);
            lmFilterOrder.And("DISTR_SID", 0);
            lmFilterOrder.And("PAY_SID", 2);


            lmFilterOrder.Top = 10;

            List<ES_ORDER> orders = decipher.SelectModels<ES_ORDER>(lmFilterOrder);


            foreach(ES_ORDER order in orders)
            {

                ES_W_ACCOUNT account = GetUserByCode(decipher, order.FK_W_CODE);

                //只有用户是分销商才创建分销明细数据
                if (account != null && account.IS_DISTR)
                {
                    CreateDistrDeta(decipher,order, account);
                }


                order.DISTR_SID = 2;
                order.DISTR_DATE = DateTime.Now;
                order.ROW_DATE_UPDATE = DateTime.Now;

                decipher.UpdateModelProps(order, "DISTR_SID", "DISTR_DATE", "ROW_DATE_UPDATE");


            }

        }


        /// <summary>
        /// 创建分销数据 的具体函数
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="order">订单对象</param>
        /// <param name="account">用户对象</param>>
        void CreateDistrDeta(DbDecipher decipher, ES_ORDER order, ES_W_ACCOUNT account)
        {


            LightModelFilter lmFilter001 = new LightModelFilter("UT_001");
            lmFilter001.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter001.And("DISTR_LEVEL", 0, Logic.Inequality);

            lmFilter001.TSqlOrderBy = "DISTR_LEVEL";


            List<LModel> ut001s = decipher.SelectModels<LModel>(lmFilter001);


            List<ES_W_ACCOUNT> distr_users = GetDistrUsers(decipher, account);

            int i = 0;

            foreach(var item in distr_users)
            {
                log.Debug("用户对象："+item.W_NICKNAME);

                i++;

                LModel ut_001_level_1 = ut001s.Find(u => u.Get<int>("DISTR_LEVEL") == i);

                //第一级分销数据
                CreateDistrDeta_2(decipher, order, account, item, ut_001_level_1);

            }


        }

        /// <summary>
        /// 根据自定义主键编码获取用户对象
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="user_code">自定义主键编码</param>
        /// <returns></returns>
        ES_W_ACCOUNT GetUserByCode(DbDecipher decipher,string user_code)
        {
            //判断用户自定义主键编码是否为空
            if (string.IsNullOrWhiteSpace(user_code))
            {
                return null;
            }

            LightModelFilter lmFilterAccount = new LightModelFilter(typeof(ES_W_ACCOUNT));
            lmFilterAccount.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterAccount.And("PK_W_CODE", user_code);

            return decipher.SelectToOneModel<ES_W_ACCOUNT>(lmFilterAccount);


        }

        /// <summary>
        /// 获取到所有分销用户集合
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="account">生成订单对象</param>
        /// <param name="deep">要获取几级分销 默认 3 级</param>
        /// <returns></returns>
        List<ES_W_ACCOUNT> GetDistrUsers(DbDecipher decipher, ES_W_ACCOUNT account,int deep =3)
        {


            List<ES_W_ACCOUNT> accounts = new List<ES_W_ACCOUNT>();

            accounts.Add(account);

            //要减一个数 因为循环的索引是从0开始的
            deep--;

            //上级编码
            string p_code = account.PARENT_CODE;

            int i = 0;

            //拿两个上级就好了
            while(i < deep)
            {
                //上级编码为空也退出了！
                if (string.IsNullOrWhiteSpace(p_code))
                {
                    break;
                }

                //获取到上级对象
                ES_W_ACCOUNT account_p = GetUserByCode(decipher, p_code);

                if(account_p == null)
                {
                    break;
                }

                accounts.Add(account_p);

                p_code = account_p.PARENT_CODE;


                i++;
            }


            return accounts;



        }

        /// <summary>
        /// 根据分销级别和分销用户来创建分销明细数据
        /// </summary>
        /// <param name="decipher">数据库助手</param>
        /// <param name="order">订单对象</param>
        /// <param name="order_user">订单用户对象</param>
        /// <param name="distr_user">分销对象</param>
        /// <param name="ut_001">分销比率对象</param>
        void CreateDistrDeta_2(DbDecipher decipher, ES_ORDER order, ES_W_ACCOUNT order_user, ES_W_ACCOUNT distr_user, LModel ut_001)
        {
            //只要一个对象为空，就不用生成分销明细数据了
            if (ut_001 == null || order == null || order_user == null || distr_user == null)
            {
                log.Debug(ut_001);
                log.Debug(order);
                log.Debug(order_user);
                log.Debug(distr_user);
                return;
            }


            decimal distr_money = order.MONEY_TOTAL * ut_001.Get<decimal>("DISTR_RATE") / 100m;

            LModel ut_002 = new LModel("UT_002");

            ut_002["PK_DISTR_CODE"] = BillIdentityMgr.NewCodeForDay("DISTR_CODE", "D", 5);
            ut_002["FK_ORDER_CODE"] = order.PK_O_CODE;
            ut_002["ORDER_CODE"] = order.ORDER_CODE;
            ut_002["ORDER_W_CODE"] = order_user.PK_W_CODE;
            ut_002["ORDER_W_NICKNAME"] = order_user.W_NICKNAME;
            ut_002["ORDER_MONEY"] = order.MONEY_TOTAL;
            ut_002["DISTR_W_CODE"] = distr_user.PK_W_CODE;
            ut_002["DISTR_W_NICKNAME"] = distr_user.W_NICKNAME;
            ut_002["DISTR_LEVEL"] = ut_001["DISTR_LEVEL"];
            ut_002["DISTR_RATE"] = ut_001["DISTR_RATE"];
            ut_002["DISTR_TYPE_CODE"] = "rate";
            ut_002["DISTR_MONEY"] = distr_money;

            ut_002["ROW_DATE_CREATE"] = ut_002["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.BeginTransaction();

            try
            {

                decipher.InsertModel(ut_002);

                //代理佣金要更新
                distr_user.AGENT_MONEY += distr_money;

                distr_user.ROW_DATE_UPDATE = DateTime.Now;

                decipher.UpdateModelProps(distr_user, "AGENT_MONEY", "ROW_DATE_UPDATE");

                decipher.TransactionCommit();


            }
            catch(Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("创建分销明细数据出错了！", ex);

                throw ex;
                    

            }


        }



    }
}