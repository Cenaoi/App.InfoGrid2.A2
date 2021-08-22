using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BizCommon;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.View.MoreView
{
    public partial class StepCopy1 : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitData();
            }
        }


        private void InitData()
        {
            int srcTableId = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            TableSet tSet = TableSet.Select(decipher, srcTableId);

            TB1.Value = tSet.Table.DISPLAY;


        }

        public void GoNext()
        {
            int srcTableId = WebUtil.QueryInt("id");

            string newViewName = BizCommon.BillIdentityMgr.NewCodeForNum("USER_VIEW", "UV_", 3);

            DbDecipher decipher = ModelAction.OpenDecipher();




            IG2_VIEW iv = decipher.SelectToOneModel<IG2_VIEW>("IG2_VIEW_ID={0} and ROW_SID >= 0", srcTableId);


            List<IG2_VIEW_FIELD> fieldList = decipher.SelectModels<IG2_VIEW_FIELD>("IG2_VIEW_ID={0} and ROW_SID >= 0", srcTableId);
            List<IG2_VIEW_TABLE> tableList = decipher.SelectModels<IG2_VIEW_TABLE>("IG2_VIEW_ID={0} and ROW_SID >= 0", srcTableId);


            #region 拷贝确保原始表字段正确


            bool flag = false;

            foreach (var item in fieldList)
            {
                //这是查看关联表是否正确
                foreach (var subTable in tableList)
                {
                    if (subTable.TABLE_NAME == item.TABLE_NAME)
                    {
                        continue;
                    }


                    flag = true;


                    item.ROW_SID = -3;
                    item.ROW_DATE_DELETE = DateTime.Now;

                    decipher.UpdateModelProps(item, "ROW_SID", "ROW_DATE_DELETE");

                }


                if (flag)
                {
                    continue;
                }

                //这是查看字段是否和原始表一致
                LModelElement modelElem = LightModel.GetLModelElement(item.TABLE_NAME);

                if (modelElem.Fields.ContainsField(item.FIELD_NAME))
                {
                    continue;
                }

                item.ROW_SID = -3;
                item.ROW_DATE_DELETE = DateTime.Now;

                decipher.UpdateModelProps(item, "ROW_SID", "ROW_DATE_DELETE");

            }



            #endregion






            TableSet tSet = TableSet.Select(decipher, srcTableId);

            tSet.Table.DISPLAY = this.TB1.Value;


            tSet.Table.TABLE_NAME = newViewName;

            foreach (var item in tSet.Cols)
            {
                item.TABLE_NAME = newViewName;
            }




            tSet.Insert(decipher);





            ViewSet vSet = ViewSet.Select(decipher, srcTableId);

            vSet.View.IG2_VIEW_ID = tSet.Table.IG2_TABLE_ID;

            vSet.View.VIEW_NAME = newViewName;

            vSet.Insert(decipher);

            MessageBox.Alert("拷贝完成！请回原来界面刷新下！");

        }

    }
}