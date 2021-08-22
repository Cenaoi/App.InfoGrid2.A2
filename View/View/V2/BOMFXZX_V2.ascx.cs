using App.BizCommon;
using App.InfoGrid2.Bll;
using App.InfoGrid2.View.Biz.PopView;
using EasyClick.BizWeb2;

using EasyClick.Web.Mini2;
using EasyClick.Web.Mini2.Data;
using EC5.DbCascade;
using EC5.IG2.BizBase;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.View.V2
{
    public partial class BOMFXZX_V2 : WidgetControl, IView
    {


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);

            Type typ = GetType();


            

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //	UT_101	吸塑指令单-单头  焦点改变事件
            this.StoreUT101.CurrentChanged += StoreUT101_CurrentChanged;
            // 	UT_102	吸塑指令单-订单明细 焦点改变事件
            this.StoreUT102.CurrentChanged += StoreUT102_CurrentChanged;

            this.StoreUT225.Updating += StoreUT225_Updating;
            this.StoreUT216.Updating += StoreUT216_Updating;
            this.StoreUT224.Updating += StoreUT224_Updating;

            if (!IsPostBack)
            {

                this.StoreUT101.DataBind();

            }
        }

        void StoreUT224_Updating(object sender, ObjectCancelEventArgs e)
        {
            LModel lm = (LModel)e.Object;

            if (lm == null)
            {
                return;
            }


            //板长
            decimal COL_6 = lm.Get<decimal>("COL_6");
            //长度
            decimal COL_19 = lm.Get<decimal>("COL_19");
            //计量单位
            string COL_39 = lm.Get<string>("COL_39");
            //需求量单位
            string COL_91 = lm.Get<string>("COL_91");
            //需求量
            decimal COL_90 = lm.Get<decimal>("COL_90");
            //数量
            decimal COL_35 = lm.Get<decimal>("COL_35");
            //密度
            decimal COL_5 = lm.Get<decimal>("COL_5");
            //厚度
            decimal COL_17 = lm.Get<decimal>("COL_17");
            //宽度
            decimal COL_4 = lm.Get<decimal>("COL_4");
            //主单位
            string COL_9 = lm.Get<string>("COL_9");
            //主数量
            decimal COL_12 = lm.Get<decimal>("COL_12");


            //计量单位值改变 就执行 需求量 换算 数量 公式 
            bool b = lm.GetBlemish("COL_39");

            MyJosn json = null;

            if (b)
            {



                ///长度COL_19 =板长COL_36          *1000+70
                COL_19 = COL_6 * 1000 + 70;

                #region 根据 需求量 COL_90  计算出 数量COL_35

                if (string.IsNullOrEmpty(COL_39) || string.IsNullOrEmpty(COL_91) || COL_90 == 0)
                {
                    MessageBox.Alert("检测到当前记录正在进行单位换算，{计量单位}或{需求单位}或{需求量}不能为空或零！");

                    return;
                }

                COL_39 = COL_39.Trim();
                COL_91 = COL_91.Trim();

                //需求量单位COL_91 和单位COL_9不相同时检查长度
                if (COL_91 != COL_39)
                {
                    //长度COL_19为空时不计算数量
                    if (COL_19 == 0)
                    {
                        MessageBox.Alert("检测到当前记录正在进行单位换算，长度项目不能为空或零！");

                        return;

                    }
                }

                //计量单位COL_9
                if (COL_39 == "KG" || COL_39 == "公斤")
                {

                    switch (COL_91)
                    {
                        case "米":
                            //数量COL_35 =   长度  COL_19                 / 1000* 需求量 COL_90
                            COL_35 = COL_19 / 1000m * COL_90;
                            break;
                        case "KG":
                        case "公斤":
                            ///数量COL_35 =需求量 COL_90
                            COL_35 = COL_90;
                            break;
                        case "张":
                            ///数量COL_35 =   需求量 COL_90   /  ( 密度COL_5 X                 (厚度COL_17 /100)                 X 宽度COL_4               X (长度COL_19 /1000) )                
                            COL_35 = COL_90 / ( COL_5 * (COL_17 / 100m) * COL_4 * (COL_19 / 1000m)) ;
                            break;
                    }

                }
                else if (COL_39 == "张")
                {

                    switch (COL_91)
                    {
                        case "米":
                            ///数量COL_35 =       需求量 COL_90      /    （长度*1000）
                            COL_35 = COL_90 / (1000m * COL_19);
                            break;
                        case "KG":
                        case "公斤":
                            ///数量COL_35 =       需求量 COL_90        *1000   /（密度                X      (厚度/100)                          X 宽度                   X (长度/1000)）（取整数，不用四舍五入）
                            COL_35 = COL_90 * 1000m / (COL_5 * (COL_17 / 100m) * COL_4 * (COL_19 / 1000m));
                            COL_35 = Math.Floor(COL_35);
                            break;
                        case "张":
                            ///数量COL_35 =需求量 COL_90
                            COL_35 = COL_90;
                            break;
                    }
                }
                else if (COL_39 == "米")
                {

                    switch (COL_91)
                    {
                        case "米":
                            //数量COL_35 =需求量 COL_90
                            COL_35 = COL_90;
                            break;
                        case "KG":
                        case "公斤":
                            //数量COL_35 =需求量 COL_90*1000/（密度 X   (厚度/100)  X 宽度  X (长度/1000)） *长度/1000
                            decimal col_10 = COL_90 * 1000m / (COL_5 * (COL_17 / 100m) * COL_4 * (COL_19 / 1000m)) * COL_19 / 1000m;
                            COL_35 = Math.Floor(col_10);
                            break;
                        case "张":
                            //数量COL_35 = 长度 / 1000 *  需求量 COL_90
                            COL_35 = COL_19 / 1000m * COL_90;
                            break;
                    }
                }

                #endregion

                lm["COL_19"] = COL_19;

                lm["COL_35"] = COL_35;


                //// 这是 需求量 换算 数量
                //json = UnitUtil.UnitConversion(lm, COL_39, COL_35, COL_19, COL_5, COL_17, COL_4, COL_6, COL_91, COL_90, "COL_35", "COL_19");


                //if (json.result != "OK")
                //{
                //    MessageBox.Alert(json.message);
                //    return;
                //}


               // COL_35 = lm.Get<decimal>("COL_35");
            }


            if ((lm.GetBlemish("COL_10") || b) && COL_35 != 0)
            {

                // 这是 数量 换算 主数量
                json = UnitUtil.UnitConversion2(lm, COL_39, COL_9, COL_35, COL_19, COL_12, COL_5, COL_17, COL_4, COL_6, "COL_12", "COL_19");


                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);
                }

            }

            string id = lm.GetPk().ToString();

            this.StoreUT224.SetRecordValue(id, "COL_12", lm["COL_12"]);
            this.StoreUT224.SetRecordValue(id, "COL_19", lm["COL_19"]);
            this.StoreUT224.SetRecordValue(id, "COL_35", lm["COL_35"]);


        }

        void StoreUT216_Updating(object sender, ObjectCancelEventArgs e)
        {
            LModel lm = e.Object as LModel;





            if (lm == null)
            {
                return;
            }

            //板长
            decimal COL_6 = lm.Get<decimal>("COL_6");
            //长度
            decimal COL_19 = lm.Get<decimal>("COL_19");
            //计量单位
            string COL_39 = lm.Get<string>("COL_39");
            //需求量单位
            string COL_97 = lm.Get<string>("COL_97");
            //需求量
            decimal COL_96 = lm.Get<decimal>("COL_96");
            //数量
            decimal COL_35 = lm.Get<decimal>("COL_35");
            //密度
            decimal COL_5 = lm.Get<decimal>("COL_5");
            //厚度
            decimal COL_17 = lm.Get<decimal>("COL_17");
            //宽度
            decimal COL_4 = lm.Get<decimal>("COL_4");
            //主单位
            string COL_9 = lm.Get<string>("COL_9");
            //主数量
            decimal COL_12 = lm.Get<decimal>("COL_12");

            //另外一个数量
            decimal COL_50 = lm.Get<decimal>("COL_50");


            //计量单位值改变 就执行 需求量 换算 数量 公式 
            bool b = lm.GetBlemish("COL_39");

            MyJosn json = null ;

            if (b)
            {


                json = UnitUtil.UnitConversion(lm, COL_39, COL_35, COL_19, COL_5, COL_17, COL_4, COL_6, COL_97, COL_96, "COL_35", "COL_19");


                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);

                    return;
                }


                //数量
                COL_35 = lm.Get<decimal>("COL_35");

            }

            if (lm.GetBlemish("COL_50") && COL_50 !=0)
            {

                json = UnitUtil.UnitConversion2(lm, COL_39, COL_9, COL_50, COL_19, COL_12, COL_5, COL_17, COL_4, COL_6, "COL_12", "COL_19");


                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);
                }
            }

            string id = lm.GetPk().ToString();

            this.StoreUT216.SetRecordValue(id, "COL_12", lm["COL_12"]);
            this.StoreUT216.SetRecordValue(id, "COL_19", lm["COL_19"]);
            this.StoreUT216.SetRecordValue(id, "COL_35", lm["COL_35"]);
        }

        

        void StoreUT225_Updating(object sender, ObjectCancelEventArgs e)
        {
            LModel lm = (LModel)e.Object;

            if (lm == null)
            {
                return;
            }

            //板长
            decimal COL_6 = lm.Get<decimal>("COL_6");
            //长度
            decimal COL_19 = lm.Get<decimal>("COL_19");
            //计量单位
            string COL_92 = lm.Get<string>("COL_92");
            //需求量单位
            string COL_91 = lm.Get<string>("COL_91");
            //需求量
            decimal COL_90 = lm.Get<decimal>("COL_90");
            //数量
            decimal COL_35 = lm.Get<decimal>("COL_35");
            //密度
            decimal COL_5 = lm.Get<decimal>("COL_5");
            //厚度
            decimal COL_17 = lm.Get<decimal>("COL_17");
            //宽度
            decimal COL_4 = lm.Get<decimal>("COL_4");
            //主单位
            string COL_9 = lm.Get<string>("COL_9");
            //主数量
            decimal COL_12 = lm.Get<decimal>("COL_12");

            //计量单位值改变 就执行 需求量 换算 数量 公式 
            bool b = lm.GetBlemish("COL_92");

            MyJosn json = null;

            if (b)
            {

                json = UnitUtil.UnitConversion(lm, COL_92, COL_35, COL_19, COL_5, COL_17, COL_4, COL_6, COL_91, COL_90, "COL_35", "COL_19");

                log.Debug(json);

                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);
                    e.Cancel = true;
                    return;
                }


                COL_35 = lm.Get<decimal>("COL_35");
            }

            if ((lm.GetBlemish("COL_35") || b) && COL_35 !=0)
            {

                json = UnitUtil.UnitConversion2(lm, COL_92, COL_9, COL_35, COL_19, COL_12, COL_5, COL_17, COL_4, COL_6, "COL_12", "COL_19");

                log.Debug(json);

                if (json.result != "OK")
                {
                    MessageBox.Alert(json.message);
                }

            }
            string id = lm.GetPk().ToString();

            this.StoreUT225.SetRecordValue(id, "COL_12", lm["COL_12"]);
            this.StoreUT225.SetRecordValue(id, "COL_19", lm["COL_19"]);
            this.StoreUT225.SetRecordValue(id, "COL_35", lm["COL_35"]);


        }

       

        /// <summary>
        /// 焦点改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StoreUT102_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {
            this.StoreUT196.DataBind();

        }
        ///焦点改变事件
        void StoreUT101_CurrentChanged(object sender, EasyClick.Web.Mini2.ObjectEventArgs e)
        {


            LModel lm101 = e.Object as LModel;

            if (lm101 == null)
            {
                return;
            }



            try
            {
                this.StoreUT102.DataBind();


                this.StoreUT196B.DataBind();

                this.StoreUT224.DataBind();
                this.StoreUT225.DataBind();
                this.StoreUT216.DataBind();
                this.StoreUT196A.DataBind();
                this.StoreUT227.DataBind();

                EntityFormEngine efe = new EntityFormEngine();
                efe.Model = "UT_101";
                efe.SetData(this.FormLayout1, lm101);


                if (lm101.Get<bool>("IS_CHECK"))
                {
                    this.Table1.ReadOnly = true;
                    this.Table3.ReadOnly = true;
                    this.UT_196_ATable.ReadOnly = true;
                    this.UT_196_BTable.ReadOnly = true;
                    this.UT_196Table.ReadOnly = true;
                    this.UT_216Table.ReadOnly = true;
                }
                else
                {
                    this.Table1.ReadOnly = false;
                    this.Table3.ReadOnly = false;
                    this.UT_196_ATable.ReadOnly = false;
                    this.UT_196_BTable.ReadOnly = false;
                    this.UT_196Table.ReadOnly = false;
                    this.UT_216Table.ReadOnly = false;
                }





            }
            catch (Exception ex)
            {
                log.Error(ex);

                MessageBox.Alert("有点错误哦...");
            }

        }

        /// <summary>
        /// 获取BOM配方
        /// </summary>
        public void GetBomFormula()
        {
            //获取到生产单明细ID
            int id = int.Parse(this.StoreUT102.CurDataId);

            //UT_101 ID
            int id101 = int.Parse(this.StoreUT101.CurDataId);


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm101 = decipher.GetModelByPk("UT_101", id101);

            if (lm101 == null || lm101.Get<bool>("IS_CHECK"))
            {
                MessageBox.Alert("分析结果已确认，无法进行此操作！");
                return;
            }



            MTable191Mgr mgr = new MTable191Mgr();

            //	UT_102	吸塑指令单-订单明细
            LModel lm102 = decipher.GetModelByPk("UT_102", id);

            Window win = new Window("产品结构");
            win.ContentPath = "/App/InfoGrid2/View/Biz/BOM/BOMTree.aspx?id=" + id;
            win.State = WindowState.Max;

            if (lm102.Get<bool>("HAS_CHILD"))
            {
                win.ShowDialog();
                return;
            }


            //产品ID
            string col42 = lm102.Get<string>("COL_42");


            LightModelFilter lmFilter = new LightModelFilter("UT_195");
            lmFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            lmFilter.And("COL_1", col42); //产品ID

            //	UT_195	BOM主表-表头
            LModel lm195 = decipher.GetModel(lmFilter);

            if (lm195 == null)
            {
                MessageBox.Alert(string.Format("产品ID：{0}在BOM主表-表头中找不到！", col42));
                return;
            }
            //设置目标表名
            mgr.TargetTable = "UT_196";

            List<LModel> lmList196 = mgr.GetModels196(56, lm195.Get<int>("ROW_IDENTITY_ID"));


            decipher.IdentityStop();

            foreach (var item in lmList196)
            {

                item["COL_20"] = this.StoreUT101.CurDataId;
                item["UT_102_ID"] = id;

            }

            //这是拿上级信息放到自身
            lmList196.ForEach((lm) =>
            {
                GetParentInfo(lm, lm102, lmList196);
            });



            


            #region 联动对象

            DbCascadeFactory dbCascadeFactory = new DbCascadeFactory();
            dbCascadeFactory.ExecEnd += delegate(object dcSnder, DbCascadeEventArges dcE)
            {
                EC5.BizLogger.LogStepMgr.Insert(dcE.Steps[0], dcE.OpText, dcE.Remark);
            };

            #endregion




            LD(lmList196, this.StoreUT196B, decipher, dbCascadeFactory);

            lm102["HAS_CHILD"] = true;
            lm102["COL_57"] = DateTime.Now;
            lm102["COL_58"] = "分析完成";

            decipher.UpdateModelProps(lm102, "HAS_CHILD", "COL_57", "COL_58");

            //开始联动
            dbCascadeFactory.Updated(this.StoreUT102, lm102);



            decipher.IdentityRecover();

            win.ShowDialog();



        }

        /// <summary>
        /// 获取父节点信息放到自身上
        /// </summary>
        void GetParentInfo(LModel lm, LModel lm102, List<LModel> lmList196)
        {
            lm["COL_24"] = lm102["COL_2"]; //订单号
            lm["COL_39"] = lm102["COL_42"]; //产品ID
            lm["COL_40"] = lm102["COL_3"];  //产品编号
            lm["COL_41"] = lm102["COL_4"]; //产品名称
            lm["COL_42"] = lm102["COL_5"]; //规格
            lm["COL_44"] = lm102["COL_9"]; // 生产数量
            lm["COL_43"] = lm102["COL_6"]; //单位
            lm["COL_22"] = lm102["COL_17"]; //生产单号


            if (lm.Get<int>("BIZ_PARENT_ID") == 0)
            {
                lm["COL_1"] = lm102["COL_42"]; //产品ID
                lm["COL_2"] = lm102["COL_3"];  //产品编号
                lm["COL_3"] = lm102["COL_4"];  //产品名称
                lm["COL_4"] = lm102["COL_5"]; //规格
            }
            else
            {
                try
                {

                    var lm196Parent = lmList196.Find(l => l.Get<int>("ROW_IDENTITY_ID") == lm.Get<int>("BIZ_PARENT_ID"));

                    lm["COL_1"] = lm196Parent["COL_1"]; //产品ID
                    lm["COL_2"] = lm196Parent["COL_2"];  //产品编号
                    lm["COL_3"] = lm196Parent["COL_3"];  //产品名称
                    lm["COL_4"] = lm196Parent["COL_4"]; //规格
                }
                catch (Exception ex)
                {
                    log.Error("父节点是这个：" + lm.Get<int>("BIZ_PARENT_ID"), ex);
                }

            }
        }



        /// <summary>
        /// 重新获取配方
        /// </summary>
        public void ClearBOM()
        {


            var checkRows = this.UT_102Table.CheckedRows;

            if (checkRows.Count == 0)
            {
                MessageBox.Alert("请选择记录！");
                return;
            }


            //获取到生产单头ID
            int id101 = int.Parse(this.StoreUT101.CurDataId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm101 = decipher.GetModelByPk("UT_101", id101);

            if (lm101 == null || lm101.Get<bool>("IS_CHECK"))
            {
                MessageBox.Alert("分析结果已确认，无法进行此操作！");
                return;
            }



            List<string> ids = new List<string>();

            foreach (var item in checkRows)
            {
                ids.Add(item.Id);
            }


            LightModelFilter lmFilter102 = new LightModelFilter("UT_102");
            lmFilter102.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            lmFilter102.And("ROW_IDENTITY_ID", ids.ToArray(), HWQ.Entity.Filter.Logic.In);

            List<LModel> lm102List = decipher.GetModelList(lmFilter102);

            foreach (var item in checkRows)
            {
                ids.Add(item.Id);
            }
            try
            {

                ///开始事务
                decipher.BeginTransaction();


                foreach (var lm102 in lm102List)
                {


                    lm102["HAS_CHILD"] = false;


                    LightModelFilter lmFilter196 = new LightModelFilter("UT_196");
                    lmFilter196.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                    lmFilter196.And("UT_102_ID", lm102["ROW_IDENTITY_ID"]);
                    lmFilter196.And("COL_20", id101);
                    //删除
                    decipher.UpdateProps(lmFilter196, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });


                    LightModelFilter lmFilter225 = new LightModelFilter("UT_225");
                    lmFilter225.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                    lmFilter225.And("UT_102_ID", lm102["ROW_IDENTITY_ID"]);
                    lmFilter225.And("COL_73", id101);
                    //删除
                    decipher.UpdateProps(lmFilter225, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });

                    LightModelFilter lmFilter224 = new LightModelFilter("UT_224");
                    lmFilter224.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                    lmFilter224.And("UT_102_ID", lm102["ROW_IDENTITY_ID"]);
                    lmFilter224.And("COL_73", id101);
                    //删除
                    decipher.UpdateProps(lmFilter224, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });


                    LightModelFilter lmFilter216 = new LightModelFilter("UT_216");
                    lmFilter216.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                    lmFilter216.And("UT_102_ID", lm102["ROW_IDENTITY_ID"]);
                    lmFilter216.And("COL_79", id101);
                    //删除
                    decipher.UpdateProps(lmFilter216, new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now });



                    //把102表的状态改为false
                    decipher.UpdateModelProps(lm102, "HAS_CHILD");


                }
                //事务提交
                decipher.TransactionCommit();


                MessageBox.Alert("清除成功！");


            }
            catch (Exception ex)
            {
                log.Error("清除失败了！", ex);
            }



        }

        /// <summary>
        /// 清除所有的记录
        /// </summary>
        public void ClearBOMAll()
        {
            int id101 = StringUtil.ToInt(this.StoreUT101.CurDataId);


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm101 = decipher.GetModelByPk("UT_101", id101);

            if (lm101 == null || lm101.Get<bool>("IS_CHECK"))
            {
                MessageBox.Alert("分析结果已确认，无法进行此操作！");
                return;
            }


            //开始事务
            decipher.BeginTransaction();


            try
            {

                LightModelFilter lmFilter102 = new LightModelFilter("UT_102");
                lmFilter102.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                lmFilter102.And("COL_13", id101);

                //更改
                decipher.UpdateProps(lmFilter102, new object[] { "HAS_CHILD", 0, "ROW_DATE_UPDATE", DateTime.Now });

                var delParam = new object[] { "ROW_SID", -3, "ROW_DATE_DELETE", DateTime.Now };

                LightModelFilter lmFilter196 = new LightModelFilter("UT_196");
                lmFilter196.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter196.And("COL_20", id101);
                //删除
                decipher.UpdateProps(lmFilter196, delParam);


                LightModelFilter lmFilter225 = new LightModelFilter("UT_225");
                lmFilter225.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter225.And("COL_73", id101);
                //删除
                decipher.UpdateProps(lmFilter225, delParam);

                LightModelFilter lmFilter224 = new LightModelFilter("UT_224");
                lmFilter224.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter224.And("COL_73", id101);
                //删除
                decipher.UpdateProps(lmFilter224, delParam);


                LightModelFilter lmFilter216 = new LightModelFilter("UT_216");
                lmFilter216.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter216.And("COL_79", id101);
                //删除
                decipher.UpdateProps(lmFilter216, delParam);


                LightModelFilter lmFilter227 = new LightModelFilter("UT_227");
                lmFilter227.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                lmFilter227.And("COL_73", id101);
                //删除
                decipher.UpdateProps(lmFilter227, delParam);



                //提交事务
                decipher.TransactionCommit();

                this.StoreUT196B.Refresh();
                this.StoreUT224.Refresh();
                this.StoreUT225.Refresh();
                this.StoreUT196A.Refresh();
                this.StoreUT227.Refresh();
                this.StoreUT216.Refresh();
                this.StoreUT196.Refresh();


                MessageBox.Alert("清空成功了！");



            }
            catch (Exception ex)
            {
                //事务回滚
                decipher.TransactionRollback();
                log.Error("清空数据出错了！101表ID：" + id101, ex);
                MessageBox.Alert("清空失败了！");
            }

        }



        /// <summary>
        /// 生产数量计算
        /// </summary>
        public void NumFormula()
        {
            //获取到生产单明细ID
            int id102 = int.Parse(this.StoreUT102.CurDataId);
            //获取到生产单头ID
            int id101 = int.Parse(this.StoreUT101.CurDataId);

            DbDecipher decipher = ModelAction.OpenDecipher();


            LModel lm101 = decipher.GetModelByPk("UT_101", id101);

            if (lm101 == null || lm101.Get<bool>("IS_CHECK"))
            {
                MessageBox.Alert("分析结果已确认，无法进行此操作！");
                return;
            }


            //	UT_102	吸塑指令单-订单明细
            LModel lm102 = decipher.GetModelByPk("UT_102", id102);


            LightModelFilter lmFilter = new LightModelFilter("UT_196");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("UT_102_ID", id102);
            lmFilter.And("COL_20", id101);
            lmFilter.And("BIZ_PARENT_ID", 0);  //拿出第一级节点

            List<LModel> lm196List = decipher.GetModelList(lmFilter);

            //计算出来的生产数量集合
            List<LModel> lm196New = new List<LModel>();

            //UT_102 表中的数量
            int col_9 = lm102.Get<int>("COL_9");

            try
            {

                //开启事务
                decipher.BeginTransaction();

                foreach (var item in lm196List)
                {
                    //自身数量
                    int col_11 = item.Get<int>("COL_11");

                    item.SetTakeChange(true);

                    //自身的生产数量 = 自身数量 * UT_102表的数量
                    item["COL_32"] = col_11 * col_9;
                    item["ROW_DATE_UPDATE"] = DateTime.Now;

                    lm196New.Add(item);

                    //递归计算生产数量
                    RecursiveNum(item, decipher, 0, lm196New);

                }

                //更新数据到数据库中
                foreach (var item in lm196New)
                {
                    decipher.UpdateModelProps(item, "COL_32", "ROW_DATE_UPDATE");
                }


                //提交事务
                decipher.TransactionCommit();


                #region  启动联动

                DbCascadeFactory casFty = new DbCascadeFactory();
                casFty.ExecEnd += delegate(object sender, DbCascadeEventArges e)
                {
                    EC5.BizLogger.LogStepMgr.Insert(e.Steps[0], e.OpText, e.Remark);
                };



                decipher.BeginTransaction();

                foreach (var item in lm196New)
                {
                    casFty.Updated(null, item);
                }

                decipher.TransactionCommit();

                #endregion

                //刷新界面
                this.StoreUT196.Refresh();

                MessageBox.Alert("计算成功了！");

            }
            catch (Exception ex)
            {
                log.Error("计算生产数量出错了！", ex);
                //事务回滚
                decipher.TransactionRollback();
            }



        }


        /// <summary>
        /// 递归生产数量
        /// </summary>
        /// <param name="decipher">数据库操作对象</param>
        /// <param name="dept">深度 > 20 就出错</param>
        /// <param name="lm196">节点对象</param>
        /// <param name="lmNew">计算出来的生产数量集合</param>
        void RecursiveNum(LModel lm196, DbDecipher decipher, int dept, List<LModel> lmNew)
        {

            if (dept > 20)
            {
                throw new Exception("产品构建出现嵌套死循环。产品ID：" + lm196["ROW_IDENTITY_ID"]);
            }

            LightModelFilter lmFilter = new LightModelFilter("UT_196");
            lmFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_PARENT_ID", lm196["ROW_IDENTITY_ID"]);  //拿出下级节点结合

            List<LModel> lm196List = decipher.GetModelList(lmFilter);
            //拿到上级的生产数量
            int col_32 = lm196.Get<int>("COL_32");


            foreach (var item in lm196List)
            {
                item.SetTakeChange(true);

                //拿到自身的数量
                int col_11 = item.Get<int>("COL_11");
                //自身的生产数量 = 自身数量 * 上级生产数量
                item["COL_32"] = col_11 * col_32;
                item["ROW_DATE_UPDATE"] = DateTime.Now;

                lmNew.Add(item);

                RecursiveNum(item, decipher, ++dept, lmNew);

            }

        }


        /// <summary>
        /// 从UT_196数据表中拿数据到UT_225和UT_224和UT_216
        /// </summary>
        public void GetUT196To225()
        {
            int id = StringUtil.ToInt(this.StoreUT101.CurDataId);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm101 = decipher.GetModelByPk("UT_101", id);

            if (lm101 == null || lm101.Get<bool>("IS_CHECK"))
            {
                MessageBox.Alert("分析结果已确认，无法进行此操作！");
                return;
            }




            LightModelFilter lmFilter224 = new LightModelFilter("UT_196");
            lmFilter224.And("COL_20", id);
            lmFilter224.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter224.And("COL_13", "外发加工");

            //这是拿到要放到UT_224 委外加工
            List<LModel> lm224List = decipher.GetModelList(lmFilter224);

            LightModelFilter lmFilter216 = new LightModelFilter("UT_196");
            lmFilter216.And("COL_20", id);
            lmFilter216.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter216.And("COL_13", "采购");


            //这是拿到要放到UT_216 采购分析
            List<LModel> lm216List = decipher.GetModelList(lmFilter216);


            
            List<LModel> lm225List = new List<LModel>();
            List<LModel> lm227List = new List<LModel>();



            //这是UT_224 数据集合
            List<LModel> lm224New = new List<LModel>();

            //这是UT_216 数据集合
            List<LModel> lm216New = new List<LModel>();

            //这是UT_225 数据集合
            List<LModel> lm225New = new List<LModel>();

            //这是UT_227 数据集合
            List<LModel> lm227New = new List<LModel>();




            //循环映射每一条数据 224
            foreach (var item in lm224List)
            {
                LModel lm224 = new LModel("UT_224");

                MapMgr.MapData(69, item, lm224);


                LightModelFilter lmFilter227 = new LightModelFilter("UT_196");
                lmFilter227.And("BIZ_PARENT_ID", item.GetPk());
                lmFilter227.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
                //拿到下级数据
                var lms = decipher.GetModelList(lmFilter227);

                lm227List.AddRange(lms);


                lm224["COL_73"] = id;

                lm224New.Add(lm224);

            }
            //循环映射每一条数据 216
            foreach (var item in lm216List)
            {
                LModel lm216 = new LModel("UT_216");

                MapMgr.MapData(62, item, lm216);

                lm216["COL_79"] = id;


                lm216New.Add(lm216);
            }

            //给每一条记录都加上 UT_101 的 ID
            foreach (var item in lm225List)
            {

                LModel lm225 = new LModel("UT_225");


                MapMgr.MapData(70, item, lm225);

                lm225["COL_73"] = id;

                lm225New.Add(lm225);

            }

            //给每一条记录都加上 UT_101 的 ID
            foreach (var item in lm227List) 
            {
                LModel lm227 = new LModel("UT_227");


                MapMgr.MapData(77, item, lm227);           

                lm227["COL_73"] = id;

                lm227New.Add(lm227);
            }

            #region 联动对象

            DbCascadeFactory dbCascadeFactory = new DbCascadeFactory();
            dbCascadeFactory.ExecEnd += delegate(object dcSnder, DbCascadeEventArges dcE)
            {
                EC5.BizLogger.LogStepMgr.Insert(dcE.Steps[0], dcE.OpText, dcE.Remark);
            };

            #endregion


            LD(lm224New, this.StoreUT224, decipher, dbCascadeFactory);

            LD(lm216New, this.StoreUT216, decipher, dbCascadeFactory);

            LD(lm225New, this.StoreUT225, decipher, dbCascadeFactory);

            LD(lm227New, this.StoreUT227, decipher, dbCascadeFactory);



            this.StoreUT224.Refresh();
            this.StoreUT225.Refresh();
            this.StoreUT216.Refresh();
            this.StoreUT196A.Refresh();
            this.StoreUT227.Refresh();


            MessageBox.Alert("物控分析成功！");

        }

        /// <summary>
        /// 采购需求分析按钮事件
        /// </summary>
        public void btnChangeRowSid0_2()
        {
            string id = this.StoreUT216.CurDataId;


            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择一条记录！");
                return;
            }
            DbDecipher decipher = ModelAction.OpenDecipher();

            //UT_101 ID
            int id101 = int.Parse(this.StoreUT101.CurDataId);

            LModel lm101 = decipher.GetModelByPk("UT_101", id101);

            if (lm101 == null || lm101.Get<bool>("IS_CHECK"))
            {
                MessageBox.Alert("分析结果已确认，无法进行此操作！");
                return;
            }


            

            LModel lm = decipher.GetModelByPk("UT_216", id);

            if (lm.Get<decimal>("COL_67") == 2)
            {
                MessageBox.Alert("已经分析过！");
                return;
            }


            ////这下面是联动要用的
            lm.SetTakeChange(true);

            lm["COL_67"] = 2;
            lm["ROW_DATE_UPDATE"] = DateTime.Now;
            lm["COL_68"] = DateTime.Now;




            //单元格公式
            LCodeFactory lcFactory = new LCodeFactory();
            lcFactory.ExecLCode(this.StoreUT216, null, lm);

            //简单流程
            LCodeValueFactory lcvFactiry = new LCodeValueFactory();
            lcvFactiry.ExecLCode(this.StoreUT216, null, lm);


            #region 联动

            DbCascadeFactory dbccFactory = new DbCascadeFactory();
            dbccFactory.ExecEnd += dbccFactory_ExecEnd;

            BizDbStepPath stepPath = null;

            decipher.BeginTransaction();

            try
            {
                decipher.UpdateModel(lm, true);


                stepPath = dbccFactory.Updated(this.StoreUT216, lm);

                decipher.TransactionCommit();


                EasyClick.Web.Mini.MiniHelper.Eval("ShowGif('分析成功了！')");



                //板长
                decimal COL_6 = lm.Get<decimal>("COL_6");
                //长度
                decimal COL_19 = lm.Get<decimal>("COL_19");
                //计量单位
                string COL_39 = lm.Get<string>("COL_39");
                //需求量单位
                string COL_97 = lm.Get<string>("COL_97");
                //需求量
                decimal COL_96 = lm.Get<decimal>("COL_96");
                //数量
                decimal COL_35 = lm.Get<decimal>("COL_35");
                //密度
                decimal COL_5 = lm.Get<decimal>("COL_5");
                //厚度
                decimal COL_17 = lm.Get<decimal>("COL_17");
                //宽度
                decimal COL_4 = lm.Get<decimal>("COL_4");
                //主单位
                string COL_9 = lm.Get<string>("COL_9");
                //主数量
                decimal COL_12 = lm.Get<decimal>("COL_12");

                //另外一个数量
                decimal COL_50 = lm.Get<decimal>("COL_50");

                MyJosn json;

             
                if (lm.GetBlemish("COL_50") && COL_50 != 0)
                {

                    json = UnitUtil.UnitConversion2(lm, COL_39, COL_9, COL_50, COL_19, COL_12, COL_5, COL_17, COL_4, COL_6, "COL_12", "COL_19");


                    if (json.result != "OK")
                    {
                        MessageBox.Alert(json.message);
                    }
                }

                decipher.UpdateModelProps(lm, "COL_2","COL_19");

                this.StoreUT216.SetRecordValue(id, "COL_12", lm["COL_12"]);
                this.StoreUT216.SetRecordValue(id, "COL_19", lm["COL_19"]);
                this.StoreUT216.SetRecordValue(id, "COL_35", lm["COL_35"]);



            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error(ex);

                EasyClick.Web.Mini.MiniHelper.Eval("ShowGif('分析错误！')");
                
            }


            #endregion

        }

        void dbccFactory_ExecEnd(object sender, DbCascadeEventArges e)
        {
            EC5.BizLogger.LogStepMgr.Insert(e.Steps[0], e.OpText, e.Remark);
        }

        /// <summary>
        /// 撤销需求分析按钮事件
        /// </summary>
        public void btnChangeRowSid2_0()
        {
            string id = this.StoreUT216.CurDataId;


            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择一条记录！");
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            //UT_101 ID
            int id101 = int.Parse(this.StoreUT101.CurDataId);

            LModel lm101 = decipher.GetModelByPk("UT_101", id101);

            if (lm101 == null || lm101.Get<bool>("IS_CHECK"))
            {
                MessageBox.Alert("分析结果已确认，无法进行此操作！");
                return;
            }


            LModel lm = decipher.GetModelByPk("UT_216", id);

            if (lm.Get<decimal>("COL_67") == 0)
            {
                MessageBox.Alert("没有分析过，不用撤销！");
                return;
            }


            lm.SetTakeChange(true);
            lm["COL_67"] = 0;
            lm["ROW_DATE_UPDATE"] = DateTime.Now;
            lm["COL_68"] = DateTime.Now;


            //单元格公式
            LCodeFactory lcFactory = new LCodeFactory();
            lcFactory.ExecLCode(this.StoreUT216, null, lm);

            //简单流程
            LCodeValueFactory lcvFactiry = new LCodeValueFactory();
            lcvFactiry.ExecLCode(this.StoreUT216, null, lm);


            #region 联动

            DbCascadeFactory dbccFactory = new DbCascadeFactory();
            dbccFactory.ExecEnd += dbccFactory_ExecEnd;

            BizDbStepPath stepPath = null;

            decipher.BeginTransaction();

            try
            {
                decipher.UpdateModel(lm, true);


                stepPath = dbccFactory.Updated(this.StoreUT216, lm);

                decipher.TransactionCommit();

                MessageBox.Alert("撤销分析成功了！");

            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();

                log.Error(ex);
                MessageBox.Alert("分析错误！");
            }


            #endregion





        }


        /// <summary>
        /// 从UT_103数据表中拿数据放到 196 225 224 216 表中
        /// </summary>
        public void GetUT103Bom()
        {
            int id = StringUtil.ToInt(this.StoreUT101.CurDataId);

            DbDecipher decipher = ModelAction.OpenDecipher();


            LModel lm101 = decipher.GetModelByPk("UT_101", id);

            if (lm101 == null || lm101.Get<bool>("IS_CHECK"))
            {
                MessageBox.Alert("分析结果已确认，无法进行此操作！");
                return;
            }


            LightModelFilter lmFilter = new LightModelFilter("UT_103");
            lmFilter.And("COL_15", id);
            lmFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);

            //	UT_103	吸塑指令单-主材料
            List<LModel> lm103List = decipher.GetModelList(lmFilter);


            if (lm103List == null || lm103List.Count == 0)
            {
                MessageBox.Alert("在生产单中找不到数据！101表ID:" + id);
                return;
            }


            //这是UT_196表的集合
            List<LModel> lm196List = new List<LModel>();

            //这是UT_225表的集合
            List<LModel> lm225List = new List<LModel>();

            //这是UT_224表的集合
            List<LModel> lm224List = new List<LModel>();

            //这是UT_216表的集合
            List<LModel> lm216List = new List<LModel>();

            //循环映射数据
            foreach (var item in lm103List)
            {
                //UT_196实体
                LModel lm196 = new LModel("UT_196");

                //UT_225实体
                LModel lm225 = new LModel("UT_225");


                //UT_216实体
                LModel lm216 = new LModel("UT_216");

                //从UT_103 映射到 UT_196
                MapMgr.MapData(68, item, lm196);

                //从UT_103 映射到 UT_225
                MapMgr.MapData(67, item, lm225);

                if (lm101.Get<string>("COL_17") == "需发外加工")
                {
                    //UT_224实体
                    LModel lm224 = new LModel("UT_224");
                    //从UT_103 映射到 UT_224
                    MapMgr.MapData(66, item, lm224);
                    lm224["COL_73"] = id;
                    lm224List.Add(lm224);

                }


                //从UT_103 映射到 UT_216
                MapMgr.MapData(65, item, lm216);

                lm196["COL_20"] = id;
                lm225["COL_73"] = id;

                lm216["COL_79"] = id;

                lm196List.Add(lm196);
                lm225List.Add(lm225);

                lm216List.Add(lm216);


            }

            #region 联动对象

            DbCascadeFactory dbCascadeFactory = new DbCascadeFactory();
            dbCascadeFactory.ExecEnd += delegate(object dcSnder, DbCascadeEventArges dcE)
            {
                EC5.BizLogger.LogStepMgr.Insert(dcE.Steps[0], dcE.OpText, dcE.Remark);
            };

            #endregion



            LD(lm196List, this.StoreUT196B, decipher, dbCascadeFactory);


            LD(lm225List, this.StoreUT225, decipher, dbCascadeFactory);


            LD(lm224List, this.StoreUT224, decipher, dbCascadeFactory);


            LD(lm216List, this.StoreUT216, decipher, dbCascadeFactory);



            this.StoreUT196B.Refresh();
            this.StoreUT216.Refresh();
            this.StoreUT224.Refresh();
            this.StoreUT225.Refresh();

            MessageBox.Alert("获取数据完成！");


        }



        /// <summary>
        /// 从订单BOM UT_219 表中取数据过来放到 UT_196表中
        /// </summary>
        public void GetUT219To196()
        {
            //获取到生产单明细ID
            int id = int.Parse(this.StoreUT102.CurDataId);

            //UT_101 ID
            int id101 = int.Parse(this.StoreUT101.CurDataId);

            DbDecipher decipher = ModelAction.OpenDecipher();


            LModel lm101 = decipher.GetModelByPk("UT_101", id101);

            if (lm101 == null || lm101.Get<bool>("IS_CHECK"))
            {
                MessageBox.Alert("分析结果已确认，无法进行此操作！");
                return;
            }


            MTable191Mgr mgr = new MTable191Mgr();

            //	UT_102	吸塑指令单-订单明细
            LModel lm102 = decipher.GetModelByPk("UT_102", id);

            Window win = new Window("产品结构");
            win.ContentPath = "/App/InfoGrid2/View/Biz/BOM/BOMTree.aspx?id=" + id;
            win.State = WindowState.Max;

            if (lm102.Get<bool>("HAS_CHILD"))
            {
                win.ShowDialog();
                return;
            }


            //产品ID
            string col42 = lm102.Get<string>("COL_42");
            //订单号
            string col2 = lm102.Get<string>("COL_2");


            LightModelFilter lmFilter = new LightModelFilter("UT_219");
            lmFilter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            lmFilter.And("COL_24", col2); //订单号
            lmFilter.And("COL_39", col42); //产品ID

            //	UT_195	BOM主表-表头
            LModel lm219 = decipher.GetModel(lmFilter);

            if (lm219 == null)
            {
                MessageBox.Alert(string.Format("产品ID：{0}或订单号：{1}在UT_219表中找不到！", col42, col2));
                return;
            }
            //设置目标表名
            mgr.TargetTable = "UT_196";

            List<LModel> lmList196 = mgr.GetModelsByName(72, lm219.Get<int>("ROW_IDENTITY_ID"), "UT_219", "BIZ_PARENT_ID");


            decipher.IdentityStop();

            foreach (var item in lmList196)
            {

                item["COL_20"] = this.StoreUT101.CurDataId;
                item["UT_102_ID"] = id;

            }

            //这是拿上级信息放到自身
            lmList196.ForEach((lm) =>
            {
                GetParentInfo(lm, lm102, lmList196);
            });


            #region 联动对象

            DbCascadeFactory dbCascadeFactory = new DbCascadeFactory();
            dbCascadeFactory.ExecEnd += delegate(object dcSnder, DbCascadeEventArges dcE)
            {
                EC5.BizLogger.LogStepMgr.Insert(dcE.Steps[0], dcE.OpText, dcE.Remark);
            };

            #endregion


            LD(lmList196, this.StoreUT196B, decipher, dbCascadeFactory);

            lm102["HAS_CHILD"] = true;
            lm102["COL_57"] = DateTime.Now;
            lm102["COL_58"] = "分析完成";

            decipher.UpdateModelProps(lm102, "HAS_CHILD", "COL_57", "COL_58");

            decipher.IdentityRecover();

            win.ShowDialog();
        }

        /// <summary>
        /// 确认分析结果按钮事件 把UT_225、UT_224、UT_216 的 BIZ_SID 字段从 0 改为 2
        /// </summary>
        public void BizSid0_2()
        {
            //UT_101表的ID
            int id101 = StringUtil.ToInt(this.StoreUT101.CurDataId);

            DbDecipher decipher = ModelAction.OpenDecipher();


            LModel lm101 = decipher.GetModelByPk("UT_101", id101);

            lm101.SetTakeChange(true);

            LightModelFilter filter;
            
            //UT_225 表 集合
            filter = new FilterForm("UT_225").Where("ROW_SID >=0 and COL_73={0} and BIZ_SID = 0", id101).Filter;
            List<LModel> lm225List = decipher.GetModelList(filter);

            //UT_224 表 集合
            filter = new FilterForm("UT_224").Where("ROW_SID >=0 and COL_73={0} and BIZ_SID = 0", id101).Filter;
            List<LModel> lm224List = decipher.GetModelList(filter);

            //UT_224 表 集合
            filter = new FilterForm("UT_216").Where("ROW_SID >=0 and COL_79={0} and BIZ_SID = 0", id101).Filter;
            List<LModel> lm216List = decipher.GetModelList(filter);

            //UT_196 表 集合
            filter = new FilterForm("UT_196").Where("ROW_SID >=0 and COL_20={0} and BIZ_SID = 0", id101).Filter;
            List<LModel> lm196List = decipher.GetModelList(filter);


            //UT_227 表 集合
            filter = new FilterForm("UT_227").Where("ROW_SID >=0 and COL_73={0} and BIZ_SID = 0", id101).Filter;
            List<LModel> lm227List = decipher.GetModelList(filter);


            if (lm101 == null || lm101.Get<bool>("IS_CHECK"))
            {
                MessageBox.Alert("已经分析过，不用再分析了！");
                return;
            }

            if ( lm216List.Count == 0 && lm224List.Count == 0 && lm225List.Count == 0 && lm196List.Count == 0  && lm227List.Count ==0)
            {
                MessageBox.Alert("找不到需要分析的数据！");
                return;
            }

           


            #region 联动对象

            DbCascadeFactory dbCascadeFactory = new DbCascadeFactory();
            dbCascadeFactory.ExecEnd += delegate(object dcSnder, DbCascadeEventArges dcE)
            {
                EC5.BizLogger.LogStepMgr.Insert(dcE.Steps[0], dcE.OpText, dcE.Remark);
            };

            #endregion


            try
            {

                //开启事务
                decipher.BeginTransaction();


                lm216List.ForEach((lm) =>{ UpdataBizSid(lm, 2, decipher, dbCascadeFactory,"UT_216");});
                lm225List.ForEach((lm) => { UpdataBizSid(lm, 2, decipher, dbCascadeFactory,"UT_225"); });
                lm224List.ForEach((lm) => { UpdataBizSid(lm, 2, decipher, dbCascadeFactory, "UT_224"); });
                lm196List.ForEach((lm) => { UpdataBizSid(lm, 2, decipher, dbCascadeFactory, "UT_196"); });
                lm227List.ForEach((lm) => { UpdataBizSid(lm, 2, decipher, dbCascadeFactory, "UT_227"); });

                //确认分析结果
                lm101["IS_CHECK"] = true;

                //单元格公式
                LCodeFactory lcFactory = new LCodeFactory();
                lcFactory.ExecLCode(this.StoreUT101, lm101);

                //简单流程
                LCodeValueFactory lcvFactiry = new LCodeValueFactory();
                lcvFactiry.ExecLCode(this.StoreUT101, lm101);

                decipher.UpdateModel(lm101, true);

                //联动开始
                dbCascadeFactory.Updated(this.StoreUT101, lm101);

                //提交事务
                decipher.TransactionCommit();

                this.Table1.ReadOnly = true;
                this.Table3.ReadOnly = true;
                this.UT_196_ATable.ReadOnly = true;
                this.UT_196_BTable.ReadOnly = true;
                this.UT_196Table.ReadOnly = true;
                this.UT_216Table.ReadOnly = true;


                this.StoreUT216.Refresh();
                this.StoreUT225.Refresh();
                this.StoreUT224.Refresh();
                this.StoreUT196A.Refresh();
                this.StoreUT227.Refresh();
                this.StoreUT196B.Refresh();
                this.StoreUT196.Refresh();

                MessageBox.Alert("分析成功了！");



            }
            catch (Exception ex)
            {
                //事务回滚
                decipher.TransactionRollback();

                MessageBox.Alert("分析出错了！");
                log.Error("更新业务状态出错了！", ex);

            }

        }

        /// <summary>
        /// 撤销分析结果按钮事件 把UT_225、UT_224、UT_216 的 BIZ_SID 字段从 2 改为 0
        /// </summary>
        public void BizSid2_0()
        {
            //UT_101表的ID
            int id101 = StringUtil.ToInt(this.StoreUT101.CurDataId);

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter filter;

            LModel lm101 = decipher.GetModelByPk("UT_101", id101);

            lm101.SetTakeChange(true);

            //UT_225 表 集合
            filter = new FilterForm("UT_225").Where("ROW_SID >=0 and COL_73={0} and BIZ_SID = 2", id101).Filter;
            List<LModel> lm225List = decipher.GetModelList(filter);

            //UT_224 表 集合
            filter = new FilterForm("UT_224").Where("ROW_SID >=0 and COL_73={0} and BIZ_SID = 2", id101).Filter;
            List<LModel> lm224List = decipher.GetModelList(filter);

            //UT_224 表 集合
            filter = new FilterForm("UT_216").Where("ROW_SID >=0 and COL_79={0} and BIZ_SID = 2", id101).Filter;
            List<LModel> lm216List = decipher.GetModelList(filter);

            //UT_196 表 集合
            filter = new FilterForm("UT_196").Where("ROW_SID >=0 and COL_20={0} and BIZ_SID = 2", id101).Filter;
            List<LModel> lm196List = decipher.GetModelList(filter);

            //UT_227 表 集合
            filter = new FilterForm("UT_227").Where("ROW_SID >=0 and COL_73={0} and BIZ_SID = 2", id101).Filter;
            List<LModel> lm227List = decipher.GetModelList(filter);


            if (lm101 == null || !lm101.Get<bool>("IS_CHECK"))
            {
                MessageBox.Alert("请先分析后再撤销！");
                return;
            }

            if (lm216List.Count == 0 && lm224List.Count == 0 && lm225List.Count == 0 && lm196List.Count == 0 && lm227List.Count == 0)
            {
                MessageBox.Alert("找不到需要撤销的数据！");
                return;
            }

            


            #region 联动对象

            DbCascadeFactory dbCascadeFactory = new DbCascadeFactory();
            dbCascadeFactory.ExecEnd += delegate(object dcSnder, DbCascadeEventArges dcE)
            {
                EC5.BizLogger.LogStepMgr.Insert(dcE.Steps[0], dcE.OpText, dcE.Remark);
            };

            #endregion

            try
            {


                //开启事务
                decipher.BeginTransaction();



                lm216List.ForEach((lm) => { UpdataBizSid(lm, 0, decipher, dbCascadeFactory,"UT_216"); });
                lm225List.ForEach((lm) => { UpdataBizSid(lm, 0, decipher, dbCascadeFactory, "UT_225"); });
                lm224List.ForEach((lm) => { UpdataBizSid(lm, 0, decipher, dbCascadeFactory, "UT_224"); });
                lm196List.ForEach((lm) => { UpdataBizSid(lm, 0, decipher, dbCascadeFactory, "UT_196"); });
                lm227List.ForEach((lm) => { UpdataBizSid(lm, 0, decipher, dbCascadeFactory, "UT_227"); });

                //撤销分析结果
                lm101["IS_CHECK"] = false;


                //单元格公式
                LCodeFactory lcFactory = new LCodeFactory();
                lcFactory.ExecLCode(this.StoreUT101, lm101);

                //简单流程
                LCodeValueFactory lcvFactiry = new LCodeValueFactory();
                lcvFactiry.ExecLCode(this.StoreUT101, lm101);

                decipher.UpdateModel(lm101,true);

                //联动开始
                dbCascadeFactory.Updated(this.StoreUT101, lm101);

                //提交事务
                decipher.TransactionCommit();


                this.Table1.ReadOnly = false;
                this.Table3.ReadOnly = false;
                this.UT_196_ATable.ReadOnly = false;
                this.UT_196_BTable.ReadOnly = false;
                this.UT_196Table.ReadOnly = false;
                this.UT_216Table.ReadOnly = false;


                this.StoreUT216.Refresh();
                this.StoreUT225.Refresh();
                this.StoreUT224.Refresh();
                this.StoreUT196A.Refresh();
                this.StoreUT227.Refresh();
                this.StoreUT196B.Refresh();
                this.StoreUT196.Refresh();

                MessageBox.Alert("撤销数据成功了！");

            }
            catch (Exception ex)
            {
                //事务回滚
                decipher.TransactionRollback();

                MessageBox.Alert("撤销出错了！");
                log.Error("更新业务状态出错了！", ex);

            }
        }

        /// <summary>
        /// 改变业务状态函数
        /// </summary>
        /// <param name="lm">实体</param>
        /// <param name="bizSid">0 或者 2</param>
        /// <param name="decipher">数据帮助类</param>
        /// <param name="dbCascadeFactory">联动相关类</param>
        /// <param name="tableName">表名</param>
        void UpdataBizSid(LModel lm, int bizSid, DbDecipher decipher, DbCascadeFactory dbCascadeFactory, string tableName)
        {
            lm.SetTakeChange(true);
            
            //decipher.UpdateModelByPk(tableName, lm.Get<int>("ROW_IDENTITY_ID"), new object[] { "BIZ_SID", bizSid, "ROW_DATE_UPDATE", DateTime.Now });

            lm["BIZ_SID"] = bizSid;
            lm["ROW_DATE_UPDATE"] = DateTime.Now;



            //单元格公式
            LCodeFactory lcFactory = new LCodeFactory();
            lcFactory.ExecLCode(null, lm);

            //简单流程
            LCodeValueFactory lcvFactiry = new LCodeValueFactory();
            lcvFactiry.ExecLCode(null, lm);


            decipher.UpdateModel(lm, true);



            dbCascadeFactory.Updated(null, lm);

        }

        /// <summary>
        /// 这是联动简单表达式的函数
        /// </summary>
        /// <param name="modeList">实体集合</param>
        /// <param name="store">数据仓库</param>
        /// <param name="decipher">数据库帮助对象</param>
        /// <param name="dbCascadeFactory">联动对象</param>
        void LD(List<LModel> modeList, Store store, DbDecipher decipher, DbCascadeFactory dbCascadeFactory)
        {

            modeList.ForEach(m => {

                m.SetTakeChange(true);
                m.SetBlemishAll(true);

                //单元格公式
                LCodeFactory lcFactory = new LCodeFactory();
                lcFactory.ExecLCode(store, m);

                //简单流程
                LCodeValueFactory lcvFactiry = new LCodeValueFactory();
                lcvFactiry.ExecLCode(store, m);


                decipher.InsertModel(m);

                dbCascadeFactory.Inserted(store, m);

            });

        }


    }
}