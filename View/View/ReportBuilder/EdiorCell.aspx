<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EdiorCell.aspx.cs" Inherits="App.InfoGrid2.View.ReportBuilder.EdiorCell" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>
<%@ Import Namespace="HWQ.Entity.Xml" %>
<%@ Import Namespace="App.InfoGrid2.Model" %>
<!doctype html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>jQuery UI Droppable - Shopping Cart Demo</title>
    <% if (false)
       { %>    <script src="/Core/Scripts/jquery/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini2/dev/lang/log.js" type="text/javascript"></script>

    <script src="/Core/Scripts/Mini2/Mini2.min.js" type="text/javascript"></script>


    <script src="/Core/Scripts/jtemplates/jquery-jtemplates.js" type="text/javascript"></script>
    <script src="/Core/Scripts/MiniHtml.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/TableTemplate.js" type="text/javascript"></script>
    <script src="/Core/Scripts/jquery.form.js" type="text/javascript"></script>
    <% } %>
    

    <script src="/Core/Scripts/jquery/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="/Core/Scripts/Mini/_Default.js"></script>

    <link href="/Core/Scripts/jquery.ui/themes/ui-lightness/jquery-ui.css" rel="stylesheet" type="text/css" />

    
    <script src="/Core/Scripts/ui-lightness/jquery-ui-1.9.2.custom.js" type="text/javascript"></script>
    <link href="/Core/Scripts/ui-lightness/redmond/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <link href="/Core/Scripts/ui-lightness/jquery-ui-1.9.0.custom.min.css" rel="stylesheet" type="text/css" />
    
    <link href="<%= JsHome %>/Core/Scripts/Mini2/Themes/theme-globel.css" rel="stylesheet" type="text/css" />
    <link href="<%= JsHome %>/Core/Scripts/Mini2/Themes/theme-window.css" rel="stylesheet" type="text/css" />
    <link href="<%= JsHome %>/Core/Scripts/Mini2/Themes/Win8/theme-win8.css" rel="stylesheet" type="text/css" />
    
    <% if (true)
       {

           string mapPath = MapPath("~/Core/Scripts/Mini2/Mini2.join.ini");
           string[] jsLines = System.IO.File.ReadAllLines(mapPath);
           
           foreach (string item in jsLines)
           {
                if(string.IsNullOrEmpty(item) || item.Trim().StartsWith("--")){
                    continue;   
                }

                Response.Write(string.Format("<script src='{0}/Core/Scripts/Mini2/dev/{1}' ></script>", JsHome, item));
           }
                
            //Response.WriteFile("~/Core/Scripts/Mini2/Mini2.script.txt");
       } %>
    <script src="/App/InfoGrid2/ReportBuilder/CrossReport.js" type="text/javascript"></script>
    <style>
        *
        {
            font-size: 12px;
        }
        
        h1
        {
            padding: .2em;
            margin: 0;
        }
                
       
        
        .products
        {
            float: left;
            width: 500px;
            margin-right: 2em;
            list-style: none;
            
        }


        .cross-cols
        {
            width: 300px;
            
        }
        
        .cross-cols ol
        {
            margin: 0;
            padding: 2px 4px 2px 20px;
            height: 150px; 
            overflow:auto;
        }
    </style>
    
</head>
<body style=" font-size:22px;">
    <form method="post" id="form1" action="/Error.aspx">
    <input type="hidden" id="hdJson" name="hdJson" />
    <input type="hidden" id="hfID" name="hfIDD" value="<%=this.m_Id %>" />
    </form>

</body>
</html>
<script>




    ///这是一个js类
    var m_builder;

    //获取 json 数据
    function clinet_GetItemJson() {

        var data = $(m_builder.curEditCol).data("EX");

        return data;
    }


    function client_SetItemJson(json) {

        $(m_builder.curEditCol).data("EX", json);

        console.log(json); /////////////////////////////////////////////////////////////////////


    }



    $(document).ready(function () {

        m_builder = Mini2.create('Mini2.report.CrossReport', {

            tableName: '<%=this.m_talbeName %>',
            init_json: '<%=this.m_json%>',
            tableID:'<%=this.m_tableID%>',
            dbFields: '<%= m_dbFields %>',
            EditTableColUrl:'<%= this.m_editColProUrl%>'
            });

            m_builder.init();
        });

    


    </script>

<script runat="server">

    public string JsHome = string.Empty;
</script>