<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TravelPortVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Travel Ports</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Travel Port</th> 
		        </tr>  
                <tr>
					<td><label for="TravelportName">Travel Port Name</label></td>
					<td><%= Html.Encode(Model.TravelPort.TravelportName) %></td>
					<td></td>
				</tr>   
				<tr>
					<td><label for="TravelPortCode">Travel Port Code</label></td>
					<td><%= Html.Encode(Model.TravelPort.TravelPortCode) %></td>
					<td></td>
				</tr>
				<tr>
                    <td><label for="TravelPortTypeId">Travel Port Type</label></td>
                    <td><%= Html.Encode(Model.TravelPort.TravelPortTypeDescription)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td><label for="CityName">Travel Port City</label></td>
                    <td><%= Html.Encode(Model.TravelPort.CityName)%> (<%= Html.Encode(Model.TravelPort.CityCode)%>)</td>
                    <td></td>
                </tr>
				<tr>
                    <td><label for="MetropolitanArea">Metropolitan Area</label></td>
                    <td><%= Html.Encode(Model.TravelPort.MetropolitanArea)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td><label for="CountryName">Country</label></td>
                    <td><%= Html.Encode(Model.TravelPort.CountryName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td><label for="StateProvince_Name">State/Province</label></td>
                    <td><%= Html.Encode(Model.TravelPort.StateProvince != null ? Model.TravelPort.StateProvince.Name : "")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td><label for="LatitudeDecimal">Latitude</label></td>
                    <td><%= Html.Encode(Model.TravelPort.LatitudeDecimal)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td><label for="LongitudeDecimal">Longitude</label></td>
                    <td><%= Html.Encode(Model.TravelPort.LongitudeDecimal)%></td>
                    <td></td>
                </tr>

				<% if(Model.AllowDelete == false) { %>
					
					<tr>
						<td colspan="3">You cannot delete this Travel Port until all items referencing it are removed:</td>
					</tr>

					<% foreach(CWTDesktopDatabase.Models.TravelPortReference travelPortReference in Model.TravelPortReferences) { %>
						<tr>
							<td colspan="3"><strong>Attached <%=travelPortReference.TableName %></strong></td>
						</tr>
					<%}%>
								
				<%}%> 
				
				<tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
                    </td>
                    <td class="row_footer_blank_right" colspan="2">
						<% if(Model.AllowDelete) { %>
							<% using (Html.BeginForm()) { %>
								<%= Html.AntiForgeryToken() %>
								<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
								<%= Html.HiddenFor(model => model.TravelPort.TravelPortCode) %>
								<%= Html.HiddenFor(model => model.TravelPort.VersionNumber) %>
							<%}%>
						<%}%>
					</td>
				</tr>
			</table>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/TravelPort.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Travel Ports", "Main", new { controller = "TravelPort", action = "List", }, new { title = "Travel Ports" })%> &gt;
Delete Travel Port &gt;
<%:Model.TravelPort.TravelportName%>
</asp:Content>