using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using EasyClick.Web.Mini;
using EC5.Utility;
using System.Web.UI;

namespace EasyClick.Web.Mini2
{
   

    /// <summary>
    /// 表单版面
    /// </summary>
    [Description("表单版面")]
    public class FormLayout:Panel
    {
        /// <summary>
        /// 表单版面
        /// </summary>
        public FormLayout()
            : base()
        {
            this.Layout = LayoutStyle.Form;
            this.JsNamespace = "Mini2.ui.panel.FormPanel";
            this.InReady = "Mini2.ui.panel.FormPanel";
            this.ItemClass = "mi-anchor-form-item";

            this.Padding = 5;

            this.FlowDirection = FlowDirection.TopDown;
        }


        /// <summary>
        /// 重置事件
        /// </summary>
        public event EventHandler Reseted;

        /// <summary>
        /// 出发重置事件
        /// </summary>
        protected void OnReseted()
        {
            Reseted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 子项目在过滤过程中,触发的事件
        /// </summary>
        public event EventHandler<FormLayoutItemEventArgs> ItemFiltering;

        /// <summary>
        /// 子项目在过滤过程中,触发的事件
        /// </summary>
        /// <param name="item">子控件</param>
        /// <param name="cancel">是否取消这个控件</param>
        protected void OnItemFiltering(Control item,out bool cancel)
        {
            cancel = false;

            if (ItemFiltering != null)
            {
                FormLayoutItemEventArgs ea = new FormLayoutItemEventArgs(item);

                ItemFiltering(this, ea);

                cancel = ea.Cancel;

                return;
            }

        }
             
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            StoreBind();
        }



        /// <summary>
        /// 保存激活,配合数据仓库
        /// </summary>
        bool m_SaveEnabled = false;

        /// <summary>
        /// 表单模式
        /// </summary>
        FormLayoutMode m_FormMode = FormLayoutMode.Auto;

        /// <summary>
        /// 控件布局方向
        /// </summary>
        [Description("控件布局方向")]
        [DefaultValue(FlowDirection.TopDown)]
        public override FlowDirection FlowDirection
        {
            get { return base.FlowDirection; }
            set { base.FlowDirection = value; }
        }

        /// <summary>
        /// 保存激活.默认 false
        /// </summary>
        [Description("保存激活.配合数据仓库使用.默认 false")]
        [DefaultValue(false)]
        public bool SaveEnabled
        {
            get { return m_SaveEnabled; }
            set { m_SaveEnabled = value; }
        }

        /// <summary>
        /// 表单模式
        /// </summary>
        [Description("表单模式")]
        [DefaultValue(FormLayoutMode.Auto)]
        public FormLayoutMode FormMode
        {
            get { return m_FormMode; }
            set { m_FormMode = value; }
        }

        /// <summary>
        /// 绑定数据仓库
        /// </summary>
        private void StoreBind()
        {
            m_Store = GetStoreByPage(m_StoreID);

            if (m_Store == null)
            {
                return;
            }

            m_Store.Updating += m_Store_Updating;
            m_Store.Filtering += new ObjectCancelEventHandler(m_Store_Filtering);
            m_Store.CurrentChanged += new ObjectEventHandler(m_Store_CurrentChanged);

        }


        void m_Store_Updating(object sender, ObjectCancelEventArgs e)
        {
            if (!m_SaveEnabled ||m_FormMode == FormLayoutMode.Filter )
            {
                return;
            }

            FormLayoutHelper.EditData(this, e.Object);

        }

        

        void m_Store_CurrentChanged(object sender, ObjectEventArgs e)
        {
            if (m_FormMode != FormLayoutMode.Auto)
            {
                return;
            }
            
            FormLayoutHelper.SetData(this,m_Store.Model, e.Object);
            
        }


        /// <summary>
        /// 重新设定
        /// </summary>
        public void Reset()
        {
            foreach (System.Web.UI.Control item in this.Controls)
            {
                FieldBase field = item as FieldBase;
                
                if (field == null )
                {
                    continue;
                }

                field.Reset();
            }

            OnReseted();
        }



        /// <summary>
        /// 查找字段的控件
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        private List<FieldBase> FindFieldControls(FormLayout layout)
        {
            List<FieldBase> cons = new List<FieldBase>();

            foreach (System.Web.UI.Control item in layout.Controls)
            {
                FieldBase field = item as FieldBase;
                
                if (field == null || StringUtil.IsBlank(field.DataField))
                {
                    continue;
                }

                cons.Add(field);
            }

            return cons;
        }


        /// <summary>
        /// 处理间距
        /// </summary>
        /// <param name="store"></param>
        /// <param name="field"></param>
        /// <param name="rangeCon"></param>
        private void Store_Filtering_IRangeControl(Store store, FieldBase field, IRangeControl rangeCon)
        {
            //rangeCon = (IRangeControl)field;

            if (!StringUtil.IsBlank(rangeCon.StartValue))
            {

                Param p = new Param(field.DataField, rangeCon.StartValue);

                p.Logic = ">=";

                store.FilterParams.Add(p);
            }

            if (!StringUtil.IsBlank(rangeCon.EndValue))
            {
                DateTime endDate;

                if (DateTime.TryParse(rangeCon.EndValue, out endDate))
                {
                    Param p = new Param(field.DataField);
                    
                    p.SetInnerValue(endDate.Date.Add(new TimeSpan(0,23,59,59,998)));

                    p.Logic = "<=";

                    store.FilterParams.Add(p);
                }
                else
                {
                    Param p = new Param(field.DataField, rangeCon.EndValue);

                    p.Logic = "<=";

                    store.FilterParams.Add(p);
                }
            }
        }


        /// <summary>
        /// 处理普通控件
        /// </summary>
        /// <param name="store"></param>
        /// <param name="field"></param>
        private void Store_Filtering_Item(Store store, FieldBase field)
        {
            string fieldValue = field.Value;

            if (StringUtil.IsBlank(fieldValue))
            {
                return;
            }


            if (StringUtil.StartsWith(fieldValue, "query:"))
            {
                if (fieldValue.Length > 6)
                {
                    string fV = fieldValue.Substring(6).Trim();

                    if (!StringUtil.IsBlank(fV))
                    {
                        TSqlWhereParam p = new TSqlWhereParam(fV);

                        store.FilterParams.Add(p);
                    }
                }

                return;
            }



            string[] values = StringUtil.Split(fieldValue, " ");

            if (values.Length == 1)
            {
                //一个关键字处理.

                if (StringUtil.StartsWith(fieldValue, "-"))
                {
                    fieldValue = fieldValue.Substring(1);

                    Param p = new Param(field.DataField, fieldValue);

                    if ("like".Equals(field.DataLogic, StringComparison.OrdinalIgnoreCase))
                    {
                        p.Logic = "not like";
                    }
                    else
                    {
                        p.Logic = StringUtil.NoBlank(field.DataLogic, "=");
                    }

                    store.FilterParams.Add(p);
                }
                else
                {
                    Param p = new Param(field.DataField, fieldValue);
                    p.Logic = StringUtil.NoBlank(field.DataLogic, "=");

                    store.FilterParams.Add(p);
                }
            }
            else
            {

                if (field.DataType == DataType.Number)
                {
                    //多个关键字的情况下，做特殊处理

                    List<decimal> valueList = new List<decimal>();

                    foreach (var v in values)
                    {
                        decimal valueD;

                        if (decimal.TryParse(v, out valueD))
                        {
                            valueList.Add(valueD);
                        }
                    }

                    Param p = new Param(field.DataField);
                    p.SetInnerValue(valueList);
                    p.Logic = "in";

                    store.FilterParams.Add(p);

                }
                else
                {
                    StringBuilder whereSb = new StringBuilder();


                    //多个关键字的情况下，做特殊处理

                    //区分 like 和 nolike 的数据
                    List<string> noLikeValues = new List<string>();
                    List<string> likeValues = new List<string>();

                    foreach (string v in values)
                    {
                        if (StringUtil.StartsWith(v, "-"))
                        {
                            if (v.Length == 1)
                            {
                                continue;
                            }

                            string vText = v.Substring(1);

                            noLikeValues.Add(vText);
                        }
                        else
                        {
                            likeValues.Add(v);
                        }
                    }


                    if (field.DataLogic == "=" || field.DataLogic == "in")
                    {
                        Param psIn = new Param(field.DataField);
                        psIn.Logic = "in";
                        psIn.SetInnerValue(likeValues);
                        store.FilterParams.Add(psIn);

                        if (noLikeValues.Count > 0)
                        {
                            Param psNoin = new Param(field.DataField);
                            psNoin.Logic = "not in";
                            psNoin.SetInnerValue(noLikeValues);
                            store.FilterParams.Add(psNoin);
                        }

                    }
                    else
                    {

                        int n = 0;


                        if (noLikeValues.Count > 0)
                        {
                            whereSb.Append("(");

                            foreach (var v in noLikeValues)
                            {
                                string key = v.Replace("'", "''");


                                if (!StringUtil.IsBlank(field.DataLikeFormat))
                                {
                                    key = string.Format(field.DataLikeFormat, key);
                                }
                                else
                                {
                                    if (!key.Contains("%"))
                                    {
                                        key = "%" + key + "%";
                                    }
                                }



                                if (n++ > 0) { whereSb.Append(" AND "); }


                                whereSb.Append($"({field.DataField} not like '{key}')");

                            }

                            whereSb.Append(")");
                        }


                        if (likeValues.Count > 0)
                        {
                            if (noLikeValues.Count > 0)
                            {
                                whereSb.Append(" AND ");
                            }

                            n = 0;

                            whereSb.Append("(");

                            foreach (var v in likeValues)
                            {
                                string key = v.Replace("'", "''");


                                if (!StringUtil.IsBlank(field.DataLikeFormat))
                                {
                                    key = string.Format(field.DataLikeFormat, key);
                                }
                                else
                                {
                                    if (!key.Contains("%"))
                                    {
                                        key = "%" + key + "%";
                                    }
                                }


                                if (n++ > 0) { whereSb.Append(" OR "); }

                                whereSb.Append($"({field.DataField} like '{key}')");
                            }
                            whereSb.Append(")");
                        }

                        TSqlWhereParam p = new TSqlWhereParam(whereSb.ToString());
                        store.FilterParams.Add(p);
                    }

                }
            }
        }


        /// <summary>
        /// 数据仓库过滤字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Store_Filtering(object sender, ObjectCancelEventArgs e)
        {
            if (m_FormMode != FormLayoutMode.Filter)
            {
                return;
            }

            IRangeControl rangeCon;

            List<FieldBase> cons = FindFieldControls(this);

            bool cancel = false;

            Store store = (Store)sender;

            foreach (FieldBase field in cons)
            {
                OnItemFiltering(field, out cancel);

                if(cancel)
                {
                    continue;
                }

                if (field is IRangeControl)
                {
                    Store_Filtering_IRangeControl(store, field, (IRangeControl)field);
                }
                else
                {
                    Store_Filtering_Item(store, field);
                }
            }
        }



        Store m_Store;

        TextAlign m_ItemLabelAlign = TextAlign.Left;

        int m_ItemLabelWidth = 0;

        /// <summary>
        /// 项目宽度
        /// </summary>
        int m_ItemWidth = 0;



        string m_StoreID;

        /// <summary>
        /// 数据仓库ID
        /// </summary>
        [Description("数据仓库ID")]
        [DefaultValue("")]
        public string StoreID
        {
            get { return m_StoreID; }
            set { m_StoreID = value; }
        }



        /// <summary>
        /// 布局样式
        /// </summary>
        [DefaultValue(LayoutStyle.Form)]
        [Description("布局样式")]
        [Browsable(true)]
        public new LayoutStyle Layout
        {
            get { return base.Layout; }
            set { base.Layout = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(TextAlign.Left)]
        [Category("Labelabel")]
        public TextAlign ItemLabelAlign
        {
            get { return m_ItemLabelAlign; }
            set { m_ItemLabelAlign = value; }
        }

        /// <summary>
        /// 子控件标题宽度
        /// </summary>
        [DefaultValue(0)]
        [Category("Labelabel")]
        [Description("子控件标题宽度")]
        public int ItemLabelWidth
        {
            get { return m_ItemLabelWidth; }
            set { m_ItemLabelWidth = value; }
        }

        /// <summary>
        /// 子控件的宽度
        /// </summary>
        [Category("Item")]
        [DefaultValue(0)]
        [Description("子控件的宽度")]
        public virtual int ItemWidth
        {
            get { return m_ItemWidth; }
            set { m_ItemWidth = value; }
        }

        /// <summary>
        /// 获取当前控件中的 StoreID 对应的唯一ID
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        private Store GetStoreByPage(string storeId)
        {
            if (string.IsNullOrEmpty(storeId))
            {
                return null;
            }

            System.Web.UI.Control con = this.Parent.FindControl(storeId);

            if (con == null)
            {
                con = this.Page.FindControl(storeId);
            }

            if (con == null)
            {
                return null;
            }

            return con as Store;
        }


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

        /// <summary>
        /// 填充脚本
        /// </summary>
        /// <param name="sb"></param>
        protected override void FullScript(StringBuilder sb)
        {
            string clientId = this.ClientID;

            ScriptTextWriter st = new ScriptTextWriter(sb, QuotationMarkConvertor.SingleQuotes);


            st.RetractBengin("var field = Mini2.create('" + this.JsNamespace + "', {");
            {
                st.WriteParam("id", this.ID);
                st.WriteParam("clientId", clientId);


                FullScript_Layout(st);
                FullScript_ItemMargin(st);
                FullScript_Padding(st);


                string globelStoreId = GetStoreIdByPage(m_StoreID);

                st.WriteParam("store", globelStoreId);

                st.WriteParam("autoSize", this.AutoSize, false);

                st.WriteParam("userCls", this.CssClass);

                st.WriteParam("width", this.Width.ToString());
                st.WriteParam("height", this.Height.ToString());


                st.WriteParam("minWidth", this.MinWidth);
                st.WriteParam("minHeight", this.MinHeight);

                st.WriteParam("maxWidth", this.MaxWidth);
                st.WriteParam("maxHeight", this.MaxHeight);

                st.WriteParam("margin", this.Margin);
                st.WriteParam("marginLeft", this.MarginLeft);
                st.WriteParam("marginRight", this.MarginRight);
                st.WriteParam("marginTop", this.MarginTop);
                st.WriteParam("marginBottom", this.MarginBottom);


                st.WriteParam("position", this.Position, StylePosition.Static, TextTransform.Lower);

                st.WriteParam("left", this.Left);
                st.WriteParam("top", this.Top);
                st.WriteParam("right", this.Right);
                st.WriteParam("bottom", this.Bottom);



                st.WriteParam("itemWidth", this.ItemWidth);
                st.WriteParam("itemLabelWidth", this.ItemLabelWidth);
                st.WriteParam("itemLabelAlign", this.ItemLabelAlign, TextAlign.Left, TextTransform.Lower);

                st.WriteParam("flowDirection", this.FlowDirection, FlowDirection.TopDown, TextTransform.Lower);
                st.WriteParam("wrapContents", this.WrapContents, true);

                st.WriteParam("visible", this.Visible, true);

                st.WriteParam("formMode", this.FormMode, FormLayoutMode.Auto, TextTransform.Lower);

                st.WriteParam("scroll", this.Scroll, ScrollBars.Auto, TextTransform.Lower);
                st.WriteParam("ui", this.Ui, UiStyle.Default, TextTransform.Lower);
                st.WriteParam("dock", this.Dock, TextTransform.Lower);
                st.WriteParam("region", this.Region, TextTransform.Lower);

                st.WriteParam("contentEl", "#" + clientId);

                st.WriteLine();
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

            st.WriteCodeLine($"window.{clientId} = field;");
            st.WriteCodeLine($"Mini2.onwerPage.controls['{ID}'] = field; ");


        }
    }
}
