


Mini2.ClassManager = new function () {
    "use strict";

    var ClassManager = this;

    Mini2.apply(ClassManager, {

        //log: Mini2.Logger.getLogger('Mini2.ClassManager'),  //日志管理器


        /**
         * @private
         */
        maps: {
            alternateToName: {},
            aliasToName: {},
            nameToAliases: {},
            nameToAlternates: {}
        },

        /**
         * Register the alias for a class.
         *
         * @param {Ext.Class/String} cls a reference to a class or a className
         * @param {String} alias Alias to use when referring to this class
         */
        setAlias: function (cls, alias) {
            var me = this,
                maps = me.maps,
                aliasToNameMap = maps.aliasToName,
                nameToAliasesMap = maps.nameToAliases,
                className;


            if (typeof cls == 'string') {
                className = cls;
            } else {
                className = this.getName(cls);
            }

            if (alias && aliasToNameMap[alias] !== className) {

                aliasToNameMap[alias] = className;
            }

            if (!nameToAliasesMap[className]) {
                nameToAliasesMap[className] = [];
            }


            if (alias) {
                Mini2.Array.include(nameToAliasesMap[className], alias);
            }

            return me;
        },

        getName: function (cls) {

            return cls;
        },


        /**
         * Get the name of a class by its alias.
         *
         * @param {String} alias
         * @return {String} className
         */
        getNameByAlias: function (alias) {
            return this.maps.aliasToName[alias] || '';
        },


    });

};