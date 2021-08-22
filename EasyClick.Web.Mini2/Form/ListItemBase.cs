using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 下拉框项目集合
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ListItemBaseCollection : List<ListItemBase>
    {
        Control m_Owner;

        public ListItemBaseCollection()
        {
        }

        public ListItemBaseCollection(Control owner)
        {
            m_Owner = owner;
        }

        public Control Owner
        {
            get { return m_Owner; }
        }

        public ListItemBase FindBy(string key, string value)
        {

            foreach (ListItemBase item in this)
            {
                string itemValue = item.GetAttribute(key);
                
                if (itemValue == value)
                {
                    return item;
                }
            }

            return null;
        }
    }

    public sealed class ListItemBase : IAttributeAccessor
    {

        Dictionary<string, string> m_Values = new Dictionary<string, string>();

        public string GetJson()
        {

            

            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            int i = 0;

            foreach (string item in m_Values.Keys)
            {
                if (i++ > 0) { sb.Append(", "); }


                string keyJson = JsonUtil.ToJson(item, JsonQuotationMark.SingleQuotes);
                string valueJson = JsonUtil.ToJson(m_Values[item], JsonQuotationMark.SingleQuotes);

                sb.AppendFormat("'{0}':'{1}'", keyJson, valueJson);
            }

            sb.Append("}");

            return sb.ToString();
        }



        public string GetAttribute(string key)
        {
            string value;

            m_Values.TryGetValue(key, out value);

            return value;
        }

        public void SetAttribute(string key, string value)
        {
            m_Values[key] = value;
        }


    }
    

}
