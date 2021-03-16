<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyCityImportStep1WithFileVM>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Policy City Group
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy City Group Items</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>
     
    <form action="" method="post" enctype="multipart/form-data">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Import Policy City Group Items</th> 
		    </tr>  
            <tr>
                <td valign="top"><label for="ClientSubUnitClientDefinedReference_Value">Find Data File to Import</label></td>
                <td valign="top"><input type="file" name="file" id="file"/><span class="error"> *</span></td>
                <td valign="top"> <%= Html.ValidationMessageFor(model => model.File)%><label id="lblClientSubUnitClientDefinedReferenceMsg"></label></td>
            </tr> 
            <tr>
                <td width="25%" class="row_footer_left"></td>
                <td width="50%" class="row_footer_centre"></td>
                <td width="25%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right"><input type="submit" value="Submit" title="Submit" class="red"/></td>
            </tr>
        </table>
        <%= Html.HiddenFor(model => model.PolicyGroupId) %>     
    </form>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_policies').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');
    });
</script>

</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroup.PolicyGroupName + " Items", "Main", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName + " Items" })%> &gt;
<%=Html.RouteLink("Policy City Group Items", "Main", new { controller = "PolicyCityGroupItem", action = "List", id = Model.PolicyGroupId }, new { title = "Policy City Group Items" })%> &gt;
<%=Html.RouteLink("Import", "Main", new { controller = "PolicyCityGroupItem", action = "ImportStep1", id = Model.PolicyGroupId }, new { title = "Import" })%>
</asp:Content>