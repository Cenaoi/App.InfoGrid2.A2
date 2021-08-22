using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.V1
{
    /// <summary>
    /// 模板参数对象
    /// </summary>
   public class TempParam
    {

        public TempParam() { }


        List<SubTableParam> m_subTableParam = null;

        SheetMargin m_sheetMargin = null;


        Dictionary<string, string> m_dict = null;


        /// <summary>
        /// 子表参数集合
        /// </summary>
        public List<SubTableParam> SubTableParam
        {

            get
            {
                if(m_subTableParam == null)
                {
                    m_subTableParam = new List<SubTableParam>();
                }


                return m_subTableParam;

            }


            set
            {

                m_subTableParam = value;


            }



        }


        /// <summary>
        /// 参数字典
        /// </summary>
        public Dictionary<string,string> ParamDict
        {
            get
            {

                if(m_dict == null)
                {

                    m_dict = new Dictionary<string, string>();

                }

                return m_dict;

            }


            set
            {

                m_dict = value;

            }

        }



        /// <summary>
        /// 数据区开始行索引  默认0
        /// </summary>
        public int FirstRowIndex { get; set; } = 0;


        /// <summary>
        /// 数据区结束行索引 默认 0
        /// </summary> 
        public int LastRowIndex { get; set; } = 0;


        /// <summary>
        /// 自定义宽度 这个要转成 PaperSize 类用的宽度
        /// </summary>
        public double PageWidth
        {
            get
            {
                return Width * 3.9370079d;

            }


        }
        /// <summary>
        /// 自定义高度  这个要转成 PaperSize 类用的高度
        /// </summary>
        public double PageHeight
        {

            get
            {

               

                return Height * 3.9370079d;


            }


        }

        /// <summary>
        /// 原始宽 默认 210mm
        /// </summary>
        public int Width { get; set; } = 210;
        /// <summary>
        /// 原始高 默认 297mm
        /// </summary>
        public int Height { get; set; } = 297;

        /// <summary>
        /// 是否合计 默认不合计  false -- 不合计 ，ture -- 合计 
        /// </summary>
        public bool IsTotal { get; set; } = false;

        /// <summary>
        /// 数据区类型  
        /// </summary>
        public DataAreaType DataAreaType { get; set; } = DataAreaType.NONE;

        /// <summary>
        /// 自定义JSON数据对象
        /// </summary>
        public SModel Sm { get; set; }


        /// <summary>
        /// 合计的json对象
        /// </summary>
        public SModel TotalSm { get; set; }



        /// <summary>
        /// 根据key来获取相对应的值  没有这个可以 就返回 string.Empty
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetParam(string key)
        {

            if (ParamDict.ContainsKey(key))
            {
                return ParamDict[key]?.Trim();
            }
            return string.Empty;

        }


    }
}
