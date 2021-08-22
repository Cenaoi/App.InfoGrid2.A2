<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Table1CopyToTable2.ascx.cs" Inherits="App.InfoGrid2.View.Biz.Input_Data.Table1CopyToTable2" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" id="form1" method="post">


<table align="center" style="width:400px;" border="0" >
 
    <tr>
      <td>
             <mi:TextBox runat="server" ID="tbxDataSource" FieldLabel="数据源表"/>
        </td>
   </tr>
   <tr>
       <td>
            <mi:TextBox runat="server" ID="tbxTarget" FieldLabel="目标表"  />
            
        </td>
   </tr>
    <tr>
        <td>
            <mi:TextBox runat="server" ID="tbxMapID" FieldLabel="映射规则ID"/>
        </td>
    </tr>
    <tr>
        <td>
            <mi:Button runat="server" ID="Button1" Text="确定" Width="80" Height="26" Command="GoSure"  />
        </td>
        <td>
            <mi:Button runat="server" ID="SubmitBtn" Text="清除目标表" Width="80" Height="26" Command="ClearData"  />
        </td>
    </tr>
</table>

</form>
