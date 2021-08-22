using EasyClick.Web.Mini;
using EC5.SystemBoard.EcReflection;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Web.UI;
namespace EC5.SystemBoard.Web.UI
{
	/// <summary>
	/// 控件
	/// </summary>
	public class WidgetControl : UserControl, IExMethodCollection
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// 模块代码
		/// </summary>
		private string m_ModuleCode;

		/// <summary>
		/// 模块虚拟目录
		/// </summary>
		private string m_ModuleDir;

		/// <summary>
		/// 模块信息
		/// </summary>
		private EcModuleInfo m_Module;

		/// <summary>
		/// 扩展函数集合
		/// </summary>
		private ExMethodCollection m_ExMethods = null;
        
        /// <summary>
        /// IsPostBack
        /// </summary>
		private bool m_IsPost = false;

		private SortedDictionary<string, Control> m_Controls = new SortedDictionary<string, Control>();

		/// <summary>
		/// 扩展函数集合
		/// </summary>
		public ExMethodCollection ExMethods
		{
			get
			{
				if (this.m_ExMethods == null)
				{
					this.m_ExMethods = new ExMethodCollection(this);
				}
				return this.m_ExMethods;
			}
		}

		/// <summary>
		/// 模块虚拟目录
		/// </summary>
		public string ModuleDir
		{
			get
			{
				return this.m_ModuleDir;
			}
		}

		/// <summary>
		/// 模块代码
		/// </summary>
		protected string ModuleCode
		{
			get
			{
				return this.m_ModuleCode;
			}
		}

		/// <summary>
		/// 模块信息
		/// </summary>
		public EcModuleInfo Module
		{
			get
			{
				return this.m_Module;
			}
		}


		/// <summary>
		/// 被改写的 Post
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool IsPostBack
		{
			get
			{
				return this.m_IsPost;
			}
		}
		public string CID
		{
			get
			{
				string _cid = base.Request.QueryString["__CID"];
				string result;
				if (!string.IsNullOrEmpty(_cid))
				{
					result = _cid;
				}
				else
				{
					result = this.ClientID;
				}
				return result;
			}
		}

        /// <summary>
        /// 获取扩展函数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
		public ExMethodInfo GetExMethod(string name)
		{
			ExMethodInfo result;
			if (this.HasExMethod())
			{
				result = this.m_ExMethods.GetMethod(name);
			}
			else
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// 判断是否包含扩展函数
		/// </summary>
		/// <returns></returns>
		public bool HasExMethod()
		{
			return this.m_ExMethods != null && this.m_ExMethods.Count > 0;
		}

		/// <summary>
		/// 注册扩展函数
		/// </summary>
		/// <param name="name"></param>
		/// <param name="methodInfo"></param>
		/// <param name="srcOwner"></param>
		public void RegMethod(string name, MethodInfo methodInfo, object srcOwner)
		{
			this.ExMethods.Add(name, methodInfo, srcOwner);
		}

		/// <summary>
		/// 设置模块信息
		/// </summary>
		/// <param name="moduleInfo"></param>
		public void SetModuleInfo(EcModuleInfo moduleInfo)
		{
			this.m_Module = moduleInfo;
			this.m_ModuleCode = moduleInfo.Code;
            this.m_ModuleDir = string.Concat("/", SysBoardManager.DefaultAppCode, "/", moduleInfo.Code, "/");
		}

		private void InitConID(Control parent)
		{
			if (parent.HasControls())
			{
				foreach (Control con in parent.Controls)
				{
					if (!string.IsNullOrEmpty(con.ID))
					{
						this.m_Controls.Add(con.ID, con);
					}
					this.InitConID(con);
				}
			}
		}

		public override Control FindControl(string id)
		{
			return this.FindControl(this, id);
		}

		protected Control FindControl(Control parent, string id)
		{
			Control result;

			if (!parent.HasControls())
			{
				result = null;
			}
			else
			{
                System.Collections.ICollection cons = null;

                object[] pc = parent.GetType().GetCustomAttributes(typeof(ParseChildrenAttribute), true);

                ParseChildrenAttribute curPC = null;

                foreach (var item in pc)
                {
                    if(item is ParseChildrenAttribute)
                    {
                        curPC = (ParseChildrenAttribute)item;
                        break;
                    }
                }

                if(curPC != null && !string.IsNullOrEmpty(curPC.DefaultProperty))
                {
                    PropertyInfo pi= parent.GetType().GetProperty(curPC.DefaultProperty);

                    cons = pi.GetValue(parent, null) as System.Collections.ICollection;
                }

                if (cons == null)
                {
                    cons = parent.Controls as System.Collections.ICollection;
                }

                if(cons  == null)
                {
                    return null;
                }

                foreach (Control con in cons)
                {
                    if (id.Equals(con.ID))
                    {
                        result = con;
                        return result;
                    }


                    Control subCon = this.FindControl(con, id);

                    if (subCon != null)
                    {
                        result = subCon;
                        return result;
                    }
                }

				//for (int i = 0; i < cons.Count; i++)
				//{
				//	Control con = cons[i];
    //                if (id.Equals(con.ID))
    //                {
    //                    result = con;
    //                    return result;
    //                }


    //                Control subCon = this.FindControl(con, id);

				//	if (subCon != null)
				//	{
				//		result = subCon;
				//		return result;
				//	}
				//}

				result = null;
			}
			return result;
		}

		protected void SetMiniControlValue(string cid)
		{
			NameValueCollection form = base.Request.Form;

            foreach (string item in form.Keys)
            {
                if (item == null || cid.Length >= item.Length || !item.StartsWith(cid))
                {
                    continue;
                }

                string keyName = item.Substring(cid.Length);
                Control child;

                try
                {
                    child = this.FindControl(keyName);
                }
                catch (Exception ex)
                {
                    throw new Exception("内部错误，按控件名查找错误.名称：" + keyName, ex);
                }

                if (child != null)
                {
                    string value = form[item];
                    if (child is CheckBox)
                    {
                        ((CheckBox)child).Checked = true;
                    }
                    else
                    {
                        MiniHelper.SetValue(child, value);
                    }
                }
            }

			this.LoadPostData(this);

			MiniScriptManager.ClientScript.Items.Clear();
			MiniScriptManager.ClientScript.ReadOnly = false;
		}

		protected void LoadPostData(Control parent)
		{
			foreach (Control conSub in parent.Controls)
			{
				if (conSub is IMiniControl)
				{
					((IMiniControl)conSub).LoadPostData();
				}
				if (conSub.HasControls())
				{
					this.LoadPostData(conSub);
				}
			}
		}


		private bool Pro_IsPost(string isPost)
		{
			bool result;
			if (base.Request.Form.Count > 0 || base.Request.HttpMethod.ToUpper() == "POST")
			{
				result = true;
			}
			else
			{
				if (isPost != null)
				{
					isPost = isPost.ToUpper();
					if ("0" == isPost || "FALSE" == isPost)
					{
						result = false;
						return result;
					}

					if ("1" == isPost || "TRUE" == isPost)
					{
						result = true;
						return result;
					}
				}

				result = ((base.Request.QueryString["__Action"] != null && base.Request.QueryString["__IsPost"] == null) || this.Page.IsPostBack);
			}
			return result;
		
        }



        /// <summary>
        /// 初始化自定义控件
        /// </summary>
        protected virtual void OnInitCustomControls(EventArgs e)
        {

        }
        

		/// <summary>
		///
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
            
			string isPost = base.Request.QueryString["__IsPost"];
			this.m_IsPost = this.Pro_IsPost(isPost);

            //初始化自定义控件
            OnInitCustomControls(EventArgs.Empty);

			if (this.m_IsPost)
			{
				this.SetMiniControlValue(this.CID + "_");
			}

            
		}

        /// <summary>
        /// 数据绑定
        /// </summary>
        public override void DataBind()
        {
            this.DataBind(this);
        }

        protected void DataBind(Control parent)
        {
            if(parent.GetType().FullName == "EasyClick.Web.Mini2.Store")
            {
                parent.DataBind();

                return;
            }

            foreach (Control conSub in parent.Controls)
            {               

                if (conSub.HasControls())
                {
                    this.DataBind(conSub);
                }
            }
        }


        public override void Dispose()
		{
			this.m_Module = null;

			if (this.m_ExMethods != null)
			{
				this.m_ExMethods.Dispose();
			}
			this.m_ExMethods = null;
			base.Dispose();
		}
	}
}
