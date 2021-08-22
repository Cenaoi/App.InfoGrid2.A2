using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{

    class _AC3TableIDX
    {
        public string Table { get; set; }

        List<ListenTable> m_Items = new List<ListenTable>();

        public List<ListenTable> Items
        {
            get { return m_Items; }
        }


    }


    class _AC3MethodIDX
    {
        public ListenMethod Method { get; set; }

        SortedList<string, _AC3TableIDX> m_Items = new SortedList<string, _AC3TableIDX>();


        public SortedList<string, _AC3TableIDX> Items
        {
            get { return m_Items; }
        }
    }

    class _AC3MethodTableIDX
    {
        SortedList<ListenMethod, _AC3MethodIDX> m_Items = new SortedList<ListenMethod, _AC3MethodIDX>();

        public SortedList<ListenMethod, _AC3MethodIDX> Items
        {
            get { return m_Items; }
        }


        internal void TryAdd(ListenTable listen)
        {
            _AC3MethodIDX methodIDX = null;
            _AC3TableIDX tableIDX = null;

            if (!m_Items.TryGetValue(listen.Method, out methodIDX))
            {
                methodIDX = new _AC3MethodIDX();
                methodIDX.Method = listen.Method;

                m_Items.Add(listen.Method, methodIDX);
            }

            if (!methodIDX.Items.TryGetValue(listen.Table, out tableIDX))
            {
                tableIDX = new _AC3TableIDX();
                tableIDX.Table = listen.Table;

                methodIDX.Items.Add(listen.Table, tableIDX);
            }

            tableIDX.Items.Add(listen);
        }

        internal List<ListenTable> TryGet(string table, ListenMethod method)
        {
            _AC3MethodIDX methodIDX = null;
            _AC3TableIDX tableIDX = null;

            if (m_Items.TryGetValue(method, out methodIDX))
            {
                if (methodIDX.Items.TryGetValue(table, out tableIDX))
                {
                    return tableIDX.Items;
                }
            }

            return null;
        }
    }


}
