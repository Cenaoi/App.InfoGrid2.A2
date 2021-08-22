using App.Register;
using EasyClick.Web.Mini;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace EC5.SystemBoard.Web.UI
{
	[ParseChildren(true, "ExTemplate")]
	public class Widget : UserControl, IAttributeAccessor
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private Exception m_Exception = null;
		private UserControl m_UserControl;
		/// <summary>
		/// 地址字符串
		/// </summary>
		private string m_UriString;
		/// <summary>
		/// 获取一个值，用以指示当前是否处于设计模式。
		/// </summary>
		private bool m_EcDesignMode = false;
		private bool m_Ajax = false;
		/// <summary>
		/// 容器ID,给 AJAX 使用的
		/// </summary>
		private string m_PanelID;
		/// <summary>
		/// 网页上的请求信息
		/// </summary>
		private string m_Query;
		private string m_Params;
		/// <summary>
		/// 延迟加载
		/// </summary>
		private bool m_EcDelay = false;
		/// <summary>
		/// 返回的数据格式 [html|script]
		/// </summary>
		private string m_EcReturnFormat = "script";
		/// <summary>
		/// 注意：不要放入默认值
		/// </summary>
		private string m_DisplayLanguage;
		/// <summary>
		/// 控件的绝对路径
		/// </summary>
		private string m_AscxPath;
		private bool m_CreateParentBox = true;
		/// <summary>
		/// 扩展属性的模板
		/// </summary>
		private string m_ExTemplate;
		internal List<Control> m_ExTemplateControls;
		private MiniHtmlAttrCollection m_HtmlAttrs = new MiniHtmlAttrCollection();
		protected internal new char ClientIDSeparator
		{
			get
			{
				return base.ClientIDSeparator;
			}
		}
		/// <summary>
		/// 创建父容器
		/// </summary>
		[DefaultValue(true)]
		public bool CreateParentBox
		{
			get
			{
				return this.m_CreateParentBox;
			}
			set
			{
				this.m_CreateParentBox = value;
			}
		}
		/// <summary>
		/// 延迟加载，默认 true
		/// </summary>
		[DefaultValue(true), Description("延迟加载，默认 true")]
		public bool EcDelay
		{
			get
			{
				return this.m_EcDelay;
			}
			set
			{
				this.m_EcDelay = value;
			}
		}
		/// <summary>
		/// 返回的数据类型,[html|script]
		/// </summary>
		[DefaultValue("script"), Description("返回的数据类型, [html|script]")]
		public string EcReturnFormat
		{
			get
			{
				return this.m_EcReturnFormat;
			}
			set
			{
				this.m_EcReturnFormat = value;
			}
		}
		/// <summary>
		/// 容器ID,给 AJAX 使用的
		/// </summary>
		[DefaultValue(""), Description("容器ID,给 AJAX 使用的")]
		public string PanelID
		{
			get
			{
				return this.m_PanelID;
			}
			set
			{
				this.m_PanelID = value;
			}
		}
		/// <summary>
		/// 启动 Ajax  ,默认 false 
		/// </summary>
		[DefaultValue(false), Description("启动 Ajax  ,默认 false")]
		public bool Ajax
		{
			get
			{
				return this.m_Ajax;
			}
			set
			{
				this.m_Ajax = value;
			}
		}
		/// <summary>
		/// 获取一个值，用以指示当前是否处于设计模式。
		/// </summary>
		[DefaultValue(false), Description("获取一个值，用以指示当前是否处于设计模式。")]
		public bool EcDesignMode
		{
			get
			{
				return this.m_EcDesignMode;
			}
			set
			{
				this.m_EcDesignMode = value;
			}
		}
		/// <summary>
		/// 参数
		/// </summary>
		[DefaultValue(""), Description("参数。")]
		public string Params
		{
			get
			{
				return this.m_Params;
			}
			set
			{
				this.m_Params = value;
			}
		}
		/// <summary>
		/// 语言. CH/EN/....
		/// </summary>
		[DefaultValue(""), Description("语言. [CH | EN]")]
		public string DisplayLanguage
		{
			get
			{
				return this.m_DisplayLanguage;
			}
			set
			{
				this.m_DisplayLanguage = value;
			}
		}
		/// <summary>
		/// 组件路径
		/// </summary>
		[DefaultValue(""), Description("组件路径")]
		public string UriString
		{
			get
			{
				return this.m_UriString;
			}
			set
			{
				this.m_UriString = value;
			}
		}
		/// <summary>
		/// 扩展属性的模板
		/// </summary>
		[Browsable(false), PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		public string ExTemplate
		{
			get
			{
				return this.m_ExTemplate;
			}
			set
			{
				this.m_ExTemplate = value;
			}
		}
		protected override void OnInit(EventArgs e)
		{
			HttpContext context = HttpContext.Current;
			this.m_Query = context.Request.Url.Query;
			base.OnInit(e);
		}
		/// <summary>
		/// 给设置子组件的属性
		/// </summary>
		/// <param name="con"></param>
		private void SetChildAttributes(UserControl con)
		{
			foreach (MiniHtmlAttr attr in this.m_HtmlAttrs)
			{
				PropertyInfo prop = con.GetType().GetProperty(attr.Key);
				if (prop != null)
				{
					string txt = attr.Value;
					object v = txt;
					if (v.GetType() != prop.PropertyType)
					{
						if (prop.PropertyType == typeof(Unit))
						{
							v = Unit.Parse(txt);
						}
						else
						{
							v = Convert.ChangeType(v, prop.PropertyType);
						}
					}
					prop.SetValue(con, v, null);
				}
			}
		}
		internal void AddExTemplateControl(Control con)
		{
			if (this.m_ExTemplateControls == null)
			{
				this.m_ExTemplateControls = new List<Control>();
			}
			this.m_ExTemplateControls.Add(con);
		}
		private void CreateUserControl()
		{
			try
			{
				this.m_UserControl = (this.Page.LoadControl(this.m_AscxPath) as UserControl);
				this.m_UserControl.Attributes.Add("EcWidgetID", this.ID);
				this.m_UserControl.ID = "UC";
				this.SetChildAttributes(this.m_UserControl);
				this.EnableViewState = false;
				WidgetFactory.PropExTemplate(this);
			}
			catch (Exception ex)
			{
				this.m_Exception = new Exception(string.Format("加载 \"{0}\" ascx 失败!", this.m_AscxPath), ex);
			}
		}
		protected override void CreateChildControls()
		{
			if (this.m_UriString.StartsWith("~/") || this.m_UriString.StartsWith("/"))
			{
				this.m_AscxPath = this.m_UriString;
			}
			else
			{
				this.m_AscxPath = this.Page.AppRelativeTemplateSourceDirectory + this.m_UriString;
			}
			if (!this.m_Ajax && !this.m_EcDelay)
			{
				this.CreateUserControl();
				this.Controls.Add(this.m_UserControl);
			}
			else
			{
				if (this.m_Ajax && !this.m_EcDelay)
				{
					this.CreateUserControl();
					this.Controls.Add(this.m_UserControl);
				}
				else
				{
					if (this.m_Ajax && this.m_EcDelay)
					{
					}
				}
			}
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (base.DesignMode)
			{
				writer.Write("组件:" + this.m_AscxPath);
			}
			else
			{
				if (!App.Register.RegHelp.IsRegister())
				{
					throw new Exception("调用限制：未注册");
				}
				base.Render(writer);
			}
		}
		public override void RenderControl(HtmlTextWriter writer)
		{
			if (!App.Register.RegHelp.IsRegister())
			{
				throw new Exception("调用限制：未注册");
			}
			if (!string.IsNullOrEmpty(this.m_AscxPath))
			{
				if (this.m_UserControl != null)
				{
					base.RenderControl(writer);
				}
				if (this.m_Exception != null)
				{
					Widget.log.Error(this.m_Exception);
					writer.Write(this.m_Exception.StackTrace);
				}
				else
				{
					if (this.m_Ajax && !this.m_EcDelay)
					{
						MiniWidget mw = new MiniWidget();
						mw.ClientID = WebUtility.Query("__CID", this.ClientID);
						mw.EcDelay = this.m_EcDelay;
						mw.EcDesignMode = this.EcDesignMode;
						mw.EcReturnFormat = this.EcReturnFormat;
						mw.Language = WebUtility.Query("__Lang", this.DisplayLanguage);
						mw.PanelID = WebUtility.Query("__PID", this.PanelID);
						mw.Params = this.m_Params;
						mw.Path = this.m_AscxPath;
						mw.Query = this.m_Query;
						mw.Con = this.m_UserControl;
						mw.CreateParentBox = this.m_CreateParentBox;
						mw.IsUser = true;
						mw.RenderAJAX(writer);
						mw.Dispose();
					}
					else
					{
						if (this.m_Ajax && this.m_EcDelay)
						{
							MiniWidget mw = new MiniWidget();
							mw.ClientID = WebUtility.Query("__CID", this.ClientID);
							mw.EcDelay = this.m_EcDelay;
							mw.EcDesignMode = this.EcDesignMode;
							mw.EcReturnFormat = this.EcReturnFormat;
							mw.IsUser = true;
							mw.Language = "";
							mw.PanelID = WebUtility.Query("__PID", this.PanelID);
							mw.Params = this.Params;
							mw.Path = this.m_AscxPath;
							mw.Query = this.m_Query;
							mw.Attributes = base.Attributes;
							mw.Con = this.m_UserControl;
							mw.CreateParentBox = this.m_CreateParentBox;
							mw.RenderAJAX(writer);
							mw.Dispose();
						}
					}
				}
			}
		}
		public string GetAttribute(string key)
		{
			string result;
			if (this.m_HtmlAttrs.ContainsAttr(key))
			{
				result = this.m_HtmlAttrs.GetAttribute(key);
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
		public void SetAttribute(string key, string value)
		{
			this.m_HtmlAttrs.SetAttribute(key, value);
		}
	}
}
