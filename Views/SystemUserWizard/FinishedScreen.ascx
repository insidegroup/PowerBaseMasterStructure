<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.Models.WizardMessages>" %>



<h3>The following updates were made:</h3>
 <% foreach (var item in Model.Messages) { %> 
 <% 

 %>
               <h4><%: item.message %></h4>
 
    
    <% } %>
     <span id="BackToStart"><small>SystemUser Wizard Home</small></span>