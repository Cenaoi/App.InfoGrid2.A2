<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Waiting.ascx.cs" Inherits="App.InfoGrid2.View.Biz.JLSLBZ.XSSCZL.Waiting" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form method="post" >
        <img id="run" src="/res/生产指令单运算.gif"  />
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





