/* 
 * 
 * 2013-3-17 19:57
 * 
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace App.Register
{


    /// <summary>
    /// 注册帮助
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class RegHelp
    {

        static string GetVersion(string fullname)
        {
            string[] lines = fullname.Split(',');

            string line = lines[1].Trim();

            string str = line.Substring(8);

            return str;
        }

        static string GetWinFormPass(string sCode, string userId, string toTime, string orgFilename, string fileVersion, string version)
        {
            string pass = "12323213432FDSA342";
            RegFactory rf = new RegFactory();

            string local = MethodBase.GetCurrentMethod().DeclaringType.Assembly.CodeBase.ToLower();
            string paPath = GetMD5(local);

            StringBuilder sb = new StringBuilder();
            sb.Append(paPath).Append("$$$");

            sb.Append(userId).Append("$$$");
            sb.Append(sCode).Append("$$$");
            sb.Append(toTime).Append("$$$");

            sb.Append(rf.GetCpuID()).Append("$$$");
            sb.Append(rf.GetHDid()).Append("$$$");
            sb.Append(rf.GetMoAddress()).Append("$$$");
            sb.Append(rf.ZbID()).Append("$$$");

            sb.Append(GetFileMD5()).Append("$$$");
            sb.Append(orgFilename).Append("$$$");
            sb.Append(fileVersion).Append("$$$");
            sb.Append(version).Append("$$$");
            sb.Append(Environment.OSVersion);

            pass = sb.ToString();

            return pass;

        }

        static string GetEc5Pass(string sCode, string userId, string toTime, string orgFilename, string fileVersion, string version)
        {
            string pass = "12323213432FDSA342";
            RegFactory rf = new RegFactory();

            SortedList<string, string> values = rf.GetContextValues();

            //Web 本地调试模式

            string local = MethodBase.GetCurrentMethod().DeclaringType.Assembly.CodeBase.ToLower();
            string paPath = GetMD5(local);

            StringBuilder sb = new StringBuilder();
            sb.Append(paPath).Append("$$$");

            sb.Append(userId).Append("$$$");
            sb.Append(sCode).Append("$$$");
            sb.Append(toTime).Append("$$$");

            sb.Append(values["CPU_ID"]).Append("$$$");
            sb.Append(values["HD_ID"]).Append("$$$");
            sb.Append(values["MO_ADD"]).Append("$$$");
            sb.Append(values["ZD_ID"]).Append("$$$");

            sb.Append(GetFileMD5()).Append("$$$");
            sb.Append(orgFilename).Append("$$$");
            sb.Append(fileVersion).Append("$$$");
            sb.Append(version).Append("$$$");
            sb.Append(Environment.OSVersion);

            pass = sb.ToString();

            return pass;
        }

        static string GetLocalPass(string sCode, string userId, string toTime, string orgFilename, string fileVersion, string version)
        {
            string pass = "12323213432FDSA342";
            RegFactory rf = new RegFactory();



            string hdId = rf.GetHDid();
            string cpuId = rf.GetCpuID();
            string zbId = rf.ZbID();
            string moAdd = rf.GetMoAddress();

            if ("unknow".Equals(hdId) && "unknow".Equals(cpuId) &&
                "unknow".Equals(zbId) && "unknow".Equals(moAdd))
            {
                pass = "ERROR:请设计师‘黄伟钦�?";
            }
            else
            {
                //Web 本地调试模式

                string local = MethodBase.GetCurrentMethod().DeclaringType.Assembly.CodeBase.ToLower();
                string paPath = GetMD5(local);

                StringBuilder sb = new StringBuilder();
                sb.Append(paPath).Append("$$$");

                sb.Append(userId).Append("$$$");
                sb.Append(sCode).Append("$$$");
                sb.Append(toTime).Append("$$$");

                sb.Append(cpuId).Append("$$$");
                sb.Append(hdId).Append("$$$");
                sb.Append(moAdd).Append("$$$");
                sb.Append(zbId).Append("$$$");

                sb.Append(GetFileMD5()).Append("$$$");
                sb.Append(orgFilename).Append("$$$");
                sb.Append(fileVersion).Append("$$$");
                sb.Append(version).Append("$$$");
                sb.Append(Environment.OSVersion);

                pass = sb.ToString();

            }

            return pass;
        }

        static string GetPass(string sCode, string userId, string toTime, string orgFilename, string fileVersion, string version)
        {
            string pass = "4345002442043420024420";

            

            if (HttpContext.Current == null)
            {
                pass = GetWinFormPass(sCode, userId, toTime, orgFilename, fileVersion, version);
            }
            else
            {
                HttpContext context = HttpContext.Current;
                HttpApplicationState appState = context.Application;


                if (appState["_EC5_ID"] != null && appState["_EC5_GID"] != null)
                {
                    pass = GetEc5Pass(sCode, userId, toTime, orgFilename, fileVersion, version);
                }
                else
                {
                    pass = GetLocalPass(sCode, userId, toTime, orgFilename, fileVersion, version);
                }

            }

            return pass;
        }


        static string GetMD5(string path)
        {
            //1.构造密�?
            // 文件�?+  用户�?
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] fileMd5 = md5.ComputeHash(Encoding.UTF8.GetBytes( path));

            return BitConverter.ToString(fileMd5).Replace("-","");
        }

        static string GetFileMD5()
        {
            //1.构造密�?
            // 文件�?+  用户�?
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            Assembly ass = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Assembly;

            FileVersionInfo fInfo = FileVersionInfo.GetVersionInfo(ass.Location);//为磁盘上的物理文件提供版本信�?

            byte[] fileMd5 = md5.ComputeHash(File.ReadAllBytes(ass.Location));

            return Convert.ToBase64String(fileMd5);
        }

        static bool m_IsReg = true;
        static bool m_IsDebug;
        static DateTime m_ToTime;

        static Int16 m_ValidNum = 0;

        static bool m_IsValidRegister = false;

        static byte[] m_TmpData;
        static string m_TmpUserId;
        static string m_TmpToTime;


        /// <summary>
        /// 授权�?
        /// </summary>
        static string m_RegCode;

        private static void ProData(byte[] data, string userId, string toTime, string regCode)
        {

            //1.构造密�?
            // 文件�?+  用户�?
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            Assembly ass = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Assembly;

            FileVersionInfo fInfo = FileVersionInfo.GetVersionInfo(ass.Location);//为磁盘上的物理文件提供版本信�?

            string xFileVersion = fInfo.FileVersion;
            string xVersion = GetVersion(ass.FullName);// fInfo.ProductVersion;
            string xOrgFilename = fInfo.OriginalFilename;
            string xFileMd5 = GetFileMD5();

            string xPass = GetPass(regCode, userId, toTime, xOrgFilename, xFileVersion, xVersion);

            RegFactory rf = new RegFactory();

            string xml = rf.AESDecrypt(data, xPass);


            if (xml == string.Empty || xml[0] != '<')
            {    
                //解密失败，退�?
                m_IsReg = false;
                return; 
            }


            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlElement root = doc.DocumentElement;

            string id = root.Name.Substring(1);

            XmlNode node = root;// root.ChildNodes[0];


            HttpContext context = HttpContext.Current;
            HttpApplicationState appState = context.Application;



            string FileName = node["FILE_NAME"].InnerText;

            string fileMd5 = node["FILE_MD5"].InnerText;
            string orgFileName = node["ORG_FILENAME"].InnerText;
            string fileVersion = node["FILE_VERSION"].InnerText;
            string version = node["VERSION"].InnerText;


            if (!fileMd5.Equals(xFileMd5)) return;

            if (!version.Equals(version)) return;
            if (!fileVersion.Equals(fileVersion)) return;

            try
            {
                string ToTime = rf.AESDecrypt(Convert.FromBase64String(node["TO_TIME"].InnerText), xFileMd5);
                string mode = node["MODE"].InnerText;
                string IsReg = node["IS_REG"].InnerText;

                m_IsDebug = (mode == "FULL" ? false : true);
                //m_ToTime = DateTime.Parse(ToTime);

                if (!DateTime.TryParse(ToTime, out m_ToTime))
                {
                    m_ToTime = DateTime.Now;
                }

                TimeSpan span = DateTime.Now - m_ToTime;
                if (span.TotalMinutes < 0)
                {
                    m_IsReg = true;
                }
                else
                {
                    m_IsReg = false;
                }
            }
            catch
            {
                m_IsReg = false;
            }
        }



        /// <summary>
        /// 设置注册信息
        /// </summary>
        /// <param name="data">解密信息</param>
        /// <param name="userId">用户ID</param>
        /// <param name="toTime">授权结束日期</param>
        /// <param name="mode">DEMO,FULL</param>
        /// <param name="isValidNow">立刻验证</param>
        public static void SetData(byte[] data, string rCode, string userId, string toTime, string mode, bool isValidNow)
        {
            if (data == null || data.Length == 0) throw new Exception("数据不能为空");
            if (string.IsNullOrEmpty(userId)) throw new Exception("id 不能为空");

            m_TmpData = data;
            m_TmpUserId = userId;
            m_TmpToTime = toTime;

            m_IsValidRegister = false;

            m_RegCode = rCode;

            if (!isValidNow)
            {
                //后期验证

                DateTime toDT;

                if (!DateTime.TryParse(toTime, out toDT))
                {
                    toDT = DateTime.Now;
                }

                TimeSpan span = DateTime.Now - toDT;
                if (span.TotalMinutes < 0)
                {
                    m_IsReg = true;
                }
                else
                {
                    m_IsReg = false;
                }

                return;
            }

            try
            {
                ProData(data, userId, toTime, rCode);

                m_IsValidRegister = true;
            }
            catch
            {
                m_IsReg = false;
            }
        }

        /// <summary>
        /// 是否已经验证注册
        /// </summary>
        /// <returns></returns>
        public static bool IsValidRegister()
        {
            return true;

            return m_IsValidRegister;
        }


        static void ResetNum()
        {
            if (m_ValidNum < 2000)
            {
                return;
            }

            try
            {
                ProData(m_TmpData, m_TmpUserId, m_TmpToTime, m_RegCode);
            }
            catch
            {
                m_IsReg = false;
                m_IsValidRegister = true;
                return;
            }

            m_IsValidRegister = true;

            TimeSpan span = DateTime.Now - m_ToTime;
            if (span.TotalMinutes < 0)
            {
                m_IsReg = true;
            }
            else
            {
                m_IsReg = false;
            }

            m_ValidNum = 0;
        }


        /// <summary>
        /// 是否已经注册
        /// </summary>
        /// <returns></returns>
        public static bool IsRegister()
        {
            return true;

            m_ValidNum++;

            ResetNum();

            return m_IsReg;
        }


    }


    class RegFactory
    {
        /// <summary>
        /// 获取 Application 里面的硬件参数�?
        /// </summary>
        /// <returns></returns>
        public SortedList<string, string> GetContextValues()
        {
            HttpContext context = HttpContext.Current;
            HttpApplicationState appState = context.Application;

            SortedList<string, string> values = new SortedList<string, string>(4);

            if (appState["_EC5_ID"] != null && appState["_EC5_GID"] != null)
            {
                string _EC5_ID = (string)HttpContext.Current.Application["_EC5_ID"];
                byte[] _EC5_GID = (byte[])HttpContext.Current.Application["_EC5_GID"];

                RegFactory rf = new RegFactory();

                string pass = ((string)appState["_EC5_ID"]);

                string xml = rf.AESDecrypt(_EC5_GID, pass);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlElement root = doc.DocumentElement;
                values["CPU_ID"] = root["CPU_ID"].InnerText;
                values["HD_ID"] = root["HD_ID"].InnerText;
                values["MO_ADD"] = root["MO_ADD"].InnerText;
                values["ZD_ID"] = root["ZD_ID"].InnerText;

            }

            return values;
        }



        ///   <summary> 
        ///   获取网卡硬件地址 
        ///   </summary> 
        ///   <returns> string </returns> 
        public string GetMoAddress()
        {
            ManagementClass mc = null ;

            string moAddress = string.Empty;

            try
            {
                string MoAddress = string.Empty;
                mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc2 = mc.GetInstances();

                foreach (ManagementObject mo in moc2)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        moAddress += mo["MacAddress"].ToString() + ";";
                    }

                    mo.Dispose();
                }


                moAddress.Trim();
            }
            catch
            {
                moAddress = "unknow";
            }
            finally
            {
                mc.Dispose();
            }

            return moAddress;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetCpuID()
        {
            try
            {
                //获取CPU序列号代�?
                string cpuInfo = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo += mo.Properties["ProcessorId"].Value.ToString() + ";";
                }
                moc = null;
                mc = null;
                return cpuInfo.Trim();
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }


        /// <summary>
        /// 主板
        /// </summary>
        /// <returns></returns>
        public string ZbID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_BaseBoard");
                ManagementObjectCollection moc = mc.GetInstances();
                string strID = null;
                foreach (ManagementObject mo in moc)
                {
                    strID = mo.Properties["SerialNumber"].Value.ToString();
                    break;
                }

                return strID.Trim();
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        ///   <summary> 
        ///   获取硬盘ID     
        ///   </summary> 
        ///   <returns> string </returns> 
        public string GetHDid()
        {
            try
            {
                string HDid = string.Empty;
                ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc1 = cimobject1.GetInstances();
                foreach (ManagementObject mo in moc1)
                {
                    string m = (string)mo.Properties["Model"].Value;

                    if (m.LastIndexOf("USB") > -1) { continue; }

                    HDid += m + "||" + ((string)mo.Properties["SerialNumber"].Value).Trim() + ";";
                }
                return HDid.Trim();
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }


        /// <summary>
        /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="DecryptData">待解密密�?/param>
        /// <param name="DecryptKey">解密密钥</param>
        /// <returns></returns>
        public string AESDecrypt(byte[] DecryptData, string DecryptKey)
        {

            //if (string.IsNullOrEmpty(DecryptString)) { throw (new Exception("密文不得为空")); }
            //if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }

            string m_strDecrypt = "";

            #region 构�?IV

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bs = md5.ComputeHash(Encoding.UTF8.GetBytes(DecryptKey));
            bs = md5.ComputeHash(bs);
            bs = md5.ComputeHash(bs);

            byte[] m_btIV = bs;

            #endregion

            Rijndael m_AESProvider = Rijndael.Create();

            try
            {
                byte[] key = GetKey16(DecryptKey);



                byte[] m_btDecryptString = DecryptData;// Convert.FromBase64String(DecryptString);
                MemoryStream m_stream = new MemoryStream();
                CryptoStream m_csstream = new CryptoStream(m_stream, m_AESProvider.CreateDecryptor(key, m_btIV), CryptoStreamMode.Write);

                m_csstream.Write(m_btDecryptString, 0, m_btDecryptString.Length);
                m_csstream.FlushFinalBlock();

                m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());
                m_stream.Close(); m_stream.Dispose();
                m_csstream.Close(); m_csstream.Dispose();

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally { m_AESProvider.Clear(); }

            return m_strDecrypt;

        }

        byte[] GetKey16(string pass)
        {
            byte[] key = Encoding.UTF8.GetBytes(pass);

            if (key.Length > 16)
            {
                byte[] key2 = new byte[16];

                for (int i = 0; i < 16; i++)
                {
                    key2[i] = key[i];
                }

                key = key2;
            }
            else if (key.Length < 16)
            {
                byte[] key2 = new byte[16];

                for (int i = 0; i < key.Length; i++)
                {
                    key2[i] = key[i];
                }

                key = key2;
            }

            return key;
        }

    }

}

