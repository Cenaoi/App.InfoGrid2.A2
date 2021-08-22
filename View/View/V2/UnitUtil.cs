using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Web;

namespace App.InfoGrid2.View.V2
{
    /// <summary>
    /// 这是返回值
    /// </summary>
    public class MyJosn 
    {
        /// <summary>
        /// 结果 OK and ERROR
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 信息，结果等于OK时没有值
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="result">结果</param>
        /// <param name="message">信息</param>
        public MyJosn(string result, string message) 
        {
            this.result = result;
            this.message = message;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}",this.result,this.message);
        }

    }


    /// <summary>
    /// 单位工具类
    /// </summary>
    public class UnitUtil
    {
        /// <summary>
        /// 单位换算 根据 需求 算出 数量 
        /// </summary>
        /// <param name="lm">实体</param>
        /// <param name="COL_9">计量单位</param>
        /// <param name="COL_35">数量</param>
        /// <param name="COL_19">长度</param>
        /// <param name="COL_5">密度</param>
        /// <param name="COL_17">厚度</param>
        /// <param name="COL_4">宽度</param>
        /// <param name="COL_36">板长</param>
        /// <param name="COL_91">需求量单位</param>
        /// <param name="COL_90">需求量</param>
        /// <param name="col_35">数量字段比如：COL_35</param>
        /// <param name="col_19">长度字段 比如：COL_19</param>
        /// <returns></returns>
        public static MyJosn UnitConversion(LModel lm, string COL_9, decimal COL_35, decimal COL_19, decimal COL_5, decimal COL_17, decimal COL_4, decimal COL_36, string COL_91, decimal COL_90,string col_35,string col_19)
        {

            

            ///长度COL_19 =板长COL_36          *1000+70
            COL_19 = COL_36 * 1000 + 70;

            #region 根据 需求量 COL_90  计算出 数量COL_35

            if (string.IsNullOrEmpty(COL_9) || string.IsNullOrEmpty(COL_91) || COL_90 == 0)
            {
                return new MyJosn("OK", "检测到当前记录正在进行单位换算，{计量单位}或{需求单位}或{需求量}不能为空或零！");
            }

            COL_9 = COL_9.Trim();
            COL_91 = COL_91.Trim();

            //需求量单位COL_91 和单位COL_9不相同时检查长度
            if (COL_91 != COL_9)
            {
                //长度COL_19为空时不计算数量
                if (COL_19 == 0)
                {
                    return new MyJosn("ERROR", "检测到当前记录正在进行单位换算，长度项目不能为空或零！");
                }
            }

            //计量单位COL_9
            if (COL_9 == "KG" || COL_9 == "公斤")
            {

                switch (COL_91)
                {
                    case "米":
                        //数量COL_35 =   长度  COL_19                 / 1000* 需求量 COL_90
                        COL_35 = COL_19 / 1000m * COL_90;
                        break;
                    case "KG":
                    case "公斤":
                        ///数量COL_35 =需求量 COL_90
                        COL_35 = COL_90;
                        break;
                    case "张":
                        ///数量COL_35 = 密度COL_5 X                 (厚度COL_17 /100)                 X 宽度COL_4               X (长度COL_19 /1000)                   X 需求量 COL_90 
                        COL_35 = COL_5 * (COL_17 / 100m) * COL_4 * (COL_19 / 1000m) * COL_90;
                        break;
                }

            }
            else if (COL_9 == "张")
            {

                switch (COL_91)
                {
                    case "米":
                        ///数量COL_35 =       需求量 COL_90      /    （长度*1000）
                        COL_35 = COL_90 / (1000m * COL_19);
                        break;
                    case "KG":
                    case "公斤":
                        ///数量COL_35 =       需求量 COL_90        *1000   /（密度                X      (厚度/100)                          X 宽度                   X (长度/1000)）（取整数，不用四舍五入）
                        COL_35 = COL_90 * 1000m / (COL_5 * (COL_17 / 100m) * COL_4 * (COL_19 / 1000m));
                        COL_35 = Math.Floor(COL_35);
                        break;
                    case "张":
                        ///数量COL_35 =需求量 COL_90
                        COL_35 = COL_90;
                        break;
                }
            }
            else if (COL_9 == "米")
            {

                switch (COL_91)
                {
                    case "米":
                        //数量COL_35 =需求量 COL_90
                        COL_35 = COL_90;
                        break;
                    case "KG":
                    case "公斤":
                        //数量COL_35 =需求量 COL_90*1000/（密度 X   (厚度/100)  X 宽度  X (长度/1000)） *长度/1000
                        decimal col_10 = COL_90 * 1000m / (COL_5 * (COL_17 / 100m) * COL_4 * (COL_19 / 1000m)) * COL_19 / 1000m;
                        COL_35 = Math.Floor(col_10);
                        break;
                    case "张":
                        //数量COL_35 = 长度 / 1000 *  需求量 COL_90
                        COL_35 = COL_19 / 1000m * COL_90;
                        break;
                }
            }

            #endregion

            lm[col_19] = COL_19;

            lm[col_35] = COL_35;

            return new MyJosn("OK","");


        }

        /// <summary>
        /// 单位换算 只根据 数量 换算出 主数量
        /// </summary>
        /// <param name="lm">实体</param>
        /// <param name="COL_9">计量单位</param>
        /// <param name="COL_59">主单位</param>
        /// <param name="COL_35">数量</param>
        /// <param name="COL_19">长度</param>
        /// <param name="COL_10">主数量</param>
        /// <param name="COL_5">密度</param>
        /// <param name="COL_17">厚度</param>
        /// <param name="COL_4">宽度</param>
        /// <param name="COL_36">板长</param>
        /// <param name="col_10">主数量字段 比如：COL_10</param>
        /// <param name="col_19">长度字段 比如：COL_19</param>
        /// <returns></returns>
        public static MyJosn UnitConversion2(LModel lm, string COL_9, string COL_59, decimal COL_35, decimal COL_19, decimal COL_10, decimal COL_5, decimal COL_17, decimal COL_4, decimal COL_36,string col_10,string col_19) 
        {
            #region 根据 数量COL_35 计算出 主数量COL_10

            

            if (string.IsNullOrEmpty(COL_9) || string.IsNullOrEmpty(COL_59) || COL_35 == 0)
            {
                return new MyJosn("ERROR", "检测到当前记录正在进行单位换算，{计量单位}或{主单位}或{数量}不能为空或零！");
            }

           COL_9 = COL_9.Trim();
           COL_59 =  COL_59.Trim();

            //主单位COL_59和单位COL_9不相同时检查长度
            if (COL_9 != COL_59)
            {
                //长度COL_19为空时不计算数量
                if (COL_19 == 0)
                {
                    return new MyJosn("ERROR", "检测到当前记录正在进行单位换算，长度项目不能为空或零！");

                }
            }

            //计量单位COL_9
            if (COL_9 == "KG" || COL_9 == "公斤")
            {

                switch (COL_59)
                {
                    case "米":
                        //主数量COL_10 =   长度  COL_19                 / 1000* 数量 COL_35
                        COL_10 = COL_19 / 1000m * COL_35;
                        break;
                    case "KG":
                    case "公斤":
                        ///主数量COL_10 =数量COL_35
                        COL_10 = COL_35;
                        break;
                    case "张":
                        ///主数量COL_10 = 密度COL_5 X                 (厚度COL_17 /100)                 X 宽度COL_4               X (长度COL_19 /1000)                   X 数量OL_35  
                        COL_10 = COL_5 * (COL_17 / 100m) * COL_4 * (COL_19 / 1000m) * COL_35;
                        break;
                }

            }
            else if (COL_9 == "张")
            {

                switch (COL_59)
                {
                    case "米":
                        ///主数量COL_10 =       数量COL_35       /    （长度*1000）
                        COL_10 = COL_35 / (1000m * COL_19);
                        break;
                    case "KG":
                    case "公斤":
                        ///主数量COL_10 =       数量COL_35        *1000   /（密度                X      (厚度/100)                          X 宽度                   X (长度/1000)）（取整数，不用四舍五入）
                        COL_10 = COL_35 * 1000m / (COL_5 * (COL_17 / 100m) * COL_4 * (COL_19 / 1000m));
                        COL_10 = Math.Floor(COL_10);
                        break;
                    case "张":
                        ///主数量COL_10 =数量OL_35 
                        COL_10 = COL_35;
                        break;
                }
            }
            else if (COL_9 == "米")
            {

                switch (COL_59)
                {
                    case "米":
                        ///主数量COL_10 =数量COL_35
                        COL_10 = COL_35;
                        break;
                    case "KG":
                    case "公斤":
                        ///主数量COL_10 =数量COL_35*1000/（密度 X   (厚度/100)  X 宽度  X (长度/1000)） *长度/1000
                        COL_10 = COL_35 * 1000m / (COL_5 * (COL_17 / 100m) * COL_4 * (COL_19 / 1000m)) * COL_19 / 1000m;
                        COL_10 = Math.Floor(COL_10);
                        break;
                    case "张":
                        ///主数量COL_10 = 长度 / 1000 *  数量OL_35
                        COL_10 = COL_19 / 1000m * COL_35;
                        break;
                }
            }

            #endregion



            lm[col_19] = COL_19;

            lm[col_10] = COL_10;



            return new MyJosn("OK", "");
        }

    }
}