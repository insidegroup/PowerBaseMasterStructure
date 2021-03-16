<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSOrderVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Orders
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Orders</div></div>
    <div id="content">
 		<% Html.EnableClientValidation(); %>
		<% Html.EnableUnobtrusiveJavaScript(); %>
		<%using (Html.BeginForm("Email", null, FormMethod.Post, new { id = "form0", autocomplete="off" })){%>
			<%= Html.AntiForgeryToken() %>
			<%= Html.ValidationSummary(true) %>
		   <table cellpadding="0" cellspacing="0" width="100%"> 
				<tr> 
					<th class="row_header" colspan="3">Email GDS Order</th> 
				</tr>
			   <tr>
					<td><label for="EmailFromAddress">From</label></td>
					<td colspan="2"><%= Html.Encode(Model.GDSOrder.EmailFromAddress)%></td>
				</tr>
				<tr>
					<td><label for="EmailToAddress">To</label></td>
					<td colspan="2">
                        <%= Html.TextAreaFor(model => model.GDSOrder.EmailToAddress)%> <span class="error"> *</span>
                        <span id="GDSOrder_EmailToAddress_Validation" class="error">To field is required</span>
                    </td>
				</tr>
				<tr>
					<td width="30%" class="row_footer_left"></td>
					<td width="50%" class="row_footer_centre"></td>
					<td width="20%" class="row_footer_right"></td>
				</tr>
				<tr>
					<td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
					<td class="row_footer_blank_right"><input type="submit" value="Send GDS Order Email" title="Send GDS Order Email" class="red"/></td>
				</tr>
			</table>
  		    <%= Html.HiddenFor(model => model.GDSOrder.GDSOrderId) %>
		<% } %>
  </div>
</div>

<script type="text/javascript">
$(document).ready(function() {

    $('#menu_gdsmanagement').click();
	$("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    $('#GDSOrder_EmailToAddress_Validation').hide();

    $('#form0').submit(function() {

        $('#GDSOrder_EmailToAddress_Validation').hide();
        $('#GDSOrder_EmailToAddress').removeClass("input-validation-error");

        var emailToAddress = $('#GDSOrder_EmailToAddress').val();

        if (emailToAddress == '') {
            $('#GDSOrder_EmailToAddress_Validation').show();
            $('#GDSOrder_EmailToAddress').addClass("input-validation-error");

            return false;
        }

        if (!$(this).valid()) {
            return false;
        }

    });
})
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("GDS Orders", "Main", new { controller = "GDSOrder", action = "List", }, new { title = "GDS Orders" })%> &gt;
Email &gt;
<% =Html.Encode(Model.GDSOrder.PseudoCityOrOfficeId) %>
</asp:Content>