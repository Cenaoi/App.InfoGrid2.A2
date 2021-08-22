using System;
namespace EC5.SystemBoard.Web.UI
{
	/// <summary>
	/// 组件显示模式
	/// </summary>
	public enum WidgetVisibleMode : byte
	{
		/// <summary>
		/// 普通组建模式
		/// </summary>
		Common,
		/// <summary>
		/// AJAX 模式
		/// </summary>
		Ajax,
		/// <summary>
		/// Ajax 和 延迟显示模式
		/// </summary>
		AjaxDelay = 3
	}
}
