Mini2.define("Mini2.ui.form.Label",{extend:"Mini2.ui.form.field.Base",mode:"ENCODE",getFieldEl:function(){var b=this,a;if("ENCODE"==b.mode){b.inputEl=a=Mini2.$joinStr(['<label class="mi-form-field mi-form-label" ','aria-invalid="false" style="width: 100%; "></label>']);if(b.name){a.attr("name",b.name)}a.text(b.value).attr("id",b.clientId).css("text-align",b.align).attr("autocomplete",b.autoComplete)}else{b.inputEl=a=Mini2.$joinStr(['<div class="mi-form-field mi-form-label" ','aria-invalid="false" style="width: 100%; "></div>'])}return a},setValue:function(d){var c=this,a=c.hideEl,b=c.inputEl;if("ENCODE"==c.mode){b.val(d)}else{b.html(d)}if(a){a.val(d)}return c},getValue:function(){var c=this,a=c.hideEl,b=c.inputEl,d;if(c.readOnly&&a){d=a.val()}else{if("ENCODE"==c.mode){d=b.val()}else{d=b.html()}}return d},render:function(){var c=this,b,e=c.value;c.baseRender();c.inputEl.removeAttr("name");c.hideEl=b=c.createHideEl();b.val(e);var a="td.mi-form-item-body:first";var d=c.getTableContainer();c.appendCell(d,b,a);c.el.data("me",c)}});