<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ValidParamSetup.ascx.cs" Inherits="App.InfoGrid2.View.OneValid.ValidParamSetup" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<form method="post">
<mi:Viewport runat="server" ID="viewport">
    <mi:Panel runat="server" ID="Panel1" Dock="Full" Region="Center" >
    
        <table dock="full" border="0"  cellspacing="0" cellpadding="2">
            <tr>
                <th>多选按钮</th>
                <th>提示信息</th>
                <th>对应的值</th>
            </tr>
            <tr>
                <td style=" width:200px;">
                    <mi:CheckBox runat="server" ID="cbRemote"  FieldLabel="远程操作" LabelAlign="Right" LabelWidth="140" />
                </td>
                <td style=" width:300px;"><mi:TextBox runat="server" ID="tbxRemote" HideLabel="true" Placeholder="使用 ajax 方法调用 check.php 验证输入值。" /></td>
                <td style=" width:200px;"><mi:TextBox runat="server" ID="tbxRemoteValue" HideLabel="true" /></td>
                <td >&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbEmail"  FieldLabel="邮箱" LabelAlign="Right" LabelWidth="140"  />
                </td>
                <td><mi:TextBox runat="server" ID="tbxEmail" HideLabel="true" Placeholder="必须输入正确格式的电子邮件。"  /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbUrl" FieldLabel="网址" LabelAlign="Right" LabelWidth="140"  />
                </td>
                <td><mi:TextBox runat="server" ID="tbxUrl"  HideLabel="true" Placeholder="必须输入正确格式的网址。" /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbDate" FieldLabel="日期" LabelAlign="Right" LabelWidth="140"  />
                </td>
                <td><mi:TextBox runat="server" ID="tbxDate" HideLabel="true" Placeholder="必须输入正确格式的日期。日期校验 ie6 出错，慎用。"  /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbDateISO"  FieldLabel="ISO日期" LabelAlign="Right" LabelWidth="140"  />
                </td>
                <td><mi:TextBox runat="server" ID="tbxDateISO" HideLabel="true" Placeholder="必须输入正确格式的日期（ISO），例如：2009-06-23，1998/01/22。只验证格式，不验证有效性。"  /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbNumber" FieldLabel="数字" LabelAlign="Right" LabelWidth="140"  />
                </td>
                <td><mi:TextBox runat="server" ID="tbxNumber"  HideLabel="true" Placeholder="必须输入合法的数字（负数，小数）。" /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbDigits" FieldLabel="整数" LabelAlign="Right"  LabelWidth="140" />
                </td>
                <td><mi:TextBox runat="server" ID="tbxDigits"  HideLabel="true" Placeholder="必须输入整数。" /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbCreditcard" FieldLabel="信用卡号" LabelAlign="Right" LabelWidth="140"  />
                </td>
                <td><mi:TextBox runat="server" ID="tbxCreditcard"  HideLabel="true" Placeholder="必须输入合法的信用卡号。" /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbEqualTo" FieldLabel="等于" LabelAlign="Right" LabelWidth="140"  />
                </td>
                <td><mi:TextBox runat="server" ID="tbxEqualTo"  HideLabel="true" Placeholder="输入值必须和 后面输入框值 相同。" /></td>
                <td><mi:TextBox runat="server" ID="tbxEqualToValue" HideLabel="true" /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbMaxlength" FieldLabel="最大长度" LabelAlign="Right" LabelWidth="140"  /> 
                </td>
                <td><mi:TextBox runat="server" ID="tbxMaxlength"  HideLabel="true" Placeholder="输入长度最多是 5 的字符串（汉字算一个字符）。" /></td>
                <td><mi:NumberBox runat="server" ID="tbxMaxlengthValue" HideLabel="true"  /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbMinlength" FieldLabel="最小长度" LabelAlign="Right" LabelWidth="140"  />
                </td>
                <td><mi:TextBox runat="server" ID="tbxMinlength"  HideLabel="true" Placeholder="输入长度最小是 10 的字符串（汉字算一个字符）。" /></td>
                <td><mi:NumberBox runat="server" ID="tbxMinlengthValue" HideLabel="true" /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbRangelength" FieldLabel="长度两个字符之间" LabelAlign="Right" LabelWidth="140"  />
                </td>
                <td><mi:TextBox runat="server" ID="tbxRangelength"  HideLabel="true" Placeholder="输入长度必须介于 5 和 10 之间的字符串（汉字算一个字符）。" /></td>
                <td><mi:NumRangeBox runat="server" ID="tbxRangelengthMaxMin" FieldLabel="长度" LabelAlign="Right" LabelWidth="40"  /></td>
      
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbRange" FieldLabel="值在两个字符之间"  LabelAlign="Right" LabelWidth="140"  />
                </td>
                <td><mi:TextBox runat="server" ID="tbxRange"  HideLabel="true" Placeholder="输入值必须介于 5 和 10 之间。" /></td>
                <td><mi:NumRangeBox runat="server" ID="tbxRangeMaxMin" FieldLabel="大小" LabelAlign="Right" Width="100%"  LabelWidth="40"  /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbMax" FieldLabel="最大值" LabelAlign="Right" LabelWidth="140"  />
                </td>
                <td><mi:TextBox runat="server" ID="tbxMax"  HideLabel="true" Placeholder="输入值不能大于 5。" /></td>
                <td><mi:NumberBox runat="server" ID="tbxMaxValue" HideLabel="true" /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <mi:CheckBox runat="server" ID="cbMin" FieldLabel="最小值" LabelAlign="Right" LabelWidth="140"  />
                </td>
                <td><mi:TextBox runat="server" ID="tbxMin"  HideLabel="true" Placeholder="输入值不能小于 10。" /></td>
                <td><mi:NumberBox runat="server" ID="tbxMinValue" HideLabel="true" /></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
            <td></td>
            <td></td>
            <td></td>
            </tr>
        </table>
    </mi:Panel>
     <mi:WindowFooter ID="WindowFooter1" runat="server">
        <mi:Button runat="server" ID="SubmitBtn" Width="120" Height="26" Command="btnSave" Text="完成"  Dock="Center" />
        <mi:Button runat="server" ID="Button2" Width="80" Height="26" OnClick="ownerWindow.close()" Text="取消" Dock="Center" />
    </mi:WindowFooter>
    </mi:Viewport>
</form>


