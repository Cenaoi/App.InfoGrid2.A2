using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HWQ.Entity.Filter;

namespace View
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ObjectDataSource1_Filtering(object sender, ObjectDataSourceFilteringEventArgs e)
        {
            DataTable tab = new DataTable();

            tab.BeginLoadData();

            tab.EndLoadData();
        }

        protected void ObjectDataSource1_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
        {

            

            
        }

        protected void ObjectDataSource1_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
        
        }

        protected void SqlDataSource1_Filtering(object sender, SqlDataSourceFilteringEventArgs e)
        {
            
        }

        protected void ObjectDataSource1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            

        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            
        }

        protected void AccessDataSource1_Filtering(object sender, SqlDataSourceFilteringEventArgs e)
        {
            
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}