<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.QueueMinderItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Queues</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Follow Up Queue Items</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Follow Up Queue Item</th> 
		        </tr> 
                 <tr>
                    <td><label for="QueueMinderItem_QueueMinderItemDescription">Description</label></td>
                    <td><%= Html.TextBoxFor(model => model.QueueMinderItem.QueueMinderItemDescription, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.QueueMinderItem.QueueMinderItemDescription)%></td>
                </tr>    
                 <tr>
                    <td><label for="QueueMinderItem_QueueMinderTypeId">Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.QueueMinderItem.QueueMinderTypeId, Model.QueueMinderTypes, "None")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.QueueMinderItem.QueueMinderTypeId)%></td>
                </tr>  
                  <tr>
                    <td><label for="QueueMinderItem_PseudoCityOrOfficeId">Pseudo City Or Office Id</label></td>
                    <td> <%= Html.TextBoxFor(model => model.QueueMinderItem.PseudoCityOrOfficeId, new { maxlength = "10", autocomplete = "off"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.QueueMinderItem.PseudoCityOrOfficeId)%></td>
                </tr> 
                  <tr>
                    <td><label for="QueueMinderItem_QueueNumber">Queue Number</label></td>
                    <td> <%= Html.TextBoxFor(model => model.QueueMinderItem.QueueNumber, new { maxlength = "9"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.QueueMinderItem.QueueNumber)%></td>
                </tr> 
                 <tr>
                    <td><label for="QueueMinderItem_QueueCategory">Queue Category</label></td>
                    <td> <%= Html.TextBoxFor(model => model.QueueMinderItem.QueueCategory, new { maxlength = "20", style = "width:250px;" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.QueueMinderItem.QueueCategory)%></td>
                </tr> 
                 <tr>
                    <td><label for="QueueMinderItem_PrefactoryCode">Queue PrefactoryCode</label></td>
                    <td> <%= Html.TextBoxFor(model => model.QueueMinderItem.PrefactoryCode, new { maxlength = "20" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.QueueMinderItem.PrefactoryCode)%></td>
                </tr> 
                 <tr>
                    <td><label for="QueueMinderItem_GDSCode">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.QueueMinderItem.GDSCode, Model.GDSs, "None")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.QueueMinderItem.GDSCode)%></td>
                </tr>  
                 <tr>
                    <td><label for="QueueMinderItem_ContextId">Context</label></td>
                    <td><%= Html.DropDownListFor(model => model.QueueMinderItem.ContextId, Model.Contexts, "None")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.QueueMinderItem.ContextId)%></td>
                </tr>  
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Queue Minder Item" title="Edit Queue Minder Item" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.QueueMinderItem.QueueMinderItemId)%>
            <%= Html.HiddenFor(model => model.QueueMinderItem.VersionNumber)%>
    <% } %>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_ticketqueuegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })

 </script>
 </asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Follow Up Queue Groups", "Main", new { controller = "FollowUpQueueGroup", action = "ListUnDeleted", }, new { title = "Follow Up Queue Groups" })%> &gt;
<%=Html.RouteLink(Model.QueueMinderItem.QueueMinderGroup.QueueMinderGroupName, "Default", new { controller = "FollowUpQueueGroup", action = "View", id = Model.QueueMinderItem.QueueMinderGroup.QueueMinderGroupId }, new { title = Model.QueueMinderItem.QueueMinderGroup.QueueMinderGroupName })%> &gt;
<%=Html.RouteLink("Queue Items", "Default", new { controller = "QueueMinderItem", action = "List", id = Model.QueueMinderItem.QueueMinderGroupId }, new { title = "Queue Items" })%></asp:Content>