using App.InfoGrid2.GBZZZD.Bll;
using EC5.IO;
using EC5.Utility.Web;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.GBZZZD.Api
{
    /// <summary>
    /// DownloadPrintFile 的摘要说明
    /// </summary>
    public class DownloadPrintFile : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            context.Response.AddHeader("Access-Control-Allow-Credentials", "true");

            context.Response.AddHeader("Access-Control-Allow-Headers", "Origin,Content-Type,Authorization,X-Token");

            int fileId = WebUtil.QueryInt("fileId");

            string filePath = WebUtil.QueryTrim("filePath");

            if (fileId == 0 && string.IsNullOrWhiteSpace(filePath))
            {
                context.Response.Write(HttpResult.Error("请传入文件Id").ToJson());
            }
            else
            {
                if (fileId != 0)
                {
                    LModel printFile = ApiHelper.GetPrintFileInfo(fileId);

                    filePath = printFile.Get<string>("COL_10");

                    //string filePath = @"D:\图片\桌面.jpg";
                }

                filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);

                if (!File.Exists(filePath))
                {
                    context.Response.Write(HttpResult.Error("找不到文件").ToJson());

                    return;
                }

                System.IO.FileInfo file = new System.IO.FileInfo(filePath);

                //byte[] fileData = File.ReadAllBytes(filePath);

                //string fileName = Path.GetFileName(filePath);

                //context.Response.ContentType = "application/octet-stream";
                //context.Response.AddHeader("Content-Disposition", fileName);

                //context.Response.BinaryWrite(fileData);

                //context.Response.End();

                context.Response.Clear();
                context.Response.Charset = "UTF-8";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                context.Response.AddHeader("Content-Type", "application/octet-stream");
                // 添加头信息，为"文件下载/另存为"对话框指定默认文件名,设定编码为UTF8,防止中文文件名出现乱码
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(file.Name, System.Text.Encoding.UTF8));
                // 添加头信息，指定文件大小，让浏览器能够显示下载进度
                context.Response.AddHeader("Content-Length", file.Length.ToString());
                //指定返回的是一个不能被客户端读取的流，必须被下载
                context.Response.ContentType = "application/ms-excel";
                // 把文件流发送到客户端
                context.Response.WriteFile(file.FullName);
                // 停止页面的执行
                context.Response.End();
            }
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

