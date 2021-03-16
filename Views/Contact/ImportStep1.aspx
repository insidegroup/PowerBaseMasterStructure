<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitContactImportStep1WithFileVM>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Client SubUnits - Contacts
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnits - Contacts</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>
     
    <form action="" method="post" enctype="multipart/form-data">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Import Contacts</th> 
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
        <%= Html.HiddenFor(model => model.ClientSubUnitGuid) %>     
    </form>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');
    });
</script>

</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName.ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid.ToString() }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName.ToString() })%> &gt;
<%=Html.RouteLink("Client SubUnits", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid.ToString() }, new { title = "Client SubUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName.ToString(), "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid.ToString() }, new { title = Model.ClientSubUnit.ClientSubUnitName.ToString() })%> &gt;
<%=Html.RouteLink("Contacts", "Main", new { controller = "Contact", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid.ToString() }, new { title = "Contacts" })%> &gt;
Import
</asp:Content>