<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientAccountClientDetailVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - ClientDetails
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Client Account - Client Details</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View Client Detail</th> 
		        </tr> 
                <tr>
                    <td>Client Detail Name</td>
                    <td><%= Html.Encode(Model.ClientDetail.ClientDetailName)%></td>
                    <td></td>
                </tr>         
                <tr>
                    <td>Website Address</td>
                    <td><%= Html.Encode(Model.ClientDetail.WebsiteAddress)%></td>
                    <td></td>
                </tr>         
                <tr>
                    <td>Logo</td>
                    <td><%= Html.Encode(Model.ClientDetail.Logo)%></td>
                    <td></td>
                </tr>         
                <tr>
                    <td>Inherit From Parent Flag</td>
                    <td><%= Html.Encode(Model.ClientDetail.InheritFromParentFlag)%></td>
                    <td></td>
                </tr>         
                <tr>
                    <td>Enabled</td>
                    <td><%= Html.Encode(Model.ClientDetail.EnabledFlag)%></td>
                    <td></td>
                </tr>         
               <tr>
                    <td>Enabled Date</td>
                    <td><%= Html.Encode(Model.ClientDetail.EnabledDate.HasValue ? Model.ClientDetail.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Expiry Date</td>
                    <td><%= Html.Encode(Model.ClientDetail.ExpiryDate.HasValue ? Model.ClientDetail.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                    <td></td>
                </tr>       
                <tr>
                    <td>Trip Type</td>
                    <td><%= Html.Encode(Model.ClientDetail.TripType)%></td>
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
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>
