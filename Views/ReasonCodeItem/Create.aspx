<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ReasonCodeItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Reason Codes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Reason Code Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Reason Code Item</th> 
		        </tr> 
                <tr>
                    <td><label for="ReasonCodeGroupName">Reason Code Group Name</label></td>
                    <td><strong><%= Html.Label(Model.ReasonCodeGroupName)%></strong></td>
                    <td></td>
                </tr>  
                <tr>
                    <td><label for="ReasonCode">Reason Code</label></td>
                    <td><%= Html.DropDownList("ReasonCode", ViewData["ReasonCodes"] as SelectList, "Please Select...")%><span class="error"> *</span> </td>
                    <td><%= Html.ValidationMessageFor(model => model.ReasonCode)%></td>
                </tr>              
                <tr>
                    <td><label for="ProductId">Product</label></td>
                    <td><%= Html.DropDownList("ProductId", new List<SelectListItem>(), "Please Select...")%><span class="error"> *</span> </td>
                    <td><%= Html.ValidationMessageFor(model => model.ProductId)%></td>
                </tr>              
                <tr>
                    <td><label for="ReasonCodeTypeId">Reason Code Type</label></td>
                    <td><%= Html.DropDownList("ReasonCodeTypeId", new List<SelectListItem>(), "Please Select...")%><span class="error"> *</span> </td>
                    <td><%= Html.ValidationMessageFor(model => model.ReasonCodeTypeId)%></td>
                </tr>    
				<tr>
                    <td><label for="TravelerFacingFlag">Traveler Facing</label></td>
                    <td><%= Html.CheckBoxFor(model => model.TravelerFacingFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TravelerFacingFlag)%></td>
                </tr>            
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Reason Code Item" title="Create Reason Code Item" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ReasonCodeGroupId) %>
    <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/ReasonCodeItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Reason Code Product Types", "Main", new { controller = "ReasonCodeProductType", action = "List", }, new { title = "Reason Code Product Types" })%> &gt;
<%=Html.RouteLink("Descriptions", "Main", new { controller = "ReasonCodeProductTypeDescription", action = "List", productId = Model.ProductId, reasonCode = Model.ReasonCode, reasonCodeTypeId = Model.ReasonCodeTypeId }, new { title = "Reason Code Product Types" })%>
</asp:Content>
