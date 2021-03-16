<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.AirlineClassCabinViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Airline Class Cabins</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
           <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Create Airline Class Cabin</th> 
		        </tr> 
                <tr>
                    <td><label for="AirlineClassCabin_AirlineClassCode">Airline Class Code</label></td>
                    <td><%= Html.TextBoxFor(model => model.AirlineClassCabin.AirlineClassCode, new { maxlength = "2" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.AirlineClassCabin.AirlineClassCode)%></td>
                </tr>
                 <tr>
                    <td><label for="AirlineClassCabin_AirlineCabinCode">Airline Cabin Code</label></td>
                    <td><%= Html.TextBoxFor(model => model.AirlineClassCabin.AirlineCabinCode, new { maxlength = "2" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.AirlineClassCabin.AirlineCabinCode)%></td>
                </tr>
              <tr>
                    <td><label for="AirlineClassCabin_ProductId">Product</label></td>
                    <td><%= Html.DropDownList("AirlineClassCabin.ProductId", ViewData["ProductList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.AirlineClassCabin.ProductId)%></td>
                </tr> 
                <tr>
                    <td><label for="AirlineClassCabin_SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.AirlineClassCabin.SupplierName)%><span class="error">*</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.AirlineClassCabin.SupplierName)%>
                        <%= Html.HiddenFor(model => model.AirlineClassCabin.SupplierCode)%>
                        <label id="lblSupplierNameMsg"/>
                    </td>
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
                    <td><label for="PolicyRouting_Name">Policy Routing Name</label></td>
                    <td><label id="lblAuto"></label></td>
                    <td>
                        <%= Html.HiddenFor(model => model.PolicyRouting.Name)%>
                        <%= Html.ValidationMessageFor(model => model.PolicyRouting.Name)%>
                    </td>
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
                    <td><label for="PolicyRouting_RoutingViceVersaFlag">Routing Vice Versa?</label></td>
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
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Create Airline Class Cabin" title="Create Airline Class Cabin" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.PolicyRouting.FromCodeType)%>
			<%= Html.HiddenFor(model => model.PolicyRouting.ToCodeType)%>
            <%= Html.HiddenFor(model => model.AirlineClassCabin.VersionNumber) %>
    <% } %>
        </div>
    </div>
   <script src="<%=Url.Content("~/Scripts/ERD/AirlineClassCabin.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/ERD/PolicyRouting.js")%>" type="text/javascript"></script>
</asp:Content>



