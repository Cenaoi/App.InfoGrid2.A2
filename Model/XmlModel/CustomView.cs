using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace App.InfoGrid2.Model.XmlModel
{
    /// <summary>
    /// 自定义界面xml
    /// </summary>
    [XmlRoot("window")]
    public class CustomView
    {

        private AppSettin m_appSettings;
        private VStruct m_struct;
        private SExpand m_expand;

        /// <summary>
        /// 参数设置
        /// </summary>
        [XmlElement("appSettings")]
        public AppSettin appSettings 
        {
            get 
            {
                if (m_appSettings == null) 
                {
                    m_appSettings = new AppSettin();
                }

                return m_appSettings;
            }

            set { m_appSettings = value; }

        }


        /// <summary>
        /// 界面结构
        /// </summary>
        [XmlElement("struct")]
        public VStruct viewStruct
        {
            get
            {
                if(m_struct == null)
                {
                    m_struct = new VStruct();
                }

                return m_struct;

            }

            set { m_struct = value; }

        }

        /// <summary>
        /// 界面结构扩展，给扩展类使用
        /// </summary>
        [XmlElement("struct_expand")]
        public SExpand structEpand 
        {
            get
            {
                if (m_expand == null)
                {
                    m_expand = new SExpand();
                } return m_expand;
            }

            set { m_expand = value; }
        }


    }

   public class AppSettin 
    {
        [XmlIgnore]
        private List<AppSettings> m_appSettings;


        
        [XmlElement("add")]
        public List<AppSettings> appSettings
        {
            get
            {
                if (m_appSettings == null)
                {
                    m_appSettings = new List<AppSettings>();
                }

                return m_appSettings;

            }

            set { m_appSettings = value; }

        }

    }

   public class VStruct
   {
       [XmlIgnore]
       private List<ViewStruct> m_viewStruct;



       [XmlElement("control")]
       public List<ViewStruct> viewStruct
       {
           get
           {
               if (m_viewStruct == null)
               {
                   m_viewStruct = new List<ViewStruct>();
               }

               return m_viewStruct;

           }

           set { m_viewStruct = value; }

       }

   }

   public class SExpand 
   {

       private List<ViewStruct> m_structEpand;

       [XmlElement("control")]
       public List<ViewStruct> structEpand
       {
           get
           {
               if (m_structEpand == null)
               {
                   m_structEpand = new List<ViewStruct>();
               }

               return m_structEpand;
           }

           set { m_structEpand = value; }
       }
   }




}
