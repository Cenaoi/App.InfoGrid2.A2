/// <reference path="C:\xiaoyufu\App.InfoGrid2.A2\View\Core/Scripts/jquery/jquery-2.0.3.min.js" />

var XYF_MessageBox = function () {

    //这是整个弹出框 最底层
    var _Window = null;

    //新建 整个弹出框 最底层
    function _newWindow() {

        return $("<!--这是提示消息窗口-->" +
                 "<div class='modal fade bs-example-modal-lg' role='dialog' data-backdrop='false' style='z-index:2001;'>" +
                    "<div class='modal-dialog'>" +
                    "</div>" +
                 "</div>");
    }

    //这是显示文字 图片 或者 输入框  的 内容框
    var _Content = null;

    //新建 显示文字 图片 或者 输入框  的 内容框
    function _newContent() {
        return $("<div class='modal-content' style='overflow:hidden;'></div>");
    }

    
    //标题 
    var _Header = null;

    //新建 标题
    function _newHeader() {
        return $("<div class='modal-header'></div>");
    }


    //主体
    var _Body = null;

    //新建 主体
    function _newBody() {
        return $("<div class='modal-body'></div>");
    }


    //底部
    var _Footer = null;

    //新建 底部
    function _newFooter() {
        return $("<div class='modal-footer'></div>");
    }

    //把整个弹出层给清除掉 
    function dispose() {

        

        if (_Window) {

            _Window.modal("hide");

            _Window.children().remove();
        }

        if (_Content) {
            _Content.children().remove();
        }

        if (_Header) {

            _Header.children().remove();
        }
       
        if (_Body) {
            _Body.children().remove();
        }

        if (_Footer) {
            _Footer.children().remove();
        }
     

        _Window = null;

        _Content = null;

        _Header = null;

        _Body = null;

        _Footer = null;

        //不知道为什么这个东西没有自动隐藏  只能手动把它删除掉了
        //$(".modal-backdrop:first").remove();
    };

    //传过来的对象属性覆盖默认对象属性
    function apply(o, c) {
        if (o && c) {

            for (var p in c) {
                o[p] = c[p];
            }

        }
        return o;
    };

    //显示提示消息框  第一个参数也是对象{title:'表示标题，没写就没有标题',item:'表示自动消失的时间',text:'要提示的信息'} 第二个参数是一个对象，里面放消息字体的设置
    this.show = function (object, Parameter) {

        dispose();

        //默认属性对象是空的
        var cfg = {};


        apply(cfg, Parameter);

        


        //消息对象
        var textEl = $("<h4>" + object.text + "</h4>");

        for (var index in cfg) {

            textEl.css(index, cfg[index]);
        }





        _Content = new _newContent();

        _Window = new _newWindow();

        _Body = new _newBody();

        _Body.append(textEl);

        _Body.css("text-align", "center");
        
        //是否显示标题
        if (object.title) {

            _Header = new _newHeader();

            _Header.css("background-color", "#f2f2f2");

            var h3El = $("<h3>" + object.title + "</h3>");

            h3El.css("margin", "0px");


            _Header.append(h3El);

            _Content.append(_Header);


        }
        else {
            //没有标题body里面的显示要紧凑一点
            _Body.css("padding", "0px");
            //这是让显示内容顶在最上面
            _Window.children(".modal-dialog").css('margin', '0px auto');


        }



        //把消息添加进内容框里面
        _Content.append(_Body);


        //把内容添加进弹窗中
        _Window.find(".modal-dialog").append(_Content);

        //显示弹窗 不要阴影层
        _Window.modal({ show: "show", backdrop: false });

        //阻止内容框里面的点击事件冒泡
        _Content.click(function (e) { e.stopPropagation(); });

        //点击弹窗时清除弹窗的所有东西
        _Window.click(function () {
            console.log("modal.hide_1");
            dispose();
            //_Window.remove();
        });


        if (object.item) {

            //多少秒后自动消失
            setTimeout(function (owner) {
                console.log("modal.hide_1");

                dispose();

            }, object.item * 1000, this);

        } else {

            //多少秒后自动消失
            setTimeout(function (owner) {

                console.log("modal.hide_1");

                dispose();

            }, 3 * 1000, this);

        }
    };

    //询问框 第一个参数是要显示的消息 第二个参数是回调函数
    this.confirm = function (text, fn) {

        dispose();

        var me = this;


        _Window = new _newWindow();

        _Content = new _newContent();

        _Header = new _newHeader();

        _Body = new _newBody();

        _Footer = new _newFooter();

        _Body.text(text);

        _Body.css("font-size", "22px");

        _Header.text("询问框");
        _Header.css({ "background-color": "#f2f2f2", "text-align": "left" });

        _Content.append(_Header).append(_Body).append(_Footer);

        //把内容添加进弹窗中
        _Window.find(".modal-dialog").append(_Content);


        _Footer.css("text-align", "center");


        var btnOKEl = $("<button class='btn btn-success btn-save btn-lg' style='width:100pt;'>确定</button>");

        
    
        var btnCancelEl = $("<di<button class='btn btn-default  btn-close btn-lg' style='width:100pt;'>取消</button>");



        btnOKEl.click(function () {

       

            dispose();

            //不知道为什么这个东西没有自动隐藏  只能手动把它删除掉了
            $(".modal-backdrop:first").remove();

            fn();

    

        });

        _Footer.append(btnOKEl).append(btnCancelEl);

        _Window.click(function () {

            dispose();

            //不知道为什么这个东西没有自动隐藏  只能手动把它删除掉了
            $(".modal-backdrop:first").remove();

        });

        _Window.modal("show");

    };


}

