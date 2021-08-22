<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ToolbarStepNew1.ascx.cs" Inherits="App.InfoGrid2.View.OneToolbar.ToolbarStepNew1" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" id="form1" method="post">


<span>设置 - 工具栏</span>
<hr />


<table align="center" style="width:600px;" border="0" >
    <tr>
        <td style="padding: 20px; border: 1px solid #CCCCCC; background-color: #FFFFFF;">
            <mi:CheckboxGroup runat="server" ID="checkboxGroup1" LabelAlign="Right" FieldLabel="默认按钮" DefaultValue="INSERT,SAVE,REFRESH,SEARCH,TO_EXCEL,DELETE">
                <mi:ListItem Value="INSERT" Text="新建" />
                <mi:ListItem Value="SAVE" Text="保存" />
                <mi:ListItem Value="REFRESH" Text="刷新" />

                
                <mi:ListItem Value="SEARCH" Text="查询" />
                <mi:ListItem Value="TO_EXCEL" Text="导出 Excel" />

                <mi:ListItem Value="DELETE" Text="删除" />
            </mi:CheckboxGroup>
        </td>
    </tr>
    <tr>
        <td align="center" style="padding:20px;">
            <mi:Button runat="server" ID="SubmitBtn" Text="下一步" Width="80" Height="26" Command="GoNext"  />
        </td>
    </tr>
</table>



   
</form>