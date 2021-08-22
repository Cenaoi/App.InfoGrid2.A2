<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsItemEdit.ascx.cs" Inherits="App.InfoGrid2.View.CMS.CmsItemEdit" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<script src="/Core/Scripts/UEditor/1.2.6.0/ueditor.config.js"></script>
<script src="/Core/Scripts/UEditor/1.2.6.0/ueditor.all.js"></script>

<link href="/Core/Scripts/webuploader-0.1.5/webuploader.css" rel="stylesheet" />
<script src="/Core/Scripts/webuploader-0.1.5/webuploader.js"></script>

<form method="post" >
    <mi:Store runat="server" ID="store1" Model="CMS_ITEM" IdField="CMS_ITEM_ID" PageSize="20" AutoSave="false">
        <FilterParams>
            <mi:Param Name="ROW_SID" DefaultValue="0" Logic="&gt;=" />
            <mi:QueryStringParam Name="CMS_ITEM_ID" QueryStringField="id" DbType="Int32" />
        </FilterParams>
    </mi:Store>
    <mi:Viewport runat="server" ID="viewport1">

        <mi:FormLayout runat="server" ID="HanderForm1" ItemWidth="800" PaddingTop="10" StoreID="store1" 
            ItemLabelAlign="Right" Region="Center"  FlowDirection="TopDown" >


            <mi:TextBox runat="server" ID="C_TITLE_tb" DataField="C_TITLE" FieldLabel="标题" />
            <mi:FileUpload runat="server" ID="C_IMAGE_fu" DataField="C_IMAGE_URL" FieldLabel="图片" />
            <mi:Textarea runat="server" ID="C_INTRO_tb" DataField="C_INTRO" FieldLabel="简介" />

<%--            <mi:Textarea runat="server" ID="C_CONTENT_tb2" DataField="C_CONTENT" FieldLabel="内容" Height="300" />--%>

            <mi:HtmlEditor runat="server" ID="htmlEditor1" DataField="C_CONTENT"  FieldLabel="内容"
                />
            <div dock="top" style="height:100px; border:none; text-align:center; vertical-align:bottom; padding:20px; color:#808080;">
                ---------------我是底线------------------
            </div>
        </mi:FormLayout>
        <mi:WindowFooter runat="server" ID="footer1">
            <mi:Button runat="server" ID="SaveBtn" Text="保存" Width="80" Dock="Center" Command="GoSave" />
        </mi:WindowFooter>
    </mi:Viewport>
</form>
