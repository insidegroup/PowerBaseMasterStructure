<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ApprovalGroupApprovalTypeVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Approval Type</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Delete Approval Type</th> 
		        </tr> 
                <tr>
                    <td>Approval Type ID</td>
                    <td><%= Html.Encode(Model.ApprovalGroupApprovalType.ApprovalGroupApprovalTypeId)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Approval Type</td>
                    <td><%= Html.Encode(Model.ApprovalGroupApprovalType.ApprovalGroupApprovalTypeDescription)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <% if(Model.AllowDelete == false) { %>
					
					<tr>
						<td colspan="3">You cannot delete this Approval Type until all items referencing it are removed:</td>
					</tr>

					<% foreach (CWTDesktopDatabase.Models.ApprovalGroupApprovalTypeReference approvalGroupApprovalTypeReference in Model.ApprovalGroupApprovalTypeReferences){ %>
						<tr>
							<td colspan="3"><strong>Attached <%=approvalGroupApprovalTypeReference.TableName %></strong></td>
						</tr>
					<%}%>
								
				<%}%>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2">
                        <% if(Model.AllowDelete) { %>
                            <% using (Html.BeginForm()) { %>
                                <%= Html.AntiForgeryToken() %>
                                <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                                <%= Html.HiddenFor(model => model.ApprovalGroupApprovalType.ApprovalGroupApprovalTypeId) %>
                                <%= Html.HiddenFor(model => model.ApprovalGroupApprovalType.VersionNumber) %>
                            <%}%>
                        <%}%>
                    </td>
               </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_admin').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>