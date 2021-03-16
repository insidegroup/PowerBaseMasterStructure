<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitContactVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit - Client Detail Contact</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		          <tr> 
			        <th class="row_header" colspan="3">View Contact</th> 
		        </tr> 
                 <tr>
                    <td>First Name</td>
                    <td><%= Html.Encode(Model.Contact.FirstName)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Middle Name</td>
                    <td><%= Html.Encode(Model.Contact.MiddleName)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Last Name</td>
                    <td><%= Html.Encode(Model.Contact.LastName)%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td>Primary Telephone Number</td>
                    <td><%= Html.Encode(Model.Contact.PrimaryTelephoneNumber)%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td>Emergency Telephone Number</td>
                    <td><%= Html.Encode(Model.Contact.EmergencyTelephoneNumber)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Email Address</td>
                    <td><%= Html.Encode(Model.Contact.EmailAddress)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Contact Type</td>
                    <td><%= Html.Encode(Model.Contact.ContactTypeName)%></td>
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

