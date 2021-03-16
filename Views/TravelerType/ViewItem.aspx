<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TravelerType>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit Traveler Types - Traveler Types</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View TravelerTypes</th> 
		        </tr> 
                 <tr>
                    <td>TravelerType Name</td>
                    <td><%= Html.Encode(Model.TravelerTypeName)%></td>
                    <td></td>
                </tr>   
                <tr>
                    <td>Employee Identifier</td>
                    <td><%= Html.Encode(Model.EmployeeIdentifier)%></td>
                    <td></td>
                </tr> 
                  <tr>
                    <td>Traveler Type Guid</td>
                    <td><%= Html.Encode(Model.TravelerTypeGuid)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Traveler BackOfficeType Description</td>
                    <td><%= Html.Encode(Model.TravelerBackOfficeTypeDescription)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Is Sponsor?</td>
                    <td>
						<% if(Model.TravelerTypeSponsor != null && Model.TravelerTypeSponsor.IsSponsorFlag != null) { %>
							<%= Html.Encode(Model.TravelerTypeSponsor.IsSponsorFlag)%>
						<% } %>
					</td>
                    <td></td>
                </tr>
				<tr>
                    <td>Requires Sponsor?</td>
                    <td>
						<% if(Model.TravelerTypeSponsor != null && Model.TravelerTypeSponsor.RequiresSponsorFlag != null) { %>
							<%= Html.Encode(Model.TravelerTypeSponsor.RequiresSponsorFlag)%>
						<% } %>
                    </td>
                    <td></td>
                </tr> 
                <tr>               
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>                    
                    <td class="row_footer_blank_right"></td>
                </tr>
            </table>
        </div>
    </div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client TopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client SubUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.RouteLink("Traveller Types", "Main", new { controller = "ClientSubUnitTravelerType", action = "ListByClientSubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.Encode(Model.TravelerTypeName) %>
</asp:Content>