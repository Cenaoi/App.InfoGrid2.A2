

//图标插件
Mini2.define("Mini2.ui.icon.Manager", {


    /**
     *单件模式
     */
    singleton: true,

    /**
     * 按钮插件
     */
    btnConfig: [{
        keys: ['添加','新建', '新增', 'new', 'create'],
        iconCls: 'fa-plus'

    }, {
        keys: ['保存', 'save'],
        iconCls: 'fa-save'
    }, {
        keys: ['删除', 'delete', 'remote'],
        icon: null,
        iconCls: 'fa-remove',
        cls: 'mi-btn-remove',
        hoverCls: null
    }, {
        keys: ['编辑', '修改', 'edit', 'update'],
        iconCls: 'fa-pencil'
    }, {
        keys: ['打印', 'print'],
        iconCls: 'fa-print'
    }, {
        keys: ['查找', '查询', '搜索', 'search'],
        iconCls: 'fa-search'
    }, {
        keys: ['刷新', 'refresh'],
        iconCls: 'fa-refresh  ',

    }, {
        keys: ['导出 Excel', '导出Excel'],
        icon: '/res/file_ico/excel.png',
        iconCls: null
    }, {
        keys: ['导入'],
        icon: '/res/icon/leading_in.png'
    }, {
        keys: ['导出', 'export'],
        icon: '/res/icon/Export.png'
    }, {
        keys: ['下载', 'download'],
        iconCls: 'fa-download'
    }, {
        keys: ['查看', 'view'],
        iconCls: 'fa-search'
    }, {
        keys: ['选择'],
        iconCls: 'fa-list-alt'
    }, {
        keys: ['提交'],
        iconCls: 'fa-upload'
    }, {
        keys: ['撤销', '撤销提交'],
        iconCls: 'fa-reply'
    }, {
        keys: ['审核', '结案', '归档'],
        iconCls: 'fa-lock'
    }],



    buffer: {},



    /**
     * 获取插件
     */
    getPlugin: function (pluginName) {

        var me = this,
            plugin = me.buffer[pluginName];

        if (!plugin) {

            if (me[pluginName]) {

                plugin = Mini2.create('Mini2.ui.icon.Plugin', {

                    cfg: me[pluginName]

                });

                me.buffer[pluginName] = plugin;

            }


        }

        return plugin;


    }


});