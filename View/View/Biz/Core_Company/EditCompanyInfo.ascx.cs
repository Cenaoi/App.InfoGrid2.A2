using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HWQ.Entity.Decipher.LightDecipher;
using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using EasyClick.Web.Mini2;
using EC5.Utility;

namespace App.InfoGrid2.View.Biz.Core_Company
{
    public partial class EditCompanyInfo : WidgetControl, IView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                InitData();
            }
            
    
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData() 
        {
            try
            {

                DbDecipher decipher = ModelAction.OpenDecipher();


                BIZ_C_COMPANY item = decipher.SelectToOneModel<BIZ_C_COMPANY>("ROW_SID >= 0");

                if (item == null)
                {

                    try
                    {

                        item = new BIZ_C_COMPANY()
                        {
                            CODE = "公司代码",
                            ADDRSSS = "公司地址",
                            FULL_NAME = "公司全称",
                            SHORT_NAME = "公司简称",
                            ROW_DATE_CREATE = DateTime.Now,
                            ROW_DATE_UPDATE = DateTime.Now
                        };

                        decipher.InsertModel(item);
                    }
                    catch (Exception ex)
                    {
                        log.Error("创建默认公司信息失败了！", ex);

                        MessageBox.Alert("创建默认公司信息失败了！");

                        return;
                    }
                }

                this.compIdLab.Value = item.BIZ_C_COMPANY_ID.ToString();


                this.tbxCode.Value = item.CODE;
                this.tbxAddrsss.Value = item.ADDRSSS;
                this.tbxBankAccount.Value = item.BANK_ACCOUNT;
                this.tbxBankName.Value = item.BANK_NAME;
                this.tbxCorporationTax.Value = item.CORPORATION_TAX;
                this.tbxEmail.Value = item.EMAIL;
                this.tbxFax.Value = item.FAX;
                this.tbxFullName.Value = item.FULL_NAME;
                this.tbxPortcode.Value = item.PORTCODE;
                this.tbxShortName.Value = item.SHORT_NAME;
                this.tbxTel.Value = item.TEL;
                this.tbxWebsite.Value = item.WEBSITE;



            }
            catch (Exception ex) 
            {
                log.Error(ex);
            }

        }


        /// <summary>
        /// 保存事件
        /// </summary>
        public void btnSave() 
        {           

            try
            {
                BIZ_C_COMPANY company = null;
                
                DbDecipher decipher = ModelAction.OpenDecipher();

                int pk = StringUtil.ToInt32(this.compIdLab.Value);

                company = decipher.SelectModelByPk<BIZ_C_COMPANY>(pk);

                company.SetTakeChange(true);

                company.ADDRSSS = this.tbxAddrsss.Value;
                company.BANK_ACCOUNT = this.tbxBankAccount.Value;
                company.BANK_NAME = this.tbxBankName.Value;
                company.CODE = this.tbxCode.Value;
                company.CORPORATION_TAX = this.tbxCorporationTax.Value;
                company.EMAIL = this.tbxEmail.Value;
                company.FAX = this.tbxFax.Value;
                company.FULL_NAME = this.tbxFullName.Value;
                company.PORTCODE = this.tbxPortcode.Value;
                company.SHORT_NAME = this.tbxShortName.Value;
                company.TEL = this.tbxTel.Value;
                company.WEBSITE = this.tbxWebsite.Value;

                decipher.UpdateModel(company, true);


                Toast.Show("保存成功！");

            }
            catch (Exception ex) 
            {
                log.Error("保存公司信息失败！",ex);

                MessageBox.Alert("保存公司信息失败");
            }

        }

    }
}