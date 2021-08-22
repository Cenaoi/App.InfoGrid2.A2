<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LogActText.ascx.cs" Inherits="App.InfoGrid2.View.LogAct.LogActText" %>

<%@ Register Assembly="EasyClick.Web.Mini2" Namespace="EasyClick.Web.Mini2" TagPrefix="mi" %>

<link href="/res/js_plugin/highlight/default.min.css" rel="stylesheet" />
<script src="/res/js_plugin/highlight/highlight.min.js"></script>
<form>
    <pre>
        <code class="xml"><%=m_text %></code>
    </pre>


</form>
<script>hljs.initHighlightingOnLoad();</script>

