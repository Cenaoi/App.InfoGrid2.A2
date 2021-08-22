<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConTest.ascx.cs" Inherits="App.InfoGrid2.View.Explorer.ConTest" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<!--域设置-->
<%if (false)
  { %>
<script src="../../Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<script src="../../Core/Scripts/Mini2/dev/Mini.js" type="text/javascript"></script>
<% } %>
<form action="" id="form1" method="post">
<mi:Store ID="Store1" runat="server" Model="UT_116" IdField="ROW_IDENTITY_ID">
</mi:Store>
<mi:Viewport runat="server" ID="viewport">
    <mi:SearchFormLayout runat="server" ID="searchForm1" Height="100"  >


        <mi:ComboBox runat="server" ID="ComboBox1" DataTable="" DataField="" TriggerMode="None" >
            <mi:ListItem Value="iPad" Text="苹果" />
            <mi:ListItem Value="Android" Text="安卓" />
        </mi:ComboBox>

        <mi:ComboBoxBase runat="server" ID="ComboBoxBase1" ValueField="Value" DisplayField="BB" TriggerMode="None" Value="3">
            <mi:ListItemBase Value="1" BB="AAAA" CC="好咯啊" />
            <mi:ListItemBase Value="2" BB="BBBB" CC="额外" />
            <mi:ListItemBase Value="3" BB="CCCC" CC="阿萨德" />
            <mi:ListItemBase Value="4" BB="DDDD" CC="客户" />
            <mi:ListItemBase Value="5" BB="EEEE" CC="变形金刚" />
        </mi:ComboBoxBase>

        <mi:SearchButtonGroup runat="server" ID="searchBtnGroups"></mi:SearchButtonGroup>

    </mi:SearchFormLayout>

    <mi:Panel runat="server" ID="panel1" Region="Center" Scroll="None">
    
        <mi:Toolbar runat="server" ID="tooblar1">
            <mi:ToolBarTitle Text="Bug 记录" />
            <mi:ToolBarButton Text="新建" OnClick="ser:Store1.Insert()" />
        </mi:Toolbar>
    
        <mi:Table ID="Table1" runat="server" Dock="Full" StoreID="Store1">
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />

                <mi:TriggerColumn >
                    
                    <MapItems>
                        <mi:MapItem SrcField="" TargetField="" />
                    </MapItems>
                </mi:TriggerColumn>
                
                <mi:SelectColumn DataField="COL_1" HeaderText="编码" TriggerMode="None" >
                    <mi:ListItem Value="A-01" Text="粤A-01" />
                    <mi:ListItem Value="A-02" Text="粤A-02" />
                    <mi:ListItem Value="A-03" Text="粤A-03" />
                    <mi:ListItem Value="A-04" Text="粤A-04" />
                    
                </mi:SelectColumn>
                    
                <mi:SelectBaseColumn DataField="COL_2" HeaderText="名称"  
                    ItemValueField="CODE" ItemDisplayField="TEXT" TriggerMode="None" >
                    <Items>
                        <mi:ListItemBase CODE="1111" TEXT="SSSS" NANE="黄伟清" />
                        <mi:ListItemBase CODE="2222" TEXT="222" NANE="刘等会" />
                        <mi:ListItemBase CODE="3333" TEXT="fs" NANE="的神色" />
                        <mi:ListItemBase CODE="4444" TEXT="fsss" NANE="企鹅额" />
                    </Items>
                    <MapItems>
                        <mi:MapItem SrcField="NANE" TargetField="COL_1" />
                    </MapItems>
                </mi:SelectBaseColumn>

                <mi:BoundField DataField="COL_1" HeaderText="COL 3" />
            </Columns>
        </mi:Table>

    </mi:Panel>

</mi:Viewport>
</form>


<script type="text/javascript">

    $(document).ready(function () {

//        var item = Mini2.create('Mini2.ui.form.field.DateRange', {
//            
//        });

//        item.render();

//        item = Mini2.create('Mini2.ui.form.field.NumRange', {

//        });

//        item.render();

    });
    

</script>