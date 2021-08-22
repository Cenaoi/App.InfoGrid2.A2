using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Xml;
using System;
using System.Collections.Generic;

namespace App.InfoGrid2.View.OneTable
{
    public partial class CreateNew1 : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 下一步
        /// </summary>
        public void GoNext()
        {
            string name = this.TB1.Value;

            if (string.IsNullOrEmpty(name))
            {
                Toast.Show("请输入数据库表名！");
                this.TB1.MarkInvalid("请输入数据库表名!");

                return;
            }


            //变大写
            name = name.Trim().ToUpper();



            int id = WebUtil.QueryInt("catalog_id");


            DbDecipher decipher = ModelAction.OpenDecipher();

            bool b = decipher.ExistsModels<IG2_TABLE>($"ROW_SID >= 0 and TABLE_TYPE_ID = 'TABLE' and TABLE_NAME = '{name}'");

            if (b)
            {
                MessageBox.Alert("创建失败","已经存在这个自定义表！");

                return;
            }


            DatabaseBuilder db = decipher.DatabaseBuilder;

            XmlModelElem xModelElem = db.GetModelElemByTable(name);

            //表描述
            string des = StringUtil.NoBlank(xModelElem.Description, name);

            //很重要的东西
            Guid uid = Guid.NewGuid();

            IG2_TABLE it = new IG2_TABLE()
            {
                TABLE_TYPE_ID = "TABLE",
                IG2_CATALOG_ID = id,
                TABLE_NAME = name,
                DISPLAY = des,
                ID_FIELD = xModelElem.PrimaryKey,
                IDENTITY_FIELD = xModelElem.DbIdentity,
                USER_SEQ_FIELD = xModelElem.PrimaryKey,
                TABLE_UID = uid
            };



            //开启事物
            decipher.BeginTransaction();

            try
            {

                decipher.InsertModel(it);


                List<IG2_TABLE_COL> colList = new List<IG2_TABLE_COL>();


                foreach (var item in xModelElem.Fields)
                {
                    //列描述
                    string colDes = StringUtil.NoBlank(item.Description, item.DBField);

                    IG2_TABLE_COL itc = new IG2_TABLE_COL()
                    {
                        IG2_TABLE_ID = it.IG2_TABLE_ID,
                        TABLE_NAME = name,
                        DB_FIELD = item.DBField,
                        F_NAME = colDes,
                        DISPLAY = colDes,
                        DB_TYPE = item.DBType.ToString(),
                        DEFAULT_VALUE = item.DefaultValue,
                        DISPLAY_LEN = item.DefaultWidth,
                        FORMAT = item.Format,
                        ANGLE = item.Alignment.ToString(),
                        DB_DOT = item.DecimalDigits,
                        DISPLAY_TYPE = item.ControlType,
                        IS_READONLY = item.ReadOnly,
                        IS_VISIBLE = item.Visible,
                        IS_LIST_VISIBLE = true,
                        IS_SEARCH_VISIBLE = true,
                        DB_LEN = item.MaxLen,
                        TABLE_UID = uid
                    };


                    colList.Add(itc);
                }


                decipher.InsertModels<IG2_TABLE_COL>(colList);


                //事物提交
                decipher.TransactionCommit();



                Toast.Show("创建自定义表成功！");

            }
            catch (Exception ex)
            {
                //事物回滚
                decipher.TransactionRollback();


                log.Error("根据表名创建实体表出错了！", ex);
                MessageBox.Alert("哎呦，出错了喔！");
            }


        }

    }
}