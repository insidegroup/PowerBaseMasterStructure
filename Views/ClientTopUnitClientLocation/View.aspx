<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientTopUnitClientLocationVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Location
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client TopUnit - Client Location</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		          <tr> 
			        <th class="row_header" colspan="3">View Client Location</th> 
		        </tr> 
                 <tr>
                    <td>Client Location Name</td>
                    <td><%= Html.Encode(Model.ClientTopUnitClientLocation.AddressLocationName)%></td>
                    <td></td>
                 </tr>
				<tr>
                    <td>First Address Line</td>
                    <td><%= Html.Encode(Model.ClientTopUnitClientLocation.FirstAddressLine)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Second Address Line</td>
                    <td><%= Html.Encode(Model.ClientTopUnitClientLocation.SecondAddressLine)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>City</td>
                    <td><%= Html.Encode(Model.ClientTopUnitClientLocation.CityName)%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td>State/Province</td>
                    <td><%= Html.Encode(Model.ClientTopUnitClientLocation.StateProvinceName)%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td>Postal Code</td>
                    <td><%= Html.Encode(Model.ClientTopUnitClientLocation.PostalCode)%></td>
                    <td></td>
                 </tr>
				<tr>
                    <td>Country</td>
                    <td><%= Html.Encode(Model.ClientTopUnitClientLocation.CountryName)%></td>
                    <td></td>
                 </tr>
				<tr>
                    <td>Latitude</td>
                    <td><%= Html.Encode(Model.ClientTopUnitClientLocation.LatitudeDecimal)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Longitude</td>
                    <td><%= Html.Encode(Model.ClientTopUnitClientLocation.LongitudeDecimal)%></td>
                    <td></td>
                 </tr>
                <tr>
                    <td>Ranking</td>
                    <td><%= Html.Encode(Model.ClientTopUnitClientLocation.Ranking)%></td>
                    <td></td>
                 </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                  <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right" colspan="2"></td>
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
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Client Locations", "Main", new { controller = "ClientTopUnitClientLocation", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client Locations" })%> &gt;
View Client Location
</asp:Content>