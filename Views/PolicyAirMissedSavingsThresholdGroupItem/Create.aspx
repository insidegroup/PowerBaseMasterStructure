<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirMissedSavingsThresholdGroupItemVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/PolicyAirMissedSavingsThresholdGroupItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Air Missed Savings Threshold Group Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Policy Air Missed Savings Threshold Group Item</th> 
		        </tr> 
                 <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_Amount">Amount</label></td>
                    <td><%= Html.TextBoxFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.MissedThresholdAmount, new { maxlength = "9" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.MissedThresholdAmount)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_CurrencyCode">Currency</label></td>
                    <td><%= Html.DropDownListFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.CurrencyCode, Model.Currencies, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.CurrencyCode)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_RoutingCode">Routing</label></td>
                    <td><%= Html.DropDownListFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.RoutingCode, Model.RoutingCodes, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.RoutingCode)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_EnabledFlag">Enabled?</label></td>
                    <td><%= Html.CheckBox("PolicyAirMissedSavingsThresholdGroupItem.EnabledFlag", true)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.EnabledFlag)%></td>
                    
                </tr>  
                <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.EnabledDate)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_ExpiryDate">Expiry Date</label> </td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.ExpiryDate)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidFrom">Travel Date Valid From</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.TravelDateValidFrom)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.TravelDateValidFrom)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirMissedSavingsThresholdGroupItem_TravelDateValidTo">Travel Date Valid To</label></td>
                    <td> <%= Html.EditorFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.TravelDateValidTo)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.TravelDateValidTo)%></td>
                </tr>  
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Save" title="Save" class="red" name="btnSubmit"/></td>
                </tr>
            </table>
           <%=Html.HiddenFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.PolicyGroupId)%>
           <%=Html.HiddenFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.PolicyProhibitedFlag)%>
           <%=Html.HiddenFor(model => model.PolicyAirMissedSavingsThresholdGroupItem.SavingsZeroedOutFlag)%>
    <% } %>
   </div>
</div>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 80), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
Policy City Group Items
</asp:Content>