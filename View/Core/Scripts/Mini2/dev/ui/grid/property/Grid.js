


Mini2.define("Mini2.ui.grid.property.Grid", {

    extend: 'Mini2.ui.panel.Table',

    checkedMode: 'SINGLE',



    onInit: function () {

        var me = this;


        if (me.isInit) {
            return;
        }

        me.isInit = true;

        me.pager = {
            visible : false
        };

        me.columns = [{
            miType: 'col',
            dataIndex: 'Display',
            headerText: '名称',
            width: 115
        }, {
            miType: 'propertyColumn',
            dataIndex: 'Value',
            headerText: '值',
            editor: { xtype: 'property' },
            width: 120
        }];


        var newStore = Mini2.create('Mini2.data.Store', {
            storeId: 'MiniStore_' + Mini2.getIdentity(),
            idField: 'Name',
            fields: ['Name', 'Value','Display', 'Type','Extend']
        });

        me.bindStore(newStore);


        delete me.m_Cols;
        me.m_Cols = [];


        var index = 0;

        $(me.columns).each(function () {
            me.getColumnForConfig(index, this);
            index++;
        });

        me.cellTpl = me.StrJoin(me.cellTpl);

    }


}, function () {
    var me = this;

    Mini2.apply(this, arguments[0]);
    me.onInit(arguments[0]);

    me.muid = Mini2.getIdentity();
});