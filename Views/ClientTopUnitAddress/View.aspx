<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientTopUnitAddressVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client TopUnit - Client Detail Address</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		          <tr> 
			        <th class="row_header" colspan="3">View Address</th> 
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
                    <td>County</td>
                    <td><%= Html.Encode(Model.Address.CountyName)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Country</td>
                    <td><%= Html.Encode(Model.Address.CountryName)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Latitude</td>
                    <td><%= Html.Encode(Model.Address.LatitudeDecimal)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Longitude</td>
                    <td><%= Html.Encode(Model.Address.LongitudeDecimal)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Mapping Quality</td>
                    <td><%= Html.Encode(Model.Address.MappingQualityCode)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Postal Code</td>
                    <td><%= Html.Encode(Model.Address.PostalCode)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Replicated From ClientMaintenance</td>
                    <td><%= Html.Encode(Model.Address.ReplicatedFromClientMaintenanceFlag)%></td>
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

