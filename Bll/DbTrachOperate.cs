using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using App.InfoGrid2.Model;
using EC5.SystemBoard;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Diagnostics;
using HWQ.Entity.LightModels;
using System.Xml.Serialization;
using HWQ.Entity;
using System.Threading;
using System.Transactions;

namespace App.InfoGrid2.Bll
{


    /// <summary>
    /// 数据操作监控
    /// </summary>
    public class DbTrachOperate :ITraecOperate
    {
        static SortedList<string, bool> FilterTables = new SortedList<string, bool>()
        {
            { "LOG4_SERVER",false },
            { "LOG_ACT_OP", false},
            { "LOG_ACT_OPDATA", false },
            { "LOG_ACT",false },
            { "SEC_UI",false }
        };

        public class OpField
        {
            [XmlAttribute]
            public string Field { get; set; }

            [XmlAttribute]
            [Description("")]
            public string Text { get; set; }

            [XmlAttribute]
            public string Src { get; set; }

            [XmlAttribute]
            [Description("")]
            public string To { get; set; }
        }

        public void Send(object sender, TraceOperateEventArgs ea)
        {
            if(ea is TraceOpModelEventArgs)
            {
                LogModel((TraceOpModelEventArgs)ea);
            }

        }


        private void LogModel(TraceOpModelEventArgs ea)
        {
            if (FilterTables.ContainsKey(ea.Table))
            {
                return;
            }

            if (Transaction.Current == null)
            {
                baseLogModel(ea);
            }
            else
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    baseLogModel(ea);
                }

            }
        }

        private  void baseLogModel(TraceOpModelEventArgs ea)
        {
            HttpContext content = HttpContext.Current;
            EcUserState user = (content != null && content.Session != null) ? EcContext.Current.User : null;

            using (DbDecipher decipher = DbDecipherManager.GetDecipherOpen("trace"))
            {
                if (ea.OpMethod == TraceOperateMethod.Update)
                {
                    LogModelUpdate(decipher, HttpContext.Current, user, ea);

                }
                else if (ea.OpMethod == TraceOperateMethod.Delete)
                {
                    LogModelDelete(decipher, HttpContext.Current, user, ea);

                }
                else if (ea.OpMethod == TraceOperateMethod.Insert)
                {
                    LogModelInsert(decipher, HttpContext.Current, user, ea);

                }
            }
        }

        private void LogModelInsert(DbDecipher decipher, HttpContext content, EcUserState user, TraceOpModelEventArgs ea)
        {

            LOG_DB_TRACE model = new LOG_DB_TRACE();

            if (user != null)
            {
                model.USER_CODE = user.ExpandPropertys["USER_CODE"];
                model.LOGIN_ID = user.LoginID;
                model.LOING_NAME = user.LoginName;

                model.HOST = user.HostIP;
            }

            if (content != null && content.Session != null)
            {
                model.SESSION_ID = content.Session.SessionID;
                model.URL = Convert.ToString(content.Items["EC_ViewUri"]);

                if (model.URL.Length > 1000)
                {
                    model.URL = model.URL.Substring(0, 1000);
                }

                HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;

                model.BROWSER = bc.Browser;
               

            }


            model.THREAD = Thread.CurrentThread.ManagedThreadId.ToString();



            model.DB_TABLE = ea.Table;
            model.DB_METHOD = ea.OpMethod.ToString();
            model.EXEC_TIMESPAN = Convert.ToDecimal(ea.ExecTimeSpan.TotalMilliseconds);
            model.OP_TIME = ea.OpTime;

            if (ea.Data is LightModel)
            {
                LightModel data = (LightModel)ea.Data;


                LModelElement modelElem = ModelElemHelper.GetElem(data);


                List<OpField> opFs = new List<OpField>();

                foreach (LModelFieldElement fieldElem in modelElem.Fields)
                {
                    object toValue = data[fieldElem];

                    opFs.Add(new OpField()
                    {
                        Field = fieldElem.DBField,
                        Text = fieldElem.Description,
                        Src = toValue?.ToString()
                    });
                }

                model.OP_CONTENT = XmlUtil.Serialize(opFs, true);

            }

            decipher.InsertModel(model);
        }

        private void LogModelDelete(DbDecipher decipher, HttpContext content, EcUserState user, TraceOpModelEventArgs ea)
        {

            LOG_DB_TRACE model = new LOG_DB_TRACE();

            if (user != null)
            {
                model.USER_CODE = user.ExpandPropertys["USER_CODE"];
                model.LOGIN_ID = user.LoginID;
                model.LOING_NAME = user.LoginName;
                model.HOST = user.HostIP;
            }
            
            if (content != null && content.Session != null)
            {
                model.SESSION_ID = content.Session.SessionID;
                model.URL = Convert.ToString(content.Items["EC_ViewUri"]);

                if (model.URL.Length > 1000)
                {
                    model.URL = model.URL.Substring(0, 1000);
                }

                HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;

                model.BROWSER = bc.Browser;

            }


            model.THREAD = Thread.CurrentThread.ManagedThreadId.ToString();
            
            

            model.DB_TABLE = ea.Table;
            model.DB_METHOD = ea.OpMethod.ToString();
            model.EXEC_TIMESPAN = Convert.ToDecimal(ea.ExecTimeSpan.TotalMilliseconds);
            model.OP_TIME = ea.OpTime;

            if (ea.Data is LightModel)
            {
                LightModel data = (LightModel)ea.Data;


                LModelElement modelElem = ModelElemHelper.GetElem(data);


                List<OpField> opFs = new List<OpField>();

                foreach (LModelFieldElement fieldElem in modelElem.Fields)
                {
                    object toValue = data[fieldElem];

                    opFs.Add(new OpField()
                    {
                        Field= fieldElem.DBField,
                        Text=fieldElem.Description,
                        Src = toValue?.ToString()
                    });
                }

                model.OP_CONTENT = XmlUtil.Serialize(opFs, true);

            }

            decipher.InsertModel(model);
        }

        private string ToFields(string[] fields)
        {
            if(fields == null || fields.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < fields.Length; i++)
            {
                if(i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(fields[i]);
            }

            return sb.ToString();
        }

        private void LogModelUpdate(DbDecipher decipher, HttpContext content, EcUserState user, TraceOpModelEventArgs ea)
        {

            LOG_DB_TRACE model = new LOG_DB_TRACE();

            if (user != null)
            {
                model.USER_CODE = user.ExpandPropertys["USER_CODE"];
                model.LOGIN_ID = user.LoginID;
                model.LOING_NAME = user.LoginName;
                model.HOST = user.HostIP;
            }

            if (content != null && content.Session != null)
            {
                model.SESSION_ID = content.Session.SessionID;
                model.URL = Convert.ToString(content.Items["EC_ViewUri"]);

                if(model.URL.Length > 1000)
                {
                    model.URL = model.URL.Substring(0, 1000);
                }

                HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;

                model.BROWSER = bc.Browser;

            }


            model.THREAD = Thread.CurrentThread.ManagedThreadId.ToString();
            


            model.DB_TABLE = ea.Table;
            model.DB_METHOD = ea.OpMethod.ToString();
            model.EXEC_TIMESPAN = Convert.ToDecimal(ea.ExecTimeSpan.TotalMilliseconds);
            model.OP_TIME = ea.OpTime;

            if (ea.Data is LightModel)
            {
                LightModel data = (LightModel)ea.Data;

                if (ea.UpdateFields != null)
                {
                    LModelElement modelElem = ModelElemHelper.GetElem(data);

                    model.UPDATE_FIELDS = ToFields(ea.UpdateFields);


                    List<OpField> opFs = new List<OpField>();

                    LModelFieldElement fieldElem;

                    foreach (var field in ea.UpdateFields)
                    {
                        if (modelElem.TryGetField(field, out fieldElem))
                        {
                            if (data.GetTakeChange())
                            {
                                object fromValue = data.GetOriginalValue(fieldElem.DBField);
                                object toValue = data[fieldElem];

                                opFs.Add(new OpField()
                                {
                                    Field = fieldElem.DBField,
                                    Text = fieldElem.Description,
                                    Src = fromValue?.ToString(),
                                    To = toValue?.ToString()
                                });
                            }
                            else
                            {
                                //object fromValue = data.GetOriginalValue(fieldElem.DBField);
                                object toValue = data[fieldElem];

                                opFs.Add(new OpField()
                                {
                                    Field = fieldElem.DBField,
                                    Text = fieldElem.Description,
                                    //Src = fromValue?.ToString(),
                                    To = toValue?.ToString()
                                });
                            }
                        }
                    }

                    model.OP_CONTENT = XmlUtil.Serialize(opFs, true);
                }
                
            }

            decipher.InsertModel(model);
        }
    }
}
