using App.InfoGrid2.Bll;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EC5.Utility;
using HWQ.Entity.LightModels;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EC5.IG2.Core.UI
{
    /// <summary>
    /// 视图实体集合
    /// </summary>
    public class VModelConfigColletion
    {
        SortedDictionary<int, VModelConfig> m_Items = new SortedDictionary<int, VModelConfig>();


        public VModelConfig this[int tableId]
        {
            get
            {
                return GetItem(tableId);
            }
        }

        public VModelConfig GetItem(int id)
        {
            VModelConfig cfg;

            m_Items.TryGetValue(id, out cfg);

            return cfg;
        }

        public void Add(VModelConfig cfg)
        {
            m_Items[cfg.TableId] = cfg;
        }

        public bool Remove(int tableId)
        {
            return m_Items.Remove(tableId);
        }

        public void Clear()
        {
            m_Items.Clear();
        }

    }

    /// <summary>
    /// 视图实体
    /// </summary>
    public class VModelConfig
    {
        int m_TableId;

        VFieldConfigCollection m_VFields = new VFieldConfigCollection();

        public VFieldConfigCollection VFields
        {
            get { return m_VFields; }
        }

        public int TableId
        {
            get { return m_TableId; }
            set { m_TableId = value; }
        }
    }

    /// <summary>
    /// 视图字段配置集合
    /// </summary>
    public class VFieldConfigCollection:IEnumerable<VFieldConfig>
    {
        SortedList<string, VFieldConfig> m_Items = new SortedList<string, VFieldConfig>();

        public VFieldConfig this[string id]
        {
            get
            {
                return GetItem(id);
            }
        }

        public VFieldConfig GetItem(string id)
        {
            VFieldConfig cfg;

            m_Items.TryGetValue(id, out cfg);

            return cfg;
        }

        public void Add(VFieldConfig cfg)
        {
            m_Items[cfg.DBField] = cfg;
        }

        public bool Remove(string id)
        {
            return m_Items.Remove(id);
        }

        public void Clear()
        {
            m_Items.Clear();
        }

        public int Count
        {
            get { return m_Items.Count; }
        }

        public IEnumerator<VFieldConfig> GetEnumerator()
        {
            return m_Items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Items.Values.GetEnumerator();
        }
    }

    /// <summary>
    /// 视图字段的配置
    /// </summary>
    public class VFieldConfig
    {
        [JsonIgnore]
        public string DBField { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// 目标字段
        /// </summary>
        [JsonProperty(PropertyName = "tar_field")]
        public string TarField { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty(PropertyName = "remark")]
        public string Remark { get; set; }
    }



    /// <summary>
    /// 视图字段的助手类，配合界面使用
    /// </summary>
    public static class M2VFieldHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static VModelConfigColletion m_VModels = new VModelConfigColletion();

        public static VModelConfigColletion VModels
        {
            get { return m_VModels; }
        }


        private static VModelConfig CreateVModel(TableSet tableSet)
        {
            IG2_TABLE tab = tableSet.Table;

            VModels.Remove(tab.IG2_TABLE_ID); 

            VModelConfig vModelCfg = new VModelConfig();
            vModelCfg.TableId = tab.IG2_TABLE_ID;

            foreach (var item in tableSet.GetVCols())
            {

                if (StringUtil.IsBlank(item.VIEW_FIELD_CONFIG))
                {
                    continue;
                }

                try
                {
                    VFieldConfig vFieldCfg = JsonConvert.DeserializeObject<VFieldConfig>(item.VIEW_FIELD_CONFIG);

                    vFieldCfg.DBField = item.DB_FIELD;

                    vModelCfg.VFields.Add(vFieldCfg);
                }
                catch 
                {
                    log.ErrorFormat("解析视图配置错误。IG2_TABLE_ID={0}, Table={1}, Field={2}, Config={3}",
                        tab.IG2_TABLE_ID, tab.TABLE_NAME, item.DB_FIELD, item.VIEW_FIELD_CONFIG);
                }
            }

            VModels.Add(vModelCfg);

            return vModelCfg;
        }

        /// <summary>
        /// 填充实体的视图字段
        /// </summary>
        /// <param name="models"></param>
        public static void Full(IList models,int tableId)
        {
            //处理特殊字段
            if (models.Count == 0)
            {
                return;
            }


            LModel model = models[0] as LModel;

            if (model == null)
            {
                return;
            }

            LModelElement modelElem = model.GetModelElement();
            string tableName = modelElem.DBTableName;

            TableSet tableSet;

            if (tableId > 0)
            {
                tableSet = TableBufferMgr.GetTable(tableId);
            }
            else
            {
                tableSet = TableBufferMgr.GetTable(tableName);
            }

            IG2_TABLE tab = tableSet.Table;


            VModelConfig vModelCfg = VModels[tab.IG2_TABLE_ID];

            if (vModelCfg == null)
            {
                vModelCfg = CreateVModel(tableSet);
            }

            if (vModelCfg.VFields.Count == 0)
            {
                return;
            }


            DateTime now = DateTime.Now;

            foreach (var obj in models)
            {
                model = obj as LModel;

                if (model == null)
                {
                    continue;
                }

                FullModel(vModelCfg, modelElem, model);

            }
            
        }

        /// <summary>
        /// 填充视图字段
        /// </summary>
        public static void FullModel(VModelConfig vModelCfg,LModelElement modelElem, LModel model)
        {
            DateTime now = DateTime.Now;

            string dbField ;

            foreach (var cfg in vModelCfg.VFields)
            {
                dbField = cfg.DBField;

                //日期间距
                if (cfg.Type == "date_span")
                {
                    LModelFieldElement fieldElem;

                    if (StringUtil.IsBlank(cfg.TarField))
                    {
                        throw new Exception(string.Format("视图字段“{0}”错误。不能出现空目标字段。",dbField));
                    }

                    if (!modelElem.TryGetField(cfg.TarField, out fieldElem))
                    {
                        throw new Exception(string.Format("视图字段“{0}”错误。数据库不存在目标“{1}.{2}”字段。",
                           dbField, modelElem.DBTableName, cfg.TarField));
                    }

                    if (model.IsNull(cfg.TarField))
                    {
                        model.SetValue(dbField, null);
                    }
                    else
                    {
                        DateTime t1 = model.Get<DateTime>(cfg.TarField);

                        TimeSpan span = now - t1;

                        model.SetValue(dbField, Math.Round(span.TotalDays, 5));
                    }
                }
            }
        }

    }
}
