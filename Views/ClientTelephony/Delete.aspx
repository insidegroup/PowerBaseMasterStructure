<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientTelephonyVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Telephony</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Client Telephony</th> 
		        </tr>  
                <tr>
					<td><label for="PhoneNumber">Phone Number</label></td>
					<td><%= Html.Encode(Model.ClientTelephony.PhoneNumber)%></td>
					<td></td>
				</tr>
				<tr>
                    <td><label for="CountryCode">Country</label></td>
                    <td><%= Html.Encode(Model.ClientTelephony.Country.CountryName)%> (<%= Html.Encode(Model.ClientTelephony.Country.InternationalPrefixCode)%>)</td>
                    <td></td>
                </tr>
				<tr>
                    <td><label for="PhoneNumberwithInternationalPrefixCode">Phone Number with Prefix</label></td>
					<td><%= Html.Encode(Model.ClientTelephony.PhoneNumberwithInternationalPrefixCode)%></td>
					<td></td>
				</tr>
                <tr>
                    <td><label for="CallerEnteredDigitDefinitionTypeDescription">Indicator</label></td>
					<td><%= Html.Encode(Model.ClientTelephony.CallerEnteredDigitDefinitionType.CallerEnteredDigitDefinitionTypeDescription)%></td>
					<td></td>
				</tr>
				<tr>
                    <td><label for="HierarchyType">Hierarchy Type</label></td>
                    <td><%= Html.Encode(Model.ClientTelephony.HierarchyType)%></td>
                    <td></td>
                </tr>
				<tr>
					<td><label for="HierarchyItem">Hierarchy Item</label></td>
					<td><%= Html.Encode(Model.ClientTelephony.HierarchyName)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="MainNumberFlag">Main Number?</label></td>
					<td><%= Html.Encode(Model.ClientTelephony.MainNumberFlagNullable)%></td>
					<td></td>
				</tr>
				<tr>
                    <td><label for="TelephoneType">Telephone Type</label></td>
					<td><%= Html.Encode(Model.ClientTelephony.TelephonyType.TelephonyTypeDescription)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td><label for="TravelerBackOfficeTypeCode">Back Office Type</label></td>
                    <td><%= Html.Encode(Model.ClientTelephony.TravelerBackOfficeTypeCode)%></td>
                    <td></td>
                </tr>
				<tr>
					<td><label for="ExpiryDate">Expiry Date</label></td>
					<td><%= Html.Encode(Model.ClientTelephony.ExpiryDate.HasValue ? Model.ClientTelephony.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ClientTelephonyDescription">Telephony Description</label></td>
					<td><%= Html.Encode(Model.ClientTelephony.ClientTelephonyDescription)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ClientSnSButtonText">Client S&S Button Text</label></td>
					<td><%= Html.Encode(Model.ClientTelephony.ClientSnSButtonText)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="ClientSnSDescription">Client S&S Description</label></td>
					<td><%= Html.Encode(Model.ClientTelephony.ClientSnSDescription)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="CreationTimestamp">Creation Date</label></td>
					<td><%= Html.Encode(Model.ClientTelephony.CreationTimestamp.Value.ToString("dd/MM/yyyy"))%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="LastUpdateTimestamp">Last Updated</label></td>
					<td>
						<% if(Model.ClientTelephony.LastUpdateTimestamp.HasValue) { %>
							<%= Html.Encode(Model.ClientTelephony.LastUpdateTimestamp.Value.ToString("dd/MM/yyyy")) %>
						<% } %>
					</td>
					<td></td>
				</tr>
				<tr>
					<td><label for="LastUpdateUserIdentifier">Last Updated By</label></td>
					<td>
						<% if(Model.ClientTelephony.LastUpdateUserIdentifier != null) { %>
							<%= Html.Encode(Model.ClientTelephony.LastUpdateUserIdentifier) %>
						<% } else { %>
							<%= Html.Encode(Model.ClientTelephony.CreationUserIdentifier) %>
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
                    <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
                    </td>
                    <td class="row_footer_blank_right" colspan="2">
							<% using (Html.BeginForm()) { %>
								<%= Html.AntiForgeryToken() %>
								<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
								<%= Html.HiddenFor(model => model.ClientTelephony.ClientTelephonyId) %>
								<%= Html.HiddenFor(model => model.ClientTelephony.VersionNumber) %>
						<%}%>
					</td>
				</tr>
			</table>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/ClientTelephony.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Client Telephony", "Main", new { controller = "ClientTelephony", action = "List", }, new { title = "Client Telephony" })%> &gt;
Delete Client Telephony &gt;
<%:Model.ClientTelephony.PhoneNumber%>
</asp:Content>