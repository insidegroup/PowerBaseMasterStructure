<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.FareRestrictionViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/FareRestrictionPolicyRouting.js")%>" type="text/javascript"></script>
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
			        <th class="row_header" colspan="3">Create Routing</th> 
		        </tr> 
                <tr>
                    <td>Fare Basis</td>
                    <td><%= Html.Encode(Model.FareRestriction.FareBasis)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Changes</td>
                    <td><%= Html.Encode(Model.FareRestriction.Changes)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Cancellations</td>
                    <td><%= Html.Encode(Model.FareRestriction.Cancellations)%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td>ReRoute</td>
                    <td><%= Html.Encode(Model.FareRestriction.ReRoute)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>ValidOn</td>
                    <td><%= Html.Encode(Model.FareRestriction.ValidOn)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Minimum Stay</td>
                    <td><%= Html.Encode(Model.FareRestriction.MinimumStay)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Maximum Stay</td>
                    <td><%= Html.Encode(Model.FareRestriction.MaximumStay)%></td>
                    <td></td>
                </tr> 
                  <tr>
                    <td>Product</td>
                    <td><%= Html.Encode(Model.FareRestriction.ProductName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Supplier</td>
                    <td><%= Html.Encode(Model.FareRestriction.SupplierName)%></td>
                    <td></td>
                </tr>
                 <tr>
                    <td>Language</td>
                    <td><%= Html.Encode(Model.FareRestriction.LanguageName)%></td>
                    <td></td>
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
			        <th class="row_header" colspan="3">PolicyRouting</th> 
		        </tr> 
                <tr>
                    <td>Routing Name</td>
                    <td><label id="lblAuto"></label></td>
                    <td><%= Html.HiddenFor(model => model.PolicyRouting.Name)%><label id="lblPolicyRoutingNameMsg"/></td>
                </tr> 
               <tr>
                    <td><label for="PolicyRouting_FromGlobalFlag">From Global</label> </td>
                    <td><%= Html.CheckBoxFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
                </tr> 
               <tr>
                    <td><label for="PolicyRouting_FromCode">From</label> </td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyRouting.FromCode)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromCode)%><label id="lblFrom"/></td>
                </tr> 
               <tr>
                    <td><label for="PolicyRouting_ToGlobalFlag">To Global</label> </td>
                    <td><%= Html.CheckBoxFor(model => model.PolicyRouting.ToGlobalFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.ToGlobalFlag)%></td>
                </tr> 
               <tr>
                    <td><label for="PolicyRouting_ToCode">To</label> </td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyRouting.ToCode)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.ToCode)%><label id="lblTo"/></td>
                </tr> 
               <tr>
                    <td><label for="PolicyRouting_RoutingViceVersaFlag">Routing Vice Versa?</label> </td>
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
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Save and Add another Routing" class="red" name="btnSubmit"/>&nbsp;<input type="submit" value="Save" class="red" name="btnSubmit"/></td>
                </tr>
            </table>
            <%=Html.HiddenFor(model => model.PolicyRouting.FromCodeType)%>
           <%=Html.HiddenFor(model => model.PolicyRouting.ToCodeType)%>
           <%=Html.HiddenFor(model => model.FareRestriction.FareRestrictionId)%>
           <%=Html.HiddenFor(model => model.FareRestriction.SupplierCode)%>
           <%=Html.HiddenFor(model => model.FareRestriction.ProductName)%>
    <% } %>

   </div>
</div>
</asp:Content>
