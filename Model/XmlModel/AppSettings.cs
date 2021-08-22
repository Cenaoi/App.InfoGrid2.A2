using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace App.InfoGrid2.Model.XmlModel
{
   [XmlRoot("app")]
   public class AppSettings
    {
       /// <summary>
       /// 键名称
       /// </summary>
       [XmlAttribute("key")]
       public  string key { get; set; }

       /// <summary>
       /// 值
       /// </summary>
       [XmlAttribute("value")]
       public string value { get; set; }
       /// <summary>
       /// 描述
       /// </summary>
       [XmlAttribute("desc")]
       public string desc { get; set; }




    }
}
