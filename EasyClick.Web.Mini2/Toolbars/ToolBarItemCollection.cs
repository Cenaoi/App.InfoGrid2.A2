using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Permissions;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 工具栏-元素控件集合
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ToolBarItemCollection : List<ToolBarItem>
    {
        SortedList<string, ToolBarItem> m_IdIndexs = null;

        private void CreateIndex()
        {
            if (m_IdIndexs != null)
            {
                return;
            }

            m_IdIndexs = new SortedList<string, ToolBarItem>();

            foreach (var item in this)
            {
                if (string.IsNullOrEmpty(item.ID))
                {
                    continue;
                }

                m_IdIndexs[item.ID] = item;
            }
        }

        /// <summary>
        /// 查找按钮
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ToolBarItem Find(string id)
        {
            CreateIndex();

            ToolBarItem item = null;

            m_IdIndexs.TryGetValue(id, out item);

            return item;
        }

        public ToolBarItem this[string id]
        {
            get
            {
                return Find(id);
            }
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        public ToolBarItem Add(string text)
        {
            if (text == "-")
            {
                ToolBarHr hr = new ToolBarHr();
                base.Add(hr);

                return hr;
            }
            else
            {
                ToolBarButton btn = new ToolBarButton(text);

                base.Add(btn);

                return btn;
            }
        }
    }

}
