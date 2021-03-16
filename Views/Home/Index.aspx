<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.HomePageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Home
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <h2><%= Html.Encode(ViewData["Message"]) %></h2>

<% Html.EnableClientValidation(); %>

<% using (Html.BeginForm()) {%>
    <%=Html.AntiForgeryToken() %>
    <%=Html.ValidationSummary(true) %>
    <%=Model.ConnectionSelectList.Message %><br /><br />
    <%=Html.DropDownList("DBList", Model.ConnectionSelectList.SelectList, "Please Select...", new { style = "width:300px;" })%>
    <br /><input type="submit" value="Submit" title="Submit" class="red" id="databaseBtn" name="databaseBtn"/><br />
    <%
    //We only show dropdown if multiple items available
    if (Model.UserProfileList.Count > 1){
        %>
        <br /><br /><br /><br />Your account is associated with multiple active Profile IDs. Please select one.<br /><br />
        <%
        int SelectListSize = Model.UserProfileList.Count < 6 ? Model.UserProfileList.Count : 6;
    %>
        <%=Html.DropDownListFor(model => model.UserProfileIdentifier, Model.MappingQualityCodes, new { size = SelectListSize, style="width:300px;" })%>
        <br /><input type="submit" value="Submit" title="Submit" class="red" id="userProfileBtn" name="userProfileBtn"/><br />
    <%
    }
    %>
            

 
    
 
<% } %>
</asp:Content>
