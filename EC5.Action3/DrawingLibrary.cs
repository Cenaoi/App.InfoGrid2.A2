using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{
    /// <summary>
    /// 库管理器
    /// </summary>
    public static class LibraryManager
    {
        static ConcurrentDictionary<string, DrawingLibrary> m_Items = new ConcurrentDictionary<string, DrawingLibrary>();

        /// <summary>
        /// 尝试添加库
        /// </summary>
        /// <param name="lib"></param>
        /// <returns></returns>
        public static bool TryAdd(DrawingLibrary lib)
        {
            return m_Items.TryAdd(lib.Code, lib);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lib"></param>
        /// <returns></returns>
        public static bool TryRemove(string key, out DrawingLibrary lib)
        {
            return m_Items.TryRemove(key, out lib);
        }

        
        /// <summary>
        /// 清除全部项目
        /// </summary>
        public static void Clear()
        {
            m_Items.Clear();
        }


        public static DrawingLibrary GetDefault()
        {
            return m_Items["default"];
        }

    }

    /// <summary>
    /// 绘制库,存放各种版面
    /// </summary>
    public class DrawingLibrary :CodeIndexItem
    {
        /// <summary>
        /// 版面库
        /// </summary>
        ActionList<DrawingPanel> m_Panels = new ActionList<DrawingPanel>();

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Autohr { get; set; }

        public void Add(DrawingPanel panel)
        {
            m_Panels.Add(panel);
        }

        public int Count
        {
            get { return m_Panels.Count; }
        }

        public List<DrawingPanel> GetPanels()
        {
            return m_Panels.ToList();
        }




        /// <summary>
        /// 获取当前图纸库
        /// </summary>
        /// <param name="table"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public List<ListenTable> GetListenItems(string table, ListenMethod method)
        {
            List<ListenTable> listens = new List<ListenTable>();    //监听集合
            
            foreach (DrawingPanel panel in m_Panels.ToList())
            {
                if (!panel.Enabled)
                {
                    continue;
                }

                GetListenItems(panel, table, method, ref listens);
            }

            return listens;
        }


        private void GetListenItems(DrawingPanel panel, string table, ListenMethod method, ref List<ListenTable> listens)
        {
            List<ListenTable> listenList = panel.GetListenItems(table, method);

            if (listenList == null)
            {
                return;
            }

            listens.AddRange(listenList);
        }
    }
}
