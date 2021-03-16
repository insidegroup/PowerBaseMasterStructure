<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientTelephony>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Client Telephony</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">View Client Telephony</th> 
		    </tr> 
			<tr>
			<td><label for="PhoneNumber">Phone Number</label></td>
				<td><%= Html.Encode(Model.PhoneNumber)%></td>
				<td></td>
			</tr>
			<tr>
                <td><label for="CountryCode">Country</label></td>
                <td><%= Html.Encode(Model.Country.CountryName)%> (<%= Html.Encode(Model.Country.InternationalPrefixCode)%>)</td>
                <td></td>
            </tr>
			<tr>
                <td><label for="PhoneNumberwithInternationalPrefixCode">Phone Number with Prefix</label></td>
                <td><%= Html.Encode(Model.PhoneNumberwithInternationalPrefixCode)%></td>
                <td></td>
            </tr>
            <tr>
                <td><label for="CallerEnteredDigitDefinitionTypeDescription">Indicator</label></td>
				<td><%= Html.Encode(Model.CallerEnteredDigitDefinitionType.CallerEnteredDigitDefinitionTypeDescription)%></td>
				<td></td>
			</tr>
			<tr>
                <td><label for="HierarchyType">Hierarchy Type</label></td>
                <td><%= Html.Encode(Model.HierarchyType)%></td>
                <td></td>
            </tr>
			<tr>
				<td><label for="HierarchyItem">Hierarchy Item</label></td>
				<td><%= Html.Encode(Model.HierarchyName)%></td>
				<td></td>
			</tr>
			<tr>
				<td><label for="MainNumberFlag">Main Number?</label></td>
				<td><%= Html.Encode(Model.MainNumberFlagNullable)%></td>
				<td></td>
			</tr>
			<tr>
                <td><label for="TelephonyType">Telephony Type</label></td>
                <td><%= Html.Encode(Model.TelephonyType.TelephonyTypeDescription)%></td>
                <td></td>
            </tr>
			<tr>
                <td><label for="TravelerBackOfficeTypeCode">Back Office Type</label></td>
                <td><%= Html.Encode(Model.TravelerBackOfficeTypeCode)%></td>
                <td></td>
            </tr>
			<tr>
				<td><label for="ExpiryDate">Expiry Date</label></td>
				<td><%= Html.Encode(Model.ExpiryDate.HasValue ? Model.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                <td></td>
			</tr>
			<tr>
				<td><label for="ClientTelephonyDescription">Telephony Description</label></td>
				<td><%= Html.Encode(Model.ClientTelephonyDescription)%></td>
                <td></td>
			</tr>
			<tr>
				<td><label for="ClientSnSButtonText">Client S&S Button Text</label></td>
				<td><%= Html.Encode(Model.ClientSnSButtonText)%></td>
                <td></td>
			</tr>
			<tr>
				<td><label for="ClientSnSDescription">Client S&S Description</label></td>
				<td><%= Html.Encode(Model.ClientSnSDescription)%></td>
                <td></td>
			</tr>
			<tr>
				<td><label for="CreationTimestamp">Creation Date</label></td>
				<td><%= Html.Encode(Model.CreationTimestamp.Value.ToString("dd/MM/yyyy"))%></td>
                <td></td>
			</tr>
			<tr>
				<td><label for="LastUpdateTimestamp">Last Updated</label></td>
				<td>
					<% if(Model.LastUpdateTimestamp.HasValue) { %>
						<%= Html.Encode(Model.LastUpdateTimestamp.Value.ToString("dd/MM/yyyy")) %>
					<% } %>
				</td>
                <td></td>
			</tr>
			<tr>
				<td><label for="LastUpdateUserIdentifier">Last Updated By</label></td>
				<td>
					<% if(Model.LastUpdateUserIdentifier != null) { %>
						<%= Html.Encode(Model.LastUpdateUserIdentifier) %>
					<% } else { %>
						<%= Html.Encode(Model.CreationUserIdentifier) %>
					<% } %>
				</td>
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
<%=Html.RouteLink("Client Telephony", "Main", new { controller = "ClientTelephony", action = "List", }, new { title = "Client Telephony" })%> &gt;
View Client Telephony &gt;
<%:Model.PhoneNumber%>
</asp:Content>