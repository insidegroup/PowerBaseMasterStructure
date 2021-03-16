<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Definition Values</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Optional Field Definitions</div>
		</div>
		<div id="content">
			<table cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<th class="row_header" colspan="3">Delete Optional Field</th>
				</tr>
				<tr>
					<td>Optional Field Name</td>
					<td><%= Html.Encode(Model.OptionalField.OptionalFieldName)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Style</td>
					<td><%= Html.Encode(Model.OptionalField.OptionalFieldStyle.OptionalFieldStyleDescription)%></td>
					<td></td>
				</tr>
				<tr>
					<td width="30%" class="row_footer_left"></td>
					<td width="40%" class="row_footer_centre"></td>
					<td width="30%" class="row_footer_right"></td>
				</tr>
				<tr>
					<td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>
					</td>
					<td class="row_footer_blank_right">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red" />
							<%= Html.HiddenFor(model => model.OptionalField.VersionNumber) %>
							<%= Html.HiddenFor(model => model.OptionalField.OptionalFieldId) %>
						<%}%>
                    </td>
				</tr>
			</table>
		</div>
	</div>

	<script type="text/javascript">
		$(document).ready(function () {
			$('#menu_passivesegmentbuilder').click();
			$('#menu_passivesegmentbuilder_optionalfields').click();
			$("tr:odd").addClass("row_odd");
			$("tr:even").addClass("row_even");
		})
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<%=Html.RouteLink("Optional Field Definitions", "Main", new { controller = "OptionalField", action = "List", }, new { title = "Optional Field Definitions" })%> &gt;
	<%=Model.OptionalField.OptionalFieldName%>
</asp:Content>
