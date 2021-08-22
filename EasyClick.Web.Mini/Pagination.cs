using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using EasyClick.Web.Mini.Utility;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 分页组件
    /// </summary>
    [ToolboxData("<{0}:Pagination runat=\"server\" className=\"\" />")]
    public class Pagination:Control,IClientIDMode,IMiniControl
    {
        /// <summary>
        /// 分页组件
        /// </summary>
        public Pagination()
        {

        }

        #region 字段

        string m_Command = "GoPage";

        /// <summary>
        /// 样式名称
        /// </summary>
        string m_ClassName = "sabrosus";

        /// <summary>
        /// 每页显示的记录数量
        /// </summary>
        int m_RowCount = 20;

        
        string m_UrlFormat = "PageIndex={0}";

        /// <summary>
        /// 显示的页数量
        /// </summary>
        int m_ButtonCount = 10;
        
        /// <summary>
        /// 当前页码
        /// </summary>
        int m_CurPage = 0;

        /// <summary>
        /// 记录总数量
        /// </summary>
        int m_ItemTotal = 0;

        string m_ButtonFirstText = "首页";

        string m_ButtonPrevText = "上一页";

        string m_ButtonNextText = "下一页";
        string m_ButtonLastText = "尾页";

        /// <summary>
        ///
        /// </summary>
        bool m_EcDesignMode = false;

        /// <summary>
        /// 设置为 AJAX
        /// </summary>
        bool m_IsAjax = false;

        CellAlign m_Align = CellAlign.Center;

        #endregion

        #region 按钮的文字

        [DefaultValue("首页")]
        [Description("首页的文字")]
        public string ButtonFistText
        {
            get { return m_ButtonFirstText; }
            set { m_ButtonFirstText = value; }
        }

        [DefaultValue("上一页")]
        [Description("上一页的文字")]
        public string ButtonPrevText
        {
            get { return m_ButtonPrevText; }
            set { m_ButtonPrevText = value; }
        }


        [DefaultValue("下一页")]
        [Description("下一页的文字")]
        public string ButtonNextText
        {
            get { return m_ButtonNextText; }
            set { m_ButtonNextText = value; }
        }

        [DefaultValue("尾页")]
        [Description("尾页的文字")]
        public string ButtonLastText
        {
            get { return m_ButtonLastText; }
            set { m_ButtonLastText = value; }
        }
        #endregion


        #region 属性

        [DefaultValue(CellAlign.Center)]
        public CellAlign Align
        {
            get { return m_Align; }
            set { m_Align = value; }
        }

        /// <summary>
        /// 设置 Ajax 模式
        /// </summary>
        [DefaultValue(false)]
        [Description("设置 Ajax 模式")]
        public bool IsAjax
        {
            get { return m_IsAjax; }
            set { m_IsAjax = value; }
        }

        /// <summary>
        /// 命令名称
        /// </summary>
        [DefaultValue("")]
        [Description("命令名称")]
        public string Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        /// <summary>
        /// 样式文件
        /// </summary>
        [DefaultValue("")]
        [Description("当前页码")]
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }

        /// <summary>
        /// 每页显示的记录数量
        /// </summary>
        [DefaultValue(0)]
        [Description("每页显示的记录数量")]
        public int RowCount
        {
            get { return m_RowCount; }
            set { m_RowCount = value; }
        }

        /// <summary>
        /// 按钮的超链接
        /// </summary>
        [DefaultValue("")]
        [Description("按钮的超链接")]
        public string UrlFormat
        {
            get { return m_UrlFormat; }
            set { m_UrlFormat = value; }
        }

        /// <summary>
        /// 按钮的数量
        /// </summary>
        [DefaultValue(0)]
        [Description("按钮的数量")]
        public int ButtonCount
        {
            get { return m_ButtonCount; }
            set { m_ButtonCount = value; }
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        [DefaultValue(0)]
        [Description("当前页码")]
        public int CurPage
        {
            get { return m_CurPage; }
            set { m_CurPage = value; }
        }

        /// <summary>
        /// 记录的总数量
        /// </summary>
        [DefaultValue(0)]
        [Description("记录的总数量")]
        public int ItemTotal
        {
            get { return m_ItemTotal; }
            set { m_ItemTotal = value; }
        }

        [DefaultValue(false)]
        public bool EcDesignMode
        {
            get { return m_EcDesignMode; }
            set { m_EcDesignMode = value; }
        }

        #endregion


        ClientIDMode m_ClientIDMode = ClientIDMode.AutoID;

        [DefaultValue(ClientIDMode.AutoID)]
        public ClientIDMode ClientIDMode
        {
            get { return m_ClientIDMode; }
            set { m_ClientIDMode = value; }
        }



        public string GetClientID()
        {
            string cId;

            switch (m_ClientIDMode)
            {
                case ClientIDMode.Static:
                    cId = this.ID;
                    break;
                default:
                    cId = this.ClientID;
                    break;
            }

            return cId;
        }


        public void Reset()
        {
            MiniScript.Add("{0}.reset({{'itemTotal':{1},'curPage':{2},'urlFormat':'{3}','buttonCount':{4},'rowCount':{5},'command':'{6}' }});", 
                GetClientID(),
                this.ItemTotal, this.CurPage,this.UrlFormat, this.ButtonCount, this.RowCount,this.Command);

            MiniScript.Add("$('#{0}_CurPIndex').val('{1}');", this.GetClientID(), this.CurPage);
        }

        bool m_Visible = true;

        [DefaultValue(true)]
        public override bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }


        protected override void Render(HtmlTextWriter writer)
        {

            if (this.DesignMode || this.EcDesignMode)
            {
                if (!m_Visible)
                {
                    return;
                }

                writer.Write("<div class='{0}' ",this.ClassName);
                writer.Write(">");

                writer.Write("<a >{0}</a>", m_ButtonFirstText);
                writer.Write("<a >{0}</a>", m_ButtonPrevText);

                writer.Write("<a >1</a>");
                writer.Write("<a >2</a>");
                writer.Write("<a >3</a>");
                writer.Write("<a >4</a>");


                writer.Write("<a >{0}</a>", m_ButtonNextText);
                writer.Write("<a >{0}</a>", m_ButtonLastText);

                writer.Write("</div>");

                return;
            }


            writer.Write("<div id='{0}' class='{1}' ", this.GetClientID(), this.ClassName);
            if (!m_Visible)
            {
                writer.Write("style='display:none;' ");
            }
            writer.Write("></div>");

            writer.WriteLine("<input type='hidden' id='{0}_CurPIndex' name='{0}_CurPIndex' value='0' />", this.GetClientID());
            writer.WriteLine("<input type='hidden' id='{0}_RowCount' name='{0}_RowCount' value='{1}' />", this.GetClientID(),m_RowCount);


            writer.WriteLine("<script type='text/javascript'>");
            writer.WriteLine("var {0} = null;", this.GetClientID());

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }

            if (jsMode == "InJs")
            {
                writer.WriteLine("In.ready('mi.Pagination',function(){");
            }
            else
            {
                writer.WriteLine("$(document).ready(function(){");
            }

            writer.WriteLine("    var c = new Mini.ui.Pagination({ id: '" + this.GetClientID() + "',allowSetupRowCount:true });");
            writer.WriteLine("    c.reset({");

            writer.WriteLine("        itemTotal:{0},", this.ItemTotal);
            writer.WriteLine("        curPage:{0},", this.CurPage);
            writer.WriteLine("        urlFormat:'{0}',", this.UrlFormat);
            writer.WriteLine("        buttonCount:{0},", this.ButtonCount);
            writer.WriteLine("        rowCount:{0},", RowCount);
            writer.WriteLine("        command:'{0}',", this.Command);
            writer.WriteLine("        buttonText:{{first:'{0}',prev:'{1}',next:'{2}',last:'{3}'}}", m_ButtonFirstText, m_ButtonPrevText, m_ButtonNextText, m_ButtonLastText);
            writer.WriteLine("    });");

            writer.WriteLine("    {0} = c;", this.GetClientID());

            if (m_IsAjax)
            {
                UserControl uc = FindWidget();

                if (uc != null)
                {
                    writer.WriteLine("  c.click(function (sender,e) {");

                    //writer.WriteLine("    if(e && e.owner && e.owner.getRowCount){");
                    //writer.WriteLine("        $('#{0}_RowCount').val( e.owner.getRowCount() );", GetClientID());
                    //writer.WriteLine("    }");

                    writer.WriteLine("    $('#{0}_CurPIndex').val( $(this).attr('commandParam') );", GetClientID());
                    writer.WriteLine("    {0}.submit(this);", uc.ClientID);
                    writer.WriteLine("    return false;");
                    writer.WriteLine("  });");
                }
            }

            writer.WriteLine("});");
            writer.WriteLine("</script>");

        }

        private UserControl FindWidget()
        {
            Control con = this.Parent;

            for (int i = 0; i < 9; i++)
            {
                if (con is UserControl)
                {
                    break;
                }

                if (con.Parent == null)
                {
                    break;
                }

                con = con.Parent;
            }

            return con as UserControl;
        }

        #region IMiniControl 成员

        public void LoadPostData()
        {
            string key = string.Format("{0}_CurPIndex", GetClientID());
            string vStr = this.Context.Request.Form[key];

            string rowCountId = string.Format("{0}_RowCount", GetClientID());
            string rowCountStr = this.Context.Request.Form[rowCountId];
            
            this.m_CurPage = StringUtility.ToInt(vStr);
            this.m_RowCount = StringUtility.ToInt(rowCountStr);
        }

        #endregion
    }
}
