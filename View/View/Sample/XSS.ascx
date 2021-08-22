<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XSS.ascx.cs" Inherits="App.InfoGrid2.View.Sample.XSS" %>
<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<!--引入CSS-->
<link rel="stylesheet" type="text/css" href="/Core/Scripts/webuploader-0.1.5/webuploader.css" />

<!--引入JS-->
<script src="/Core/Scripts/webuploader-0.1.5/webuploader.js"></script>

<script src="/Core/Scripts/UEditor/1.2.6.0/ueditor.config.js"></script>
<script src="/Core/Scripts/UEditor/1.2.6.0/ueditor.all.js"></script>



<form action="" id="form1" method="post">
    
    <mi:Viewport runat="server" ID="viewport1">


        <mi:WindowFooter runat="server" ID="footer1" Align="Right" ItemClass=""  >
            <mi:Button runat="server" ID="btnX" Text="确定" Width="80"   />
            <mi:Button runat="server" ID="Button1" Text="取消" Width="80"  />
        </mi:WindowFooter>


        <mi:Panel runat="server" ID="mainPanel" Region="Center" Padding="6">

        <fieldset style=" width:100% " class="mi-fieldset mi-fieldset-with-title mi-fieldset-with-legend mi-fieldset-default">

            <legend class="mi-fieldset-header mi-fieldset-header-default" id="fieldset-1014-legend">
                <span id="fieldset-1014-legend-outerCt" style="display: table;" role="presentation">
                    <div id="fieldset-1014-legend-innerCt" style="height: 100%; vertical-align: top; display: table-cell;" class="" role="presentation">
                        <div class="mi-tool mi-tool-default" style="width: 15px; height: 15px;" id="fieldset-1014-legendToggle">
                            <img id="fieldset-1014-legendToggle-toolEl" src="data:image/gif;base64,R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==" class="mi-tool-img mi-tool-toggle" role="presentation"></div>
                        <div class="mi-component mi-fieldset-header-text mi-fieldset-header-text-collapsible mi-component-default" id="fieldset-1014-legendTitle">联系信息</div>


                    
                    </div>
                </span>
            </legend>

            <div class="mi-fieldset-body ">

                <mi:FormLayout runat="server" ID="formLayout1" ItemWidth="300" ItemLabelAlign="Right"  Dock="None" AutoSize="true" Layout="HBox" FlowDirection="LeftToRight">

                    <mi:ComboBox runat="server" ID="comboBox2"  FieldLabel="下拉框" Value="破解" Width="300" MinWidth="600px" LabelAlign="Right" TriggerMode="None">
                        <mi:ListItem Value="破解" />
                        <mi:ListItem Value="呵呵" />
                        <mi:ListItem Value="呵呵323" />
                    </mi:ComboBox>

                    <mi:CheckBox runat="server" ID="checkBox1" LabelAlign="Right"  />

                    <mi:CheckboxGroup runat="server" ID="checkboxGroup1" LabelAlign="Right" DefaultValue="呵呵323" MinWidth="600px" >
                        <mi:ListItem Value="破解" />
                        <mi:ListItem Value="呵呵" />
                        <mi:ListItem Value="呵呵323" />
                    </mi:CheckboxGroup>

                    <mi:RadioGroup runat="server" ID="radioGroup1" LabelAlign="Right" DefaultValue="呵呵" MinWidth="604px" >
                        <mi:ListItem Value="破解" />
                        <mi:ListItem Value="呵呵" />
                        <mi:ListItem Value="呵呵323" />
                    </mi:RadioGroup>

                    <mi:TriggerBox runat="server" ID="targgerBox1" LabelAlign="Right" Width="300" ButtonType="More" OnButtonClick="alert('ddd')"  />
                    
                    <mi:TriggerBox runat="server" ID="TriggerBox1" LabelAlign="Right" Width="300" ButtonType="More" OnButtonClick="alert('ddd')"   />

                    
                    <mi:TextBox runat="server" ID="ttt1"  />
                    <mi:TextBox runat="server" ID="TextBox1"   />
                    <mi:TextBox runat="server" ID="TextBox2"  Placeholder="34234" />

                    <mi:NumberBox runat="server" ID="num1"  />

                    <mi:TextBox runat="server" ID="TextBox4"  TabStop="false"  />
                    <mi:TextBox runat="server" ID="TextBox5"  />
                    <mi:TextBox runat="server" ID="TextBox6"  />
                    <mi:TextBox runat="server" ID="TextBox7" FieldLabel="中华人民共和国"  />
                    <mi:TextBox runat="server" ID="TextBox8"   />
                    <mi:TextBox runat="server" ID="TextBox9"   />
                    <mi:TextBox runat="server" ID="TextBox10"  />
                    <mi:TextBox runat="server" ID="TextBox11"  />
                    <mi:TextBox runat="server" ID="TextBox12"  />
                    <mi:TextBox runat="server" ID="TextBox13"   />

                </mi:FormLayout>
            </div>

        </fieldset>


        </mi:Panel>


    </mi:Viewport>



</form>

<% if (false)
    { %>
<script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js"></script>
<% } %>
<script type="text/javascript">

    var thumbnailWidth = 256;
    var thumbnailHeight = 256;


    $(document).ready(function () {

        var el = $('#widget1_I_FileUpload2');

        var progressEl = $(el).find('.progress');

        progValue(progressEl);

    });

    var m_V = 0;

    function progValue(progressEl) {

        m_V += 1;

        progressEl.css('width', m_V + '%');

        if (m_V == 100) {
            return;
        }

        setTimeout(function () {

            progValue(progressEl);

        }, 100);
    }


</script>
