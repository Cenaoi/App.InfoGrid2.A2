using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.Utility;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using EC5.SystemBoard;
using App.InfoGrid2.Model.SecModels;
using App.InfoGrid2.Bll;
using EC5.Entity.Expanding.ExpandV1;
using App.InfoGrid2.Model.WeChat;
using App.InfoGrid2.Model.FlowModels;
using App.InfoGrid2.View;

namespace App.InfoGrid2.WF.Bll
{
    /// <summary>
    /// 业务助手类
    /// </summary>
    public class BusHelper
    {
        /// <summary>
        /// 获取附件信息
        /// </summary>
        /// <param name="table_name">表名</param>
        /// <param name="table_id">IG2_TABLE里面的ID</param>
        /// <param name="row_id">行ID</param>
        /// <param name="tag_code">标记编码</param>
        /// <returns></returns>
        public static SModelList GetFiles(string table_name,int table_id,int row_id,string tag_code)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();


            LightModelFilter lmFilter = new LightModelFilter(typeof(BIZ_FILE));
            lmFilter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("TABLE_NAME", table_name);
            lmFilter.And("TABLE_ID", table_id);
            lmFilter.And("ROW_ID", row_id);
            lmFilter.And("TAG_CODE", tag_code);

            List<BIZ_FILE> imgs = decipher.SelectModels<BIZ_FILE>(lmFilter);

            SModelList sm_imgs = new SModelList();

            foreach (var item in imgs)
            {

                SModel data = new SModel();

                item.CopyTo(data);

                data["FILE_SIZE_STR"] = NumberUtil.FormatFileSize(item.FILE_SIZE);

                string fex = item.FILE_EX.TrimStart('.');

                string path = $"/res/file_icon_256/{fex}.png";

                if (WebFile.Exists(path))
                {
                    data["EX_PATH"] = path;
                }
                else
                {
                    data["EX_PATH"] = "/res/file_icon_256/undefined.png";
                }

                sm_imgs.Add(data);


            }


            return sm_imgs;


        } 

        /// <summary>
        /// 从字段中获取文件集合数据
        /// </summary>
        /// <returns></returns>
        public static SModelList GetFilesByField(string field_value)
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
                SModel sm = new SModel
                {
                    ["url"] = pro[0],
                    ["thumb_url"] = pro[1],
                    ["name"] = pro[2],
                    ["code"] = pro[3]
                };
                sm_imgs.Add(sm);
            }

            return sm_imgs;
            
        }

        /// <summary>
        /// 解析传上来的附件json数据成一个字符串
        /// </summary>
        /// <returns></returns>
        public static string ParseFiles(string file_json)
        {

            SModelList sms = SModelList.ParseJson(file_json);


            StringBuilder sb = new StringBuilder();


            foreach(var sm in sms)
            {
                string url = sm.Get<string>("url");

                string name = sm.Get<string>("name");

                string thumb_url = sm.Get<string>("thumb_url");

                string code = sm.Get<string>("code");


                sb.Append($"{url}||{thumb_url}||{name}||{code}\n");

            }


            return sb.ToString();


        }


        /// <summary>
        /// 格式化时间 给界面上用的
        /// </summary>
        /// <param name="date_str">事件字符串</param>
        /// <param name="format_text">时间格式化样式</param>
        /// <returns></returns>
        public static string FormatDate(string date_str,string format_text = "yyyy-MM-dd")
        {

            if (string.IsNullOrWhiteSpace(date_str))
            {
                return "";
            }


            DateTime date;

            if(!DateTime.TryParse(date_str,out date))
            {

                return "";

            }


            return date.ToString(format_text);

        }


        /// <summary>
        /// 创建流程模板
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="id">行ID</param>
        /// <param name="flowMgr">流程操作对象</param>
        /// <param name="view_code">界面编码</param>
        public static void CreateFlowTemp(string table,int id, FlowOperateMgr flowMgr,string view_code)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();


            LModel lm = decipher.GetModelByPk(table, id);

            BizFilter filter = new BizFilter(typeof(FLOW_INST));
            filter.And("INST_CODE", lm["BIZ_FLOW_INST_CODE"]);


            FLOW_INST f_inst = decipher.SelectToOneModel<FLOW_INST>(filter);


            string fist_value = $"[{f_inst.START_USER_TEXT}][{f_inst.EXTEND_BILL_CODE}][{f_inst.EXTEND_BILL_TYPE}]审核结果";


            StringBuilder sb = new StringBuilder();

            for (int i = 1; i < 5; i++)
            {
                if (!StringUtil.IsBlank(f_inst["EXT_COL_VALUE_" + i]))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("；");
                    }

                    sb.Append($"{f_inst["EXT_COL_TEXT_" + i]}:{f_inst["EXT_COL_VALUE_" + i]}");
                }

            }


            string title_value = $"{f_inst.EXTEND_BILL_CODE},[{sb.ToString()}]";

            //这是生成扫送模板的
            CreateTempData(flowMgr.CopyToPartys, id, view_code, fist_value, f_inst.EXTEND_BILL_TYPE, title_value, "点击进入查看单据明细和审核过程");

            string fist_value_1 = $"您有[{f_inst.START_USER_TEXT}][{f_inst.EXTEND_BILL_TYPE}]需审批";

            string keyword3 = flowMgr.MaxTagID == 0 ? "正常" : "加急";

            //这是生成审批模板的
            CreateTemp2(flowMgr.SubmitPartys, id, view_code, fist_value_1, f_inst.EXTEND_BILL_CODE, f_inst.EXTEND_BILL_TYPE, keyword3, $"内容消息：{sb.ToString()},请尽快登录平台进行处理");


        }


        /// <summary>
        /// 创建扫送微信模板数据
        /// </summary>
        /// <param name="codes">所有抄送用户编码</param>
        /// <param name="id">行ID</param>
        /// <param name="view_code">界面编码</param>
        /// <param name="first_value">模板第一参数</param>
        /// <param name="type_value">模板发布类型</param>
        /// <param name="title_value">模板发布时间</param>
        /// <param name="remark_value">模板备注</param>
        public static void CreateTempData(List<string> codes,int id,string view_code,string first_value, string type_value, string title_value, string remark_value)
        {

            if(codes == null || codes.Count == 0)
            {
                return;
            }


            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(SEC_LOGIN_ACCOUNT));
            lmFilter.And("ROW_STATUS_ID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_USER_CODE", codes.ToArray(), Logic.In);

            List<SEC_LOGIN_ACCOUNT> accounts = decipher.SelectModels<SEC_LOGIN_ACCOUNT>(lmFilter);
            
            foreach(var item in accounts)
            {

                if (string.IsNullOrWhiteSpace(item.W_CODE))
                {
                    continue;
                }

                BizFilter filterWx = new BizFilter(typeof(WX_ACCOUNT));
                filterWx.And("PK_W_CODE", item.W_CODE);

                WX_ACCOUNT wxAccount = decipher.SelectToOneModel<WX_ACCOUNT>(filterWx);

                string wx_appid = GlobelParam.GetValue<string>("WX_APPID", "wx6fa24581855394a9", "微信上的APPID");

                string me_domain = GlobelParam.GetValue<string>("ME_DOMAIN", "http://pay.46202.yq-ic.com", "自己的域名");

                //授权完成后跳转的地址
                string redirect_url = HttpContext.Current.Server.UrlEncode($"{me_domain}/App/InfoGrid2/WF/Handlers/UrlMap.ashx?id={id}&view_code={view_code}");



                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" +
                             $"appid={wx_appid}&redirect_uri=http%3a%2f%2fwc.51.yq-ic.com%2FAPI%2FMenuUrl.ashx&response_type=code&scope=snsapi_base&" +
                             $"state=key%3dMTC%26url%3d{redirect_url}#wechat_redirect";

                //创建申请模板消息 南沙共青团用的

                var obj = new
                {

                    touser = wxAccount.W_OPENID,
                    template_id = "aC7IwFuLVEMIOdv-XkzoBiubMKPv52yasT1HUvdaOiM",
                    url = url,
                    data = new
                    {
                        first = new
                        {
                            value = first_value
                        },
                        keyword1 = new
                        {
                            value = type_value
                        },
                        keyword2 = new
                        {
                            value = title_value

                        },
                        keyword3 = new
                        {
                            value = DateTime.Now.ToString("yyyy-MM-dd HH:mm")

                        },
                        keyword4 = new
                        {
                            value = "审核通过"

                        },
                        remark = new
                        {
                            value = remark_value
                        }

                    }


                };

                SModel sm = SModel.Parse(obj);


                InsertTempCurrency(sm.ToJson(), wxAccount);



            }

        }

        /// <summary>
        /// 创建审批微信模板
        /// </summary>
        /// <param name="codes">所有要审批的用户编码</param>
        /// <param name="id">行ID</param>
        /// <param name="view_code">界面编码</param>
        /// <param name="first_value">模板第一个参数</param>
        /// <param name="keyword1">关键字1</param>
        /// <param name="keyword2">关键字2</param>
        /// <param name="keyword3">关键字3</param>
        /// <param name="remark_value">模板备注</param>
        public static void CreateTemp2(List<string> codes, int id, string view_code, string first_value, string keyword1, string keyword2, string keyword3, string remark_value)
        {
            if(codes == null || codes.Count == 0)
            {
                return;
            }

            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter lmFilter = new LightModelFilter(typeof(SEC_LOGIN_ACCOUNT));
            lmFilter.And("ROW_STATUS_ID", 0, Logic.GreaterThanOrEqual);
            lmFilter.And("BIZ_USER_CODE", codes.ToArray(), Logic.In);

            List<SEC_LOGIN_ACCOUNT> accounts = decipher.SelectModels<SEC_LOGIN_ACCOUNT>(lmFilter);

            foreach (var item in accounts)
            {

                if (string.IsNullOrWhiteSpace(item.W_CODE))
                {
                    continue;
                }

                BizFilter filterWx = new BizFilter(typeof(WX_ACCOUNT));
                filterWx.And("PK_W_CODE", item.W_CODE);

                WX_ACCOUNT wxAccount = decipher.SelectToOneModel<WX_ACCOUNT>(filterWx);


                string wx_appid = GlobelParam.GetValue<string>("WX_APPID", "wx6fa24581855394a9", "微信上的APPID");

                string me_domain = GlobelParam.GetValue<string>("ME_DOMAIN", "http://pay.46202.yq-ic.com", "自己的域名");

                //授权完成后跳转的地址
                string redirect_url = HttpContext.Current.Server.UrlEncode($"{me_domain}/App/InfoGrid2/WF/Handlers/UrlMap.ashx?id={id}&view_code={view_code}");



                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" +
                             $"appid={wx_appid}&redirect_uri=http%3a%2f%2fwc.51.yq-ic.com%2FAPI%2FMenuUrl.ashx&response_type=code&scope=snsapi_base&" +
                             $"state=key%3dMTC%26url%3d{redirect_url}#wechat_redirect";

                //创建申请模板消息 南沙共青团用的

                var obj = new
                {

                    touser = wxAccount.W_OPENID,
                    template_id = "RDqR6iJxqM7viYOoQcEwO0e6aVmBkgrsi5SxRGjuV04",
                    url = url,
                    data = new
                    {
                        first = new
                        {
                            value = first_value
                        },
                        keyword1 = new
                        {
                            value = keyword1
                        },
                        keyword2 = new
                        {
                            value = keyword2

                        },
                        keyword3 = new
                        {
                            value = keyword3

                        },
                        remark = new
                        {
                            value = remark_value
                        }

                    }


                };

                SModel sm = SModel.Parse(obj);


                InsertTempCurrency(sm.ToJson(), wxAccount);



            }


               
        }


        

        /// <summary>
        /// 插入模板到数据库中的通用方法
        /// </summary>
        /// <param name="temp_str">模板数据</param>
        /// <param name="wxAccount">微信用户对象</param>
        static void InsertTempCurrency(string temp_str, WX_ACCOUNT wxAccount)
        {

            DbDecipher decipher = ModelAction.OpenDecipher();

            WX_TEMP_MSG wtm = new WX_TEMP_MSG();
            wtm.TMEP_MSG_DATA = temp_str;
            wtm.FK_W_CODE = wxAccount.PK_W_CODE;
            wtm.ROW_DATE_CREATE = wtm.ROW_DATE_UPDATE = DateTime.Now;
            wtm.BIZ_SID = 0;
            wtm.W_OPENID = wxAccount.W_OPENID;

            decipher.InsertModel(wtm);
        }


        /// <summary>
        /// 根据某些流程发送微信模板消息
        /// </summary>
        /// <param name="flowMgr">流程操作对象</param>
        public static void SendWxTmep(FlowOperateMgr flowMgr)
        {

            string proj_tag = GlobelParam.GetValue("PROJ_TAG", "MALL", "项目标记");

            //如果不是木头厂就不处理
            if (!proj_tag.Equals("WF"))
            {
                return;
            }

            //只有这些流程才能发送微信模板
            Dictionary<string, string> key_value = new Dictionary<string, string>
            {
                { "FLOW-16120602", "reim" },
                { "FLOW-17021502","report"}
            };


            //当前流程编码不在发送模板消息列表中
            if (!key_value.ContainsKey(flowMgr.CurFlowCode))
            {
                return;
            }


            string table_name = flowMgr.TableName;
            int row_id = flowMgr.RowId;
            string view_code = key_value[flowMgr.CurFlowCode];

            CreateFlowTemp(table_name, row_id, flowMgr, view_code);

        }

    }
}