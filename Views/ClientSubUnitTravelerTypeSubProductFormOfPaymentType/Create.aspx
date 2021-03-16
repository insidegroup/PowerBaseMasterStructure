<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitTravelerTypeSubProductFormOfPaymentTypeVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit TravelerType - Client Detail SubProduct</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create SubProduct FormOfPaymentType</th> 
		        </tr> 
                <tr>
                    <td><label for="ClientDetailSubProductFormOfPaymentType_SubProductId">Sub Product</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientDetailSubProductFormOfPaymentType.SubProductId, Model.SubProducts, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDetailSubProductFormOfPaymentType.SubProductId)%></td>
                </tr>              
                     <tr>
                    <td><label for="ClientDetailSubProductFormOfPaymentType_FormOfPaymentTypeId">Form Of PaymentType</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientDetailSubProductFormOfPaymentType.FormOfPaymentTypeId, Model.FormOfPaymentTypes, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDetailSubProductFormOfPaymentType.FormOfPaymentTypeId)%></td>
                </tr>              
         
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create SubProduct FormOfPaymentType" class="red" title="Create SubProduct FormOfPaymentType"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ClientDetail.ClientDetailId) %>
    <% } %>


    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>