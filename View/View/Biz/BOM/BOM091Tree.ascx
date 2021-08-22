<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BOM091Tree.ascx.cs" Inherits="App.InfoGrid2.View.Biz.BOM.BOM091Tree" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>


<link rel="stylesheet" type="text/css" href="/Core/Scripts/jstree/3.0.2/themes/default/style.css" />
<script src="/Core/Scripts/jstree/3.0.2/jstree.js" type="text/javascript"></script>

<form action="" method="post">

<mi:Viewport runat="server" ID="viewport1" Main="true">
     <mi:Panel runat="server" ID="leftPanel" Region="West" Dock="Full" Scroll="Auto" Width="800">
        <mi:TreePanel runat="server" ID="TreePanel1" Dock="Full" Width="500" Region="West" OnSelected="TreePanel1_Selected" AllowDragDrop="true">
            <Types>
                <mi:TreeNodeType Name="default" Icon="/res/icon/application_view_columns.png" />
                <mi:TreeNodeType Name="table" Icon="/res/icon/table.png" />
                <mi:TreeNodeType Name="view" Icon="/res/icon/view.png" />
            </Types>
        </mi:TreePanel>
    </mi:Panel>
    <mi:Panel runat="server" ID="panel11"  Region="Center" Scroll="None" >
    <mi:FormLayout runat="server" ID="FormLayout1" ItemWidth="600" ItemLabelAlign="Right" 
        Height="600" Dock="Full" Region="Center" SaveEnabled="true">
        

                <mi:TriggerBox runat="server" ID="trdf" FieldLabel="物料编号" DataField="COL_7" OnButtonClick="test()" ButtonType="More"/>
                
                <mi:TextBox runat="server" ID="COL_8TB" FieldLabel="物料名称" DataField="COL_8" />
                <mi:TextBox runat="server" ID="COL_9TB" FieldLabel="物料规格" DataField="COL_9" />
                <mi:TextBox runat="server" ID="COL_10TB" FieldLabel="用料单位" DataField="COL_10" />
                <mi:TextBox runat="server" ID="COL_11TB" FieldLabel="使用量" DataField="COL_11" />
                <mi:TextBox runat="server" ID="COL_12TB" FieldLabel="损耗率%" DataField="COL_12" />
                <mi:TextBox runat="server" ID="COL_17TB" FieldLabel="成本价" DataField="COL_17" />
                <mi:TextBox runat="server" ID="COL_18TB" FieldLabel="成本额" DataField="COL_18" />
                <mi:TextBox runat="server" ID="COL_14TB" FieldLabel="工时" DataField="COL_14" />
                <mi:TextBox runat="server" ID="COL_13TB" FieldLabel="物料来源" DataField="COL_13" />
                <mi:TextBox runat="server" ID="COL_15TB" FieldLabel="所属部门" DataField="COL_15" />
                <mi:TextBox runat="server" ID="COL_16TB" FieldLabel="备注" DataField="COL_16" />
    </mi:FormLayout>

        <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="SubmitBtn" Width="120" Height="26" Command="btnSave"
            Text="保存" Dock="Center" />
    </mi:WindowFooter>
    </mi:Panel>
</mi:Viewport>

</form>
<script type="text/javascript">
    
    function test()
    {
        alert("这个忘了要弹出哪个界面了！");
    }
</script>

