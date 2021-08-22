using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EasyClick.Web.Mini2
{



    /// <summary>
    /// 输入提示
    /// </summary>
    public class Typeahead
    {
        /// <summary>
        /// 激活
        /// </summary>
        [DefaultValue(true)]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 远程地址
        /// </summary>
        [DefaultValue("")]
        public string RemoteUrl { get; set; }

        /// <summary>
        /// 查询的关键名
        /// </summary>
        [DefaultValue("name")]
        public string SearchKey { get; set; } = "name";

        /// <summary>
        /// 展示的格式
        /// </summary>
        [DefaultValue("")]
        public string DisplayFormat { get; set; }

        /// <summary>
        /// 最少输入一个字符
        /// </summary>
        [DefaultValue(1)]
        public int MinLength { get; set; } = 1;

        /// <summary>
        /// 延迟显示
        /// </summary>
        [DefaultValue(0)]
        public int Delay { get; set; }

        /// <summary>
        /// 显示的数量
        /// </summary>
        [DefaultValue(8)]
        public int Items { get; set; } = 8;

        /// <summary>
        /// 自动选中
        /// </summary>
        [DefaultValue(false)]
        public bool AutoSelect { get; set; } = false;

        /// <summary>
        /// 赋值的字段名
        /// </summary>
        [DefaultValue("")]
        public string Field { get; set; }


        /// <summary>
        /// 输出到 json 
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            ScriptTextWriter st = new ScriptTextWriter(QuotationMarkConvertor.SingleQuotes);

            st.RetractBengin("{");

            st.WriteParam("enabled", this.Enabled,false);
            st.WriteParam("remoteUrl", this.RemoteUrl);
            st.WriteParam("searchKey", this.SearchKey, "name");
            st.WriteParam("displayFormat", this.DisplayFormat);
            st.WriteParam("minLength", this.MinLength);
            st.WriteParam("delay", this.Delay);
            st.WriteParam("items", this.Items);
            st.WriteParam("autoSeelct", this.AutoSelect, false);
            st.WriteParam("field", this.Field);

            st.RetractEnd("}");

            string json = st.ToString();

            return json;
        }

        public override string ToString()
        {
            return ToJson();
        }
    }
}
