﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<OxiteViewModelItems<PostComment>>" %>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Modules.Blogs.Models"
%><?xml version="1.0" encoding="utf-8"?>
<feed xmlns="http://www.w3.org/2005/Atom"><%
    Html.RenderPartialFromSkin("AtomFeedElements", Model); %>
</feed>
