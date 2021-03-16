<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPolicyGroupCountrySequences_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - LocalOperatingHoursGroups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Group Countries - Edit Sequencing</div></div>
        <div id="content">
         <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
            <table width="100%" cellpadding="0" cellspacing="0" id="sortable">
                <tr> 
                	
			        <th width="10%" class="row_header_left">Sequence</th> 
			        <th width="25%">Country</th> 
			        <th width="60%">Policy Group</th>
                    <th width="5%" class="row_header_right">&nbsp;</th>
		        </tr> 
                 <tbody class="content">  
                <%
                foreach (var item in Model) {
                   
                %>
                  <tr id="<%= Html.Encode(item.PolicyGroupId) %>_<%= Html.Encode(item.CountryCode) %>_<%= Html.Encode(item.VersionNumber) %>">
                    <td width="10%"><%= Html.Encode(item.SequenceNumber) %></td>
                    <td width="25%"><%= Html.Encode(item.CountryName)%></td>
                    <td width="60%"><%= Html.Encode(item.PolicyGroupName) %></td>
                    <td width="5%"><img src="../../images/arrow.png" /></td>
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
                            <%= Html.RouteLink("<<Previous Page", "Main", new { page = (Model.PageIndex - 1), type = ViewData["type"] },new { onclick="return checkSaved();" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                            <%= Html.RouteLink("Next Page>>>", "Main", new { page = (Model.PageIndex + 1), type = ViewData["type"] }, new { onclick = "return checkSaved();" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                    </td>
                </tr>
         
		        <tr> 
                 <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                 <td class="row_footer_blank_right" colspan="3"><%= Html.ActionLink("Cancel", "EditSequence", new { }, new { @class = "red" })%>&nbsp;<input type="submit" value="Save" title="Save" class="red"/></td>
		        </tr> 
		        
            </table>
           <input type="hidden" name="PolicySequenceOriginal" id="PolicySequenceOriginal" />
            <input type="hidden" name="PolicySequence" id="PolicySequence" />
            <input type="hidden" name="type" id="type" value="<%=ViewData["Type"] %>" />
            <input type="hidden" name="page" id="page" value="<%=ViewData["Page"] %>" />
             <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/Sequencing/PolicyGroup.js")%>" type="text/javascript"></script>
</asp:Content>

