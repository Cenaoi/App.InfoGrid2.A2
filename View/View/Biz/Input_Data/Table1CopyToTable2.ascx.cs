using App.BizCommon;
using App.InfoGrid2.Bll;
using EasyClick.Web.Mini2;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Input_Data
{
    public partial class Table1CopyToTable2 : System.Web.UI.UserControl
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 清除目标表数据
        /// </summary>
        public void ClearData()
        {

            //目标表名
            string targetTable = tbxTarget.Value.Trim();

            if ( string.IsNullOrEmpty(targetTable))
            {
                MessageBox.Alert("目标表不能为空！");
                return;
            }


            string sql = string.Format("UPDATE {0} SET ROW_SID = -3 ,ROW_DATE_DELETE ='{1}' where ROW_SID >=0 ",
                targetTable,  DateUtil.ToDateTimeString() );

            DbDecipher decipher = ModelAction.OpenDecipher();

            try
            {

                decipher.ExecuteNonQuery(sql);

                MessageBox.Alert("清除数据成功了！");
            }
            catch (Exception ex)
            {
                log.Error("清除目标表数据出错了！",ex);
                MessageBox.Alert("清除目标表数据出错了!");
            }


        }

        /// <summary>
        /// 拷贝进行中
        /// </summary>
        public void GoSure() 
        {
            //数据源表名
            string dataSource = tbxDataSource.Value.Trim();
            //目标表名
            string targetTable = tbxTarget.Value.Trim();

            //映射规则ID
            int mapId = 0;

            if (int.TryParse(tbxMapID.Value.Trim(), out mapId) == false)
            {
                MessageBox.Alert("请输入正确的映射规则ID！");
                return;
            }


            if (StringUtil.IsBlank(dataSource, targetTable))
            {
                MessageBox.Alert("数据源或目标表不能为空！");
                return;
            }




            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(dataSource);
            lmFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);

            //数据源表的所有数据
            List<LModel> lmDataSourceList = decipher.GetModelList(lmFilter);

            //目标表要拷贝的数据
            List<LModel> lmTargetList = new List<LModel>();

            try
            {


                foreach (var item in lmDataSourceList)
                {

                    //目标实体
                    LModel lmTarget = new LModel(targetTable);
                    //从UT_103 映射到 UT_224
                    MapMgr.MapData(mapId, item, lmTarget);

                    lmTargetList.Add(lmTarget);


                }
            }
            catch (Exception ex)
            {
                log.Error("映射数据出错了！",ex);
                MessageBox.Alert("映射数据出错了！");
                return;
            }


            decipher.BeginTransaction();

            try
            {
                decipher.InsertModels(lmTargetList);

                decipher.TransactionCommit();

                MessageBox.Alert("拷贝数据成功了！");

            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error("拷贝数据出错了！", ex);
                MessageBox.Alert("拷贝数据出错了！，请联系系统管理员！");
                return;
            }
        }
    }
}