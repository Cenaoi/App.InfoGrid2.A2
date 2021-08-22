<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreeNodeStruceEdit.ascx.cs" Inherits="App.InfoGrid2.Sec.StructDefine.TreeNodeStruceEdit" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<form method="post">

    <mi:Store runat="server" ID="store1" Model="SEC_STRUCT" IdField="SEC_STRUCT_ID" PageSize="1" >
        <FilterParams>
            <mi:QueryStringParam Name="SEC_STRUCT_ID" QueryStringField="id" DbType="Int32"  />
        </FilterParams>
    </mi:Store>

<mi:Viewport runat="server" ID="viewport">
    <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="400" ItemLabelAlign="Right" StoreID="store1"
        Height="600">

        <mi:Label runat="server" ID="CodeLab" FieldLabel="编码" DataField="STRUCE_CODE" />
        <mi:TextBox runat="server" ID="tbText" FieldLabel="结构名称" DataField="STRUCE_TEXT" />

    </mi:FormLayout>
    <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="SubmitBtn" Width="120" Height="26" Command="btnSave" 
            Text="完成" Dock="Center" />
        <mi:Button runat="server" ID="Button2" Width="80" Height="26" OnClick="ownerWindow.close()"
            Text="取消" Dock="Center" />
    </mi:WindowFooter>
</mi:Viewport>
</form>
