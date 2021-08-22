using System;
using System.Collections.Generic;
using System.Web.UI;
namespace EC5.SystemBoard.Web.UI
{
	/// <summary>
	///
	/// </summary>
	public static class PageFactory
	{
		private static SortedDictionary<string, IAppPageHandler> m_HeaderForLang = null;
		/// <summary>
		/// 按语种获取属性值
		/// </summary>
		/// <param name="lang">语言</param>
		/// <param name="page"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string GetAttrForLang(string lang, Page page, string name)
		{
			string key = "Lang";
			string result;
			if (PageFactory.m_HeaderForLang == null || !PageFactory.m_HeaderForLang.ContainsKey(key))
			{
				result = string.Empty;
			}
			else
			{
				page.Items[key] = lang;
				IAppPageHandler aph = PageFactory.m_HeaderForLang[key];
				result = aph.GetAttr(page.Items, name);
			}
			return result;
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="lang"></param>
		/// <param name="appPageHandler"></param>
		public static void AddLangHander(string lang, IAppPageHandler appPageHandler)
		{
			if (PageFactory.m_HeaderForLang == null)
			{
				PageFactory.m_HeaderForLang = new SortedDictionary<string, IAppPageHandler>();
			}
			PageFactory.m_HeaderForLang.Add(lang, appPageHandler);
		}
	}
}
