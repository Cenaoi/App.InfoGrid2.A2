using App.BizCommon;
using App.InfoGrid2.Handlers;
using App.InfoGrid2.Model;
using App.InfoGrid2.WF.Bll;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.WF.Handlers
{
    /// <summary>
    /// 上传文件处理类
    /// </summary>
    public class UploaderFileHandle : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";

            HttpResult result = null;



            try
            {

                string action = WebUtil.FormTrimUpper("action");

               
                switch (action)
                {

                    case "DELETE_ITEM":
                        result = DeleteItem();
                        break;
                    case "SAVE_FILE_INFO_BY_FIELD":
                        result = SaveFileInfoByField();
                        break;
                    default:
                        result = HttpResult.Error("写错了吧");

                        

                        break;


                }



            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = HttpResult.Error(ex.Message);

            }


            context.Response.Write(result);

        }



        /// <summary>
        /// 删除附件的，根据ID
        /// </summary>
        HttpResult DeleteItem()
        {

            int file_id = WebUtil.FormInt("delete_code");

            string table_name = WebUtil.FormTrim("table_name");

            int row_id = WebUtil.FormInt("row_id");

            string field_text = WebUtil.FormTrim("field_text");

            string field_value = BusHelper.ParseFiles(WebUtil.FormTrim("file_json"));


     

            DbDecipher decipher = ModelAction.OpenDecipher();



            LModel lm = decipher.GetModelByPk(table_name, row_id);

            if(lm == null)
            {
                throw new Exception($"找不到【{table_name}】表行ID【{row_id}】记录！");

            }

            lm[field_text] = field_value;
            lm["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.UpdateModelProps(lm, field_text, "ROW_DATE_UPDATE");


            EcUserState user = EcContext.Current.User;


            BIZ_FILE biz_file = decipher.SelectModelByPk<BIZ_FILE>(file_id);

            if (biz_file != null)
            {
                biz_file.ROW_SID = -3;
                biz_file.ROW_DATE_DELETE = DateTime.Now;
                biz_file.REMARKS = $"[{user.LoginName}]删除的";

                decipher.UpdateModelProps(biz_file, "ROW_SID", "ROW_DATE_DELETE", "REMARKS");

            }


            return HttpResult.SuccessMsg("");


        }

        /// <summary>
        /// 保存文件信息到表的字段里面去
        /// </summary>
        HttpResult SaveFileInfoByField()
        {
            
            string file_json = WebUtil.FormTrim("file_json");

            string table_name = WebUtil.FormTrim("table_name");

            int row_id = WebUtil.FormInt("row_id");

            string field_text = WebUtil.FormTrim("field_text");


            if (string.IsNullOrWhiteSpace(table_name) || string.IsNullOrWhiteSpace(field_text))
            {
                return HttpResult.Error("表名不能为空！");
            }

            string field_value = BusHelper.ParseFiles(file_json);

            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm = decipher.GetModelByPk(table_name, row_id);

            if (lm == null)
            {
                return HttpResult.Error($"找不到【{table_name}】表行ID【{row_id}】记录！");

            }

            lm[field_text] = field_value;
            lm["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.UpdateModelProps(lm, field_text, "ROW_DATE_UPDATE");


            return HttpResult.SuccessMsg("保存上传文件数据成功了！");


        }

            
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}