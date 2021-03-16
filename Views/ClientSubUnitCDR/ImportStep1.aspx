<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.CDRImportStep1WithFileVM>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Clients
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Sub Units - CDR Link</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>
     
    <form action="" method="post" enctype="multipart/form-data">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Create CDR Link</th> 
		    </tr>  
            <tr>
                <td valign="top"><label for="ClientSubUnitClientDefinedReference_Value">CDR Value</label></td>
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
                <td class="row_footer_blank_right"><input type="submit" value="Create CDR Link" title="Create CDR Link" class="red"/></td>
            </tr>
        </table>
        <%= Html.HiddenFor(model => model.ClientSubUnitGuid) %>
        <%= Html.HiddenFor(model => model.ClientDefinedReferenceItemId) %>
        <%= Html.HiddenFor(model => model.DisplayName) %>
		<%= Html.HiddenFor(model => model.RelatedToDisplayName) %>        
    </form>
    </div>
</div>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Client Sub Units", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client Sub Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.RouteLink("CDR Links", "Main", new {action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "CDR Links" })%>
</asp:Content>