<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitCDRItemsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Sub Units - CDR Link - Validate Values</div></div>
    <div id="content">
		<% Html.EnableClientValidation(); %>
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr> 
                <th width="70%" class="row_header_left"><%= Html.RouteLink("Validate Value", "ListMain", new { page = 1, sortField = "RelatedToValue", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id= Model.ClientSubUnitClientDefinedReference.ClientSubUnitClientDefinedReferenceId, csu = Model.ClientSubUnit.ClientSubUnitGuid, RelatedToDisplayName = Request.QueryString["RelatedToDisplayName"] })%></th> 
                <th width="15%">&nbsp;</th> 
			    <th width="15%" class="row_header_right">&nbsp;</th> 	
		    </tr> 
            <% foreach (var item in Model.ClientSubUnitCDRItems){ %>
				<tr>
					<td><%= Html.Encode(item.RelatedToValue)%></td>
					<td>
						<%if (Model.HasWriteAccess){ %>
							<%=  Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.ClientSubUnitClientDefinedReferenceItemId, csu = Model.ClientSubUnit.ClientSubUnitGuid}, new { title = "Edit" })%>
						<%} %>
					</td>
					<td>
						<%if (Model.HasWriteAccess){ %>
							<%=  Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.ClientSubUnitClientDefinedReferenceItemId, csu = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Delete" })%>
						<%} %>
					</td>     
				</tr>        
            <% } %>
			<tr>
				<td colspan="3" class="row_footer">
					<div class="paging_container">
						<div class="paging_left">
							<% if (Model.ClientSubUnitCDRItems.HasPreviousPage)
								{ %>
						<%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", id = Model.ClientSubUnitClientDefinedReference.ClientSubUnitClientDefinedReferenceId, csu = Model.ClientSubUnit.ClientSubUnitGuid, RelatedToDisplayName = Request.QueryString["RelatedToDisplayName"], page = (Model.ClientSubUnitCDRItems.PageIndex - 1), sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
						<%}%>
						</div>
						<div class="paging_right">
							<% if (Model.ClientSubUnitCDRItems.HasNextPage)
							{ %>
						<%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "List", id = Model.ClientSubUnitClientDefinedReference.ClientSubUnitClientDefinedReferenceId, csu = Model.ClientSubUnit.ClientSubUnitGuid, RelatedToDisplayName = Request.QueryString["RelatedToDisplayName"], page = (Model.ClientSubUnitCDRItems.PageIndex + 1), sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
						<%}%> 
						</div>
						<div class="paging_centre">
							<%if (Model.ClientSubUnitCDRItems.TotalPages > 0) { %> Page <%=Model.ClientSubUnitCDRItems.PageIndex%> of <%=Model.ClientSubUnitCDRItems.TotalPages%><%} %>
						</div>
					</div>
				</td>
			</tr>
		    <tr> 
                <td class="row_footer_blank_left" colspan="1">
					<a href="javascript:history.back();" class="red" title="Back">Back</a> 
					<a href="javascript:window.print();" class="red" title="Print">Print</a>
                </td>
			    <td class="row_footer_blank_right" colspan="2"><%= Html.ActionLink("Create Validate Value", "Create", new { id = Model.ClientSubUnitClientDefinedReference.ClientSubUnitClientDefinedReferenceId, csu = Model.ClientSubUnit.ClientSubUnitGuid, RelatedToDisplayName = Request.QueryString["RelatedToDisplayName"]}, new { @class = "red", title = "Create Validate Value" })%></td> 
		    </tr> 
        </table>
        </div>
    </div>
	<script type="text/javascript">
		$(document).ready(function () {
			$('#menu_clients').click();
			$("tr:odd").addClass("row_odd");
			$("tr:even").addClass("row_even");
			$('#search').show();
			$('#search_wrapper').css('height', '32px');

			//Search
			$('#frmSearch').submit(function () {
				if ($('#filter').val() == "") {
					alert("No Search Text Entered");
					return false;
				}
				$('<input>').attr({ type: 'hidden', id: 'id', name: 'id', value: '<%= Html.Encode(Model.ClientSubUnitClientDefinedReference.ClientSubUnitClientDefinedReferenceId) %>' }).appendTo('#frmSearch');
				$("#frmSearch").attr("action", "/ClientSubUnitCDRItem.mvc/List");

			});

			$('#btnSearch').click(function () {
				$("#frmSearch").submit();

			});
		})
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Client Sub Units", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client Sub Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.RouteLink("CDR Links", "Main", new { controller="ClientSubUnitCDR", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "CDR Links" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnitClientDefinedReference.ClientSubUnitClientDefinedReferenceId.ToString(), "Main", new { controller="ClientSubUnitCDR", action = "View", id = Model.ClientSubUnitClientDefinedReference.ClientSubUnitClientDefinedReferenceId }, new { title = Model.ClientSubUnitClientDefinedReference.ClientSubUnitClientDefinedReferenceId.ToString() })%> &gt;
Validate Values
</asp:Content>
