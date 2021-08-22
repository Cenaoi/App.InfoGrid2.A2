<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Waiting2.ascx.cs" Inherits="App.InfoGrid2.View.Biz.JLSLBZ.XSSCZL.Waiting2" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form method="post" >
        <img id="run" src="/res/采购分析.gif"  />
</form>

<script>

    document.onload = function () {
        setTimeout(function () {
            ownerWindow.close();

            //widget1.submit('form:first', {
            //    action: 'CloseWin'
            //});

        }, 1000 * 3);

    }();


</script>


