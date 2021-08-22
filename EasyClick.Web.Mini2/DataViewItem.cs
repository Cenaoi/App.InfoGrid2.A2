using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 
    /// </summary>
    public class DataViewItem:Control
    {

        /// <summary>
        /// 脚本集合
        /// </summary>
        List<string> m_ScriptList = new List<string>();

        /// <summary>
        /// 渲染的 html
        /// </summary>
        public string RenderTemplate { get; set; }


        /// <summary>
        /// 添加脚本
        /// </summary>
        /// <param name="script">脚本</param>
        public void AddScript(string script)
        {
            m_ScriptList.Add(script);
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            foreach (var item in this.Controls)
            {
                FieldBase fb = item as FieldBase;

                if (fb != null)
                {
                    fb.SubItemMode = true;
                    fb.SubScript = m_ScriptList;
                }
            }

            base.RenderControl(writer);
        }

        /// <summary>
        /// 输出所有脚本
        /// </summary>
        /// <returns></returns>
        public string ToScriptAll()
        {
            if(m_ScriptList == null || m_ScriptList.Count == 0) { return null; }

            StringBuilder sb = new StringBuilder();

            foreach (var item in m_ScriptList)
            {
                sb.AppendLine(item);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 存在脚本
        /// </summary>
        /// <returns></returns>
        public bool HasScript()
        {
            return m_ScriptList != null && m_ScriptList.Count > 0;
        }
    }
}
