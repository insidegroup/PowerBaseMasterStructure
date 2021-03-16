<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.City>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">City</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>

        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit City</th> 
		        </tr>  
                <tr>
					<td><label for="Name">City Name</label></td>
					<td><%= Html.TextBoxFor(model => model.Name, new { maxlength = "100" })%><span class="error"> *</span></td>
					 <td><%= Html.ValidationMessageFor(model => model.Name)%></td>
				</tr>
				<tr>
					<td><label for="CityCode">City Code</label></td>
					<td><%= Html.Encode(Model.CityCode)%></td>
					<td></td>
				</tr>       
				<tr>
                    <td><label for="CountryCode">Country</label></td>
                    <td> <%= Html.TextBoxFor(model => model.CountryName)%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.CountryCode)%>
                        <%= Html.Hidden("CountryCode")%>
                        <label id="lblCountryNameMsg"/>
                    </td>
                </tr>
				<tr>
                    <td><label for="StateProvinceCode">State/Province</label></td>
                    <td><%= Html.DropDownListFor(model => model.StateProvinceCode, ViewData["StateProvinceList"] as SelectList, "Please Select...")%><span class="stateProvinceCodeError error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.StateProvinceCode)%>
						 <label id="lblStateProvinceCodeMsg"/>
                    </td>
                </tr>
				<tr>
					<td><label for="LatitudeDecimal">Latitude</label></td>
					<td><%= Html.TextBoxFor(model => model.LatitudeDecimal, new { maxlength = "10" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.LatitudeDecimal)%></td>
				</tr>	
				<tr>
					<td><label for="LongitudeDecimal">Longitude</label></td>
					<td><%= Html.TextBoxFor(model => model.LongitudeDecimal, new { maxlength = "10" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.LongitudeDecimal)%></td>
				</tr>
                <tr>
                    <td><label for="TimeZoneRuleCode">Time Zone</label></td>
                    <td><%= Html.DropDownListFor(model => model.TimeZoneRuleCode, ViewData["TimeZoneRules"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TimeZoneRuleCode)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="50%" class="row_footer_centre"></td>
                    <td width="20%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit City" title="Edit City" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.CityCode) %>
			<%= Html.HiddenFor(model => model.VersionNumber) %>
		 <% } %>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/City.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Cities", "Main", new { controller = "City", action = "List", }, new { title = "Cities" })%> &gt;
Edit City &gt;
<%:Model.Name%>
</asp:Content>