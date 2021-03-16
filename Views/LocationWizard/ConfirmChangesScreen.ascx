<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.Models.WizardMessages>" %>
<div style="float:right;">         
    <span id="confirmChangesBackButton"><small><< Back</small></span>&nbsp;
    <span id="confirmChangesNextButton"><small>Finish >> </small></span>
</div>
<input type="hidden" id="changesMade" value="<%=Model.Messages.Count%>" />
<div id="changesSummary">
<h3>Here is a list of changes you are making to this Location, please confirm your changes or cancel:</h3>
<% foreach (var item in Model.Messages) { %> 
               <h4> <%: item.message %></h4>
<% } %>
<br /><br />
<span id="cancelAllChanges" style="float:right; right:19px;"><small>Cancel all changes</small></span>
</div>