<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CurrencyWaiting.ascx.cs" Inherits="App.InfoGrid2.View.Biz.JLSLBZ.XSSCZL.CurrencyWaiting" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<div style="width:100%;height:100%">
    <img id="doc_img"  src="/res/采购分析.gif"  />
</div>


<script>


    $(function () {

        var img_src = $.query.get("img_src");

        $('#doc_img').attr('src', img_src);

        setTimeout(function () {
            ownerWindow.close();

            //widget1.submit('form:first', {
            //    action: 'CloseWin'
            //});

        }, 1000 * 8);

    });


</script>

