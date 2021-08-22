using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web.UI;
using System.ComponentModel;
using System.Collections.Specialized;

namespace EasyClick.Web.Mini
{

    public class StoreMethodEventArgs : CancelEventArgs
    {
        IOrderedDictionary m_InputParameters;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputParameters"></param>
        public StoreMethodEventArgs(IOrderedDictionary inputParameters)
        {
            m_InputParameters = inputParameters;
        }

        /// <summary>
        /// 
        /// </summary>
        public IOrderedDictionary InputParameters
        {
            get { return m_InputParameters; }
        }

    }

    public class StoreStatusEventArgs : EventArgs
    {
        object m_ReturnValue;
        IDictionary m_OutputParameters;
        Exception m_Exception;
        int m_AffectedRows = 0;

        public StoreStatusEventArgs(object returnValue, IDictionary outputParameters)
        {
            m_ReturnValue = returnValue;
            m_OutputParameters = outputParameters;
        }


        public StoreStatusEventArgs(object returnValue, IDictionary outputParameters, Exception exception)
        {
            m_ReturnValue = returnValue;
            m_OutputParameters = outputParameters;
            m_Exception = exception;
        }

        public int AffectedRows
        {
            get { return m_AffectedRows; }
            set { m_AffectedRows = value; }
        }


        public Exception Exception
        {
            get { return m_Exception; }
        }


        public bool ExceptionHandled { get; set; }


        public IDictionary OutputParameters
        {
            get { return m_OutputParameters; }
        }


        public object ReturnValue
        {
            get { return m_ReturnValue; }
        }

    }

    public class StoreCancelEventArgs : CancelEventArgs
    {
        object m_Data;

        int m_Length;

        public StoreCancelEventArgs()
        {
            this.Cancel = false;
        }

        public StoreCancelEventArgs(int length, object data)
        {
            this.Cancel = false;

            m_Length = length;
            m_Data = data;
        }

        public StoreCancelEventArgs(int length)
        {
            this.Cancel = false;

            m_Length = length;
        }

        public object Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

        public int Length
        {
            get { return m_Length; }
        }
    }



    public delegate void StoreStatusDelegate(object sender,StoreStatusEventArgs e);

    public delegate void StoreMethodDelegate(object sender,StoreMethodEventArgs e);



    public delegate void StoreCancelDelegate(object sender,StoreCancelEventArgs e);


    /// <summary>
    /// 数据仓库抽象类
    /// </summary>
    public abstract class StoreAbstract : Control
    {
        #region 事件

        public event StoreCancelDelegate Adding;
        public event StoreStatusDelegate Added;

        public event StoreCancelDelegate Deleting;
        public event StoreStatusDelegate Deleted;

        public event StoreMethodDelegate Updateing;
        public event StoreStatusDelegate Updated;

        /// <summary>
        /// 加载数据前,触发事件
        /// </summary>
        [Description("加载数据前,触发事件")]
        public event StoreCancelDelegate Loading;

        /// <summary>
        /// 加载数据以后,触发事件
        /// </summary>
        [Description("加载数据以后,触发事件")]
        public event StoreStatusDelegate Loaded;


        public event StoreMethodDelegate Filtering;


        protected void OnFiltering(IOrderedDictionary inputParams)
        {
            if (Filtering != null) { Filtering(this, new StoreMethodEventArgs(inputParams)); }
        }

        /// <summary>
        /// 加载数据前触发事件
        /// </summary>
        /// <param name="length"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool OnLoading(int length, object data)
        {
            if (Loading != null)
            {
                StoreCancelEventArgs e = new StoreCancelEventArgs(length, data);

                Loading(this, e);

                return e.Cancel;
            }

            return false;
        }

        /// <summary>
        /// 加载数据以后触发事件
        /// </summary>
        /// <param name="length"></param>
        /// <param name="data"></param>
        protected void OnLoaded(object returnValue, IDictionary ouputParameters, Exception exception)
        {
            if (Loaded != null) { Loaded(this, new StoreStatusEventArgs(returnValue, ouputParameters, exception)); }
        }

        /// <summary>
        /// 天机数据之前触发事件
        /// </summary>
        /// <param name="length"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool OnAdding(int length, object data)
        {
            if (Adding != null)
            {
                StoreCancelEventArgs e = new StoreCancelEventArgs(length, data);

                Adding(this, e);

                return e.Cancel;
            }

            return false;
        }

        /// <summary>
        /// 添加数据以后触发事件
        /// </summary>
        /// <param name="length"></param>
        /// <param name="data"></param>
        protected void OnAdded(object returnValue, IDictionary ouputParameters, Exception exception)
        {
            if (Added != null) { Added(this, new StoreStatusEventArgs(returnValue, ouputParameters, exception)); }
        }



        protected void OnDeleted(object returnValue)
        {
            if (Deleted != null) { Deleted(this, new StoreStatusEventArgs(returnValue,null)); }
        }

        protected bool OnDeleting(int length, object data)
        {
            if (Deleting != null)
            {
                StoreCancelEventArgs e = new StoreCancelEventArgs(length, data);

                Deleting(this, e);

                return e.Cancel;
            }

            return false;
        }


        protected void OnUpdated(object returnValue,IDictionary ouputParameters,Exception exception)
        {
            if (Updated != null) { Updated(this, new StoreStatusEventArgs(returnValue,ouputParameters,exception)); }
        }

        protected bool OnUpdateing(IOrderedDictionary inputParameters)
        {
            if (Updateing != null)
            {
                StoreMethodEventArgs e = new StoreMethodEventArgs(inputParameters);

                Updateing(this, e);

                return e.Cancel;
            }

            return false;
        }

        #endregion

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        public abstract int Add();

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public abstract int Delete();

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <returns></returns>
        public abstract int Update();

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable Load();


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {


            writer.WriteLine("<script type=\"text/javascript\">");

            string jsMode = null;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("JS_MODE"))
            {
                jsMode = (string)System.Web.HttpContext.Current.Items["JS_MODE"];
            }

            if (jsMode == "InJs")
            {
                writer.Write("In.ready('mi.data.EntityStore',function()", this.ClientID);
                writer.WriteLine("{");
            }
            else
            {
                writer.WriteLine("$(document).ready(function(){");
            }


            writer.WriteLine("  var store = new Mini.ui.data.EntityStore({");
            writer.WriteLine("    id:'{0}'", this.ClientID);
            writer.WriteLine("  });");

            writer.WriteLine("  window.{0} = window.{1} = store;", this.ClientID, this.ID);


            writer.WriteLine("});");


            writer.WriteLine("</script>");

        }
    }
}
