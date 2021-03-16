<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.Team>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Teams</div></div>
        <div id="content">
       <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete="off" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%" border="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Team</th> 
		        </tr> 
                <tr>
                    <td><label for="TeamName">Team Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.TeamName, new{maxlength="50", autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TeamName)%></td>
                </tr> 
                <tr>
                    <td><label for="TeamEmail">Team Email</label></td>
                    <td> <%= Html.TextBoxFor(model => model.TeamEmail, new { maxlength = "50", autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TeamEmail)%></td>
                </tr> 
                <tr>
                    <td><label for="TeamPhoneNumber">Team Phone Number</label></td>
                    <td><%= Html.TextBoxFor(model => model.TeamPhoneNumber, new { maxlength = "50", autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TeamPhoneNumber)%></td>
                </tr>  <tr>
                    <td><label for="CityCode">Team City Code</label></td>
                    <td><strong><%= Html.TextBoxFor(model => model.CityCode, new { maxlength="5", autocomplete="off"})%></strong></td>
                    <td><%= Html.ValidationMessageFor(model => model.CityCode)%>                   
                        <label id="lblCityCode"/>
                    </td>
                </tr>   
                <tr>
                    <td><label for="TeamQueue">Team Queue</label></td>
                    <td><%= Html.TextBoxFor(model => model.TeamQueue, new { maxlength = "50", autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TeamQueue)%></td>
                </tr> 
               
                <tr>
                    <td><label for="TeamTypeCode">Team Type</label></td>
                    <td><%= Html.DropDownList("TeamTypeCode", ViewData["TeamTypes"] as SelectList, "None", new { autocomplete = "off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TeamTypeCode)%></td>
                </tr> 
                <tr>
                    <td><label for="HierarchyType">Hierarchy</label></td>
                    <td><%= Html.TextBoxFor(model => model.HierarchyType, new { disabled="disabled", Value = "Location", autocomplete="off"})%></td>
                    <td>
						<%= Html.HiddenFor(model => model.HierarchyType, new { Value = "Location", autocomplete="off"})%>
						<%= Html.ValidationMessageFor(model => model.HierarchyType)%>
                    </td>
                </tr>               
                <tr>
                    <td><label id="lblHierarchyItem"/>HierarchyItem</td>
                    <td> <%= Html.TextBoxFor(model => model.HierarchyItem, new { disabled="disabled",  size = "30" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.HierarchyItem)%>
                        <%= Html.Hidden("HierarchyCode")%>
                        <label id="lblHierarchyItemMsg"/>
                    </td>
                </tr> 
                <tr style="display:none" id="TravelerType">
                    <td><label for="TravelerTypeName">TravelerTypeName*</label></td>
                    <td><%= Html.TextBoxFor(model => model.TravelerTypeName, new { size = "30" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.TravelerTypeGuid)%>
                        <%= Html.Hidden("TravelerTypeGuid")%>
                        <label id="lblTravelerTypeMsg"/>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><%= Html.ActionLink("Back to List", "List",null,new{ @class="red", title="Back To List"}) %></td>
                    <td class="row_footer_blank"></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Team" title="Create Team" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.TeamId) %>
            <%= Html.HiddenFor(model => model.SourceSystemCode) %>

    <% } %>


        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/team.js")%>" type="text/javascript"></script>
</asp:Content>


