

Mini2.define('Mini2.ui.form.field.TextArea', {

    extend: 'Mini2.ui.form.field.Text',

    alias: 'widget.textarea',

    cols: 20,

    rows: 4,

    //获取或设置一个值，该值指示用户能否使用 Tab 键将焦点放到该控件上。
    tabStop: false,

    bindInputEl_Key: function (inputEl) {

        var me = this;

        $(inputEl)
        .on('keydown', function (e) {

            if (13 == e.keyCode) {
                me.insertText(this, "\n");

                Mini2.EventManager.stopEvent(e);

                return;
            }

            me.on('keydown', e);
        })
        .on('keyup', function (e) {

            me.on('keyup', e);

            //Mini2.EventManager.stopEvent(e);
        })
        .on('keypress', function (e) {

            me.on('keypress', e);
            //Mini2.EventManager.stopEvent(e);
        });
    },



    setValue: function (value) {
        var me = this,
            hideEl = me.hideEl,
            inputEl = me.inputEl;

        inputEl.val(value);

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

        if (me.readOnly && hideEl) {
            value = hideEl.val();
        }
        else {
            value = inputEl.val();
        }
        
        return value;
    },

    getFieldEl: function () {

        var me = this,
            inputEl;

        me.inputEl = inputEl = Mini2.$joinStr([
            '<textarea class="mi-form-field mi-form-text mi-form-textarea" ',
                'aria-invalid="false" style="width: 100%;resize: none; " ',
                'spellcheck="false" autocapitalize="off" autocomplete="off" autocorrect="off">',
            '</textarea>']);


        if (me.name) {
            inputEl.attr('name', me.name);
        }

        inputEl.attr("id", me.clientId)
            .attr('placeholder', me.placeholder)
            .attr('autocomplete', me.autoComplete)
            .attr('cols', me.cols)
            .attr('rows', me.rows);

        inputEl.css({
            'text-align': me.align,
            'height': me.height
        });


        if (me.readOnly) {
            inputEl.attr('readonly', 'readonly');
        }

        //附加属性
        //me.inputEl.attr("data-errorqtip", "");
        //

        //inputEl.addClass("mi-form-required-field");

        me.bindInputEl_Mouse(inputEl);
        me.bindInputEl_Key(inputEl);


        

        return inputEl;

    }

});