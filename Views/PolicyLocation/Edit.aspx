<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicyLocation>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policies
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Locations</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using (Html.BeginForm(null, null, new { id = Model.PolicyLocationId }, FormMethod.Post, new { id = "form0", autocomplete = "off" }))
		   {%>
        <%= Html.AntiForgeryToken()%>
    
        <table cellpadding="0" cellspacing="0" width="100%"> 
	        <tr> 
		        <th class="row_header" colspan="3">Edit Policy Location</th> 
	        </tr> 
            <tr>
                <td><label for="PolicyLocationName">Name</label></td>
                <td><strong><%= Html.TextBoxFor(model => model.PolicyLocationName, new { maxlength = "100", autocomplete = "off" })%></strong><span class="error"> *</span></td>
                <td>
					<%= Html.ValidationMessageFor(model => model.PolicyLocationName)%>
					<span class="error" id="policyLocationError">A Policy Location already exists for this location. Please use the existing Policy Location.</span>
                </td>
            </tr>   
            <tr>
                <td><label for="GlobalFlag">Global Location?</label></td>
                <td><%= Html.CheckBoxFor(model => model.GlobalFlag, new { autocomplete = "off" })%></td>
                <td><%= Html.ValidationMessageFor(model => model.GlobalFlag)%></td>
            </tr> 
            <tr>
                <td><label for="Location">Location</label></td>
                <td><strong><%= Html.TextBoxFor(model => model.LocationName, new { maxlength = "100", autocomplete = "off" })%></strong><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.LocationName)%>
                    
                    <%=Html.HiddenFor(model => model.LocationCode)%>
                    <label id="lblLocation"/>
                </td>
            </tr>
            <tr>
                <td><label for="TravelPortTypeId">TravelPort Type</label></td>
                <td><%= Html.DropDownList("TravelPortTypeId", ViewData["TravelPortTypeList"] as SelectList, "None", new { autocomplete = "off" })%></td>
                <td><%= Html.ValidationMessageFor(model => model.TravelPortTypeId)%></td>
            </tr>  
             <tr>
                <td><label for="TravelPortName">TravelPort Name</label></td>
                <td><strong><%= Html.TextBoxFor(model => model.TravelPortName, new { maxlength = "100", autocomplete = "off" })%></strong></td>
                <td>
                    <%= Html.ValidationMessageFor(model => model.TravelPortName)%>
                    <%= Html.HiddenFor(model => model.TravelPortCode)%>
                    <label id="lblTravelPortNameMsg"/>
                </td>
            </tr> 
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
           <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right"><input type="submit" value="Edit Policy Location" title="Edit Policy Location" class="red"/></td>
            </tr>
        </table>
        <%=Html.HiddenFor(model => model.PolicyLocationId)%>
		<%=Html.HiddenFor(model => model.LocationType)%>
        <%= Html.HiddenFor(model => model.VersionNumber)%>
<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/PolicyLocation.js")%>" type="text/javascript"></script>
</asp:Content>



