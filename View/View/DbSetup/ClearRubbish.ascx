<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClearRubbish.ascx.cs" Inherits="App.InfoGrid2.View.DbSetup.ClearRubbish" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<style>
    body{
        background-color:#FFFFFF;
    }
</style>
<form action="" id="form1" method="post">

    <mi:Viewport runat="server" ID="viewport1">

        <mi:FormLayout runat="server" ID="formLayout1" Region="North" >

            <mi:NumberBox runat="server" ID="num1" FieldLabel="清除n天以前数据" Value="1" ChangedCallback="" />

            <mi:Button runat="server" ID="btn1" Text="清理" Width="100" MarginLeft="10px" />
        </mi:FormLayout>

        <mi:Panel runat="server" ID="panel1" Region="Center">

            <mi:Table runat="server" ID="table1" Dock="Center" PagerVisible="false">
                <Columns>
                    <mi:RowNumberer />
                    <mi:RowCheckColumn />

                    <mi:BoundField HeaderText="表名" />
                    <mi:BoundField HeaderText="描述" />

                    <mi:NumColumn HeaderText="作废数量" />

                </Columns>
                
            </mi:Table>

        </mi:Panel>
        
    </mi:Viewport>


</form>

<script>

    $(document).ready(function () {

        setTimeout(function () {


        }, 550);


    });

</script>   