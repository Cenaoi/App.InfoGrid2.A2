


Mini2.ns('Mini2.ui');

Mini2.ui.SecManager = new function () {
    "use strict";

    Mini2.apply(this, {

        defaultProp :'visible',

        /**
         * 属性索引
         */
        propDict: {
            'visible': {},
            'readonly': {}
        },


        /**
         * 注册控件
         *
         * @param {String} 权限编码
         * @param {Object} 控件
         */
        reg: function (secFunCode, con, prop) {
            "use strict";
            
            if (Mini2.isBlank(secFunCode)) { return this; }

            prop = prop || this.defaultProp;

            var me = this,
                cons = me.propDict[prop],// me.secControls,
                items = cons[secFunCode];


            if (!items) {
                cons[secFunCode] = items = [];
            }

            items.push(con);


            return me;
        },


        /**
         * 卸载注册控件
         * @param {String} 权限编码
         * @param {Object} 控件
         */
        unreg: function(secFunCode, con, prop){
            "use strict";

            if (Mini2.isBlank(secFunCode)) { return this; }

            prop = prop || this.defaultProp;

            var me = this,
                cons = me.propDict[prop],//me.secControls,
                items = cons[secFunCode];
                        
            if (!items) {
                cons[secFunCode] = items = [];
            }

            Mini2.Array.remove(items, con);

            return me;
        },

        /**
         * 获取控件集合
         * @param {Array} 
         */
        getItems : function(secFunCode){
            "use strict";

            var me = this,
                cons = me.secControls,
                items = cons[secFunCode];

            return items || null;
        },

        /**
         * 设置可视的
         */
        setFun: function (defaultEnabled, secFunCodeList) {
            "use strict";

            var me = this;

            me.setFun_ForVisible(defaultEnabled, secFunCodeList);

            me.setFun_FroReadonly(defaultEnabled, secFunCodeList);
                        

            //重新刷新界面
            Mini2.LoaderManager.preResize();

            return me;
        },

        /**
         * 获取激活的分组
         */
        getEnableGroup: function (cons, secFunCodeList) {
            "use strict";
            var me = this,
                tItems = [],
                fItems = [],
                group = {
                    T: tItems,
                    F: fItems
                };
            
            for (var funCode in cons) {

                if (Mini2.Array.contains(secFunCodeList, funCode, true)) {
                    tItems[funCode] = cons[funCode];
                }
                else {
                    fItems[funCode] = cons[funCode];
                }

            }

            return group;
        },

        setFun_ForVisible:function(defaultEnabled, secFunCodeList ){
            "use strict";

            var me = this,
                funName = 'setVisible',
                cons = me.propDict['visible'],
                group ;// me.secControls;


            group = me.getEnableGroup(cons, secFunCodeList);
            
            me.callFun(group.T, funName, true);
            me.callFun(group.F, funName, false);
            
            return me;
        },

        setFun_FroReadonly:function(defaultEnabled, secFunCodeList){
            "use strict";

            var me = this,
                funName = 'setReadOnly',
                cons = me.propDict['readonly'],
                group;// me.secControls;

            group = me.getEnableGroup(cons, secFunCodeList);

            me.callFun(group.T, funName, true);
            me.callFun(group.F, funName, false);

            return me;
        },

        callFun:function( cons, funName , value){
            var me = this,
                fun,
                items,
                item;

            for (var funCode in cons) {

                items = cons[funCode] || [];

                for (var i = 0; i < items.length; i++) {

                    item = items[i];
                    
                    fun = item[funName];

                    if (fun) {
                        fun.call(item, value);
                    }
                    else {
                        console.warn('没有找到这个控件对应的函数名: ' + funName);
                    }

                }

            }

            return me;
        }


    });

};