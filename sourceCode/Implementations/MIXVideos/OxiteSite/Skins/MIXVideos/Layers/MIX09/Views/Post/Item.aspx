﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../../../../../../Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelItem<Post>>" %>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Models.Extensions" %>
<%@ Import Namespace="MIXVideos.Oxite.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server"><%

string postClassName = "post";
var tagNames = from t in Model.Item.Tags select t.Name.CleanCssClassName();
if (tagNames.Count() > 0)
    postClassName = string.Format("{0} {1}", postClassName, string.Join(" ", tagNames.ToArray()));

bool showAdvertisement = Model.Item.Tags.Count(t => t.Name.Contains("Breakout")) > 0;
                                                                                                                                           
 %>
<div class="sections">
    <div class="primary">
        <div class="<%=postClassName %>">
            <% Html.RenderPartialFromSkin("ManagePost"); %>
            <h2 class="title"><%=Model.Item.Title.CleanText() %></h2>
            <div class="posted"><%=Html.Published() %></div>
            <% Html.RenderPlayer("MixVideosPlayer", showAdvertisement ? Url.CssPath("/images/ie8_218x73.png", ViewContext) : ""); %><%
            if (showAdvertisement) { %>
            <div class="sponsor"><%=string.Format(Model.Localize("SessionSponsoredByFormat", "This session recording was made possible by {0}"), Html.Link("Internet Explorer 8", "http://www.visitmix.com/ie8"))%></div><%
            } %>
            <div class="content"><%=Model.Item.Body %></div>
            <ul class="more">
                <li><%=Model.Localize("Tags") %>: <%
                if (Model.Item.Tags.Count > 0)
                {
                    %><%=Html.UnorderedList(Model.Item.Tags.OrderBy(t => t.DisplayName), (t, i) => Html.Link(t.GetDisplayName().CleanText(), Url.Posts(t), new { rel = "tag" }), "tags") %><%
                } else { %>none<% } %></li><%
                if (Model.Item.Files.Count > 0)
                { %>
                <li><% Html.RenderPartialFromSkin("Download", new OxiteModelPartial<Post>(Model, Model.Item)); %></li><%
                } %>
                <li><% Html.RenderPartialFromSkin("Share", new OxiteModelPartial<Post>(Model, Model.Item), ViewData); %></li>
            </ul><%
                if(!(Model.CommentingDisabled && Model.Item.Comments.Count < 1))
                {
                    Html.RenderPartialFromSkin("Comments");
                }

                if (Model.CommentingDisabled)
                {
                    %><div class="message"><%=Model.Localize("CommentingDisabled", "Commenting is disabled for this post.")%></div><%
                } %>
        </div>
    </div>
    <div class="secondary"><%
        Html.RenderPartialFromSkin("SideBar"); %>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Site.HasMultipleAreas ? Model.Container.GetDisplayName() : null, Model.Item.GetDisplayName()) %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MetaDescription" runat="server">
    <%=Html.PageDescription(Model.Item.GetBodyShort()) %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptVariablesPre" runat="server">
    <script type="text/javascript">
        <% Html.RenderScriptVariable("computeHashPath", Url.ComputeHash()); %>
    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Scripts" runat="server"><%
    Html.RenderScriptTag("base.js"); %>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadCustom" runat="server"><%
                                                                                Html.RenderSearchTags(Model.Item);
                                                                                Html.RenderFeedDiscoveryRss("Post Comments (RSS)", Url.Comments(Model.Item, "RSS"));
    Html.RenderFeedDiscoveryAtom("Post Comments (ATOM)", Url.Comments(Model.Item, "ATOM"));
    if (Model.Site.HasMultipleAreas)
    {
        Html.RenderFeedDiscoveryRss(string.Format("{0} Posts (RSS)", Model.Container.GetDisplayName()), Url.Container(Model.Container, "RSS"));
        Html.RenderFeedDiscoveryAtom(string.Format("{0} Posts (ATOM)", Model.Container.GetDisplayName()), Url.Container(Model.Container, "ATOM"));
        Html.RenderFeedDiscoveryRss(string.Format("All {0} Comments (RSS)", Model.Container.GetDisplayName()), Url.ContainerComments(Model.Container, "RSS"));
        Html.RenderFeedDiscoveryAtom(string.Format("All {0} Comments (ATOM)", Model.Container.GetDisplayName()), Url.ContainerComments(Model.Container, "ATOM"));
    }
    Html.RenderFeedDiscoveryRss(string.Format("{0} (RSS)", Model.Site.DisplayName), Url.Posts("RSS"));
    Html.RenderFeedDiscoveryAtom(string.Format("{0} (ATOM)", Model.Site.DisplayName), Url.Posts("ATOM"));
    Response.Write(Html.PingbackDiscovery(Model.Item)); %>
</asp:Content>
