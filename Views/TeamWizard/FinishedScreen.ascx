<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.Models.WizardMessages>" %>



<h3>The following updates were made:</h3>
 <% foreach (var item in Model.Messages) { %> 
 <% 
     var color="red";
        if (item.success == true){
            color = "white";  
        }%>
               <h4><%: item.message %></h4>
 
    
    <% } %>
     <span id="BackToStart"><small>Team Wizard Home</small></span>