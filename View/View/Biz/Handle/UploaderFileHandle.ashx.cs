using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.SystemBoard;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.View.Biz.Handle
{
    /// <summary>
    /// UploaderFileHandle 的摘要说明
    /// </summary>
    public class UploaderFileHandle : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";

            try
            {

                string action = WebUtil.Query("action");

                switch (action)
                {

                    case "BATCH_UPLOAD":
                        BatchUpload();
                        break;
                    case "DELETE_ITEM":
                        DeleteItem();
                        break;
                    default:
                        Result_error("写错了吧");
                        break;


                }
            }catch(Exception ex)
            {
                log.Error(ex);
                Result_error("哦噢，出错了喔！");

            }

        }


        /// <summary>
        /// 多文件上传事件
        /// </summary>
        void BatchUpload()
        {
            string table_name = WebUtil.QueryTrim("table_name");

            int table_id = WebUtil.QueryInt("table_id");

            int row_id = WebUtil.QueryInt("row_id");

            string table_type = WebUtil.Query("table_type");


            if (string.IsNullOrWhiteSpace(table_name))
            {
                Result_error("表名不能为空！");
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();


            #region   限制biz_sid > 0 的不能上传



            LModel lm = decipher.GetModelByPk(table_name, row_id);

            if(lm == null)
            {
                Result_error("找不到当前记录信息！");
                return;
            }


            if(lm.Get<int>("BIZ_SID") > 0)
            {

                Result_error("业务状态大于0时不能上传文件！");

                return;

            }


            #endregion


            HttpPostedFile _upfile = HttpContext.Current.Request.Files[0];

            if (_upfile == null)
            {
                throw new Exception("没有文件！");
            }

           

            //上传文件路径
            string base_file_path = GlobelParam.GetValue("UPLOADER_FILE_PATH", "C:\\用户自定义目录\\", "上传文件路径");

            string fileName = _upfile.FileName;/*获取文件名： C:\Documents and Settings\Administrator\桌面\123.jpg*/

            string  suffix = Path.GetExtension(fileName);  // fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();/*获取后缀名并转为小写： jpg*/

            int file_size = _upfile.ContentLength;//获取文件的字节大小  

            string file_path = CreateFolder();

            string old_file_name = Path.GetFileNameWithoutExtension(fileName);

            string file_name = $"{old_file_name}_{DateTime.Now.ToString("ssfff")}{suffix}";


           _upfile.SaveAs( $"{base_file_path}\\{file_path}{file_name}");


            BIZ_FILE bf = new BIZ_FILE();


            bf.TABLE_NAME = table_name;
          
            bf.TABLE_ID = table_id;
            bf.ROW_ID = row_id;
            bf.TABLE_TYPE =  table_type;
            bf.FILE_NAME = fileName;
            bf.FILE_PATH = file_path +file_name;
            bf.FILE_SIZE = file_size;
            bf.FILE_EX = suffix;
            bf.TAG_CODE = "form_annex";


            decipher.InsertModel(bf);


            //string rowJson = ModelConvert.ToJson(bf);
            //string json = "{\"result\":\"ok\",\"msg\":\"\",\"data\":" + rowJson + "}";

            SModel data = new SModel();
            bf.CopyTo(data);
            
            data["FILE_SIZE_STR"] = NumberUtil.FormatFileSize(file_size);

            string fex = suffix.TrimStart('.');

            string path = $"/res/file_icon_256/{fex}.png";

            if (WebFile.Exists(path))
            {
                data["EX_PATH"] = path;
            }
            else
            {
                data["EX_PATH"] = "/res/file_icon_256/undefined.png";
            }

            SModel result = new SModel();
            result["result"] = "ok";
            result["msg"] = string.Empty;
            result["data"] = data;

            string json = result.ToJson();
             

            HttpContext.Current.Response.Write(json);


        }


        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <returns></returns>
        string  CreateFolder()
        {

            //上传文件路径
            string base_file_path = GlobelParam.GetValue<string>("UPLOADER_FILE_PATH", "C:", "上传文件路径");

            string table_name = WebUtil.QueryTrim("table_name");

            int table_id = WebUtil.QueryInt("table_id");

            int row_id = WebUtil.QueryInt("row_id");

            string table_type = WebUtil.Query("table_type");

            int aa = row_id % 500;
            int bb = row_id - aa;
            int cc = bb + 500;


            string file_path_1 = $"{base_file_path}\\{table_name}\\{bb:0000}_{cc:0000}\\{aa:0000}\\";


            if (!Directory.Exists(file_path_1))
            {
                Directory.CreateDirectory(file_path_1);

            }

            return $"{table_name}\\{bb:0000}_{cc:0000}\\{aa:0000}\\";

        }

        /// <summary>
        /// 删除附件的，根据ID
        /// </summary>
        void DeleteItem()
        {

            int file_id = WebUtil.QueryInt("biz_file_id");

            string table_name = WebUtil.QueryTrim("table_name");


            int row_id = WebUtil.QueryInt("row_id");

            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();


            #region   限制biz_sid > 0 的不能上传



            LModel lm = decipher.GetModelByPk(table_name, row_id);

            if (lm == null)
            {
                Result_error("找不到当前记录信息！");
                return;
            }


            if (lm.Get<int>("BIZ_SID") > 0)
            {

                Result_error("业务状态大于0时不能删除文件！");

                return;

            }


            #endregion


            BIZ_FILE biz_file = decipher.SelectModelByPk<BIZ_FILE>(file_id);


            if(biz_file == null)
            {
                Result_error("找不到附件数据！");
                return;
            }

            
            biz_file.ROW_SID = -3;
            biz_file.ROW_DATE_DELETE = DateTime.Now;
            biz_file.REMARKS = $"[{user.LoginName}]删除的";

            decipher.UpdateModelProps(biz_file, "ROW_SID", "ROW_DATE_DELETE", "REMARKS");


            Result_ok(null);

        }



        void Result_error(string msg)
        {

            SModel sm = new SModel();
            sm["result"] = "error";
            sm["msg"] = msg;
            sm["data"] = null;


            HttpContext.Current.Response.Write(sm.ToJson());



        }

        void Result_ok(SModel sm_data)
        {

            SModel sm = new SModel();
            sm["result"] = "ok";
            sm["msg"] = "";
            sm["data"] = sm_data;

            HttpContext.Current.Response.Write(sm.ToJson());

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