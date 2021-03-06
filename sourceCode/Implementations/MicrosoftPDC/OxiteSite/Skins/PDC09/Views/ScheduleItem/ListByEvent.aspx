﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<OxiteViewModelItems<ScheduleItem>>" %>
<%@ Import Namespace="Oxite.Modules.CMS.Extensions"%>
<%@ Import Namespace="Oxite.Modules.Conferences.Models"%>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Models.Extensions" %>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Modules.Search.Extensions" %>
<asp:Content ID="searchtags" ContentPlaceHolderID="SearchTags" runat="server"><%=Html.SearchTag("Section", "Sessions", false)%><%=Html.SearchTag("PageType", "List", false)%>
<%=Html.SearchTag("Section", Model.Container.Name, false)%></asp:Content>
<asp:Content ID="robotBlock" ContentPlaceHolderID="robots" runat="server"><meta name="robots" content="noindex,follow" /></asp:Content>
<asp:Content ContentPlaceHolderID="ContentHeader" runat="server">
    <h1><%=Model.Localize("Sessions") %></h1>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <%=Html.Content("SessionsDescription") %>
    <% if (Model.User.IsInRole("Admin")) { %><a href="<%= Url.RouteUrl("SummaryReport") %>"><%= Model.Localize("See Session Stats") %></a>
    <%}%>
    <div id="browser">
	    <div id="tabs">
		    <a href="/Sessions" class="tab" id="sessionstab">Sessions</a>
		    <a href="/Schedule" class="tab" id="scheduletab">Schedule</a>
		    <a href="/Videos" class="tab">Videos</a>
		    <!--<% Html.RenderPartialFromSkin("ScheduleShare"); %>-->
	    </div>    
        <% Html.RenderPartialFromSkin("SessionBrowser"); %>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Localize("Sessions")) %>
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadCssFiles" runat="server"><%
    Html.RenderCssFile("jquery.autocomplete.css"); %>
</asp:Content>
<asp:Content ContentPlaceHolderID="Scripts" runat="server"><%
    Html.RenderScriptTag("base.js");
    Html.RenderScriptTag("Sessions.js?ver=2");
    Html.RenderScriptTag("jquery.autocomplete.js");
    Html.RenderScriptTag("sessionbrowser.4.js"); %>
    <script type="text/javascript">/*<![CDATA[*/pdc.sessionBrowser.which = "sessions";//]]></script>
    <script type="text/javascript">/*<![CDATA[*/
        pdc.sessionBrowser.sessions.speakers = ("").split("|");
        pdc.sessionBrowser.sessions.tags = ("").split("|");
    //]]></script>
</asp:Content>
