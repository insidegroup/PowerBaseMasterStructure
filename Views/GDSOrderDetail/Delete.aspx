<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSOrderDetailVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Order Detail
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Order Detail</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete GDS Order Detail</th> 
		        </tr>
				<tr>
                    <td><label for="GDSOrderDetail_GDSOrderDetailName">GDS Order Detail</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderDetail.GDSOrderDetailName)%></td>
                </tr>
				<tr>
                    <td>
						<label for="GDSOrderDetail_AbacusFlag" class="GDSOrderDetailLabel">Abacus (1B)</label>
						<label for="GDSOrderDetail_GDS" class="GDSOrderDetailLabelGDS">GDS</label>
                    </td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderDetail.AbacusFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_AllGDSSystemsFlag" class="GDSOrderDetailLabel">All GDS Systems (ALL)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderDetail.AllGDSSystemsFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_AmadeusFlag" class="GDSOrderDetailLabel">Amadeus (1A)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderDetail.AmadeusFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_ApolloFlag" class="GDSOrderDetailLabel">Apollo (1V)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderDetail.ApolloFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_EDSFlag" class="GDSOrderDetailLabel">EDS (1C)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderDetail.EDSFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_GalileoFlag" class="GDSOrderDetailLabel">Galileo (1V)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderDetail.GalileoFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_RadixxFlag" class="GDSOrderDetailLabel">Radixx (1D)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderDetail.RadixxFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_SabreFlag" class="GDSOrderDetailLabel">Sabre (1S)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderDetail.SabreFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_TravelskyFlag" class="GDSOrderDetailLabel">Travelsky (1E)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderDetail.TravelskyFlagNullable)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_WorldspanFlag" class="GDSOrderDetailLabel">Worldspan (1P)</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSOrderDetail.WorldspanFlagNullable)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>

				<% if(Model.AllowDelete == false) { %>
					
					<tr>
						<td colspan="3">You cannot delete this GDSOrderDetail until all items referencing it are removed:</td>
					</tr>

					<% foreach(CWTDesktopDatabase.Models.GDSOrderDetailReference gdsOrderDetailReference in Model.GDSOrderDetailReferences) { %>
						<tr>
							<td colspan="3"><strong>Attached <%=gdsOrderDetailReference.TableName %></strong></td>
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
								<%= Html.HiddenFor(model => model.GDSOrderDetail.GDSOrderDetailId) %>
								<%= Html.HiddenFor(model => model.GDSOrderDetail.VersionNumber) %>
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