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
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.Wxhb.Handlers
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
                    case "UPLOAD_IMG":
                        result = UploadImg();
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
        /// 上传图片函数
        /// </summary>
        /// <returns></returns>
        HttpResult UploadImg()
        {

            string media_id = WebUtil.FormTrim("media_id");

            string table_name = WebUtil.FormTrim("table_name");    //CMS 模块

            string cata_code = WebUtil.FormTrim("cata_code");

            int item_id = WebUtil.FormInt("item_id");

            string item_code = WebUtil.FormTrim("item_code");

            string tag_code = WebUtil.FormTrim("tag_code");

            string field_text = WebUtil.FormTrim("field_text");

            if (string.IsNullOrWhiteSpace(media_id))
            {
                return HttpResult.Error("图片的媒体ID不能为空！");
            }

            NameValueCollection nv = new NameValueCollection();

            var url = GlobelParam.GetValue<string>("WX_MENU_API", "http://yq.gzbeih.com/API", "微信公共API地址");

            nv["key"] = GlobelParam.GetValue<string>("WX_MENU_API_KEY", "WXHB", "在微信公共API中的关键字");

            nv["media_id"] = media_id;

            SModel sm_result = null;

            using (WebClient wc = new WebClient())
            {
                //这是提交键值对类型的
                byte[] json = wc.UploadValues(url + "/DownloadImg.ashx", "POST", nv);

                sm_result = ImageUpload(table_name, field_text, item_id, item_code, tag_code, json);

            }


            return HttpResult.Success(sm_result);


        }


        /// <summary>
        /// 上传图片功能
        /// </summary>
        /// <param name="table_name">表名</param>
        /// <param name="field_text">字段名称</param>
        /// <param name="item_id">行ID</param>
        /// <param name="item_code">行编码</param>
        /// <param name="tag_code">标签编码</param>
        /// <param name="img_data">图片流数据</param>
        /// <returns></returns>
        private SModel ImageUpload(string table_name, string field_text, int item_id, string item_code, string tag_code,byte[] img_data)
        {
            int span = 500;
            int m0 = (item_id - item_id % span) / span;
            int m1 = m0 + 1;

            int span0 = m0 * span;
            int span1 = m1 * span;

            string revDir = $"/UserFile/form/{table_name}/{span0:0000}-{span1:0000}/{item_id:0000}";

            string dir = HttpContext.Current.Server.MapPath(revDir);

        
            string file_name = DateTime.Now.ToString("yyyy_MM_dd HH_mm_ss_ffff") +".png";

            string newFilename = FileUtil.GetUniqueName(dir, file_name);


            WebFileInfo fi = new WebFileInfo(revDir, newFilename);


            FileUtil.AutoCreateDir(fi.PhysicalDir);

            //保存图片
            File.WriteAllBytes(fi.PhysicalPath, img_data);


            #region 产生缩略图

            string thumbPath = fi.RelativePath;


            string thumbRevDir = revDir + "/_thumb";

            string thumbDir = HttpContext.Current.Server.MapPath(thumbRevDir);

            //string thumbFullname = thumbDir + newFilename;

            string thumbFullname = FileUtil.GetUniqueName(thumbDir, newFilename);

            WebFileInfo thumbFI = new WebFileInfo(thumbRevDir, newFilename);

            using (MemoryStream ms = new MemoryStream(img_data))
            {

                Bitmap bmp = ImageUtil.CreateThumb(ms, 128, 128);

                FileUtil.AutoCreateDir(thumbFI.PhysicalDir);

                bmp.Save(thumbFI.PhysicalPath, ImageFormat.Jpeg);

            }

         
            #endregion


            EcUserState userState = EcContext.Current.User;

            BIZ_FILE bFile = new BIZ_FILE();
            bFile.TAG_CODE = tag_code;

            bFile.FILE_NAME = file_name;
            bFile.TABLE_NAME = table_name;
            bFile.FIELD_NAME = field_text;

            bFile.ROW_ID = item_id;
            bFile.ROW_CODE = item_code;
            bFile.FILE_PATH = fi.RelativePath;
            bFile.FILE_SIZE = img_data.Length;
            bFile.FILE_EX = fi.Extension;
            bFile.TABLE_TYPE = "table";
            bFile.ROW_AUTHOR_USER_CODE = userState.ExpandPropertys["USER_CODE"];

            DbDecipher decipher = ModelAction.OpenDecipher();

            decipher.InsertModel(bFile);

            SModel msg = new SModel();
            msg["state"] = "SUCCESS";
            msg["type"] = fi.Extension;
            msg["size"] = img_data.Length;
            msg["url"] = fi.RelativePath;
            msg["name"] = fi.Filename;
            msg["thumb_url"] = thumbPath;
            msg["original"] = file_name;
            msg["code"] = bFile.BIZ_FILE_ID;

            return msg;
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

            if (lm == null)
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

                string file_path = HttpContext.Current.Server.MapPath(biz_file.FILE_PATH);
                //删除文件
                File.Delete(file_path);

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

            if (StringUtil.HasBlank(table_name, field_text))
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