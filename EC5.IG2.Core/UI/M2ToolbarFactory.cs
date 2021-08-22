using System;
using System.Collections.Generic;
using System.Web;
using App.InfoGrid2.Model;
using App.InfoGrid2.Model.DataSet;
using EasyClick.Web.Mini2;
using EC5.Utility;

namespace EC5.IG2.Core.UI
{
    public class M2ToolbarFactory
    {

        string m_StoreId ="store1";

        string m_TableId;

        string m_SearchFormID = "searchForm";

        /// <summary>
        /// 查询的表单
        /// </summary>
        public string SearchFormID
        {
            get { return m_SearchFormID; }
            set { m_SearchFormID = value; }
        }

        /// <summary>
        /// 数据仓库ID
        /// </summary>
        public string StoreId
        {
            get { return m_StoreId; }
            set { m_StoreId = value; }
        }

        /// <summary>
        /// 表格ID
        /// </summary>
        public string TableId
        {
            get { return m_TableId; }
            set { m_TableId = value; }
        }



        /// <summary>
        /// 创建工具栏条目
        /// </summary>
        /// <param name="tSet"></param>
        public void CreateItems(Toolbar toolbar, ToolbarSet tSet, TableSet tableSet)
        {
            if (tSet.Toolbar == null)
            {
                CreateDefaultItems(toolbar);   //创建默认按钮
                return;
            }


            foreach (IG2_TOOLBAR_ITEM item in tSet.Items)
            {
                if (item.ROW_SID < 0 || !item.VISIBLE)
                {
                    continue;
                }

                ToolBarItem tItem = ConvertFrom(item, toolbar, tableSet);

                toolbar.Items.Add(tItem);
            }

            /*
             * 添加权限，
             * 1.把权限取出来。 
             * 2.把控件找出来.
             * 3.对控件设置权限。
             * 
             */




        }



        /// <summary>
        /// 创建工具栏条目
        /// </summary>
        /// <param name="tSet"></param>
        public void CreateItems(Toolbar toolbar, ToolbarSet tSet)
        {
            CreateItems(toolbar, tSet, null);

        }



        /// <summary>
        /// 创建默认按钮
        /// </summary>
        private void CreateDefaultItems(Toolbar toolbar)
        {
            ToolBarButton insertBtn = new ToolBarButton("新建");
            insertBtn.OnClick = string.Format("ser:{0}.Insert()", m_StoreId);
            toolbar.Items.Add(insertBtn);

            ToolBarButton saveBtn = new ToolBarButton("保存");
            saveBtn.OnClick = string.Format("ser:{0}.SaveAll()", m_StoreId);
            toolbar.Items.Add(saveBtn);

            ToolBarButton refreshBtn = new ToolBarButton("刷新");
            refreshBtn.OnClick = string.Format("ser:{0}.Refresh()", m_StoreId);
            toolbar.Items.Add(refreshBtn);


            toolbar.Items.Add("-");

            ToolBarButton searchBtn = new ToolBarButton("查询");
            searchBtn.OnClick = "try{ " + string.Format("widget1_I_{0}.toggle()", m_SearchFormID) + "; } catch(ex){ alert('执行错误:' + ex.description ); }";
            toolbar.Items.Add(searchBtn);

            toolbar.Items.Add("-");

            ToolBarButton toExcelBtn = new ToolBarButton("导出");
            toExcelBtn.Command = "ToExcel";
            toolbar.Items.Add(toExcelBtn);

            toolbar.Items.Add("-");

            ToolBarButton deleteBtn = new ToolBarButton("删除");
            deleteBtn.BeforeAskText = "您确定删除记录?";
            deleteBtn.OnClick = string.Format("ser:{0}.Delete()", m_StoreId);
            toolbar.Items.Add(deleteBtn);
        }

        private ToolBarItem ConvertFrom(IG2_TOOLBAR_ITEM item, Toolbar toolbar, TableSet tableSet)
        {
            ToolBarItem tItem = null;

            string itemType = item.ITEM_TYPE_ID.ToUpper();

            switch (itemType)
            {
                case "HR":
                    ToolBarHr hr = new ToolBarHr();
                    hr.Visible = item.VISIBLE;
                    hr.Enabled = item.ENABLED;
                    tItem = hr;
                    break;
                case "TITLE":

                    ToolBarTitle title = new ToolBarTitle(item.ITEM_TEXT);

                    tItem = title;


                    break;
                default:
                    ToolBarButton btn = new ToolBarButton(item.ITEM_TEXT);
                    btn.Align = EnumUtil.Parse<ToolBarItemAlign>(item.ALIGN, ToolBarItemAlign.Left);
                    btn.BeforeAskText = item.ASK_MSG;
                    btn.OnBeforeClick = item.ON_BEFORE_CLICK;
                    btn.Command = item.COMMAND;
                    btn.Params = item.COMMAND_PARAMS;
                    btn.Tooltip = item.TOOLTIP;

                    btn.Icon = item.ICON_PATH;


                    if (!StringUtil.IsBlank(item.COMMAND_PARAMS))
                    {
                        btn.Params = item.COMMAND_PARAMS.Replace("'", "\'");
                    }


                    tItem = btn;

                    if (item.EVENT_MODE_ID == "SYS")
                    {
                        string itemId = item.ITEM_ID;

                        if (m_DefaultItems.TryGetValue(itemId, out string srcCodeFormat))
                        {
                            switch (itemId)
                            {
                                case "SEARCH":
                                    btn.OnClick = "try{ " + string.Format(srcCodeFormat, m_SearchFormID) + "; } catch(ex){ alert('执行错误:' + ex.description ); }";
                                    break;
                                case "TO_EXCEL": btn.Command = "ToExcel"; break;
                                case "TO_PRINT": btn.Command = "ToPrint"; break;
                                case "TO_PRINT_MAIN": btn.Command = "ToPrintMain"; break;
                                case "CHANGE_BIZ_SID":
                                    btn.Command = "ChangeBizSID";

                                    //参数特殊处理， 加上 表格id，和数据仓库的id
                                    btn.Params += "," + m_TableId + "," + m_StoreId;
                                    break;
                                case "CHANGE_FIELD": btn.Command = "ChangeField"; break;
                                case "SHOW_CATA_CHANGE": btn.OnClick = "showCataDialog(this)"; break;
                                case "CHANGE_BIZ_SID_ALL": btn.Command = "ChangeBizSIDALL"; break;
                                case "CHANGE_FIELD_NOW": btn.Command = "ChangeFieldNow"; break;
                                case "CHANGE_FSTABLE": btn.Command = "ChangeFieldSubTable"; break; //改变子表的值
                                case "CLOSE_AND_NEW": btn.Command = "CloseAndNew"; break;
                                default:
                                    btn.OnClick = string.Format(srcCodeFormat, m_StoreId);
                                    break;
                            }

                        }
                    }
                    else if (item.EVENT_MODE_ID == "PLUG")
                    {
                        btn.Command = "ExecPlugin";
                        btn.Params = item.IG2_TOOLBAR_ITEM_ID + ";" + m_StoreId + ";" + m_TableId + ";" + m_SearchFormID;
                    }
                    else
                    {
                        btn.OnClick = item.ON_CLICK;
                    }


                    break;
            }

            if (tItem != null)
            {
                tItem.ID = "toolbarItem" + item.IG2_TOOLBAR_ITEM_ID;
                tItem.Visible = item.VISIBLE ;
                tItem.Enabled = item.ENABLED;
                tItem.Align = EnumUtil.Parse<ToolBarItemAlign>(item.ALIGN, ToolBarItemAlign.Left);
            }

            return tItem;
        }




        //INSERT,SAVE,REFRESH,SEARCH,TO_EXCEL,DELETE

        static SortedList<string,string> m_DefaultItems = new SortedList<string,string>(){
            { "INSERT", "ser:{0}.Insert()"},
            { "REFRESH", "ser:{0}.Refresh()"},
            { "SAVE", "ser:{0}.SaveAll()"},
            { "DELETE", "ser:{0}.Delete()"},
            { "SEARCH", "widget1_I_{0}.toggle()"},
            { "TO_EXCEL", ""},
            { "TO_PRINT", ""},
            { "TO_PRINT_MAIN", ""},
            {"CHANGE_FIELD",""},
            {"CHANGE_BIZ_SID",""},
            {"CHANGE_BIZ_SID_ALL","" },    //业务状态改变,全部
            {"SHOW_CATA_CHANGE","" },    //显示目录改变
            {"CHANGE_FIELD_NOW","" },
            {"CHANGE_FSTABLE","" },   //改变子表的值
            {"CLOSE_AND_NEW","" } //关闭并新建
        };


    }
}