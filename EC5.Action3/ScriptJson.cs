using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EC5.Action3
{
    /// <summary>
    /// Json 脚本
    /// </summary>
    public sealed class ScriptJson : ScriptBase
    {
        public ScriptJson()
        {

        }

        public ScriptJson(string code)
        {
            this.Code = code;
        }


        public ScriptJson(object obj)
        {
            SModel sm = SModel.Parse(obj);
            
            this.Code = sm.ToJson();
        }

        /// <summary>
        /// 已经编译
        /// </summary>
        bool m_IsCompiled = false;

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="code"></param>
        private void Compile(string code)
        {
            SModel smSQL = SModel.ParseJson(code);




            m_IsCompiled = true;
        }
    }



}
