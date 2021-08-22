using EasyClick.BizWeb2;
using EasyClick.Web.Mini2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.View.CustomPage.LM
{
    /// <summary>
    /// 给 销售管理里面 的 新工程项目列表用的
    /// </summary>
    public class T2370_1005_Page: ExPage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit()
        {

            MainTable.Command += MainTable_Command;

            var col = MainTable.Columns[2];

            if(col is ActionColumn act)
            {
                act.Items.Clear();
            
                ActionItem item = new ActionItem();
                item.Text = "编辑";
                item.Command = "edit_from";

                act.Items.Add(item);


            }



        }

        public void MainTable_Command(object sender, EasyClick.Web.Mini2.TableCommandEventArgs e)
        {

            if(e.CommandName == "edit_from")
            {

                var id =  e.Record.Id;


                EcView.Show($"/App/InfoGrid2/View/Biz/LM/NewProjectForm.aspx?id={id}", "新工程项目正式单-表单编辑");

            }


        }
    }
} 