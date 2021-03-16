<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TeamOutOfOfficeGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Team Out of Office Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Team Out of Office Group</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <%// Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Team Out of Office Group</th> 
		        </tr> 
		       <tr>
                    <td>Group Name</td>
                    <td><label id="lblAuto"><%=Model.TeamOutOfOfficeGroupName%></label></td>
                    <td>
                        <%= Html.HiddenFor(model => model.TeamOutOfOfficeGroupName) %>
                        <label id="lblTeamOutOfOfficeGroupNameMsg"/>
                        <label id="lblTeamOutOfOfficeGroupActiveMsg"/>
                    </td>
                </tr>
                <tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
                    <td><%= Html.CheckBox("EnabledFlag", true)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledFlag) %></td>
                </tr>  
                <tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td> <%= Html.EditorFor(model => model.EnabledDate)%></td>                    
                    <td><%= Html.ValidationMessageFor(model => model.EnabledDate) %></td>
                </tr> 
                <tr>
                    <td><label for="ExpiryDate">Expiry Date</label> </td>
                    <td> <%= Html.EditorFor(model => model.ExpiryDate)%></td>                    
                    <td><%= Html.ValidationMessageFor(model => model.ExpiryDate) %></td>
                </tr>
                <tr>
                    <td><label for="HierarchyType">Hierarchy Type</label></td>
                    <td><%= Html.DropDownList("HierarchyType", ViewData["HierarchyTypes"] as SelectList, "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.HierarchyType)%></td>
                </tr>               
                <tr>
                    <td><label id="lblHierarchyItem"/>Hierarchy Item</td>
                    <td> <%= Html.TextBoxFor(model => model.HierarchyItem, new { size = "30" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.HierarchyItem)%>
                        <%= Html.Hidden("HierarchyCode")%>
                        <%= Html.Hidden("ClientSubUnitName")%>
                        <label id="lblHierarchyItemMsg"/>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="45%" class="row_footer_centre"></td>
                    <td width="25%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Confirm Create" title="Confirm Create" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.TeamOutOfOfficeGroupId) %>
		<% } %>
        </div>
    </div>
    <script src="<%=Url.Content("~/Scripts/ERD/TeamOutOfOfficeGroup.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Team Out of Office Groups", "Main", new { controller = "TeamOutOfOfficeGroup", action = "ListUnDeleted", }, new { title = "Team Out of Office Groups" })%> &gt;
Create
</asp:Content>