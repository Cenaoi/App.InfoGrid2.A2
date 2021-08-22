
Mini.ui.Fileupdate = function (options) {
    /// <summary>文件上传组件</summary>

    var m_Swfu = null;

    var defaults = {
        file_types: "*.*",
        file_size_max: 100000, //文件的大小
        file_queue_limit: 1  //每次只能上传的文件数
    };

    function init(options) {


    }

    this.show = function () {
        /// <summary>显示上传窗体</summary>


        if (m_Swfu == null) {
            m_Swfu = new SWFUpload({

                upload_url: "/Core/SWFUpload/Upload.aspx",
                flash_url: "/Core/SWFUpload/swfupload.swf",

                flash_width: "100px",
                flash_height: "20px"

            });
        }

        m_Swfu.selectFiles();
    }

    init(options);
}
