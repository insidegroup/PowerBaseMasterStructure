<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirVendorGroupItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/PolicyAirVendorGroupItemPolicyRouting.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Policy AirVendor Group Items</div></div>
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
                    <td>PolicyGroup Name</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.PolicyGroupName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>AirStatus</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.PolicyAirStatus)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Enabled?</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.EnabledFlag)%></td>
                    <td></td>
                </tr>  
                <tr>
                <td>Enabled Date</td>
                <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.EnabledDate.HasValue ? Model.PolicyAirVendorGroupItem.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Expiry Date</td>
                <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.ExpiryDate.HasValue ? Model.PolicyAirVendorGroupItem.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td></td>
                </tr> 
                <tr>
                    <td>TravelDate Valid From</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.TravelDateValidFrom)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>TravelDate Valid To</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.TravelDateValidTo)%></td>
                    <td></td>
                </tr> 
                 
                <tr>
                    <td>Supplier</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.SupplierName)%></td>
                    <td></td>
                </tr>
                 <tr>
                    <td>Product</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.ProductName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Ranking</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.AirVendorRanking)%></td>
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
			        <th class="row_header" colspan="3">Policy Routing<span class="error"> *</span></th> 
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
           <%=Html.HiddenFor(model => model.PolicyAirVendorGroupItem.PolicyGroupId)%>
           <%=Html.HiddenFor(model => model.PolicyAirVendorGroupItem.SupplierCode)%>
           <%=Html.HiddenFor(model => model.PolicyAirVendorGroupItem.AirVendorRanking)%>
    <% } %>

   </div>
</div>
</asp:Content>
