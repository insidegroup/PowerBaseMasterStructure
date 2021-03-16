<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.FareRestrictionViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/FareRestriction.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Fare Restrictions</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Fare Restriction</th> 
		        </tr> 
                <tr>
                    <td><label for="FareRestriction_FareBasis">Fare Basis</label></td>
                    <td><%= Html.TextBoxFor(model => model.FareRestriction.FareBasis, new { maxlength="50"})%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.FareRestriction.FareBasis)%></td>
                </tr>
                
                <tr>
                    <td><label for="FareRestriction_Changes">Changes</label></td>
                    <td><%= Html.TextBoxFor(model => model.FareRestriction.Changes, new { maxlength = "255" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.FareRestriction.Changes)%></td>
                </tr>  
                <tr>
                    <td><label for="FareRestriction_Cancellations">Cancellations</label></td>
                    <td><%= Html.TextBoxFor(model => model.FareRestriction.Cancellations, new { maxlength = "255" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.FareRestriction.Cancellations)%></td>
                </tr> 
                <tr>
                    <td><label for="FareRestriction_ReRoute">ReRoute</label></td>
                    <td> <%= Html.TextBoxFor(model => model.FareRestriction.ReRoute, new { maxlength = "255" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.FareRestriction.ReRoute)%></td>
                </tr> 
                <tr>
                    <td><label for="FareRestriction_ValidOn">ValidOn</label></td>
                    <td><%= Html.TextBoxFor(model => model.FareRestriction.ValidOn, new { maxlength = "100" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.FareRestriction.ValidOn)%></td>
                </tr> 
                <tr>
                    <td><label for="FareRestriction_MinimumStay">Minimum Stay</label></td>
                    <td> <%= Html.TextBoxFor(model => model.FareRestriction.MinimumStay, new { maxlength = "100" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.FareRestriction.MinimumStay)%></td>
                </tr> 
               <tr>
                    <td><label for="FareRestriction_MaximumStay">Maximum Stay</label></td>
                    <td> <%= Html.TextBoxFor(model => model.FareRestriction.MaximumStay, new { maxlength = "100" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.FareRestriction.MaximumStay)%></td>
                </tr> 
                 <tr>
                    <td><label for="FareRestriction_ProductId">Product</label></td>
                    <td><%= Html.DropDownList("FareRestriction.ProductId", ViewData["ProductList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.FareRestriction.ProductId)%></td>
                </tr> 
                <tr>
                    <td><label for="FareRestriction_SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.FareRestriction.SupplierName)%></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.FareRestriction.SupplierName)%>
                        <%= Html.HiddenFor(model => model.FareRestriction.SupplierCode)%>
                        <label id="lblSupplierNameMsg"/>
                    </td>
                </tr>
                 <tr>
                    <td><label for="FareRestriction_LanguageCode">Language</label></td>
                    <td><%= Html.DropDownList("FareRestriction.LanguageCode", ViewData["LanguageList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.FareRestriction.LanguageCode)%></td>
                </tr> 
                <tr>
                    <td><label for="FareRestriction_DefaultChecked">DefaultChecked</label></td>
                    <td><%= Html.CheckBoxFor(model => model.FareRestriction.DefaultChecked)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.FareRestriction.DefaultChecked)%></td>
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
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Save and Add another Routing" title="Save and Add another Routing" class="red" name="btnSubmit"/>&nbsp;<input type="submit" value="Save" title="Save" class="red" name="btnSubmit"/></td>
                </tr>
            </table>
           <%=Html.HiddenFor(model => model.PolicyRouting.FromCodeType)%>
           <%=Html.HiddenFor(model => model.PolicyRouting.ToCodeType)%>
           <%=Html.HiddenFor(model => model.FareRestriction.FareRestrictionId)%>

    <% } %>

   </div>
</div>
</asp:Content>
