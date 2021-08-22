using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.IG2.BizBase;
using EC5.IG2.Core;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.CustomPage
{
    /// <summary>
    /// 给车间成本表view界面用的 和 成品入库单 view 界面用的   
    /// </summary>
    public class T1901_0_Page : ExPage
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit()
        {


        }

        protected override void OnLoad()
        {

            Toolbar tbar = this.FindControl("Toolbar1") as Toolbar;

            if (tbar != null)
            {

                ToolBarButton btn = new ToolBarButton("全部导入");
                btn.OnClick = GetMethodJs("DaoRu");
                tbar.Items.Add(btn);
            }
            

            ////这个界面显示所有的数据，不用分页
            //MainStore.PageSize = 0;

            base.OnLoad();


        }


        public void DaoRu()
        {


            int p_id = WebUtil.QueryInt("p_id");

            string p_id_field = WebUtil.QueryTrim("p_id_field");

            int map_id = WebUtil.QueryInt("map_id");

            List<LModel> rModels = MainStore.GetList() as List<LModel>;

            List<LModel> lModels = new List<LModel>();

            DbDecipher decipher = ModelAction.OpenDecipher();

            //查找映射规则
            IG2_MAP map = decipher.SelectToOneModel<IG2_MAP>("ROW_SID >=0 AND IG2_MAP_ID={0} ", map_id);


            foreach (LModel rModel in rModels)
            {

                LModel lModel = new LModel(map.L_TABLE);

                M2MapHelper.MapTable(rModel, map_id, lModel);

                lModel[p_id_field] = p_id;
                lModels.Add(lModel);
            }


            try
            {
                decipher.BeginTransaction();

                foreach (LModel model in lModels)
                {

                    // decipher.InsertModel(model);

                    DbCascadeRule.Insert(model);

                }


                decipher.TransactionCommit();

                MessageBox.Alert("全部导入成功了！");


            }
            catch (Exception ex)
            {

                decipher.TransactionRollback();

                log.Error("插入数据失败了！", ex);

                MessageBox.Alert("插入数据失败了！");

            }


        }





    }
}