using App.BizCommon;
using App.InfoGrid2.Model.WeChat;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using Sysboard.Web.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace App.InfoGrid2.WF.Task
{
    /// <summary>
    /// 发送微信模板消息任务类
    /// </summary>
    public class SendWxTempMsg : WebTask
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SendWxTempMsg()
        {

            this.TaskSpan = new TimeSpan(0, 0, 10);

            this.TaskType = WebTaskTypes.Span;

            this.TaskTime = DateTime.Now;

        }


        public override void Exec()
        {

            DbDecipher decipher = null;

            try
            {
                decipher = DbDecipherManager.GetDecipherOpen();
            }
            catch (Exception ex)
            {
                log.Error("打开数据库失败了！", ex);
                return;

            }


            try
            {

                SendTempMsg(decipher);

            }
            catch (Exception ex)
            {


                log.Error("发送模板消息出错了！-------------------------------------------------------------------------", ex);
            }
            finally
            {

                decipher.Dispose();

            }


        }

        /// <summary>
        /// 发送模板消息函数
        /// </summary>
        /// <param name="decipher"></param>
        void SendTempMsg(DbDecipher decipher)
        {


            LightModelFilter lmFilter = new LightModelFilter(typeof(WX_TEMP_MSG));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID", 0);
            lmFilter.Top = 1;

            WX_TEMP_MSG wtm = decipher.SelectToOneModel<WX_TEMP_MSG>(lmFilter);

            if (wtm == null)
            {
                return;
            }


            NameValueCollection nv = new NameValueCollection();

            var url = GlobelParam.GetValue<string>(decipher, "WX_MENU_API", "http://wc.51.yq-ic.com/API", "微信公共API地址");


            nv["key"] = GlobelParam.GetValue<string>(decipher, "WX_MENU_API_KEY", "MTC", "在微信公共API中的关键字");


            nv["post_data"] = wtm.TMEP_MSG_DATA;


            using (WebClient wc = new WebClient())
            {
                //这是提交键值对类型的
                byte[] json = wc.UploadValues(url + "/TemplateMsg.ashx", "POST", nv);

                string result = Encoding.UTF8.GetString(json);

                log.Info("发送模板返回的数据：" + result);

                wtm.RETURN_DATA = result;

            }


            wtm.BIZ_SID = 2;
            wtm.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(wtm, "BIZ_SID", "RETURN_DATA", "ROW_DATE_UPDATE");


        }


    }
}