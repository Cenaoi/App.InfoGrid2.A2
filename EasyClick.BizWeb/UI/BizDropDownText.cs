using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using EasyClick.Web.Mini;
using EC5.SystemBoard.Clouds;
using EC5.SystemBoard.Interfaces;
using HWQ.Entity.LightModels;
using EC5.Utility;
using System.Web.UI;

namespace EasyClick.BizWeb.UI
{
    [ParseChildren(true), PersistChildren(false)]
    public class BizDropDownText : DropDownText
    {

        #region Biz 参数



        string m_BizUri;

        string m_BizParams;

        public string BizUri
        {
            get { return m_BizUri; }
            set { m_BizUri = value; }
        }

        [DefaultValue("")]
        public string BizParams
        {
            get { return m_BizParams; }
            set { m_BizParams = value; }
        }

        string m_ValueField;

        string m_TextField;

        [DefaultValue("")]
        public string ValueField
        {
            get { return m_ValueField; }
            set { m_ValueField = value; }
        }

        [DefaultValue("")]
        public string TextField
        {
            get { return m_TextField; }
            set { m_TextField = value; }
        }

        private void InitBiz()
        {
            if (string.IsNullOrEmpty(m_BizUri))
            {
                return;
            }

            string[] sp = null;

            if (!string.IsNullOrEmpty(m_BizParams))
            {
                sp = StringUtil.Split( m_BizParams);
            }

            ICloudMethod method = CloudManager.GetObj().GetMethod(m_BizUri.Trim());
            IList reValue = method.Invoke(sp) as IList;

            if (reValue == null)
            {
                return;
            }

            FillItems(reValue);
        }

        private void FillItems(IList models)
        {

            string text;
            string value;


            foreach (object item in models)
            {
                if (item is string)
                {
                    string v = (string)item;
                    this.Items.Add(v, v);
                }
                else
                {
                    if (!string.IsNullOrEmpty(m_ValueField))
                    {
                        value = LightModel.GetFieldValue(item, m_ValueField).ToString();
                    }
                    else
                    {
                        value = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(m_TextField))
                    {
                        text = LightModel.GetFieldValue(item, m_TextField).ToString();
                    }
                    else
                    {
                        text = string.Empty;
                    }

                    this.Items.Add(value, text);
                }
            }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            InitBiz();
        }


    }
}
