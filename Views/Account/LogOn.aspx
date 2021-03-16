<%@ Page Language="C#" MasterPageFile="~/Views/Shared/NoAuth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    DesktopDataAdmin - Authorization Required
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Authorization Required</h2>
    <p>
    You are here because you have no authorization to view this application or the credentials you provided were incorrect.
    </p>
   <%= Html.Encode(ViewData["msg"]) %>
</asp:Content>
