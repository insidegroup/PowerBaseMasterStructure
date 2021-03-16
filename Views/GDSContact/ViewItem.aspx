<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.GDSContact>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Contact</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">View GDS Contact</th> 
		    </tr> 
			<tr>
				<td><label for="PhoneNumber">GDSName</label></td>
				<td><%= Html.Encode(Model.GDS.GDSName)%></td>
				<td></td>
			</tr>
			<tr>
                <td><label for="CountryCode">Country</label></td>
                <td><%= Html.Encode(Model.Country.CountryName)%></td>
                <td></td>
            </tr>
			<tr>
                <td><label for="GlobalRegionName">Global Region</label></td>
                <td><%= Html.Encode(Model.GlobalRegion.GlobalRegionName)%></td>
                <td></td>
            </tr>
			<tr>
				<td><label for="PseudoCityOrOfficeDefinedRegionName">Pseudo City/Office ID Defined Region</label></td>
				<td><%= Html.Encode(Model.PseudoCityOrOfficeDefinedRegion != null ? Model.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionName : "")%></td>
				<td></td>
			</tr>
			<tr>
				<td><label for="PseudoCityOrOfficeBusinessName">Pseudo City/Office ID Business</label></td>
				<td><%= Html.Encode(Model.PseudoCityOrOfficeBusiness.PseudoCityOrOfficeBusinessName)%></td>
				<td></td>
			</tr>
			<tr>
                <td><label for="GDSRequestTypes">GDS Request Type</label></td>
                <td>
					<% if(Model.GDSRequestTypes != null) { %>
						<% =Html.Encode(String.Join(", ", Model.GDSRequestTypes.Select(x => x.GDSRequestTypeName).ToList())) %>
					<% } %>
				</td>
                <td></td>
            </tr>
			<tr>
                <td><label for="LastName">Last Name</label></td>
                <td><%= Html.Encode(Model.LastName)%></td>
                <td></td>
            </tr>
			<tr>
				<td><label for="FirstName">First Name</label></td>
				<td><%= Html.Encode(Model.FirstName)%></td>
                <td></td>
			</tr>
			<tr>
				<td><label for="Email">Email</label></td>
				<td><%= Html.Encode(Model.EmailAddress)%></td>
                <td></td>
			</tr>
			<tr>
				<td><label for="Phone">Phone</label></td>
				<td><%= Html.Encode(Model.Phone)%></td>
                <td></td>
			</tr>
			<tr>
                <td width="40%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="20%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                <td class="row_footer_blank_right" colspan="2"></td>
            </tr>
        </table>
    </div>
</div>

<script type="text/javascript">
$(document).ready(function() {
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
})
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("GDS Contact", "Main", new { controller = "GDSContact", action = "List", }, new { title = "GDS Contact" })%> &gt;
View
</asp:Content>