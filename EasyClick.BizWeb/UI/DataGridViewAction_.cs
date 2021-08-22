using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using HWQ.Entity.Decipher.LightDecipher;
using System.ComponentModel;

namespace EasyClick.BizWeb.UI
{
    public class ModelEventArgs : EventArgs
    {
        internal DbDecipher m_Decipher;

        internal object m_Model;

        public ModelEventArgs(object model)
        {
            m_Model = model;
        }

        public ModelEventArgs(object model, DbDecipher decipher)
        {
            m_Model = model;
            m_Decipher = decipher;
        }

        public object Model
        {
            get { return m_Model; }
            set { m_Model = value; }
        }

        public DbDecipher Decipher
        {
            get { return m_Decipher; }
            set { m_Decipher = value; }
        }
    }

    public class ModelEventArgs<ModelT> : ModelEventArgs
    {
        public ModelEventArgs(ModelT model)
            : base(model)
        {
            m_Model = model;
        }

        public ModelEventArgs(ModelT model, DbDecipher decipher)
            : base(model, decipher)
        {
            m_Decipher = decipher;
            m_Model = model;
        }

        public ModelT Model
        {
            get { return (ModelT)m_Model; }
        }

    }

    public class ModelCancelEventArgs : CancelEventArgs
    {
        internal DbDecipher m_Decipher;
        internal object m_Model;
        internal string[] m_Fields;

        public ModelCancelEventArgs(object model, string[] fields)
        {
            m_Model = model;
            m_Fields = fields;
        }

        /// <summary>
        /// 被更新的字段
        /// </summary>
        public string[] Fields
        {
            get { return m_Fields; }
        }

        public ModelCancelEventArgs(object model)
        {
            this.Cancel = false;
            m_Model = model;
        }

        public ModelCancelEventArgs(object model, DbDecipher decipher)
        {
            m_Decipher = decipher;
            m_Model = model;
        }

        public object Model
        {
            get { return m_Model; }
        }

        public DbDecipher Decipher
        {
            get { return m_Decipher; }
        }
    }

    public class ModelCancelEventArgs<ModelT> : ModelCancelEventArgs
    {
        public ModelCancelEventArgs(ModelT model, string[] fields)
            : base(model, fields)
        {
            m_Model = model;
            m_Fields = fields;
        }

        public ModelCancelEventArgs(ModelT model)
            : base(model)
        {
            this.Cancel = false;
            m_Model = model;
        }

        public ModelCancelEventArgs(ModelT model, DbDecipher decipher)
            : base(model, decipher)
        {
            m_Decipher = decipher;
            m_Model = model;
        }


        public new ModelT Model
        {
            get { return (ModelT)m_Model; }
        }

    }


    public class ModelListEventArgs : EventArgs
    {
        IList m_Models;

        public ModelListEventArgs(IList models)
        {
            m_Models = models;
        }

        public IList Models
        {
            get { return m_Models; }
        }
    }

    public class ModelListEventArgs<ModelT> : EventArgs
    {
        IList<ModelT> m_Models;

        public ModelListEventArgs(IList<ModelT> models)
        {
            m_Models = models;
        }

        public IList<ModelT> Models
        {
            get { return m_Models; }
        }
    }

    public class ModelListCancelEventArgs : EventArgs
    {
        IList<object> m_Models;

        public ModelListCancelEventArgs(IList<object> models)
        {
            m_Models = models;
        }

        public IList<object> Models
        {
            get { return m_Models; }
        }
    }


    public class ModelListCancelEventArgs<ModelT> : EventArgs
    {
        IList<ModelT> m_Models;

        public ModelListCancelEventArgs(IList<ModelT> models)
        {
            m_Models = models;
        }

        public IList<ModelT> Models
        {
            get { return m_Models; }
        }
    }

    public delegate void ModelCancelEventHandler(object sender, ModelCancelEventArgs e);

    public delegate void ModelCancelEventHandler<ModelT>(object sender, ModelCancelEventArgs<ModelT> e);

}
