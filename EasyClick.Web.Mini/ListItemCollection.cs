using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;
using System.Web.UI;
using EasyClick.Web.Mini.Utility;
using System.Collections;


namespace EasyClick.Web.Mini
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed class ListItemCollection : List<ListItem>
    {
        MiniHtmlListBase m_Owner;

        public ListItemCollection(MiniHtmlListBase owner)
        {
            m_Owner = owner;          
        }

        public ListItemCollection()
        {
            
        }

        /// <summary>
        /// (JScript)
        /// </summary>
        public new void Clear()
        {
            //string txt = value.Replace("\"", "\\\"").Replace("\r", @"\r").Replace("\n", @"\n");

            base.Clear();

            if (m_Owner == null)
            {
                return;
            }

            MiniScript.Add("$('#{0} option').remove();", m_Owner.GetClientID());
        }


        /// <summary>
        /// (JScript)
        /// </summary>
        /// <param name="item"></param>
        public new void Add(ListItem item)
        {
            base.Add(item);

            if (m_Owner == null)
            {
                return;
            }

            string v = JsonUtility.ToJson( item.Value);
            string txt = JsonUtility.ToJson(item.Text);

            MiniScript.Add("$('#{0}').get(0).options.add(new Option('{2}','{1}'));", m_Owner.GetClientID(), v, txt);
        }


        /// <summary>
        /// (JScript)
        /// </summary>
        /// <param name="value"></param>
        public void Add(string value)
        {
            base.Add(new ListItem(value)); 
            
            if (m_Owner == null)
            {
                return;
            }

            string v = JsonUtility.ToJson( value);

            MiniScript.Add("$('#{0}').get(0).options.add(new Option('{2}','{1}'));", m_Owner.GetClientID(), v, v);
        }

        /// <summary>
        /// (JScript)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="text"></param>
        public void Add(string value, string text)
        {
            base.Add(new ListItem(value, text));

            if (m_Owner == null)
            {
                return;
            }

            string v = JsonUtility.ToJson(value);//.Replace("\"", "\\\"").Replace("\r", @"\r").Replace("\n", @"\n");
            string txt = JsonUtility.ToJson(text);//.Replace("\"", "\\\"").Replace("\r", @"\r").Replace("\n", @"\n");

            MiniScript.Add("$('#{0}').get(0).options.add(new Option('{2}','{1}'));", m_Owner.GetClientID(), v, txt);
        }


        public void AddRange(IList items)
        {
            foreach (object item in items)
            {
                Add(item.ToString());
            }
        }
    
    }
}
