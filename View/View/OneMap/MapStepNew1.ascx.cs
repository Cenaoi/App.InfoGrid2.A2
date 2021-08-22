using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EasyClick.Web.Mini2;
using App.InfoGrid2.Model;
using EC5.Utility;

namespace App.InfoGrid2.View.OneMap
{
    public partial class MapStepNew1 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //if(!IsPostBack)
            //{
            //    this.store1.DataBind();
            //}
        }

        /// <summary>
        /// 显示目标表
        /// </summary>
        /// <param name="id">选中的ID</param>
        public void SetLTable(string id) 
        {
            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();


                IG2_TABLE table = decipher.SelectModelByPk<IG2_TABLE>(id);

                if (table == null)
                {
                    MessageBox.Alert("找不到选中的表！");
                    return;
                }


                this.tbxLTalbe.Value = table.TABLE_NAME;
                this.tbxLDisplay.Value = table.DISPLAY;
                
            }
            catch (Exception ex) 
            {
                log.Error(ex);
            }

        }


        /// <summary>
        /// 显示数据表
        /// </summary>
        /// <param name="id">选中的ID</param>
        public void SetRTable(string id)
        {
            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();


                IG2_TABLE table = decipher.SelectModelByPk<IG2_TABLE>(id);

                if (table == null)
                {
                    MessageBox.Alert("找不到选中的表！");
                    return;
                }


                this.tbxRTable.Value = table.TABLE_NAME;
                this.tbxRDisplay.Value = table.DISPLAY;
            }
            catch (Exception ex) 
            {
                log.Error(ex);
            }

        }


        /// <summary>
        /// 下一步
        /// </summary>
        public void GoNext() 
        {
            try
            {

                if (string.IsNullOrEmpty(tbxLDisplay.Value) || string.IsNullOrEmpty(tbxLTalbe.Value) )
                {
                    MessageBox.Alert("目标表不能为空！");
                    return;
                }

                if (string.IsNullOrEmpty(tbxRDisplay.Value) || string.IsNullOrEmpty(tbxRTable.Value))
                {
                    MessageBox.Alert("数据表不能为空！");
                    return;
                }


                IG2_MAP map = new IG2_MAP();
                map.L_TABLE = this.tbxLTalbe.Value;
                map.L_DISPLAY = this.tbxLDisplay.Value;

                map.R_TABLE = this.tbxRTable.Value;
                map.R_DISPLAY = this.tbxRDisplay.Value;

                map.ROW_SID = 0;

                DbDecipher decipher = ModelAction.OpenDecipher();

                IG2_TABLE lTable = null;

                //目标表
                lTable = decipher.SelectToOneModel<IG2_TABLE>("TABLE_NAME='{0}' and TABLE_TYPE_ID='TABLE' and ROW_SID >= 0",
                    map.L_TABLE);
                

                if(lTable == null)
                {
                    MessageBox.Alert("目标表有误，请查询！");
                    return;
                }


                decipher.InsertModel(map);



                ///目标列
                List<IG2_TABLE_COL> lColList = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0} and ROW_SID >=0 and SEC_LEVEL <= 6", lTable.IG2_TABLE_ID);


                foreach (IG2_TABLE_COL col in lColList)
                {
                    IG2_MAP_COL mapCol = new IG2_MAP_COL();
                    mapCol.IG2_MAP_ID = map.IG2_MAP_ID;
                    mapCol.VALUE_MODE = "TABLE";
                    mapCol.L_COL = col.DB_FIELD;
                    mapCol.L_COL_TEXT = col.DISPLAY;
                    mapCol.LONIN = "=";

                    decipher.InsertModel(mapCol);

                }


                string url = string.Format("MapStepNew2.aspx?id={0}", map.IG2_MAP_ID);



                MiniPager.Redirect(url);


            }
            catch (Exception ex) 
            {

                log.Error("插入数据出错了！",ex);

                MessageBox.Alert("插入数据出错了！");
            }



        }



    }
}