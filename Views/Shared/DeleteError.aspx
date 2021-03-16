<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Error</div></div>
        <div id="content">   
    <h2>The record that you are trying to delete cannot be removed because it is referenced by other records</h2>
    <%
        /*
     *  This View is shown if the record that user is trying to delete cannot be deleted due to database constraints:
      *  15 June 2011 - DM - Used for ContactType
     * 
     * 
     *  (SQLDeleteError is raised by SQL statement)
     * 
     * 
     */
     %>
    <p><a href="<%= Html.Encode(ViewData["ReturnURL"])%>" class="red">Re-Open Record</a></p>  
</div>
</div>
</asp:Content>