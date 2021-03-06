﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../../../../../../Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<Post>>" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Models.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
            <div class="sections">
                <div class="primary">
                    <%=Html.PageState((IPageOfList<Post>)Model.List, (k, v) => Model.Localize(k, v))%><% 
                    Html.RenderPartialFromSkin("PostListWithFiles"); %>
                </div>
            </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="Scripts" runat="server"><%
    Html.RenderScriptTag("base.js");
    Html.RenderScriptTag("list.js");%>
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadCustom" runat="server"><%
    Html.RenderFeedDiscoveryRss(string.Format("{0} (RSS)", Model.Site.DisplayName), Url.Posts("RSS"));
    Html.RenderFeedDiscoveryAtom(string.Format("{0} (ATOM)", Model.Site.DisplayName), Url.Posts("ATOM"));
    Html.RenderFeedDiscoveryRss("All Comments (RSS)", Url.Comments("RSS"));
    Html.RenderFeedDiscoveryAtom("All Comments (ATOM)", Url.Comments("ATOM"));
    Html.RenderRsd();
    Html.RenderLiveWriterManifest(); %>
</asp:Content>