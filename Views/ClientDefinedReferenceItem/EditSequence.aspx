 <%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientDefinedReferenceItemSequences_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnit - CDRs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">CDRs - Ordering</div></div>
    <div id="content">
       <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
			 <table width="100%" cellpadding="0" cellspacing="0" border="0" id="sortable">
            <thead>
				<tr> 
		        <th width="5%" class="row_header_left">Order</th> 
		        <th width="20%">Display Name Alias</th> 
		        <th width="18%">Back Office Display Name</th> 
		        <th width="11%">Entry Format</th> 
		        <th width="7%">Mandatory</th> 
		        <th width="5%">Min</th>
				<th width="8%">Max</th> 
		        <th width="8%">Droplist</th>
		        <th width="13%" class="row_header_right">&nbsp;</th> 
	        </tr>
			</thead> 
			<tbody class="content">
				<%
				var iCounter = 0;
				foreach (var item in Model) {
					iCounter++; %>
				<tr width="100%" id="<%= Html.Encode(item.ClientDefinedReferenceItemId)%>_<%= Html.Encode(item.VersionNumber)%>">
					<td><%= Html.Encode(item.SequenceNumber) %></td>
					<td><%= CWTDesktopDatabase.Helpers.CWTStringHelpers.TrimString(Html.Encode(item.DisplayNameAlias), 20)%></td>
					<td><%= CWTDesktopDatabase.Helpers.CWTStringHelpers.TrimString(Html.Encode(item.DisplayName), 20) %></td>
					<td><%= CWTDesktopDatabase.Helpers.CWTStringHelpers.TrimString(Html.Encode(item.EntryFormat), 20) %></td>
					<td><%= Html.Encode(item.MandatoryFlag) %></td>
					<td><%= Html.Encode(item.MinLength) %></td>
					<td><%= Html.Encode(item.MaxLength) %></td>
					<td><%= Html.Encode(item.TableDrivenFlag) %></td>
					<td><img src="../../images/arrow.png" alt=""/></td>
				</tr>
			      
            <% 
            } 
            %>
			</tbody> 
            <tr>
                <td colspan="9" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientSubUnitClientAccount", action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"].ToString(), page = (Model.PageIndex - 1), sortField = "AccountName", sortOrder = "1" }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientSubUnitClientAccount", action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"].ToString(), page = (Model.PageIndex + 1), sortField = "AccountName", sortOrder = "1" }, new { title = "Next Page" })%>
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
		        <td class="row_footer_blank_right" colspan="7">
					<a href="javascript:history.back();" class="red" title="Cancel">Cancel</a>&nbsp;<input type="submit" value="Save" title="Save" class="red"/>
		        </td> 
	        </tr> 
        </table>
		<input type="hidden" name="SequenceOriginal" id="SequenceOriginal" />
        <input type="hidden" name="Sequence" id="Sequence" />
		<input type="hidden" name="page" id="page" value="<%=ViewData["Page"] %>" />
		<input type="hidden" name="id" id="id" value="<%= ViewData["ClientSubUnitGuid"].ToString() %>" />
		<input type="hidden" name="can" id="can" value="<%= ViewData["ClientAccountNumber"].ToString() %>" />
		<input type="hidden" name="ssc" id="ssc" value="<%=ViewData["SourceSystemCode"].ToString() %>" />
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/Sequencing/ClientDefinedReferenceItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("Client Accounts", "Main", new { controller = "ClientSubUnitClientAccount", action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title ="Client Accounts" })%> &gt;
<%=Html.RouteLink(ViewData["ClientAccountName"].ToString(), "Main", new { controller = "ClientAccount", action = "ViewItem", ssc = ViewData["SourceSystemCode"].ToString(), can = ViewData["ClientAccountNumber"].ToString()}, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
CDRs
</asp:Content>