
Mini2.GroupManager = new function () {
	"use strict";

	var GroupManager = this;

	Mini2.apply(GroupManager, {

		log: Mini2.Logger.getLogger('Mini2.EventManager'),  //日志管理器

		/**
        * 字典索引
        */
		dictIndex: {},

		add: function (groupName, item) {
			var me = this,
                dict = me.dictIndex,
                items = dict[groupName];

			if (!items) {
				items = [];
				dict[groupName] = items;
			}

			items.push(item);

			return me;
		},

		/**
        * 删除元素
        */
		remove: function (groupName, item) {
			var me = this,
                dict = me.dictIndex,
                items = dict[groupName];

			if (!items) {
				return false;
			}

			Mini2.Array.remove(items, item);
		},

		get: function (groupName) {
			var me = this,
                dict = me.dictIndex,
                items = dict[groupName];

			return items;
		},

		/**
        * 清理数组
        */
		clear: function (groupName) {
			var me = this,
                dict = me.dictIndex,
                items = dict[groupName];

			if (!items) {
				return false;
			}

			dict[groupName] = [];
		}

	});

}
