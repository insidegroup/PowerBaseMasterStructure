<%@ Page Language="C#" MasterPageFile="~/Views/Shared/NoAuth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    DesktopDataAdmin - Logged Out
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Logged Out</h2>
    <p>
        You are now logged out of CWT Desktop Database
   </p>
</asp:Content>
