using System;
using System.Collections.Generic;
using System.Text;

namespace EasyClick.Web.Mini2
{
    /// <summary>
    /// 记录集合事件定义
    /// </summary>
    /// <param name="sender">触发源对象</param>
    /// <param name="e">事件参数</param>
    public delegate void ObjectListEventHandler(object sender, ObjectListEventArgs e);
    /// <summary>
    /// 可取消的记录事件集合定义
    /// </summary>
    /// <param name="sender">触发源对象</param>
    /// <param name="e">事件参数</param>
    public delegate void ObjectListCancelEventHandler(object sender, ObjectListCancelEventArgs e);

    /// <summary>
    /// 记录事件定义
    /// </summary>
    /// <param name="sender">触发源对象</param>
    /// <param name="e">事件参数</param>
    public delegate void ObjectEventHandler(object sender, ObjectEventArgs e);

    /// <summary>
    /// 可取消的事件定义
    /// </summary>
    /// <param name="sender">触发源对象</param>
    /// <param name="e">事件参数</param>
    public delegate void ObjectCancelEventHandler(object sender, ObjectCancelEventArgs e);

    /// <summary>
    /// 可取消的翻页事件定义
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CancelPageEventHandler(object sender, CancelPageEventArags e);
}
