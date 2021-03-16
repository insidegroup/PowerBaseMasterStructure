<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TravelPort>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Travel Ports</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">View Travel Port</th> 
		    </tr> 
            <tr>
			<td><label for="TravelportName">Travel Port Name</label></td>
				<td><%= Html.Encode(Model.TravelportName) %></td>
				<td></td>
			</tr>   
			<tr>
				<td><label for="TravelPortCode">Travel Port Code</label></td>
				<td><%= Html.Encode(Model.TravelPortCode) %></td>
				<td></td>
			</tr>
			<tr>
                <td><label for="TravelPortTypeId">Travel Port Type</label></td>
                <td><%= Html.Encode(Model.TravelPortTypeDescription)%></td>
                <td></td>
            </tr>
			<tr>
                <td><label for="CityName">Travel Port City</label></td>
                <td><%= Html.Encode(Model.CityName)%> (<%= Html.Encode(Model.CityCode)%>)</td>
                <td></td>
            </tr>
			<tr>
                <td><label for="MetropolitanArea">Metropolitan Area</label></td>
                <td><%= Html.Encode(Model.MetropolitanArea)%></td>
                <td></td>
            </tr>
			<tr>
                <td><label for="CountryName">Country</label></td>
                <td><%= Html.Encode(Model.CountryName)%></td>
                <td></td>
            </tr>
			<tr>
                <td><label for="StateProvince_Name">State/Province</label></td>
                <td><%= Html.Encode(Model.StateProvince != null ? Model.StateProvince.Name : "")%></td>
                <td></td>
            </tr>
			<tr>
                <td><label for="LatitudeDecimal">Latitude</label></td>
                <td><%= Html.Encode(Model.LatitudeDecimal)%></td>
                <td></td>
            </tr>
			<tr>
                <td><label for="LongitudeDecimal">Longitude</label></td>
                <td><%= Html.Encode(Model.LongitudeDecimal)%></td>
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
<%=Html.RouteLink("Travel Ports", "Main", new { controller = "TravelPort", action = "List", }, new { title = "Travel Ports" })%> &gt;
View Travel Port &gt;
<%:Model.TravelportName%>
</asp:Content>