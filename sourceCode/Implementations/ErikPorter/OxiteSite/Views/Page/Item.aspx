﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelItem<Oxite.Models.Page>>" %>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Models.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="post page"><% 
    Html.RenderPartial("ManagePage"); %>
    <h2 class="title"><%=Model.Item.Title %></h2>
    <%=Model.Item.Body %>
</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Item.Title) %>
</asp:Content>
<asp:Content ContentPlaceHolderID="MetaDescription" runat="server">
    <%=Html.PageDescription(Model.Item.GetBodyShort()) %>
</asp:Content>