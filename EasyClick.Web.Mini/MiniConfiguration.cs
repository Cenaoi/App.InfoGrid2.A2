using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 货币配置信息
    /// </summary>
    public class CurrencyConfig
    {

    }

    /// <summary>
    /// 工具栏按钮的配置文件
    /// </summary>
    public class ToolBarButtonConfig
    {

        /// <summary>
        /// OnClick 默认值
        /// </summary>
        string m_OnClickDefaultValue = "widget1.submit(this);return false;";

        /// <summary>
        /// OnClick 默认值
        /// </summary>
        public string OnClickDefaultValue
        {
            get { return m_OnClickDefaultValue; }
            set { m_OnClickDefaultValue = value; }
        }

    }

    /// <summary>
    /// 图标资源
    /// </summary>
    public struct MiniIconRes
    {
        string m_Name;

        /// <summary>
        /// 图标名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        string m_Path;

        /// <summary>
        /// 路径
        /// </summary>
        public string Path
        {
            get { return m_Path; }
            set { m_Path = value; }
        }


    }

    /// <summary>
    /// 图标资源
    /// </summary>
    public class IconResourcesConfig
    {
        SortedDictionary<string, MiniIconRes> m_Items = new SortedDictionary<string, MiniIconRes>();

        object m_WriteObj = new object();

        /// <summary>
        /// 资源的目录物理路径
        /// </summary>
        /// <param name="resPath"></param>
        public void Load(string resPath,string resUrl)
        {
            lock (m_WriteObj)
            {
                int dirLen = resPath.Length  + 1;

                DirectoryInfo di = new DirectoryInfo(resPath);

                FileInfo[] files = di.GetFiles("*.png");

                foreach (FileInfo fi in files)
                {
                    string shortName = fi.FullName.Substring(dirLen).Replace('\\', '/');
                    string fileName = Path.GetFileNameWithoutExtension(fi.FullName).ToLower();

                    MiniIconRes iconRes = new MiniIconRes();
                    iconRes.Name = fileName;
                    iconRes.Path = resUrl + "/" + shortName;

                    m_Items.Add(fileName, iconRes);
                }
            }
        }

        public void Clear()
        {
            lock (m_WriteObj)
            {
                m_Items.Clear();
            }
        }


        /// <summary>
        /// 获取图标路径
        /// </summary>
        /// <param name="iconName">图标变量名称</param>
        /// <returns></returns>
        public string GetUrl(string iconName)
        {
            if (iconName[0] == '$')
            {
                iconName = iconName.Substring(1);
            }

            iconName = iconName.ToLower();

            string url = string.Empty;

            if (m_Items.ContainsKey(iconName))
            {
                MiniIconRes icon = m_Items[iconName];

                url = icon.Path;
            }
            else
            {
                url = string.Format("/res/icon/{0}.png", iconName);
            }

            return url;
        }
    }


    /// <summary>
    /// 配置
    /// </summary>
    public static class MiniConfiguration
    {
        static string m_ActionPath = "/Core/Mini/WidgetAction.aspx";

        /// <summary>
        /// JSON 数据构造工厂
        /// </summary>
        static IMiniJsonFactory m_JsonFactory = new MiniJsonFactory();

        /// <summary>
        /// 货币配置信息
        /// </summary>
        static CurrencyConfig m_DefaultCurrency = new CurrencyConfig();

        /// <summary>
        /// 工具栏按钮的配置信息
        /// </summary>
        static ToolBarButtonConfig m_ToolBarBtnConfig = new ToolBarButtonConfig();

        /// <summary>
        /// 图标资源配置信息
        /// </summary>
        static IconResourcesConfig m_IconResConfig = new IconResourcesConfig();

        /// <summary>
        /// 图标资源配置
        /// </summary>
        public static IconResourcesConfig IconResConfig
        {
            get { return m_IconResConfig; }
            set { m_IconResConfig = value; }
        }

        /// <summary>
        /// 工具栏按钮的配置信息
        /// </summary>
        public static ToolBarButtonConfig ToolBarBtnConfig
        {
            get { return m_ToolBarBtnConfig; }
            set { m_ToolBarBtnConfig = value; }
        }

        static string[] m_ServerAttrTags = new string[] { "DBText", "DBField", "DBLogic" };

        /// <summary>
        /// 默认货币类型
        /// </summary>
        public static CurrencyConfig DefaultCurrency
        {
            get { return m_DefaultCurrency; }
        }

        /// <summary>
        /// 服务器属性标签.用于隐藏客户端
        /// </summary>
        public static string[] ServerAttrTags
        {
            get { return m_ServerAttrTags; }
            set { m_ServerAttrTags = value; }
        }

        /// <summary>
        /// JSON 数据的转换工厂 
        /// </summary>
        public static IMiniJsonFactory JsonFactory
        {
            get { return m_JsonFactory; }
            set { m_JsonFactory = value; }
        }

        /// <summary>
        /// ajax 反馈的接受地址，默认是：“/Core/Mini/WidgetAction.aspx”
        /// </summary>
        public static string ActionPath
        {
            get { return m_ActionPath; }
            set { m_ActionPath = value; }
        }

        
    }



}
