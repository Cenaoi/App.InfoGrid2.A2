<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepCopySEC.ascx.cs" Inherits="App.InfoGrid2.Sec.UIFilterTU.StepCopySEC" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form action="" id="form1" method="post">

<span>拷贝 - 用户权限</span>
<hr />


<table align="center" style="width:400px;" border="0" >
     <tr>
        <td align="center" style="padding: 20px; border-left:1px solid #CCCCCC; border-right:1px solid #CCCCCC; border-top: 1px solid #CCCCCC; background-color: #FFFFFF; color:red;">
            <P>请注意，拷贝权限会把自身权限给删除掉！</P>
            <p>从你点击的用户权限复制到下面你写的用户代码里</p>

        </td>
    
     </tr>

    <tr>
        <td style="padding: 20px; border-left:1px solid #CCCCCC; border-right:1px solid #CCCCCC;   background-color: #FFFFFF;">
            <mi:ComboBox runat="server" ID="cbOneOrAll" TriggerMode="None" FieldLabel="单界面或全部" Value="1" >
                <mi:ListItem Text="单界面" Value="1" />
                <mi:ListItem Text="全部界面" Value="2" />
            </mi:ComboBox>
        </td>
    </tr>

    <tr>
        <td style="padding: 20px; border-left:1px solid #CCCCCC; border-right:1px solid #CCCCCC;   background-color: #FFFFFF;">
            <mi:ComboBox runat="server" ID="cbModeID" DefaultValue="2" TriggerMode="None" FieldLabel="类型"  >
                <mi:ListItem Text="用户" Value="2" />
                <mi:ListItem Text="角色" Value="1" />
            </mi:ComboBox>
        </td>
    </tr>
    <tr>
        <td style="padding: 20px; border-left:1px solid #CCCCCC; border-right:1px solid #CCCCCC;  background-color: #FFFFFF; border-bottom: 1px solid #CCCCCC;">
            <mi:TextBox runat="server" ID="TB1" FieldLabel="用户或角色代码" LabelAlign="Right" />
        </td>
    </tr>
    <tr>
        <td align="center">
            <mi:Button runat="server" ID="SubmitBtn" Text="确定拷贝" Width="80" Height="26" OnClick="checkAge()"  />
        </td>
    </tr>
</table>

</form>

<script>

    function wSubmit(sender) {
        var me = sender;

        widget1.submit(me.el, {
            command: me.command,
            commandParams: me.commandParams
        });
    }


    //提示询问框是否真的要执行代码
    function checkAge() {

        //用户或角色
        var cbModeID = $("#widget1_I_cbModeID").val();

        //用户代码或角色代码
        var code = $("#widget1_I_TB1").val();

        //单界面还是全部界面
        var OneOrAll = $("#widget1_I_cbOneOrAll").val();


        if (code === undefined || code.length == 0 || code === "") {

            Mini2.Msg.alert("信息", "用户代码不能为空！");

            return;

        }



        var text = "是否确定给 "+cbModeID+" ,代码为："+code +"  复制 " +OneOrAll +" 吗？";


        Mini2.Msg.confirm("警告", text, function () {
            
            widget1.submit('form:first', {
                action: 'GoNext'
            });

        });

 







    }

</script>   

