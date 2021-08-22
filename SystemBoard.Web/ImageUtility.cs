using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace EC5.SystemBoard.Web
{
    [Obsolete]
    public static class ImageUtility
    {

        /// <summary>
        /// 获取缩略图的大小
        /// </summary>
        /// <param name="smallW">小图宽度</param>
        /// <param name="smallH">小图高度</param>
        /// <param name="srcW">原图宽度</param>
        /// <param name="srcH">原图高度</param>
        /// <returns></returns>
        private static Size GetNewSz(int smallW, int smallH, int srcW, int srcH)
        {
            int newWidth, newHeight;

            if (srcW > smallW || srcH > smallH)
            {
                if (srcW > srcH)
                {
                    newWidth = smallW;
                    newHeight = (int)((double)srcH / (double)srcW * (double)newWidth);
                }
                else
                {
                    newHeight = smallH;
                    newWidth = (int)((double)srcW / (double)srcH * (double)newHeight);
                }
            }
            else
            {
                newHeight = srcH;
                newWidth = srcW;
            }

            return new Size(newWidth, newHeight);
        }

        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="imagePath">原图片路径</param>
        /// <param name="toWidth"></param>
        /// <param name="toHeight"></param>
        /// <returns>缩略图</returns>
        public static Bitmap CreateThumb(string imagePath, int toWidth, int toHeight)
        {
            FileStream ms = File.OpenRead(imagePath);

            Bitmap bmp = CreateThumb(ms, toWidth, toHeight);

            return bmp;
        }

        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="imagePath">原图片路径</param>
        /// <param name="savePath"></param>
        /// <param name="toWidth"></param>
        /// <param name="toHeight"></param>
        /// <returns></returns>
        public static void CreateThumb(string imagePath, string savePath, int toWidth, int toHeight)
        {
            FileStream ms = File.OpenRead(imagePath);

            Bitmap bmp = CreateThumb(ms, toWidth, toHeight);

            bmp.Save(savePath, ImageFormat.Jpeg);

            bmp.Dispose();
        }


        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="imagePath">原图片路径</param>
        /// <param name="toWidth"></param>
        /// <param name="toHeight"></param>
        /// <returns></returns>
        public static void CreateThumb(string imagePath,string savePath, int toWidth, int toHeight,ImageFormat imageFormat)
        {
            FileStream ms = File.OpenRead(imagePath);

            Bitmap bmp = CreateThumb(ms, toWidth, toHeight);

            bmp.Save(savePath, imageFormat);

            bmp.Dispose() ;
        }




        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="bigStream">客户传上来的图片数据流</param>
        /// <param name="toWidth"></param>
        /// <param name="toHeight"></param>
        /// <returns></returns>
        public static Bitmap CreateThumb(Stream bigStream, int toWidth, int toHeight)
        {
            Color backColor = Color.White;  //缩放后的图片颜色

            //Size sz = newSz;
            Bitmap smallImg = new Bitmap(toWidth, toHeight);

            Graphics g = Graphics.FromImage(smallImg);

            Bitmap bigImage = new Bitmap(bigStream);


            int newWidth, newHeight;

            Size nSz = GetNewSz(toWidth, toHeight, bigImage.Width, bigImage.Height);
            newWidth = nSz.Width;
            newHeight = nSz.Height;


            Point ps = Point.Empty;

            ps.X = (toWidth - newWidth) / 2;
            ps.Y = (toHeight - newHeight) / 2;

            g.Clear(backColor);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(bigImage, new Rectangle(ps.X, ps.Y, newWidth, newHeight), new Rectangle(0, 0, bigImage.Width, bigImage.Height), GraphicsUnit.Pixel);

            g.Dispose();

            bigImage.Dispose();

            return smallImg;

        }


        /// <summary>
        /// 保存缩略图
        /// </summary>
        /// <param name="thumbPath"></param>
        /// <param name="bigStream"></param>
        /// <param name="toWidth"></param>
        /// <param name="toHeight"></param>
        public static void CreateThumb(string thumbPath, Stream bigStream, int toWidth, int toHeight)
        {
            Bitmap thumbImg = ImageUtility.CreateThumb(bigStream, toWidth, toHeight);

            string dirPath = Path.GetDirectoryName(thumbPath);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            thumbImg.Save(thumbPath);

            thumbImg.Dispose();
        }



        static string[] m_FileEx = new string[] {".jpg",".png",".gif",".jpeg",".bmp" };

        /// <summary>
        /// 判断是否为图片文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsImage(string filename)
        {
            string fileEx = Path.GetExtension(filename).ToLower();

            bool exist = false;

            for (int i = 0; i < m_FileEx.Length; i++)
            {
                if (m_FileEx[i].Equals(fileEx, StringComparison.Ordinal))
                {
                    exist = true;
                    break;
                }
            }

            return exist;
        }
    }
}
