<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageSingleTableTemlate.ascx.cs" Inherits="App.InfoGrid2.View.PrintTemplate.ManageSingleTableTemlate" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" method="post">

<mi:Store runat="server" ID="store1" Model="BIZ_PRINT_TEMPLATE" IdField="BIZ_PRINT_TEMPLATE_ID"  >
    <FilterParams>
        <mi:QueryStringParam Name="PAGE_ID" QueryStringField="pageID" DbType="Int32" />
        <mi:Param Name="ROW_SID" Logic="&gt;=" DefaultValue="0"  />
    </FilterParams>
    <DeleteQuery>
        <mi:ControlParam Name="BIZ_PRINT_TEMPLATE_ID" ControlID="table1" PropertyName="CheckedRows" />
    </DeleteQuery>
</mi:Store>

<mi:Viewport runat="server" ID="viewport">
    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="Center" Scroll="None" >
        <mi:Toolbar runat="server" ID="toolbar2">
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarButton Text="删除" OnClick="ser:store1.Delete()" BeforeAskText="确定删除了吗？" />
            <mi:ToolBarButton Text="上传模板" Command="InputTemplate" />
            <mi:ToolBarButton Text="下载模板" Command="DowTemplate" />
            <mi:ToolBarButton Text="导出Excel文件" Command="InputOut" />
            
        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" RowLines="true" ColumnLines="false" StoreID="store1" Dock="Full" CheckedMode="Single" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField DataField="PAGE_TEXT" HeaderText="页面名称" EditorMode="None" />
                <mi:BoundField DataField="TEMPLATE_NAME" HeaderText="模板名称" />
                <mi:BoundField DataField="TEMPLATE_URL" HeaderText="模板路径" EditorMode="None" />
                <mi:BoundField DataField="TEMPLATE_TYPE" HeaderText="模板类型" EditorMode="None" />
                <mi:BoundField DataField="MAIN_TABLE_NAME" HeaderText="主表名" EditorMode="None" />
                <mi:BoundField DataField="SUB_TABLE_NAME" HeaderText="子表名" EditorMode="None" />
                <mi:BoundField DataField="REMARKS" HeaderText="备注" Visible="false" />
            </Columns>
        </mi:Table>
    </mi:Panel>
    <mi:WindowFooter runat="server" ID="footer1">
        <mi:Button runat="server" ID="Button2" Text="取消" Width="80" Height="26" Dock="Center"
            OnClick="ownerWindow.close()" />
    </mi:WindowFooter>
</mi:Viewport>

</form>


<script>

    function ShowUrl(urlStr) {

        var win = Mini2.create('Mini2.ui.Window', {
            mode: true,
            text: '列表框架',
            iframe: true,
            width: 300,
            height: 200,
            startPosition: 'center_screen',
            url: urlStr
        });

        win.show();

        win.formClosed(function (e) {
            if (e.result != 'ok') { return; }

            widget1.submit('form:first', {
                action: 'UpdateData',
                actionPs: e.id
            });
            //record.set('ACTION_TABLE_ITEMS', 'TABLE,' + e.sviewId);

        });
    }


    ///新的下载界面
    function DonwloadShow(fileName, fileUrl) {

        var dw = Mini2.create('Mini2.ui.extend.DownloadWindow', {
            fileName: fileName,
            fielUrl: fileUrl
        });

        dw.show();
    }

</script>


