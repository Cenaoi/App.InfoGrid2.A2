<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CmsView.ascx.cs" Inherits="App.InfoGrid2.View.CMS.CmsView" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<link href="/Core/Scripts/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />

<div class="main-container container">

    <div class="page-content">
                
        <div class="text-center">
            <h3><%= T.C_TITLE %></h3>
        </div>

        <div class="panel panel-default">

            <div class="panel-body">
                <%= T.C_CONTENT.Replace("\n","<br />") %>
            </div>

        </div>

    </div>

</div>




