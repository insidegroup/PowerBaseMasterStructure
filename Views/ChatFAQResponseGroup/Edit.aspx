<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ChatFAQResponseGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Chat FAQ Response Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Chat FAQ Response Group</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "ChatFAQResponseGroup", action = "Edit", id = Model.ChatFAQResponseGroupId }, FormMethod.Post, new { id = "form0", autocomplete="off" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Chat FAQ Response Group</th> 
		        </tr> 
		         <tr>
                    <td><label for="ChatFAQResponseGroupName">Group Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.ChatFAQResponseGroupName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ChatFAQResponseGroupName)%><label id="Label1"/></td>
                </tr> 
                <tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.EnabledFlag, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledFlag) %></td>
                </tr>  
               <tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td> <%= Html.EditorFor(model => model.EnabledDate, new { autocomplete="off" })%></td>                    
                    <td><%= Html.ValidationMessageFor(model => model.EnabledDate) %></td>
                </tr> 
                <tr>
                    <td><label for="ExpiryDate">Expiry Date</label> </td>
                    <td> <%= Html.EditorFor(model => model.ExpiryDate, new { autocomplete="off" })%></td>                    
                    <td><%= Html.ValidationMessageFor(model => model.ExpiryDate) %></td>
                </tr> 
                <%
    if (!Model.IsMultipleHierarchy)
    {
                %>
                 <tr>
                    <td><label for="HierarchyType">Hierarchy Type</label></td>
                    <td><%= Html.DropDownList("HierarchyType", ViewData["HierarchyTypes"] as SelectList, "Please Select...", new { autocomplete = "off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.HierarchyType)%></td>
                </tr>               
                <tr>
                    <td><label for="lblHierarchyItem" id="lblHierarchyItem"/>Hierarchy Item</td>
                    <td> <%= Html.TextBoxFor(model => model.HierarchyItem, new { disabled = "disabled", size = "30", autocomplete = "off" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.HierarchyItem)%>
                        <%= Html.Hidden("HierarchyCode")%>
                        <%= Html.Hidden("ClientSubUnitName")%>
                        <label id="lblHierarchyItemMsg"/>
                    </td>
                </tr> 
                 <tr style="display:none" id="TravelerType">
                    <td><label for="TravelerTypeName">Traveler Type</label></td>
                    <td><%= Html.TextBoxFor(model => model.TravelerTypeName, new { size = "30", autocomplete = "off" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.TravelerTypeGuid)%>
                        <%= Html.Hidden("TravelerTypeGuid")%>
                        <label id="lblTravelerTypeMsg"/>
                    </td>
                </tr>
                <% }  %>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" title="Back" class="red">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Confirm Edit" title="Confirm Edit" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.VersionNumber) %>
            <%= Html.HiddenFor(model => model.ChatFAQResponseGroupId) %>
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

<script src="<%=Url.Content("~/Scripts/ERD/ChatFAQResponseGroup.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Chat FAQ Response Groups", "Main", new { controller = "ChatFAQResponseGroup", action = "ListUnDeleted", }, new { title = "Chat FAQ Response Groups" })%> &gt;
<%=Model.ChatFAQResponseGroupName%>
</asp:Content>