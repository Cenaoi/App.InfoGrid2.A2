using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using EC5.Utility;

namespace App.InfoGrid2.View
{
    /// <summary>
    /// 404 错误
    /// </summary>
    public class Error404
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 错误内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [JsonIgnore]
        public Exception Exception { get; set; }

        public string StackTrace { get; set; }

        public Error404()
        {

        }


        public Error404( string content)
        {
            this.Title = "错误信息";
            this.Content = content;
        }

        public Error404(string title, string content)
        {
            this.Title = title;
            this.Content = content;
        }

        public Error404(string title,string content,Exception ex)
        {
            this.Title = title;
            this.Content = content;
            this.Exception = ex;
        }

        public string GetBase64()
        {
            string json =  JsonConvert.SerializeObject(this);

            string base64 = Base64Util.ToString( json, Base64Mode.Http);

            return base64;
        }

        public void SetBase64(string base64)
        {
            string json  = Base64Util.FromString(base64, Base64Mode.Http);

            Error404 err404 = JsonConvert.DeserializeObject<Error404>(json);

            this.Title = err404.Title;
            this.Content = err404.Content;
            this.StackTrace = err404.StackTrace;
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        public static void Send( string content)
        {
            Error404 err404 = new Error404("错误信息", content);

            Send(err404);
        }


        /// <summary>
        /// 显示错误信息
        /// </summary>
        public static void Send(string title, string content)
        {
            Error404 err404 = new Error404(title, content);

            Send(err404);
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        public static void Send(Error404 err404)
        {

            HttpContext context = HttpContext.Current;

            HttpResponse response = context.Response;

            string base64 = err404.GetBase64();

            context.Items["ERR_CODE"] = "200";

            response.Redirect("/App/InfoGrid2/View/Explorer/ErrMessage.aspx?msg=" + base64,false);
        }

    }


}