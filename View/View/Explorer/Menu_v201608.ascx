<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu_v201608.ascx.cs" Inherits="App.InfoGrid2.View.Explorer.Menu_v201608" %>



<style>

    .mu-icon-box{
        padding:30px 20px 30px 20px; 
        position:relative;
    }

    .mu-icon{
        font-family:'微软雅黑';
        height:72px; 
        width:72px; 
        border-radius:4px; 
        overflow:hidden;
        padding-top:16px;
        color:white;
        font-size:30px;
        margin-left:auto;
        margin-right:auto; 
        cursor:pointer; 
        text-align: center;
    }

    .mu-icon-text{
        font-family:'微软雅黑';
        color:white;
        font-size:30px;
        cursor:pointer; 
        pointer-events:none;
    }

    .mi-icon-label{
        overflow: hidden;
        color:#08c;
        margin-top:5px;
        font-weight:normal;
        margin-left:auto;
        margin-right:auto;
        text-align: center;
    }

    .mi-icon-tag{
        right:36px;
        top: 32px;
        position:absolute;

        width:16px;
        height:16px;
        background-image:url(/res/icon/application_view_columns.png)
    }

</style>


<div class="container" style="max-width:1000px; margin-right:auto;margin-left:auto;">

       
    <div style="height:200px;width:200px; padding:30px; float:left;" v-for="m in menu_list">

        <div class="menu_box mu-icon-box"  onmousemove="changeColor(this)" onmouseout="changColor_1(this)" 
            >
            <div class="text-center mu-icon" v-bind:style="{backgroundColor:m.color_value}"  @click="btn_click($index,$event)" @mouseout="btn_mouseout($index)"  @mousemove="btn_mouseover($index,$event)" >
                <span class="mu-icon-text" v-bind:style="{color:m.color}"  v-if="m.ICON_CHAT">{{m.ICON_CHAT}}</span>
                <span class="mu-icon-text" v-bind:style="{color:m.color}" v-else>{{m.TEXT.substr(0,1)}}</span>
            </div>
            <div class="text-center mi-icon-label" >
                {{m.TEXT}}
            </div>
            <div class="mi-icon-tag" v-if="m.ITEMS" ></div>
        </div>
    </div>
      
</div>

 <div class="" style="height:150px;width:100%; float:left;">
          
 </div>
<script src="/Core/scripts/bootstrap/3.3.4/js/bootstrap.min.js"></script>

<script src="/Core/scripts/XYF/MessageBox_xyf.js"></script>

<script src="/Core/scripts/vue/vue.js"></script>



<script>


    $(document).ready(function () {



        var messageBox = new XYF_MessageBox();


        var menu_json = <%=GetMenuJson() %>;
        
        //前面圆点显示样式
        var classIndex = 0;

        var color_arr = ['#2e6da4','#5cb85c','#5bc0de','#f0ad4e','#c9302c','#9f5edd','#bd753b','#e87f5a','#d7d91a','#89cd38','#686767'];


        menu_json.forEach(function(v,i){

            classIndex = classIndex % color_arr.length;

            if(v.ICON_BG_COLOR){
                v['color_value'] = v.ICON_BG_COLOR;
            }
            else{
                v['color_value'] = color_arr[classIndex];
                
                classIndex++;
            }

            if(v.ICON_COLOR){
                v['color'] = v.ICON_COLOR;
            }
            else{
                //v['color'] = '#FF0000';
            }


        });



        var my_vue = new Vue({
            el:'.container',
            data:{
                menu_list:menu_json,
                id:0
            },
            methods:{

                btn_click:function(index,evt){

                    var me = this;

                    var item = me.menu_list[index];

                    if(item.ITEMS){
                        
                        showMenu(evt.target, index, item.ITEMS);
                    }
                    else{
                        if(item.URL == '#'){
                            var url = Mini2.urlAppend('/App/InfoGrid2/View/Explorer/Menu_v201608.aspx',{
                                p_id: item.ID ,
                                menu_level : item.MENU_LEVEL
                            });

                            Mini2.EcView.show(url,item.TEXT);
                        }
                        else{

                            Mini2.EcView.show(item.URL,item.TEXT);
                        }
                    }

                    Mini2.EventManager.stopEvent(evt);
                },

                btn_mouseover:function(index, evt){
                    var me = this;

                    if(index != me.curIndex){

                        me.curIndex = index;

                        var item = me.menu_list[index];

                        if(item.ITEMS){
                        
                            showMenu(evt.target, index, item.ITEMS);
                        }
                    }

                },

                btn_mouseout:function(index){
                    var me = this;
                    me.curIndex = null;

                }
            }

        });

    });

    var m_Menus = {};

    function showMenu(sender,index, menuItems){
        var me = sender;

        var pos = $(sender).offset();
        


        var innerMenu = m_Menus[index] ;

        if(innerMenu){
            innerMenu.setTop(pos.top - 6);
            innerMenu.setLeft(pos.left + 80);
            innerMenu.delayShow();   
            return;
        }
        
        var items = [];

        for (var i = 0; i < menuItems.length; i++) {
            
            var it = menuItems[i];

            items.push({
                text : it.TEXT,
                url : it.URL,
                click: function(){
                    Mini2.EcView.show(this.url, this.text);
                }
            });

        }

        var menu = Mini2.create('Mini2.ui.menu.Menu',{
            left: pos.left+80,
            top : pos.top - 6,
            items:items
        });

        menu.render();

        m_Menus[index] = menu;
    }


    function changeColor(me){

        $(me).css("background-color","#f3f3f3");

        $(me).children(".div_delete_btn").show();


    }

    function changColor_1(me){

        $(me).css("background-color","");

        $(me).children(".div_delete_btn").hide();

    }

</script>

