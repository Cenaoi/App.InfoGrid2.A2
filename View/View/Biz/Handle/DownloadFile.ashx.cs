using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.View.Biz.Handle
{
    /// <summary>
    /// DownloadFile 的摘要说明
    /// </summary>
    public class DownloadFile : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void ProcessRequest(HttpContext context)
        {

            int biz_file_id = WebUtil.QueryInt("biz_file_id");


            DbDecipher decipher = ModelAction.OpenDecipher();

            BIZ_FILE bf = decipher.SelectModelByPk<BIZ_FILE>(biz_file_id);

            //上传文件路径
            string base_file_path = GlobelParam.GetValue("UPLOADER_FILE_PATH", "C:", "上传文件路径");


            string file_path = base_file_path +"\\" +bf.FILE_PATH;


            FileInfo file = new FileInfo(file_path);//创建一个文件对象

            HttpResponse response = context.Response;

            response.Clear();//清除所有缓存区的内容
            response.Charset = "UTF8";//定义输出字符集
            response.ContentEncoding = Encoding.UTF8;//输出内容的编码为默认编码
            response.AddHeader("Content-Disposition", "attachment;filename=" + file.Name);//添加头信息。为“文件下载/另存为”指定默认文件名称
            response.AddHeader("Content-Length", file.Length.ToString());//添加头文件，指定文件的大小，让浏览器显示文件下载的速度
            response.WriteFile(file.FullName);// 把文件流发送到客户端
            response.End();//将当前所有缓冲区的输出内容发送到客户端，并停止页面的执行

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