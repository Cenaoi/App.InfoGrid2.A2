
/// <reference path="define.js" />
/// <reference path="../../../jquery/jquery-1.4.1-vsdoc.js" />

//焦点管理
Mini2.define("Mini2.ModelManager", {


    log: Mini2.Logger,  //日志管理器

    alternateClassName: 'Mini2.ModelMgr',

    typeName: 'mtype',

    singleton: true,

    types: {},

    create: function (config, name, id) {
        "use strict";
        var conObj,
            Con = (typeof name == 'function') ? name : this.types[name || config.name];

        conObj = new Con(config, id);

        return conObj;
    },



    registerType: function (name, config) {
        "use strict";
        /// <summary>注册类型</summary>

        //log.begin("ModelManager.registerType(...)   " + name);

        var proto = config.prototype,
            model;

        if (proto && proto.isModel) {
            model = config;
        }
        else {

            if (Mini2.String.isBlank(config.extend)) {
                config.extend = 'Mini2.data.Model';
            }

            model = Mini2.define(name, config);
        }


        this.types[name] = model;

        //log.end();

        return model;
    },



    getModel: function (id) {
        "use strict";
        var model = id;
        if (typeof model == 'string') {
            model = this.types[model];

            //if (model == null) {
            //    model = Mini2.create(id);
            //}
        }

        return model;
    }


});