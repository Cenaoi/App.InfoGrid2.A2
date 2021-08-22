using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HWQ.Entity.LightModels;

namespace EC5.Action3
{
    public static class SampleTest
    {

        public static DrawingPanel Test1()
        {

            //实例化一个图纸
            DrawingPanel dp = new DrawingPanel("DWG_001");


            #region 添加 "监听表", 监听到有记录插入

            ListenTable lt01 = new ListenTable("lt_001", ListenMethod.Insert);
            lt01.Table = "ORDER";

            //lt01.CondScript = new ScriptTSql("TITLE = '核武器'");
            lt01.CondScript = new ScriptJson(new
            {
                where = new object[] {
                    new {
                        field = "TITLE",
                        logic = "=",
                        value = "核武器"
                    }
                }
            });

            //"[{field:'TITLE', logic:'=', value:'核武器'}]");


            #endregion


            #region  添加 "操作表", 做一个创建记录的操作

            OperateTable ot01 = new OperateTable("ot_001", OperateMethod.Insert);

            ot01.Table = "ORDER_HIST";
            
            ot01.NewFields.Add("TEST", "{{ P.ORDER.TITLE }}" );
            ot01.NewFields.Add("AGE", 100);                       

            #endregion


            #region 更新操作

            OperateTable ot_002 = new OperateTable("ot_002", OperateMethod.Update);
            ot_002.Table = "ORDER_HIST";

            // [{ filter: 'T.TEST = P.ORDER.TITLE' }]

            //string tWhere = "ORDER_HIST_ID >= {{ P.ORDER_HIST.ORDER_HIST_ID - 2 }}";

            ot_002.FilterScript = new ScriptJson(new
            {
                where = new object[] {
                    new {
                        field = "ORDER_HIST_ID",
                        logic = ">=",
                        value = "P.ORDER_HIST_ORDER_HIST_ID - 2",
                        mode = "fun"
                    }
                }
            });

            //"[{field:'ORDER_HIST_ID', logic:'>=', value:'P.ORDER_HIST.ORDER_HIST_ID - 2', mode: 'fun'} ]


            ot_002.UpdateFields.Add("AGE", 200);

            //ot_002.LinkTo("01", "ot_003");

            #endregion



            #region 删除操作

            OperateTable ot_003 = new OperateTable("ot_003", OperateMethod.Delete);
            ot_003.Table = "ORDER_HIST";

            // ORDER_HIST_ID < {{ First.ORDER_HIST.ORDER_HIST_ID - 2 }}
            ot_003.FilterScript = new ScriptJson(new
            {
                where = new object[] {
                    new {
                        field = "ORDER_HIST_ID",
                        logic = "<",
                        value = "FIRST.ORDER_HIST.ORDER_HIST_ID - 2",
                        mode = "fun"
                    }
                }
            });

            //[{field:'ORDER_HIST_ID', logic:'<', value: 'FIRST.ORDER_HIST.ORDER_HIST_ID - 2' , mode: 'fun'}]

            //.Add(new OperateField("TEST", "P.ORDER.TITLE", ActionModeType.Fun));

            //ot_003.FilterScript = new ScriptTSql("ORDER_HIST_ID < {{ First.ORDER_HIST.ORDER_HIST_ID - 2 }}");

            #endregion




            lt01.LinkTo("01", "ot_001");
            ot01.LinkTo("01", "ot_002");


            dp.AddRange(lt01, ot01, ot_002, ot_003);
            


            #region 添加 "监听表", 监听到有记录插入

            ListenTable lt02 = new ListenTable("lt02", ListenMethod.Update);
            lt02.Table = "ORDER_HIST";

            #endregion


            #region  添加 "操作表", 做一个创建记录的操作

            OperateTable ot04 = new OperateTable("ot04", OperateMethod.Update);

            ot04.Table = "ORDER_HIST";
            ot04.FilterScript = new ScriptJson("{ where : [{field:'ORDER_HIST_ID', logic:'=', value:'P.ORDER_HIST.ORDER_HIST_ID ', value_mode: 'fun'} ] }");

            ot04.UpdateFields.AddFun("REMARK", "(T.AGE + 100) + \"岁\"");

            ot04.AutoContinue = false;  //运行到这里就停止,不再继续触发联动

            #endregion


            lt02.LinkTo("01", "ot04");

            dp.AddRange(lt02, ot04);


            dp.Builder();

            return dp;

        }

    }
}
