<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="App.InfoGrid2.JF.View.Footer" %>


  <nav class="bar bar-tab">
            <a class="tab-item external  <%= CanAction("home") %>" href="/JF/View/Home.aspx">
                <span class="icon icon-home"></span>
                <span class="tab-label">首页</span>
            </a>
              <a class="tab-item external  <%= CanAction("order") %>" href="/JF/View/Order/OrderContent.aspx">
                <span class="icon icon-menu"></span>
                <span class="tab-label">订单</span>
            </a>
            <a class="tab-item external  <%= CanAction("cart") %>" href="/JF/View/Prod/CartContent.aspx">
                <span class="icon icon-cart"></span>
                <span class="tab-label">购物车</span>
            </a>
            <a class="tab-item external  <%= CanAction("me") %>" href="/JF/View/User/Home.aspx">
                <span class="icon icon-me"></span>
                <span class="tab-label">我</span>
            </a>
        </nav>

<script runat="server">

    string m_ActionName = "home";

    /// <summary>
    /// 当前焦点的按钮. home 
    /// </summary>
    public string ActionName
    {
        get { return m_ActionName; }
        set { m_ActionName = value; }
    }

    public string CanAction(string itemName)
    {
        bool isAction =  (itemName == this.ActionName);

        return isAction ? "active" : "";
    }
</script>
