<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.SystemUser>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - SystemUsers
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
        <div id="banner"><div id="banner_text">System Users</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View SystemUser</th> 
		        </tr> 
                <tr>
                    <td>Name</td>
                    <td><%= Html.Encode(Model.LastName)%>, <%= Html.Encode(Model.FirstName)%> <%= Html.Encode(Model.MiddleName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Language</td>
                    <td><%= Html.Encode(Model.LanguageName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>IsActive?</td>
                    <td><%= Html.Encode(Model.IsActiveFlag)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Login</td>
                    <td><%= Html.Encode(Model.SystemUserLoginIdentifier)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Profile ID</td>
                    <td><%= Html.Encode(Model.UserProfileIdentifier)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Location</td>
                    <td><%= Html.Encode(Model.LocationName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Cuba Booking Allowed</td>
                    <td><%=Html.Encode(Model.CubaBookingAllowanceIndicator.HasValue && Model.CubaBookingAllowanceIndicator.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Military and Government User</td>
                    <td><%=Html.Encode(Model.MilitaryAndGovernmentUserFlag.HasValue && Model.MilitaryAndGovernmentUserFlag.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>  
				<tr>
                    <td>Default Profile?</td>
                    <td><%=Html.Encode(Model.DefaultProfileIdentifier.HasValue && Model.DefaultProfileIdentifier.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Restricted?</td>
                    <td><%=Html.Encode(Model.RestrictedFlag.HasValue && Model.RestrictedFlag.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Online User?</td>
                    <td><%=Html.Encode(Model.OnlineUserFlag.HasValue && Model.OnlineUserFlag.Value == true ? "True" : "False")%></td>
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
        $('#menu_teams').click();
		  $("tr:odd").addClass("row_odd");
      $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

