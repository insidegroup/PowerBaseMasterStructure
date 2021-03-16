<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.CityVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">City</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete City</th> 
		        </tr>  
                <tr>
					<td>City Name</td>
					<td><%= Html.Encode(Model.City.Name)%></td>
					<td></td>
				</tr>
				<tr>
					<td>City Code</td>
					<td><%= Html.Encode(Model.City.CityCode)%></td>
					<td></td>
				</tr>       
				<tr>
					<td>Country</td>
					<td><%= Html.Encode(Model.City.CountryName)%></td>
					<td></td>
				</tr> 
				<tr>
					<td>State/Province</td>
					<td><%= Html.Encode(Model.City.StateProvince != null ? Model.City.StateProvince.Name : "")%></td>
					<td></td>
				</tr>
				<tr>
					<td>Latitude</td>
					<td><%= Html.Encode(Model.City.LatitudeDecimal)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Longitude</td>
					<td><%= Html.Encode(Model.City.LongitudeDecimal)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Time Zone</td>
				    <td><%= Html.Encode(Model.City.TimeZoneRule != null ? Model.City.TimeZoneRule.TimeZoneRuleCodeDesc : "")%></td>
					<td></td>
				</tr> 

				<% if(Model.AllowDelete == false) { %>
					
					<tr>
						<td colspan="3">You cannot delete this City until all items referencing it are removed:</td>
					</tr>

					<% foreach (CWTDesktopDatabase.Models.CityReference cityReference in Model.CityReferences){ %>
						<tr>
							<td colspan="3"><strong>Attached <%=cityReference.TableName %></strong></td>
						</tr>
					<%}%>
								
				<%}%> 
				
				<tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right" colspan="2">
						<% if(Model.AllowDelete) { %>
							<% using (Html.BeginForm()) { %>
								<%= Html.AntiForgeryToken() %>
								<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
								<%= Html.HiddenFor(model => model.City.CityCode) %>
								<%= Html.HiddenFor(model => model.City.VersionNumber) %>
							<%}%>
						<%}%>
					</td>
				</tr>
			</table>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/City.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Cities", "Main", new { controller = "City", action = "List", }, new { title = "Cities" })%> &gt;
Delete City &gt;
<%:Model.City.Name%>
</asp:Content>