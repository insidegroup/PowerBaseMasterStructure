<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.AddressLocationVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Locations</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View Location</th> 
		        </tr> 
                 <tr>
                    <td>Location Name</td>
                    <td><%= Html.Encode(Model.Location.LocationName)%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td>Country</td>
                    <td><%= Html.Encode(Model.Location.CountryRegion.Country.CountryName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Country Region</td>
                    <td><%= Html.Encode(Model.Location.CountryRegion.CountryRegionName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>First Address Line</td>
                    <td><%= Html.Encode(Model.Address.FirstAddressLine)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Second Address Line</td>
                    <td><%= Html.Encode(Model.Address.SecondAddressLine)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>City</td>
                    <td><%= Html.Encode(Model.Address.CityName)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>State/Province</td>
                    <td><%= Html.Encode(Model.Address.StateProvinceName)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>Postal Code</td>
                    <td><%= Html.Encode(Model.Address.PostalCode)%></td>
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
    $(document).ready(function() {
    $('#menu_admin').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
})
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Locations", "Main", new { controller = "Location", action = "List", }, new { title = "Locations" })%> &gt;
View Location &gt;
<%= Html.Encode(Model.Location.LocationName) %>
</asp:Content>