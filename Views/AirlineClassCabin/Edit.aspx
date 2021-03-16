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
        <%using (Html.BeginRouteForm("Default", new { controller = "AirlineClassCabin", action = "Edit", id = Model.AirlineClassCabin.AirlineCabinCode, sCode = Model.AirlineClassCabin.SupplierCode, rId = Model.PolicyRouting.PolicyRoutingId }, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
           <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Edit Airline Class Cabin</th> 
		        </tr> 
                <tr>
                    <td>Airline Class Code</td>
                    <td><%= Html.Encode(Model.AirlineClassCabin.AirlineClassCode)%></td>
                    <td></td>
                </tr>
                   <tr>
                    <td>Product</td>
                    <td><%= Html.Encode(Model.AirlineClassCabin.ProductName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Supplier</td>
                    <td><%= Html.Encode(Model.AirlineClassCabin.SupplierName)%></td>
                    <td><%= Html.HiddenFor(model => model.AirlineClassCabin.SupplierName)%></td>
                </tr>
                <tr>
                    <td><label for="AirlineClassCabin_AirlineCabinCode">Airline Cabin Code</label></td>
                    <td><%= Html.TextBoxFor(model => model.AirlineClassCabin.AirlineCabinCode, new {maxlength="2" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.AirlineClassCabin.AirlineCabinCode)%></td>
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
                    <td><label id="lblAuto"><%=Html.Encode(Model.PolicyRouting.Name) %></label></td>
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
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Edit Airline Class Cabin" title="Edit Airline Class Cabin" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.PolicyRouting.FromCodeType)%>
			<%= Html.HiddenFor(model => model.PolicyRouting.ToCodeType)%>
            <%= Html.HiddenFor(model => model.AirlineClassCabin.AirlineClassCode)%>
            <%= Html.HiddenFor(model => model.AirlineClassCabin.SupplierCode)%>
            <%= Html.HiddenFor(model => model.AirlineClassCabin.ProductId)%>
            <%= Html.HiddenFor(model => model.AirlineClassCabin.PolicyRoutingId)%>
    <% } %>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/AirlineClassCabin.js")%>" type="text/javascript"></script>
	<script src="<%=Url.Content("~/Scripts/ERD/PolicyRouting.js")%>" type="text/javascript"></script>
</asp:Content>



