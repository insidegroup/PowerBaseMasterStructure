<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PointOfSaleFeeLoadVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Point of Sale Fee Definitions</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Point of Sale Fee Definitions</div></div>
     <div id="content">

        <table cellpadding="0" border="0" width="100%" cellspacing="0" class="main-table"> 
		    <tr> 
			    <th class="row_header" colspan="3">Delete Point of Sale Fee Definition</th> 
		    </tr> 
            <tr>
                <td><label for="PointOfSaleFeeLoad_ClientTopUnitGuid">Client TopUnit Name</label></td>
                <td colspan="2"><%= Html.Encode(Model.PointOfSaleFeeLoad.ClientTopUnitName)%></td>
            </tr>
            <tr>
                <td><label for="PointOfSaleFeeLoad_ClientSubUnitGuid">Client SubUnit Name</label></td>
                <td colspan="2"><%= Html.Encode(Model.PointOfSaleFeeLoad.ClientSubUnitName)%></td>
            </tr> 
            <tr>
                <td><label for="PointOfSaleFeeLoad_TravelerTypeGuid">Traveler Type</label></td>
                <td colspan="2"><%= Html.Raw(Model.PointOfSaleFeeLoad.TravelerTypeName)%></td>
            </tr>
            <tr>
                <td><label for="PointOfSaleFeeLoad_FeeLoadDescriptionTypeCode">POS Fee Description</label></td>
                <td colspan="2"><%= Html.Raw(Model.PointOfSaleFeeLoad.FeeLoadDescriptionTypeCode)%></td>
            </tr>
            <tr>
                <td><label for="PointOfSaleFeeLoad_ProductName">Product</label></td>
                <td colspan="2"><%= Html.Raw(Model.PointOfSaleFeeLoad.Product.ProductName)%></td>
            </tr>
            <tr>
                <td><label for="PointOfSaleFeeLoad_AgentInitiatedFlag">Agent Initiated?</label></td>
                <td colspan="2"><%= Html.Raw(Model.PointOfSaleFeeLoad.AgentInitiatedFlag ? "True" : "False")%></td>
            </tr>
            <tr>
                <td><label for="PointOfSaleFeeLoad_TravelIndicator">Travel Indicator</label></td>
                <td colspan="2"><%= Html.Raw(Model.PointOfSaleFeeLoad.TravelIndicator)%></td>
            </tr>
            <tr>
                <td><label for="PointOfSaleFeeLoad_FeeLoadAmount">Fee Amount</label></td>
                <td colspan="2"><%= Html.Raw(string.Format("{0:0..####}", Model.PointOfSaleFeeLoad.FeeLoadAmount))%></td>
            </tr>
            <tr>
                <td><label for="PointOfSaleFeeLoad_FeeLoadCurrencyCode">Fee Currency</label></td>
                <td colspan="2"><%= Html.Raw(Model.PointOfSaleFeeLoad.FeeLoadCurrencyCode)%></td>
            </tr>
			<tr>
				<td width="30%" class="row_footer_left"></td>
				<td width="40%" class="row_footer_centre"></td>
				<td width="30%" class="row_footer_right"></td>
			</tr>
			<tr>
				<td class="row_footer_blank_left">
					<a href="javascript:history.back();" class="red" title="Back">Back</a>
				</td>                    
				<td class="row_footer_blank_right" colspan="2">
					<% using (Html.BeginForm()) { %>
						<%= Html.AntiForgeryToken() %>
						<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
						<%= Html.HiddenFor(model => model.PointOfSaleFeeLoad.VersionNumber) %>
						<%= Html.HiddenFor(model => model.PointOfSaleFeeLoad.PointOfSaleFeeLoadId) %>
					<%}%>
				</td>
			</tr>
		</table>
    </div>
</div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#breadcrumb').css('width', 'auto');
		$('.full-width #search_wrapper').css('height', '22px');

    	$("#content tr:odd").addClass("row_odd");
	    $("#content tr:even").addClass("row_even");
    })
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%= Html.ActionLink("Point of Sale Fee Definitions", "List", "PointOfSaleFeeLoad")%> &gt;
<%= Html.Raw(Model.PointOfSaleFeeLoad.FeeLoadDescriptionTypeCode) %>
</asp:Content>