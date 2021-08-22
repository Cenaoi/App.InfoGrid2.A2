using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.InfoGrid2.WF.View
{
    public partial class Success : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            byte[] bytes = Encoding.UTF8.GetBytes("/WF/View/Home.aspx");
           string value =  Convert.ToBase64String(bytes);


            //"ztKwrsTj"是“我爱你”的base64编码
            byte[] outputb = Convert.FromBase64String(value);
            string orgStr = Encoding.Default.GetString(outputb);


        }
    }
}