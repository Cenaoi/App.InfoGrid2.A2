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

            if (fileId == 0)
            {
                context.Response.Write(HttpResult.Error("请传入文件Id").ToJson());
            }
            else
            {
                LModel printFile = ApiHelper.GetPrintFileInfo(fileId);

                string filePath = printFile.Get<string>("COL_10");

                //string filePath = @"D:\图片\桌面.jpg";

                filePath = System.Web.HttpContext.Current.Server.MapPath(filePath);

                string fileName = Path.GetFileName(filePath);

                if (!File.Exists(filePath)) 
                {
                    context.Response.Write(HttpResult.Error("找不到文件").ToJson());

                    return;
                }

                byte[] fileData = File.ReadAllBytes(filePath);

                context.Response.ContentType = "application/octet-stream";
                context.Response.AddHeader("Content-Disposition", fileName);

                context.Response.BinaryWrite(fileData);

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

