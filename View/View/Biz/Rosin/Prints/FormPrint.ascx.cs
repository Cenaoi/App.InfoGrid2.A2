using App.BizCommon;
using EC5.IG2.Core;
using EC5.SystemBoard.Interfaces;
using EC5.SystemBoard.Web.UI;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace App.InfoGrid2.View.Biz.Rosin.Prints
{
    public partial class FormPrint : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 用户信息
        /// </summary>
        public EC5.SystemBoard.EcUserState EcUser
        {
            get { return EC5.SystemBoard.EcContext.Current.User; }
        }


        /// <summary>
        /// 是否为设计师
        /// </summary>
        /// <returns></returns>
        public bool IsBuilder()
        {
            return this.EcUser.Roles.Exist(IG2Param.Role.BUILDER);
        }

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = IG2Param.PageMode.MInJs2;

            base.OnInit(e);

        }

        /// <summary>
        /// 获取单头
        /// </summary>
        /// <returns></returns>
        public LModel GetBill()
        {
            int id = WebUtil.QueryInt("id");

            DbDecipher decipher = ModelAction.OpenDecipher();


            LModel model001 = decipher.GetModelByPk("UT_001", id);

            return model001;

        }

        /// <summary>
        /// 获取货物明细
        /// </summary>
        /// <returns></returns>
        public LModelList<LModel> GetProds()
        {
            int row_id = WebUtil.QueryInt("row_id");

            string pMode = WebUtil.QueryLower("p_mode");

            int startI = WebUtil.QueryInt("p_start") - 1;
            int endI = WebUtil.QueryInt("p_end") - 1;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter("UT_017_PROD");
            filter.AddFilter("ROW_SID >=0");
            filter.And("DOC_PARENT_ID", row_id);
            filter.And("IO_TAG", "I");

            if (pMode != "all")
            {
                filter.Limit = new Limit(endI - startI + 1, startI);
            }

            LModelList<LModel> model001 = decipher.GetModelList(filter);

            return model001;

        }

        /// <summary>
        /// 获取条码的图片地址
        /// </summary>
        /// <param name="billCode"></param>
        /// <param name="prod"></param>
        /// <returns></returns>
        public  string GetCode1(string billCode, LModel prod)
        {


            Image image;

            byte[] buffer = GetBarcode(80, 400, BarcodeLib.TYPE.CODE128, billCode, out image);

            if(buffer == null)
            {
                return string.Empty;
            }

            string file = "/_Temporary/C2_Code128/" + billCode + ".jpg";

            string aPath = MapPath(file);   //物理路径

            FileUtil.WriteAllBytes(aPath, buffer);
                       

            return file;
        }



        //生成一维码图片  
        private byte[] GetBarcode(int height, int width, BarcodeLib.TYPE type,
                                           string code, out System.Drawing.Image image)
        {
            image = null;
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            b.BackColor = System.Drawing.Color.White;
            b.ForeColor = System.Drawing.Color.Black;
            b.IncludeLabel = true;

            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
            b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
            b.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
            System.Drawing.Font font = new System.Drawing.Font("verdana", 10f);
            b.LabelFont = font;

            b.Height = height;
            b.Width = width;

            try
            {
                image = b.Encode(type, code);
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                image = null;
            }
            //SaveImage(image, Guid.NewGuid().ToString("N") + ".png");  
            byte[] buffer = b.GetImageData(BarcodeLib.SaveTypes.GIF);
            return buffer;
        }

    }
}