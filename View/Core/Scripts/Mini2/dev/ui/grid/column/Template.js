
/// <reference path="../../define.js" />
/// <reference path="../../../../../jquery/jquery-1.4.1-vsdoc.js" />

Mini2.define('Mini2.ui.grid.column.Template', {

    extend: 'Mini2.ui.grid.column.Colunm',

    content: '',

    artTemplate: null, //var render = template.compile(source);

    vurTemplate: null, //

    /**
    * 模板名称  art | vue
    */
    engineName: 'j',    //模板名称



    // private
    renderer: function (value, metaData, cellValues, record, rowIdx, colIdx, store, table, rowEl, cellEl) {
        "use strict";
        var me = this,
            tmpName = me.engineName,
            rendererFn,
            valueStr,
            srcValue = value;

        if ('j' == tmpName) {
            rendererFn = me.renderer_j;;
        }
        else if ('vue' == tmpName) {
            rendererFn = me.renderer_vue;
        }
        else if ('art' == tmpName) {
            rendererFn = me.renderer_art;
        }

        try{
            valueStr = rendererFn.call(me, value, metaData, cellValues, record, rowIdx, colIdx, store, table, rowEl, cellEl);

        }
        catch (ex) {
            console.error("输出错误 ", ex);
        }

        return valueStr;;
    },


    renderer_j:function(value, metaData, cellValues, record, rowIdx, colIdx, store, table, rowEl, cellEl){
        "use strict";
        var me = this,
            valueStr,
            jTemp = me.jTemplate,
            srcValue = value;

        if (!jTemp) {
            jTemp = $.createTemplate(me.content);

            me.jTemplate = jTemp;
        }


        return jTemp.get(record, {
            value: value,
            row_index: rowIdx,
            col_index : colIdx
        });

    },



    curVueTemlateId: null,

    /**
    * 注册 VUE 组件
    *
    */
    regVue: function (cellEl, record) {
        "use strict";
        var me = this,
            vueTemp = me.vueTemplate,
            cptName = 'coltemplate';

        //<mi-progressbar :ex_cls="ex_cls"  :value="value"></mi-progressbar>

        if (!vueTemp) {

            var id = 'templatecolumn';// Mini2.Guid.newGuid('N');

            vueTemp = Vue.extend({
                template: me.content,
                props: ["t"]
            });

            me.vueTemplate = vueTemp;

            // 注册
            Vue.component(id, vueTemp);

            Mini2.vue = Mini2.vue || {};
            Mini2.vue.component = Mini2.vue.component || {};
            Mini2.vue.component[id] = true;

            me.curVueTemlateId = id;
        }


        setTimeout(function () {


            // 创建根实例
            me.vue = new Vue({
                el: cellEl[0], // '<div><templatecolumn :t="t" ></templatecolumn></div>',
                data: {
                    t: record
                }
            });
        }, 1000);

    },

    /**
    * vue.js 渲染
    *
    */
    renderer_vue: function (value, metaData, cellValues, record, rowIdx, colIdx, store, table, rowEl, cellEl) {
        "use strict";
        var me = this,
            valueStr,
            srcValue = value;

        me.regVue(cellEl, record);

        return '<templatecolumn :t="t" ></templatecolumn>';

        //return vueTemp.render();

    },

    renderer_art: function (value, metaData, cellValues, record, rowIdx, colIdx, store, table, rowEl, cellEl) {
        "use strict";
        var me = this,
            valueStr,
            artTemp = me.artTemplate,
            srcValue = value;


        if (!artTemp) {
            me.artTemplate = artTemp = template.compile(me.content);
        }
        
        valueStr = artTemp({
            data:record
        });

        return valueStr;
    }

});
