using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace App.InfoGrid2.Bll.Builder
{
    /// <summary>
    /// 脚本的节点
    /// </summary>
    public class TItemScript:TemplateItem
    {

        /// <summary>
        /// 存放src属性的值
        /// </summary>
        public string Src { get; set; }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="rowData"></param>
        public override void DeserializeJsonAttrs(JToken rowData)
        {
            base.DeserializeJsonAttrs(rowData);

            this.Src = GetJsonAttr(rowData, "src");
        }

        public override void SerializableXmlAttrs(StringBuilder sb)
        {
            base.SerializableXmlAttrs(sb);

            SetXmlAttr(sb, "src", this.Src);
        }
    }
}
