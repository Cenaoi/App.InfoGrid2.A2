using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace EC5.IG2.Core.DymCSharpFile
{
    /// <summary>
    /// C# 文件动态加载器
    /// </summary>
    public class CSharpFileLoader
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// C# 文件动态加载器（构造函数）
        /// </summary>
        public CSharpFileLoader()
        {

        }

        /// <summary>
        /// 编译后的 Type 对行啊
        /// </summary>
        List<Type> m_TypeList = new List<Type>();


        List<string> m_SystemReferenceedAssembilies = new List<string>()
        {
            "System.dll",
            "System.Xml.dll",
            "System.Data.dll",
            "System.Drawing.dll",
            "System.Web.dll"
        };

        /// <summary>
        /// 引用对象
        /// </summary>
        List<string> m_ReferenceedAssembilies = new List<string>(){
            

            @"\bin\log4net.dll",
            @"\bin\EC5.SystemBoard.dll",
            @"\bin\EC5.SystemBoard.Web.dll",
            @"\bin\EC5.Utility.dll",
            @"\bin\EC5.Utility.Web.dll",
            @"\bin\EC5.XScriptEngine.dll",
            @"\bin\Newtonsoft.Json.Net20.dll",
            @"\bin\NPOI.dll",
            @"\bin\App.BizCommon.dll",
            @"\bin\HWQ.Entity4.dll",
            @"\bin\App.InfoGrid2.Model.dll",
            @"\bin\EasyClick.BizWeb.dll",
            @"\bin\EasyClick.BizWeb2.dll",
            @"\bin\EasyClick.Web.Mini.dll",
            @"\bin\EasyClick.Web.Mini2.Data.dll",
            @"\bin\EasyClick.Web.Mini2.dll",
            @"\bin\EasyClick.Web.Validator.dll",
            @"\bin\EC5.Antlr.ExpLexer.dll",
            @"\bin\EC5.BizCoder.dll",
            @"\bin\EC5.BizLogger.dll",
            @"\bin\EC5.BizLogger.Model.dll",
            @"\bin\EC5.DbCascade.dll",
            @"\bin\EC5.DbCascade.Model.dll",
            @"\bin\EC5.LCodeEngine.dll",
            @"\bin\EC5.LcValueEngine.dll",
            @"\bin\EC5.IG2.Core.dll"

        };


        /// <summary>
        /// 加载目录下面的
        /// </summary>
        /// <param name="dir">目录</param>
        public void Load(string dir)
        {
            if (!Directory.Exists(dir))
            {
                return;
            }

            string[] csFiles = Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories);


            try
            {
                CompiledSet(csFiles);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("动态编译失败: {0},\r\n错误信息:", csFiles, ex.StackTrace);
            }


            return;

            Type[] ts;

            foreach (string csFile in csFiles)
            {
                try
                {
                    string code = File.ReadAllText(csFile);

                    ts = Compiled(code);

                    m_TypeList.AddRange(ts);

                    log.DebugFormat("动态编译成功：{0}", csFile);
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("动态编译失败: {0},\r\n错误信息:", csFile, ex.StackTrace);
                }

                
            }
        }


        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private Type[] Compiled(string code)
        {
            // 创建编译器对象
            CSharpCodeProvider p = new CSharpCodeProvider();
            ICodeCompiler cc = p.CreateCompiler();

            // 设置编译参数
            CompilerParameters options = new CompilerParameters();
            options.ReferencedAssemblies.AddRange(m_SystemReferenceedAssembilies.ToArray());

            HttpServerUtility server =  HttpContext.Current.Server;

            foreach (var item in m_ReferenceedAssembilies)
            {
                options.ReferencedAssemblies.Add(server.MapPath(item));
            }


            options.GenerateExecutable = false;
            options.GenerateInMemory = true;

            //options.OutputAssembly = "MyTest";

            // 开始编译
            CodeSnippetCompileUnit cu = new CodeSnippetCompileUnit(code);
            

            CompilerResults cr = cc.CompileAssemblyFromDom(options, cu);

            if (cr.Errors.Count > 0)
            {
                throw new Exception(cr.Errors[0].ErrorText);
            }

            Type[] ts = cr.CompiledAssembly.GetTypes();

            return ts;
            
        }


        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private Type[] CompiledSet(string[] files)
        {
            // 创建编译器对象
            CSharpCodeProvider p = new CSharpCodeProvider();

            // 设置编译参数
            CompilerParameters op = new CompilerParameters();
            op.ReferencedAssemblies.AddRange(m_SystemReferenceedAssembilies.ToArray());

            HttpServerUtility server = HttpContext.Current.Server;

            foreach (var item in m_ReferenceedAssembilies)
            {
                op.ReferencedAssemblies.Add(server.MapPath(item));
            }


            op.GenerateExecutable = false;
            op.GenerateInMemory = true;
            
            CompilerResults cr = p.CompileAssemblyFromFile(op, files);


            if (cr.Errors.Count > 0)
            {
                throw new Exception(cr.Errors[0].ErrorText);
            }

            Type[] ts = cr.CompiledAssembly.GetTypes();

            return ts;

        }

    }
}
