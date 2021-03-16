<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldItemOrderSelectionVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Groups</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Optional Field Order</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(true); %>
         <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
          <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Optional Field Order</th> 
		        </tr>                 
                <tr>
                    <td><label for="OptionalFieldTypeId">Optional Field to Edit Order</label></td>
                    <td><%= Html.DropDownListFor(model => model.ProductId, ViewData["Products"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ProductId)%></td>
                </tr>  
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Continue" title="Continue" class="red"/></td>
                </tr>
             </table>
             <%= Html.HiddenFor(model => model.OptionalFieldGroup.OptionalFieldGroupId)%>
    <% } %>
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
<%=Html.RouteLink(Model.OptionalFieldGroup.OptionalFieldGroupName, "Main", new { controller = "OptionalFieldGroup", action = "View", id = Model.OptionalFieldGroup.OptionalFieldGroupId })%> &gt;
<%=Html.RouteLink("Optional Field Group Items", "Main", new { controller = "OptionalFieldItem", action = "List", id = Model.OptionalFieldGroup.OptionalFieldGroupId })%> &gt;
Order
</asp:Content>