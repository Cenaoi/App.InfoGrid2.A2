using App.InfoGrid2.Model.XmlModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace App.InfoGrid2.Bll
{
    /// <summary>
    /// 读取自定义配置文件管理类
    /// </summary>
    public static class CustomViewMgr
    {


        /// <summary>
        /// 获取配置文件信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static CustomView GetXml(string fileName)
        {

            CustomView info = null;

            if (string.IsNullOrEmpty(fileName))
            {
                throw new Exception("配置文件地址不能为空："+fileName);
            }
                try
                {
                    string c_ui = EC5.SystemBoard.SysBoardManager.CurrentApp.AppSettings["custom_ui"];

                    string url = c_ui + "/" + fileName;

                    //xml来源可能是外部文件，也可能是从其他系统获得
                    string paths = HttpContext.Current.Server.MapPath(url);

                    if (!File.Exists(paths))
                    {
                        throw new Exception("找不到配置文件路径："+fileName);
                    }

                    FileStream file = new FileStream(paths, FileMode.Open, FileAccess.Read);

                    XmlSerializer xmlSearializer = new XmlSerializer(typeof(CustomView));

                    info = (CustomView)xmlSearializer.Deserialize(file);

                    file.Close();

                    file.Dispose();

                    return info;


                }
                catch (Exception ex)
                {
                    throw new Exception("反系列化xml文件出错了！", ex);
                }

        }


    }
}
