<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PseudoCityOrOfficeDefinedRegionVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Pseudo City/Office ID Defined Region
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Pseudo City/Office ID Defined Region</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Pseudo City/Office ID Defined Region</th> 
		        </tr> 
                <tr>
                    <td><label for="PseudoCityOrOfficeDefinedRegion_GlobalRegionName">Global Region</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeDefinedRegion.GlobalRegion.GlobalRegionName)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeDefinedRegion_PseudoCityOrOfficeDefinedRegionName">Pseudo City/Office ID Defined Region</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionName)%></td>
                </tr>  
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
				<% if(Model.AllowDelete == false) { %>
					
					<tr>
						<td colspan="3">You cannot delete this Pseudo City/Office ID Defined Region until all items referencing it are removed:</td>
					</tr>

					<% foreach(CWTDesktopDatabase.Models.PseudoCityOrOfficeDefinedRegionReference pseudoCityOrOfficeDefinedRegionReference in Model.PseudoCityOrOfficeDefinedRegionReferences) { %>
						<tr>
							<td colspan="3"><strong>Attached <%=pseudoCityOrOfficeDefinedRegionReference.TableName %></strong></td>
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
								<%= Html.HiddenFor(model => model.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionId) %>
								<%= Html.HiddenFor(model => model.PseudoCityOrOfficeDefinedRegion.VersionNumber) %>
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