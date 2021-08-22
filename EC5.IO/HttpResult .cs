using HWQ.Entity;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.IO
{
    /// <summary>
    /// Json 数据包
    /// </summary>
    public class HttpResult : SModel
    {

        public HttpResult()
        {
            this["success"] = false;
        }



        public bool success
        {
            get { return this.Get<bool>("success", false); }
            set { this["success"] = value; }
        }

        public string msg
        {
            get { return Get("msg", string.Empty); }
            set { this["msg"] = value; }
        }

        public string error_msg
        {
            get { return Get("error_msg", string.Empty); }
            set { this["error_msg"] = value; }
        }

        public dynamic data
        {
            get { return this["data"]; }
            set { this["data"] = value; }
        }


        public static HttpSuccessResult Success()
        {
            return new HttpSuccessResult();
        }

        public static HttpSuccessResult Success(object data)
        {
            return new HttpSuccessResult(data);
        }

        public static HttpSuccessResult SuccessMsg(string msg)
        {
            return new HttpSuccessResult { msg = msg };
        }

        public static HttpErrorResult Error(string error_msg)
        {
            return new HttpErrorResult(error_msg);
        }

        public static HttpErrorResult Error()
        {
            return new HttpErrorResult();
        }

        public static HttpErrorResult Error(string error_code, string error_msg)
        {
            return new HttpErrorResult(error_code, error_msg);
        }


        /// <summary>
        /// 把 Json 数据转换为 SModel 对象
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public new static HttpResult ParseJson(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) throw new ArgumentNullException("jsonData", "转换的 json 对象不能为空.");

            JObject jobj;

            try
            {
                jobj = JsonConvert.DeserializeObject(jsonData) as JObject;
            }
            catch (Exception ex)
            {
                throw new Exception("json 格式不符合标准格式。\n" + jsonData,ex);
            }

            HttpResult model = new HttpResult();

            ModelHelper.ParseJson(model, jobj);

            return model;
        }


        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>
    /// 成功的数据包
    /// </summary>
    public class HttpSuccessResult : HttpResult
    {
        public HttpSuccessResult()
        {
            this.success = true;
        }
        

        public HttpSuccessResult(object data)
        {
            this.success = true;
            this.data = data;
        }


        public HttpSuccessResult(string msg, object data)
        {
            this.success = false;
            this.msg = msg;
            this.data = data;
        }


    }

    /// <summary>
    /// 失败的数据包
    /// </summary>
    public class HttpErrorResult : HttpResult
    {
        public HttpErrorResult()
        {
            this.success = false;
        }

        public HttpErrorResult(string error_msg)
        {
            this.success = false;
            this.error_msg = error_msg;
        }

        public HttpErrorResult(string error_msg, object data)
        {
            this.success = false;
            this.error_msg = error_msg;
            this.data = data;
        }


        public HttpErrorResult(string error_code, string error_msg)
        {
            this.success = false;
            this.error_code = error_code;
            this.error_msg = error_msg;
        }

        public HttpErrorResult(string error_code, string error_msg, object data)
        {
            this.success = false;
            this.error_code = error_code;
            this.error_msg = error_msg;
            this.data = data;
        }



        /// <summary>
        /// 错误编码
        /// </summary>
        public string error_code
        {
            get { return this.Get<string>("error_code"); }
            set { this["error_code"] = value; }
        }
    }
}
