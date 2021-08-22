using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EasyClick.Web.Mini
{

    /// <summary>
    /// 轻量级脚本
    /// </summary>
    public class MiniScript
    {
        List<string> m_Scripts = new List<string>();

        class ScriptList : List<string>
        {
        }

        SortedList<string, ScriptList> m_Group;

        bool m_ReadOnly = false;

        string m_CurGroup;

        public bool ReadOnly
        {
            get
            {
                return m_ReadOnly;
            }
            set
            {
                m_ReadOnly = value;
            }
        }

        public static string[] GetForGroup(string groupName)
        {
            MiniScript ms = MiniScriptManager.ClientScript;

            if(ms.m_Group == null || !ms.m_Group.ContainsKey(groupName))
            {
                return new string[0];
            }

            ScriptList list = ms.m_Group[groupName];

            return list.ToArray();
        }

        public static void AddForGroup(string groupName, string script)
        {
            MiniScript ms = MiniScriptManager.ClientScript;

            if (ms.ReadOnly)
            {
                return;
            }

            if (ms.m_Group == null)
            {
                ms.m_Group = new SortedList<string, ScriptList>();
            }

            if (!ms.m_Group.ContainsKey(groupName))
            {
                ms.m_Group.Add(groupName, new ScriptList());
            }

            ms.m_Group[groupName].Add(script);
        }

        public static void AddForGroup(string groupName, string scriptFormat,params object[] args)
        {
            MiniScript ms = MiniScriptManager.ClientScript;

            if (ms.ReadOnly)
            {
                return;
            }

            if (ms.m_Group == null)
            {
                ms.m_Group = new SortedList<string, ScriptList>();
            }

            if (!ms.m_Group.ContainsKey(groupName))
            {
                ms.m_Group.Add(groupName, new ScriptList());
            }

            string script = string.Format(scriptFormat, args);

            ms.m_Group[groupName].Add(script);
        }



        /// <summary>
        /// 添加 JScript 
        /// </summary>
        /// <param name="script"></param>
        public static void Add(string script)
        {
            MiniScript ms = MiniScriptManager.ClientScript;

            if (ms.ReadOnly)
            {
                return;
            }


            if (!string.IsNullOrEmpty(ms.m_CurGroup))
            {
                if (ms.m_Group == null)
                {
                    ms.m_Group = new SortedList<string, ScriptList>();
                }

                if (!ms.m_Group.ContainsKey(ms.m_CurGroup))
                {
                    ms.m_Group.Add(ms.m_CurGroup, new ScriptList());
                }

                ms.m_Group[ms.m_CurGroup].Add(script);

            }
            else
            {
                ms.Items.Add(script);
            }
        }

        /// <summary>
        /// 添加 JScript 
        /// </summary>
        /// <param name="scriptFormat"></param>
        /// <param name="args"></param>
        public static void Add(string scriptFormat, params object[] args)
        {
            MiniScript ms = MiniScriptManager.ClientScript;

            if (ms.ReadOnly)
            {
                return;
            }

            string script = string.Format(scriptFormat, args);

            if (!string.IsNullOrEmpty(ms.m_CurGroup))
            {
                if (ms.m_Group == null)
                {
                    ms.m_Group = new SortedList<string, ScriptList>();
                }

                if (!ms.m_Group.ContainsKey(ms.m_CurGroup))
                {
                    ms.m_Group.Add(ms.m_CurGroup, new ScriptList());
                }

                ms.m_Group[ms.m_CurGroup].Add(script);
                
            }
            else
            {
                ms.Items.Add(script);
            }
        }

        public List<string> Items
        {
            get { return m_Scripts; }
        }

        /// <summary>
        /// 输出到网页面
        /// </summary>
        /// <param name="writer"></param>
        public void Write(TextWriter writer)
        {

            foreach (string item in m_Scripts)
            {
                writer.Write(item);

                if (!item.EndsWith(";"))
                {
                    writer.Write(";");
                }

                writer.WriteLine();
            }
        }

        public string ToString(string groupName)
        {
            if (m_Group == null || !m_Group.ContainsKey(groupName))
            {
                return string.Empty;
            }

            ScriptList scripts = m_Group[groupName];

            StringBuilder sb = new StringBuilder();

            foreach (string item in scripts)
            {
                sb.Append(item);

                if (!item.EndsWith(";"))
                {
                    sb.Append(";");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (string item in m_Scripts)
            {
                sb.Append(item);

                if (!item.EndsWith(";"))
                {
                    sb.Append(";");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public void BeginGroup(string groupName)
        {
            m_CurGroup = groupName;
        }

        public void EndGroup()
        {
            m_CurGroup = string.Empty;
        }
    }
}
