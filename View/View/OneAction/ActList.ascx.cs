using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using EasyClick.Web.Mini2;
using HWQ.Entity.LightModels;
using EC5.Utility;
using App.InfoGrid2.Model;
using HWQ.Entity.Filter;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using System.Data;
using System.Text;
using EC5.DbCascade.Model;
using EasyClick.BizWeb2;
using EC5.IG2.BizBase;

namespace App.InfoGrid2.View.OneAction
{
    public partial class ActList : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";


            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {

            if (!this.IsPostBack)
            {
                this.DataBind();
            }
        }

        protected override void OnInitCustomControls(EventArgs e)
        {
            CreateUI();
        }


        private void CreateUI()
        {
            string l_table = WebUtil.Query("l_table");

            if (StringUtil.IsBlank(l_table))
            {
                return;
            }


            l_table = l_table.ToUpper();

            LModelElement modelElem = LModelDna.GetElementByName(l_table);


            LightModelFilter filter = new LightModelFilter(typeof(IG2_TABLE));
            filter.And("TABLE_TYPE_ID","TABLE");
            filter.And("TABLE_NAME", l_table);
            filter.And("ROW_SID",0, Logic.GreaterThanOrEqual);
            
            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_TABLE table = decipher.SelectToOneModel<IG2_TABLE>(filter);

            this.store1.FilterParams.Add(new Param("L_TABLE", l_table));
            
            this.store1.InsertParams.Add(new Param("L_TABLE_ID",table.IG2_TABLE_ID.ToString(), DbType.Int32));
            this.store1.InsertParams.Add(new Param("L_TABLE",table.TABLE_NAME));
            this.store1.InsertParams.Add(new Param("L_TABLE_TEXT", table.DISPLAY));
            this.store1.InsertParams.Add(new Param("L_TABLE_UID", table.TABLE_UID.ToString(), DbType.Guid));

            string src_url = WebUtil.Query("src_url");

            if (!StringUtil.IsBlank(src_url))
            {
                WindowFooter1.Visible = true;
            }
        }

        /// <summary>
        /// 完成
        /// </summary>
        public void GoLast()
        {
            string src_url = WebUtil.QueryBase64("src_url");

            MiniPager.Redirect(src_url);
        }


        /// <summary>
        /// 复制一条新的数据出来
        /// </summary>
        public void CopyData()
        {
            string id = this.store1.CurDataId;
            LightModelFilter filter = new LightModelFilter(typeof(IG2_ACTION));
            filter.And("IG2_ACTION_ID", id);
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);

            DbDecipher decipher = ModelAction.OpenDecipher();

            IG2_ACTION action = decipher.SelectToOneModel<IG2_ACTION>(filter);


            if(action == null)
            {
                MessageBox.Alert("请选择一条记录再复制！");
                return;
            }

            IG2_ACTION newAction = new IG2_ACTION();
            List<IG2_ACTION_FILTER> iafList = decipher.SelectModels<IG2_ACTION_FILTER>("IG2_ACTION_ID={0} and ROW_SID >= 0",action.IG2_ACTION_ID);
            List<IG2_ACTION_ITEM> iaiList = decipher.SelectModels<IG2_ACTION_ITEM>("IG2_ACTION_ID={0} and ROW_SID >= 0", action.IG2_ACTION_ID);
            List<IG2_ACTION_LISTEN> ialList = decipher.SelectModels<IG2_ACTION_LISTEN>("IG2_ACTION_ID={0} and ROW_SID >= 0", action.IG2_ACTION_ID);

            List<IG2_ACTION_FILTER> newIafList = new List<IG2_ACTION_FILTER>();
            List<IG2_ACTION_ITEM> newIaiList = new List<IG2_ACTION_ITEM>();
            List<IG2_ACTION_LISTEN> newIalList = new List<IG2_ACTION_LISTEN>();




            decipher.BeginTransaction();

            try
            {
                
                action.CopyTo(newAction, true);
                newAction.ROW_DATE_CREATE = newAction.ROW_DATE_UPDATE = DateTime.Now;
                newAction.REMARK = action.REMARK + " (复制)";
                newAction.ENABLED = false;


                decipher.InsertModel(newAction);

                foreach (var item in iaiList)
                {
                    IG2_ACTION_ITEM iai = new IG2_ACTION_ITEM();
                    item.CopyTo(iai,true);
                    iai.IG2_ACTION_ID = newAction.IG2_ACTION_ID;
                    iai.ROW_DATE_CREATE = iai.ROW_DATE_UPDATE = DateTime.Now;

                    newIaiList.Add(iai);

                }

                foreach (var item in iafList)
                {
                    IG2_ACTION_FILTER iaf = new IG2_ACTION_FILTER();
                    item.CopyTo(iaf, true);
                    iaf.IG2_ACTION_ID = newAction.IG2_ACTION_ID;
                    iaf.ROW_DATE_CREATE = iaf.ROW_DATE_UPDATE = DateTime.Now;
                    newIafList.Add(iaf);

                }

                foreach (var item in ialList)
                {
                    IG2_ACTION_LISTEN ial = new IG2_ACTION_LISTEN();
                    item.CopyTo(ial, true);
                    ial.IG2_ACTION_ID = newAction.IG2_ACTION_ID;
                    ial.ROW_DATE_CREATE = ial.ROW_DATE_UPDATE = DateTime.Now;
                    newIalList.Add(ial);

                }

                

                decipher.InsertModels<IG2_ACTION_FILTER>(newIafList);
                decipher.InsertModels<IG2_ACTION_ITEM>(newIaiList);
                decipher.InsertModels<IG2_ACTION_LISTEN>(newIalList);

                decipher.TransactionCommit();

                this.store1.DataBind();

                Toast.Show("复制成功！");

            }
            catch (Exception ex) 
            {
                decipher.TransactionRollback();
                throw new Exception("复制一条数据出错了！",ex);
            }


        }

        /// <summary>
        /// 应用到系统
        /// </summary>
        public void GoApply()
        {
            try
            {
                DbCascadeLoader dbCCLoader = new DbCascadeLoader();
                dbCCLoader.InitDbcc();

                Toast.Show("更新成功.");
            }
            catch (Exception ex)
            {
                log.Error("应用到系统错误。", ex);
                MessageBox.Alert("更新失败!");
            }
        }



    }
}