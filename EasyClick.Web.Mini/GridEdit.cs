using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using EasyClick.Web.Mini.Utility;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{


    /// <summary>
    /// 可编辑控件（复杂型）
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public class GridEdit : GridBase
    {
        
    }


}
