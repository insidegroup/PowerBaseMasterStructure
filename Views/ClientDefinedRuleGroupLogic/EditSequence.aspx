 <%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientDefinedRuleLogicSequences_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - <%=ViewData["ClientDefinedRuleGroupType"] %> Rule Items - Ordering
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text"><%=ViewData["ClientDefinedRuleGroupType"] %> Rule Items - Ordering</div></div>
    <div id="content">
       <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
			 <table width="100%" cellpadding="0" cellspacing="0" border="0" id="sortable">
            <thead>
				<tr> 
		        <th width="10%" class="row_header_left">Order</th> 
		        <th width="40%">Conditions Description</th> 
		        <th width="37%">Operator</th>
		        <th width="13%" class="row_header_right">&nbsp;</th> 
	        </tr>
			</thead> 
			<tbody class="content">
				<%
				var iCounter = 0;
				foreach (var item in Model) {
					iCounter++; %>	
				<tr width="100%" id="<%= Html.Encode(item.ClientDefinedRuleLogicItemId)%>_<%= Html.Encode(item.VersionNumber)%>">
					<td><%= Html.Encode(item.LogicSequenceNumber) %></td>
					<td><%= Html.Encode(item.ClientDefinedRuleLogicItemValue) %></td>
					<td><%= Html.Encode(item.RelationalOperatorName) %></td>
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
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "EditSequence", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["NewSortOrder"].ToString(), filter = ViewData["Filter"], clientTopUnitName = ViewData["ClientTopUnitName"], clientSubUnitName = ViewData["ClientSubUnitName"], clientAccountNumber = ViewData["ClientAccountNumber"], sourceSystemCode = ViewData["SourceSystemCode"], travelerTypeName = ViewData["TravelerTypeName"],  clientDefinedRuleGroupName = ViewData["ClientDefinedRuleGroupName"]}, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "EditSequence", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["NewSortOrder"].ToString(), filter = ViewData["Filter"], clientTopUnitName = ViewData["ClientTopUnitName"], clientSubUnitName = ViewData["ClientSubUnitName"], clientAccountNumber = ViewData["ClientAccountNumber"], sourceSystemCode = ViewData["SourceSystemCode"], travelerTypeName = ViewData["TravelerTypeName"],  clientDefinedRuleGroupName = ViewData["ClientDefinedRuleGroupName"]}, new { title = "Next Page" })%>
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
		<input type="hidden" name="clientDefinedRuleGroupId" id="clientDefinedRuleGroupId" value="<%=ViewData["ClientDefinedRuleGroupId"].ToString() %>" />
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/Sequencing/ClientDefinedRuleGroupLogic.js")%>" type="text/javascript"></script>
<script type="text/javascript">
	$(document).ready(function () {
		$('#breadcrumb').css('width', 'auto');
		$('.full-width #search_wrapper').css('height', '22px');
	})
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%= Html.ActionLink(ViewData["ClientDefinedRuleGroupType"] + " Rules Groups", "ListUndeleted", new { controller = ViewData["ClientDefinedRuleGroupController"] })%> &gt;
<%= Html.ActionLink(ViewData["ClientDefinedRuleGroupName"].ToString(), "View", new { controller = "ClientDefinedRuleGroup", id = ViewData["ClientDefinedRuleGroupId"].ToString() })%> &gt;
Items &gt;
Conditions
</asp:Content>