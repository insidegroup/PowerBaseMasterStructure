<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPolicyAirVendorGroupItemSequences_v1Result>>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy AirVendor Group Items - Edit Sequencing</div></div>
        <div id="content">
         <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
            <table width="100%" cellpadding="0" cellspacing="0" id="sortable">
                <tr> 
                	
			        <th width="10%" class="row_header_left">Sequence</th> 
			        <th width="33%">Routing</th> 
			        <th width="12%">Enabled Date</th> 
			        <th width="25%">Supplier</th> 
			        <th width="15%">Status</th> 
                    <th width="5%" class="row_header_right">&nbsp;</th>
		        </tr> 
		       
                  <tbody class="content">  
		               
                <%
                var iCounter = 0;
                foreach (var item in Model) {
                    iCounter++;
                   
                %>
                <tr width="100%" id="<%= Html.Encode(item.PolicyAirVendorGroupItemId)%>_<%= Html.Encode(item.VersionNumber) %>">
                    <td width="10%"><%= Html.Encode(item.SequenceNumber) %></td>
                    <td width="33%"><%= Html.Encode(item.Name)%></td>
                    <td width="12%"><%= Html.Encode(item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString("MMM dd, yyyy") : "")%></td>                    
                    <td width="25%"><%= Html.Encode(item.SupplierName) %></td>
                    <td width="15%"><%= Html.Encode(item.PolicyAirStatusDescription) %></td>
                    <td width="5%"><img src="../../images/arrow.png" /></td>
                </tr>
				<% 
                } 
				
                %>

                
       </tbody>
                <tr>
                    <td colspan="6" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                            <%= Html.RouteLink("<<Previous Page", "Default", new { page = (Model.PageIndex - 1), id = ViewData["PolicyGroupId"] }, new { onclick = "return checkSaved();" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                            <%= Html.RouteLink("Next Page>>>", "Default", new { page = (Model.PageIndex + 1), id = ViewData["PolicyGroupId"] }, new { onclick = "return checkSaved();" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                    </td>
                </tr>
         
		         <tr> 
                 <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                 <td class="row_footer_blank_right" colspan="2"><%= Html.ActionLink("Reset", "EditSequence", new { id = ViewData["PolicyGroupId"] }, new { @class = "red" })%>&nbsp;<input type="submit" value="Save" title="Save" class="red"/></td>
		        </tr> 
		        
            </table>
            <input type="hidden" name="PolicySequenceOriginal" id="PolicySequenceOriginal" />
            <input type="hidden" name="PolicySequence" id="PolicySequence" />
            <input type="hidden" name="type" id="type" value="<%=ViewData["PolicyGroupId"] %>" />
            <input type="hidden" name="page" id="page" value="<%=ViewData["Page"] %>" />
             <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/Sequencing/PolicyGroup.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Html.Encode(CWTStringHelpers.TrimString(ViewData["PolicyGroupName"].ToString(), 60)), "Default", new { controller = "PolicyGroup", action = "View", id = ViewData["PolicyGroupId"] }, new { title = ViewData["PolicyGroupName"] })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = ViewData["PolicyGroupId"] }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Air Vendor Group Items", "Default", new { controller = "PolicyAirVendorGroupItem", action = "List", id = ViewData["PolicyGroupId"] }, new { title = "Policy Air Vendor Group Items" })%> &gt;
Item
</asp:Content>