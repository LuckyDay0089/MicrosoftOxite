﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<OxiteViewModelItems<Oxite.Modules.Conferences.Models.Speaker>>" %>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Modules.Conferences.Models" %>
<Speakers xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <% foreach (var speaker in Model.Items) { %>
        <% Html.RenderPartialFromSkin("ItemXml", new OxiteViewModelItemItems<Speaker,ScheduleItem>(speaker, speaker.ScheduleItems)); %>
    <% } %>
</Speakers>
