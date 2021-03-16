<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientSubUnit>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Clients
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
        <div id="banner"><div id="banner_text">ClientSubUnits</div></div>

        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View ClientSubUnit</th> 
		        </tr> 
                 <tr>
                    <td>Name</td>
                    <td><%= Html.Encode(Model.ClientSubUnitName)%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td>Display Name</td>
                    <td><%= Html.Encode(Model.ClientSubUnitDisplayName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Guid</td>
                    <td><%= Html.Encode(Model.ClientSubUnitGuid)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Expiry Date/Time</td>
                    <td><%= Html.Encode(Model.ExpiryDate.HasValue ? Model.ExpiryDate.Value.ToString("MM/dd/yyyy hh:mm:ss tt") : "No Expiry Date")%></td>
                    <td></td>
                </tr>
                 <tr>
                    <td>Portrait Status</td>
                    <td><%= Html.Encode(Model.PortraitStatus.PortraitStatusDescription)%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td>TopUnit</td>
                    <td><%= Html.Encode(Model.ClientTopUnitName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Country</td>
                    <td><%= Html.Encode(Model.CountryName)%></td>
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
    $('#menu_clients').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
})
 </script>
</asp:Content>

