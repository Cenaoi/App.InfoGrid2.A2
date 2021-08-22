using System;
using System.Collections.Generic;
using System.Reflection;
namespace EC5.SystemBoard.Web.UI
{
	/// <summary>
	/// 扩展函数集合
	/// </summary>
	public class ExMethodCollection : IDisposable
	{
		private List<ExMethodInfo> m_Items = new List<ExMethodInfo>();
		private SortedList<string, ExMethodInfo> m_Sorted = new SortedList<string, ExMethodInfo>();
		private object m_Owner;

		/// <summary>
		/// 当前对象
		/// </summary>
		public object Owner
		{
			get
			{
				return this.m_Owner;
			}
		}

		/// <summary>
		/// 数量
		/// </summary>
		public int Count
		{
			get
			{
				return this.m_Items.Count;
			}
		}
		/// <summary>
		/// 扩展函数集合(构造函数)
		/// </summary>
		/// <param name="owner"></param>
		public ExMethodCollection(object owner)
		{
			this.m_Owner = owner;
		}
		/// <summary>
		/// 添加扩展函数
		/// </summary>
		/// <param name="name">扩展函数名称</param>
		/// <param name="exMethod">扩展函数</param>
		/// <param name="srcOwner">原对象</param>
		public void Add(string name, MethodInfo exMethod, object srcOwner)
		{
			ExMethodInfo exMI = new ExMethodInfo(name, exMethod, srcOwner, this.m_Owner);
			this.Remove(name);
			this.m_Items.Add(exMI);
			this.m_Sorted.Add(name, exMI);
		}
		/// <summary>
		/// 删除扩展函数
		/// </summary>
		/// <param name="name">扩展函数名</param>
		/// <returns>是否删除成功</returns>
		public bool Remove(string name)
		{
			ExMethodInfo srcExMI = null;
			bool exist = this.m_Sorted.TryGetValue(name, out srcExMI);
			if (!exist)
			{
				this.m_Items.Remove(srcExMI);
				this.m_Sorted.Remove(name);
			}
			return exist;
		}
		/// <summary>
		/// 获取函数
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ExMethodInfo GetMethod(string name)
		{
			ExMethodInfo method = null;
			this.m_Sorted.TryGetValue(name, out method);
			return method;
		}
		/// <summary>
		/// 清理
		/// </summary>
		public void Clear()
		{
			foreach (ExMethodInfo mi in this.m_Items)
			{
				mi.Dispose();
			}
			this.m_Sorted.Clear();
			this.m_Items.Clear();
		}
		public void Dispose()
		{
			this.Clear();
			this.m_Sorted = null;
			this.m_Items = null;
			this.m_Owner = null;
		}
	}
}
