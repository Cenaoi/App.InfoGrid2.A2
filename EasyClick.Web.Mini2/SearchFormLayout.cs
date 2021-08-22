using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 查询的表单框架
    /// </summary>
    public class SearchFormLayout:FormLayout
    {
        /// <summary>
        /// （构造函数）查询的表单框架
        /// </summary>
        public SearchFormLayout()
            : base()
        {
            //ItemWidth="300" ItemLabelAlign="Right"  FlowDirection="TopDown" ItemClass="mi-box-item" Layout="HBox"
            //StoreID="store1" FormMode="Filter" Scroll="None"

            //<mi:FormLayout runat="server" ID="searchForm" Dock="Top" Region="North" FlowDirection="TopDown"
            //ItemWidth="300" ItemLabelAlign="Right" ItemClass="mi-box-item" Layout="HBox"
            //StoreID="store1" FormMode="Filter" Scroll="None" Visible="False">


            this.ItemWidth = 300;
            this.ItemLabelAlign = TextAlign.Right;
            this.FlowDirection = Mini2.FlowDirection.LeftToRight;
            this.ItemClass = "mi-box-item";
            this.Layout = LayoutStyle.HBox;
            this.FormMode = FormLayoutMode.Filter;
            this.Scroll = ScrollBars.None;
            this.Region = RegionType.North;
            
            this.Padding = 5;
        }

        
        
    }


}
