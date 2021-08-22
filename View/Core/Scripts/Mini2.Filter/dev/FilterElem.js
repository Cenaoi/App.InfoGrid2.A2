
/// <reference path="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" />
/// <reference path="/Core/Scripts/Mini2/Mini2.js" />


Mini2.define('Mini2.ui.filter.FilterElem', {

    extend: 'Mini2.ui.filter.Item',


    renderTpl: Mini2.join([
        '<li class="ui-state-default mi-filter-item">',
            '<div class="mi-filter-text">COL_3 = \'GAME\'</div>',
            '<a class="mi-filter-remove" style="" href="#" >',
                '<div class="mi-filter-remove-inner">X</div>',
            '</a>',
        '</li>'
    ]),

    textEl: null,
    

    role: 'elem',

    //field: 'COL_1',
    //logic: '>=',
    //value: 2,

    data: null,

    setData:function(data){
        var me = this,
            el = me.el,
            textEl = me.textEl;
        
        me.data = data;


        textEl.text(me.getFormatText());

        return me;
    },

    getData:function(){
        var me = this;

        return me.data;
    },

    getFormatText:function(){
        var me = this,
            d = me.data;

        if (!d) {
            return "Item";
        }

        if (d.to_value || d.from_value) {
            return $.format('{0} {1} [ {2}->{3} ]', d.field, d.logic, d.from_value, d.to_value);
        }
        else if (d.value) {
            return $.format('{0} {1} {2}', d.field, d.logic, d.value);
        }
        else {
            return 'Item';
        }

    },

    render: function () {
        var me = this,
            el, textEl;

        me.baseRender();

        el = me.el;



        textEl = el.children('.mi-filter-text');
        textEl.text(me.getFormatText() );

        me.textEl = textEl;


        el.muBind('dblclick', function () {

            me.trigger('dblclick');

        });

        el.data('me', me);

        return me;
    },


    getJson: function () {
        var me = this,
            cfg;

        //cfg = {
            
        //    field: me.field,
        //    logic: me.logic,
        //    value: me.value
        //};

        return me.data;
    }

});