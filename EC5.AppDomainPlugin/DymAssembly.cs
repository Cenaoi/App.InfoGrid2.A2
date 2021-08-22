using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EC5.AppDomainPlugin
{
    public class DymAssembly
    {

        /// <summary>
        /// 编译后的程序集
        /// </summary>
        public Assembly Assembly { get; set; }

        public string Code { get; set; }

        public int CodeHash { get; set; }

        public DymAssembly()
        {

        }

        public DymAssembly(int codeHash, Assembly assembly)
        {
            this.CodeHash = codeHash;
            this.Assembly = assembly;
        }


    }
}
