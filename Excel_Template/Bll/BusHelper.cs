using EC5.Utility;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace App.InfoGrid2.Excel_Template.Bll
{
    /// <summary>
    /// 这是业务助手
    /// </summary>
    public class BusHelper
    {


        /// <summary>
        /// 解析图片字段
        /// </summary>
        /// <param name="field_value">图片字段里面的值</param>
        /// <returns></returns>
        public static SModelList ParseImgField(string field_value)
        {
            SModelList sm_imgs = new SModelList();

            if (string.IsNullOrWhiteSpace(field_value))
            {

                return sm_imgs;

            }

            string[] imgs = StringUtil.Split(field_value, "\n");

            foreach (string img in imgs)
            {
                if (string.IsNullOrEmpty(img))
                {
                    continue;
                }

                string[] pro = StringUtil.Split(img, "||");
                SModel sm = new SModel();
                sm["url"] = pro[0];
                sm["thumb_url"] = pro[1];
                sm["name"] = pro[2];
                sm["code"] = pro[3];
                sm_imgs.Add(sm);
            }

            return sm_imgs;

        }


        /// <summary>
        /// 通过FileStream 来打开文件，这样就可以实现不锁定Image文件，到时可以让多用户同时访问Image文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Bitmap ReadImageFile(string path)
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            Image result = Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }

    }
}
