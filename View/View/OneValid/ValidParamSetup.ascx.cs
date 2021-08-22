using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EC5.Utility.Web;
using EC5.SystemBoard.Web.UI;
using EC5.SystemBoard.Interfaces;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EasyClick.Web.Mini2;
using EC5.Utility;

namespace App.InfoGrid2.View.OneValid
{
    public partial class ValidParamSetup : WidgetControl, IView
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnInit(EventArgs e)
        {
            System.Web.HttpContext.Current.Items["JS_MODE"] = "MInJs2";

            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string ruleJson = WebUtil.QueryBase64("rule");

                    JObject o = JObject.Parse(ruleJson);

                    InitData(o);
                }

            }
            catch (Exception ex) 
            {
                log.Error("解析json数据出错！",ex);
            }

            //加密例子

            
        }


        /// <summary>
        /// 保存按钮事件
        /// </summary>
        public void btnSave()
        {

            string json = GetJson();

            json = EC5.Utility.JsonUtil.ToJson(json);

            EasyClick.Web.Mini.MiniHelper.Eval("ownerWindow.close({result:\"ok\", rule:\"" + json + "\"} );");

        }



        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="o">json对象</param>
        public void InitData(JObject o) 
        {
          
            foreach (JProperty prop in o.Properties())
            {

                string name = prop.Name;

                if (name == "messages")
                {
                    o = JObject.Parse(prop.Value.ToString());
                    foreach (JProperty item in o.Properties())
                    {
                        SetMessageControl(item);
                    }
                    
                }
                else
                {
                    SetValueControl(prop);
                }
            }

        }

        /// <summary>
        /// 设置提示消息文本框
        /// </summary>
        /// <param name="prop">Json属性</param>
        public void SetMessageControl(JProperty prop) 
        {
            try
            {
                ///属性名称
                string name = prop.Name;

                ///属性值
                string value = prop.Value.Value<string>();

             
                ///根据节点名称复制给相应的控件
                switch (name)
                {
                    case "remote":  this.tbxRemote.Value  = value; break;
                    case "email": this.tbxEmail.Value  = value; break;
                    case "url": this.tbxUrl.Value  = value; break;
                    case "date": this.tbxDate.Value  = value; break;
                    case "dateISO": this.tbxDateISO.Value  = value; break;
                    case "number": this.tbxNumber.Value  = value; break;
                    case "digits": this.tbxDigits.Value  = value; break;
                    case "creditcard": this.tbxCreditcard.Value  = value; break;
                    case "equalTo": this.tbxEqualTo.Value  = value; break;
                    case "maxlength": this.tbxMaxlength.Value  = value; break;
                    case "minlength": this.tbxMinlength.Value  = value; break;
                    case "rangelength": this.tbxRangelength.Value  = value; break;
                    case "range": this.tbxRange.Value  = value; break;
                    case "max": this.tbxMax.Value  = value; break;
                    case "min": this.tbxMin.Value  = value; break;
                }
            }
            catch (Exception ex)
            {
                log.Error("数据初始化出错！", ex);
            }
        }


        /// <summary>
        /// 设置值文本框
        /// </summary>
        /// <param name="prop">Json属性</param>
        public void SetValueControl(JProperty prop) 
        {
            try
            {
                //属性名称
                string name = prop.Name;
                //最大值
                string maxValue = "";
                //最小值
                string minValue = "";

                //属性值
                string value = prop.Value.ToString();  //prop.Value.Value<string>();

                //这是拿出最大值和最小值
                if (value.IndexOf("[") == 0 && value.IndexOf("]") > 0)
                {
                    string newValue = value.Substring(1, value.Length - 2);
                    string[] strList = newValue.Split(',');
                    minValue = strList[0].Replace("\r\n", "");
                    maxValue = strList[1].Replace("\r\n", ""); ;

                }

                //去除前后冒号""
                //value = value.Substring(1,value.Length-2);

                decimal decValue = 0;

                if(prop.Type == JTokenType.Integer)
                {
                    decValue = prop.Value<int>();
                }

                
                //根据节点名称复制给相应的控件
                switch (name)
                {
                    case "remote": this.cbRemote.Checked = true; this.tbxRemoteValue.Value = value; break;
                    case "email": this.cbEmail.Checked = true; break;
                    case "url": this.cbUrl.Checked = true; break;
                    case "date": this.cbDate.Checked = true; break;
                    case "dateISO": this.cbDateISO.Checked = true; break;
                    case "number": this.cbNumber.Checked = true; break;
                    case "digits": this.cbDigits.Checked = true; break;
                    case "creditcard": this.cbCreditcard.Checked = true; break;
                    case "equalTo": this.cbEqualTo.Checked = true; this.tbxEqualToValue.Value = value; break;
                    case "maxlength":
                        this.cbMaxlength.Checked = true;
                        this.tbxMaxlengthValue.Value = value;
                        break;
                    case "minlength":
                        this.cbMinlength.Checked = true;
                        this.tbxMinlengthValue.Value = value;
                        break;
                    case "rangelength": this.cbRangelength.Checked = true; this.tbxRangelengthMaxMin.StartValue = minValue; this.tbxRangelengthMaxMin.EndValue = maxValue; break;
                    case "range": this.cbRange.Checked = true; this.tbxRangeMaxMin.EndValue = maxValue; this.tbxRangeMaxMin.StartValue = minValue; break;
                    case "max": this.cbMax.Checked = true;
                        this.tbxMaxValue.Value = value;
                        break;
                    case "min":
                        this.cbMin.Checked = true;
                        this.tbxMinValue.Value = value; break;
                }
            }
            catch (Exception ex) 
            {
                log.Error("数据初始化出错！",ex);
            }
        }



        /// <summary>
        /// 转换成json
        /// </summary>
        /// <returns></returns>
        public string GetJson() 
        {

            StringBuilder sb = new StringBuilder();
            
            if (this.cbRemote.Checked) 
            {
                sb.AppendFormat("remote: \"{0}\",",this.tbxRemoteValue.Value);               
            }
            if(this.cbEmail.Checked)
            {
                sb.AppendFormat("email:true,");
            }
            if(this.cbUrl.Checked)
            {
                sb.AppendFormat("url:true,");
            }
            if(this.cbDate.Checked)
            {
                sb.AppendFormat("date:true,");
            }
            if(this.cbDateISO.Checked)
            {
                sb.AppendFormat("dateISO:true,");
            }
            if(this.cbNumber.Checked)
            {
                sb.AppendFormat("number:true,");
            }
            if(this.cbDigits.Checked)
            {
                sb.AppendFormat("digits:true,");
            }
            if(this.cbCreditcard.Checked)
            {
                sb.AppendFormat("creditcard:true,");
            }
            if(this.cbEqualTo.Checked)
            {
                sb.AppendFormat("equalTo:\"{0}\",",this.tbxEqualToValue.Value);
            }
            if(this.cbMaxlength.Checked)
            {
                sb.AppendFormat("maxlength:{0},", StringUtil.ToInt( this.tbxMaxlengthValue.Value));
            }
            if(this.cbMinlength.Checked)
            {

                sb.AppendFormat("minlength:{0},", StringUtil.ToInt(this.tbxMinlengthValue.Value));
            }
            if(this.cbRangelength.Checked)
            {

                sb.AppendFormat("rangelength:[{0},{1}],", StringUtil.ToDecimal(this.tbxRangelengthMaxMin.StartValue), StringUtil.ToDecimal(this.tbxRangelengthMaxMin.EndValue));
            }
            if(this.cbRange.Checked)
            {
                sb.AppendFormat("range:[{0},{1}],", StringUtil.ToDecimal(this.tbxRangeMaxMin.StartValue), StringUtil.ToDecimal(this.tbxRangeMaxMin.EndValue));
            }
            if(this.cbMax.Checked)
            {
                sb.AppendFormat("max:{0},", StringUtil.ToDecimal(this.tbxMaxValue.Value));
            }
            if(this.cbMin.Checked)
            {
                sb.AppendFormat("min:{0},", StringUtil.ToDecimal( this.tbxMinValue.Value));
            }


            StringBuilder msgs = new StringBuilder();


            //这下面是生成提示消息的

            if (this.cbRemote.Checked && !string.IsNullOrEmpty(this.tbxRange.Value))
            {
                msgs.AppendFormat("remote:\"{0}\",", this.tbxRemote.Value);
            }
            if (this.cbEmail.Checked && !string.IsNullOrEmpty(this.tbxEmail.Value))
            {
                msgs.AppendFormat("email:\"{0}\",", this.tbxEmail.Value);
            }
            if (this.cbUrl.Checked && !string.IsNullOrEmpty(this.tbxUrl.Value))
            {
                msgs.AppendFormat("url:\"{0}\",", this.tbxUrl.Value);
            }
            if (this.cbDate.Checked && !string.IsNullOrEmpty(this.tbxDate.Value))
            {
                msgs.AppendFormat("date:\"{0}\",", this.tbxDate.Value);
            }
            if (this.cbDateISO.Checked && !string.IsNullOrEmpty(this.tbxDateISO.Value))
            {
                msgs.AppendFormat("dateISO:\"{0}\",", this.tbxDateISO.Value);
            }
            if (this.cbNumber.Checked && !string.IsNullOrEmpty(this.tbxNumber.Value))
            {
                msgs.AppendFormat("number:\"{0}\",", this.tbxNumber.Value);
            }
            if (this.cbDigits.Checked && !string.IsNullOrEmpty(this.tbxDigits.Value))
            {
                msgs.AppendFormat("digits:\"{0}\",", this.tbxDigits.Value);
            }
            if (this.cbCreditcard.Checked && !string.IsNullOrEmpty(this.tbxCreditcard.Value))
            {
                msgs.AppendFormat("creditcard:\"{0}\",", this.tbxCreditcard.Value);
            }
            if (this.cbEqualTo.Checked && !string.IsNullOrEmpty(this.tbxEqualTo.Value))
            {
                msgs.AppendFormat("equalTo:\"{0}\",", this.tbxEqualTo.Value);
            }
            if (this.cbMaxlength.Checked && !string.IsNullOrEmpty(this.tbxMaxlength.Value))
            {
                msgs.AppendFormat("maxlength:\"{0}\",", this.tbxMaxlength.Value);
            }
            if (this.cbMinlength.Checked && !string.IsNullOrEmpty(this.tbxMinlength.Value))
            {

                msgs.AppendFormat("minlength:\"{0}\",", this.tbxMinlength.Value);
            }
            if (this.cbRangelength.Checked && !string.IsNullOrEmpty(this.tbxRangelength.Value))
            {

                msgs.AppendFormat("rangelength:\"{0}\",", this.tbxRangelength.Value);
            }
            if (this.cbRange.Checked && !string.IsNullOrEmpty(this.tbxRange.Value))
            {
                msgs.AppendFormat("range:\"{0}\",", this.tbxRange.Value);
            }
            if (this.cbMax.Checked && !string.IsNullOrEmpty(this.tbxMax.Value))
            {
                msgs.AppendFormat("max:\"{0}\",", this.tbxMax.Value);
            }
            if (this.cbMin.Checked && !string.IsNullOrEmpty(this.tbxMin.Value))
            {
                msgs.AppendFormat("min:\"{0}\",", this.tbxMin.Value);
            }

            
            ///这是去掉最后面的逗号
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            if (msgs.Length > 0)
            {
                msgs.Remove(sb.Length - 1, 1);

                if (sb.Length > 0)
                {
                    sb.Append(",");
                }

                sb.Append("messages:{");
                sb.Append(msgs.ToString());
                sb.Append("}");
            }

            
            return "{" +  sb.ToString() + "}";

        }

    }
}