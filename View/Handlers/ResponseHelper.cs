using App.InfoGrid2.Model;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.InfoGrid2.Handlers
{
    /// <summary>
    /// Page Response 助手类
    /// </summary>
    public static class ResponseHelper
    {
        /// <summary>
        /// 输出错误消息
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="msg">错误消息</param>
        public static void Result_error(string msg)
        {

            HttpContext.Current.Response.Write(CreateJson(false, msg, new SModel()));

        }

        /// <summary>
        /// 输出成功消息
        /// </summary>
        /// <param name="sm_data">要返回的数据</param>
        /// <param name="context">上下文</param>
        /// <param name="msg">消息，默认为空</param>
        public static void Result_ok(SModel sm_data, string msg = "")
        {

            HttpContext.Current.Response.Write(CreateJson(true, msg, sm_data));

        }


        /// <summary>
        /// 输出成功消息
        /// </summary>
        /// <param name="sm_data">要返回的数据</param>
        /// <param name="context">上下文</param>
        /// <param name="msg">消息，默认为空</param>
        public static void Result_ok(LightModel sm_data, string msg = "")
        {

            HttpContext.Current.Response.Write(CreateJson(true, msg, sm_data));

        }


        /// <summary>
        /// 输出成功消息
        /// </summary>
        /// <param name="sm_data">要返回的数据</param>
        /// <param name="context">上下文</param>
        /// <param name="msg">消息，默认为空</param>
        public static void Result_ok(SModelList sm_data, string msg = "")
        {
            HttpContext.Current.Response.Write(CreateJson(true, msg, sm_data));
        }

        /// <summary>
        /// 输出成功消息
        /// </summary>
        /// <param name="context">上下文对象</param>
        /// <param name="msg">消息</param>
        public static void Result_ok(string msg)
        {

            HttpContext.Current.Response.Write(CreateJson(true, msg, new SModel()));
        }

        /// <summary>
        /// 创建sm对象
        /// </summary>
        /// <param name="result_text">result属性的文本</param>
        /// <param name="msg">消息</param>
        /// <param name="sm_data">数据对象</param>
        /// <returns>json字符串</returns>
        static string CreateJson(bool result_text, string msg, SModel sm_data)
        {
            SModel sm = new SModel();
            sm["success"] = result_text;
            sm["msg"] = msg;
            sm["data"] = sm_data;

            return sm.ToJson();

        }

        static string CreateJson(bool result_text, string msg, LightModel sm_data)
        {
            SModel sm = new SModel();
            sm["success"] = result_text;
            sm["msg"] = msg;
            sm["data"] = sm_data;

            return sm.ToJson();
        }

        /// <summary>
        /// 创建sm对象
        /// </summary>
        /// <param name="result_text">result属性的文本</param>
        /// <param name="msg">消息</param>
        /// <param name="sm_data">数据对象</param>
        /// <returns>json字符串</returns>
        static string CreateJson(bool result_text, string msg, SModelList sm_data)
        {
            SModel sm = new SModel();
            sm["success"] = result_text;
            sm["msg"] = msg;
            sm["data"] = sm_data;

            return sm.ToJson();
        }
    }
}