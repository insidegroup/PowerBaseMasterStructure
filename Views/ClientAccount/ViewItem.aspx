<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientAccount>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Client Accounts</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View Client Account</th> 
		        </tr> 
                 <tr>
                    <td>Client Account Number</td>
                    <td><%= Html.Encode(Model.ClientAccountNumber)%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td>Client Account Name</td>
                    <td><%= Html.Encode(Model.ClientAccountName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Source System Code</td>
                    <td><%= Html.Encode(Model.SourceSystemCode)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Glory Account Name</td>
                    <td><%= Html.Encode(Model.GloryAccountName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Client Master Code</td>
                    <td><%= Html.Encode(Model.ClientMasterCode)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Effective Date</td>
                    <td><%= Html.Encode(Model.EffectiveDate.ToString("MMM dd, yyyy"))%></td>
                    <td></td>
                </tr>
                 <tr>
                    <td width="30%">Country</td>
                    <td width="40%"><%= Html.Encode(Model.CountryName)%></td>
                    <td width="30%"></td>
                </tr>   
                <tr>
                    <td>CFA</td>
                    <td><%= Html.Encode(Model.CFA)%></td>
                    <td></td>
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

