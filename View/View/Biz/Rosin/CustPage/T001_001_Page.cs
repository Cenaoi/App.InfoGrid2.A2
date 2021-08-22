using App.BizCommon;
using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.Biz.Rosin.CustPage
{
    public class T001_001_Page : ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnLoad()
        {
            Toolbar tbar = this.FindControl("toolbarT5") as Toolbar;

            ToolBarButton btn = new ToolBarButton("从入库表导入");
            btn.OnClick = GetMethodJs("GetBomFormula");

            if(tbar != null)
            {
                tbar.Items.Add(btn);
            }
        }



        /// <summary>
        /// 物料配方按钮点击事件
        /// </summary>
        public void GetBomFormula()
        {
            //Store st091 = this.FindControl("Store_UT_091") as Store;

            //int id = int.Parse(st091.CurDataId);

            int viewId = 38;

            DbDecipher decipher = ModelAction.OpenDecipher();




            Window win = new Window("产品结构");
            win.ContentPath = $"/App/InfoGrid2/View/OneSearch/SelectPreview.aspx?type=VIEW&viewId={viewId}";
            win.State = WindowState.Max;

            win.ShowDialog();



        }


        /// <summary>
        /// 获取调用的函数 js
        /// </summary>
        /// <param name="methodName">函数名称</param>
        /// <returns></returns>
        public string GetMethodJs(string methodName)
        {
            return GetMethodJs(methodName, null);
        }

        /// <summary>
        /// 获取调用的函数 js
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public string GetMethodJs(string methodName, string methodParams)
        {
            string js;

            if (string.IsNullOrEmpty(methodParams))
            {
                js = string.Format("widget1.subMethod('form:first',{{subName:'{0}', subMethod:'{1}'}});", this.ID, methodName);
            }
            else
            {
                if (StringUtil.StartsWith(methodParams, "{") && StringUtil.EndsWith(methodParams, "}"))
                {

                }
                else
                {
                    methodParams = string.Concat("'", methodParams, "'");
                }

                js = string.Format("widget1.subMethod('form:first',{{subName:'{0}', subMethod:'{1}', commandParam:{2} }});", this.ID, methodName, methodParams);
            }

            return js;
        }

    }
}