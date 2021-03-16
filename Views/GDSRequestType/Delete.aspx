<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSRequestTypeVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Request Type
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Request Type</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete GDS Request Type</th> 
		        </tr> 
                <tr>
                    <td><label for="GDSRequestType_GDSRequestType1">GDS Request Type</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSRequestType.GDSRequestTypeName)%></td>
                </tr> 
                 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>

				<% if(Model.AllowDelete == false) { %>
					
					<tr>
						<td colspan="3">You cannot delete this GDSRequestType until all items referencing it are removed:</td>
					</tr>

					<% foreach(CWTDesktopDatabase.Models.GDSRequestTypeReference gdsRequestTypeReference in Model.GDSRequestTypeReferences) { %>
						<tr>
							<td colspan="3"><strong>Attached <%=gdsRequestTypeReference.TableName %></strong></td>
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
								<%= Html.HiddenFor(model => model.GDSRequestType.GDSRequestTypeId) %>
								<%= Html.HiddenFor(model => model.GDSRequestType.VersionNumber) %>
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