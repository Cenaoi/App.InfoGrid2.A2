using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EC5.SystemBoard.Web
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    [Obsolete]
    public static class FileUtiltiy
    {
        /// <summary>
        /// 获取新文件名,如果重名，就加上 （n) 序号
        /// </summary>
        /// <param name="dirPath">目录名称</param>
        /// <param name="orgFileName">文件原名</param>
        /// <returns></returns>
        public static string GetUniqueName(string dirPath, string orgFileName)
        {
            string text2 = string.Empty;
            string text = orgFileName;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(orgFileName);
            int num2 = 0;
            int num = 0;

            while (true)
            {
                text2 = Path.Combine(dirPath, text);

                if (!File.Exists(text2))
                {
                    break;
                }

                num2++;
                text = string.Concat(new object[]
					{
						fileNameWithoutExtension, 
						"(", 
						num2, 
						")", 
						Path.GetExtension(orgFileName)
					});
                num = 201;
            }

            return text;
        }


    }
}
