using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.AppDomainPlugin
{
    public class DymAssemblyBuffer
    {
        ConcurrentDictionary<int, DymAssembly> m_Items = new ConcurrentDictionary<int, DymAssembly>();

        public ConcurrentDictionary<int,DymAssembly> Items
        {
            get { return m_Items; }
        }
    }

    public static class DymAssemblyHelper
    {
        static DymAssemblyBuffer m_Buffer = new DymAssemblyBuffer();

        public static DymAssemblyBuffer Buffer
        {
            get { return m_Buffer; }
        }

        
    }
}
