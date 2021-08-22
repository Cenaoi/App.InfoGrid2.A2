using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EC5.Action3.CodeProcessors;
using EC5.Utility;
using HWQ.Entity.LightModels;

namespace EC5.Action3.JsonSQL
{
    public class ScriptJQL:Filter
    {
        /// <summary>
        /// 返回唯一不同的值
        /// </summary>
        public bool Distinct { get; set; } = false;

        /// <summary>
        /// 分页
        /// </summary>
        public Limit Limit { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public List<OrderField> Order { get; set; } = new List<OrderField>();

        public string OrderText { get; set; }

        /// <summary>
        /// 获取 Order 的字符串
        /// </summary>
        /// <returns></returns>
        public string GetOrderString()
        {
            if (!StringUtil.IsBlank(this.OrderText))
            {
                return this.OrderText;
            }

            if(this.Order == null || this.Order.Count == 0)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();

            int n = 0;
            
            foreach (var item in this.Order)
            {
                if(n++ > 0)
                {
                    sb.Append(", ");
                }

                sb.Append(item.Field);
                sb.Append(" ");
                sb.Append((item.Type == OrderType.ASC) ? "ASC" : "DESC");
            }

            this.OrderText = sb.ToString();

            return this.OrderText;
        }

        /// <summary>
        /// 过滤条件
        /// </summary>
        public Filter Where { get; set; }

        public override bool Valid(CodeContext context, object item)
        {
            if (item is SModel)
            {
                return Valid((SModel)item);
            }
            else if (item is LightModel)
            {
                return Valid((LightModel)item);
            }

            return false;
        }

        /// <summary>
        /// 验证条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override bool Valid(SModel model)
        {
            bool result = true;

            result = FilterValidateSModel.ValidGroup(model, this.Where);

            return result;
        }

        /// <summary>
        /// 验证条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override bool Valid(LightModel model)
        {
            bool result = true;

            result = FilterValidateLightModel.ValidGroup(model, this.Where);

            return result;
        }


        /// <summary>
        /// 解析过滤条件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public new static ScriptJQL ParseJson(string data)
        {
            ScriptJQL jql = new ScriptJQL();

            SModel sm = SModel.ParseJson(data);
            
            SModelList where = sm["where"];

            Filter filter = new Filter();

            Filter.ParseSModel(filter, where);


            jql.Where = filter;

            if (sm["distinct"] != null)
            {
                jql.Distinct = Convert.ToBoolean(sm["distinct"]);
            }


            object limit = sm["limit"];

            if(limit != null)
            {
                if( TypeUtil.IsNumberType(limit.GetType()))
                {
                    jql.Limit = new Limit(Convert.ToInt32( limit));
                }
                else if(limit.GetType() == typeof(SModel))
                {
                    SModel limitSM = (SModel)limit;
                    int count = limitSM.Get<int>("count");
                    int start = limitSM.Get<int>("start");

                    jql.Limit = new Limit(start, count);
                }
            }

            if (sm["order"] != null)
            {
                if (  sm["order"] is string)
                {
                    jql.OrderText = sm["order"];
                }
                else
                {
                    IList orderFields = sm["order"];

                    if (orderFields != null)
                    {
                        foreach (var item in orderFields)
                        {
                            if (item == null)
                            {
                                continue;
                            }

                            if (item is SModel)
                            {
                                SModel orderSM = (SModel)item;

                                foreach (var order in orderSM.GetFields())
                                {
                                    OrderType ot = EnumUtil.Parse<OrderType>(orderSM[order], OrderType.ASC, true);

                                    jql.Order.Add(new OrderField(order, ot));
                                }

                            }
                            else if (item.GetType() == typeof(string))
                            {
                                jql.Order.Add(new OrderField((string)item));
                            }
                        }
                    }
                }
            }


            return jql;
        }
    }
}
