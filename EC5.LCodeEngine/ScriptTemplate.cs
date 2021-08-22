using EC5.AppDomainPlugin;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.LCodeEngine
{



    /// <summary>
    /// 脚本模板
    /// </summary>
    public class ScriptTemplate
    {
        /// <summary>
        /// 源代码
        /// </summary>
        public string SrcCode { get; set; }

        /// <summary>
        /// 源代码的 Hash 码
        /// </summary>
        public int HashCode { get; set; }
        
        string m_CC_Code;

        string m_TypeName;

        private ScriptTemplate()
        {

        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object Exec(LModel model)
        {
            CSharpScript cs = new CSharpScript(m_CC_Code);
            cs.Compiler();


            DynModel dymLModel = new DynModel(model);

            object result = cs.ExecuteMethod(m_TypeName, "Exec", new object[] { model });

            return result;
        }

        public void  Test()
        {
            CSharpScript cs = new CSharpScript(m_CC_Code);
            cs.Compiler();
        }


        /// <summary>
        /// 解析代码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ScriptTemplate Parse(string code)
        {
            ScriptTemplate st = new ScriptTemplate();

            st.SrcCode = code;
            st.HashCode = code.GetHashCode();

            string namespaanName = "EC5_CSharpScript";
            string className;

            if (st.HashCode < 0)
            {
                className = "Script_F" + (-st.HashCode);
            }
            else
            {
                className = "Script_" + st.HashCode;
            }

            string codeStr = @"
                using System;
                using System.Text;
                using System.IO;
                using System.Collections.Generic;
                using System.Xml;
                using HWQ.Entity.LightModels;
                using EC5.Utility;
                using EC5.AppDomainPlugin;

                namespace " + namespaanName +
                @"{
                  public class " + className + @"
                  {


                    public object Exec(dynamic T)
                    {                        
                        " + code + @"
                    }

                  }
                }
              ";


            st.m_CC_Code = codeStr;
            st.m_TypeName = namespaanName + "." + className;


            return st;
        }
    }
}
