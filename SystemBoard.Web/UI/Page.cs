using App.Register;
using System;
using System.Web.UI;

namespace EC5.SystemBoard.Web.UI
{
    public class Page : System.Web.UI.Page
	{
		private string m_MetaDescription;
		private string m_MetaKeywords;
		private string m_Title;
		private string m_Lang = "CN";
		private bool m_IsInitConfig = false;
		public new string Title
		{
			get
			{
				if (!App.Register.RegHelp.IsRegister())
				{
					throw new Exception("调用限制：未注册");
				}
				if (string.IsNullOrEmpty(this.m_Title))
				{
					this.OnInitConfig();
					this.m_Title = PageFactory.GetAttrForLang(this.m_Lang, this, "Title");
				}
				return this.m_Title;
			}
			set
			{
				this.m_Title = value;
			}
		}
		public string Lang
		{
			get
			{
				return this.m_Lang;
			}
			set
			{
				this.m_Lang = value;
			}
		}
		public string MetaDescription
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_MetaDescription))
				{
					this.OnInitConfig();
					this.m_MetaDescription = PageFactory.GetAttrForLang(this.m_Lang, this, "MetaDescription");
				}
				return this.m_MetaDescription;
			}
			set
			{
				this.m_MetaDescription = value;
			}
		}
		public string MetaKeywords
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_MetaKeywords))
				{
					this.OnInitConfig();
					this.m_MetaKeywords = PageFactory.GetAttrForLang(this.m_Lang, this, "MetaKeywords");
				}
				return this.m_MetaKeywords;
			}
			set
			{
				this.m_MetaKeywords = value;
			}
		}
		private void OnInitConfig()
		{
			if (!this.m_IsInitConfig)
			{
				this.m_IsInitConfig = true;
				string rawUrl = base.Request.PhysicalPath;
				string rootUrl = base.Server.MapPath("~/");
				string path = rawUrl.Substring(rootUrl.Length).Replace('\\', '/');
				string[] pathItems = path.Split(new char[]
				{
					'/'
				});
				if (pathItems.Length > 0)
				{
					string pItem = pathItems[0].ToUpper();
					this.m_Lang = pItem;
				}
			}
		}
	}
}
