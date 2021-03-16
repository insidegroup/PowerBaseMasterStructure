<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.CommissionableRouteItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Commissionable Route Item
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/CommissionableRouteItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Commissionable Route Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
		<% Html.EnableUnobtrusiveJavaScript(); %>
        <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Commissionable Route Item</th> 
		        </tr> 
                <tr>
                    <td><label for="CommissionableRouteItem_CommissionableRouteGroup_CommissionableRouteGroupName">Commissionable Route Group</label></td>
                    <td colspan="2"><%= Html.Encode(ViewData["CommissionableRouteGroupName"])%></td>
                </tr>
                <tr>
                    <td><label for="CommissionableRouteItem_ProductId">Product</label></td>
                    <td><%= Html.DropDownListFor(model => model.CommissionableRouteItem.ProductId, ViewData["ProductList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CommissionableRouteItem.ProductId)%></td>
                </tr> 
                <tr>
                    <td><label for="CommissionableRouteItem_SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.CommissionableRouteItem.SupplierName)%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.CommissionableRouteItem.SupplierName)%>
                        <%= Html.HiddenFor(model => model.CommissionableRouteItem.SupplierCode)%>
                        <label id="lblSupplierNameMsg"/>
                    </td>
                </tr>
				<tr>
                    <td><label for="CommissionableRouteItem_TravelIndicator">Travel Indicator</label></td>
                    <td><%= Html.DropDownListFor(model => model.CommissionableRouteItem.TravelIndicator, ViewData["TravelIndicatorList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CommissionableRouteItem.TravelIndicator)%></td>
                </tr>
				<tr>
                    <td><label for="CommissionableRouteItem_ClassOfTravel">Class of Service</label></td>
                    <td><%= Html.TextBoxFor(model => model.CommissionableRouteItem.ClassOfTravel, new { Value = "*", maxlength = "2" }) %><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CommissionableRouteItem.ClassOfTravel)%></td>
                </tr>
				<tr>
                    <td><label for="CommissionableRouteItem_CommissionAmount">Commission Amount</label></td>
                    <td><%= Html.EditorFor(model => model.CommissionableRouteItem.CommissionAmount) %></td>
                    <td><%= Html.ValidationMessageFor(model => model.CommissionableRouteItem.CommissionAmount)%></td>
                </tr>
				<tr>
                    <td><label for="CommissionableRouteItem_CommissionAmountCurrencyCode">Commission Currency</label></td>
                    <td><%= Html.DropDownListFor(model => model.CommissionableRouteItem.CommissionAmountCurrencyCode, ViewData["CommissionAmountCurrencyCodeList"] as SelectList, "Please Select...")%></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.CommissionableRouteItem.CommissionAmountCurrencyCode)%>
						<span id="CommissionAmountCurrencyCodeError" class="error"></span>
                    </td>
                </tr>
				<tr>
                    <td><label for="CommissionableRouteItem_CommissionOnTaxes">Commission %</label></td>
                    <td><%= Html.TextBoxFor(model => model.CommissionableRouteItem.BSPCommission) %></td>
                    <td><%= Html.ValidationMessageFor(model => model.CommissionableRouteItem.BSPCommission)%></td>
                </tr>
				<tr>
                    <td><label for="CommissionableRouteItem_CommissionOnTaxes">Commission % on Tax</label></td>
                    <td><%= Html.TextBoxFor(model => model.CommissionableRouteItem.CommissionOnTaxes, new { maxlength = "80" }) %></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.CommissionableRouteItem.CommissionOnTaxes)%>
						<span id="CommissionOnTaxesError" class="error"></span>	
					</td>
                </tr>
				<tr>
                    <td><label for="CommissionableRouteItem_CommissionOnTaxes">Commission Tax Code</label></td>
                    <td><%= Html.TextBoxFor(model => model.CommissionableRouteItem.CommissionableTaxCodes)%></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.CommissionableRouteItem.CommissionableTaxCodes)%>
						<span id="CommissionableTaxCodesError" class="error"></span>	
                    </td>
                </tr>
				<tr valign="top">
                    <td><label for="CommissionableRouteItem_NegotiatedFareFlag">Include on Negotiated Fare?</label></td>
                    <td><%= Html.CheckBox("CommissionableRouteItem.NegotiatedFareFlag", (Model.CommissionableRouteItem.NegotiatedFareFlag != null && Model.CommissionableRouteItem.NegotiatedFareFlag != false)) %></td>
                    <td><%= Html.ValidationMessageFor(model => model.CommissionableRouteItem.NegotiatedFareFlag)%></td>
                </tr>
				<tr valign="top">
                    <td><label for="CommissionableRouteItem_RemarksOrRoute">Remarks</label></td>
                    <td><%= Html.TextAreaFor(model => model.CommissionableRouteItem.RemarksOrRoute, new { maxlength = "350" }) %></td>
                    <td><%= Html.ValidationMessageFor(model => model.CommissionableRouteItem.RemarksOrRoute)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                 <tr>
                    <td class="row_footer_blank" colspan="3"></td>
                </tr>
                <tr> 
			        <th class="row_header" colspan="3">Policy Routing</th> 
		        </tr> 
		         <tr>
                    <td>Routing Name</td>
                    <td><label id="lblAuto"></label></td>
                    <td><%= Html.HiddenFor(model => model.PolicyRouting.Name)%><label id="lblPolicyRoutingNameMsg"/></td>
                </tr>    
               <tr>
                    <td><label for="PolicyRouting_FromGlobalFlag">From Global?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
                </tr> 
               <tr>
                    <td><label for="PolicyRouting_FromCode">From</label></td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyRouting.FromCode)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromCode)%><label id="lblFrom"/></td>
                </tr> 
				<tr>
                    <td><label for="PolicyRouting_ToGlobalFlag">To Global?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PolicyRouting.ToGlobalFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.ToGlobalFlag)%></td>
               </tr> 
               <tr>
                    <td><label for="PolicyRouting_ToCode">To</label></td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyRouting.ToCode)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.ToCode)%><label id="lblTo"/></td>
                </tr> 
               <tr>
                    <td><label for="PolicyRouting_RoutingViceVersaFlag">Routing ViceVersa?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PolicyRouting.RoutingViceVersaFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.RoutingViceVersaFlag)%></td>
                </tr> 
				<tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Create Commissionable Route Item" title="Create Commissionable Route Item" class="red"/></td>
                </tr>
            </table>
           <%=Html.HiddenFor(model => model.PolicyRouting.FromCodeType)%>
           <%=Html.HiddenFor(model => model.PolicyRouting.ToCodeType)%>
           <%=Html.HiddenFor(model => model.CommissionableRouteItem.CommissionableRouteGroupId)%>

    <% } %>

   </div>
</div>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Commissionable Route Groups", "Main", new { controller = "CommissionableRouteGroup", action = "ListUnDeleted", id = ViewData["CommissionableRouteGroupId"] }, new { title = "Commissionable Route Groups" })%> &gt;
<%=ViewData["CommissionableRouteGroupName"]%> &gt;
Create Commissionable Route Items
</asp:Content>