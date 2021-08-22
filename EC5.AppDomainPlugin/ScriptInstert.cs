using HWQ.Entity.LightModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace EC5.AppDomainPlugin
{
    /// <summary>
    /// 脚本工场
    /// </summary>
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
using EC5.AppDomainPlugin;
using EC5.WScript;

namespace " + namespaanName +
@"{
    public class " + className + @": EC5.WScript.CScriptBase
    {");

            //        public object Exec(dynamic T)
            //        {                        
            //            " + code + @"
            //        }

            //sb.AppendLine("public SortedList<string,dynamic> Params;");


            /**
             * T 当前对象, R 资源 , P 参数集合
             */

            sb.AppendLine("        public object Exec(dynamic T,dynamic R, dynamic P)");
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

    /// <summary>
    /// 脚本参数
    /// </summary>
    public class ScriptParam
    {
        public string Name { get; set; }

        public dynamic Value { get; set; }
    }

    /// <summary>
    /// 脚本参数集合
    /// </summary>
    public class ScriptParamCollection:SortedList<string,dynamic>
    {

    }


    /// <summary>
    /// 脚本实例对象
    /// </summary>
    public class ScriptInstance
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
        /// <returns></returns>
        public object Exec()
        {

            CSharpScript cs = new CSharpScript(m_CC_Code);
            cs.Compiler();

            foreach (var item in m_Params)
            {
                cs.UserParams.Add(item.Key, item.Value);
            }

            DynModel pDynModel = new DynModel(new SModel());

            foreach (var item in this.Params)
            {
                pDynModel[item.Key] = item.Value;
            }

            object result = cs.ExecuteMethod(m_TypeName, "Exec", new object[] { null, null, pDynModel});

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


            SModel pDynModel = new SModel();

            foreach (var item in this.Params)
            {
                pDynModel[item.Key] = item.Value;
            }

            object result = cs.ExecuteMethod(m_TypeName, "Exec", new object[] { model, null, pDynModel });

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

            SModel pDynModel = new SModel();
            
            foreach (var item in this.Params)
            {
                pDynModel[item.Key] = item.Value;
            }
            
            object result = cs.ExecuteMethod(m_TypeName, "Exec", new object[] { model, null, pDynModel});

            return result;
        }



        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public object Exec(IList models)
        {

            CSharpScript cs = new CSharpScript(m_CC_Code);
            cs.Compiler();

            

            List<dynamic> dymList = new List<dynamic>();

            foreach (var model in models)
            {
                DynModel dymLModel = new DynModel(model);

                dymList.Add(dymLModel);
            }

            

            DynModel pDynModel = new DynModel(new SModel());

            foreach (var item in this.Params)
            {
                pDynModel[item.Key] = item.Value;
            }

            object result = cs.ExecuteMethod(m_TypeName, "Exec", new object[] { dymList, null, pDynModel });

            return result;
        }

        public void Test()
        {
            CSharpScript cs = new CSharpScript(m_CC_Code);
            cs.Compiler();
        }


    }
}
