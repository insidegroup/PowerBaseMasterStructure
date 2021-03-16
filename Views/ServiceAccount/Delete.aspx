<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ServiceAccountVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Service Accounts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Service Accounts</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Service Account</th> 
		        </tr> 
               <tr>
                    <td>Service Account ID</td>
                    <td><%= Html.Encode(Model.ServiceAccount.ServiceAccountId)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Service Account Name</td>
                    <td><%= Html.Encode(Model.ServiceAccount.ServiceAccountName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Last Name</td>
                    <td><%= Html.Encode(Model.ServiceAccount.LastName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>First Name</td>
                    <td><%= Html.Encode(Model.ServiceAccount.FirstName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Email</td>
                    <td><%= Html.Encode(Model.ServiceAccount.Email)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Is Active?</td>
                    <td><%= Html.Encode(Model.ServiceAccount.IsActiveFlag.HasValue && Model.ServiceAccount.IsActiveFlag.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Cuba Booking Allowed</td>
                    <td><%=Html.Encode(Model.ServiceAccount.CubaBookingAllowanceIndicator.HasValue && Model.ServiceAccount.CubaBookingAllowanceIndicator.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Military and Government User</td>
                    <td><%=Html.Encode(Model.ServiceAccount.MilitaryAndGovernmentUserFlag.HasValue && Model.ServiceAccount.MilitaryAndGovernmentUserFlag.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Robotic User</td>
                    <td><%=Html.Encode(Model.ServiceAccount.RoboticUserFlag.HasValue && Model.ServiceAccount.RoboticUserFlag.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>CWT Manager</td>
                    <td><%= Html.Encode(Model.ServiceAccount.CWTManager)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>User Type</td>
                    <td><%= Html.Encode(Model.ServiceAccount.ThirdPartyUserType)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Deleted?</td>
                    <td><%= Html.Encode(Model.ServiceAccount.DeletedFlag.HasValue && Model.ServiceAccount.DeletedFlag.Value == true ? "True" : "False")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Deleted Date Time</td>
                    <td><%= Html.Encode(Model.ServiceAccount.DeletedDateTime.HasValue ? Model.ServiceAccount.DeletedDateTime.Value.ToShortDateString() : "No Deleted Date")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Internal Remarks</td>
                    <td colspan="2">
						<% if (Model.ServiceAccount.ServiceAccountInternalRemarks != null && Model.ServiceAccount.ServiceAccountInternalRemarks.Count > 0) { %>
							<dl class="InternalRemarks">
								<% foreach (CWTDesktopDatabase.Models.ServiceAccountInternalRemark item in Model.ServiceAccount.ServiceAccountInternalRemarks){ %>
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
							<%= Html.HiddenFor(model => model.ServiceAccount.ServiceAccountId) %>
							<%= Html.HiddenFor(model => model.ServiceAccount.VersionNumber) %>
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
<%=Html.RouteLink("Service Accounts", "Main", new { controller = "ServiceAccount", action = "ListUnDeleted", }, new { title = "Service Accounts" })%> &gt;
<%= Html.Encode(Model.ServiceAccount.ServiceAccountName)%>, <%= Html.Encode(Model.ServiceAccount.ServiceAccountId)%> &gt;
Delete
</asp:Content>