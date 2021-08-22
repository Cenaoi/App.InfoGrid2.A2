using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using Sysboard.Web.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace App.InfoGrid2.View
{
    public class TelInfoTask : WebTask
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TelInfoTask()
        {
            this.TaskSpan = new TimeSpan(0, 0, 10);

            this.TaskType = WebTaskTypes.Span;

            this.TaskTime = DateTime.Parse("2016-1-1");
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
                log.Error("打开数据库失败", ex);
                return;
            }

            try
            {
                ProTel(decipher);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                if (decipher != null)
                {
                    decipher.Dispose();
                }
            }

        }


        int m_BufferLen = 1024 * 4;

        private byte[] GetData(Stream stream)
        {
            byte[] buffer = new byte[m_BufferLen];

            int actual = 0;

            //先保存到内存流中MemoryStream  
            MemoryStream ms = new MemoryStream();
            while ((actual = stream.Read(buffer, 0, m_BufferLen)) > 0)
            {
                ms.Write(buffer, 0, actual);
            }

            ms.Position = 0;

            buffer = ms.ToArray();

            ms.Dispose();

            return buffer;
        }

        private void ProTel(DbDecipher decipher)
        {
            LightModelFilter filter = new LightModelFilter("UT_001");
            filter.And("ROW_SID", 0, HWQ.Entity.Filter.Logic.GreaterThanOrEqual);
            filter.And("COL_3", "");
            filter.Top = 60;


            LModelList<LModel> models = decipher.GetModelList(filter);

            foreach (LModel m in models)
            {
                string poeName = (string)m["COL_3"];

                if (!string.IsNullOrEmpty(poeName))
                {
                    continue;
                }

                string tel = (string)m["COL_2"];

                if (string.IsNullOrEmpty(tel))
                {
                    continue;
                }

                try
                {
                    string area = GetTelArea(tel);

                    m["COL_3"] = string.IsNullOrEmpty(area) ? "NA" : area;
                }
                catch(Exception ex)
                {
                    log.Error("获取远程手机号码所在地失败。 TEL=" + tel, ex);
                    continue;
                }

                decipher.UpdateModelProps(m, "COL_3");

            }



        }

        private string GetTelArea(string tel)
        {
            string appkey = "12390";
            string sign = "068e57d16901fe35b89bb126853b1905";
            string format = "xml";

            string url = $"http://api.k780.com:88/?app=phone.get&phone={tel}&appkey={appkey}&sign={sign}&format={format}";

            System.Net.HttpWebResponse response = EC5.YQIC.Windows.HttpUtilitys.HttpRequestUtil.CreateGetHttpResponseBase(url, null);

            Stream stream = response.GetResponseStream();   //获取响应的字符串流

            byte[] data = GetData(stream);

            string text = Encoding.UTF8.GetString(data);
                       


            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text);

            XmlNode node = doc.SelectSingleNode("/root/result/att");

            if(node == null)
            {
                throw new Exception("获取节点数据失败，请稍后再尝试.\r\n" + text);
            }

            string area = null;

            if(node != null)
            {
                area = node.InnerText;
            }
        
            return area;
        }


        
    }

}