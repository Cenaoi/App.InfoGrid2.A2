using App.BizCommon;
using App.InfoGrid2.Model.WeChat;
using App.InfoGrid2.Wxhb.Bll;
using EC5.IO;
using EC5.SystemBoard;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace App.InfoGrid2.Wxhb.Handlers
{
    /// <summary>
    /// �̻���صĴ�����
    /// </summary>
    public class BusHandler : IHttpHandler,IRequiresSessionState
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";


            HttpResult result = null;



            EcUserState userState = EcContext.Current.User;

            if (!userState.Roles.Exist("WX"))
            {
                context.Response.Write(HttpResult.Error("��������д���ˣ�"));
                return;
            }


            try
            {
                string action = WebUtil.FormTrimUpper("action");


                switch (action)
                {

                    case "BUS_RECHARGE":
                       result =  BusRecharge();
                        break;
                    case "BUS_REGISTER":
                       result = BusRegister();
                        break;
                    case "NEW_ADVE":
                        result = NewAdve();
                        break;
                    case "GET_ADVE":
                        result = GetAdve();
                        break;
                    case "GET_ADVE_BY_CODE":
                        result = GetAdveByCode();
                        break;
                    case "SAVE_ADVE":
                        result = SaveAdve();
                        break;
                    case "ADVE_SUBMIT":
                        result = AdveSubmit();
                        break;
                    case "GET_ADVES_BY_USER_ID":
                        result = GetAdvesByUserId();
                        break;
                    case "DELETE_ADVE":
                        result = DeleteAdve();
                        break;
                    case "GET_HB":
                        result = GetHb();
                        break;
                    case "RECEIVE_HB":
                        result = ReceiveHb();
                        break;
                    case "BROWSE_ADVE":
                        result = BrowseAdve();
                        break;
                    case "DELETE_HB_ADDRESS":
                        result = DeleteHbAddress();
                        break;
                    
                    default:
                        result = HttpResult.Error("Ŷ�ޣ�д���ˣ�");
                        break;
                   
                }

            }catch(Exception ex)
            {

                log.Error(ex);

                result = HttpResult.Error(ex.Message);


            }


            context.Response.Write(result);

        }


        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <returns></returns>
        HttpResult ReceiveHb()
        {


            int adve_id = WebUtil.FormInt("adve_id");

            int hb_id = WebUtil.FormInt("hb_id");

            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_ACCOUNT wx_account = decipher.SelectModelByPk<WX_ACCOUNT>(userState.Identity);

            if (wx_account == null)
            {
                throw new Exception("�Ҳ����û����ݣ�");
            }


            


            LModel lm002 = decipher.GetModelByPk("UT_002", adve_id);

            if(lm002.Get<string>("FK_W_CODE") == wx_account.PK_W_CODE)
            {

                return HttpResult.Error("�����Լ����ĺ�����Լ�����������");

            }

            var create_date = DateUtil.StartDay(DateUtil.ToDateString(DateTime.Now));

            LightModelFilter lmFilter005 = new LightModelFilter("UT_005");
            lmFilter005.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter005.And("FK_RE_W_CODE", wx_account.PK_W_CODE);
            lmFilter005.And("FK_ADVE_CODE", lm002["PK_ADVE_CODE"]);
            lmFilter005.And("ROW_DATE_CREATE", create_date, Logic.GreaterThanOrEqual);

            //�������жϽ��쵱ǰ΢���û��Ƿ��Ѿ���ȡ��������ĺ����
            bool falg = decipher.ExistsModels(lmFilter005);

            if (falg)
            {
                return HttpResult.Error("�����Ѿ���ȡ���ˣ�����������");
            }

            
            LightModelFilter lmFilter = new LightModelFilter("UT_003");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("ROW_IDENTITY_ID", hb_id);
            lmFilter.And("FK_ADVE_CODE", lm002["PK_ADVE_CODE"]);

            LModel lm003 = decipher.GetModel(lmFilter);

            //������
            decimal hb_money = lm003.Get<decimal>("HB_MONEY");



            LModel lm005 = new LModel("UT_005");
            lm005["FK_ADVE_CODE"] = lm003["FK_ADVE_CODE"];
            lm005["FK_RE_W_CODE"] = wx_account.PK_W_CODE;
            lm005["FK_HB_CODE"] = lm003["PK_HB_CODE"];
            lm005["FK_CREATE_W_CODE"] = lm003["FK_WX_CODE"];
            lm005["HB_MONEY"] = hb_money;
            lm005["HB_TYPE"] = lm003["HB_TYPE"];
            lm005["HB_LEVEL"] = lm003["HB_LEVEL"];
            lm005["PK_RE_HB_CODE"] = BillIdentityMgr.NewCodeForDay("RE_HB_CODE", "R", 6);

            decipher.BeginTransaction();

            try
            {

                decipher.InsertModel(lm005);

                wx_account.SetTakeChange(true);

                wx_account.COL_2 += hb_money;
                wx_account.COL_1++;
                wx_account.COL_13 += hb_money;
                wx_account.COL_16 += hb_money;

                wx_account.ROW_DATE_UPDATE = DateTime.Now;

                decipher.UpdateModel(wx_account, true);


                decipher.DeleteModel(lm003);

                decipher.TransactionCommit();

                return HttpResult.Success(lm003);

            }catch(Exception ex)
            {
                decipher.TransactionRollback();

                log.Error(ex);
                throw ex;

            }

        }


        /// <summary>
        /// ��ȡ���
        /// </summary>
        /// <returns></returns>
        HttpResult GetHb()
        {

            int adve_id = WebUtil.FormInt("adve_id");


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm002 = decipher.GetModelByPk("UT_002", adve_id);

            LightModelFilter lmFilter = new LightModelFilter("UT_003");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_ADVE_CODE", lm002["PK_ADVE_CODE"]);

            lmFilter.Top = 1;

            LModel lm003 = decipher.GetModel(lmFilter);

            if (lm003 == null)
            {
                return HttpResult.Error("�����ȡ���ˣ�");
            }


            return HttpResult.Success(new { id= lm003.GetPk()});

        }


        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <returns></returns>
        HttpResult DeleteAdve()
        {

            int adve_id = WebUtil.FormInt("adve_id");


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm002 = decipher.GetModelByPk("UT_002", adve_id);


            LightModelFilter lmFilter = new LightModelFilter("UT_003");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_ADVE_CODE", lm002["PK_ADVE_CODE"]);

            bool flag = decipher.ExistsModels(lmFilter);

            if (flag)
            {

                return HttpResult.Error("�����û�������꣬����ɾ����棡");

            }




            return HttpResult.SuccessMsg("Ŷ�ޣ�����ɾ����ร�");

        }

        HttpResult GetAdvesByUserId()
        {
            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_ACCOUNT wx_account = decipher.SelectModelByPk<WX_ACCOUNT>(userState.Identity);

            if (wx_account == null)
            {
                throw new Exception("�Ҳ����û����ݣ�");
            }


            LightModelFilter lmFilter = new LightModelFilter("UT_002");
            lmFilter.And("ROW_SID", 0,Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_SID", 2, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_W_CODE", wx_account.PK_W_CODE);
            lmFilter.TSqlOrderBy = "ROW_DATE_CREATE desc";

            List<LModel> lm002s = decipher.GetModelList(lmFilter);

            SModelList sm_data = new SModelList();

            foreach(LModel lm002 in lm002s)
            {

                SModel sm = new SModel();

                lm002.CopyTo(sm);

                sm_data.Add(sm);

            }

            return HttpResult.Success(sm_data);
            

        }

        /// <summary>
        /// ����ύ�¼�
        /// </summary>
        /// <returns></returns>
        HttpResult AdveSubmit()
        {


            decimal re_money = WebUtil.FormDecimal("re_money");

            int adve_id = WebUtil.FormInt("adve_id");

            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_ACCOUNT wx_account = decipher.SelectModelByPk<WX_ACCOUNT>(userState.Identity);

            if (wx_account == null)
            {
                throw new Exception("�Ҳ����û����ݣ�");
            }

            if (re_money < 1)
            {

                return HttpResult.Error("Ԥ�����С��1Ԫ");

            }


            LModel lm002 = decipher.GetModelByPk("UT_002", adve_id);

            //�������
            int col_10 = lm002.Get<int>("COL_10");

            if(col_10 < 1)
            {
                return HttpResult.Error("�����������С��1��");
            }


            //�����߽��
            decimal col_11 = lm002.Get<decimal>("COL_11");

            if(col_11 < 0.1m)
            {
                return HttpResult.Error("���������С��0.1Ԫ��");

            }


            decimal hb_total_money = col_10 * lm002.Get<decimal>("COL_11");

            if(hb_total_money > re_money)
            {
                return HttpResult.Error("���������Ԥ���ˣ�");
            }

            List<LModel> lm003s = new List<LModel>();

            for (int i = 0; i < col_10; i++)
            {
                LModel lm003 = new LModel("UT_003");
                lm003["PK_HB_CODE"] = BillIdentityMgr.NewCodeForDay("HB_CODE", "H", 6);
                lm003["FK_ADVE_CODE"] = lm002["PK_ADVE_CODE"];
                lm003["FK_WX_CODE"] = wx_account.PK_W_CODE;
                lm003["W_OPENID"] = wx_account.W_OPENID;
                lm003["HB_MONEY"] = col_11;

                lm003s.Add(lm003);

            }



            LightModelFilter lmFilter010 = new LightModelFilter("UT_010");
            lmFilter010.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter010.And("FK_ADVE_CODE", lm002["PK_ADVE_CODE"]);

            //�Ѻ����ַ��ҵ��״̬Ҳ��Ϊ2
            decipher.UpdateProps(lmFilter010, new object[] { "BIZ_SID",2, "ROW_DATE_UPDATE", DateTime.Now});


            LModel lm004 = new LModel("UT_004");
            lm004["PK_BG_CODE"] = BillIdentityMgr.NewCodeForDay("BG_CODE", "B", 6);
            lm004["FK_ADVE_CODE"] = lm002["PK_ADVE_CODE"];
            lm004["FK_W_CODE"] = wx_account.PK_W_CODE;
            lm004["HB_NUM"] = col_10;
            lm004["HB_MAX_MONEY"] = col_11;
            lm004["BUDGET_MONEY"] = re_money;

            decipher.BeginTransaction();

            try
            {

                decipher.InsertModel(lm004);



                decipher.InsertModels(lm003s);

                lm002["MONEY_TOTAL"] = lm002.Get<decimal>("MONEY_TOTAL") + hb_total_money;
                lm002["MONEY_SURPLUS"] = lm002.Get<decimal>("MONEY_TOTAL") - lm002.Get<decimal>("MONEY_CONSUME");
                lm002["HB_SURPLUS"] = lm003s.Count();
                lm002["ROW_DATE_UPDATE"] = DateTime.Now;
                lm002["BIZ_SID"] = 2;

                decipher.UpdateModelProps(lm002, "MONEY_TOTAL", "MONEY_SURPLUS", "HB_SURPLUS", "ROW_DATE_UPDATE", "BIZ_SID");

                decipher.TransactionCommit();

                return HttpResult.SuccessMsg("���ɺ���ɹ��ˣ�");

            }
            catch (Exception ex)
            {
                decipher.TransactionRollback();
                throw ex;
            }
        }


        /// <summary>
        /// ����һ����� ���֮ǰ��δ��ɵľ���֮ǰ��
        /// </summary>
        /// <returns></returns>
        HttpResult NewAdve()
        {

            //if (!IsNewAdve())
            //{

            //    return HttpResult.Error("���㣬���ֵ��");


            //}


            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_ACCOUNT wx_account = decipher.SelectModelByPk<WX_ACCOUNT>(userState.Identity);

            if (wx_account == null)
            {
                throw new Exception("�Ҳ����û����ݣ�");
            }


            LightModelFilter lmFilter = new LightModelFilter("UT_002");
            lmFilter.And("FK_W_CODE", wx_account.PK_W_CODE);
            lmFilter.And("BIZ_SID", 0);
            lmFilter.TSqlOrderBy = "ROW_DATE_CREATE desc";
            lmFilter.Top = 1;

            LModel lm002 = decipher.GetModel(lmFilter);

            if(lm002 == null)
            {
                lm002 = new LModel("UT_002");

                lm002["W_OPENID"] = wx_account.W_OPENID;
                lm002["FK_W_CODE"] = wx_account.PK_W_CODE;
                lm002["COL_4"] = wx_account.COL_24;
                lm002["COL_5"] = wx_account.COL_25;
                lm002["COL_6"] = wx_account.COL_27;
                lm002["COL_7"] = wx_account.COL_27;
                lm002["PK_ADVE_CODE"] = BillIdentityMgr.NewCodeForDay("ADVE_CODE", "A", 6);
                lm002["ROW_DATE_CREATE"] = lm002["ROW_DATE_UPDATE"] = DateTime.Now;

                decipher.InsertModel(lm002);

                //Ĭ������һ�������ַ
                LModel lm010 = new LModel("UT_010");
                lm010["PK_A_HB_CODE"] = BillIdentityMgr.NewCodeForDay("A_HB_CODE", "A", 6);
                lm010["FK_ADVE_CODE"] = lm002["PK_ADVE_CODE"];
                lm010["FK_W_CODE"] = wx_account.PK_W_CODE;
                lm010["W_NICKNAME"] = wx_account.W_NICKNAME;
                lm010["HB_LONGITUDE"] = wx_account.COL_24;
                lm010["HB_LATITUDE"] = wx_account.COL_25;
                lm010["HB_PRECISION"] = wx_account.COL_26;
                lm010["HB_ADDRESS"] = wx_account.COL_27;

                decipher.InsertModel(lm010);
                  


            }

            return HttpResult.Success(new { adve_id = lm002["ROW_IDENTITY_ID"] });

        }

        /// <summary>
        /// ����ID��ȡ�������
        /// </summary>
        /// <returns></returns>
        HttpResult GetAdve()
        {

            int adve_id = WebUtil.FormInt("adve_id");


            DbDecipher decipher = ModelAction.OpenDecipher();

            LModel lm002 = decipher.GetModelByPk("UT_002", adve_id);

            SModel sm_result = new SModel();


            lm002.CopyTo(sm_result);

            SModel imgs = new SModel();

            imgs["data"] = BusHelper.GetFilesByField(sm_result.Get<string>("COL_3"));
            imgs["server_url"] = "/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_002&tag_code=adve_img&row_id=" + lm002.GetPk();
            imgs["delete_img_url"] = "/Wxhb/Handlers/UploaderFileHandle.ashx";
            imgs["row_id"] = lm002.GetPk();
            imgs["table_name"] = "UT_002";
            imgs["tag_code"] = "adve_img";
            imgs["btn_id"] = "uploader_img_" + lm002.GetPk();
            imgs["field_text"] = "COL_3";

            sm_result["imgs"] = imgs;


            //��ȡ��������������к����ַ
            LightModelFilter lmFilter = new LightModelFilter("UT_010");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_ADVE_CODE", lm002["PK_ADVE_CODE"]);

            sm_result["hb_addr_list"] = decipher.GetSModelList(lmFilter); 

           
            return HttpResult.Success(sm_result);


        }

        /// <summary>
        /// �����Զ������������ȡ�������
        /// </summary>
        /// <returns></returns>
        HttpResult GetAdveByCode()
        {

            string adve_code = WebUtil.FormTrim("adve_code");


            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter002 = new LightModelFilter("UT_002");
            lmFilter002.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter002.And("PK_ADVE_CODE", adve_code);
            lmFilter002.And("FK_W_CODE",userState.LoginID);

            LModel lm002 = decipher.GetModel(lmFilter002);

            SModel sm_result = new SModel();

            lm002.CopyTo(sm_result);

            SModel imgs = new SModel();

            imgs["data"] = BusHelper.GetFilesByField(sm_result.Get<string>("COL_3"));
            imgs["server_url"] = "/View/OneForm/FormHandle.aspx?method=IMAGE_UPLOAD&table_name=UT_002&tag_code=adve_img&row_id=" + lm002.GetPk();
            imgs["delete_img_url"] = "/Wxhb/Handlers/UploaderFileHandle.ashx";
            imgs["row_id"] = lm002.GetPk();
            imgs["table_name"] = "UT_002";
            imgs["tag_code"] = "adve_img";
            imgs["btn_id"] = "uploader_img_" + lm002.GetPk();
            imgs["field_text"] = "COL_3";

            sm_result["imgs"] = imgs;


            //��ȡ��������������к����ַ
            LightModelFilter lmFilter = new LightModelFilter("UT_010");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_ADVE_CODE", lm002["PK_ADVE_CODE"]);

            sm_result["hb_addr_list"] = decipher.GetSModelList(lmFilter);


            return HttpResult.Success(sm_result);


        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        HttpResult SaveAdve()
        {

            string reim_deta_json_str = WebUtil.Form("reim_deta_json_str");

            string change_field_str = WebUtil.Form("change_files_str");

            string table_name = WebUtil.FormTrim("table_name");

            if (string.IsNullOrWhiteSpace(table_name))
            {
                return HttpResult.Error("����û�б�����");
            }

            if (string.IsNullOrWhiteSpace(change_field_str))
            {
                return HttpResult.SuccessMsg("û�иı��ֶΣ����Բ��ñ��棡");
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            int cur_id = 0;


            SModel sm = BusHelper.ParseSModel(reim_deta_json_str, change_field_str, table_name, "ROW_IDENTITY_ID", out cur_id);

            LModel lm = decipher.GetModelByPk(table_name, cur_id);

            lm.SetTakeChange(true);

            //foreach (var s in sm.GetFields())
            //{
            //    lm[s] = sm[s];

            //}

            lm.TrySetValues(sm);    //����������

            decipher.UpdateModel(lm, true);

            return HttpResult.SuccessMsg("�������ݳɹ��ˣ�");
        }

        /// <summary>
        /// �̼�ע�ắ��
        /// </summary>
        /// <returns></returns>
        HttpResult BusRegister()
        {

            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_ACCOUNT wx_account = decipher.SelectModelByPk<WX_ACCOUNT>(userState.Identity);

            if (wx_account == null)
            {
                throw new Exception("�Ҳ����û����ݣ�");
            }


            string data = WebUtil.FormTrim("data");

            SModel sm = SModel.ParseJson(data);

            wx_account.SetTakeChange(true);

            wx_account.COL_20 = sm["col_20"];
            wx_account.COL_23 = sm["col_23"];
            wx_account.COL_24 = sm.Get<decimal>("col_24");
            wx_account.COL_25 = sm.Get<decimal>("col_25");
            wx_account.COL_27 = sm["col_27"];
            wx_account.COL_19 = true;
            wx_account.ROW_DATE_UPDATE = DateTime.Now;

            decipher.UpdateModel(wx_account, true);

            return HttpResult.SuccessMsg("ע��ɹ��ˣ�");

        }

        /// <summary>
        /// �̻���ֵ����
        /// </summary>
        /// <returns></returns>
        HttpResult BusRecharge()
        {
            EcUserState userState = EcContext.Current.User;

            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_ACCOUNT wx_account = decipher.SelectModelByPk<WX_ACCOUNT>(userState.Identity);

            if (wx_account == null)
            {
                throw new Exception("�Ҳ����û����ݣ�");
            }

            int bus_recharge = WebUtil.FormInt("bus_recharge");

            if(bus_recharge < 1)
            {

                return HttpResult.Error("��ֵ����С��1Ԫ��");
            }

            string body = $"��ֵ{bus_recharge}Ԫ";

            string openid = wx_account.W_OPENID;

            var url = GlobelParam.GetValue<string>("WX_MENU_API", "http://yq.gzbeih.com/API", "΢�Ź���API��ַ");

            var key = GlobelParam.GetValue<string>("WX_MENU_API_KEY", "WXHB", "��΢�Ź���API�еĹؼ���");

            var order_no = BillIdentityMgr.NewCodeForDay("ORDER_NO", "O", 6);

            LModel lm001 = new LModel("UT_001");
            lm001["COL_3"] = order_no;
            lm001["COL_1"] = wx_account.W_NICKNAME;
            lm001["COL_2"] = wx_account.W_OPENID;
            lm001["COL_4"] = bus_recharge;
            lm001["COL_6"] = body;

            lm001["ROW_DATE_CREATE"] = lm001["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.InsertModel(lm001);

            var notify_url = HttpContext.Current.Server.UrlEncode("http://wxhb.gzbeih.com/Wxhb/Handlers/WxNotifyHandler.ashx");

            NameValueCollection nv = new NameValueCollection();
            nv["key"] = key;
            nv["body"] = body;
            nv["order_no"] = order_no;
            nv["fee"] = bus_recharge.ToString();
            nv["openid"] = wx_account.W_OPENID;
            nv["notify_url"] = notify_url;


            string result = string.Empty;

            using (WebClient wc = new WebClient())
            {

                byte[] json = wc.UploadValues(url + "/Pay.ashx", "POST", nv);

                result = Encoding.UTF8.GetString(json);

            }

            HttpResult sm_result = HttpResult.ParseJson(result);

            return sm_result;

        }

        /// <summary>
        /// �Ǽ�����������
        /// </summary>
        /// <returns></returns>
        HttpResult BrowseAdve()
        {

            int adve_id = WebUtil.FormInt("adve_id");

            DbDecipher decipher = ModelAction.OpenDecipher();

            EcUserState userState = EcContext.Current.User;

            LModel lm002 = decipher.GetModelByPk("UT_002", adve_id);

            lm002["BROWSE_NUM"] = lm002.Get<int>("BROWSE_NUM") + 1;
            lm002["ROW_DATE_UPDATE"] = DateTime.Now;

            decipher.UpdateModelProps(lm002, "BROWSE_NUM", "ROW_DATE_UPDATE");



            LightModelFilter lmFilter = new LightModelFilter("UT_009");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("FK_ADVE_CODE", lm002["PK_ADVE_CODE"]);
            lmFilter.And("FK_W_CODE", userState.LoginID);

            LModel lm009 = decipher.GetModel(lmFilter);




            if(lm009 != null)
            {
                
                return HttpResult.SuccessMsg("�Ѿ������������¼��");

            }



            lm009 = new LModel("UT_009");
            lm009["ROW_DATE_CREATE"] = lm009["ROW_DATE_UPDATE"] = DateTime.Now;
            lm009["PK_BROWSE_CODE"] = BillIdentityMgr.NewCodeForDay("BROWSE_CODE", "B", 6);
            lm009["FK_W_CODE"] = userState.LoginID;
            lm009["ADVE_TITLE"] = lm002["COL_1"];
            lm009["ADVE_CONTENT"] = lm002["COL_2"];
            lm009["FK_ADVE_CODE"] = lm002["PK_ADVE_CODE"];

            decipher.InsertModel(lm009);


            return HttpResult.SuccessMsg("����һ�������¼�ɹ���");

        }

        /// <summary>
        /// ɾ���������ĺ����ַ
        /// </summary>
        /// <returns></returns>
        HttpResult DeleteHbAddress()
        {

            int id = WebUtil.FormInt("id");

            string adve_code = WebUtil.FormTrim("adve_code");

            DbDecipher decipher = ModelAction.OpenDecipher();



            LightModelFilter lmFilterNum = new LightModelFilter("UT_010");
            lmFilterNum.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilterNum.And("FK_ADVE_CODE", adve_code);

            int num = decipher.SelectCount(lmFilterNum);

            if(num < 2)
            {

                return HttpResult.Error("ֻ��һ�������ַ�ǲ���ɾ���ģ�");
            }

            LightModelFilter lmFilter = new LightModelFilter("UT_010");
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("ROW_IDENTITY_ID", id);
            lmFilter.And("FK_ADVE_CODE", adve_code);

            LModel lm010 = decipher.GetModel(lmFilter);

            if (lm010 == null)
            {
                return HttpResult.Error("Ŷ�ޣ�������ร�");
            }

            lm010["ROW_SID"] = -3;
            lm010["ROW_DATE_DELETE"] = DateTime.Now;

            decipher.UpdateModelProps(lm010, "ROW_SID", "ROW_DATE_DELETE");

            return HttpResult.SuccessMsg("ɾ���ɹ��ˣ�");


        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}