using App.BizCommon;
using App.InfoGrid2.View.Biz.Rosin2.Actual;
using EasyClick.Web.Mini2;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.Biz.Rosin.Transform_V1
{
    public partial class WarehouseBeg : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            store1.Inserted += Store1_Inserted;
            store1.Deleting += Store1_Deleting;
            table1.Command += Table1_Command;

            if (!IsPostBack)
            {

                this.store1.DataBind();

            }
        }

        private void Store1_Inserted(object sender, ObjectEventArgs e)
        {

            LModel lm = e.Object as LModel;


            string url = "/App/InfoGrid2/View/Biz/Rosin/Transform_V1/WarehouseList.aspx?row_id=" + lm.GetPk();

            H_ID.Value = lm.GetPk().ToString();

            Window win = new Window("过户单详情");
            win.ContentPath = url;

            win.WindowClosed += Win_WindowClosed;
            win.State = WindowState.Max;

            win.ShowDialog();
        }

        private void Store1_Deleting(object sender, ObjectCancelEventArgs e)
        {
            LModel lm = e.Object as LModel;


            if(lm == null)
            {
                return;
            }


            if(lm.Get<int>("BIZ_SID") != 0)
            {
                e.Cancel = true;
                Toast.Show("只有状态是草稿的才能删除！");
                return;
            }

            DeleteSubData((int)lm.GetPk());

        }


        void DeleteSubData(int row_id)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter019 = new LightModelFilter("UT_019");
            lmFilter019.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter019.AddFilter("BIZ_SID >= 0");
            lmFilter019.And("DOC_PARENT_ID", row_id);

            decipher.UpdateProps(lmFilter019, new object[] { "ROW_SID",-3, "ROW_DATE_DELETE", DateTime.Now });

        }


        private void Table1_Command(object sender, TableCommandEventArgs e)
        {
            
            if(e.CommandName == "GoShowUT_019")
            {
                GoShowUT_019(e.Record.Id);

            }else if(e.CommandName == "GoShowUT_019_2")
            {
                GoShowUT_019_2(e.Record.Id);
            }

        }

        public void Win_WindowClosed(object sender, string data)
        {

            string pk = H_ID.Value;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm016 = decipher.GetModelByPk("UT_018", pk);

            //把货物编码和货物名称放到主表上去
            ActualMgr.FillGoodsCode();


            if (lm016 == null)
            {
                this.store1.Refresh();
                return;
            }


            //如果数据没有改变过，就自动给他删除掉
            if (lm016.Get<DateTime>("ROW_DATE_CREATE") == lm016.Get<DateTime>("ROW_DATE_UPDATE"))
            {
                lm016["ROW_SID"] = -3;
                lm016["ROW_DATE_DELETE"] = DateTime.Now;

                decipher.UpdateModelProps(lm016, "ROW_SID", "ROW_DATE_DELETE");
            }


            store1.Refresh();
        }

        public void GoShowUT_019(string row_id)
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm018 = decipher.GetModelByPk("UT_018",row_id);

            if(lm018 == null)
            {
                MessageBox.Alert("哦噢，找不到数据哦！");
                return;
            }


            if(lm018.Get<int>("BIZ_SID") < 2)
            {
                MessageBox.Alert("只有审核完成的才能查看！");
                return;
            }



                string url = "/App/InfoGrid2/View/Biz/Rosin/Transform_V1/WarhouseList_2.aspx?row_id=" + row_id;

                Window win2 = new Window("过户单详情");
                win2.ContentPath = url;

                win2.State = WindowState.Max;

                win2.ShowDialog();

              
            


        }


        public  void GoShowUT_019_2(string row_id)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm018 = decipher.GetModelByPk("UT_018", row_id);

            if (lm018 == null)
            {
                MessageBox.Alert("哦噢，找不到数据哦！");
                return;
            }


            if (lm018.Get<int>("BIZ_SID") == 2)
            {
                MessageBox.Alert("审核完成的不能修改！");

                return;
            }


            string url = "/App/InfoGrid2/View/Biz/Rosin/Transform_V1/WarehouseList.aspx?row_id=" + row_id;

            Window win = new Window("过户单详情");
            win.ContentPath = url;

            win.WindowClosed += Win_WindowClosed;
            win.State = WindowState.Max;

            win.ShowDialog();

        }


    }
}