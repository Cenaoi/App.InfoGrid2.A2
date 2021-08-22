using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyClick.Web.Mini2.Data;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 命令事件
    /// </summary>
    public class TableCommandEventArgs :EventArgs
    {

        /// <summary>
        /// 命令事件
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="commandParam"></param>
        /// <param name="record"></param>
        public TableCommandEventArgs(string commandName, string commandParam, DataRecord record)
        {
            this.CommandName = commandName;
            this.CommandParam = commandParam;
            this.Record = record;
        }


        /// <summary>
        /// 命令名称
        /// </summary>
        public string CommandName { get; internal set; }

        /// <summary>
        /// 命令参数
        /// </summary>
        public string CommandParam { get; internal set; }


        /// <summary>
        /// 当前记录数据
        /// </summary>
        public DataRecord Record { get; internal set; }

    }


    /// <summary>
    /// 命令事件
    /// </summary>
    public class DataViewCommandEventArgs :EventArgs
    {

        /// <summary>
        /// 命令事件
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="commandParam"></param>
        /// <param name="record"></param>
        public DataViewCommandEventArgs(string commandName, string commandParam, DataRecord record)
        {
            this.CommandName = commandName;
            this.CommandParam = commandParam;
            this.Record = record;
        }


        /// <summary>
        /// 命令名称
        /// </summary>
        public string CommandName { get; internal set; }

        /// <summary>
        /// 命令参数
        /// </summary>
        public string CommandParam { get; internal set; }


        /// <summary>
        /// 当前记录数据
        /// </summary>
        public DataRecord Record { get; internal set; }

    }
}
