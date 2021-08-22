using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Xml;
namespace EC5.SystemBoard.Web.UI
{
	public static class WidgetFactory
	{
		/// <summary>
		/// 扩展属性的组件,继承 IExTemplateControl 接口的
		/// </summary>
		private static SortedList<string, Type> m_ExTemplateTypes;
		/// <summary>
		/// 扩展属性的类型,继承 IExTemplateControl 接口的
		/// </summary>
		public static SortedList<string, Type> ExTemplateTypes
		{
			get
			{
				if (WidgetFactory.m_ExTemplateTypes == null)
				{
					WidgetFactory.m_ExTemplateTypes = new SortedList<string, Type>();
				}
				return WidgetFactory.m_ExTemplateTypes;
			}
		}
		internal static void PropExTemplateAttrs(XmlNode node, Control con, Type exType)
		{
			foreach (XmlAttribute attr in node.Attributes)
			{
				PropertyInfo pi = exType.GetProperty(attr.Name);
				if (pi == null)
				{
					break;
				}
				if (pi.PropertyType != typeof(string))
				{
					object v = Convert.ChangeType(attr.Value, pi.PropertyType);
					pi.SetValue(con, v, null);
				}
				else
				{
					pi.SetValue(con, attr.Value, null);
				}
			}
		}
		/// <summary>
		/// 扩展模板属性
		/// </summary>
		internal static XmlDocument PropExTemplate(Widget widget)
		{
			XmlDocument result;
			if (widget.ExTemplate == null || widget.ExTemplate.Trim().Length == 0)
			{
				result = null;
			}
			else
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml("<Root>" + widget.ExTemplate + "</Root>");
				foreach (XmlNode node in doc.DocumentElement.ChildNodes)
				{
					if (WidgetFactory.m_ExTemplateTypes.ContainsKey(node.Name))
					{
						Type exType = WidgetFactory.m_ExTemplateTypes[node.Name];
						Control exCon = (Control)Activator.CreateInstance(exType);
						exCon.ID = widget.ClientID + widget.ClientIDSeparator + node.Name;
						WidgetFactory.PropExTemplateAttrs(node, exCon, exType);
						widget.AddExTemplateControl(exCon);
					}
				}
				result = doc;
			}
			return result;
		}
	}
}
