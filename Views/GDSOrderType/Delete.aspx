<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSOrderTypeVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Order Type
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Order Type</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete GDS Order Type</th> 
		        </tr>
				<tr>
                    <td><label for="GDSOrderType_GDSOrderTypeName">GDS Order Type</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderType.GDSOrderTypeName)%></td>
                </tr>
				<tr>
                    <td>
						<label for="GDSOrderType_AbacusFlag" class="GDSOrderTypeLabel">Abacus (1B)</label>
						<label for="GDSOrderType_GDS" class="GDSOrderTypeLabelGDS">GDS</label>
                    </td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderType.AbacusFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_AllGDSSystemsFlag" class="GDSOrderTypeLabel">All GDS Systems (ALL)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderType.AllGDSSystemsFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_AmadeusFlag" class="GDSOrderTypeLabel">Amadeus (1A)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderType.AmadeusFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_ApolloFlag" class="GDSOrderTypeLabel">Apollo (1V)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderType.ApolloFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_EDSFlag" class="GDSOrderTypeLabel">EDS (1C)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderType.EDSFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_GalileoFlag" class="GDSOrderTypeLabel">Galileo (1V)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderType.GalileoFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_RadixxFlag" class="GDSOrderTypeLabel">Radixx (1D)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderType.RadixxFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_SabreFlag" class="GDSOrderTypeLabel">Sabre (1S)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderType.SabreFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_TravelskyFlag" class="GDSOrderTypeLabel">Travelsky (1E)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderType.TravelskyFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_WorldspanFlag" class="GDSOrderTypeLabel">Worldspan (1P)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderType.WorldspanFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_IsThirdPartyFlag">3<sup>rd</sup> Party?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderType.IsThirdPartyFlagNullable)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>

				<% if(Model.AllowDelete == false) { %>
					
					<tr>
						<td colspan="3">You cannot delete this GDSOrderType until all items referencing it are removed:</td>
					</tr>

					<% foreach(CWTDesktopDatabase.Models.GDSOrderTypeReference gdsOrderTypeReference in Model.GDSOrderTypeReferences) { %>
						<tr>
							<td colspan="3"><strong>Attached <%=gdsOrderTypeReference.TableName %></strong></td>
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
								<%= Html.HiddenFor(model => model.GDSOrderType.GDSOrderTypeId) %>
								<%= Html.HiddenFor(model => model.GDSOrderType.VersionNumber) %>
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