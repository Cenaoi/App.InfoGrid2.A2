using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Reflection;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{

    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed class ListItemCollection : List<ListItem>
    {
        Control m_Owner;

        BoundField m_OwnerElem;

        public ListItemCollection(Control owner)
        {
            m_Owner = owner;
        }

        public ListItemCollection(BoundField owner)
        {
            m_OwnerElem = owner;
        }

        public ListItemCollection()
        {

        }

        private string GetOwnerClientID()
        {
            string id = null;

            if (m_Owner != null && !string.IsNullOrEmpty(m_Owner.ClientID))
            {
                id =  m_Owner.ClientID;
            }
            else if (m_OwnerElem != null && !string.IsNullOrEmpty(m_OwnerElem.ClientID))
            {
               id = m_OwnerElem.ClientID;
            }

            return id;
        }


        /// <summary>
        /// (JScript)
        /// </summary>
        public new void Clear()
        {
            //string txt = value.Replace("\"", "\\\"").Replace("\r", @"\r").Replace("\n", @"\n");

            base.Clear();

            string clientId = GetOwnerClientID();

            if (!string.IsNullOrEmpty(clientId))
            {
                ScriptManager.Eval("{0}.getStore().removeAll();", clientId);
            }

        }

        /// <summary>
        /// (JScript)
        /// </summary>
        /// <param name="item"></param>
        public new void Add(ListItem item)
        {
            base.Add(item);
            
            string clientId = GetOwnerClientID();

            if (!string.IsNullOrEmpty(clientId))
            {
                string v = JsonUtil.ToJson(item.Value);
                string txt = JsonUtil.ToJson(item.Text);

                if (StringUtil.IsBlank(item.TextEx))
                {
                    ScriptManager.Eval($"{clientId}.getStore().add({{ value:'{v}', text:'{txt}' }});");
                }
                else
                {
                    string textEx = JsonUtil.ToJson(item.TextEx);

                    ScriptManager.Eval($"{clientId}.getStore().add({{ value:'{v}', text:'{txt}', textEx:'{textEx}' }});");

                }
            }            

        }

        /// <summary>
        /// (JScript)
        /// </summary>
        /// <param name="value"></param>
        public void Add(string value)
        {
            base.Add(new ListItem(value,value));
            

            string clientId = GetOwnerClientID();

            if (!string.IsNullOrEmpty(clientId))
            {
                string v = JsonUtil.ToJson(value);

                ScriptManager.Eval("{0}.getStore().add({1});", clientId, v);
            }

        }

        /// <summary>
        /// (JScript)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="text"></param>
        public void Add(string value, string text)
        {
            base.Add(new ListItem(value, text));


            string clientId = GetOwnerClientID();

            if (!string.IsNullOrEmpty(clientId))
            {
                string v = JsonUtil.ToJson(value);
                string txt = JsonUtil.ToJson(text);
                ScriptManager.Eval("{0}.getStore().add({{ value:'{1}', text:'{2}' }});", clientId, v, txt);
            }


        }


        public void AddRange(IList items)
        {
            foreach (object item in items)
            {
                if (item is ListItem)
                {
                    Add((ListItem)item);
                }
                else
                {
                    Add(item.ToString());
                }
            }
        }

    }
}
