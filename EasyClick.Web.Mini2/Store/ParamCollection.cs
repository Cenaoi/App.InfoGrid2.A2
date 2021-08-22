using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 参数集合
    /// </summary>
    public class ParamCollection : List<Param>
    {
        /// <summary>
        /// 参数集合
        /// </summary>
        public ParamCollection()
        {
        }

        //public void UpdateValues(HttpContext context, System.Web.UI.Control control)
        //{
        //    foreach (Param p in this)
        //    {
        //        object value = p.Evaluate(context, control);


        //    }

        //}


        /// <summary>
        /// 将对象添加多到数组结尾处
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Param Add(string name, string defaultValue)
        {
            Param p = new Param(name, defaultValue);

            base.Add(p);

            return p;
        }

        /// <summary>
        /// 将对象添加多到数组结尾处
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Param Add(string name, object defaultValue)
        {
            Param p = new Param(name, defaultValue);

            base.Add(p);

            return p;
        }

        public Param Add(string name, string defaultValue, System.Data.DbType dbType)
        {
            Param p = new Param(name, defaultValue, dbType);

            base.Add(p);

            return p;
        }

        /// <summary>
        /// 将对象添加多到数组结尾处
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Param Add(string name, object defaultValue, System.Data.DbType dbType)
        {
            Param p = new Param(name, defaultValue);
            p.DbType = dbType;

            base.Add(p);

            return p;
        }

        public virtual string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;

            sb.Append("[");

            foreach (var item in this)
            {
                if (i++ > 0) { sb.Append(", "); }
                sb.Append(item.ToJson());
            }

            sb.Append("]");

            string json = sb.ToString();

            return json;
        }

        public override string ToString()
        {
            return ToJson();
        }

    }
}
