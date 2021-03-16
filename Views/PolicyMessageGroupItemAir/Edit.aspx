<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyMessageGroupItemAirVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/PolicyMessageGroupItemAir.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Message Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%" border="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Policy Message Group Item - Air</th> 
		        </tr> 
                <tr>
                    <td><label for="PolicyMessageGroupItemAir_PolicyMessageGroupItemName">Policy Message Name</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyMessageGroupItemAir.PolicyMessageGroupItemName, new { @maxlength = "50"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemAir.PolicyMessageGroupItemName)%></td>
                </tr>
                <tr>
                    <td><label for="PolicyMessageGroupItemAir_SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyMessageGroupItemAir.SupplierName)%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemAir.SupplierName)%>
                        <%= Html.HiddenFor(model => model.PolicyMessageGroupItemAir.SupplierCode)%>
                        <label id="lblSupplierNameMsg"/>
                    </td>
                </tr>
                <tr>
                    <td><label for="PolicyMessageGroupItemAir_EnabledFlag">Enabled</label></td>  
                    <td><%= Html.CheckBoxFor(model => model.PolicyMessageGroupItemAir.EnabledFlag)%>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemAir.EnabledFlag)%></td>
                </tr>  
                <tr>
                     <td><label for="PolicyMessageGroupItemAir_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyMessageGroupItemAir.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemAir.EnabledDate)%></td>
                </tr> 
                <tr>
                     <td><label for="PolicyMessageGroupItemAir_ExpiryDate">Expiry Date</label></td>
                    <td> <%= Html.EditorFor(model => model.PolicyMessageGroupItemAir.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemAir.ExpiryDate)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyMessageGroupItemAir_TravelDateValidFrom">Travel Date Valid From</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyMessageGroupItemAir.TravelDateValidFrom)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemAir.TravelDateValidFrom)%></td>
                </tr> 
                <tr>
                     <td><label for="PolicyMessageGroupItemAir_TravelDateValidTo">Travel Date Valid To</label></td>
                    <td> <%= Html.EditorFor(model => model.PolicyMessageGroupItemAir.TravelDateValidTo)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemAir.TravelDateValidTo)%></td>
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
                <td><label id="lblAuto"><%= Html.Encode(Model.PolicyRouting.Name)%></label></td>
                <td><%= Html.HiddenFor(model => model.PolicyRouting.Name)%><label id="lblPolicyRoutingNameMsg"/></td>
            </tr> 
            <tr>
                <td><label for="PolicyRouting_FromGlobalFlag">From Global?</label> </td>
                <td><%= Html.CheckBoxFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyRouting_FromCode">From</label> </td>
                <td> <%= Html.TextBoxFor(model => model.PolicyRouting.FromCode)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromCode)%><label id="lblFrom"/></td>
            </tr> 
            <tr>
                <td><label for="PolicyRouting_ToGlobalFlag">To Global?</label> </td>
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
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Edit Policy Message Group Item" class="red" title="Edit Policy Message Group Item"/></td>
                </tr>
            </table>
            <%=Html.HiddenFor(model => model.PolicyRouting.FromCodeType)%>
            <%=Html.HiddenFor(model => model.PolicyRouting.ToCodeType)%>  
            <%=Html.HiddenFor(model => model.PolicyMessageGroupItemAir.PolicyMessageGroupItemId)%>
            <%=Html.HiddenFor(model => model.PolicyMessageGroupItemAir.PolicyGroupId)%>
            <%=Html.HiddenFor(model => model.PolicyMessageGroupItemAir.ProductId)%>
            <%=Html.HiddenFor(model => model.PolicyMessageGroupItemAir.VersionNumber)%>
    <% } %>

   </div>
    </div>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(CWTStringHelpers.TrimString(Model.PolicyGroupName, 80), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroupId }, new { title = Model.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Message Group Items", "Default", new { controller = "PolicyMessageGroupItem", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Message Group Items" })%>
</asp:Content>