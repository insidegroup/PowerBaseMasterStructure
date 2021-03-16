<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TeamOutOfOfficeGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Team Out of Office Group
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Team Out of Office Group</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Delete Team Out of Office Group</th> 
		        </tr> 
                <tr>
                    <td>Group Name</td>
                    <td colspan="2"><%= Html.Encode(Model.TeamOutOfOfficeGroupName)%></td>
                </tr> 
                <tr>
                    <td>Enabled?</td>
                    <td><%= Html.Encode(Model.EnabledFlag)%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td>Enabled Date</td>
                    <td><%= Html.Encode(Model.EnabledDate.HasValue ? Model.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Expiry Date</td>
                    <td><%= Html.Encode(Model.ExpiryDate.HasValue ? Model.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                    <td></td>
				</tr>
                <tr>
                    <td><label for="DeletedFlag">Deleted?</label></td>
                    <td><%= Html.Encode(Model.DeletedFlag == true ? "True" : "False")%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td><label for="DeletedDateTime">Deleted Date/Time</label></td>
                    <td><%= Html.Encode(Model.DeletedDateTime.HasValue ? string.Format("{0} {1}", Model.DeletedDateTime.Value.ToShortDateString(), Model.DeletedDateTime.Value.ToLongTimeString()) : "No Deleted Date")%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td><label for="HierarchyType">Hierarchy Type</label> </td>
                    <td><%= Html.Encode(Model.HierarchyType.Replace("SubUnit", " SubUnit"))%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Hierarchy Item</td>
                    <td colspan="2"><%= Html.Encode(Model.HierarchyItem)%> <%= Html.Encode(Model.HierarchyCode)%>, <%= Html.Encode(Model.ClientTopUnitName) %></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="45%" class="row_footer_centre"></td>
                    <td width="25%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.VersionNumber) %>
                    <%}%>
                    </td>                
               </tr>
            </table>
        </div>   
</div>
<script type="text/javascript">
    $(document).ready(function() {
    	$('#teamoutofofficegroups').click();
	    $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Team Out of Office Groups", "Main", new { controller = "TeamOutOfOfficeGroup", action = "ListUnDeleted", }, new { title = "Team Out of Office Groups" })%> &gt;
<%=Model.TeamOutOfOfficeGroupName%>
</asp:Content>