using App.BizCommon;
using App.InfoGrid2.Model;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.InfoGrid2.Bll
{
    /// <summary>
    /// 展示规则管理
    /// </summary>
    public static class DisplayRuleMgr
    {
        static string m_JScriptBuffer = null;

        public static LModelList<IG2_DISPLAY_RULE> GetRules()
        {
            DbDecipher decipher = ModelAction.OpenDecipher();

            LightModelFilter filter = new LightModelFilter(typeof(IG2_DISPLAY_RULE));
            filter.And("ROW_SID", 0, Logic.GreaterThanOrEqual);
            filter.And("ENABLED", true);


            LModelList<IG2_DISPLAY_RULE> rules = decipher.SelectModels<IG2_DISPLAY_RULE>(filter);

            return rules;
        }

        /// <summary>
        /// 清理展示规则
        /// </summary>
        public static void Clear()
        {
            m_JScriptBuffer = null;
        }

        /// <summary>
        /// 获取 JS
        /// </summary>
        /// <returns></returns>
        public static string GetJScript()
        {
            if (m_JScriptBuffer != null)
            {
                return m_JScriptBuffer;
            }

            var rules = GetRules();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<script>");
            sb.AppendLine("$(document).ready(function () {");

            sb.AppendLine("Mini2.define('Mini2.DisplayRule',{");
            sb.AppendLine("  singleton:true,");
            sb.Append("  getRule:function(ruleName){ return this[ruleName]; }");

            foreach (var rule in rules)
            {
                if (StringUtil.IsBlank(rule.RULE_CODE) || 
                    StringUtil.IsBlank(rule.EX_RETURN_JS_FUN))
                {
                    continue;
                }

                sb.AppendLine(",");

                sb.AppendFormat("  '{0}':{1}", rule.RULE_CODE, rule.EX_RETURN_JS_FUN).AppendLine();
            }

            sb.AppendLine("});");

            sb.AppendLine("});");

            sb.AppendLine("</script>");

            m_JScriptBuffer = sb.ToString();

            return m_JScriptBuffer;
        }

    }
}
