<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PointOfSaleFeeLoadsVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Point of Sale Fee Definitions</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
   <link href="<%=Url.Content("~/Style/Themes/Aristo/jquery-ui-1.8.7.custom.css") %>" rel="Stylesheet" type="text/css" />
	<style type="text/css">
		#content { padding-top: 5px; }
   </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Point of Sale Fee Definitions</div></div>

	<div id="divRuleSearch">
		<% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Get, new { id = "form0", autocomplete="off" })) {%>
        <%= Html.AntiForgeryToken() %>
			<table class="tablesorter" cellspacing="0">
				<thead>
					<tr>
						<th width="10%">Search field:</th>
						<th width="35%">
							<span class="hierarchy-row">
                                <% =Html.TextBoxFor(model => Model.ClientTopUnitName, new { autocomplete="off", placeholder = "Client TopUnit Name" }) %><span class="error"> *</span>
                                <% =Html.HiddenFor(model => Model.ClientTopUnitGuid) %>
                                <label id="lblClientTopUnitGuid_Msg"></label>
							</span>
						</th>
						<th width="40%" class="search-column">
							<span class="hierarchy-row">
                                Client SubUnit&nbsp;<% =Html.TextBoxFor(model => Model.ClientSubUnitName, new { autocomplete="off" }) %>
                                <% =Html.HiddenFor(model => Model.ClientSubUnitGuid) %>
                                <label id="lblClientSubUnitGuid_Msg"></label>
							</span>
                            <br />
							<span class="hierarchy-row">
                                Traveler Type:&nbsp;<%= Html.TextBoxFor(model => Model.TravelerTypeName,  new { autocomplete="off" })%>
                                <% =Html.HiddenFor(model => Model.TravelerTypeGuid) %>
                                <label id="lblTravelerTypeGuid_Msg"></label>
							</span>
						</th>
						<th width="15%" class="search-column"><span id="SearchButton"><small>Search >> </small></span></th>
					</tr>
				</thead> 
			</table>
		<% } %>
		<div class="home_button">
			<a href="/" class="red">Home</a>
		</div>
	</div>
	<div id="content">
        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed;">
            <tr> 
				<th width="13%" class="row_header_left"><%= Html.RouteLink("Client TopUnit Name", "ListMain", new { page = 1, sortField = "ClientTopUnitName", sortOrder = ViewData["NewSortOrder"].ToString(), clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, travelerTypeName = Model.TravelerTypeName, clientTopUnitGuid = Model.ClientTopUnitGuid, clientSubUnitGuid = Model.ClientSubUnitGuid, travelerTypeGuid = Model.TravelerTypeGuid })%></th> 
				<th width="15%"><%= Html.RouteLink("Client SubUnit Name", "ListMain", new { page = 1, sortField = "ClientSubUnitName", sortOrder = ViewData["NewSortOrder"].ToString(), clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, travelerTypeName = Model.TravelerTypeName, clientTopUnitGuid = Model.ClientTopUnitGuid, clientSubUnitGuid = Model.ClientSubUnitGuid, travelerTypeGuid = Model.TravelerTypeGuid })%></th> 
				<th width="12%"><%= Html.RouteLink("Traveler Type", "ListMain", new { page = 1, sortField = "TravelerTypeName", sortOrder = ViewData["NewSortOrder"].ToString(), clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, travelerTypeName = Model.TravelerTypeName, clientTopUnitGuid = Model.ClientTopUnitGuid, clientSubUnitGuid = Model.ClientSubUnitGuid, travelerTypeGuid = Model.TravelerTypeGuid })%></th> 
                <th width="15%"><%= Html.RouteLink("POS Fee Description", "ListMain", new { page = 1, sortField = "FeeLoadDescriptionTypeCode", sortOrder = ViewData["NewSortOrder"].ToString(), clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, travelerTypeName = Model.TravelerTypeName, clientTopUnitGuid = Model.ClientTopUnitGuid, clientSubUnitGuid = Model.ClientSubUnitGuid, travelerTypeGuid = Model.TravelerTypeGuid})%></th> 
			    <th width="6%"><%= Html.RouteLink("Product", "ListMain", new { page = 1, sortField = "ProductName", sortOrder = ViewData["NewSortOrder"].ToString(), clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, travelerTypeName = Model.TravelerTypeName, clientTopUnitGuid = Model.ClientTopUnitGuid, clientSubUnitGuid = Model.ClientSubUnitGuid, travelerTypeGuid = Model.TravelerTypeGuid })%></th> 
			    <th width="6%"><%= Html.RouteLink("Agent?", "ListMain", new { page = 1, sortField = "AgentInitiatedFlag", sortOrder = ViewData["NewSortOrder"].ToString(), clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, travelerTypeName = Model.TravelerTypeName, clientTopUnitGuid = Model.ClientTopUnitGuid, clientSubUnitGuid = Model.ClientSubUnitGuid, travelerTypeGuid = Model.TravelerTypeGuid })%></th> 
			    <th width="8%"><%= Html.RouteLink("Indicator", "ListMain", new { page = 1, sortField = "TravelIndicator", sortOrder = ViewData["NewSortOrder"].ToString(), clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, travelerTypeName = Model.TravelerTypeName, clientTopUnitGuid = Model.ClientTopUnitGuid, clientSubUnitGuid = Model.ClientSubUnitGuid, travelerTypeGuid = Model.TravelerTypeGuid })%></th> 
			    <th width="13%"><%= Html.RouteLink("Amt/Currency", "ListMain", new { page = 1, sortField = "FeeLoadAmount", sortOrder = ViewData["NewSortOrder"].ToString(), clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, travelerTypeName = Model.TravelerTypeName, clientTopUnitGuid = Model.ClientTopUnitGuid, clientSubUnitGuid = Model.ClientSubUnitGuid, travelerTypeGuid = Model.TravelerTypeGuid })%></th> 
			    <th width="4%">&nbsp;</th> 
			    <th width="5%" class="row_header_right">&nbsp;</th> 
		    </tr> 
            <%
            foreach (var item in Model.PointOfSaleFeeLoads) { 
            %>
            <tr>
				<td><%= Html.Encode(item.ClientTopUnitName) %></td>
                <td><%= Html.Encode(item.ClientSubUnitName) %></td>
                <td><%= Html.Encode(item.TravelerTypeName) %></td>
                <td><%= Html.Encode(item.FeeLoadDescriptionTypeCode) %></td>
                <td><%= Html.Encode(item.ProductName) %></td>
                <td><%= Html.Encode(item.AgentInitiatedFlag) %></td>
                <td><%= Html.Encode(item.TravelIndicator) %></td>
                <td><%= Html.Encode(string.Format("{0:0.####}", item.FeeLoadAmount)) %>/<%= Html.Encode(item.FeeLoadCurrencyCode) %></td>
                <td>
                    <%if (item.HasWriteAccess.Value) { %>
						<%= Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.PointOfSaleFeeLoadId }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if (item.HasWriteAccess.Value){ %>
						<%= Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.PointOfSaleFeeLoadId }, new { title = "Edit" })%>
                    <%} %>
                </td>
            </tr>        
            <% 
            } 
            %>
            <tr>
                <td colspan="10" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.PointOfSaleFeeLoads.HasPreviousPage) { %><%= Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.PointOfSaleFeeLoads.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, travelerTypeName = Model.TravelerTypeName, clientTopUnitGuid = Model.ClientTopUnitGuid, clientSubUnitGuid = Model.ClientSubUnitGuid, travelerTypeGuid = Model.TravelerTypeGuid }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"><% if (Model.PointOfSaleFeeLoads.HasNextPage) {  %><%= Html.RouteLink("Next Page>>>", "ListMain", new { page = (Model.PointOfSaleFeeLoads.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), clientTopUnitName = Model.ClientTopUnitName, clientSubUnitName = Model.ClientSubUnitName, travelerTypeName = Model.TravelerTypeName, clientTopUnitGuid = Model.ClientTopUnitGuid, clientSubUnitGuid = Model.ClientSubUnitGuid, travelerTypeGuid = Model.TravelerTypeGuid }, new { title = "Next Page" })%><%}%> </div>
                        <div class="paging_centre"><%if (Model.PointOfSaleFeeLoads.TotalPages > 0) { %>Page <%=Model.PointOfSaleFeeLoads.PageIndex%> of <%=Model.PointOfSaleFeeLoads.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
                <tr> 
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="9">
					<%if (Model.HasDomainWriteAccess) { %>
						<%= Html.ActionLink("Create POS Fee", "Create", null, new { @class = "red", title = "Create POS Fee" })%>
					<%}%>
                </td> 
            </tr> 
		</table>
	</div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/PointOfSaleFeeLoad.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<% if (!string.IsNullOrEmpty(Model.ClientTopUnitName)) { %>
    <%= Html.ActionLink("Point of Sale Fee Definitions", "List", "PointOfSaleFeeLoad")%> &gt; 
    <%= Html.Raw(Model.ClientTopUnitName) %>
<% } %>
<% if (!string.IsNullOrEmpty(Model.ClientSubUnitName)) { %>
    &gt; <%= Html.Raw(Model.ClientSubUnitName) %>
<% } %>
<% if (!string.IsNullOrEmpty(Model.TravelerTypeName)) { %>
    &gt; <%= Html.Raw(Model.TravelerTypeName) %>
<% } %>
</asp:Content>