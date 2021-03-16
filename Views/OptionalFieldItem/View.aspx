<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
<div id="banner"><div id="banner_text">Optional Field Item</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">View Optional Field Item</th> 
		    </tr> 
            <tr>
                <td>Group Name</td>
                <td><%= Html.Encode(Model.OptionalFieldItem.OptionalFieldGroup.OptionalFieldGroupName)%></td>
                <td></td>
            </tr>
            <tr>
                <td>Optional Field Name</td>
                <td><%= Html.Encode(Model.OptionalFieldItem.OptionalField.OptionalFieldName)%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Product Type</td>
                <td><%= Html.Encode(Model.OptionalFieldItem.ProductName)%></td>
                <td></td>
            </tr>       
            <tr>
                <td>Supplier Code</td>
                <td><%= Html.Encode(Model.OptionalFieldItem.SupplierName)%></td>
                <td></td>
            </tr>
			<tr>
                <td>Mandatory</td>
                <td><%= Html.Encode(Model.OptionalFieldItem.Mandatory)%></td>
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
    $(document).ready(function () {
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
<%=Html.RouteLink(Model.OptionalFieldGroup.OptionalFieldGroupName, "Main", new { controller = "OptionalFieldGroup", action = "View", id = Model.OptionalFieldGroup.OptionalFieldGroupName })%> &gt;
Optional Field Group Items
</asp:Content>

