using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using App.Register;

namespace EasyClick.BizWeb
{
    /// <summary>
    /// 
    /// </summary>
    public class BizHttpRoutingModule : IHttpModule
    {

        public void Init(HttpApplication context)
        {
            

            RegFactory rf = new RegFactory();

            //1.构造密码
            // 文件名 +  用户名
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            StringBuilder sb = new StringBuilder(); ;

            string guid = Guid.NewGuid().ToString("N");

            sb.AppendFormat("<N{0}>", guid);
            {
                sb.AppendFormat("<CPU_ID>{0}</CPU_ID>", rf.GetCpuID());
                sb.AppendFormat("<HD_ID>{0}</HD_ID>", rf.GetHDid());
                sb.AppendFormat("<MO_ADD>{0}</MO_ADD>", rf.GetMoAddress());
                sb.AppendFormat("<ZD_ID>{0}</ZD_ID>", rf.ZbID());
            }
            sb.AppendFormat("</N{0}>",guid);


            System.Web.HttpContext.Current.Application.Lock();
            System.Web.HttpContext.Current.Application["_EC5_ID"] = guid;
            System.Web.HttpContext.Current.Application["_EC5_GID"] = AESEncryptBytes(sb.ToString(), guid +  "-设计:黄伟钦");
            System.Web.HttpContext.Current.Application.UnLock();
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

        /// <summary>
        /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="EncryptString"></param>
        /// <param name="EncryptKey"></param>
        /// <returns></returns>
        byte[] AESEncryptBytes(string EncryptString, string EncryptKey)
        {
            if (string.IsNullOrEmpty(EncryptString)) { return null; }
            if (string.IsNullOrEmpty(EncryptKey)) { return null; }

            //string m_strEncrypt = "";

            #region 构造 IV

            //Assembly ass = this.GetType().Assembly;
            //string localFs = ass.Location;

            //byte[] fs = File.ReadAllBytes(localFs);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bs = md5.ComputeHash(Encoding.UTF8.GetBytes(EncryptKey));
            bs = md5.ComputeHash(bs);
            bs = md5.ComputeHash(bs);

            byte[] m_btIV = bs;// Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");

            #endregion

            Rijndael m_AESProvider = Rijndael.Create();

            //m_AESProvider.KeySize = 128;
            //m_AESProvider.BlockSize = 128;
            //m_AESProvider.Mode = CipherMode.CBC;
            //m_AESProvider.Padding = PaddingMode.PKCS7;

            byte[] resultData = null;

            try
            {
                byte[] m_btEncryptString = Encoding.UTF8.GetBytes(EncryptString);

                byte[] key = GetKey16(EncryptKey);


                ICryptoTransform ctf = m_AESProvider.CreateEncryptor(key, m_btIV);


                MemoryStream m_stream = new MemoryStream();
                CryptoStream m_csstream = new CryptoStream(m_stream, ctf, CryptoStreamMode.Write);

                m_csstream.Write(m_btEncryptString, 0, m_btEncryptString.Length);
                m_csstream.FlushFinalBlock();

                resultData = m_stream.ToArray();
                //m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());

                m_stream.Close(); m_stream.Dispose();
                m_csstream.Close(); m_csstream.Dispose();
            }
            catch (Exception ex)
            {

            }
            finally { m_AESProvider.Clear(); }


            return resultData;
        }

        public void Dispose()
        {

        }
    }
}
