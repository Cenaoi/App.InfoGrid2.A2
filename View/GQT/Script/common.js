
//加载srcipt
function loadScript(url, callback) {


    var script = document.createElement("script");
    script.type = "text/javascript";
    script.onload = function (e) {
        if (callback) {
            callback(e);
        }
    };

    script.src = url;

    var head = document.head || document.getElementsByTagName("head")[0];

    head.appendChild(script);
};

//加载样式
function loadCss(url) {

    var css = document.createElement("link");
    css.rel = "stylesheet";

    css.href = url;

    var head = document.head || document.getElementsByTagName("head")[0];

    head.appendChild(css);


}

//加载style样式数据
function loadStyle(text) {
    
    var style_a = document.createElement("style");

    style_a.textContent  += text;

    var head = document.head || document.getElementsByTagName("head")[0];


    head.appendChild(style_a);



}


loadCss("/Core/Scripts/SUI/sm.css");


loadCss("/Core/Scripts/SUI/swiper-3.4.0.min.css");


//加载主要srcipt
loadScript("/Core/Scripts/m5/M5.min.js", function () {


    loadScript("/Core/Scripts/vue/vue-2.0.1.js", function () {

        loadScript("/Core/Scripts/moment/moment-2.9.js");
        loadScript("/Core/Scripts/XYF/xyfUtil.js");

    });

    loadScript("/Core/Scripts/SUI/swiper-3.4.0.min.js");


});


//界面加载完成事件   这是给禁止苹果浏览器的动态效果
window.addEventListener("load", function () {

    console.info("界面加载完成了！");

    //FastClick.attach(document.body);

    $("body").height($(window).height());


    $("body").width($(window).width());

    $.router = Mini2.create('Mini2.ui.PageRoute', {});

    if ($.device.ios) {

        loadStyle(" .page .bar .back {   display:none;  } ");

    }
})



//界面加载完成事件  这是界面随着window的高度和宽度改变而改变
window.addEventListener("resize", function () {

    $("body").height($(window).height());

    $("body").width($(window).width());

})


