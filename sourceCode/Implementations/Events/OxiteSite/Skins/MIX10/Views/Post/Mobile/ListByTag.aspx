﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<OxiteViewModelItems<Post>>" MasterPageFile="../Shared/Site.Master" %>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Models.Extensions" %>
<%@ Import Namespace="Oxite.Modules.Blogs.Extensions" %>
<%@ Import Namespace="Oxite.Modules.Blogs.Models" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Localize("TagsTitle","Tags"), Model.Container.GetDisplayName()) %>
</asp:Content>

<asp:Content ID="robotBlock" ContentPlaceHolderID="robots" runat="server"><meta name="robots" content="noindex,follow" /></asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadCustom" runat="server">
<%--<%
    Html.RenderFeedDiscoveryRss(string.Format("{0} Posts (RSS)", Model.Container.Name), Url.Container(Model.Container, "RSS"));
    Html.RenderFeedDiscoveryAtom(string.Format("{0} Posts (ATOM)", Model.Container.Name), Url.Container(Model.Container, "ATOM"));
    Html.RenderFeedDiscoveryRss(string.Format("All {0} Comments (RSS)", Model.Container.Name), Url.ContainerComments(Model.Container, "RSS"));
    Html.RenderFeedDiscoveryAtom(string.Format("All {0} Comments (ATOM)", Model.Container.Name), Url.ContainerComments(Model.Container, "ATOM"));
    Html.RenderFeedDiscoveryRss(string.Format("{0} (RSS)", Model.Site.DisplayName), Url.Posts("RSS"));
    Html.RenderFeedDiscoveryAtom(string.Format("{0} (ATOM)", Model.Site.DisplayName), Url.Posts("ATOM"));
%>--%>
</asp:Content>

<asp:Content ID="searchtags" ContentPlaceHolderID="SearchTags" runat="server">
	<%=Html.SearchTag("Section", "Blog", false)%>
	<%=Html.SearchTag("PageType", "List", false)%>
	<%=Html.SearchTag("Section", Model.Container.Name, false)%>
</asp:Content>


<asp:Content ID="subNav" ContentPlaceHolderID="ContentHeader" runat="server">

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
     <% if (Model.Container.GetDisplayName() != null) {%>
     Posts tagged with <strong><%= Model.Container.GetDisplayName() %></strong>
     <% } %>
     <% Html.RenderPartialFromSkin("PostListMedium"); %>
    
    <div id="blogPagerContainer">
		<%= Html.PageState((IPageOfItems<Post>)Model.Items, (k, v) => Model.Localize(k, v))%>
		<%=Html.PostListByTagPager((IPageOfItems<Post>)Model.Items,(k,v) => Model.Localize(k,v), Model.Container.Name) %>
	</div>
</asp:Content>