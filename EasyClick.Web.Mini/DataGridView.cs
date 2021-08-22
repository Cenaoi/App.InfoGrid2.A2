using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Web.UI;

namespace EasyClick.Web.Mini
{
    /// <summary>
    /// 可编辑表格
    /// </summary>
    [DisplayName("可编辑表格")]
    public class DataGridView : DataGrid, ISecCommand,ISecControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 数据主键
        /// </summary>
        string m_DataKeys;

        /// <summary>
        /// 锁行标记字段名
        /// </summary>
        string m_LockedKey = "_MODEL_LOCKED";

        bool m_ReadOnly = false;

        /// <summary>
        /// 固定字段
        /// </summary>
        string m_FixedFields;

        string m_OnFocused;

        /// <summary>
        /// 焦点触发的事件名称
        /// </summary>
        [DefaultValue("")]
        [Description("焦点触发的事件名称")]
        public string OnFocused
        {
            get { return m_OnFocused; }
            set { m_OnFocused = value; }
        }


        /// <summary>
        /// 固定字段，用于必须返回的值
        /// </summary>
        [DefaultValue(""),Description("固定字段，用于必须返回的值")]
        public string FixedFields
        {
            get { return m_FixedFields; }
            set { m_FixedFields = value; }
        }

        /// <summary>
        /// 只读.
        /// </summary>
        [DefaultValue(false),Description("只读")]
        public bool ReadOnly
        {
            get { return m_ReadOnly; }
            set
            {
                m_ReadOnly = value;

                if (this.DesignMode)
                {
                    return;
                }

                MiniHelper.Eval(string.Format("{0}.readOnly({1})", this.GetClientID(), value.ToString().ToLower()));
            }
        }

        /// <summary>
        /// 锁行标记字段名
        /// </summary>
        [DefaultValue("_MODEL_LOCKED")]
        [Description("锁行标记字段名")]
        public string LockedKey
        {
            get { return m_LockedKey; }
            set { m_LockedKey = value; }
        }

        /// <summary>
        /// 数据主键。字段1,字段2,...
        /// </summary>
        [Description("数据主键。字段1,字段2,...")]
        public string DataKeys
        {
            get { return m_DataKeys; }
            set { m_DataKeys = value; }
        }


        /// <summary>
        /// 数据仓库变化状态
        /// </summary>
        HiddenField m_DataStoreStatusHid;

        HiddenField m_SelectedStatusHid;

        public DataGridView()
            : base()
        {
            m_DataStoreStatusHid = new HiddenField();
            m_SelectedStatusHid = new HiddenField();

        }

        /// <summary>
        /// 数据记录的选择状态
        /// </summary>
        [Description("数据记录的选择状态")]
        [Browsable(false)]
        [DefaultValue("")]
        public string SelectedStatus
        {
            get
            {
                if (m_SelectedStatusHid == null)
                {
                    return string.Empty;
                }

                return m_SelectedStatusHid.Value;
            }
        }

        /// <summary>
        /// 数据仓库变化状态
        /// </summary>
        [Description("数据仓库变化状态")]
        [Browsable(false)]
        [DefaultValue("")]
        public string DataStoreStatus
        {
            get
            {
                if (m_DataStoreStatusHid == null)
                {
                    return string.Empty;
                }

                return m_DataStoreStatusHid.Value;
            }
        }


        DataSeletedItem m_FocusedItem = null;

        /// <summary>
        /// 获取焦点的行
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable( EditorBrowsableState.Advanced)]
        [Description("获取焦点的行")]
        [DefaultValue("")]
        public DataSeletedItem FocusedItem
        {
            get
            {
                if (m_FocusedItem != null)
                {
                    return m_FocusedItem;
                }

                DataSeletedItem item = new DataSeletedItem();

                if (m_DataKeys == "$P.guid")
                {
                    item.Guid = GetFocusedItemValue();
                    item.Pk = GetFocusedItemFixedValue();
                }
                else
                {
                    item.Pk = GetFocusedItemValue();
                }

                item.Index = GetFocusedItemIndex();

                m_FocusedItem = item;

                return item;
            }
        }

        /// <summary>
        /// 设置焦点行
        /// </summary>
        /// <param name="itemIndex">行索引</param>
        public void SetItemFocus(int itemIndex)
        {
            MiniHelper.Eval(string.Format("{0}.setItemFocus({1})", this.GetClientID(),itemIndex));
        }


        /// <summary>
        /// 清理数据仓库的数据(支持 JS)
        /// </summary>
        public void ClearDataStoreStatus()
        {
            m_DataStoreStatusHid.Value = string.Empty;


            MiniHelper.Eval(string.Format("{0}.clearDataStoreStatus()",this.GetClientID()));
        }

        /// <summary>
        /// 清理被选中的记录
        /// </summary>
        public void ClearSelectedStatus()
        {
            m_SelectedStatusHid.Value = string.Empty;

            MiniHelper.Eval(string.Format("{0}.clearSelectedStatus()", this.GetClientID()));

            SelectItemField field = this.Columns.Find(delegate(BoundField bf)
            {
                return (bf is SelectItemField);
            }) as SelectItemField;

            if (field != null)
            {
                MiniHelper.Eval(string.Format("{0}_{1}_EditorCell.clearAll()", this.ClientID, field.ID));
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            m_DataStoreStatusHid.ID = this.ID + "_DSStatus";
            this.Controls.Add(m_DataStoreStatusHid);


            m_SelectedStatusHid.ID = this.ID + "_SStatus";
            this.Controls.Add(m_SelectedStatusHid);



        }

        public override StoreAbstract GetStore()
        {
            if (!string.IsNullOrEmpty(this.StoreId))
            {
                Control con = FindWidget();

                StoreAbstract store = con.FindControl(this.StoreId) as StoreAbstract;

                m_Store = store;

                store.Loaded += new StoreStatusDelegate(store_Loaded);
                store.Added += new StoreStatusDelegate(store_Added);
                store.Deleting += new StoreCancelDelegate(store_Deleting);
                store.Deleted += new StoreStatusDelegate(store_Deleted);

                store.Updateing += new StoreMethodDelegate(store_Updateing);
                store.Updated += new StoreStatusDelegate(store_Updated);

            }

            return m_Store;
        }

        void store_Updated(object sender, StoreStatusEventArgs e)
        {
            this.ClearDataStoreStatus();
            this.ClearModifiedTagAll();
        }

        protected void store_Updateing(object sender, StoreMethodEventArgs e)
        {
            DataStoreStatus dss;

            if (!string.IsNullOrEmpty(this.DataStoreStatus))
            {
                dss = Mini.DataStoreStatus.Parse(this.DataStoreStatus);
            }
            else
            {
                dss = new DataStoreStatus();
            }

            e.InputParameters.Add("DataBatch", dss);

        }




        List<string> m_InitJavaScript = new List<string>();

        /// <summary>
        /// 填充数据仓库
        /// </summary>
        public void FillDataStore()
        {
            string js = string.Format("{0}.fillDataStore();", this.GetClientID());

            m_InitJavaScript.Add(js);

            MiniHelper.Eval(js);
            
        }

        /// <summary>
        /// 重新设置
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            MiniHelper.Eval(string.Format("{0}.reset()", GetClientID()));
        }

        /// <summary>
        /// 新建行
        /// </summary>
        public void NewRow()
        {
            string js = string.Format("{0}.newRow();", this.GetClientID());

            m_InitJavaScript.Add(js);

            MiniHelper.Eval(js);
        }

        /// <summary>
        /// 清楚全部修改标记
        /// </summary>
        public void ClearModifiedTagAll()
        {
            string js = string.Format("$('#{0} tbody').find('.Modified').removeClass('Modified');", this.GetClientID());

            m_InitJavaScript.Add(js);

            MiniHelper.Eval(js);
        }

        /// <summary>
        /// 设置单元格值（支持 JS）
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <param name="dbField">字段名称</param>
        /// <param name="value">值</param>
        public void SetCellValueForGuid(string pkValue, string dbField,string value)
        {
            string js = string.Format("{0}.setCellValueForGuid(\"{1}\",\"{2}\",\"{3}\");", this.GetClientID(), pkValue, dbField, value);

            MiniHelper.Eval(js);
        }

        /// <summary>
        /// 设置单元格值（支持 JS）
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <param name="dbField">字段名称</param>
        /// <param name="value">值</param>
        public void SetFixedValueForGuid(string pkValue, string dbField, string value)
        {
            string js = string.Format("{0}.setFixedValueForGuid(\"{1}\",\"{2}\",\"{3}\");", this.GetClientID(), pkValue, dbField, value);

            MiniHelper.Eval(js);
        }

        /// <summary>
        /// 输出客户端脚本
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderClientScript(System.Web.UI.HtmlTextWriter writer)
        {
            writer.WriteLine("<script type=\"text/javascript\">");

            writer.WriteLine("var {0} = null;", this.GetClientID());

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }

            if (jsMode == "InJs")
            {
                writer.WriteLine("In.ready('mi.DataGridView',function(){");
            }
            else
            {
                writer.WriteLine("$(document).ready(function(){");
            }

            writer.WriteLine("    window.{0} = new Mini.ui.DataGridView({{", this.GetClientID());
            writer.WriteLine("        id:'{0}', ", this.GetClientID());
            writer.WriteLine("        oddClass:'{0}', ", this.OddClass);
            writer.WriteLine("        dataStoreStatusId:'#{0}', ", this.m_DataStoreStatusHid.ClientID);
            writer.WriteLine("        selectedStatusId:'#{0}', ", this.m_SelectedStatusHid.ClientID);
            writer.WriteLine("        focusConId:'#{0}', ", this.FocusRow.ClientID);
            writer.WriteLine("        dataKeys:'{0}', ", this.DataKeys);
            writer.WriteLine("        fixedFields:'{0}', ", this.FixedFields);
            writer.WriteLine("        readOnly:{0},", this.ReadOnly.ToString().ToLower());
            writer.WriteLine("        lockedKey:'{0}' ", this.LockedKey);
            writer.WriteLine("    });");

            writer.WriteLine("    {0}.setTemplate({1});", this.GetClientID(),this.Template.ClientID);

            writer.WriteLine("    {0}.reset();", this.GetClientID());

            writer.WriteLine("});");

            writer.WriteLine("</script>");

            
        }

        /// <summary>
        /// 输出控件 Html 代码
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                if (!App.Register.RegHelp.IsRegister()) throw new Exception("调用限制：未注册");
            }

            this.CreateChildControls();

            
            base.Render(writer);
            
            m_DataStoreStatusHid.RenderControl(writer);
            m_SelectedStatusHid.RenderControl(writer);

            if (this.DesignMode)
            {
                return;
            }

            writer.WriteLine("<script type=\"text/javascript\">");


            writer.WriteLine("  function {0}_InitEditorCell() {{", GetClientID());

            writer.WriteLine("    try{");

            foreach (BoundField boundField in this.Columns)
            {
                EditorTextCell editorCell = boundField as EditorTextCell;

                if (editorCell == null)
                {
                    continue;
                }

                editorCell.RenderClientScript(writer);

            }

            writer.Write("    } catch(ex) { alert('ERROR:' + ex.Messsage); } ");

            writer.WriteLine("  }");

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }

            if (jsMode == "InJs")
            {
                writer.WriteLine("In.ready('mi.DataGridView',{0}_InitEditorCell);", GetClientID());
            }
            else
            {
                writer.WriteLine("$(document).ready({0}_InitEditorCell);", GetClientID());
            }


            if (m_InitJavaScript.Count > 0)
            {

                if (jsMode == "InJs")
                {
                    writer.WriteLine("In.ready('mi.DataGridView',function(){");
                }
                else
                {
                    writer.WriteLine("$(document).ready(function(){");
                }

                foreach (string js in m_InitJavaScript)
                {
                    writer.WriteLine(js);
                }

                writer.WriteLine("  });");
            }

            writer.WriteLine("</script>");
        }


        #region IMiniControl 成员

        public new void LoadPostData()
        {
            base.LoadPostData();


        }

        #endregion


        public void SecCommand(string funCode)
        {
            try
            {
                this.ReadOnly = !Boolean.Parse(funCode);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        string m_SecFunCode;
        string m_SecFunID;


        [Category("Security")]
        public string SecFunCode
        {
            get { return m_SecFunCode; }
            set { m_SecFunCode = value; }
        }


        [Category("Security")]
        public string SecFunID
        {
            get { return m_SecFunID; }
            set { m_SecFunID = value; }
        }


        [Category("Security")]
        [Browsable(false)]
        public SecControlCollection SecControls
        {
            get { return null; }
        }
    }
}
