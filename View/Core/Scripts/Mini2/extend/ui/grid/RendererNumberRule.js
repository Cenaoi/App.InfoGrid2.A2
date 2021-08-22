/// <reference path="../../../jquery/jquery-1.4.1-vsdoc.js" />

(function () {
    Mini2.ns('Mini2.ui.extend.grid');

    var RendererNumberRule = Mini2.ui.extend.grid.RendererNumberRule = {};


    Mini2.apply(RendererNumberRule, {

        
        rule1:function(value){
            /// <summary>
            /// 规则1
            /// </summary>
        },

        rule2:function(value){

        },


        format:function(ruleName,value){
            var me = this,
                rule = me[ruleName];

            if (rule) {
                value = rule.call(me, value);
            }

            return value;
        }

    });

}());

