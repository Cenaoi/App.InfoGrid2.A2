using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EC5.Action3.SEngine
{


    /// <summary>
    /// C# 脚本编译器
    /// </summary>
    public class CSharpScript : IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string AppPathBase { get; set; } = "/";

        static string[] DEFAULT_IMPORT_LIST = new string[]
        {
            "System.dll",
            "System.Core.dll",
            "System.Data.dll",
            "System.Xml.dll"
        };

        #region 构造函数

        /// <summary>
        /// C# 脚本编译器
        /// </summary>
        public CSharpScript()
        {

        }

        /// <summary>
        /// C# 脚本编译器
        /// </summary>
        /// <param name="code">脚本代码</param>
        public CSharpScript(string code)
        {
            this.Code = code;
        }


        #endregion


        /// <summary>
        /// 用户参数
        /// </summary>
        SortedList<string, dynamic> m_UserParams = new SortedList<string, dynamic>();

        /// <summary>
        /// 全局字符串参数
        /// </summary>
        SortedList<string, string> m_GlobelStringParams = new SortedList<string, string>();

        /// <summary>
        /// 引用的类库
        /// </summary>
        List<string> m_ImportList = new List<string>();

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }


        DymAssembly m_DymAssembly;


        /// <summary>
        /// 用户参数
        /// </summary>
        public SortedList<string, dynamic> UserParams
        {
            get { return m_UserParams; }
        }



        /// <summary>
        /// 编译代码
        /// </summary>
        public void Compiler()
        {
            int codeHash = this.Code.GetHashCode();

            DymAssemblyBuffer buffer = DymAssemblyHelper.Buffer;

            if (buffer.Items.TryGetValue(codeHash, out m_DymAssembly))
            {
                return;
            }


            // 创建编译器对象
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();

            // 设置编译参数
            CompilerParameters options = new CompilerParameters();

            

            foreach (var assDll in DEFAULT_IMPORT_LIST)
            {
                options.ReferencedAssemblies.Add(assDll);
            }

            

           
            options.ReferencedAssemblies.Add( "Microsoft.CSharp.dll");

            string[] userDlls = new string[]
            {
                "log4net.dll",
                "EC5.Utility.dll",
                "HWQ.Entity4.dll",
                "EC5.WScript.dll"
            };

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            foreach (var dllName in userDlls)
            {
                string basePath = baseDir + dllName;

                if (File.Exists(basePath))
                {
                   options.ReferencedAssemblies.Add(basePath);
                }
                else if(File.Exists(baseDir + "bin\\" + dllName))
                {
                    options.ReferencedAssemblies.Add(baseDir + "bin\\" + dllName);
                }
            }




            options.GenerateExecutable = false;
            options.GenerateInMemory = true;
            //options.OutputAssembly = "MyTest";

            // 开始编译
            CodeSnippetCompileUnit compileUnit = new CodeSnippetCompileUnit(this.Code);

            CompilerResults results = codeProvider.CompileAssemblyFromDom(options, compileUnit);


            if (results.Errors.Count > 0)
            {
                throw new Exception(results.Errors[0].ErrorText);
            }


            DymAssembly dymAss = new DymAssembly(codeHash, results.CompiledAssembly);

            if (!buffer.Items.TryAdd(codeHash, dymAss))
            {
                throw new Exception("编译后，添加到啥失败了。。");
            }

            this.m_DymAssembly = dymAss;

        }


        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="methodName">函数名</param>
        /// <param name="parameters">函数的参数</param>
        /// <returns></returns>
        public object ExecuteMethod(string typeName, string methodName, object[] parameters)
        {
            if(m_DymAssembly == null)
            {
                this.Compiler();
            }

            if (this.m_DymAssembly == null)
            {
                throw new Exception("Assembly 无法动态编译。" + typeName);
            }

            Type type ;
            MethodInfo mi;

            object obj;
            object result;

            
            type = this.m_DymAssembly.Assembly.GetType(typeName);

            if(type == null)
            {
                throw new Exception($"动态对象 Type=\"{typeName}\" 对象不存在.");
            }

            mi = type.GetMethod(methodName);

            try
            {
                obj = Activator.CreateInstance(type);
            }
            catch(Exception ex)
            {
                throw new Exception($"动态对象 Type=\"{typeName}\" 实例化错误.", ex);
            }

            try
            {
                result = mi.Invoke(obj, parameters);
            }
            catch(Exception ex)
            {
                throw new Exception($"动态对象 Type=\"{typeName}\" 执行函数 \"{methodName}\" 错误.", ex);
            }

            if (obj is IDisposable)
            {
                ((IDisposable)obj).Dispose();
            }

            return result;
        }


        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public object ExecuteMothod(string typeName, string methodName)
        {
            return ExecuteMethod(typeName, methodName, null);
        }





        bool m_IsDisposed = false;

        /// <summary>
        /// 释放内存
        /// </summary>
        public void Dispose()
        {
            if (!m_IsDisposed)
            {
                m_DymAssembly = null;


                m_GlobelStringParams = null;
                m_ImportList = null;
                m_UserParams = null;


                m_IsDisposed = true;
            }

        }

    }
}
