<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteFooter.ascx.cs" Inherits="App.InfoGrid2.Mobile_V1.SiteFooter" %>


  <nav class="bar bar-tab">
            <a class="tab-item external  <%= CanAction("home") %>" href="/Mobile_V1/EcHome.aspx">
                <span class="icon icon-home"></span>
                <span class="tab-label">首页</span>
            </a>
              <a class="tab-item external  <%= CanAction("classroom") %>" href="/Mobile_V1/EcHome.aspx">
                <span class="icon icon-clock"></span>
                <span class="tab-label">课堂</span>
            </a>
              <a class="tab-item external  <%= CanAction("find") %>" href="/Mobile_V1/EcHome.aspx">
                <span class="icon icon-gift"></span>
                <span class="tab-label">发现</span>
            </a>
            <a class="tab-item external  <%= CanAction("cart") %>" href="/Mobile_V1/Order/EcCart.aspx">
                <span class="icon icon-cart"></span>
                <span class="tab-label">购物车</span>
            </a>
            <a class="tab-item external  <%= CanAction("me") %>" href="/Mobile_V1/Person/EcCenter.aspx">
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