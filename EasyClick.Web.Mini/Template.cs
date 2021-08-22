using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using EasyClick.Web.Mini.Utility;

namespace EasyClick.Web.Mini
{

    public class DataFormatCollection:List<DataFormat>
    {
        public string GetFormatString(string dataField)
        {
            string dataFormat = string.Empty;

            for (int i = 0; i < base.Count; i++)
            {
                DataFormat df = base[i];

                if (df.DataField == dataField)
                {
                    dataFormat = df.ForamtString;

                    break;
                }
            }

            return dataFormat;
        }

        public void Add(string dataField, string formatStr)
        {
            DataFormat df = new DataFormat();
            df.DataField = dataField;
            df.ForamtString = formatStr;

            base.Add(df);
        }
    }

    public class DataFormat
    {
        public string DataField { get; set; }

        public string ForamtString { get; set; }
    }

    public class TemplateParam
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    /// <summary>
    /// 轻量级模板
    /// </summary>
    [DefaultProperty("ItemTemplate")]
    [ParseChildren(true, "ItemTemplate")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:Template  runat=\"server\" >\r\n\t<ItemTemplate>\r\n\t</ItemTemplate>\r\n</{0}:Template>")]
    public class Template : Control, IAttributeAccessor,IClientIDMode,IMiniControl
    {
                
        public Template()
        {
            
        }

        ClientIDMode m_ClientIDMode = ClientIDMode.AutoID;

        [DefaultValue(ClientIDMode.AutoID)]
        public ClientIDMode ClientIDMode
        {
            get { return m_ClientIDMode; }
            set { m_ClientIDMode = value; }
        }

        public string GetClientID()
        {
            string cId;

            switch (m_ClientIDMode)
            {
                case ClientIDMode.Static:
                    cId = this.ID;
                    break;
                default:
                    cId = this.ClientID;
                    break;
            }

            return cId;
        }


        /// <summary>
        /// 获取一个值，用以指示当前是否处于设计模式。
        /// </summary>
        bool m_EcDesignMode = false;

        /// <summary>
        /// 获取一个值，用以指示当前是否处于设计模式。
        /// </summary>
        public bool EcDesignMode
        {
            get 
            {

                return m_EcDesignMode; 
            }
            set { m_EcDesignMode = value; }
        }

        bool m_FilterField = true;

        int[] m_ItemGuids = null;

        string m_ItemGudisStr; 

        /// <summary>
        /// 过滤字段
        /// </summary>
        [Description("过滤字段,默认 true")]
        [DisplayName("过滤字段")]
        [DefaultValue(true)]
        public bool FilterField
        {
            get { return m_FilterField; }
            set { m_FilterField = value; }
        }

        public int[] GetItemGuids()
        {
            if (m_ItemGuids == null)
            {
                m_ItemGuids = StringUtility.ToIntList(m_ItemGudisStr);
            }

            return m_ItemGuids;
        }
        

        /// <summary>
        /// 数据条目集合
        /// </summary>
        TemplateItemCollection m_Items;

        DataFormatCollection m_DataFormats;

        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public DataFormatCollection DataFormats
        {
            get
            {
                if (m_DataFormats == null)
                {
                    m_DataFormats = new DataFormatCollection();
                }
                return m_DataFormats;
            }
        }

        /// <summary>
        /// 数据条目集合
        /// </summary>
        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TemplateItemCollection Items
        {
            get
            {
                if ( m_Items == null)
                {
                    if (this.DesignMode || this.EcDesignMode)
                    {
                        m_Items = new TemplateItemCollection();
                    }
                    else
                    {
                        m_Items = new TemplateItemCollection(this);
                    }
                }
                return m_Items;
            }
        }


        private SortedList<string, string> GetItems()
        {
            SortedList<string, string> items = new SortedList<string, string>();

            string itemEx = this.ClientID + "_Items_";

            int itemExLen = itemEx.Length;

            foreach (string itemKey in this.Page.Request.Form.Keys)
            {
                if (itemKey.Length <= itemExLen)
                {
                    continue;
                }

                if (itemKey.StartsWith(itemEx))
                {
                    string newKey = itemKey.Substring(itemEx.Length);

                    items.Add(newKey, this.Page.Request.Form[itemKey]);
                }
            }

            return items;
        }

        protected override void OnInit(EventArgs e)
        {            
            //base.LoadControlState(savedState);

            if (this.DesignMode)
            {
                return;
            }

            string key = this.ClientID + "__ItemGudisBox__";

            string v = this.Page.Request.Form[key];
            
            if (v != null)
            {
                string[] ids = v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                SortedList<string, string> items = GetItems();

                SortedList<int, TemplateItemData> datas = new SortedList<int, TemplateItemData>();

                bool readOnly = MiniScriptManager.ClientScript.ReadOnly;

                MiniScriptManager.ClientScript.ReadOnly = true;

                foreach (string itemKey in items.Keys)
                {
                    int n = itemKey.IndexOf('_');

                    if (n == -1)
                    {
                        continue;
                    }

                    string itemNumStr = itemKey.Substring(0, n);
                    string itemName = itemKey.Substring(n + 1);

                    int itemNum = 0;

                    if (!int.TryParse(itemNumStr, out itemNum))
                    {
                        continue;
                    }

                    TemplateItemData cell = null;

                    if (!datas.ContainsKey(itemNum))
                    {
                        cell = new TemplateItemData();
                        datas.Add(itemNum, cell);

                        this.Items.Add(cell);
                    }

                    cell = datas[itemNum];

                    cell.Add(itemName, items[itemKey]);

                }

                MiniScriptManager.ClientScript.ReadOnly = readOnly;
            }


            base.OnInit(e);
        }

        protected internal string[] GetFieldNames(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new string[0] ;
            }

            //string input = @"{$T.Name},今天是星期{$T.XXS}xxxxx";

            string pattern = @"\{\$T\.(?<Field>\w+)\}";
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(pattern, options);
            MatchCollection matches = regex.Matches(input);

            //string[] fs = new string[matches.Count];

            List<string> fs = new List<string>();

            for (int i = 0; i < matches.Count; i++)
            {
                string field =  matches[i].Groups["Field"].Value;

                if (!fs.Contains(field))
                {
                    fs.Add(field);
                }
            }

            return fs.ToArray();
        }

        protected void Render_DesignMode(System.Web.UI.HtmlTextWriter writer)
        {
            string txt = m_ItemTemplate;

            string[] fs = GetFieldNames(txt);

            foreach (object item in this.Items)
            {
                string itemTxt = txt;

                Type objT = item.GetType();


                if (item is ICustomTypeDescriptor)
                {
                    ICustomTypeDescriptor customTs = (ICustomTypeDescriptor)item;

                    PropertyDescriptorCollection propList = customTs.GetProperties();

                    PropertyDescriptor prop = null;

                    for (int i = 0; i < fs.Length; i++)
                    {
                        prop = propList[fs[i]];

                        object srcV = prop.GetValue(item);

                        string v;

                        if (srcV == null)
                        {
                            v = string.Empty;
                        }
                        else
                        {
                            v = srcV.ToString();
                        }

                        itemTxt = itemTxt.Replace("{$T." + fs[i] + "}", v);
                    }

                }
                else
                {

                    for (int i = 0; i < fs.Length; i++)
                    {
                        PropertyInfo pi = objT.GetProperty(fs[i]);

                        object srcV = pi.GetValue(item, null);

                        string v;

                        if (srcV == null)
                        {
                            v = string.Empty;
                        }
                        else
                        {
                            v = srcV.ToString();
                        }

                        itemTxt = itemTxt.Replace("{$T." + fs[i] + "}", v);
                    }
                }

                writer.Write(itemTxt);


            }
        }



        protected internal virtual string GetItemJson(object item)
        {
            return GetItemJson(item,null, null);
        }



        protected internal virtual string GetItemJson(object item, DataFormatCollection dataFormats, string[] fields)
        {
            if (item is string)
            {
                return (string)item;
            }

            return MiniConfiguration.JsonFactory.GetItemJson(item,dataFormats, fields);
        }

        

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                if (!App.Register.RegHelp.IsRegister()) throw new Exception("调用限制：未注册");
            }

            if (this.DesignMode )
            {
                writer.Write(this.ItemTemplate);

                return;
            }

            if (this.EcDesignMode)
            {
                Render_DesignMode(writer);

                return;
            }

            writer.Write("<script type='text/html' id='{0}'>", GetClientID());
            writer.Write(m_ItemTemplate);
            writer.Write("</script>");

            writer.WriteLine("<script type=\"text/javascript\">");

            writer.WriteLine("var {0} = null;", GetClientID());

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }


            if (jsMode == "InJs")
            {
                writer.WriteLine("In.ready('mi.Template',function () {");
            }
            else
            {
                writer.WriteLine("$(document).ready(function () {");
            }

            writer.WriteLine("      {0} = new Mini.ui.Template({{ 'id':'{0}' }});", GetClientID());

            //writer.Write("{0}.addItem({{'name':'核武器','age':23 }});\r\n",this.ClientID);

            string[] fs = null;

            if (this.FilterField)
            {
                fs = GetFieldNames(this.ItemTemplate);
            }

            if (this.Items.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("try { ");
                sb.AppendLine();

                sb.AppendFormat("{0}.addItemRange([", GetClientID());

                sb.AppendLine(GetItemJson(this.Items[0], m_DataFormats,  fs));
                sb.AppendLine();

                for (int i = 1; i < this.Items.Count; i++)
                {
                    sb.Append(",");
                    sb.Append(GetItemJson(this.Items[i], m_DataFormats, fs));
                    sb.AppendLine();
                }

                sb.Append("]);");

                sb.AppendLine();
                sb.AppendLine("} catch(ex) { alert(ex.Message); }");
                sb.AppendLine();

                writer.WriteLine(sb.ToString());
            }

            writer.WriteLine("});");



            writer.WriteLine("</script>");
        }

        /// <summary>
        ///  (jTemplate 模板)
        /// </summary>
        string m_ItemTemplate;

        /// <summary>
        /// 项目的模板 (jTemplate 模板)
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DefaultValue("")]
        [Browsable(false)]
        public virtual string ItemTemplate
        {
            get { return m_ItemTemplate; }
            set 
            { 
                m_ItemTemplate = value;

                if (this.DesignMode || MiniScriptManager.ClientScript.ReadOnly)
                {
                    return;
                }

                string txt = JsonUtility.ToJson( value);//.Replace("\"", "\\\"").Replace("\r", @"\r").Replace("\n", @"\n");
                MiniScript.Add("{0}.itemTemplate(\"{1}\");", GetClientID(), txt);
            }
        }

        string m_Value;


        /// <summary>
        /// (JScript) 值
        /// </summary>
        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.Attribute), MergableProperty(false)]
        [Browsable(false)]
        protected string Value
        {
            get { return m_Value; }
            set
            {
                if (!this.DesignMode && m_Value != value && !MiniScriptManager.ClientScript.ReadOnly)
                {

                }

                m_Value = value;
            }
        }


        #region Attribute

        internal MiniHtmlAttrCollection m_HtmlAttrs = new MiniHtmlAttrCollection();

        /// <summary>
        /// 是否存在此对应属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsAttr(string key)
        {
            return m_HtmlAttrs.ContainsAttr(key);
        }

        public string GetAttribute(string key)
        {
            return m_HtmlAttrs.GetAttribute(key);
        }

        public void SetAttribute(string key, string value)
        {
            m_HtmlAttrs.SetAttribute(key, value);
        }

        #endregion

        /// <summary>
        /// （JScript）触发条目发生变化的事件
        /// </summary>
        public void OnItemsChanged()
        {
            MiniScript.Add("{0}.onItemsChanged();",GetClientID());
        }



        #region IMiniControl 成员

        public void LoadPostData()
        {
            HttpContext content = HttpContext.Current;

            string key = this.GetClientID() + "__ItemGudisBox__";

            try
            {
                string value = content.Request.Form[key];

                m_ItemGudisStr = value;
            }
            catch
            { }
        }

        #endregion
    }
}
