using System;
using System.Collections.Generic;
using System.Reflection;
namespace EC5.SystemBoard.Web.UI
{
	/// <summary>
	/// 扩展函数
	/// </summary>
	public class ExMethodInfo : IDisposable
	{
		private string m_Name;
		/// <summary>
		/// 原函数
		/// </summary>
		private MethodInfo m_SrcMethodInfo;
		/// <summary>
		/// 原对象类型
		/// </summary>
		private object m_SrcOwner;
		/// <summary>
		/// 被扩展的对象
		/// </summary>
		private object m_Owner;
		/// <summary>
		/// 扩展函数名
		/// </summary>
		public string Name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				this.m_Name = value;
			}
		}
		public MethodInfo SrcMethod
		{
			get
			{
				return this.m_SrcMethodInfo;
			}
			set
			{
				this.m_SrcMethodInfo = value;
			}
		}
		public object SrcOwner
		{
			get
			{
				return this.m_SrcOwner;
			}
			set
			{
				this.m_SrcOwner = value;
			}
		}
		public object Owner
		{
			get
			{
				return this.m_Owner;
			}
			set
			{
				this.m_Owner = value;
			}
		}
		/// <summary>
		/// 扩展函数（构造函数)
		/// </summary>
		public ExMethodInfo()
		{
		}
		/// <summary>
		/// 扩展函数（构造函数)
		/// </summary>
		/// <param name="name"></param>
		/// <param name="srcMethodInfo"></param>
		/// <param name="srcOwner"></param>
		/// <param name="owner"></param>
		public ExMethodInfo(string name, MethodInfo srcMethodInfo, object srcOwner, object owner)
		{
			this.m_Name = name;
			this.m_SrcMethodInfo = srcMethodInfo;
			this.m_SrcOwner = srcOwner;
			this.m_Owner = owner;
		}
		/// <summary>
		/// 扩展函数（构造函数)
		/// </summary>
		/// <param name="name"></param>
		/// <param name="srcMethodInfo"></param>
		/// <param name="srcOwner"></param>
		/// <param name="owner"></param>
		public ExMethodInfo(string name, MethodInfo srcMethodInfo, object owner)
		{
			this.m_Name = name;
			this.m_SrcMethodInfo = srcMethodInfo;
			this.m_Owner = owner;
			this.m_SrcOwner = owner;
		}
		/// <summary>
		/// 执行函数
		/// </summary>
		/// <returns></returns>
		public object Invoke(object[] paramList)
		{
			object result;
			if (paramList != null && paramList.Length == 0)
			{
				result = this.m_SrcMethodInfo.Invoke(this.m_SrcOwner, new object[]
				{
					this.m_Owner
				});
			}
			else
			{
				List<object> ps = new List<object>();
				ps.Add(this.m_Owner);
				ps.AddRange(paramList);
				result = this.m_SrcMethodInfo.Invoke(this.m_SrcOwner, ps.ToArray());
			}
			return result;
		}
		public void Dispose()
		{
			this.m_Owner = null;
			this.m_SrcOwner = null;
			this.m_SrcMethodInfo = null;
		}
	}
}
