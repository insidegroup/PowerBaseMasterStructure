<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TravelerTypeVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/ERD/TravelerTypeSponsor.js")%>" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Traveler Type - Sponsor Setup</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
                <tr>
                    <td><label for="TravelerTypeSponsor_IsSponsorFlag">Is Sponsor?</label></td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.CheckBoxFor(model => Model.TravelerTypeSponsor.IsSponsorFlag)%>
						<% } else { %>
							<%= Html.CheckBoxFor(model => Model.TravelerTypeSponsor.IsSponsorFlag, new { disabled = "true" })%>
						<% } %>
                    </td>
                    <td><%= Html.ValidationMessageFor(model => model.TravelerTypeSponsor.IsSponsorFlag)%></td>
                </tr>
                <tr>
                    <td><label for="TravelerTypeSponsor_RequiresSponsorFlag">Requires Sponsor?</label></td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.CheckBoxFor(model => Model.TravelerTypeSponsor.RequiresSponsorFlag)%>
						<% } else { %>
							<%= Html.CheckBoxFor(model => Model.TravelerTypeSponsor.RequiresSponsorFlag, new { disabled = "true" })%>
						<% } %>
                    </td>
                    <td><%= Html.ValidationMessageFor(model => model.TravelerTypeSponsor.RequiresSponsorFlag)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right">
						<%if (ViewData["Access"] == "WriteAccess"){ %>
							<input type="submit" value="Save" title="Save" class="red"/></td>
						<% } %>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.TravelerTypeSponsor.TravelerTypeGuid) %>
			<%= Html.HiddenFor(model => model.TravelerTypeSponsor.VersionNumber) %>
			<%= Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
		<% } %>
        </div>
    </div>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.RouteLink("Traveler Types", "Main", new { controller = "ClientSubUnitTravelerType", action = "ListByClientSubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Traveler Types" })%> &gt;
<%=Model.TravelerType.TravelerTypeName%>
</asp:Content>