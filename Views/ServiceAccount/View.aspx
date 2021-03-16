<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ServiceAccount>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Service Accounts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
        <div id="banner"><div id="banner_text">Service Account</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View Service Account</th> 
		        </tr> 
                <tr>
                    <td>Service Account ID</td>
                    <td><%= Html.Encode(Model.ServiceAccountId)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Service Account Name</td>
                    <td><%= Html.Encode(Model.ServiceAccountName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Last Name</td>
                    <td><%= Html.Encode(Model.LastName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>First Name</td>
                    <td><%= Html.Encode(Model.FirstName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Email</td>
                    <td><%= Html.Encode(Model.Email)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Is Active?</td>
                    <td><%= Html.Encode(Model.IsActiveFlag.HasValue && Model.IsActiveFlag.Value == true ? "True" : "False")%></td>
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
                    <td>Robotic User</td>
                    <td><%=Html.Encode(Model.RoboticUserFlag.HasValue && Model.RoboticUserFlag.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>CWT Manager</td>
                    <td><%= Html.Encode(Model.CWTManager)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>User Type</td>
                    <td><%= Html.Encode(Model.ThirdPartyUserType)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Deleted?</td>
                    <td><%= Html.Encode(Model.DeletedFlag.HasValue && Model.DeletedFlag.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Deleted Date Time</td>
                    <td><%= Html.Encode(Model.DeletedDateTime.HasValue ? Model.DeletedDateTime.Value.ToShortDateString() : "No Deleted Date")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Internal Remarks</td>
                    <td colspan="2">
						<% if (Model.ServiceAccountInternalRemarks != null && Model.ServiceAccountInternalRemarks.Count > 0) { %>
							<dl class="InternalRemarks">
								<% foreach (CWTDesktopDatabase.Models.ServiceAccountInternalRemark item in Model.ServiceAccountInternalRemarks){ %>
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
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right" colspan="2"></td>
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
<% if(Model.DeletedFlag == true) { %>
	<%=Html.RouteLink("Service Accounts (Deleted)", "Main", new { controller = "ServiceAccount", action = "ListUnDeleted", }, new { title = "Service Accounts (Deleted)" })%> &gt;
<% } else { %>
	<%=Html.RouteLink("Service Accounts", "Main", new { controller = "ServiceAccount", action = "ListUnDeleted", }, new { title = "Service Accounts" })%> &gt;
<% } %>>
<%= Html.Encode(Model.ServiceAccountName)%>, <%= Html.Encode(Model.ServiceAccountId)%> &gt;
View
</asp:Content>