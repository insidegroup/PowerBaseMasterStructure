 <%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPolicyOtherGroupHeaderColumnNameSequences_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Column Names
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Column Names - Ordering</div></div>
    <div id="content">
       <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
			 <table width="100%" cellpadding="0" cellspacing="0" border="0" id="sortable">
            <thead>
				<tr> 
		        <th width="17%" class="row_header_left">Order</th> 
		        <th width="70%">Column Name</th>
		        <th width="13%" class="row_header_right">&nbsp;</th> 
	        </tr>
			</thead> 
			<tbody class="content">
				<%
				var iCounter = 0;
				foreach (var item in Model) { iCounter++; %>
				<tr width="100%" id="<%= Html.Encode(item.PolicyOtherGroupHeaderColumnNameId)%>_<%= Html.Encode(item.VersionNumber)%>">
					<td><%= Html.Encode(item.DisplayOrder) %></td>
					<td><%= Html.Encode(item.ColumnName) %></td>
					<td><img src="/images/arrow.png" alt=""/></td>
				</tr>			      
            <% 
            } 
            %>
			</tbody> 
            <tr>
                <td colspan="3" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "EditSequence", id = ViewData["PolicyOtherGroupHeaderId"], page = (Model.PageIndex - 1), sortOrder = "1" }, new { onclick = "return checkSaved();", title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "EditSequence", id = ViewData["PolicyOtherGroupHeaderId"], page = (Model.PageIndex + 1), sortOrder = "1" }, new { onclick = "return checkSaved();", title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
	        <tr> 
	            <td class="row_footer_blank_left" colspan="1">
					<a href="javascript:window.print();" class="red" title="Print">Print</a>
	            </td>
		        <td class="row_footer_blank_right" colspan="2">
					<%=Html.RouteLink("Cancel", "Main", new { controller = "PolicyOtherGroupHeaderColumnName", action = "List", id = ViewData["PolicyOtherGroupHeaderId"].ToString() }, new { title = "Cancel", @class = "red" })%>
					&nbsp;<input type="submit" value="Save" title="Save" class="red"/>
		        </td> 
	        </tr> 
        </table>
		<input type="hidden" name="SequenceOriginal" id="SequenceOriginal" />
        <input type="hidden" name="Sequence" id="Sequence" />
		<input type="hidden" name="page" id="page" value="<%=ViewData["Page"] %>" />
		<input type="hidden" name="id" id="id" value="<%= ViewData["PolicyOtherGroupHeaderId"].ToString() %>" />
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/Sequencing/PolicyOtherGroupHeaderColumnName.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Other Group Header", "Main", new { controller = "PolicyOtherGroupHeader", action = "List", id = ViewData["PolicyOtherGroupHeaderId"].ToString() }, new { title = "Policy Other Group Header" })%> &gt;
<%=Html.RouteLink(ViewData["Label"].ToString(), "Main", new { controller = "PolicyOtherGroupHeader", action = "List", id = ViewData["PolicyOtherGroupHeaderId"].ToString() }, new { title = ViewData["Label"].ToString() })%> &gt;
<%=Html.RouteLink("Column Names", "Main", new { controller = "PolicyOtherGroupHeaderColumnName", action = "List", id = ViewData["PolicyOtherGroupHeaderId"].ToString() }, new { title = "Column Names" })%>&gt;
Edit Order
</asp:Content>