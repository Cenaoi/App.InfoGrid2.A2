using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using App.InfoGrid2.Model;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Bll;

namespace App.InfoGrid2.View.OneMap
{
    public partial class MapStepNew2 : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);


            if (!this.IsPostBack)
            {
                InitData();
            }

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                this.store1.DataBind();
                this.store2.DataBind();
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData()
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            int id = WebUtil.QueryInt("id");

            IG2_MAP map = decipher.SelectModelByPk<IG2_MAP>(id);

            if (map == null)
            {
                Error404 err404 = new Error404("错误", "映射表不存在.");

                Response.Redirect("/App/InfoGrid2/View/Explorer/ErrMessage.aspx?msg=" + err404.GetBase64());
            }

          
            ///数据表
            IG2_TABLE rTable = decipher.SelectToOneModel<IG2_TABLE>("TABLE_NAME='{0}' and TABLE_TYPE_ID in ('TABLE','MORE_VIEW') and ROW_SID >= 0", map.R_TABLE);

            if (rTable == null)
            {
                Error404 err404 = new Error404("错误", string.Format("右表“{0}”[{1}] 已经被删除。", map.R_TABLE,map.R_DISPLAY));

                Response.Redirect("/App/InfoGrid2/View/Explorer/ErrMessage.aspx?msg=" + err404.GetBase64());
            }


            ///数据列
            List<IG2_TABLE_COL> rColList = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0} and ROW_SID >= 0 and SEC_LEVEL < 6", rTable.IG2_TABLE_ID);

            
            EasyClick.Web.Mini2.SelectColumn bf = this.table1.Columns.FindByDataField("R_COL") as EasyClick.Web.Mini2.SelectColumn ;

            if(bf == null)
            {
                Error404 err404 = new Error404("错误", "UI 界面下拉框列，不存在");

                Response.Redirect("/App/InfoGrid2/View/Explorer/ErrMessage.aspx?msg=" + err404.GetBase64());
            }

            bf.Items.Add(new ListItem() { TextEx = "----空----" });

            foreach (IG2_TABLE_COL col in rColList)
            {
                bf.Items.Add(col.DB_FIELD, col.DISPLAY);
            }

        }


        /// <summary>
        /// 完成，跳转到所有复制信息表去
        /// </summary>
        public void Complete()
        {
            int id = WebUtil.QueryInt("id");



            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                IG2_MAP map = decipher.SelectModelByPk<IG2_MAP>(id);

                if (map == null)
                {
                    Response.Redirect("https://www.baidu.com");
                }

                map.ROW_SID = 1;


                MiniPager.Redirect("OneMapList.aspx");

                MapMgr.ClearBuffer(id);
            }
            catch (Exception ex) 
            {
                log.Error("跳转出错！",ex);
                MessageBox.Alert("跳转出错了！");
            }

        }


        /// <summary>
        /// 重置目标表列
        /// </summary>
        public void GoResetTargetFields()
        {

            int id = WebUtil.QueryInt("id");

            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();

                IG2_MAP map = decipher.SelectModelByPk<IG2_MAP>(id);


                ///目标表
                IG2_TABLE lTable = decipher.SelectToOneModel<IG2_TABLE>(
                    "TABLE_NAME='{0}' and TABLE_TYPE_ID in ('TABLE','MORE_VIEW') and ROW_SID >= 0", map.L_TABLE);

                //目标列
                LModelList<IG2_TABLE_COL> lColList = decipher.SelectModels<IG2_TABLE_COL>(
                    "IG2_TABLE_ID={0} and ROW_SID >=0 and SEC_LEVEL <= 6", lTable.IG2_TABLE_ID);


                LModelList<IG2_MAP_COL> mapCols = decipher.SelectModels<IG2_MAP_COL>("IG2_MAP_ID={0} AND ROW_SID >= 0", id);

                LModelGroup<IG2_MAP_COL, string> mapGrousp = mapCols.ToGroup<string>("L_COL");


                List<IG2_MAP> newMapCols = new List<IG2_MAP>(); //用于存放新增的字段

                foreach (IG2_TABLE_COL col in lColList)
                {
                    if (mapGrousp.ContainsKey(col.DB_FIELD))
                    {
                        continue;
                    }



                    IG2_MAP_COL mapCol = new IG2_MAP_COL();
                    mapCol.IG2_MAP_ID = map.IG2_MAP_ID;
                    mapCol.VALUE_MODE = "TABLE";
                    mapCol.L_COL = col.DB_FIELD;
                    mapCol.L_COL_TEXT = col.DISPLAY;
                    mapCol.LONIN = "=";

                    decipher.InsertModel(mapCol);

                }

                Toast.Show("同步完成");

                MapMgr.ClearBuffer(id);
            }
            catch (Exception ex)
            {
                log.Error("同步失败", ex);
                MessageBox.Alert("同步失败.");
            }



        }

    }
}