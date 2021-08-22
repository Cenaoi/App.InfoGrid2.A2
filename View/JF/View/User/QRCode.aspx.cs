using App.BizCommon;
using App.InfoGrid2.JF.Bll;
using App.InfoGrid2.Model.JF;
using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;
using Gma.QrCodeNet.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing;
using System.Net;
using EC5.Utility;
using System.Drawing.Imaging;

namespace App.InfoGrid2.JF.View.User
{
    public partial class QRCode : System.Web.UI.Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!BusHelper.AutoLogin())
                {

                    Response.Redirect("/JF/WeChat/Index.ashx");

                }

            }
        }

        /// <summary>
        /// 获取到专属二维码图片地址
        /// </summary>
        /// <returns></returns>
        public string QrCodeImgUrl()
        {

            EcUserState user = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            ES_W_ACCOUNT account = decipher.SelectModelByPk<ES_W_ACCOUNT>(user.Identity);


            string qr_code_url = account.QR_CODE_IMG_URL;


            if (string.IsNullOrWhiteSpace(qr_code_url))
            {

                qr_code_url = CreateQrCodeImg(account);

            }


            string file_path = Server.MapPath(qr_code_url);

            if (!File.Exists(file_path))
            {
                qr_code_url = CreateQrCodeImg(account);
            }


            account.QR_CODE_IMG_URL = qr_code_url;
            account.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModelProps(account, "QR_CODE_IMG_URL", "ROW_DATE_UPDATE");


            return qr_code_url;


        }

        /// <summary>
        /// 创建专属二维码图片
        /// </summary>
        /// <param name="account">用户对象</param>
        /// <returns>专属二维码地址</returns>
        string CreateQrCodeImg(ES_W_ACCOUNT account)
        {



            string qrCodeText = "http://wshop.gzbeih.com/JF/WeChat/index.ashx?parent_code=" + account.PK_W_CODE;


            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode = qrEncoder.Encode(qrCodeText);

            //文件名称
            string file_name = DateTime.Now.ToString("yyMMddHHmmssfff")+ "-" + account.W_NICKNAME + ".png";

            ///保存二维码图片路径
            string mapath = Server.MapPath("/_QrCode/");

            ///判断文件夹是否存在
            if (!Directory.Exists(mapath))
            {
                Directory.CreateDirectory(mapath);
            }

            string qrCodeImgPath = mapath + "\\" + file_name;


            GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);

            DrawingSize dSize = render.SizeCalculator.GetSize(qrCode.Matrix.Width);
            Bitmap map = new Bitmap(dSize.CodeWidth, dSize.CodeWidth);
            Graphics g = Graphics.FromImage(map);
            render.Draw(g, qrCode.Matrix);

            WebClient wc = null;
            try
            {
                wc = new WebClient();

                byte[] bytes = wc.DownloadData(account.HEAD_IMG_URL);


                //创建一个 46 * 46 的缩略图
                Bitmap img = ImageUtil.CreateThumb(new MemoryStream(bytes), 46, 46);

                log.Debug($"map.Width = {map.Width}  img.Width = {img.Width}  map.Height = {map.Height}  img.Height = {img.Height}");


                Point imgPoint = new Point((map.Width - img.Width) / 2, (map.Height - img.Height) / 2);
                g.DrawImage(img, imgPoint.X, imgPoint.Y, img.Width, img.Height);
                map.Save(qrCodeImgPath, ImageFormat.Png);

                return "/_QrCode/" + file_name;


            }
            catch (Exception ex)
            {
                log.Error("读取图片时出错了！", ex);
                throw ex;
            }
            finally
            {
                wc.Dispose();
            }


        }






    }
}