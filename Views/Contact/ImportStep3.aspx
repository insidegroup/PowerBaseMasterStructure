<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitContactImportStep3VM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Client SubUnits - Contacts
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnits - Contacts</div></div>
    <div id="content">
     <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Contacts Import Summary</th> 
		    </tr>  
            <tr>
                <td valign="top" colspan="3">
					<% if (Model != null && Model.ReturnMessages != null && Model.ReturnMessages.Count > 0){ %>
						<%foreach (string msg in Model.ReturnMessages){ 
							Response.Write(msg); %> <br/><br/>
						<%}%>
					<% } else { %>
						<p style="color:red;">The import of the selected data file has failed.<br />Please try again or contact your administrator</p>
					<% } %>
                </td>
            </tr>  
            <tr>
                <td width="25%" class="row_footer_left"></td>
                <td width="50%" class="row_footer_centre"></td>
                <td width="25%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="<%= Url.Action("List","Contact", new {id= Model.ClientSubUnitGuid})%>" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right">
                </td>
            </tr>
        </table>

        <%= Html.HiddenFor(model => model.ClientSubUnitGuid) %>

         <% } %>

    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName.ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid.ToString() }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName.ToString() })%> &gt;
<%=Html.RouteLink("Client SubUnits", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid.ToString() }, new { title = "Client SubUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName.ToString(), "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid.ToString() }, new { title = Model.ClientSubUnit.ClientSubUnitName.ToString() })%> &gt;
<%=Html.RouteLink("Contacts", "Main", new { controller = "Contact", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid.ToString() }, new { title = "Contacts" })%> &gt;
Import Summary
</asp:Content>