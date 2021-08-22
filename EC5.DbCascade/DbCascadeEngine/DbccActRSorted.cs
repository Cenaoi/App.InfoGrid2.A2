using System;
using System.Collections.Generic;
using System.Text;

namespace EC5.DbCascade.DbCascadeEngine
{



    /// <summary>
    /// 表名索引类
    /// </summary>
    public class DbccActRSorted : SortedDictionary<string, DbccActCodeRSorted>
    {



        public void Add(DbccModel item)
        {
            string rTable = item.R_Table;
            string rActCode = item.R_ActCode;

            DbccActCodeRSorted actCodeR = null;

            DbccModelCollection models = null;


            if (!base.TryGetValue(rTable, out actCodeR))
            {
                actCodeR = new DbccActCodeRSorted();

                base.Add(rTable, actCodeR);
            }



            if (rActCode == "ALL")
            {

                if (!actCodeR.TryGetValue("DELETE", out models))
                {
                    models = new DbccModelCollection();

                    actCodeR.Add("DELETE", models);
                }

                models.Add(item);


                if (!actCodeR.TryGetValue("UPDATE", out models))
                {
                    models = new DbccModelCollection();

                    actCodeR.Add("UPDATE", models);
                }
                models.Add(item);

                if (!actCodeR.TryGetValue("INSERT", out models))
                {
                    models = new DbccModelCollection();

                    actCodeR.Add("INSERT", models);
                }

                models.Add(item);
            }
            else
            {
                if (!actCodeR.TryGetValue(rActCode, out models))
                {
                    models = new DbccModelCollection();

                    actCodeR.Add(rActCode, models);
                }

                models.Add(item);
            }


        }


        /// <summary>
        /// 获取被影响的联动对象
        /// </summary>
        /// <param name="rTable">右表</param>
        /// <param name="rActCode"></param>
        /// <returns></returns>
        public DbccModelCollection GetModels(string rTable, string rActCode)
        {
            DbccActCodeRSorted actCodeR = null;

            DbccModelCollection models = null;

            if (!base.TryGetValue(rTable, out actCodeR))
            {
                return null;
            }

            if (!actCodeR.TryGetValue(rActCode, out models))
            {
                return null;
            }

            return models;
        }

    }

}
