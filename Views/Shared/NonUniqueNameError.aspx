<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Error</div></div>
        <div id="content">   
    <h2>Another user has created an item with this name, please go back and try again</h2>
    <%
    /*
     *  This View is shown if the record that user is trying to modify/create 
     *  has the same name as another item:
     *  
     *  (NonUniqueName is raised by SQL Insert statement)
     * 
     */
     %>
    <p><a href="javascript:history.back()" class="red" title="Back">Back</a></p>  
</div>
</div>
</asp:Content>