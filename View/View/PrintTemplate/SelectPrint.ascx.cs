using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EasyClick.Web.Mini2.Data;
using EasyClick.Web.Mini2;
using App.InfoGrid2.Excel_Template;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using HWQ.Entity.LightModels;
using System.IO;
using App.InfoGrid2.Model;

namespace App.InfoGrid2.View.PrintTemplate
{
    /// <summary>
    /// 这个是 委外加工发货单的打印界面 单独的
    /// </summary>
    public partial class SelectPrint : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// 这是分组信息
        /// </summary>
        private Dictionary<string, List<DataGroup>> m_dic;

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                this.store1.DataBind();

                this.store2.DataBind();

            }
        }

        public void btnPrint()
        {

            DataRecord printDr = this.store1.GetDataCurrent();

            DataRecord templateDr = store2.GetDataCurrent();


            if (templateDr == null)
            {
                MessageBox.Alert("请选择打印模板！");

                return;
            }


            if (printDr == null)
            {
                MessageBox.Alert("请选择打印机！");
                return;
            }

            string code = printDr.Fields["PRINT_CODE"].Value;

            string url = templateDr.Fields["TEMPLATE_URL"].Value;


            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Alert("打印机没有相应的代码！");
                return;
            }

            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:'ok',ids:'"+code+"|"+url+"'});");


        }

        /// <summary>
        /// 确定打印按钮事件
        /// </summary>
        //public void btnPrint() 
        //{
        //    DataRecord printDr = this.store1.GetDataCurrent();

        //    DataRecord templateDr = store2.GetDataCurrent();


        //    if(templateDr == null)
        //    {
        //        MessageBox.Alert("请选择打印模板！");

        //        return;
        //    }


        //    if(printDr == null)
        //    {
        //        MessageBox.Alert("请选择打印机！");
        //        return;
        //    }

        //    string code = printDr.Fields["PRINT_CODE"].Value;

        //    string url = templateDr.Fields["TEMPLATE_URL"].Value;


        //    if (string.IsNullOrEmpty(code))
        //    {
        //        MessageBox.Alert("打印机没有相应的代码！");
        //        return;
        //    }

        //    //主表ID
        //    int id = WebUtil.QueryInt("id");






        //    DbDecipher decipher = ModelAction.OpenDecipher();
        //    LModel lm136 = decipher.GetModelByPk("UT_136", id);




        //    //	UT_137	委外加工发料单-工序工艺明细
        //    LightModelFilter lmFilter137 = new LightModelFilter("UT_137");
        //    lmFilter137.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
        //    lmFilter137.And("COL_12", id);
        //    List<LModel> lmList137 = decipher.GetModelList(lmFilter137);

        //    //		UT_138	委外加工发料单-主材料
        //    LightModelFilter lmFilter138 = new LightModelFilter("UT_138");
        //    lmFilter138.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
        //    lmFilter138.And("COL_15", id);
        //    List<LModel> lmList138 = decipher.GetModelList(lmFilter138);


        //    //	UT_139	委外加工发料单-订单明细
        //    LightModelFilter lmFilter139 = new LightModelFilter("UT_139");
        //    lmFilter139.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
        //    lmFilter139.And("COL_13", id);
        //    List<LModel> lmList139 = decipher.GetModelList(lmFilter139);

        //    // 实例化一个字典
        //    m_dic = new Dictionary<string, List<DataGroup>>();

        //    try
        //    {



        //        foreach (var item in lmList139)
        //        {
        //            string col_17 = item.Get<string>("COL_17");
        //            //如果指令单号为空就不处理
        //            if (string.IsNullOrEmpty(col_17)) { continue; }

        //            if (m_dic.ContainsKey(col_17))
        //            {

        //                var lm = m_dic[col_17][0][0];

        //                //产品名称
        //                var col_4 = lm.Get<string>("COL_4").Trim();

        //                lm["COL_4"] = string.Format("{0}/{1}", col_4, item.Get<string>("COL_4").Trim());


        //                //m_dic[col_17][0].Add(item);

        //            }
        //            else
        //            {
        //                List<DataGroup> dgList = new List<DataGroup>();
        //                DataGroup dg = new DataGroup();
        //                dg.Add(item);
        //                dgList.Add(dg);
        //                m_dic.Add(col_17, dgList);

        //            }
        //        }


        //        foreach (var item in m_dic)
        //        {
        //            if (item.Value.Count < 1)
        //            {
        //                item.Value.Add(null);
        //            }

        //        }


        //        foreach (var item in lmList137)
        //        {
        //            string col_11 = item.Get<string>("COL_11");

        //            //如果指令单号为空就不处理
        //            if (string.IsNullOrEmpty(col_11)) { continue; }

        //            //看看字典里面是否已经有值
        //            if (m_dic.ContainsKey(col_11))
        //            {

        //                if (m_dic[col_11].Count == 1)
        //                {
        //                    DataGroup dg = new DataGroup();
        //                    dg.Add(item);
        //                    m_dic[col_11].Add(dg);
        //                }
        //                else
        //                {
        //                    m_dic[col_11][1].Add(item);
        //                }

        //            }
        //            else
        //            {

        //                List<DataGroup> dgList = new List<DataGroup>();
        //                DataGroup dg = new DataGroup();
        //                dg.Add(item);
        //                dgList.Add(null);
        //                dgList.Add(dg);
        //                m_dic.Add(col_11, dgList);
        //            }
        //        }

        //        foreach (var item in m_dic)
        //        {
        //            if (item.Value.Count < 2)
        //            {
        //                item.Value.Add(null);
        //            }

        //        }


        //        foreach (var item in lmList138)
        //        {
        //            string col_16 = item.Get<string>("COL_16");
        //            //如果指令单号为空就不处理
        //            if (string.IsNullOrEmpty(col_16)) { continue; }

        //            if (m_dic.ContainsKey(col_16))
        //            {
        //                if (m_dic[col_16].Count == 2)
        //                {
        //                    DataGroup dg = new DataGroup();
        //                    dg.Add(item);
        //                    m_dic[col_16].Add(dg);
        //                }
        //                else
        //                {
        //                    m_dic[col_16][2].Add(item);
        //                }
        //            }
        //            else
        //            {
        //                List<DataGroup> dgList = new List<DataGroup>();
        //                DataGroup dg = new DataGroup();
        //                dg.Add(item);
        //                dgList.Add(null);
        //                dgList.Add(null);
        //                dgList.Add(dg);
        //                m_dic.Add(col_16, dgList);
        //            }
        //        }




        //        foreach (var item in m_dic)
        //        {
        //            if (item.Value.Count < 3)
        //            {
        //                item.Value.Add(null);

        //            }
        //        }


        //        CreateExcel(lm136, code,url);


        //        MessageBox.Alert("打印成功了！");


        //    }
        //    catch (Exception ex)
        //    {

        //        log.Error("打印出错了！", ex);
        //        MessageBox.Alert("打印出错了！");
        //    }


        //}

        ///// <summary>
        ///// 创建Excle文件
        ///// </summary>
        ///// <param name="lm136">主表数据</param>
        ///// <param name="PrintCode">打印机代码</param>
        ///// <param name="url">模板路径</param>
        //void CreateExcel(LModel lm136, string PrintCode,string url)
        //{

        //    try
        //    {

        //        DbDecipher decipher = ModelAction.OpenDecipher();


        //        NOPIHandlerEX2 nopiEx = new NOPIHandlerEX2();

        //        string patha = System.Web.HttpContext.Current.Server.MapPath(url);

        //        SheetPro sp = nopiEx.ReadExcel(patha);

        //        nopiEx.InsertSubData(sp, m_dic, lm136);


        //        //导出路径
        //        string mapath = System.Web.HttpContext.Current.Server.MapPath("/_Temporary/Excel");

        //        //判断文件夹是否存在
        //        if (!Directory.Exists(mapath))
        //        {
        //            Directory.CreateDirectory(mapath);
        //        }

        //        //文件名为当前时间时分秒都有
        //        string fileName = DateTime.Now.ToString("yyMMddHHmmssffff");

        //        //这是绝对物理路径
        //        string filePath = string.Format("{0}/{1}.xls", mapath, fileName);

        //        nopiEx.WriteExcel(sp, filePath);


        //        BIZ_PRINT_FILE file = new BIZ_PRINT_FILE()
        //        {
        //            FILE_URL = string.Format("/_Temporary/Excel/{0}.xls", fileName),
        //            PRINT_CODE = PrintCode,
        //            ROW_DATE_CREATE = DateTime.Now,
        //            ROW_SID = 0
        //        };

        //        decipher.InsertModel(file);


        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("导出 Excel文件出错了！", ex);
        //    }

        //}

    }
}