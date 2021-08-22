using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.LightModels;

namespace EasyClick.Web.Mini2.Data
{
    /// <summary>
    /// Tree 数据仓库引擎
    /// </summary>
    public class EntityTreeStoreEngine : EntityStoreEngine, ITreeStoreEngine
    {
        public new TreeStore Store
        {
            get { return base.Store as TreeStore; }
            set { base.Store = value; }
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="oldText"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool Rename(string oldText, string text)
        {
            TreeStore store = this.Store;

            DbDecipher decipher = this.Decipher;

            LightModelFilter filter = new LightModelFilter(store.Model);
            filter.And(store.IdField, store.CurDataId);

            LModel model = decipher.GetModel(filter);

            if (model == null)
            {
                return false;
            }

            model.SetTakeChange(true);

            model[store.TextField] = text;


            store.OnProUpdateing(null, model, null);

            int count = decipher.UpdateModelProps(model, new string[] { store.TextField });

            store.OnProUpdated(null, model, null);


            return count == 1;
        }
    }
}
