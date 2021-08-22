using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Yahoo.Yui.Compressor;

namespace EC5.IG2.Core
{
    public static class Mini2JScriptHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 合并 Mini.js
        /// </summary>
        public static void UniteMiniSystemJS()
        {
            bool isMinFile = true;

            log.InfoFormat("{0}() Begin: 合并 Mini.js", System.Reflection.MethodBase.GetCurrentMethod().Name);

            HttpContext con = HttpContext.Current;

            if (con == null || con.Server == null) { return; }

            string uniteConfigFile = con.Server.MapPath("~/Core/Scripts/Mini2/Mini2.join.ini");
            string targetFile;

            if (isMinFile)
            {
                targetFile = con.Server.MapPath("~/Core/Scripts/Mini2/Mini2.min.js");
            }
            else
            {
                targetFile = con.Server.MapPath("~/Core/Scripts/Mini2/Mini2.js");
            }

            string targetAllFile = con.Server.MapPath("~/Core/Scripts/Mini2/Mini2.js");


            StringBuilder jsScript = new StringBuilder();

            StringBuilder fs = new StringBuilder();

            StringBuilder jsAll = new StringBuilder();

            fs.AppendFormat("/// Mini2.js 创建日期:{0}", DateTime.Now).AppendLine();

            jsAll.AppendFormat("/// Mini2.js 创建日期:{0}", DateTime.Now).AppendLine();

            string[] jsFiles = File.ReadAllLines(uniteConfigFile);


            foreach (string jsFile in jsFiles)
            {
                if (jsFile.Trim().Length == 0)
                {
                    jsScript.AppendLine();
                    continue;
                }

                if (jsFile.StartsWith("--") || jsFile.StartsWith("//"))
                {
                    continue;
                }

                string file = con.Server.MapPath("~/Core/Scripts/Mini2/dev/" + jsFile);

                if (!File.Exists(file))
                {
                    log.ErrorFormat("文件不存在:\"{0}\"", jsFile);
                    continue;
                }

                jsScript.Append("<script src=\"");
                jsScript.Append("/Core/Scripts/Mini2/dev/" + jsFile);
                jsScript.AppendLine("\" type=\"text/javascript\" ></script>");



                string lines = File.ReadAllText(file);
                

                jsAll.AppendLine().AppendLine();
                jsAll.AppendLine("/************************************************************************/");
                jsAll.AppendLine("/*                                           */");
                jsAll.AppendLine(("/*    " + jsFile).PadRight(75) + "*/");
                jsAll.AppendLine("/*                                           */");
                jsAll.AppendLine("/************************************************************************/").AppendLine() ;


                jsAll.AppendLine(lines);

                try
                {
                    //初始化JS压缩类
                    var js = new JavaScriptCompressor();
                    js.CompressionType = CompressionType.Standard;//压缩类型
                    js.Encoding = Encoding.UTF8;//编码
                    js.IgnoreEval = false;//大小写转换

                    js.ObfuscateJavascript = true;


                    js.ThreadCulture = System.Globalization.CultureInfo.CurrentCulture;


                    //压缩该js
                    string strContent = js.Compress(lines);

                    //string targetMiniFile = con.Server.MapPath("~/Core/Scripts/Mini2/mini/" + jsFile);

                    //if (isMinFile)
                    //{
                    fs.AppendLine(strContent);
                    //}
                    //else
                    //{
                    //    fs.Append(lines);
                    //}

                    //EcFile.WriteAllText(targetMiniFile, strContent, true);
                }
                catch (Exception ex)
                {
                    log.Error("压缩 " + jsFile + " 错误", ex);


                }
            }

            string targetFile2 = con.Server.MapPath("~/Core/Scripts/Mini2/Mini2.script.txt");

            string jsContent = fs.ToString();

            File.WriteAllText(targetFile2, jsScript.ToString(), Encoding.UTF8);

            File.WriteAllText(targetFile, fs.ToString(), Encoding.UTF8);

            File.WriteAllText(targetAllFile, jsAll.ToString(), Encoding.UTF8);


            log.InfoFormat("{0}() End", System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
    }
}
