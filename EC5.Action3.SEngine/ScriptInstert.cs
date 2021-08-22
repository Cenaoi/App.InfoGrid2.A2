using HWQ.Entity.LightModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace EC5.Action3.SEngine
{
    public class ScriptFactory
    {

        protected static string GetClassName(int hashCode)
        {
            string name;

            if (hashCode < 0)
            {
                name = "Script_F" + (-hashCode);
            }
            else
            {
                name = "Script_" + hashCode;
            }

            return name;
        }

        /// <summary>
        /// 解析代码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ScriptInstance Create(string code)
        {
            ScriptInstance st = new ScriptInstance();

            st.SrcCode = code;
            st.HashCode = code.GetHashCode();

            string namespaanName = "EC5_CSharpScript";
            string className = GetClassName(st.HashCode);



            //string codeStr = @"
            //    using System;
            //    using System.Text;
            //    using System.IO;
            //    using System.Collections.Generic;
            //    using System.Xml;
            //    using HWQ.Entity.LightModels;
            //    using EC5.Utility;

            //    namespace " + namespaanName +
            //    @"{
            //      public class " + className + @"
            //      {


            //        public object Exec(dynamic T)
            //        {                        
            //            " + code + @"
            //        }

            //      }
            //    }
            //  ";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using HWQ.Entity.LightModels;
using EC5.Utility;
using EC5.WScript;

namespace " + namespaanName +
@"{
    public class " + className + ": EC5.WScript.CScriptBase" +
    @"{ ");

            //        public object Exec(dynamic T)
            //        {                        
            //            " + code + @"
            //        }

            //sb.AppendLine("public SortedList<string,dynamic> Params;");

            

            sb.AppendLine("        public object Exec(dynamic T,dynamic P)");
            sb.AppendLine("        {");

            sb.AppendLine("            " + code);

            sb.AppendLine("        }");

            sb.AppendLine("    }");
            sb.AppendLine("}");



            st.CC_Code = sb.ToString();
            st.TypeName = namespaanName + "." + className;


            return st;
        }
    }


    public class ScriptParam
    {
        public string Name { get; set; }

        public dynamic Value { get; set; }
    }

    public class ScriptParamCollection:SortedList<string,dynamic>
    {

    }


    /// <summary>
    /// 脚本工场
    /// </summary>
    public class ScriptInstance:IDisposable
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

        SortedList<string, dynamic> m_Params = new SortedList<string, dynamic>();

        public SortedList<string, dynamic> Params
        {
            get { return m_Params; }
        }

        internal string CC_Code
        {
            get { return m_CC_Code; }
            set { m_CC_Code = value; }
        }

        public string TypeName
        {
            get { return m_TypeName; }
            internal set { m_TypeName = value; }
        }

        internal ScriptInstance()
        {

        }


        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object Exec()
        {

            CSharpScript cs = new CSharpScript(m_CC_Code);
            cs.Compiler();

            SModel pDynModel = new SModel();

            foreach (var item in this.Params)
            {
                pDynModel[item.Key] = item.Value;
            }

            object result = cs.ExecuteMethod(m_TypeName, "Exec", new object[] { null, pDynModel });

            return result;
        }


        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object Exec(SModel model)
        {

            CSharpScript cs = new CSharpScript(m_CC_Code);
            cs.Compiler();


            object dymLModel = model;


            SModel pDynModel = new SModel();

            foreach (var item in this.Params)
            {
                pDynModel[item.Key] = item.Value;
            }

            object result = cs.ExecuteMethod(m_TypeName, "Exec", new object[] { dymLModel, pDynModel });

            return result;
        }


        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object Exec(LightModel model)
        {

            CSharpScript cs = new CSharpScript(m_CC_Code);
            cs.Compiler();


            DynModel dymLModel = new DynModel(model);

            DynModel pDynModel = new DynModel(new SModel());
            
            foreach (var item in this.Params)
            {
                pDynModel[item.Key] = item.Value;
            }
            
            object result = cs.ExecuteMethod(m_TypeName, "Exec", new object[] { dymLModel,pDynModel });

            return result;
        }


        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object Exec(IList models)
        {

            CSharpScript cs = new CSharpScript(m_CC_Code);
            cs.Compiler();

            
            DynModel pDynModel = new DynModel(new SModel());

            foreach (var item in this.Params)
            {
                pDynModel[item.Key] = item.Value;
            }

            object result = cs.ExecuteMethod(m_TypeName, "Exec", new object[] { models, pDynModel});

            return result;
        }

        /// <summary>
        /// 测试函数
        /// </summary>
        public void Test()
        {
            CSharpScript cs = new CSharpScript(m_CC_Code);
            cs.Compiler();
        }

        bool m_IsDisposed = false;

        /// <summary>
        /// 释放内存
        /// </summary>
        public void Dispose()
        {
            if (!m_IsDisposed)
            {
                m_IsDisposed = true;

                if (m_Params != null)
                {
                    m_Params.Clear();
                    m_Params = null;
                }

                GC.SuppressFinalize(this);
            }    
        }


    }
}
