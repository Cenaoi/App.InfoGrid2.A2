using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.InfoGrid2.Model.AC3;
using EC5.Action3.Xml;
using EC5.Utility;
using HWQ.Entity.Decipher.LightDecipher;
using HWQ.Entity.Filter;
using HWQ.Entity.LightModels;

namespace EC5.IG2.Core
{
    /// <summary>
    /// Ac3 数据操作类
    /// </summary>
    public class Ac3DataMgr
    {


        public XDocument ToDocXml(DbDecipher decipher )
        {
            XDocument doc = new XDocument();
            
            List<AC3_DWG> models = decipher.SelectModels<AC3_DWG>("ROW_SID >= 0");

            foreach (var item in models)
            {
                XPanel xp = ToPanelXml(decipher, item);

                doc.Library.Panels.Add(xp);
            }

            return doc;
        }

        public XPanel ToPanelXml(DbDecipher decipher, AC3_DWG dwg)
        {
            var nodes = decipher.SelectModels<AC3_DWG_NODE>(LOrder.By("EX_NODE_TYPE"), $"ROW_SID >= 0 and FK_DWG_CODE='{dwg.PK_DWG_CODE}'");
            var lines = decipher.SelectModels<AC3_DWG_LINE>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg.PK_DWG_CODE}'");

            var ltTables = decipher.SelectModels<AC3_LISTEN_TABLE>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg.PK_DWG_CODE}'");
            var opTables = decipher.SelectModels<AC3_OPERATE_TABLE>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg.PK_DWG_CODE}'");

            var ltFields = decipher.SelectModels<AC3_LISTEN_TABLE_FIELD>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg.PK_DWG_CODE}'");

            var opInsertFields = decipher.SelectModels<AC3_OPERATE_TABLE_FIELD>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg.PK_DWG_CODE}' and METHOD_TYPE='insert'");
            var opUpdateFields = decipher.SelectModels<AC3_OPERATE_TABLE_FIELD>($"ROW_SID >= 0 and FK_DWG_CODE='{dwg.PK_DWG_CODE}' and METHOD_TYPE='update'");
            



            var ltTableDict = ltTables.ToSortedList<string>("FK_NODE_CODE");
            var opTableDict = opTables.ToSortedList<string>("FK_NODE_CODE");

            var ltFieldGroup = ltFields.ToGroup<string>("FK_NODE_CODE");

            var opInsertFieldGroup = opInsertFields.ToGroup<string>("FK_NODE_CODE");
            var opUpdateFieldGroup = opUpdateFields.ToGroup<string>("FK_NODE_CODE");

            var lineGroup = lines.ToGroup<string>("FROM_NODE_CODE");



            SortedList<string, XActionItem> actItems = new SortedList<string, XActionItem>();

            XPanel xPanel = new XPanel();

            xPanel.Code = dwg.PK_DWG_CODE;


            foreach (var node in nodes)
            {
                if(node.EX_NODE_TYPE == "listen_table")
                {
                    var ltTab = ltTableDict[node.PK_NODE_CODE];

                    if (StringUtil.IsBlank( ltTab.DB_METHOD))
                    {
                        continue;
                    }

                    LModelList<AC3_LISTEN_TABLE_FIELD> ltFieldList=null;

                    ltFieldGroup.TryGetValue(node.PK_NODE_CODE, out ltFields);


                    XListenTable xli = ToListenXml(node, ltTab, ltFieldList);

                    xPanel.Items.Add(xli);

                    actItems.Add(node.PK_NODE_CODE, xli);
                }
                else  if(node.EX_NODE_TYPE == "operate_table")
                {
                    var opTab = opTableDict[node.PK_NODE_CODE];

                    if (StringUtil.IsBlank(opTab.OP_METHOD))
                    {
                        continue;
                    }

                    LModelList<AC3_OPERATE_TABLE_FIELD> opInsertFieldList = null;
                    LModelList<AC3_OPERATE_TABLE_FIELD> opUpdateFieldList = null;

                    opInsertFieldGroup.TryGetValue(node.PK_NODE_CODE, out opInsertFieldList);
                    opUpdateFieldGroup.TryGetValue(node.PK_NODE_CODE, out opUpdateFieldList);

                    XOperateTable xop = ToOperateXml(node, opTab, opInsertFieldList, opUpdateFieldList);

                    xPanel.Items.Add(xop);

                    actItems.Add(node.PK_NODE_CODE, xop);
                }
            }


            foreach (var fromNodeCode in lineGroup.Keys)
            {
                XActionItem fromNode;

                if(!actItems.TryGetValue(fromNodeCode,out fromNode))
                {
                    continue;
                }


                LModelList<AC3_DWG_LINE> ac3Lines = lineGroup[fromNodeCode];

                foreach (var adLine in ac3Lines)
                {
                    XActionItem toNode;

                    if (!actItems.TryGetValue(adLine.TO_NODE_CODE,out toNode))
                    {
                        continue;
                    }

                    string lineName = StringUtil.NoBlank(adLine.LINE_IDENTIFIER, adLine.PK_LINE_CODE);

                    if(fromNode is XOperateTable)
                    {
                        XOperateTable oo = fromNode as XOperateTable;

                        oo.Routes.Add(new XRoute(lineName, toNode.Code));
                    }
                    else if(fromNode is XListenTable)
                    {
                        XListenTable oo = fromNode as XListenTable;

                        oo.Routes.Add(new XRoute(lineName, toNode.Code));
                    }

                }


            }


            return xPanel;
        }

        private XListenTable ToListenXml(AC3_DWG_NODE node, AC3_LISTEN_TABLE ltTable, List<AC3_LISTEN_TABLE_FIELD> ltFieldList)
        {
            XListenTable xLt = new XListenTable();
            xLt.Code = StringUtil.NoBlank(node.NODE_IDENTIFIER, node.PK_NODE_CODE);

            xLt.Method = ltTable.DB_METHOD;
            xLt.Table = ltTable.DB_TABLE;
            xLt.TableText = ltTable.DB_TABLE_TEXT;

            string scriptType = ltTable.COND_SCRIPT_TYPE.ToLower();

            if (scriptType == "json")
            {
                if (!StringUtil.IsBlank(ltTable.COND_SCRIPT_JSON))
                {
                    xLt.CondScript = new XScript
                    {
                        Lang = "json",
                        Content = ltTable.COND_SCRIPT_JSON
                    };
                }
            }
            else if(scriptType == "cs")
            {
                if (!StringUtil.IsBlank(ltTable.COND_SCRIPT_CS))
                {
                    xLt.CondScript = new XScript
                    {
                        Lang = "cs",
                        Content = ltTable.COND_SCRIPT_CS
                    };
                }
            }

            string vcScriptType = ltTable.VCHANGE_SCRIPT_TYPE.ToLower();

            if (vcScriptType == "json")
            {
                if (!StringUtil.IsBlank(ltTable.VCHANGE_SCRIPT_JSON))
                {
                    xLt.VChangeScript = new XScript
                    {
                        Lang = "json",
                        Content = ltTable.VCHANGE_SCRIPT_JSON
                    };
                }
            }

            
            return xLt;
        }

        private XOperateTable ToOperateXml(AC3_DWG_NODE node, AC3_OPERATE_TABLE opTable, 
            LModelList<AC3_OPERATE_TABLE_FIELD> opInsertFieldList, 
            LModelList<AC3_OPERATE_TABLE_FIELD> opUpdateFieldList)
        {
            XOperateTable xop = new XOperateTable();
            xop.Code = StringUtil.NoBlank( node.NODE_IDENTIFIER, node.PK_NODE_CODE);

            xop.Method = opTable.OP_METHOD;
            xop.Table = opTable.DB_TABLE;
            xop.TableText = opTable.DB_TABLE_TEXT;

            ToXFields(xop.NewFields, opInsertFieldList);
            ToXFields(xop.UpdateFields, opUpdateFieldList);

            string condScriptType = opTable.COND_SCRIPT_TYPE.ToLower();

            if (condScriptType == "json")
            {
                xop.FilterScript = new XScript
                {
                    Lang = "json",
                    Content = opTable.COND_SCRIPT_JSON
                };
            }
            else if(condScriptType == "cs")
            {
                xop.FilterScript = new XScript
                {
                    Lang="cs" ,
                    Content=opTable.COND_SCRIPT_CS
                };
            }

            xop.SelectingScript = ToXScript(opTable.SELECTING_SCRIPT);
            xop.SelectedScript = ToXScript(opTable.SELECTED_SCRIPT);

            xop.InsertingScript = ToXScript(opTable.INSERTING_SCRIPT);
            xop.InsertedScript = ToXScript(opTable.INSERTED_SCRIPT);

            xop.UpdatingScript = ToXScript(opTable.UPDATING_SCRIPT);
            xop.UpdatedScript = ToXScript(opTable.UPDATED_SCRIPT);

            xop.DeletingScript = ToXScript(opTable.DELETING_SCRIPT);
            xop.DeletedScript = ToXScript(opTable.DELETED_SCRIPT);

            
            

            return xop;
        }

        private static XScript ToXScript(string script)
        {
            XScript xs = null;

            if (!StringUtil.IsBlank(script))
            {
                xs = new XScript
                {
                    Lang = "cs",
                    Content = script
                };
            }

            return xs;
        }
        
        private static void ToXFields(List<XOperateField> fields , LModelList<AC3_OPERATE_TABLE_FIELD> opFieldList)
        {
            if(opFieldList == null || opFieldList.Count == 0)
            {
                return;
            }

            foreach (var item in opFieldList)
            {
                XOperateField xField = new XOperateField();
                xField.Name = item.DB_FIELD;
                xField.Text = item.DB_FIELD_TEXT;
                xField.ValueMode = item.VALUE_TYPE;
             
                xField.Value = item.DB_VALUE;
                xField.Value2 = item.DB_VALUE_2;

                fields.Add(xField);
            }
        }

    }
}
