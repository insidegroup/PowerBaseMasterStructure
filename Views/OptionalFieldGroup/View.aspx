<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">View Optional Field Groups</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">View <%= Html.Encode(Model.OptionalFieldGroup.OptionalFieldGroupName)%></th> 
		    </tr> 
            <tr>
                <td>Optional Field Group Name</td>
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
                <td>Hierarchy Item</td>
                <td><%= Html.Encode(Model.OptionalFieldGroup.HierarchyItem)%></td>
                <td></td>
            </tr>
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>                    
                <td class="row_footer_blank_right"></td>
            </tr>
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        //Navigation
    	$('#menu_passivesegmentbuilder').click();
    	$('#menu_passivesegmentbuilder_optionalfields').click();
	    $('#breadcrumb').css('width', 'auto');
    	$("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Optional Field Groups", "Main", new { controller = "OptionalFieldGroup", action = "ListUnDeleted" }, new { title = "Optional Field Groups" })%> &gt;
<%=Model.OptionalFieldGroup.OptionalFieldGroupName%>
</asp:Content>

