<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Error</div></div>
        <div id="content">   
        <h2>The record that you are trying to access does not exist</h2>
        <p>
        <%
        /*
         *  This View is shown if the record does not exist :
         *  
         *  Record may have been deleted by another user (GET + POST)
         *  User may have altered URL to use id of non-existent record (GET)
         * 
         */
         
        if (ViewData["ActionMethod"].ToString() == "ViewGet")
        {
            Response.Write("You have opened an invalid URL or the item has been deleted by another user");
        }
        if (ViewData["ActionMethod"].ToString() == "EditGet")
        {
            Response.Write("You have opened an invalid URL or the item has been deleted by another user");
        }
        if (ViewData["ActionMethod"].ToString() == "DeleteGet")
        {
            Response.Write("You have opened an invalid URL or the item has been deleted by another user");
        }
        
        if (ViewData["ActionMethod"].ToString() == "EditPost")
        {
            Response.Write("The item may have been deleted by another user");
        }
        if (ViewData["ActionMethod"].ToString() == "DeletePost")
        {
            Response.Write("The item may have been deleted by another user");
        }
     %>
     <br /><%=ViewData["ActionMethod"] %>
    </p>  
            </div>
</div>
</asp:Content>
