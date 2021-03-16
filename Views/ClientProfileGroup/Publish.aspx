<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientProfileGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Profile Publish</asp:Content>
<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
	<script src="<%=Url.Content("~/Scripts/ERD/ClientProfileGroupPublish.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Client Profile Publish</div>
		</div>
		<div id="content">
			<%using (Html.BeginForm("Publish", "ClientProfileGroup", FormMethod.Post, new { @id = "form0" })) {%>
				<table width="100%" cellpadding="0" cellspacing="0" border="0">
					<tr>
						<td width="15%">Client Top</td>
						<td width="15%"><%=Html.Encode(ViewData["ClientTopUnitName"]) %></td>
						<td width="15%">GDS</td>
						<td width="15%"><%=Html.Encode(Model.ClientProfileGroup.GDSCode) %></td>
						<td width="15%">Last Published</td>
						<td width="20%"><label id="lastPublishedDate"><%=Html.Encode(Model.ClientProfileGroup.LastRevisionPublishDate) %></label></td>
					</tr>
					<tr>
						<td>Client SubUnit</td>
						<td><%=Html.Encode(Model.ClientProfileGroup.HierarchyItem) %></td>
						<td>PCC/Office Id</td>
						<td><%=Html.Encode(Model.ClientProfileGroup.PseudoCityOrOfficeId) %></td>
						<td>Last Saved</td>
						<td><%=Html.Encode(Model.ClientProfileGroup.LastUpdateTimestamp) %></td>
					</tr>
					<tr>
						<td>Profile Hierarchy</td>
						<td><%=Html.Encode(Model.ClientProfileGroup.HierarchyType) %></td>
						<td>Profile Name</td>
						<td><%=Html.Encode(Model.ClientProfileGroup.ClientProfileGroupName) %></td>
						<td colspan="2" align="left">
							<div class="home_button">
								<a href="<%=Url.Content("~/ClientProfileGroup.mvc/ListUnDeleted")%>" class="red">Home</a>
							</div>
						</td>
					</tr>
					<tr>
						<td colspan="6">
							<%= Html.ActionLink("Client Profiles", "ListUnDeleted", "ClientProfileGroup", null, new { title = "Client Profiles" })%> &gt;
						<%=Html.RouteLink(Model.ClientProfileGroup.UniqueName, "Default", new { controller = "ClientProfileGroup", action = "Edit", id = Model.ClientProfileGroup.ClientProfileGroupId }, new { title = Model.ClientProfileGroup.UniqueName }) %> &gt; 
						Client Profile Builder
						</td>
					</tr>
				</table>
				<div class="publish-window">
				
					<input type="submit" value="Publish" class="red" id="publish" />
					
					<%if(ViewData["clientProfileText"] != null && !string.IsNullOrEmpty(ViewData["clientProfileText"].ToString())) { %>
						<textarea rows="20" cols="20" readonly="readonly" id="GDS-window"><%=ViewData["clientProfileText"] %></textarea>
					<% } %>
					
					<%= Html.HiddenFor(model => Model.ClientProfileGroup.ClientProfileGroupId)%>
					<%= Html.HiddenFor(model => Model.ClientProfileGroup.GDSCode)%>
					<%= Html.HiddenFor(model => Model.SabreFormat)%>
				
					<div id="dialog-outer">
						<div id="dialog-message"></div>
					</div>

					<div id="sabre-outer">
						<div id="sabre-message"></div>
					</div>
			
				</div>
				<% } %>
			</div>
		</div>
</asp:Content>
