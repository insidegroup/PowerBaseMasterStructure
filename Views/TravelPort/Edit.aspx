<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TravelPort>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Travel Ports</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>

        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Travel Port</th> 
		        </tr>  
                <tr>
					<td><label for="TravelportName">Travel Port Name</label></td>
					<td><%= Html.TextBoxFor(model => model.TravelportName, new { maxlength = "100" })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.TravelportName)%>
						<label id="lblTravelPortNameMsg"/>
                    </td>
				</tr>
				<tr>
					<td><label for="TravelPortCode">Travel Port Code</label></td>
					<td><%= Html.Encode(Model.TravelPortCode)%></td>
					<td><%= Html.ValidationMessageFor(model => model.TravelPortCode)%></td>
				</tr>   
				<tr>
                    <td><label for="TravelPortTypeId">Travel Port Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.TravelPortTypeId, ViewData["TravelPortTypeList"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TravelPortTypeId)%> </td>
                </tr>
				<tr>
					<td><label for="CityCode">Travel Port City</label></td>
					<td><%= Html.TextBoxFor(model => model.CityName)%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.CityCode)%>
						<%= Html.HiddenFor(model => model.CityCode)%>
                    </td>
				</tr>
				<tr>
					<td><label for="MetropolitanArea">Metropolitan Area</label></td>
					<td><%= Html.TextBoxFor(model => model.MetropolitanArea, new { maxlength = "50" })%></td>
					<td><%= Html.ValidationMessageFor(model => model.MetropolitanArea)%></td>
				</tr>
				<tr>
                    <td><label for="CountryName">Country</label></td>
                    <td><%= Html.TextBoxFor(model => model.CountryName)%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.CountryCode)%>
						<%= Html.HiddenFor(model => model.CountryCode)%>
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
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit TravelPort" title="Edit TravelPort" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.TravelPortCode) %>
			<%= Html.HiddenFor(model => model.VersionNumber) %>
		 <% } %>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/TravelPort.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Travel Ports", "Main", new { controller = "TravelPort", action = "List", }, new { title = "Travel Ports" })%> &gt;
Edit Travel Port &gt;
<%:Model.TravelportName%>
</asp:Content>