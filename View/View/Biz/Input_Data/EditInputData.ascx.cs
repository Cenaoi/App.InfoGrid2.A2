using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EasyClick.Web.Mini2;
using EC5.Utility.Web;
using System.Text;
using Newtonsoft.Json.Linq;
using EC5.Utility;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;

namespace App.InfoGrid2.View.Biz.Input_Data
{
    public partial class EditInputData : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    
                    string ruleJson = WebUtil.QueryBase64("rule");

                    JObject o = JObject.Parse(ruleJson);

                    InitData(o);
                }
            }
            catch (Exception ex)
            {
                log.Error("解析json数据出错了！", ex);
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData(JObject o) 
        {
            string name = WebUtil.Query("parent_table");

            DbDecipher decipher = ModelAction.OpenDecipher();
            IG2_TABLE tables = decipher.SelectToOneModel<IG2_TABLE>("TABLE_NAME='{0}' and ROW_SID >= 0 and TABLE_TYPE_ID='TABLE'", name);

            if(tables == null)
            {
                return;
            }

            List<IG2_TABLE_COL> colList = decipher.SelectModels<IG2_TABLE_COL>("IG2_TABLE_ID={0}", tables.IG2_TABLE_ID);

            foreach(IG2_TABLE_COL item in colList)
            {
                this.cbxParentID.Items.Add(item.DB_FIELD,item.DISPLAY);
            }

            foreach (JProperty prop in o.Properties())
            {

                string value = prop.Name;

                
                SetValueControl(prop);
                
            }


        }

        /// <summary>
        /// 设置值文本框
        /// </summary>
        /// <param name="prop">Json属性</param>
        public void SetValueControl(JProperty prop)
        {
            try
            {
                ///属性名称
                string name = prop.Name;

                ///属性值
                string value = prop.Value.Value<string>();



                ///根据节点名称复制给相应的控件
                switch (name)
                {
                    case "dialog_id":
                        this.tbxDialogID.Value = value;
                        
                        break;
                    case "map_id": this.tbxMapID.Value = value; break;
                    case "parent_id_field": cbxParentID.Value = value; break;
                    case "l_table": this.tbxLTable.Value = value; break;
                    case "r_table": this.tbxRTable.Value = value; break;

                }
            }
            catch (Exception ex)
            {
                log.Error("数据初始化出错！", ex);
            }
        }


        /// <summary>
        /// 设置导入窗口ID
        /// </summary>
        public void SetDialogID(string id) 
        {
            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();
                IG2_TABLE tables = decipher.SelectModelByPk<IG2_TABLE>(id);
                if (tables == null)
                {
                    MessageBox.Alert("找不到数据表！");
                    return;
                }

                this.tbxDialogID.Value = tables.IG2_TABLE_ID.ToString();
                this.tbxDialogText.Value = tables.DISPLAY;
            }
            catch (Exception ex) 
            {
                log.Error("设置表ID出错！",ex);
            }

        }


        /// <summary>
        /// 设置映射表ID
        /// </summary>
        public void SetMapID(string id) 
        {
            try
            {
                DbDecipher decipher = ModelAction.OpenDecipher();
                IG2_MAP maps = decipher.SelectModelByPk<IG2_MAP>(id);

                if (maps == null)
                {
                    MessageBox.Alert("找不到数据表！");
                    return;
                }

                this.tbxMapID.Value = maps.IG2_MAP_ID.ToString();
                this.tbxLTable.Value = maps.L_TABLE;
                this.tbxLTableDisplay.Value = maps.L_DISPLAY;
                this.tbxRTable.Value = maps.R_TABLE;
                this.tbxRTableDisplay.Value = maps.R_DISPLAY;
                
            }
            catch (Exception ex)
            {
                log.Error("设置表ID出错！", ex);
            }
        }


        /// <summary>
        /// 保存事件
        /// </summary>
        public void btnSave() 
        {

            if (StringUtil.IsBlank(this.tbxDialogID.Value,
                this.tbxMapID.Value,
                this.cbxParentID.Value))
            {
                MessageBox.Alert("请把信息填完成！");
                return;
            }

            //if(string.IsNullOrEmpty(this.tbxDialogID.Value) || string.IsNullOrEmpty(this.tbxMapID.Value) || string.IsNullOrEmpty(this.cbxParentID.Value))
            //{
            //    MessageBox.Alert("请把信息填完成！");
            //    return;
            //}


            string json = GetJson();



        }

        /// <summary>
        /// 获取json
        /// </summary>
        private string GetJson() 
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat(
                "dialog_id:{0},map_id:{1},parent_id_field:{2},l_table:{3},r_table:{4}",
                this.tbxDialogID.Value,
                this.tbxMapID.Value,
                this.cbxParentID.Value,
                this.tbxLTable.Value,
                this.tbxRTable.Value
                );
            sb.Append("}");
            return sb.ToString();

        }

    }
}