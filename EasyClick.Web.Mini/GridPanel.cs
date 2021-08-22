using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Security.Permissions;

namespace EasyClick.Web.Mini
{

    /// <summary>
    /// 表格版面
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public class GridPanel : Control
    {

        public GridPanel()
        {

        }

        /// <summary>
        /// 工具栏
        /// </summary>
        ToolBar m_Toolbar;

        /// <summary>
        /// 表格组件
        /// </summary>
        GridBase m_Grid;

        /// <summary>
        /// 分页组件
        /// </summary>
        Pagination m_Pagination;

        /// <summary>
        /// 数据仓库
        /// </summary>
        DataStoreControl m_DataStore;


        bool m_InitCreateChild = false;

        protected override void CreateChildControls()
        {

            if (m_InitCreateChild) { return; }

            m_InitCreateChild = true;

            //表格控件
            m_Grid = new GridBase();
            m_Grid.ID = this.ID + "_Grid";
            this.Controls.Add(m_Grid);



            //分页栏控件
            m_Pagination.ID = this.ID + "_Page";
            m_Pagination.IsAjax = true;
            m_Pagination.ClassName = "flickr";

            this.Controls.Add(m_Pagination);


            //数据仓库
            m_DataStore = DataStoreManager.GetDefaultStore();
            m_DataStore.ID = this.ID + "_DataStore";
            this.Controls.Add(m_DataStore);


            //连接表格和数据仓库
            m_Grid.DataStore = m_DataStore;


            //工具栏
            m_Toolbar = new ToolBar();
            m_Toolbar.ID = this.ID + "_Toolbar";
            this.Controls.Add(m_Toolbar);

        }

        protected override void Render(HtmlTextWriter writer)
        {
            


        }

    }

}
