Mini2.define("Mini2.ui.MessageBox",{extend:"Mini2.ui.Window",mode:true,buttonAlign:"center",startPosition:"center_screen",width:350,height:130,icon:"info",buttons:[{text:"ȷ��",width:80,click:function(a){this.ownerWindow.close()}}],INFO:Mini2.baseCSSPrefix+"message-box-info",WARNING:Mini2.baseCSSPrefix+"message-box-warning",QUESTION:Mini2.baseCSSPrefix+"message-box-question",ERROR:Mini2.baseCSSPrefix+"message-box-error",contentTpl:['<div class="mi-box-inner " role="presentation" style="height: 55px; width: 339px;" dock="full">','<div class="mi-box-target" style="width: 339px;">','<div class="mi-container mi-box-item mi-window-item mi-container-default mi-box-layout-ct" ','style="overflow: hidden; padding: 10px; margin: 0px; right: auto; left: 0px; top: 0px;">','<div class="mi-box-inner " role="presentation" style="width: 319px; height: 35px;">','<div class="mi-box-target" style="width: 319px;">','<div class="mi-component mi-box-item mi-component-default mi-dlg-icon " ','style="width: 50px; height: 35px; margin: 0px; right: auto; left: 0px; top: 0px;">',"</div>",'<div class="mi-container mi-box-item mi-container-default" style="margin: 0px;right: auto; left: 50px; top: 0px;">','<span style="display: table;">','<div class="mi-message-box-textbox" style="height: 100%; vertical-align: top; display: table-cell;">',"</div>","</span>","</div>","</div>","</div>","</div>","</div>","</div>"],title:"����",msg:"��ʾ��Ϣ",renderHeader:function(a){var c=this;if(c.headerVisible){var b=c.header=Mini2.create("Mini2.ui.WindowHeader",{width:c.width,owner:c,renderTo:a,text:c.title});b.render();$(b).bind("action",c,c.header_Action)}},renderContent:function(){var f=this,b,c,e,d,a=f.bodyEl;f.contentEl=b=Mini2.$joinStr(f.contentTpl);a.append(b);f.iconEl=c=b.find(".mi-dlg-icon");f.inputBodyEl=d=b.find(".mi-message-box-textbox");switch(f.icon){case"error":c.addClass(f.ERROR);break;case"question":c.addClass(f.QUESTION);break;case"warning":c.addClass(f.WARNING);break;default:c.addClass(f.INFO);break}f.inputEl=e=Mini2.create("Mini2.ui.form.Label",{renderTo:d,hideLabel:true,value:f.msg});e.render()},render:function(){var a=this;a.renderMask();a.renderEl();a.renderContent();a.setSize(a.width,a.height);a.setLocation(a.left,a.top,true)}});Mini2.Msg={alert:function(e,c,b){var d,a;a={title:e,msg:c,icon:"info",buttons:[{text:"ȷ��",width:80,click:b?b:function(f){this.ownerWindow.close()}}]};d=Mini2.create("Mini2.ui.MessageBox",a);d.show()},prompt:function(e,c,b){var d,a;a={title:e,msg:c,icon:"question",buttons:[{text:"ȷ��",width:80,click:function(f){this.ownerWindow.close();b.call(this)}},{text:"ȡ��",width:80,click:function(f){this.ownerWindow.close()}}]};d=Mini2.create("Mini2.ui.MessageBox",a);d.show()}};