<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DwgFilterVChanegSetup.ascx.cs" Inherits="App.InfoGrid2.View.MoreActionBuilder.DwgFilterVChanegSetup" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<link href="/Core/Scripts/codemirror/codemirror-5.21.0/codemirror.css" rel="stylesheet" />
<script src="/Core/Scripts/codemirror/codemirror-5.21.0/codemirror.js"></script>
<script src="/Core/Scripts/codemirror/codemirror-5.21.0/mode/clike/clike.js"></script>

<form action="" method="post">


    <div runat="server" id="StoreSet">
        <mi:Store runat="server" ID="storeMain1" AutoSave="false" AutoFocus="true" AutoCallback="false" >
            <StringFields>field,field_text,logic,from_value.to_value</StringFields>
        </mi:Store>
    </div>

    <mi:Viewport runat="server" ID="viewport1" Main="true" MarginTop="0" Padding="0">


        <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="300" PaddingTop="10" StoreID="storeMain1"
            ItemLabelAlign="Right" Region="Center" FlowDirection="TopDown" AutoSize="true">

            <mi:TriggerBox runat="server" ID="field_tb" DataField="field" FieldLabel="字段名" ButtonType="More"></mi:TriggerBox>
            <mi:TextBox runat="server" ID="TriggerBox3" DataField="field_text" FieldLabel="字段描述" Placeholder="字段描述" />

            <mi:ComboBox runat="server" ID="TriggerBox1" DataField="logic" FieldLabel="逻辑" TriggerMode="None">
                <mi:ListItem Value="" Text="" TextEx="-- N/A --" />
                <mi:ListItem Value="=" Text="等于" />
                <mi:ListItem Value=">" Text="大于" />
                <mi:ListItem Value="<" Text="小于" />
                <mi:ListItem Value="!=" Text="不等于" />
                <mi:ListItem Value="&lt;=" Text="小于或等于" />
                <mi:ListItem Value="&gt;=" Text="大于或等于" />
                <mi:ListItem Value="In" Text="In" />
                <mi:ListItem Value="NotIn" Text="NotIn" />
                <mi:ListItem Value="Like" Text="Like" />
                <mi:ListItem Value="NotLike" Text="NotLike" />
            </mi:ComboBox>
            
            <mi:CodeEditor runat="server" ID="tt3"  DataField="from_value" FieldLabel="原始值" MinWidth="600px" Height="180" />
            <mi:CodeEditor runat="server" ID="CodeEditor1"  DataField="to_value" FieldLabel="当前值" MinWidth="600px" Height="180" />


        </mi:FormLayout>


        <mi:WindowFooter runat="server" ID="footer1">
            <mi:Button runat="server" ID="Button1" Text="确定" Width="80" Height="26" Dock="Center" OnClick="ownerClose(true)" />
            <mi:Button runat="server" ID="Button2" Text="取消" Width="80" Height="26" Dock="Center" OnClick="ownerClose()" />
        </mi:WindowFooter>

    </mi:Viewport>
</form>


<script>
    "use strict";

    function ownerClose(success) {

        if (ownerWindow) {
            ownerWindow.close({ success: success });
        }

    }

    Mini2.ready(function () {

        var win = ownerWindow;

        var store = Mini2.find('storeMain1');

        if (!win.userData) {
            win.userData = {};
        }

        store.add(win.userData);


        win.formClosing(function () {
            var me = this;

            var rect = store.getCurrent();

            me.userData ={};
            
            ['field','field_text','logic','from_value','to_value'].forEach(function(){
                me.userData[this] = rect.get(this);
            });           
            

        });
                
    });

</script>