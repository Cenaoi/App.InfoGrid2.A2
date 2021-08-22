using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace EasyClick.Web.Mini2
{

    /// <summary>
    /// 表单绑定
    /// </summary>
    public interface IFormLayoutBindings
    {
        

        /// <summary>
        /// 实体名称
        /// </summary>
        string Model { get; set; }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        object GetData(FormLayout layout);

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="data"></param>
        void SetData(FormLayout layout, object data);

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="data"></param>
        void EditData(FormLayout layout, object data);
   
    }






    


    /// <summary>
    /// 数据库引擎接口
    /// </summary>
    public interface IStoreEngine
    {

        

        Store Store { get;  set; }

        /// <summary>
        /// 当前页面索引
        /// </summary>
        int CurPage { get; set; }

        /// <summary>
        /// 记录总数
        /// </summary>
        int ItemTotal { get; set; }

        /// <summary>
        /// 每页显示的记录数量
        /// </summary>
        int PageSize { get; set; }


        /// <summary>
        /// 加载汇总信息
        /// </summary>
        void LoadSummary();


        /// <summary>
        /// 加载
        /// </summary>
        void OnLoad();

        /// <summary>
        /// 按页加载数据
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        IList LoadPage(int page);


        /// <summary>
        /// 筛选选择数据
        /// </summary>
        /// <returns></returns>
        IList Select();

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        int SaveAll();


        /// <summary>
        /// 更新数据
        /// </summary>
        /// <returns></returns>
        int Update();


        /// <summary>
        /// 插入数据
        /// </summary>
        /// <returns></returns>
        int Insert();


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        int Delete();

        /// <summary>
        /// 焦点行上移
        /// </summary>
        /// <returns></returns>
        bool MoveUp();

        /// <summary>
        /// 焦点行下移
        /// </summary>
        /// <returns></returns>
        bool MoveDown();

        /// <summary>
        /// 
        /// </summary>
        bool ContainsListCollection { get; }


        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        IList GetList();

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        DataTable GetDataTable();

        /// <summary>
        /// 加载汇总
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="summartyType">汇总类型</param>
        /// <returns>汇总值</returns>
        decimal GetSummary(string field, SummaryType summartyType);

        /// <summary>
        /// 加载汇总
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="summartyType">汇总类型</param>
        /// <param name="summaryFilter"></param>
        /// <returns>汇总值</returns>
        decimal GetSummary(string field, SummaryType summartyType, ParamCollection summaryFilter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        void SetCurRecord(EasyClick.Web.Mini2.Data.DataRecord record);

        /// <summary>
        /// 重建排序索引
        /// </summary>
        /// <returns></returns>
        bool SortReset();

        /// <summary>
        /// 触发状态修改
        /// </summary>
        /// <param name="record"></param>
        /// <param name="data"></param>
        void PreCurrentChanged(Data.DataRecord record,object data);

        /// <summary>
        /// 判断是否有子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        bool HasChild(object parent);

    }

    /// <summary>
    /// 树控件引擎
    /// </summary>
    public interface ITreeStoreEngine
    {
        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="oldText"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        bool Rename( string oldText, string text);
    }
}
