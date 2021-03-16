<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSThirdPartyVendorVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Third Party Vendor
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Third Party Vendor</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Third Party Vendor</th> 
		        </tr> 
				<tr>
                    <td><label for="GDSThirdPartyVendor_GDSThirdPartyVendorName">Third Party Vendor Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSThirdPartyVendor.GDSThirdPartyVendorName)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>

				<% if(Model.AllowDelete == false) { %>
					
					<tr>
						<td colspan="3">You cannot delete this Third Party Vendor until all items referencing it are removed:</td>
					</tr>

					<% foreach(CWTDesktopDatabase.Models.GDSThirdPartyVendorReference thirdPartyVendorReference in Model.GDSThirdPartyVendorReferences) { %>
						<tr>
							<td colspan="3"><strong>Attached <%=thirdPartyVendorReference.TableName %></strong></td>
						</tr>
					<%}%>
				<%}%>

                <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right">
						<% if(Model.AllowDelete) { %>
							<% using (Html.BeginForm()) { %>
								<%= Html.AntiForgeryToken() %>
								<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
								<%= Html.HiddenFor(model => model.GDSThirdPartyVendor.GDSThirdPartyVendorId) %>
								<%= Html.HiddenFor(model => model.GDSThirdPartyVendor.VersionNumber) %>
							<%}%>
						<%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_admin, #menu_admin_gdsmanagement').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
	})
</script>
</asp:Content>