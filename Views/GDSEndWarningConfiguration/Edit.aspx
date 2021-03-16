<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSEndWarningConfigurationVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - GDS Response Messages</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">GDS Response Messages</div></div>
        <div id="content">
            <% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<% using (Html.BeginForm()) {%>
				<%= Html.AntiForgeryToken() %>
				<table cellpadding="0" border="0" width="100%" cellspacing="0"> 
					<tr> 
						<th class="row_header" colspan="3">Edit GDS Response Message</th> 
					</tr> 
					<tr>
						<td><label for="GDSEndWarningConfiguration_GDS">GDS</label></td>
						<td><%= Html.DropDownListFor(model => model.GDSEndWarningConfiguration.GDSCode, Model.GDSs, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSEndWarningConfiguration.GDSCode)%></td>
					</tr>
					<tr valign="top">
						<td><label for="GDSEndWarningConfiguration_IdentifyingWarningMessage">GDS Response Message</label></td>
						<td><%= Html.TextBoxFor(model => model.GDSEndWarningConfiguration.IdentifyingWarningMessage, new { maxlength = "255"  })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSEndWarningConfiguration.IdentifyingWarningMessage)%></td>
					</tr>
					<tr>
						<td><label for="GDSEndWarningConfiguration_Behaviour">Behaviour</label></td>
						<td><%= Html.DropDownListFor(model => model.GDSEndWarningConfiguration.GDSEndWarningBehaviorTypeId, Model.GDSEndWarningBehaviorTypes, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSEndWarningConfiguration.GDSEndWarningBehaviorTypeId)%></td>
					</tr>           
					<% if (Model.GDSEndWarningConfiguration.AutomatedCommands.Count > 0) { 
						foreach(var automatedCommand in Model.GDSEndWarningConfiguration.AutomatedCommands) { %>
						<tr class="AutomatedCommandsLine" valign="top">
							<td><label id="GDSEndWarningConfiguration_AutomatedCommand_1"/>Automated Command <%=automatedCommand.CommandExecutionSequenceNumber %></td>
							<td><%= Html.TextBox("AutomatedCommand_" + automatedCommand.CommandExecutionSequenceNumber.ToString(), automatedCommand.CommandText, new { @class = "automated-input" }) %> 
								<span class="error"> *</span>
								<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/delete.png" alt="Remove" /></a> 
								<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a>
							</td>
							<td></td>
						</tr>
						<% }
					} else { %>
					<tr class="AutomatedCommandsLine" valign="top">
						<td><label id="GDSEndWarningConfiguration_AutomatedCommand_1"/>Automated Command 1</td>
						<td>
							<%= Html.TextBox("AutomatedCommand_1", null, new { @class = "automated-input" }) %>
							<span class="error"> *</span> 
							<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/delete.png" alt="Remove" /></a> 
							<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a>
						</td>
						<td></td>
					</tr>
					<% } %>
					<tr>
						<td width="30%" class="row_footer_left"></td>
						<td width="40%" class="row_footer_centre"></td>
						<td width="30%" class="row_footer_right"></td>
					</tr>
				   <tr>
						<td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>                    
						<td class="row_footer_blank_right">
							<input type="submit" value="Edit GDS Response Message" title="Edit GDS Response Message" class="red"/>
							<%= Html.HiddenFor(model => model.GDSEndWarningConfiguration.VersionNumber)%>
							<%= Html.HiddenFor(model => model.GDSEndWarningConfiguration.GDSEndWarningConfigurationId)%>
						</td>
					</tr>
			   </table>
			<%}%>
        </div>
    </div>
    
	<script src="<%=Url.Content("~/Scripts/ERD/GDSEndWarningConfiguration.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("GDS Response Messages", "Main", new { controller = "GDSEndWarningConfiguration", action = "List" }, new { title = "GDS Response Messages" })%> &gt;
<%= CWTDesktopDatabase.Helpers.CWTStringHelpers.TrimString(ViewData["WarningMessage"].ToString(), 30) %> &gt;
Edit GDS Response Message
</asp:Content>