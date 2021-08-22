using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using EasyClick.Web.Mini;
using System.ComponentModel;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 分页控件
    /// </summary>
    [Description("分页控件")]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public class Pagination : Control
    {


        #region 字段

        string m_Command = "loadPage";

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

        string m_FirstText = "首页";

        string m_PrevText = "上一页";

        string m_NextText = "下一页";
        string m_LastText = "尾页";

        /// <summary>
        ///
        /// </summary>
        bool m_EcDesignMode = false;

        /// <summary>
        /// 设置为 AJAX
        /// </summary>
        bool m_IsAjax = false;

        CellAlign m_Align = CellAlign.Center;

        /// <summary>
        /// 隐藏行数选择框
        /// </summary>
        bool m_HideRowCountSelect = false;

        string m_StoreID;

        #endregion

        #region 按钮的文字

        /// <summary>
        /// 首页的文字
        /// </summary>
        [DefaultValue("首页")]
        [Description("首页的文字")]
        public string FistText
        {
            get { return m_FirstText; }
            set { m_FirstText = value; }
        }

        /// <summary>
        /// 上一页的文字
        /// </summary>
        [DefaultValue("上一页")]
        [Description("上一页的文字")]
        public string PrevText
        {
            get { return m_PrevText; }
            set { m_PrevText = value; }
        }

        /// <summary>
        /// 下一页的文字
        /// </summary>
        [DefaultValue("下一页")]
        [Description("下一页的文字")]
        public string NextText
        {
            get { return m_NextText; }
            set { m_NextText = value; }
        }

        /// <summary>
        /// 尾页的文字
        /// </summary>
        [DefaultValue("尾页")]
        [Description("尾页的文字")]
        public string LastText
        {
            get { return m_LastText; }
            set { m_LastText = value; }
        }
        #endregion


        #region 属性

        Store m_Store;

        internal void SetStore(Store store)
        {
            m_Store = store;

            m_Store.PageChanged += new EventHandler(m_Store_PageChanged);
        }

        void m_Store_PageChanged(object sender, EventArgs e)
        {
                
        }

        /// <summary>
        /// 数据仓库的对象ID
        /// </summary>
        [Description("数据仓库的对象ID")]
        public string StoreID
        {
            get { return m_StoreID; }
            set { m_StoreID = value; }
        }

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

        #endregion




        public void Reset()
        {
            //MiniScript.Add("{0}.reset({{'itemTotal':{1},'curPage':{2},'urlFormat':'{3}','buttonCount':{4},'rowCount':{5},'command':'{6}' }});",
            //    GetClientID(),
            //    this.ItemTotal, this.CurPage, this.UrlFormat, this.ButtonCount, this.RowCount, this.Command);

            //MiniScript.Add("$('#{0}_CurPIndex').val('{1}');", this.GetClientID(), this.CurPage);
        }

        bool m_Visible = true;

        [DefaultValue(true)]
        public override bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        /// <summary>
        /// 隐藏行数选择框
        /// </summary>
        [Description("隐藏行数选择框")]
        [DefaultValue(false)]
        public bool HideRowCountSelect
        {
            get { return m_HideRowCountSelect; }
            set { m_HideRowCountSelect = value; }
        }

        private void RenderDesign(HtmlTextWriter writer)
        {

            if (!m_Visible)
            {
                return;
            }

            writer.Write("<div class='{0}' ", this.ClassName);
            writer.Write(">");

            writer.Write("<a >{0}</a>", m_FirstText);
            writer.Write("<a >{0}</a>", m_PrevText);

            writer.Write("<a >1</a>");
            writer.Write("<a >2</a>");
            writer.Write("<a >3</a>");
            writer.Write("<a >4</a>");


            writer.Write("<a >{0}</a>", m_NextText);
            writer.Write("<a >{0}</a>", m_LastText);

            writer.Write("</div>");

        }

        protected override void Render(HtmlTextWriter writer)
        {

            if (this.DesignMode )
            {
                RenderDesign(writer);
                return;
            }

            string tempBoxID = "Box" + RandomUtil.Next();


            writer.Write("<div class='{0} {1}' ", tempBoxID, this.ClassName);
            if (!m_Visible)
            {
                writer.Write("style='display:none;' ");
            }
            writer.Write("></div>\n");

            //writer.WriteLine("<input type='hidden' id='{0}_CurPIndex' name='{0}_CurPIndex' value='0' />", this.ClientID);

            writer.WriteLine("<script type='text/javascript'>");

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }

            if (jsMode == "MInJs2X")
            {
                writer.WriteLine("In.ready('Mini2.ui.Pagination',function(){");
            }
            else
            {
                writer.WriteLine("$(document).ready(function(){");
            }



            writer.WriteLine("    var pager = Mini2.create('Mini2.ui.Pagination',{");
            writer.WriteLine("        id:'{0}',", this.ClientID);

            writer.WriteLine("        renderTo:'.{0}',", tempBoxID);

            writer.WriteLine("        itemTotal:{0},", this.ItemTotal);
            writer.WriteLine("        curPage:{0},", this.CurPage);
            writer.WriteLine("        urlFormat:'{0}',", this.UrlFormat);
            writer.WriteLine("        buttonCount:{0},", this.ButtonCount);
            writer.WriteLine("        rowCount:{0},", RowCount);
            writer.WriteLine("        command:'{0}',", this.Command);

            writer.WriteLine("        firstText:'{0}',",m_FirstText);
            writer.WriteLine("        prevText:'{0}',",m_PrevText); 
            writer.WriteLine("        nextText:'{0}',",m_NextText);
            writer.WriteLine("        lastText:'{0}'",m_LastText);

            writer.WriteLine("        hideRowCountSelect: '{0},", m_HideRowCountSelect?"true":"false");

            writer.WriteLine("        click: function(page, store){");

            writer.WriteLine("            alert(page);");

            writer.WriteLine("        },");
        
        
            writer.WriteLine("    });");
            writer.WriteLine("    pager.render();");

            writer.WriteLine("    {0} = pager;", this.ClientID);


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

            return (UserControl)con;
        }

    }
}
