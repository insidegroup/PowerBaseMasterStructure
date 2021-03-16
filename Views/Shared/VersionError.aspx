<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Error</div></div>
        <div id="content">   
    <h2>The record that you are trying to modify has been modified or deleted by another user</h2>
    <%
        /*
     *  This View is shown if the record that user is trying to modify 
     *  has a different version number than the record stored in the database:
     *  
     *  Another user viewed+edited/deleted record while current user was viewing
     *  Another user deleted record while current user was viewing
     * 
     *  (SQLVersioningError is raised by SQL statement)
     * 
     */
     %>
    <p><a href="<%= Html.Encode(ViewData["ReturnURL"])%>" class="red">Re-Open Record</a></p>  
</div>
</div>
</asp:Content>