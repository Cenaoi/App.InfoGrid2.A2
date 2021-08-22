using System;
using System.Collections;
namespace EC5.SystemBoard.Web.UI
{
	/// <summary>
	/// App 页面工厂
	/// </summary>
	public interface IAppPageHandler
	{
		/// <summary>
		/// 获取属性
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		string GetAttr(IDictionary pageItems, string name);
	}
}
