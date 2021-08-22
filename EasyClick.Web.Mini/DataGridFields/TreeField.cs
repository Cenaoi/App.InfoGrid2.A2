using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;

namespace EasyClick.Web.Mini
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class TreeField : BoundField
    {
        /// <summary>
        /// 深度字段
        /// </summary>
        string m_DepthField;

        int m_RootDepth = 0;

        [DefaultValue(0)]
        public int RootDepth
        {
            get { return m_RootDepth; }
            set { m_RootDepth = value; }
        }

        /// <summary>
        /// 深度字段
        /// </summary>
        public string DepthField
        {
            get { return m_DepthField; }
            set { m_DepthField = value; }
        }

        public override string CreateHtmlTemplate(MiniDataControlCellType cellType, MiniDataControlRowState rowState)
        {
            if (cellType == MiniDataControlCellType.Header)
            {
                return string.Format("<th>{0}</th>", this.HeaderText);
            }
            else
            {
                if (string.IsNullOrEmpty(this.DepthField))
                {
                    return string.Format("<td>{0}&nbsp;{{$T.{1}}}</td>", string.Empty, this.DataField);
                }
                else
                {
                    string depthCode = "{#for index = " + m_RootDepth + " to $T." + this.DepthField + "}……{#/for}";
                    return string.Format("<td>{0}&nbsp;{{$T.{1}}}</td>", depthCode, this.DataField);
                }
            }
        }
    }
}
