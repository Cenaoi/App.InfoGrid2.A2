using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using EC5.Utility;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 查询按钮组
    /// </summary>
    public class SearchButtonGroup : Panel
    {
        public SearchButtonGroup()
            : base(300, 30)
        {
            this.PaddingLeft = 15;
            this.Scroll = ScrollBars.None;
            this.ItemMarginRight = "10px";

        }

        /// <summary>
        /// 按钮的高度, 默认26
        /// </summary>
        [Description("按钮的高度, 默认26")]
        [DefaultValue(26)]
        public int ButtonHeight { get; set; } = 26;


        /// <summary>
        /// 按钮的宽度, 默认 80
        /// </summary>
        [Description("按钮的宽度, 默认 80")]
        [DefaultValue(80)]
        public int ButtonWidth { get; set; } = 80;

        /// <summary>
        /// 提交按钮的样式
        /// </summary>
        [DefaultValue("")]
        public string SearchButtonClass { get; set; }

        /// <summary>
        /// 重置按钮的样式
        /// </summary>
        [DefaultValue("")]
        public string ResetButtonClass { get; set; } = "mi-btn-white";

        /// <summary>
        /// 点击查询按钮触发的 js事件
        /// </summary>
        [Description]
        public string SearchClick { get; set; }


        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            FormLayout formLayout = this.Parent as FormLayout;

            Button searchBtn = new Button();
            searchBtn.Text = "查询";
            searchBtn.Width = this.ButtonWidth;
            searchBtn.Height = this.ButtonHeight;
            searchBtn.Dock = DockStyle.Left;
            searchBtn.MarginRight = "10px";
            searchBtn.Class = this.SearchButtonClass;

            if (!StringUtil.IsBlank(this.SearchClick))
            {
                searchBtn.OnClick = this.SearchClick;
            }
            else if (formLayout != null && !StringUtil.IsBlank(formLayout.StoreID))
            {
                searchBtn.OnClick = "widget1.subMethod($('form:first'), {subName: '" + formLayout.StoreID + "', subMethod: 'Search'});";
            }


            Button resetBtn = new Button();
            resetBtn.Text = "清空";
            resetBtn.Width = this.ButtonWidth;
            resetBtn.Height = this.ButtonHeight;
            resetBtn.Dock = DockStyle.Left;
            resetBtn.Class = this.ResetButtonClass;

            resetBtn.OnClick = "widget1.subMethod($('form:first'), {subName: '" + formLayout.ID + "', subMethod: 'Reset'});";

            this.Controls.Add(searchBtn);
            this.Controls.Add(resetBtn);
        }


    }


    ////按钮版面
    //Panel btnPanel = new Panel(300, 26);
    //btnPanel.ID = "searchBtnPanel";
    //btnPanel.PaddingLeft = 105;
    //btnPanel.Scroll = ScrollBars.None;
    //btnPanel.ItemMarginRight = 10;

    //Button searchBtn = new Button();
    //searchBtn.Text = "查询";
    //searchBtn.OnClick = "widget1.subMethod($('form:first'), {subName: '" + this.StoreID + "', subMethod: 'Refresh'});";   //"ser:store1.Refresh()";
    //searchBtn.Width = 80;
    //searchBtn.Height = 26;
    //searchBtn.Dock = DockStyle.Left;



    //btnPanel.Controls.Add(searchBtn);

    //Button resetBtn = new Button();
    //resetBtn.Text = "清空";
    ////resetBtn.Command = "Reset";
    //resetBtn.OnClick = "widget1.subMethod($('form:first'), {subName: '" + searchForm.ID + "', subMethod: 'Reset'});";
    //resetBtn.Width = 80;
    //resetBtn.Height = 26;
    //resetBtn.Dock = DockStyle.Left;

    //btnPanel.Controls.Add(resetBtn);

    //cons.Add(btnPanel);

}
