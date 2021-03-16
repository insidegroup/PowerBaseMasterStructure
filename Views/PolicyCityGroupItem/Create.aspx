<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyCityGroupItemVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/PolicyCityGroupItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy City Group Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Policy City Group Item</th> 
		        </tr> 
                 <tr>
                    <td><label for="CityName">City</label></td>
                    <td><%= Html.TextBox("CityName")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.CityCode)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyCityGroupItem_PolicyCityStatusId">City Status</label></td>
                    <td><%= Html.DropDownListFor(model => model.PolicyCityGroupItem.PolicyCityStatusId, Model.PolicyCityStatuses, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.PolicyCityStatusId)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyCityGroupItem_EnabledFlag">Enabled?</label></td>
                    <td><%= Html.CheckBox("PolicyCityGroupItem.EnabledFlag", true)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.EnabledFlag)%></td>
                    
                </tr>  
                <tr>
                    <td><label for="PolicyCityGroupItem_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyCityGroupItem.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.EnabledDate)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyCityGroupItem_ExpiryDate">Expiry Date</label> </td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyCityGroupItem.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.ExpiryDate)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyCityGroupItem_TravelDateValidFrom">Travel Date Valid From</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyCityGroupItem.TravelDateValidFrom)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.TravelDateValidFrom)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyCityGroupItem_TravelDateValidTo">Travel Date Valid To</label></td>
                    <td> <%= Html.EditorFor(model => model.PolicyCityGroupItem.TravelDateValidTo)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.TravelDateValidTo)%></td>
                </tr>  
                <tr>
                    <td><label for="PolicyCityGroupItem_InheritFromParentFlag">Inherit From Parent?</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyCityGroupItem.InheritFromParentFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyCityGroupItem.InheritFromParentFlag)%></td>
                    
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
           <%=Html.HiddenFor(model => model.PolicyCityGroupItem.CityCode)%>
           <%=Html.HiddenFor(model => model.PolicyCityGroupItem.PolicyGroupId)%>

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