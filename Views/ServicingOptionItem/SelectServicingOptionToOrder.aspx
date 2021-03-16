<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ServicingOptionHFLFVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Servicing Option Groups
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Servicing Option Order</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Servicing Option Order</th> 
		        </tr>  
                <tr>
                    <td><label for="ServicingOptionId">Servicing Option to Edit Order</label></td>
                    <td><%= Html.DropDownListFor(model => model.ServicingOptionId, Model.ServicingOptions, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServicingOptionId)%></td>
                </tr>  
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="50%" class="row_footer_centre"></td>
                    <td width="20%" class="row_footer_right"></td>
                </tr>
               <tr>
                   <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td> 
                   <td class="row_footer_blank_right"><input type="submit" value="Continue" title="Continue" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ServicingOptionGroup.ServicingOptionGroupId)%>
    <% } %>

    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_servicingoptions').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })


 </script>
 </asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Servicing Option Groups", "Main", new { controller = "ServicingOptionGroup", action = "ListUnDeleted", }, new { title = "Servicing Option Groups" })%> &gt;
<%=Html.RouteLink(Model.ServicingOptionGroup.ServicingOptionGroupName, "Default", new { controller = "ServicingOptionGroup", action = "View", id = Model.ServicingOptionGroup.ServicingOptionGroupId }, new { title = Model.ServicingOptionGroup.ServicingOptionGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "ServicingOptionItem", action = "List", id = Model.ServicingOptionGroup.ServicingOptionGroupId }, new { title = "Servicing Option Items" })%> &gt;
Order
</asp:Content>