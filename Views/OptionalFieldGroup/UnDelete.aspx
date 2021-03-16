<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Delete Optional Field Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Delete Optional Field Groups</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">UnDelete Optional Field Group</th> 
		        </tr> 
                  <tr>
                     <td>Group Name</td>
                    <td><%= Html.Encode(Model.OptionalFieldGroup.OptionalFieldGroupName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Enabled?</td>
                    <td><%= Html.Encode(Model.OptionalFieldGroup.EnabledFlag)%></td>
                    <td></td>
                </tr>  
               <tr>
                    <td>Enabled Date</td>
                    <td><%= Html.Encode(Model.OptionalFieldGroup.EnabledDate.HasValue ? Model.OptionalFieldGroup.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Expiry Date</td>
                    <td><%= Html.Encode(Model.OptionalFieldGroup.ExpiryDate.HasValue ? Model.OptionalFieldGroup.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                    <td></td>
                </tr>  
               <tr>
                    <td>Deleted?</td>
                    <td><%= Html.Encode(Model.OptionalFieldGroup.DeletedFlag)%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td>Deleted Date/Time</td>
                    <td><%= Html.Encode(Model.OptionalFieldGroup.DeletedDateTime.HasValue ? Model.OptionalFieldGroup.DeletedDateTime.Value.ToString("MMM dd, yyyy") : "No Deleted Date")%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Hierarchy Type</td>
                    <td><%= Html.Encode(Model.OptionalFieldGroup.HierarchyType)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><%= Html.Label(Model.OptionalFieldGroup.HierarchyItem)%> </td>
                    <td><%= Html.Encode(Model.OptionalFieldGroup.HierarchyItem)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm UnDelete" title="Confirm UnDelete" class="red"/>
                        <%= Html.HiddenFor(model => model.OptionalFieldGroup.OptionalFieldGroupId)%>
                        <%= Html.HiddenFor(model => model.OptionalFieldGroup.VersionNumber)%>
                    <%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
		$('#menu_passivesegmentbuilder').click();
		$('#menu_passivesegmentbuilder_optionalfields').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

