<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Websites.ascx.cs" Inherits="App.InfoGrid2.View.Biz.MR.Websites" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<!--网站信息-->
<form action="" method="post" >
<mi:Store runat="server" ID="store1" Model="IG2_MAP" IdField="MR_WEBSITE_ID"  PageSize="100" AutoSave="true">
    <StringFields>WEBSITE_TEXT,IS_STOP,REG_DATE,END_DATE,WS_USER_TEL,WS_USER_TEXT,WS_USER_QQ,WS_USER_ADDRESS,REMARKS,ROOT_CATALOG,BIZ_SID</StringFields>
    <TSqlQuery Enabeld="true"></TSqlQuery>
</mi:Store>

<mi:Store runat="server" ID="store2" Model="IG2_MAP" IdField="MR_WEBSITE_MAP_ID"  PageSize="0" AutoSave="true" >
    <StringFields>DOMAIN_TEXT,DB_CATALOG,USER_CODE</StringFields>
    <TSqlQuery Enabeld="true"></TSqlQuery>
</mi:Store>

<mi:Viewport runat="server" ID="viewport1" Main="true">

    <mi:Panel runat="server" ID="centerPanel" Dock="Full" Region="North" Height="300" Scroll="None" >
        <mi:SearchFormLayout runat="server" ID="searchForm" StoreID="store1" Visible="false">
            <mi:TextBox runat="server" ID="textBox3" FieldLabel="网站名称" DataField="WEBSITE_TEXT" DataLogic="like" />
            <mi:Button runat="server" ID="Button1"  Text="查询"  Command="GoStore1Select"/>
        </mi:SearchFormLayout>
        <mi:Toolbar ID="Toolbar1" runat="server">

            <mi:ToolBarTitle ID="tableNameTB1" Text="网站信息" />

            <mi:ToolBarButton Text="新增" OnClick="ser:store1.Insert()" />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store1.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="查找" OnClick="widget1_I_searchForm.toggle()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store1.Delete()" />

            <mi:ToolBarButton Text="生成目录" Command="GoCreateDir" />

            <mi:ToolBarButton Text="资源管理器" Command="GoShowExplorer" />


        </mi:Toolbar>
        <mi:Table runat="server" ID="table1" StoreID="store1" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:SelectColumn HeaderText="业务状态" DataField="BIZ_SID" TriggerMode="None" EditorMode="None">
                    <mi:ListItem Text="未创建目录" Value="0" />
                    <mi:ListItem Text="已创建目录" Value="999" />
                </mi:SelectColumn>
                <mi:BoundField HeaderText="网站名称" DataField="WEBSITE_TEXT" />
                <mi:BoundField HeaderText="目录名称" DataField="ROOT_CATALOG" />
                <mi:CheckColumn HeaderText="停止网站" DataField="IS_STOP" Width="100"/>
                <mi:DateColumn HeaderText="注册时间" DataField="REG_DATE" />
                <mi:DateColumn HeaderText="到期时间" DataField="END_DATE" />
                <mi:BoundField HeaderText="网站用户电话" DataField="WS_USER_TEL" />
                <mi:BoundField HeaderText="网站用户名称" DataField="WS_USER_TEXT" />
                <mi:BoundField HeaderText="网站用户QQ或者微信" DataField="WS_USER_QQ" />
                <mi:BoundField HeaderText="网站用户地址" DataField="WS_USER_ADDRESS" />
                <mi:BoundField HeaderText="备注" DataField="REMARKS" />
            </Columns>
        </mi:Table>
    </mi:Panel>


     <mi:Panel runat="server" ID="Panel1" Dock="Full" Region="Center" Scroll="None" >

        <mi:SearchFormLayout runat="server" ID="SearchFormLayout1" StoreID="store1" Visible="false">
            <mi:TextBox runat="server" ID="textBox1" FieldLabel="域名" DataField="DOMAIN_TEXT" DataLogic="like" />
            <mi:Button runat="server" ID="Button2"  Text="查询"  Command="GoStore2Select"/>
        </mi:SearchFormLayout>

        <mi:Toolbar ID="Toolbar2" runat="server">
            <mi:ToolBarTitle ID="tableNameTB2" Text="域名列表" />

            <mi:ToolBarButton Text="新增" OnClick="insertDomain()"  />
            <mi:ToolBarButton Text="刷新" OnClick="ser:store2.Refresh()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="查找" OnClick="widget1_I_SearchFormLayout1.toggle()" />
            <mi:ToolBarHr />
            <mi:ToolBarButton Text="删除" BeforeAskText="您确定删除记录?"  OnClick="ser:store2.Delete()" />





        </mi:Toolbar>
        <mi:Table runat="server" ID="table2" StoreID="store2" Dock="Full" >
            <Columns>
                <mi:RowNumberer />
                <mi:RowCheckColumn />
                <mi:BoundField  HeaderText="用户编码" DataField="USER_CODE" Visible="false"/>
                <mi:BoundField HeaderText="域名" DataField="DOMAIN_TEXT" />
                <mi:BoundField HeaderText="数据库目录名称" DataField="DB_CATALOG" />
            </Columns>
        </mi:Table>
    </mi:Panel>



</mi:Viewport>

</form>

<script>

    //域名插入按钮事件
    function insertDomain() {

        Mini2.Msg.prompt("请输入域名",function () {

           var va =  this.getValue();
           
           widget1.submit('form:first', {
               action: 'GoInsertDomain',
               actionPs: va
           });


        })


    }


</script>



