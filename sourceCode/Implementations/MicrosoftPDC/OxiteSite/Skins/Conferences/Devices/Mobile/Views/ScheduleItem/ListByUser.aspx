﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<OxiteViewModelItems<ScheduleItem>>" %>
<%@ Import Namespace="Oxite.Modules.CMS.Extensions"%>
<%@ Import Namespace="Oxite.Modules.Conferences.Models"%>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Models.Extensions" %>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Modules.Search.Extensions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Localize("Sessions")) %>
</asp:Content>

<asp:Content ID="searchtags" ContentPlaceHolderID="SearchTags" runat="server">
	<%=Html.SearchTag("Section", "Sessions", false)%>
	<%=Html.SearchTag("PageType", "List", false)%>
	<%=Html.SearchTag("Section", Model.Container.Name, false)%>
</asp:Content>

<asp:Content ID="additionalCSS" ContentPlaceHolderID="HeadCssFiles" runat="server">

<%--<style type="text/css">
	div#pagingContainer ul.paging li.selected
	{  
		background: expression( this.previousSibling == null ? '#ffffff' : '#df8536'); 
		color: expression( this.previousSibling == null ? '#5f779c' : '#ffffff');
		font-weight: expression( this.previousSibling == null ? 'normal' : 'bold');
	}
</style>--%>

</asp:Content>

<asp:Content ID="robotBlock" ContentPlaceHolderID="robots" runat="server"><meta name="robots" content="noindex,follow" /></asp:Content>

<asp:Content ID="Header" ContentPlaceHolderID="ContentHeader" runat="server">
    <% Html.RenderPartialFromSkin("SessionBrowser"); %>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server"> 
	<div id="sessions" class="sessions">  
		<% Html.RenderPartialFromSkin("ListByUser"); %>
	</div>
</asp:Content>


