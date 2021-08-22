using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using EasyClick.Web.Mini2.Data;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 数据视图
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    [Description("数据视图")]
    public partial class DataView :Component, IAttributeAccessor, Mini.IMiniControl, IPanel, IDelayRender
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// (构造函数)表格
        /// </summary>
        public DataView()
        {
            this.JsNamespace = "Mini2.ui.panel.DataView";
            this.InReady = "Mini2.ui.panel.DataView";

        }

        protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CreateChildControls();
        }
        



        /// <summary>
        /// 获取提交数据的 JSON 原始数据
        /// </summary>
        /// <returns></returns>
        public string GetCheckedJson()
        {
            if (m_Checked == null)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(m_Checked.Value))
            {
                HttpRequest request = this.Context.Request;
                string json = request.Form[m_Checked.ClientID];

                m_Checked.Value = json;
            }

            return m_Checked.Value;
        }

        /// <summary>
        /// 获取选中记录的批次
        /// </summary>
        /// <returns></returns>
        public DataBatch GetCheckedBatch()
        {
            string json = GetCheckedJson();

            if (string.IsNullOrEmpty(json))
            {
                return new DataBatch();
            }

            DataBatch db = DataBatch.Parse(json);

            return db;
        }


        /// <summary>
        /// 客户端脚本 函数名
        /// </summary>
        public string OnItemAdded { get; set; }

        /// <summary>
        /// 客户端脚本 
        /// </summary>
        public string OnItemRender { get; set; }

        public string OnItemRendered { get; set; }

        public string OnItemRendering { get; set; }


        /// <summary>
        /// 获取当前控件中的 StoreID 对应的唯一ID
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        private string GetStoreIdByPage(string storeId)
        {
            if (m_Store != null)
            {
                return m_Store.ClientID;
            }

            if (string.IsNullOrEmpty(storeId))
            {
                return string.Empty;
            }

            Control con = this.Parent.FindControl(storeId);

            if (con == null)
            {
                con = this.Page.FindControl(storeId);
            }

            if (con == null)
            {
                return string.Empty;
            }

            return con.ClientID;
        }


        protected override void CreateChildControls()
        {
            if (m_InitCreateChild)
            {
                return;
            }

            m_InitCreateChild = true;

            base.CreateChildControls();


            m_Checked = new Hidden();
            m_Checked.ID = this.ID + "_Checked";
            this.Controls.Add(m_Checked);

            m_Pager = new Pagination();
            m_Pager.ID = this.ID + "_Pager";
            this.Controls.Add(m_Pager);
        }



        /// <summary>
        /// (支持 JS)设置记录选择状态
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="isCheck"></param>
        public void SetRecordCheck(string recordId, bool isCheck)
        {
            if (m_RecordCheckList == null)
            {
                m_RecordCheckList = new SortedList<string, bool>();
            }

            if (m_RecordCheckList.ContainsKey(recordId))
            {
                m_RecordCheckList[recordId] = isCheck;
            }
            else
            {
                m_RecordCheckList.Add(recordId, isCheck);
            }

            string isCheckStr = isCheck.ToString().ToLower();

            ScriptManager.Eval("{0}.setRecordCheck('{1}',{2})", this.ClientID, recordId, isCheckStr);
        }

        /// <summary>
        /// 获取项目的 item 
        /// </summary>
        /// <returns></returns>
        private DataViewItem GetItem()
        {
            DataViewItem dvItem;

            StringBuilder sb = new StringBuilder();

            using (HtmlTextWriter htmlTW = new HtmlTextWriter(new StringWriter(sb)))
            {

                dvItem = new DataViewItem();

                this.ItemTemplate.InstantiateIn(dvItem);
                
                dvItem.RenderControl(htmlTW);
                dvItem.RenderTemplate = sb.ToString();

            }

            return dvItem;
        }

        protected virtual void FullScript(StringBuilder sb)
        {
            int iWidth, iHeight, iItemWidth, iItemHeigh;


            string storeId = GetStoreIdByPage(this.StoreID);

            ScriptTextWriter st = new ScriptTextWriter(sb, QuotationMarkConvertor.SingleQuotes);

            st.RetractBengin("var field = Mini2.create('" + this.JsNamespace + "', {");
            {
                st.WriteParam("id", this.ID);
                st.WriteParam("clientId", this.ClientID);

                st.WriteParam("applyTo", "#" + this.ClientID);

                st.WriteParam("store", storeId);

                st.WriteParam("visible", this.Visible, true);


                st.WriteParam("checkedMode", this.CheckedMode, CheckedMode.Multi, TextTransform.Lower);
                st.WriteParam("jsonMode", this.JsonMode, DataViewJsonMode.Simple, TextTransform.Lower);

                if (int.TryParse(this.Width,out iWidth))
                {
                    st.WriteParam("width", iWidth);
                }
                else
                {
                    st.WriteParam("width", this.Width);
                }

                if (int.TryParse(this.Height, out iHeight))
                {
                    st.WriteParam("height", iHeight);
                }
                else
                {
                    st.WriteParam("height", this.Height);
                }


                st.WriteParam("selectCls", this.SelectClass);
                st.WriteParam("focusCls", this.FocusClass);

                st.WriteParam("itemCls", this.ItemClass);


                if (int.TryParse(this.ItemWidth, out iItemWidth))
                {
                    st.WriteParam("itemWidth", iItemWidth);
                }
                else
                {
                    st.WriteParam("itemWidth", this.ItemWidth);
                }

                if (int.TryParse(this.ItemHeight, out iItemHeigh))
                {
                    st.WriteParam("itemHeight", iItemHeigh);
                }
                else
                {
                    st.WriteParam("itemHeight", this.ItemHeight);
                }

                st.WriteParam("scroll", this.Scroll, ScrollMode.Auto, TextTransform.Lower);

                st.WriteParam("autoSize", this.AutoSize);


                st.WriteParam("dock", this.Dock, DockStyle.Top, TextTransform.Lower);
                st.WriteParam("region", this.Region, RegionType.North, TextTransform.Lower);


                st.WriteParam("pagerVisible", this.PagerVisible, true);
                st.WriteParam("hidePagerRowCountSelect", this.HidePagerRowCountSelect, false);


                DataViewItem dvItem = GetItem();
              
                st.WriteParam("itemTemplate", dvItem.RenderTemplate);

                if (dvItem.HasScript())
                {
                    StringBuilder fun = new StringBuilder("function(e){");

                    fun.AppendLine(dvItem.ToScriptAll());

                    fun.Append("}");

                    //item 渲染的脚本
                    st.WriteFunction("itemRenderScript", fun.ToString());
                }

                                

                //函数名
                if (!StringUtil.IsBlank(this.OnItemAdded))
                {
                    st.WriteFunction("itemAdded", this.OnItemAdded);
                }

                if (!StringUtil.IsBlank(this.OnItemRendering))
                {
                    st.WriteFunction("itemRendering", this.OnItemRendering);
                }

                if (!StringUtil.IsBlank(this.OnItemRender))
                {
                    st.WriteFunction("itemRender", this.OnItemRender);
                }

                if (!StringUtil.IsBlank(this.OnItemRendered))
                {
                    st.WriteFunction("itemRendered", this.OnItemRendered);
                }

                if (this.CellDbclick != null)
                {

                    StringBuilder fun = new StringBuilder("function(){");
                    fun.Append("widget1.subMethod('form:first',{");
                    fun.AppendFormat("subName:'{0}',", this.ID);
                    fun.AppendFormat("subMethod:'{0}'", "PreCellDbclick");
                    fun.Append("});");
                    fun.Append("}");

                    st.WriteFunction("cellDbclick", fun.ToString());
                }

            }
            st.RetractEnd("});");
            



            if (this.IsDelayRender)
            {
                st.WriteCodeLine("field.delayRender();");
            }
            else
            {
                st.WriteCodeLine("field.render();");
            }

            st.WriteCodeLine($"window.{this.ClientID} = field;");
            st.WriteCodeLine($"Mini2.onwerPage.controls['{ID}'] = field; ");


        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                return;
            }

            EnsureChildControls();

            ScriptManager script = ScriptManager.GetManager(this.Page);

            writer.WriteLine("    <div id='{0}' style='display:none;'></div>", this.ClientID);

            string jsCode = string.Format("$('#{0}').val({1}.getJson())", m_Checked.ClientID, this.ClientID);
            m_Checked.SetAttribute("SubmitBufore", jsCode);

            m_Checked.RenderControl(writer);



            StringBuilder sb = new StringBuilder();


            BeginReady(sb);

            FullScript(sb);

            EndReady(sb);

            script.AddScript(sb.ToString());
        }

        public void LoadPostData()
        {

        }
    }
}
