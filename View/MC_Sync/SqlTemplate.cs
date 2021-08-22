using System;
using System.Collections.Generic;
using App.BizCommon;
using EC5.IO;
using EC5.SystemBoard.Interfaces;
using EC5.Utility.Web;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;
using System.Xml.Serialization;
using System.ComponentModel;
using EC5.Utility;
using System.Data.SqlClient;
using EC5.AppDomainPlugin;
using System.Text;
using System.Transactions;
using System.Linq;
using EC5.Entity.Expanding.ExpandV1;


namespace App.InfoGrid2.MC_Sync
{

    /// <summary>
    /// 文本模板
    /// </summary>
    public class SqlTemplate
    {

        public const string L_Chat = "{{";

        public const string R_Chat = "}}";

        VTmpItemCollection m_Items = new VTmpItemCollection();

        public VTmpItemCollection Items
        {
            get { return m_Items; }
        }



        public static SqlTemplate Parse(string text)
        {
            SqlTemplate tt = new SqlTemplate();

            int n0 = text.IndexOf(SqlTemplate.L_Chat);
            int n1;

            string tmpText = text;

            while (n0 >= 0)
            {
                n1 = tmpText.IndexOf(R_Chat, n0 + 1);

                if (n1 >= 0)
                {
                    string txt = tmpText.Substring(0, n0);
                    string code = tmpText.Substring(n0 + L_Chat.Length, n1 - n0 - R_Chat.Length);

                    VTmpItem item0 = new VTmpItem(VTmpItemType.Text, txt);

                    VTmpItem item1 = new VTmpItem(VTmpItemType.Code, code);

                    tt.Items.Add(item0);
                    tt.Items.Add(item1);

                    tmpText = tmpText.Substring(n1 + R_Chat.Length);

                }
                else
                {
                    break;
                }

                n0 = tmpText.IndexOf(L_Chat);

            }

            if (tmpText.Length > 0)
            {
                VTmpItem lastItem = new VTmpItem(VTmpItemType.Text, tmpText);

                tt.Items.Add(lastItem);
            }


            

            return tt;
        }


        public SqlCommand Exec(LModel T, SModel from)
        {
            foreach (var item in this.Items)
            {
                if (item.Type != VTmpItemType.Code)
                {
                    continue;
                }

                string code = item.Text;

                if (!code.Contains("return "))
                {
                    code = "return " + code + ";";
                }

                ScriptInstance inst = ScriptFactory.Create(code);
                inst.Params["from"] = from;

                object result = inst.Exec(T);

                item.ResultValue = result;
            }


            SqlCommand cmd = new SqlCommand();

            StringBuilder sb = new StringBuilder();


            int varIndex = 0;

            foreach (var item in this.Items)
            {
                if (item.Type == VTmpItemType.Text)
                {
                    sb.Append(item.Text);
                }
                else
                {
                    string pStr = $"@var_{varIndex++}";

                    SqlParameter param = new SqlParameter(pStr, item.ResultValue ?? DBNull.Value);

                    cmd.Parameters.Add(param);

                    sb.Append(pStr);
                }
            }

            cmd.CommandText = sb.ToString();

            return cmd;

        }

    }
}