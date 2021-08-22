
/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="/Core/Scripts/Mini2/Mini2.js" />



Mini2.define('Mini2.ui.filter.Panel', {


    mixins: ['Mini2.EventBuilder'],
    
    /**
    * 过滤结构的元素版面
    */
    el:null,

    /**
    * 排序的元素版面
    */
    orderEl : null,

    initComponent: function () {

    },



    item_dblclick:function(sender){
        var me = this;

        me.trigger('itemDblclick', {
            item: sender
        });
    },

    
    /**
    * 添加过滤元素
    */
    insertItem: function () {
        var me = this,
            el = me.el;

        var item = Mini2.create('Mini2.ui.filter.FilterElem', {

            renderTo: el
        });

        item.on('dblclick', function () {
            me.item_dblclick(this);            
        });

        item.render();
    },

    /**
    * 添加 And 组合元素
    */
    insertAnd: function () {
        var me = this,
           el = me.el;

        var item = Mini2.create('Mini2.ui.filter.FilterGroup', {
            role:'and',
            renderTo: el

        });

        item.render();

    },

    /**
    * 添加 Or 组合元素
    */
    insertOr: function () {
        var me = this,
           el = me.el;

        var item = Mini2.create('Mini2.ui.filter.FilterGroup', {
            role: 'or',
            renderTo: el

        });

        item.render();

    },

    render: function () {
        var me = this,
            el = me.el,
            orderEl = me.orderEl;

        el.sortable({
            connectWith: ".mi-filter-inner"
        }).disableSelection();





        $(el).data('me', me);
    },


    parseJson: function (parentEl, data, depth) {

        var me = this,
            i,
            //items = dataItems || [],
            //len = items.length,
            item ,
            role,
            newFElem,

            pWhere,
            pOrder,
            pLimit,
            pDistinct;


        pWhere = data.where || [];
        pOrder = data.order;
        pLimit = data.limit || 0;
        pDistinct = data.distinct || false;
        pFields = data.fields;
        
        if (pOrder) {

            var orderTextEl = me.orderEl.find('.filter-order-text');
            
            if (Mini2.isString(pOrder)) {
                orderTextEl.val(pOrder);
            }
            else {
                //orderTextEl.val(JSON.stringify(pOrder));
            }
        }

        if (pFields) {
            var fieldsTextEl = me.fieldsEl.find('.filter-fields-text');

            if (Mini2.isString(pFields)) {
                fieldsTextEl.val(pFields);
            }
            else {
                //fieldsTextEl.val(JSON.stringify(pFields));
            }
        }


        if (pDistinct) {
            $(me.distinctEl).attr('checked', 'checked');
        }

        if (pLimit) {
            if (Mini2.isArray(pLimit)) {
                $(me.limitCountEl).val(pLimit[0]);
                $(me.limitStartEl).val(pLimit[1]);
            }
            else {
                $(me.limitCountEl) = pLimit;
            }
        }

        me.parseJson_Where(parentEl, pWhere, 1);

    },

    parseJson_Where: function (parentEl, data, depth) {
        var me = this,
            role,
            item,
            len = data.length;

        if (0 == data.length) {
            return;
        }


        for (i = 0; i < len; i++) {

            item = data[i];

            try{
                role = item.role || 'elem';
            }
            catch (ex) {
                console.error("data : ", data);
                console.error("item 获取角色错误,", ex);
            }

            if ('elem' == role) {

                var itemData = Mini2.clone(item);

                if (itemData.role) {
                    delete itemData.role;
                }

                var cfg = Mini2.apply(item, {

                    data: itemData,

                    renderTo: parentEl
                });

                newFElem = Mini2.create('Mini2.ui.filter.FilterElem', cfg);

                newFElem.render();

                newFElem.on('dblclick', function () {
                    me.item_dblclick(this);
                });

            }
            else if ('and' == role) {
                newFElem = Mini2.create('Mini2.ui.filter.FilterGroup', {
                    role: 'and',
                    renderTo: parentEl
                });

                newFElem.render();

                me.parseJson_Where(newFElem.innerEl, item.items, depth + 1);
            }
            else if ('or' == role) {
                newFElem = Mini2.create('Mini2.ui.filter.FilterGroup', {
                    role: 'or',
                    renderTo: parentEl
                });

                newFElem.render();

                me.parseJson_Where(newFElem.innerEl, item.items, depth + 1);
            }
            else {
                console.debug("不明确对象 ", item);
            }
        }

    },


    clear:function(){
        var me = this,
            el = me.el;

        el.children().remove();
    },

    /**
    * 设置 json 数据;
    * @param {Array} jsonData json格式数组
    */
    setJson: function (jsonData) {
        var me = this,
            el = me.el,
            items = jsonData;


        me.parseJson(el, items,0);

    },

    /**
    * 获取 json 格式数据;
    *
    */
    getJson: function () {
        var me = this,
            el = me.el,
           cfg = {},
           innerEl = me.innerEl,
           itemEls,
           items ;

        itemEls = el.children('.mi-filter-item');

        if (itemEls.length) {

            items = [];

            for (var i = 0; i < itemEls.length; i++) {

                var itemEl = itemEls[i];

                var item = $(itemEl).data('me');

                var itemCfg = item.getJson();

                items.push(itemCfg);
            }

            cfg.where = items;
        }

        var fieldsTextEl = me.fieldsEl.find('.filter-fields-text');
        var fieldsText = fieldsTextEl.val();

        if (!Mini2.isBlank(fieldsText)) {
            cfg.fields = fieldsText;
        }

        if (me.orderEl) {
            var orderTextEl = me.orderEl.find('.filter-order-text');

            var orderText = orderTextEl.val();

            if (!Mini2.isBlank(orderText)) {
                if (Mini2.isString(orderText)) {
                    cfg.order = orderText;// JSON.parse(orderText);
                }
            }
        }

        if (me.distinctEl) {
            var dist = $(me.distinctEl).is(':checked');

            if (dist) {
                cfg.distinct = dist;
            }
        }

        var limitCount = $(me.limitCountEl).val();
        var limitStart = $(me.limitStartEl).val();

        if (!Mini2.isBlank(limitCount)) {

            if (!Mini2.isBlank(limitStart)) {
                cfg.limit = [parseInt(limitCount), parseInt( limitStart)];
            }
            else {
                cfg.limit = parseInt(limitCount);
            }
        }

        return cfg;
    }

}, function () {
    var me = this;


    Mini2.apply(me, arguments[0]);

    me.initComponent();

});
