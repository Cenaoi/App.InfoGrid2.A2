using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel;

namespace EasyClick.BizWeb2
{
    public class row_style
    {
        public row_style()
        {

        }

        /// <summary>
        /// 行背景颜色
        /// </summary>
        [DefaultValue(""), JsonProperty(NullValueHandling=NullValueHandling.Ignore) ]
        public string bg_color { get; set; }

        /// <summary>
        /// 行字体颜色
        /// </summary>
        [DefaultValue(""), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string font_color { get; set; }


        col_style_Collection m_Cols;


        /// <summary>
        /// 列信息
        /// </summary>
        public col_style_Collection cols
        {
            get
            {
                if (m_Cols == null)
                {
                    m_Cols = new col_style_Collection();
                }
                return m_Cols;
            }
        }


        public string ToJsonString()
        {
            if (m_Cols != null)
            {
                List<col_style> deleteCols = new List<col_style>();

                foreach (col_style col in m_Cols)
                {
                    if (col.msgs.Count == 0)
                    {
                        deleteCols.Add(col);
                    }
                }

                foreach (col_style col in deleteCols)
                {
                    m_Cols.Remove(col);
                }
            }


            return JsonConvert.SerializeObject(this); ;
        }

        public static row_style ParseJson(string json)
        {
            try
            {
                row_style rsi = JsonConvert.DeserializeObject<row_style>(json);

                return rsi;
            }
            catch (Exception ex)
            {
                throw new Exception("序列化 JSON 失败。"+ json, ex);
            }
           // return null;
           
        }
    }


    public class col_style_Collection:List<col_style>
    {
        public col_style this[string field]
        {
            get
            {
                return FindByField(field);
            }
        }


        public col_style FindByField(string field)
        {
            foreach (col_style col in this)
            {
                if (col.name == field)
                {
                    return col;
                }
            }

            return null;
        }

    }


    public class col_style
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 列背景颜色
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string bg_color { get; set; }

        /// <summary>
        /// 列字体颜色
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string font_color { get; set; }


        List<col_msg> m_msgs;

        /// <summary>
        /// 信息列表
        /// </summary>
        public List<col_msg> msgs
        {
            get
            {
                if (m_msgs == null)
                {
                    m_msgs = new List<col_msg>();
                }
                return m_msgs;
            }
        }

    }

    /// <summary>
    /// 列消息
    /// </summary>
    public class col_msg
    {
        /// <summary>
        /// 信息类型
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }

        /// <summary>
        /// 信息内容
        /// </summary>
        public string message { get; set; }

        public col_msg()
        {
        }

        /// <summary>
        /// 列消息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        public col_msg(string type, string message)
        {
            this.type = type;
            this.message = message;
        }

    }


    /*
     {
        bg_color:'008895',
        font_color:'008895',
        cols:[
       
         { 
            col: '列名' , bg_color:'背景颜色' , font_color:'字体颜色', 
            msg ：[ { type:1, text:'信息1' },{ type:2, text:'信息2' },{ text:'信息3' } ]
         }，
         { 
             col: '列名' , bg_color:'背景颜色' , font_color:'字体颜色', 
             msg ：[ { text:'不能为0' },{ text:'不能纯英文' },{ text:'必须以250开头' } ]     
        }
         
        ]
      } 
     
     */


}
