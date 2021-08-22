using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;

namespace EasyClick.BizWeb2
{
    /// <summary>
    /// 业务页面
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [PersistChildren(false), ParseChildren(true)]
    public class BizPage:System.Web.UI.Control
    {
        /// <summary>
        /// 页面信息
        /// </summary>
        public BizPage()
        {

        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public string Height { get; set; }


        /// <summary>
        /// 页面的业务逻辑
        /// </summary>
        /// <example>例: [{'F1':1,'F2':2},{...},...]</example>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string Path { get; set; }

        /// <summary>
        /// 菜单路径, 如果不填写, 跟页面的业务逻辑相同
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string MenuPath { get; set; }



    }
}
