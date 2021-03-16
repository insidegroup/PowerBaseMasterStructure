<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ContactsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnits - Contacts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnits - Contacts</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="15%" class="row_header_left"><%=Html.RouteLink("Contact Type", "ListMain", new { controller = "Contact", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "ContactTypeName" }, new { title = "Contact Type" })%></th>
			        <th width="12%"><%=Html.RouteLink("LastName", "ListMain", new { controller = "Contact", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "LastName" }, new { title = "LastName" })%></th>
			        <th width="12%"><%=Html.RouteLink("FirstName", "ListMain", new { controller = "Contact", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "FirstName" }, new { title = "FirstName" })%></th>
			        <th width="10%">Phone</th>
					<th width="15%">Email</th>
					<th width="10%"><%=Html.RouteLink("Country", "ListMain", new { controller = "Contact", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CountryCode" }, new { title = "Country" })%></th>
			        <th width="15%">Responsibility</th>
					<th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <% foreach (var item in Model.Contacts) { %>
                <tr>
                    <td><%= Html.Encode(item.ContactTypeName) %></td>
					<td><%= Html.Encode(item.LastName) %></td>
					<td><%= Html.Encode(item.FirstName) %></td>
                    <td><%= Html.Encode(item.PrimaryTelephoneNumber) %></td>
					<td><%= Html.Encode(item.EmailAddress) %></td>
					<td><%= Html.Encode(item.CountryCode) %></td>
					<td><%= Html.Encode(item.ResponsibilityDescription) %></td>
                    <td align="center"><%=  Html.ActionLink("View", "View", new { controller = "Contact", action = "View", id = item.ContactId })%></td>
					<td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Edit", "Edit", new { controller = "Contact", action = "Edit", id = item.ContactId })%>
                        <%} %>
                    </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Delete", "Delete", new { controller = "Contact", action = "Delete", id = item.ContactId })%>
                        <%} %>
                    </td>
                </tr>        
                <% } %>
                <tr>
					<td colspan="10" class="row_footer">
						<div class="paging_container">
							<div class="paging_left"><% if (Model.Contacts.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { id = Model.ClientSubUnit.ClientSubUnitGuid, page = (Model.Contacts.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.Contacts.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { id = Model.ClientSubUnit.ClientSubUnitGuid, page = (Model.Contacts.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.Contacts.TotalPages > 0){ %>Page <%=Model.Contacts.PageIndex%> of <%=Model.Contacts.TotalPages%><%} %></div>
						</div>
					</td>
				</tr>
		        <tr> 
                    <td class="row_footer_blank_left" colspan="4">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>  
						<a href="javascript:window.print();" class="red" title="Print">Print</a> 
						<%= Html.ActionLink("Export", "Export", new { id = Model.ClientSubUnit.ClientSubUnitGuid }, new { @class = "red", title = "Export" })%>
                    </td>
			        <td class="row_footer_blank_right" colspan="6">
						<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Import", "ImportStep1", new { id = Model.ClientSubUnit.ClientSubUnitGuid }, new { @class = "red", title = "Import" })%>
							<%= Html.ActionLink("Create Contact", "Create", new { id = Model.ClientSubUnit.ClientSubUnitGuid }, new { @class = "red", title = "Create Contact" })%>
						<%} %>
			        </td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_clients').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$(".full-width #search_wrapper").css("height", "auto");

		//Search
		$('#ft').attr('name', 'id');
		$("#frmSearch input[name='id']").val('<%=Model.ClientSubUnit.ClientSubUnitGuid%>');
		$('#search').show();
		$('#btnSearch').click(function () {
			if ($('#filter').val() == "") {
				alert("No Search Text Entered");
				return false;
			}
			$("#frmSearch").attr("action", "/Contact.mvc/List");
			$("#frmSearch").submit();

		});

	})
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName.ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid.ToString() }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName.ToString() })%> &gt;
<%=Html.RouteLink("Client SubUnits", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid.ToString() }, new { title = "Client SubUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName.ToString(), "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid.ToString() }, new { title = Model.ClientSubUnit.ClientSubUnitName.ToString() })%> &gt;
Contacts
</asp:Content>