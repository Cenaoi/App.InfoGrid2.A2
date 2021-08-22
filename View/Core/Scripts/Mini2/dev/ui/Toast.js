
//Notiser.init = function (text) {
//    this.text = text || '';
//    this.style = 'basic';
//    this.age = basicAge;
//    this.validate();
//};

Mini2.define('Mini2.ui.Toast', {

    //基础容器
    basePanelTpl: [
        '<div class="mi-toast-panel" >',
            '<div class="mi-toast-panel-innser" >',
                '<div class="mi-toast-target mi-toast-left-notify" style="width:25%; pointer-events:none;" ></div>',
                '<div class="mi-toast-target mi-toast-right-notify" style="left:75%; width:25%; pointer-events:none;" ></div>',
                '<div class="mi-toast-target mi-toast-center-notify" style="left:33.33%; width:33.33%; pointer-events:none;" ></div>',
            '</div>',
        '</div>'],

    

    supportedStyles : ['basic', 'warning', 'success'],

    basicDuration: 3000,

    //text: '',

    style:'basic',


    validate: function () {
        var me = this,
            supportedStyles = me.supportedStyles;

        // if user sets not supported style, setting style to basic
        if (Mini2.Array.indexOf(supportedStyles,me.style) === -1) {
            me.style = supportedStyles[0];
        }
    },

    // Setting style
    setStyle: function (style) {
        var me = this;

        me.style = style;
        me.validate();
        return me;
    },

    // Setting text
    setText: function (text) {
        var me = this;
        me.text = text || '';
        return me;
    },

    // Setting time how long notify is showing
    setDuration: function (duration) {
        var me = this;

        me.duration = duration > 0 ? duration : me.basicDuration;
        return this;
    },

    show : function(selector) {
        var me = this,
            basePanelEl,
            notiserElement;

        if ($('.mi-toast-panel').length == 0) {
            basePanelEl = Mini2.$join(me.basePanelTpl);

            $('body').append(basePanelEl);
        }

        notiserElement = $(document.createElement('div'))
          .addClass('mi-toast-notiser ' + me.style).append(me.text);

        // adding created div to first in notify list
        notiserElement.prependTo($(selector)).hide().slideDown();

        notiserElement.one('click', function() {
            me.fadeOutAndRemoveElement(me.el);
        });

        me.clear(notiserElement)

        return me;
    },

    // clearing notify after timeout
    clear: function (element) {
        var me = this;

        setTimeout(me.fadeOutAndRemoveElement, me.duration, element);
    },

    // fading out element and removing element from dom
    fadeOutAndRemoveElement : function(element) {
        element.fadeOut(function() {
            this.remove();
        });
    }
    
});

//显示一个消息，会在2秒钟后自动消失
Mini2.toast = function (text, duration, style) {     

    var frm = Mini2.createTop('Mini2.ui.Toast', {
        text: text,
        duration: duration || 3000,
        style: style || 'basic'
    });


    frm.show('.mi-toast-center-notify');

    //frm.render();

    return frm;
};