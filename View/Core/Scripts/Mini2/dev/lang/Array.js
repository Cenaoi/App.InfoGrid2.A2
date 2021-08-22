

/**
* 数组操作类
*/
Mini2.Array = new function () {
    "use strict";
    Mini2.apply(this, {

        /**
        * 克隆函数
        */ 
        clone: Mini2.clone,


        /**
        * 判断数组是否包含元素
        * 
        * @param {Array} srcArray 数据集合
        * @param {Object} item 需要判断的元素对象
        * @param {Boolean} isValueType 是否值类型
        * @return {Boolean} 是否包含对象
        */
        contains: function (srcArray, item, isValueType) {

            var isExist = false,
                i=0,
                len = srcArray.length;

            if (isValueType) {
                for (; i < len; i++) {
                    if (srcArray[i] == item) {
                        isExist = true;
                        break;
                    }
                }
            }
            else {
                for (; i < len; i++) {
                    if (srcArray[i] === item) {
                        isExist = true;
                        break;
                    }
                }
            }

            return isExist;
        },

        
        lastIndexOf: function (srcArray, item) {

            var n = -1,
                len = srcArray.length,
                index = len;

            while (index--) {
                if (srcArray[index] === item) {
                    n = index;
                    break;
                }
            }
            return n;
        },


        indexOf: function (srcArray, item) {

            var n = -1,
                i = 0,
                result = false,
                srcItem,
                len = srcArray.length;

            if (Mini2.isFunction(item)) {
                for (; i < len; i++) {
                    srcItem = srcArray[i];
                    result = item.call( srcItem )

                    if (result) {
                        n = i;
                        break;
                    }
                }
            }
            else {
                for (; i < len; i++) {
                    if (srcArray[i] === item) {
                        n = i;
                        break;
                    }
                }
            }

            return n;
        },


        /**
        * 包含元素, 如果不存在,就直接添加
        *
        * @parma {Array} srcArray 原数据数组
        * @param {Object} item 数据元素
        */
        add: function (srcArray, item) {

            srcArray.push(item);

        },


        /**
        * 包含元素, 如果不存在,就直接添加
        *
        * @parma {Array} srcArray 原数据数组
        * @param {Object} item 数据元素
        */
        include: function (srcArray, item) {
            var c = [];
            if (this.contains(srcArray, item)) {
                return;
            }
            srcArray.push(item);
        },

        /**
        * 删除数据的元素
        * 
        * @param {Array} srcArray 原数据数组
        * @param {Object} item 数据元素
        * @return {Int} 已删除元素的位置
        */
        remove: function (srcArray, item) {
            var c = [],
                n;

            n = this.indexOf(srcArray, item);

            if (n > -1) {
                srcArray.splice(n, 1);
            }

            return n;
        },

        /**
        * 移动元素
        *
        * @param {Array} srcArray 原数据数组
        * @param {Int} fromIndex 移动前的位置
        * @parma {Int} toIndex 移动的位置
        */
        move: function (srcArray, fromIndex, toIndex) {
            var item = srcArray[fromIndex],
                newIndex = toIndex;

            srcArray.splice(fromIndex, 1);

            if (fromIndex > toIndex) {
                newIndex++;
            }

            srcArray.splice(newIndex, 0, item);
        }

    });

};
