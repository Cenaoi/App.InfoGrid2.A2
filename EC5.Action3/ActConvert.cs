using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EC5.Action3.Xml;
using EC5.Utility;

namespace EC5.Action3
{
    /// <summary>
    /// 转换类型
    /// </summary>
    public static class ActConvert
    {
        public static DrawingLibrary LibFrom(XLibrary xLib)
        {
            DrawingLibrary lib = new DrawingLibrary();
            lib.Code = xLib.Code;
            
            foreach (var xPanel in xLib.Panels)
            {
                DrawingPanel panel = PanelFrom(xPanel);

                panel.Builder();

                lib.Add(panel);
            }

            return lib;
        }

        public static DrawingPanel PanelFrom(XPanel xPanel)
        {
            DrawingPanel panel = new DrawingPanel();

            panel.Code = xPanel.Code;
            panel.Enabled = xPanel.Enabled;

            ActionItemBase actItem = null;

            foreach (var item in xPanel.Items)
            {
                if (item is XListenTable)
                {
                    actItem = ListenFrom((XListenTable)item);
                }
                else if (item is XOperateTable)
                {
                    actItem = OperateFrom((XOperateTable)item);
                }
                else
                {
                    throw new Exception($"未知类型,无法转换. Type={item.GetType().FullName}");
                }

                panel.Add(actItem);
            }

            return panel;
        }

        private static ListenTable ListenFrom(XListenTable xListen)
        {
            ListenTable li = new ListenTable();
            li.Code = xListen.Code;
            li.Enabled = xListen.Enabled;
            li.Method = EnumUtil.Parse<ListenMethod>(xListen.Method, true);
            li.Remark = xListen.Remark;
            li.Table = xListen.Table;
            li.TableText = xListen.TableText;
            li.CondScript = FromScript(xListen.CondScript);
            li.VChangeScript = FromScript(xListen.VChangeScript);
            

            //foreach (var xLiField in xListen.ListenFields)
            //{

            //}


            foreach (var xRoute in xListen.Routes)
            {
                li.LinkTo(xRoute.Name, xRoute.LinkTo);
            }

            return li;
        }

        private static ScriptBase FromScript(XScript xScript)
        {
            ScriptBase sc = null;

            if (xScript != null)
            {

                string lang = xScript.Lang;

                string code = xScript.Content;

                if (StringUtil.IsBlank(code))
                {
                    return null;
                }

                if (lang == "json")
                {
                    sc = new ScriptJson(code);
                }
                else if (lang == "cs")
                {
                    if(!code.Contains("return "))
                    {
                        code = "return " + code + ";";
                    }

                    sc = new ScriptCSharp(code);

                }
                else if (lang == "")
                {

                }
                else
                {
                    throw new Exception($"脚本类型无法识别.lang={lang}");
                }
            }

            return sc;
        }

        private static OperateTable OperateFrom(XOperateTable xOp)
        {
            OperateTable op = new OperateTable();

            op.AutoContinue = xOp.AutoContinue;
            op.Code = xOp.Code;
            op.Enabled = xOp.Enabled;
            op.Table = xOp.Table;
            op.TableText = xOp.TableText;
            op.Method = EnumUtil.Parse<OperateMethod>(xOp.Method);
            op.MixingMode = OperateMixingMode.None;
            
            op.FilterScript = FromScript(xOp.FilterScript);

            op.UpdatedScript = FromScript(xOp.UpdatedScript);
            op.UpdatingScript = FromScript(xOp.UpdatingScript);

            op.InsertingScript = FromScript(xOp.InsertingScript);
            op.InsertedScript = FromScript(xOp.InsertedScript);

            op.DeletingScript = FromScript(xOp.DeletingScript);
            op.DeletedScript = FromScript(xOp.DeletedScript);


            ProOperateFields(op.NewFields, xOp.NewFields);
            ProOperateFields(op.UpdateFields, xOp.UpdateFields);

            foreach (var xRoute in xOp.Routes)
            {
                op.LinkTo(xRoute.Name, xRoute.LinkTo);
            }

            return op;
        }

        private static void ProOperateFields(OperateFieldCollection opFields, List<XOperateField> xFields)
        {
            foreach (var xItem in xFields)
            {
                OperateField field = ConvertField(xItem);

                opFields.Add(field);
            }
        }

        private static OperateField ConvertField(XOperateField xItem)
        {
            OperateField field = new OperateField();
            field.Name = xItem.Name;
            field.Value = xItem.Value;
            field.Value2 = xItem.Value2;
            field.ValueMode = EnumUtil.Parse(xItem.ValueMode, ActionModeType.Fixed);
            field.ValueType = xItem.ValueType;

            field.IsNull = xItem.IsNull;
            field.IsNull2 = xItem.IsNull2;
            field.Remark = xItem.Remark;
            field.Table = xItem.Table;
            field.TableText = xItem.TableText;
            field.Text = xItem.Text;

            if(field.ValueMode == ActionModeType.Fun)
            {
                field.DebugText = field.Value;
            }

            return field;
        }
    }
}
