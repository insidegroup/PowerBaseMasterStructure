<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientDefinedRuleGroupSequences_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Rules Group - Ordering
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Rules Group - Ordering</div></div>
    <div id="content">
       <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
			 <table width="100%" cellpadding="0" cellspacing="0" border="0" id="sortable">
            <thead>
				<tr> 
		        <th width="10%" class="row_header_left"><%= Html.RouteLink("Order", "ListMain", new { page = 1, sortField = "GroupSequenceNumber", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], clientTopUnitName = ViewData["ClientTopUnitName"], clientSubUnitName = ViewData["ClientSubUnitName"], clientAccountNumber = ViewData["ClientAccountNumber"], sourceSystemCode = ViewData["SourceSystemCode"], travelerTypeName = ViewData["TravelerTypeName"], clientDefinedRuleGroupName = ViewData["ClientDefinedRuleGroupName"] })%></th> 
		        <th width="40%"><%= Html.RouteLink("Group Name", "ListMain", new { page = 1, sortField = "ClientDefinedRuleGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], clientTopUnitName = ViewData["ClientTopUnitName"], clientSubUnitName = ViewData["ClientSubUnitName"], clientAccountNumber = ViewData["ClientAccountNumber"], sourceSystemCode = ViewData["SourceSystemCode"], travelerTypeName = ViewData["TravelerTypeName"], clientDefinedRuleGroupName = ViewData["ClientDefinedRuleGroupName"] })%></th> 
		        <th width="37%"><%= Html.RouteLink("Category", "ListMain", new { page = 1, sortField = "Category", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], clientTopUnitName = ViewData["ClientTopUnitName"], clientSubUnitName = ViewData["ClientSubUnitName"], clientAccountNumber = ViewData["ClientAccountNumber"], sourceSystemCode = ViewData["SourceSystemCode"], travelerTypeName = ViewData["TravelerTypeName"], clientDefinedRuleGroupName = ViewData["ClientDefinedRuleGroupName"]})%></th>
		        <th width="13%" class="row_header_right">&nbsp;</th> 
	        </tr>
			</thead> 
			<tbody class="content">
				<%
				var iCounter = 0;
				foreach (var item in Model) {
					iCounter++; %>
				<tr width="100%" id="<%= Html.Encode(item.ClientDefinedRuleGroupId)%>_<%= Html.Encode(item.HierarchyType)%>_<%= Html.Encode(item.VersionNumber)%>">
					<td><%= Html.Encode(item.GroupSequenceNumber) %></td>
					<td><%= Html.Encode(item.ClientDefinedRuleGroupName) %></td>
					<td><%= Html.Encode(item.Category) %></td>
					<td><img src="<%=Url.Content("~/Images/arrow.png")%>" alt=""/></td>
				</tr>
			      
            <% 
            } 
            %>
			</tbody> 
            <tr>
                <td colspan="4" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "EditSequence", sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], clientTopUnitName = ViewData["ClientTopUnitName"], clientSubUnitName = ViewData["ClientSubUnitName"], clientAccountNumber = ViewData["ClientAccountNumber"], sourceSystemCode = ViewData["SourceSystemCode"], travelerTypeName = ViewData["TravelerTypeName"], clientDefinedRuleGroupName = ViewData["ClientDefinedRuleGroupName"], page = (Model.PageIndex - 1) }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "EditSequence", sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], clientTopUnitName = ViewData["ClientTopUnitName"], clientSubUnitName = ViewData["ClientSubUnitName"], clientAccountNumber = ViewData["ClientAccountNumber"], sourceSystemCode = ViewData["SourceSystemCode"], travelerTypeName = ViewData["TravelerTypeName"], clientDefinedRuleGroupName = ViewData["ClientDefinedRuleGroupName"], page = (Model.PageIndex + 1) }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
	        <tr> 
	            <td class="row_footer_blank_left" colspan="2">
					<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a>
	            </td>
		        <td class="row_footer_blank_right" colspan="2">
					<a href="javascript:history.back();" class="red" title="Cancel">Cancel</a>&nbsp;<input type="submit" value="Save" title="Save" class="red"/>
		        </td> 
	        </tr> 
        </table>
		<input type="hidden" name="SequenceOriginal" id="SequenceOriginal" />
        <input type="hidden" name="Sequence" id="Sequence" />
		<input type="hidden" name="page" id="page" value="<%=ViewData["Page"] %>" />
		<input type="hidden" name="ClientTopUnitName" id="ClientTopUnitName" value="<%=ViewData["ClientTopUnitName"] %>" />
		<input type="hidden" name="ClientSubUnitName" id="ClientSubUnitName" value="<%=ViewData["ClientSubUnitName"] %>" />
		<input type="hidden" name="ClientAccountNumber" id="ClientAccountNumber" value="<%=ViewData["ClientAccountNumber"] %>" />
		<input type="hidden" name="SourceSystemCode" id="SourceSystemCode" value="<%=ViewData["SourceSystemCode"] %>" />
		<input type="hidden" name="TravelerTypeName" id="TravelerTypeName" value="<%=ViewData["TravelerTypeName"] %>" />
		<input type="hidden" name="ClientDefinedRuleGroupName" id="ClientDefinedRuleGroupName" value="<%=ViewData["ClientDefinedRuleGroupName"]  %>" />
		<input type="hidden" name="Filter" id="Filter" value="<%=ViewData["Filter"] %>" />
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/Sequencing/ClientDefinedRuleGroup.js")%>" type="text/javascript"></script>
<script type="text/javascript">
	$(document).ready(function () {
		$('#breadcrumb').css('width', 'auto');
		$('.full-width #search_wrapper').css('height', '22px');
	})
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%= Html.ActionLink("Client Rules Groups", "ListUndeleted", "ClientDefinedRuleGroup")%> &gt;
<%= Html.Encode(ViewData["Filter"].ToString()) %>
</asp:Content>