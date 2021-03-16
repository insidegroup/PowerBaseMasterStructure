<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PointOfSaleFeeLoadVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Point of Sale Fee Definitions</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Point of Sale Fee Definitions</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete="off" })) {%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0" class="main-table"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Point of Sale Fee Definition</th> 
		        </tr> 
		        <tr>
                    <td><label for="PointOfSaleFeeLoad_ClientTopUnitGuid">Client TopUnit Name</label></td>
                    <td>
                        <%= Html.TextBoxFor(model => model.PointOfSaleFeeLoad.ClientTopUnitName, new { autocomplete="off" })%><span class="error"> *</span>
                        <%= Html.HiddenFor(model => model.PointOfSaleFeeLoad.ClientTopUnitGuid)%><label id="lblPointOfSaleFeeLoad_ClientTopUnitGuid_Msg"/>
                    </td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.PointOfSaleFeeLoad.ClientTopUnitGuid)%>
                        <label id="PointOfSaleFeeLoad_Error"/>
                    </td>
			    </tr>  
                <tr>
                    <td><label for="PointOfSaleFeeLoad_ClientSubUnitGuid">Client SubUnit Name</label></td>
                    <td>
                        <%= Html.TextBoxFor(model => model.PointOfSaleFeeLoad.ClientSubUnitName, new { autocomplete="off" })%>
                        <%= Html.HiddenFor(model => model.PointOfSaleFeeLoad.ClientSubUnitGuid)%><label id="lblPointOfSaleFeeLoad_ClientSubUnitGuid_Msg"/>
                    </td>
                    <td><%= Html.ValidationMessageFor(model => model.PointOfSaleFeeLoad.ClientSubUnitGuid)%></td>
			    </tr>  
                <tr>
                    <td><label for="PointOfSaleFeeLoad_TravelerTypeGuid">Traveler Type</label></td>
                    <td>
                        <%= Html.TextBoxFor(model => model.PointOfSaleFeeLoad.TravelerTypeName, new { autocomplete="off" })%>
                        <%= Html.HiddenFor(model => model.PointOfSaleFeeLoad.TravelerTypeGuid)%><label id="lblPointOfSaleFeeLoad_TravelerTypeGuid_Msg"/>
                    </td>
                    <td><%= Html.ValidationMessageFor(model => model.PointOfSaleFeeLoad.TravelerTypeGuid)%></td>
			    </tr>
                <tr>
					<td><label for="PointOfSaleFeeLoad_PointOfSaleFeeLoad">POS Fee Description</label></td>
					<td><%= Html.DropDownListFor(model => model.PointOfSaleFeeLoad.FeeLoadDescriptionTypeCode, Model.FeeLoadDescriptionTypeCodes, "Please Select...", new { })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.PointOfSaleFeeLoad.FeeLoadDescriptionTypeCode)%>
					</td>
				</tr>
                <tr>
					<td><label for="PointOfSaleFeeLoad_ProductId">Product</label></td>
					<td><%= Html.DropDownListFor(model => model.PointOfSaleFeeLoad.ProductId, Model.Products, "Please Select...", new { })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.PointOfSaleFeeLoad.ProductId)%>
					</td>
				</tr>
                <tr>
                    <td><label for="PointOfSaleFeeLoad_AgentInitiatedFlag">Agent Initiated?</label></td>
                    <td>
                        <%= Html.CheckBoxFor(model => model.PointOfSaleFeeLoad.AgentInitiatedFlag, new { autocomplete="off" })%>
                        <%= Html.HiddenFor(model => model.PointOfSaleFeeLoad.AgentInitiatedFlag)%>
                    </td>
                    <td><%= Html.ValidationMessageFor(model => model.PointOfSaleFeeLoad.AgentInitiatedFlag)%></td>
			    </tr>
                <tr>
					<td><label for="PointOfSaleFeeLoad_TravelIndicator">Travel Indicator</label></td>
					<td><%= Html.DropDownListFor(model => model.PointOfSaleFeeLoad.TravelIndicator, Model.TravelIndicators, "Please Select...", new { })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.PointOfSaleFeeLoad.TravelIndicator)%>
					</td>
				</tr>
                <tr>
                    <td><label for="PointOfSaleFeeLoad_FeeLoadAmount">Fee Amount</label></td>
                    <td>
                        <%= Html.TextBoxFor(model => model.PointOfSaleFeeLoad.FeeLoadAmount, new { autocomplete="off", @Class = "FeeLoadAmountCreate" })%><span class="error"> *</span>
                        <%= Html.HiddenFor(model => model.PointOfSaleFeeLoad.FeeLoadAmount)%>
                    </td>
                    <td><%= Html.ValidationMessageFor(model => model.PointOfSaleFeeLoad.FeeLoadAmount)%></td>
			    </tr>
                <tr>
					<td><label for="PointOfSaleFeeLoad_FeeLoadCurrencyCode">Fee Currency</label></td>
					<td><%= Html.DropDownListFor(model => model.PointOfSaleFeeLoad.FeeLoadCurrencyCode, Model.Currencies, "Please Select...", new { })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.PointOfSaleFeeLoad.FeeLoadCurrencyCode)%>
					</td>
				</tr>
				<tr>
					<td width="30%" class="row_footer_left"></td>
					<td width="40%" class="row_footer_centre"></td>
					<td width="30%" class="row_footer_right"></td>
				</tr>
			    <tr>
				    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
				    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Create POS Fee" class="red" title="Create POS Fee"/></td>
			    </tr>
		    </table>
		<% } %>
    </div>
</div>
    
<script src="<%=Url.Content("~/Scripts/ERD/PointOfSaleFeeLoadCreate.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%= Html.ActionLink("Point of Sale Fee Definitions", "List", "PointOfSaleFeeLoad")%> &gt;
Create
</asp:Content>