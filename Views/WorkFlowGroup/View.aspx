<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.WorkFlowGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - WorkFlow Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">WorkFlow Groups</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">View WorkFlow Group</th> 
		    </tr> 
            <tr>
                <td>Workflow Group Name</td>
                <td><%= Html.Encode(Model.WorkFlowGroupName)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Workflow Type</td>
                <td><%= Html.Encode(Model.WorkFlowType.WorkFlowTypeDescription)%></td>
                <td></td>
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
                <td>Trip Type</td>
                <td><%= Html.Encode(Model.TripType.TripTypeDescription)%></td>
                <td></td>
            </tr> 
            <% if (Model.HierarchyType == "ClientSubUnitTravelerType"){ %>
                
            <tr>
                <td>Client Sub Unit Name</td>
                <td><%= Html.Encode(Model.ClientSubUnitName)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Traveler Type</td>
                <td><%= Html.Encode(Model.TravelerTypeName)%></td>
                <td></td>
            </tr> <%}else{ %>
            <tr>
                <td><%= Html.Encode(Model.HierarchyType)%> </td>
                <td><%= Html.Encode(Model.HierarchyItem)%></td>
                <td></td>
            </tr> 
            <%}%>
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                <td class="row_footer_blank_right" colspan="2"></td>
            </tr>
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_workflowgroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

