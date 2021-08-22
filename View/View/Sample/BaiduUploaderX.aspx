<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaiduUploaderX.aspx.cs" Inherits="App.InfoGrid2.View.Sample.BaiduUploaderX" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>



    <link href="/Core/Scripts/Mini2/Themes/theme-globel.css" rel="stylesheet" />
    <link href="/Core/Scripts/Mini2/Themes/theme-window.css" rel="stylesheet" />

    <script src="/Core/Scripts/jquery/jquery-2.0.3.js"></script>


    <script src="/Core/Scripts/Mini2/Mini2.js"></script>
    
    
    <!--引入CSS-->
    <link rel="stylesheet" type="text/css" href="/Core/Scripts/webuploader-0.1.5/webuploader.css" />

    <!--引入JS-->
    <script src="/Core/Scripts/webuploader-0.1.5/webuploader.js"></script>
</head>
<body style="font-size:12px;">


    <div id="game3X">


    </div>


    <script type="text/javascript">

        var thumbnailWidth = 256;
        var thumbnailHeight = 256;

        var renderTpl = [
            '<a class="mi-btn ',
                'mi-unselectable ',
                'mi-btn-default-small ',
                '" ',
                'role="button" hidefocus="on" unselectable="on" tabindex="0" >',
                
            '</a>'
        ];


        function joinStr(array) {
            var i=0,str = "";
            for (; i < array.length; i++) {
                str += array[i];
            }

            return str;
        }

        $(document).ready(function () {


            var btn = Mini2.create('Mini2.ui.button.Button', {
                text: '文件上传',
                renderTo: $('#game3X')
            })

            btn.render();


            for (var i = 0; i < 2; i++) {

                var file = Mini2.create('Mini2.ui.form.field.FileUpload', {
                    renderTo: $('#game3X')
                });

                file.render();

            }


        });


        $(document).ready(function () {

            return;

            $list = $('#thelist');

            var uploader = new WebUploader.Uploader({
                swf: '\Core\Scripts\webuploader-0.1.5\Uploader.swf',
                pick: '#picker',
                // 文件接收服务端。
                server: '/View/Sample/WebForm1.aspx',

                auto: true,
                chunked: true,
                resize: false

                // 其他配置项
            });

            // 当有文件添加进来的时候
            uploader.on('fileQueued', function (file) {
                var $li = $(
                        '<div id="' + file.id + '" class="file-item thumbnail">' +
                            '<img>' +
                            '<div class="info">' + file.name + '</div>' +
                        '</div>'
                        ),
                    $img = $li.find('img');


                // $list为容器jQuery实例
                $list.append($li);

                // 创建缩略图
                // 如果为非图片文件，可以不用调用此方法。
                // thumbnailWidth x thumbnailHeight 为 100 x 100
                uploader.makeThumb(file, function (error, src) {
                    if (error) {
                        $img.replaceWith('<span>不能预览</span>');
                        return;
                    }

                    $img.attr('src', src);
                }, thumbnailWidth, thumbnailHeight);
            });


            // 文件上传过程中创建进度条实时显示。
            uploader.on('uploadProgress', function (file, percentage) {
                var $li = $('#' + file.id),
                    $percent = $li.find('.progress span');

                // 避免重复创建
                if (!$percent.length) {
                    $percent = $('<p class="progress"><span></span></p>')
                            .appendTo($li)
                            .find('span');
                }

                $percent.css('width', percentage * 100 + '%');
            });

            // 文件上传成功，给item添加成功class, 用样式标记上传成功。
            uploader.on('uploadSuccess', function (file) {
                $('#' + file.id).addClass('upload-state-done');
            });

            // 文件上传失败，显示上传出错。
            uploader.on('uploadError', function (file) {
                var $li = $('#' + file.id),
                    $error = $li.find('div.error');

                // 避免重复创建
                if (!$error.length) {
                    $error = $('<div class="error"></div>').appendTo($li);
                }

                $error.text('上传失败');
            });

            // 完成上传完了，成功或者失败，先删除进度条。
            uploader.on('uploadComplete', function (file) {
                $('#' + file.id).find('.progress').remove();
            });

        });

    </script>
</body>
</html>
