<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.City>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Admin</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
<div id="banner"><div id="banner_text">City</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">View City</th> 
		    </tr> 
            <tr>
                <td>City Name</td>
                <td><%= Html.Encode(Model.Name)%></td>
                <td></td>
            </tr>
            <tr>
                <td>City Code</td>
                <td><%= Html.Encode(Model.CityCode)%></td>
                <td></td>
            </tr>       
            <tr>
                <td>Country</td>
                <td><%= Html.Encode(Model.CountryName)%></td>
                <td></td>
            </tr> 
			<tr>
                <td>State/Province</td>
                <td><%= Html.Encode(Model.StateProvince != null ? Model.StateProvince.Name : "")%></td>
                <td></td>
            </tr>
			<tr>
                <td>Latitude</td>
                <td><%= Html.Encode(Model.LatitudeDecimal)%></td>
                <td></td>
            </tr>
			<tr>
                <td>Longitude</td>
                <td><%= Html.Encode(Model.LongitudeDecimal)%></td>
                <td></td>
            </tr>
			<tr>
				<td>Time Zone</td>
				<td><%= Html.Encode(Model.TimeZoneRule != null ? Model.TimeZoneRule.TimeZoneRuleCodeDesc : "")%></td>
				<td></td>
			</tr> 
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                <td class="row_footer_blank_right"></td>
            </tr>
        </table>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_admin').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Cities", "Main", new { controller = "City", action = "List", }, new { title = "Cities" })%> &gt;
View City &gt;
<%:Model.Name%>
</asp:Content>

