using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EC5.Utility;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Bll
{
    /// <summary>
    /// 流程 URL 地址配置
    /// </summary>
    public static class FlowUrlMgr
    {

        class FlowUrlConfig
        {
            /// <summary>
            /// 电脑端 url
            /// </summary>
            public FlowUrlList Company { get; set; }

            /// <summary>
            /// 手机端 url
            /// </summary>
            public FlowUrlList Mobile { get; set; }
        }
        
        class FlowUrlList :List<FlowMenuConfig>
        {

        }

        class FlowMenuConfig
        {
            public int MenuId { get; set; }

            public string Url { get; set; }
        }

        




        static FlowUrlConfig m_Global;

        static ConcurrentDictionary<string, FlowUrlConfig> m_TableDefine;

        static string[] EX_FIELDS = new string[]
        {
            "EXTEND_TABLE",
            "EXTEND_ROW_ID",
            "EXTEND_PAGE_ID",
            "EXTEND_ROW_CODE",
            "EXTEND_MENU_ID",
            "EXTEND_MENU_IDENTIFIER",
            "EXTEND_DOC_TEXT",
            "EXTEND_BILL_TYPE",
            "EXTEND_BILL_CODE",
            "EXTEND_BILL_CODE_FIELD"
        };
        

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="jsonFile">json 格式的配置文件</param>
        public static void ReadConfig(string jsonFile)
        {
            string json = File.ReadAllText(jsonFile);

            dynamic cfg = SModel.ParseJson(json);

            dynamic global = cfg.globel;


            m_Global = new FlowUrlConfig();
            m_TableDefine = new ConcurrentDictionary<string, FlowUrlConfig>();


            SModel tableDefine = cfg["table-define"];

            m_Global.Company = global.company;
            m_Global.Mobile = global.mobile;


            foreach (var table in tableDefine.GetFields())
            {
                dynamic tabCfg = tableDefine[table];

                FlowUrlConfig urlCfg = new FlowUrlConfig();


                if (tabCfg.company is SModelList)
                {
                    urlCfg.Company = new FlowUrlList();

                    foreach (var item in tabCfg.company)
                    {
                        urlCfg.Company.Add(new FlowMenuConfig() { MenuId = Convert.ToInt32( item.menu_id), Url = item.url });
                    }
                }
                else
                {
                    urlCfg.Company = new FlowUrlList();
                    urlCfg.Company.Add(new FlowMenuConfig() { MenuId = 0, Url = tabCfg.company });
                }

                if (tabCfg.mobile is SModelList)
                {

                }
                else
                {
                    urlCfg.Mobile = new FlowUrlList();
                    urlCfg.Mobile.Add(new FlowMenuConfig() { MenuId = 0, Url = tabCfg.mobile });
                }

                m_TableDefine.TryAdd(table, urlCfg);

            }

        }


        public static string Get(string device, int menuId, string table, LModel record)
        {
            string cfgStr = GetConfigString(device, menuId, table);

            if (StringUtil.IsBlank(cfgStr))
            {
                return cfgStr;
            }

            foreach (var item in EX_FIELDS)
            {
                if (record.HasField(item))
                {
                    string value = Convert.ToString(record[item]);
                    cfgStr = cfgStr.Replace("{{" + item + "}}", value);
                }
                else
                {
                    cfgStr = cfgStr.Replace("{{" + item + "}}", string.Empty);
                }
            }


            return cfgStr;
        }

        public static string Get(string device, int menuId, string table, SModel record)
        {
            string cfgStr = GetConfigString(device, menuId, table);

            if (StringUtil.IsBlank(cfgStr))
            {
                return cfgStr;
            }

            foreach (var item in EX_FIELDS)
            {
                if (record.HasField(item))
                {
                    string value = Convert.ToString( record[item]);

                    cfgStr = cfgStr.Replace("{{" + item + "}}", value);
                }
                else
                {
                    cfgStr = cfgStr.Replace("{{" + item + "}}", string.Empty);
                }
            }


            return cfgStr;
        }

        public static string GetConfigString(string device,int menuId, string table)
        {
            FlowUrlConfig cfgBase = null;

            if("company" == device)
            {
                if (m_TableDefine.TryGetValue(table, out cfgBase) )
                {
                    if(cfgBase.Company.Count == 1 || menuId <= 0)
                    {
                        if (!StringUtil.IsBlank(cfgBase.Company[0].Url))
                        {
                            return cfgBase.Company[0].Url;
                        }
                    }
                    else
                    {
                        foreach (var item in cfgBase.Company)
                        {
                            if(item.MenuId == menuId)
                            {
                                return item.Url;
                            }
                        }
                    }

                }
            }
            else if("mobile" == device)
            {
                if (m_TableDefine.TryGetValue(table, out cfgBase))
                {
                    if (menuId <= 0 && cfgBase.Mobile.Count >= 0)
                    {
                        if (!StringUtil.IsBlank(cfgBase.Mobile[0].Url))
                        {
                            return cfgBase.Mobile[0].Url;
                        }
                    }
                }
            }


            return string.Empty;
        }

    }
}
