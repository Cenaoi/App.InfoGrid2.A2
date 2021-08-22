using EC5.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EC5.LCodeEngine.Xml
{
    /// <summary>
    /// 实体文件
    /// </summary>
    [Serializable]
    public class XLcModelFile
    {
        XLcModelCollection m_Models;

        public XLcModelCollection Models
        {
            get
            {
                if(m_Models == null)
                {
                    m_Models = new XLcModelCollection();
                }
                return m_Models;
            }
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static XLcModelFile Open(string filename)
        {
            XLcModelFile file = XmlUtil.OpenXmlFile<XLcModelFile>(filename);            

            return file;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            XmlUtil.SaveXmlFile(filename, this);
        }

    }

    /// <summary>
    /// 实体集合
    /// </summary>
    [Serializable]
    public class XLcModelCollection : List<XLcModel>
    {

    }

    /// <summary>
    /// 实体
    /// </summary>
    [Serializable]
    public class XLcModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        [XmlAttribute]
        public string Table { get; set; }


        XLcFieldCollection m_Fields = null;


        public XLcFieldCollection Fields
        {
            get
            {
                if(m_Fields == null)
                {
                    m_Fields = new XLcFieldCollection();
                }
                return m_Fields;
            }
        }

    }

    /// <summary>
    /// 字段集合
    /// </summary>
    [Serializable]
    public class XLcFieldCollection : List<XLcField>
    {

    }

    /// <summary>
    /// 字段
    /// </summary>
    [Serializable]
    public class XLcField
    {

        public XLcField()
        {

        }

        public XLcField(string name, string lCode)
        {
            this.Name = name;
            this.Code = lCode;
        }

        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// light code 编码
        /// </summary>
        [XmlAttribute]
        public string Code { get; set; }
    }
}
