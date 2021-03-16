<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Error</div></div>
        <div id="content">   
        <h2>General Error</h2>
        <%
        /*
         *  This View is shown for an unexpected error or an error where we want to give no information
         *  
         * When trapping errors we check for expected errors and perform appropriate action
         * If the error is not exppected we go here
         * 
         * Possible reasons to be here:
         * User alters ID in URL of Edit Form (item exists but we dont want to tell user the ID is valid)
         * User permissions are revoked in the middle of operation
         * UpdateModel Fails validation against database
         * Update fails but not with versioning error
         * Delete fails but not with versioning error
         */
         %> 
         <%=Html.Encode(ViewData["Message"]) %>
    <p><a href="javascript:history.back();" class="red" title="Back">Back</a></p>  
    </div>
</div>
</asp:Content>
