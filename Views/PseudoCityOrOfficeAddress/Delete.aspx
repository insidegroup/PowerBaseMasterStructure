<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PseudoCityOrOfficeAddressVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Pseudo City/Office ID Address
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Pseudo City/Office ID Address</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Pseudo City/Office ID Address</th> 
		        </tr> 
                <tr>
                    <td><label for="PseudoCityOrOfficeAddress_FirstAddressLine">First Address Line</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeAddress.FirstAddressLine)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeAddress_SecondAddressLine">Second Address Line</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeAddress.SecondAddressLine)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeAddress_CityName">City</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeAddress.CityName)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeAddress_Country_CountryName">Country</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeAddress.Country.CountryName)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeAddress_StateProvinceName">State/Province</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeAddress.StateProvinceName)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeAddress_PostalCode">Postal Code</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeAddress.PostalCode)%></td>
                </tr> 
				<tr>
					<td width="30%" class="row_footer_left"></td>
					<td width="40%" class="row_footer_centre"></td>
					<td width="30%" class="row_footer_right"></td>
				</tr>
				<% if(Model.AllowDelete == false) { %>
					
					<tr>
						<td colspan="3">You cannot delete this PseudoCityOrOfficeAddress until all items referencing it are removed:</td>
					</tr>

					<% foreach(CWTDesktopDatabase.Models.PseudoCityOrOfficeAddressReference pseudoCityOrOfficeAddressReference in Model.PseudoCityOrOfficeAddressReferences) { %>
						<tr>
							<td colspan="3"><strong>Attached <%=pseudoCityOrOfficeAddressReference.TableName %></strong></td>
						</tr>
					<%}%>
				<%}%>
				<tr>
					<td width="30%" class="row_footer_left"></td>
					<td width="40%" class="row_footer_centre"></td>
					<td width="30%" class="row_footer_right"></td>
				</tr>
                <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right">
						<% if(Model.AllowDelete) { %>
							<% using (Html.BeginForm()) { %>
								<%= Html.AntiForgeryToken() %>
								<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
								<%= Html.HiddenFor(model => model.PseudoCityOrOfficeAddress.PseudoCityOrOfficeAddressId) %>
								<%= Html.HiddenFor(model => model.PseudoCityOrOfficeAddress.VersionNumber) %>
							<%}%>
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
	})
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("Pseudo City/Office ID Address", "Main", new { controller = "PseudoCityOrOfficeAddress", action = "List", }, new { title = "Pseudo City/Office ID Address" })%> &gt;
Delete
</asp:Content>