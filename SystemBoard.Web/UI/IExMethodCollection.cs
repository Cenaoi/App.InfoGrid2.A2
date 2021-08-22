using System;
using System.Reflection;
namespace EC5.SystemBoard.Web.UI
{
	/// <summary>
	/// 扩展函数集合
	/// </summary>
	public interface IExMethodCollection
	{
		void RegMethod(string name, MethodInfo methodInfo, object srcOwner);
		bool HasExMethod();
		ExMethodInfo GetExMethod(string name);
	}
}
