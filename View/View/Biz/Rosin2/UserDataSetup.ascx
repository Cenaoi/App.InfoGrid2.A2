<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDataSetup.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Rosin2.UserDataSetup" %>
<!--用户数据清空-->

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" id="form1" method="post">


    
<div class="mi-newline">
    <div class="mi-newline-text">清空用户数据库</div>
</div>

<table align="center" style="width:400px;" border="0" >
    

    <tr>
        <td align="center">
            
            <mi:Button runat="server" ID="Button1" Text="初始化-基础数据" Height="26" Command="GoResetDict" 
                BeforeAskText="确定要初始化数据库? 将不可恢复."  />

            <mi:Button runat="server" ID="SubmitBtn" Text="初始化-计划数据" Height="26" Command="GoResetPlan" 
                BeforeAskText="确定要初始化数据库? 将不可恢复."  />

            <mi:Button runat="server" ID="Button2" Text="初始化-其它单据" Height="26" Command="GoResetOther" 
                BeforeAskText="确定要初始化数据库? 将不可恢复."  />
        </td>
    </tr>
</table>

</form>

