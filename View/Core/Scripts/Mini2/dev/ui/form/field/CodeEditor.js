

Mini2.define('Mini2.ui.form.field.CodeEditor', {

    extend: 'Mini2.ui.form.field.Text',

    alias: 'widget.codeeditor',
    cols: 20,

    rows: 4,

    extraFieldBodyCls:'mi-code-editor-panel',


    mode: 'text/x-csharp',

    setValue: function (value) {
        var me = this,
            hideEl = me.hideEl,
            codeEl = me.codeEl,
            editor = me.editor;

        me.value = value;
        codeEl.val(value);

        if (editor && value) {
            try{
                editor.setValue(value);
            }
            catch (ex) {
                console.debug('editor.setValue(...) = ', value);
                console.error('代码编辑框赋值错误. ', ex);
            }
        }

        if (hideEl) {
            hideEl.val(value);
        }

        return me;
    },

    getValue: function () {
        var me = this,
            hideEl = me.hideEl,
            inputEl = me.inputEl,
            value;

        return me.value;

        //if (me.readOnly && hideEl) {
        //    value = hideEl.val();
        //}
        //else {
        //    value = me.codeEl.val();
        //}

        return value;
    },

    setSize:function(width, height){
        var me = this;

        console.debug("dddddddddddddd", height);
    },

    setHeight: function (value) {
        var me = this,
            editor = me.editor;

        me.height = value;
        me.el.css('height', value);

        if (editor) {
            editor.setSize("100%", me.height + 'px');
        }

        return me;
    },

    getFieldEl: function () {

        var me = this,
            inputEl,
            codeEl;


        me.inputEl = inputEl = Mini2.$joinStr([
            '<div>',
                '<div role="toolbar"></div>',
                '<textarea class="code-box" style="display:none;">',
                '</textarea>',
            '</div>'
        ]);

        me.codeEl = codeEl = $(inputEl).children('.code-box');

        //return inputEl;

        //me.inputEl = inputEl = Mini2.$joinStr([
        //    '<textarea class="mi-form-field mi-form-text mi-form-textarea" ',
        //        'aria-invalid="false" style="width: 100%;resize: none; " ',
        //        'spellcheck="false" autocapitalize="off" autocomplete="off" autocorrect="off">',
        //    '</textarea>']);


        if (me.name) {
            codeEl.attr('name', me.name);
        }

        inputEl.attr("id", me.clientId);

        //inputEl.attr("id", me.clientId)
        //    .attr('placeholder', me.placeholder)
        //    .attr('autocomplete', me.autoComplete)
        //    .attr('cols', me.cols)
        //    .attr('rows', me.rows);

        //inputEl.css({
        //    'text-align': me.align,
        //    'height': me.height
        //});


        //if (me.readOnly) {
        //    inputEl.attr('readonly', 'readonly');
        //}

        

        return inputEl;

    },

    
    render: function () {
        "use strict";
        var me = this,
            editor,
            inputEl,
            codeEl,
            bodyCls = '.mi-form-item-body:first';


        me.baseRender();

        me.el.data('me', me);

        inputEl = me.inputEl;

        //var codeEl = $(inputEl).children('.code-box');

        //console.debug("me.height = ", me.height);
        codeEl = me.codeEl;


        //console.debug("初始化", me.value);

        var editor = CodeMirror.fromTextArea(codeEl[0], {
            lineNumbers: true,
            mode: me.mode,
            autofocus: false

        });


        editor.setSize("100%", me.height + 'px')


        me.editor = editor;
        
        setTimeout(function () {

            editor.on("blur", function (Editor, changes) {
                
                me.value = Editor.getValue();

                //console.debug("触发保存",me.value);

                inputEl.val(me.value);

                me.autoSave();
            });

        }, 1000);

    },

    /**
    * 重置层次
    */
    resetPaint: function () {
        var me = this,
            editor = me.editor;

        //console.debug("CodeEditor resetPaint()");

        editor.refresh();

        return me;
    },

    autoSave: function () {
        "use strict";
        var me = this,
            saveTimer = me.saveTimer;

        if (saveTimer) {
            saveTimer.resetStart();
        }
        else {

            me.saveTimer = saveTimer = Mini2.setTimer(function () {

                me._isValueChanged = true;

                var owner = me.ownerParent;

                if (owner && owner.setStoreRecrod) {
                    owner.setStoreRecrod.call(owner, me);
                }

                var editor = me.editor;

                if (editor) {
                    editor.setSize("100%", me.height + 'px')
                }

            }, {
                interval: 500,
                limit: 1
            });
        }
    }
});