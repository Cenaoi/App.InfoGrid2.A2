using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3.Steps
{


    /// <summary>
    /// 步骤节点
    /// </summary>
    public class StepNode: CodeRegion
    {

        public StepNode()
        {

        }


        public StepNode(StepNodeType stepType)
        {
            this.StepType = stepType;
        }

        public StepNode(StepNodeType stepType, ActionItemBase actionItem)
        {
            this.StepType = stepType;
            this.ActionItem = actionItem;
        }



        /// <summary>
        /// 节点类型
        /// </summary>
        public StepNodeType StepType { get; set; }

        /// <summary>
        /// 触发源
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// 动作项目
        /// </summary>
        public ActionItemBase ActionItem { get; set; }

        /// <summary>
        /// 动作执行后, 返回的值
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 操作过的数据
        /// </summary>
        public object OperateData { get; set; }



        public override string ToString()
        {
            if(this.StepType == StepNodeType.Group)
            {
                return $"{this.StepType}, 组合";
            }
            else if(this.ActionItem is OperateTable)
            {
                OperateTable op = (OperateTable)this.ActionItem;

                return $"{this.StepType}, table={op.Table}, {op.Method}";
            }
            else if(this.ActionItem is ListenTable)
            {
                ListenTable li = (ListenTable)this.ActionItem;

                return $"{this.StepType}, table={li.Table}, {li.Method}";
            }

            return base.ToString();
        }
    }






}
