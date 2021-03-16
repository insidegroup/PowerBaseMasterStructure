<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.CommissionableRouteGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Commissionable Routes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Commissionable Route Group</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "CommissionableRouteGroup", action = "Edit", id = Model.CommissionableRouteGroupId }, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Commissionable Route Group</th> 
		        </tr> 
		         <tr>
                    <td><label for="CommissionableRouteGroupName">Group Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.CommissionableRouteGroupName)%></td>
                </tr> 
                <tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.EnabledFlag)%></td>
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
                    <td> <label for="InheritFromParentFlag">Inherit From Parent?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.InheritFromParentFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.InheritFromParentFlag) %></td>
                </tr>  
                <%
                if(!Model.IsMultipleHierarchy){ 
                %>
                 <tr>
                    <td><label for="HierarchyType">Hierarchy Type</label></td>
                    <td><%= Html.DropDownList("HierarchyType", ViewData["HierarchyTypes"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.HierarchyType)%></td>
                </tr>               
                <tr>
                    <td><label for="lblHierarchyItem" id="lblHierarchyItem">Hierarchy Item</label></td>
                    <td> <%= Html.TextBoxFor(model => model.HierarchyItem, new { disabled="disabled",  size = "30" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.HierarchyItem)%>
                        <%= Html.Hidden("HierarchyCode")%>
                        <label id="lblHierarchyItemMsg"></label>
                    </td>
                </tr> 
                 <tr style="display:none" id="TravelerType">
                    <td><label for="TravelerTypeName">Traveler Type</label></td>
                    <td><%= Html.TextBoxFor(model => model.TravelerTypeName, new { size = "30" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.TravelerTypeGuid)%>
                        <%= Html.Hidden("TravelerTypeGuid")%>
                        <label id="lblTravelerTypeMsg"></label>
                    </td>
                </tr>
                <% } %>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" title="Back" class="red">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit CommissionableRouteGroup" title="Edit CommissionableRouteGroup" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.CommissionableRouteGroupName) %>
			<%= Html.HiddenFor(model => model.VersionNumber) %>
            <%= Html.HiddenFor(model => model.CommissionableRouteGroupId) %>
            <%= Html.HiddenFor(model => model.SourceSystemCode)%>
            <%if(Model.IsMultipleHierarchy){ %>
				<%= Html.HiddenFor(model => model.HierarchyType)%>
				<%= Html.Hidden("HierarchyCode", Model.HierarchyCode)%>
				<%= Html.HiddenFor(model => model.HierarchyItem)%>
				<%= Html.HiddenFor(model => model.TravelerTypeGuid)%>
				<%= Html.HiddenFor(model => model.IsMultipleHierarchy)%>
            <% } %>
    <% } %>
    </div>
</div>

<script src="<%=Url.Content("~/Scripts/ERD/CommissionableRouteGroup.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Commissionable Route Groups", "Main", new { controller = "CommissionableRouteGroup", action = "ListUnDeleted", }, new { title = "Commissionable Route Groups" })%> &gt;
<%=Model.CommissionableRouteGroupName%>
</asp:Content>

