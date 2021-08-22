using System;
using System.Collections.Generic;
using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.IG2.Core;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using App.InfoGrid2.Excel_Template;
using App.InfoGrid2.Model;
using System.IO;
using EC5.Utility.Web;

namespace App.InfoGrid2.View.CustomPage.v2
{
    /// <summary>
    /// 委外加工管理 / 委外加工发货单  副本
    /// </summary>
    public class T343_164_Page_V2 : ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 这是分组信息
        /// </summary>
        private Dictionary<string, List<DataGroup>> m_dic;

        protected override void OnInit()
        {
            //TabPanel tabPanel = this.FindControl("tabs0") as TabPanel;
            //tabPanel.ButtonVisible = true;

        }

        protected override void OnLoad()
        {
            //StringBuilder sb = new StringBuilder();

            //foreach (var item in this.Controls)
            //{
            //    sb.AppendLine(item.ID);   
            //}


            string regJs = "function tijiao(e){ " +
                "if(e.result != 'ok'){return;};  \n" +
                "widget1.subMethod('form:first',{ subName:'ExPage', subMethod:'DaoRu_Step2', commandParam: e.ids || ''  });" +
                    "}";
            //打印机窗口关闭事件
            string printEnd = "function printEnd(e){ " +
                "if(e.result != 'ok'){return;};  \n" +
                "widget1.subMethod('form:first',{ subName:'ExPage', subMethod:'btnPrint', commandParam: e.ids || ''  });" +
                    "}";

            EasyClick.Web.Mini.MiniHelper.Eval(regJs);

            //弹出打印机列表窗口
            EasyClick.Web.Mini.MiniHelper.Eval(printEnd);



            //外发材料工具栏
            Toolbar tbar = this.FindControl("toolbarT644") as Toolbar;
            //外发材料数据仓库
            Store st138 = this.FindControl("Store_UT_138") as Store;

            //发料单头工具栏
            Toolbar tbar643 = this.FindControl("toolbarT643") as Toolbar;
            ToolBarButton btnPrint = new ToolBarButton("打印");
            btnPrint.OnClick = GetMethodJs("ShowPrint");
            tbar643.Items.Add(btnPrint);






            ToolBarButton btn = new ToolBarButton("导入");
            btn.OnClick = GetMethodJs("DaoRu", "你好");
            tbar.Items.Add(btn);



        }

       

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="name"></param>
        public void DaoRu(string name)
        {
            //MessageBox.Alert("你好:" + name);

            //弹出导入界面

            string id = this.MainStore.CurDataId;

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择项目！");
                return;
            }

            Window win = new Window("导入");
            win.State = WindowState.Max;
            win.ContentPath = "/App/InfoGrid2/View/Biz/JLSLBZ/WWJGFLD/FormWwjglb.aspx";
            win.FormClosedForJS = "tijiao"; //导入界面关闭后，执行的脚本函数名

            win.ShowDialog();

        }

        /// <summary>
        /// 关闭导入窗口后，执行的语句
        /// </summary>
        /// <param name="checkeIds"></param>
        public void DaoRu_Step2(string checkeIds)
        {

            int[] ids = StringUtil.ToIntList(checkeIds);

            DbDecipher decipher = ModelAction.OpenDecipher();
            LightModelFilter lmFilter = new LightModelFilter("UT_104");

            lmFilter.And("ROW_IDENTITY_ID", ids, HWQ.Entity.Filter.Logic.In);

            List<LModel> lm104List = decipher.GetModelList(lmFilter);
            List<LModel> Lm103List = new List<LModel>();
            List<LModel> Lm102List = new List<LModel>();

            List<LModel> lm137List = new List<LModel>();
            List<LModel> lm138List = new List<LModel>();
            List<LModel> lm139List = new List<LModel>();

            string id = this.MainStore.CurDataId;

            try
            {

                foreach (var lm104 in lm104List)
                {
                    LightModelFilter lmFilter103 = new LightModelFilter("UT_103");
                    lmFilter103.And("COL_16", lm104.Get<string>("COL_11"));
                    lmFilter103.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                    var lm103 = decipher.GetModelList(lmFilter103);


                    LightModelFilter lmFilter102 = new LightModelFilter("UT_102");
                    lmFilter102.And("COL_17", lm104.Get<string>("COL_11"));
                    lmFilter102.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
                    var lm102 = decipher.GetModelList(lmFilter102);

                    Lm103List.AddRange(lm103);
                    Lm102List.AddRange(lm102);


                    var lm137 = new LModel("UT_137");

                    M2MapHelper.MapTable(lm104, 44, lm137);

                    lm137["COL_12"] = id;

                    lm137List.Add(lm137);

                }

                foreach (var lm103 in Lm103List)
                {
                    var lm138 = new LModel("UT_138");

                    M2MapHelper.MapTable(lm103, 45, lm138);

                    lm138["COL_15"] = id;

                    lm138List.Add(lm138);


                }


                foreach (var lm102 in Lm102List)
                {
                    var lm139 = new LModel("UT_139");

                    M2MapHelper.MapTable(lm102, 46, lm139);

                    lm139["COL_13"] = id;

                    lm139List.Add(lm139);

                }

                decipher.InsertModels<LModel>(lm137List);
                decipher.InsertModels<LModel>(lm138List);
                decipher.InsertModels<LModel>(lm139List);


                Store st137 = this.FindControl("Store_UT_137") as Store;
                Store st138 = this.FindControl("Store_UT_138") as Store;
                Store st139 = this.FindControl("Store_UT_139") as Store;


                st137.Refresh();
                st138.Refresh();
                st139.Refresh();

                MessageBox.Alert("导入成功！");

            }
            catch (Exception ex)
            {
                log.Error("导入数据出错了！", ex);
                MessageBox.Alert("导入数据出错了！");
            }



        }

        /// <summary>
        /// 弹出打印列表
        /// </summary>
        public void ShowPrint() 
        {
            string id = this.MainStore.CurDataId;

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Alert("请选择项目！");
                return;
            }

            int pageid = WebUtil.QueryInt("pageid");

            Window win = new Window("打印机列表");
            win.Width = 1000;
            win.ContentPath = "/App/InfoGrid2/View/PrintTemplate/SelectPrint.aspx?pageid=" + pageid + "&id=" + id;
            win.FormClosedForJS = "printEnd"; //导入界面关闭后，执行的脚本函数名

            win.ShowDialog();
        }

        /// <summary>
        /// 打印功能
        /// </summary>
        /// <param name="code">打印机代码</param>
        public void btnPrint(string ids)
        {
            string[] codes = StringUtil.Split(ids, "|");

            if (codes.Length < 2)
            {
                MessageBox.Alert("请选择打印机或打印模板！");
                return;
            }


            //打印机编码
            string code = codes[0];
            //模板路径
            string url = codes[1];

            //主表ID
            string id = this.MainStore.CurDataId;



            DbDecipher decipher = ModelAction.OpenDecipher();
            LModel lm136 = decipher.GetModelByPk("UT_136", id);




            //	UT_137	委外加工发料单-工序工艺明细
            LightModelFilter lmFilter137 = new LightModelFilter("UT_137");
            lmFilter137.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            lmFilter137.And("COL_12", id);
            List<LModel> lmList137 = decipher.GetModelList(lmFilter137);

            //		UT_138	委外加工发料单-主材料
            LightModelFilter lmFilter138 = new LightModelFilter("UT_138");
            lmFilter138.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            lmFilter138.And("COL_15", id);
            List<LModel> lmList138 = decipher.GetModelList(lmFilter138);


            //	UT_139	委外加工发料单-订单明细
            LightModelFilter lmFilter139 = new LightModelFilter("UT_139");
            lmFilter139.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            lmFilter139.And("COL_13", id);
            List<LModel> lmList139 = decipher.GetModelList(lmFilter139);

            // 实例化一个字典
            m_dic = new Dictionary<string, List<DataGroup>>();

            try
            {



                foreach (var item in lmList139)
                {
                    string col_17 = item.Get<string>("COL_17");
                    //如果指令单号为空就不处理
                    if (string.IsNullOrEmpty(col_17)) { continue; }

                    if (m_dic.ContainsKey(col_17))
                    {

                        //var lm = m_dic[col_17][0][0];

                        ////产品名称
                        //var col_4 = lm.Get<string>("COL_4").Trim();

                        //lm["COL_4"] = string.Format("{0}/{1}", col_4, item.Get<string>("COL_4").Trim());

                        m_dic[col_17][0].Add(item);

                    }
                    else
                    {
                        List<DataGroup> dgList = new List<DataGroup>();
                        DataGroup dg = new DataGroup();
                        dg.Add(item);
                        dgList.Add(dg);
                        m_dic.Add(col_17, dgList);

                    }
                }


                foreach (var item in m_dic)
                {
                    if (item.Value.Count < 1)
                    {
                        item.Value.Add(null);
                    }

                }


                foreach (var item in lmList137)
                {
                    string col_11 = item.Get<string>("COL_11");

                    //如果指令单号为空就不处理
                    if (string.IsNullOrEmpty(col_11)) { continue; }

                    //看看字典里面是否已经有值
                    if (m_dic.ContainsKey(col_11))
                    {

                        if (m_dic[col_11].Count == 1)
                        {
                            DataGroup dg = new DataGroup();
                            dg.Add(item);
                            m_dic[col_11].Add(dg);
                        }
                        else
                        {
                            m_dic[col_11][1].Add(item);
                        }

                    }
                    else
                    {

                        List<DataGroup> dgList = new List<DataGroup>();
                        DataGroup dg = new DataGroup();
                        dg.Add(item);
                        dgList.Add(null);
                        dgList.Add(dg);
                        m_dic.Add(col_11, dgList);
                    }
                }

                foreach (var item in m_dic)
                {
                    if (item.Value.Count < 2)
                    {
                        item.Value.Add(null);
                    }

                }


                foreach (var item in lmList138)
                {
                    string col_16 = item.Get<string>("COL_16");
                    //如果指令单号为空就不处理
                    if (string.IsNullOrEmpty(col_16)) { continue; }

                    if (m_dic.ContainsKey(col_16))
                    {
                        if (m_dic[col_16].Count == 2)
                        {
                            DataGroup dg = new DataGroup();
                            dg.Add(item);
                            m_dic[col_16].Add(dg);
                        }
                        else
                        {
                            m_dic[col_16][2].Add(item);
                        }
                    }
                    else
                    {
                        List<DataGroup> dgList = new List<DataGroup>();
                        DataGroup dg = new DataGroup();
                        dg.Add(item);
                        dgList.Add(null);
                        dgList.Add(null);
                        dgList.Add(dg);
                        m_dic.Add(col_16, dgList);
                    }
                }




                foreach (var item in m_dic)
                {
                    if (item.Value.Count < 3)
                    {
                        item.Value.Add(null);

                    }
                }


                CreateExcel(lm136, code,url);



            }
            catch (Exception ex)
            {

                log.Error("打印出错了！", ex);
                MessageBox.Alert("打印出错了！");
            }


        }
        /// <summary>
        /// 创建Excle文件
        /// </summary>
        /// <param name="lm136">主表数据</param>
        /// <param name="PrintCode">打印机代码</param>
        /// <param name="url">模板路径</param>
        void CreateExcel(LModel lm136, string PrintCode,string url)
        {

            try
            {

                DbDecipher decipher = ModelAction.OpenDecipher();


                NOPIHandlerEX2 nopiEx = new NOPIHandlerEX2();

                string patha = System.Web.HttpContext.Current.Server.MapPath(url);

                SheetPro sp = nopiEx.ReadExcel(patha);

                nopiEx.InsertSubData(sp, m_dic, lm136);


                WebFileInfo wFile = new WebFileInfo("/_Temporary/Excel", FileUtil.NewFielname(".xls"));
                wFile.CreateDir();
                
                nopiEx.WriteExcel(sp, wFile.PhysicalPath);


                BIZ_PRINT_FILE file = new BIZ_PRINT_FILE()
                {
                    FILE_URL = wFile.RelativePath,
                    PRINT_CODE = PrintCode,
                    ROW_DATE_CREATE = DateTime.Now,
                    ROW_SID = 0
                };

                decipher.InsertModel(file);


            }
            catch (Exception ex)
            {
                throw new Exception("导出 Excel文件出错了！", ex);
            }

        }

      



    }

   


}