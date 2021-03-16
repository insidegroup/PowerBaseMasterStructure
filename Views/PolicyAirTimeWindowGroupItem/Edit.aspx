<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirParameterGroupItemVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/PolicyAirParameterGroupItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Air Time Window Group Item</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
    <%= Html.AntiForgeryToken() %>
    <%= Html.ValidationSummary(true) %>
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Edit Policy Air Time Window Group Item</th> 
		    </tr> 
           <tr>
                <td>Policy Group Name</td>
                <td colspan="2"><strong><%= Html.Encode(CWTStringHelpers.TrimString(Model.PolicyAirParameterGroupItem.PolicyGroupName, 80))%></strong></td>
            </tr>
            <tr>
                <td><label for="PolicyAirParameterGroupItem_PolicyAirParameterValue">Time Window Minutes</label></td>
                <td><%= Html.TextBoxFor(model => model.PolicyAirParameterGroupItem.PolicyAirParameterValue, new { maxlength = "4" })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirParameterGroupItem.PolicyAirParameterValue)%></td>
            </tr> 
           <tr>
                <td><label for="PolicyAirParameterGroupItem_EnabledFlag">Enabled?</label></td>
                <td><%= Html.CheckBoxFor(model => model.PolicyAirParameterGroupItem.EnabledFlag)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirParameterGroupItem.EnabledFlag)%></td>
            </tr>  
            <tr>
                <td><label for="PolicyAirParameterGroupItem_EnabledDate">Enabled Date</label></td>
                <td><%= Html.EditorFor(model => model.PolicyAirParameterGroupItem.EnabledDate)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirParameterGroupItem.EnabledDate)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyAirParameterGroupItem_ExpiryDate">Expiry Date</label> </td>
                <td> <%= Html.EditorFor(model => model.PolicyAirParameterGroupItem.ExpiryDate)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirParameterGroupItem.ExpiryDate)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyAirParameterGroupItem_TravelDateValidFrom">Travel Date Valid From</label></td>
                <td><%= Html.EditorFor(model => model.PolicyAirParameterGroupItem.TravelDateValidFrom)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirParameterGroupItem.TravelDateValidFrom)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyAirParameterGroupItem_TravelDateValidTo">Travel Date Valid To</label></td>
                <td> <%= Html.TextBoxFor(model => model.PolicyAirParameterGroupItem.TravelDateValidTo)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirParameterGroupItem.TravelDateValidTo)%></td>
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
                <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Edit Policy Air Time Window Group Item" title="Edit Policy Air Time Window Group Item" class="red" name="btnSubmit"/></td>
            </tr>
        </table> 
        <input type="hidden" id="RoutingNameExtension" name="RoutingNameExtension" value="TimeWindow" />
        <%=Html.HiddenFor(model => model.PolicyRouting.FromCodeType)%>
        <%=Html.HiddenFor(model => model.PolicyRouting.ToCodeType)%>
		<%=Html.HiddenFor(model => model.PolicyAirParameterGroupItem.PolicyAirParameterTypeId)%>
        <%=Html.HiddenFor(model => model.PolicyAirParameterGroupItem.PolicyGroupId)%>
        <%=Html.HiddenFor(model => model.PolicyAirParameterGroupItem.VersionNumber)%>
<% } %>

       
    </div>
</div>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 60)), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Air Time Window Group Items", "Default", new { controller = "PolicyAirParameterGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy Air Time Window Group Items" })%> &gt;
Item
</asp:Content>