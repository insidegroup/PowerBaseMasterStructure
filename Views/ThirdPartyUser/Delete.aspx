<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ThirdPartyUserVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Third Party Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Third Party Users</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Third Party User</th> 
		        </tr> 
               <tr>
                    <td>Third Party User ID</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.TISUserId)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Third Party User Name</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.ThirdPartyName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Last Name</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.LastName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>First Name</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.FirstName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Email</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.Email)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Is Active?</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.IsActiveFlag.HasValue && Model.ThirdPartyUser.IsActiveFlag.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Cuba Booking Allowed</td>
                    <td><%=Html.Encode(Model.ThirdPartyUser.CubaBookingAllowanceIndicator.HasValue && Model.ThirdPartyUser.CubaBookingAllowanceIndicator.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Military and Government User</td>
                    <td><%=Html.Encode(Model.ThirdPartyUser.MilitaryAndGovernmentUserFlag.HasValue && Model.ThirdPartyUser.MilitaryAndGovernmentUserFlag.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Robotic User</td>
                    <td><%=Html.Encode(Model.ThirdPartyUser.RoboticUserFlag.HasValue && Model.ThirdPartyUser.RoboticUserFlag.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>CWT Manager</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.CWTManager)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>User Type</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.ThirdPartyUserType.ThirdPartyUserTypeName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Client TopUnit</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.ClientTopUnit != null ? Model.ThirdPartyUser.ClientTopUnit.ClientTopUnitName : "")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Client SubUnit</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.ClientSubUnit != null ? Model.ThirdPartyUser.ClientSubUnit.ClientSubUnitName : "")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Partner Name</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.Partner != null ? Model.ThirdPartyUser.Partner.PartnerName : "")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Vendor Name</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.GDSThirdPartyVendor != null ? Model.ThirdPartyUser.GDSThirdPartyVendor.GDSThirdPartyVendorName : "")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>First Address Line</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.FirstAddressLine)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Second Address Line</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.SecondAddressLine)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>City</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.CityName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Postal Code</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.PostalCode)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Country</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.Country != null ? Model.ThirdPartyUser.Country.CountryName : "")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>State/Province</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.StateProvinceCode != null && Model.ThirdPartyUser.StateProvince != null ? Model.ThirdPartyUser.StateProvince.Name : "")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Phone Number</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.Phone)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Deleted?</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.DeletedFlag.HasValue && Model.ThirdPartyUser.DeletedFlag.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Deleted Date Time</td>
                    <td><%= Html.Encode(Model.ThirdPartyUser.DeletedDateTime.HasValue ? Model.ThirdPartyUser.DeletedDateTime.Value.ToShortDateString() : "No Deleted Date")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Internal Remarks</td>
                    <td colspan="2">
						<% if (Model.ThirdPartyUser.ThirdPartyUserInternalRemarks != null && Model.ThirdPartyUser.ThirdPartyUserInternalRemarks.Count > 0) { %>
							<dl class="InternalRemarks">
								<% foreach (CWTDesktopDatabase.Models.ThirdPartyUserInternalRemark item in Model.ThirdPartyUser.ThirdPartyUserInternalRemarks){ %>
									<dt><%= Html.Encode(item.CreationTimestamp.Value.ToString("yyyy-MM-dd")) %></dt>
									<dd><%= Html.Encode(item.InternalRemark) %></dd>
								<% } %>
							</dl>
						<% } %>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a> 
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
                    </td>                    
                    <td class="row_footer_blank_right">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
							<%= Html.HiddenFor(model => model.ThirdPartyUser.ThirdPartyUserId) %>
							<%= Html.HiddenFor(model => model.ThirdPartyUser.VersionNumber) %>
						<%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_gdsmanagement').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
	});
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("Third Party Users", "Main", new { controller = "ThirdPartyUser", action = "ListUnDeleted", }, new { title = "Third Party Users" })%> &gt;
<%= Html.Encode(Model.ThirdPartyUser.ThirdPartyName)%>, <%= Html.Encode(Model.ThirdPartyUser.TISUserId)%> &gt;
Delete
</asp:Content>