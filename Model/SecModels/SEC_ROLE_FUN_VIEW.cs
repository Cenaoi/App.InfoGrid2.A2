
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using HWQ.Entity.LightModels;

namespace App.InfoGrid2.Model.SecModels
{
    /// <summary>
    /// 
    /// </summary>
    [DBTable("SEC_ROLE_FUN_VIEW", DBTableMode.Virtual)]
    [Description("")]
    [Serializable]
    public class SEC_FUN_DEF_VIEW : SEC_FUN_DEF
    {


        /// <summary>
        /// 
        /// </summary>
        [DBField]
        [DisplayName("")]
        [UIField(visible = true)]
        public Boolean VISIBLE { get; set; }


    }

}