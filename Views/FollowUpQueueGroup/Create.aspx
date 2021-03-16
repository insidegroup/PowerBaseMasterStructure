<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.QueueMinderGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Queues</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Follow Up Queue Group</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>
    <% Html.EnableUnobtrusiveJavaScript(); %>
    <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete="off" })){%>
    <%= Html.AntiForgeryToken() %>
        
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Create Follow Up Queue Group</th> 
		    </tr> 
            <tr>
                <td>Group Name</td>
                <td><label id="lblAuto"></label></td>
                <td><%= Html.HiddenFor(model => model.QueueMinderGroup.QueueMinderGroupName)%><label id="lblQueueMinderGroupNameMsg"/></td>
            </tr>
            <tr> 
                <td><label for="QueueMinderGroup_EnabledFlag">Enabled?</label></td>
                <td><%= Html.CheckBox("QueueMinderGroup.EnabledFlag", true, new { autocomplete="off" })%></td>
                <td><%= Html.ValidationMessageFor(model => model.QueueMinderGroup.EnabledFlag)%></td>
            </tr>  
            <tr>
                <td><label for="QueueMinderGroup_EnabledDate">Enabled Date</label></td>
                <td> <%= Html.EditorFor(model => model.QueueMinderGroup.EnabledDate, new { autocomplete="off" })%></td>                    
                <td><%= Html.ValidationMessageFor(model => model.QueueMinderGroup.EnabledDate)%></td>
            </tr> 
            <tr>
                <td><label for="QueueMinderGroup_ExpiryDate">Expiry Date</label> </td>
                <td> <%= Html.EditorFor(model => model.QueueMinderGroup.ExpiryDate, new { autocomplete="off" })%></td>                    
                <td><%= Html.ValidationMessageFor(model => model.QueueMinderGroup.ExpiryDate)%></td>
            </tr> 
            <tr>
                <td><label for="QueueMinderGroup_InheritFromParentFlag">Inherit From Parent?</label></td>
                <td><%= Html.CheckBox("QueueMinderGroup.InheritFromParentFlag", true, new { autocomplete="off" })%></td>
                <td><%= Html.ValidationMessageFor(model => model.QueueMinderGroup.InheritFromParentFlag)%></td>
            </tr> 
            <tr>
                <td><label for="QueueMinderGroup_TripTypeID">Trip Type</label></td>
                <td><%= Html.DropDownListFor(model => model.QueueMinderGroup.TripTypeId, Model.TripTypes, "None", new { autocomplete="off" })%></td>
                <td><%= Html.ValidationMessageFor(model => model.QueueMinderGroup.TripTypeId)%></td>
            </tr> 
            <tr>
                <td><label for="QueueMinderGroup_HierarchyType">Hierarchy Type</label></td>
                <td><%= Html.DropDownListFor(model => model.QueueMinderGroup.HierarchyType, Model.HierarchyTypes, "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.QueueMinderGroup.HierarchyType)%></td>
            </tr>               
            <tr>
                <td><label id="lblHierarchyItem"/>HierarchyItem</td>
                <td> <%= Html.TextBoxFor(model => model.QueueMinderGroup.HierarchyItem, new { disabled = "disabled", size = "30", autocomplete="off" })%><span class="error"> *</span></td>
                <td>
                    <%= Html.ValidationMessageFor(model => model.QueueMinderGroup.HierarchyItem)%>
                    <%= Html.HiddenFor(model => model.QueueMinderGroup.HierarchyCode)%>
					<%= Html.Hidden("TT_CSU")%>
                    <label id="lblHierarchyItemMsg"/>
                </td>
            </tr> 
            <tr style="display:none" id="TravelerType">
                <td><label for="QueueMinderGroup_TravelerTypeName">Traveler Type</label></td>
                <td><%= Html.TextBoxFor(model => model.QueueMinderGroup.TravelerTypeName, new { size = "30", autocomplete="off" })%><span class="error"> *</span></td>
                <td>
                    <%= Html.ValidationMessageFor(model => model.QueueMinderGroup.TravelerTypeGuid)%>
                    <%= Html.HiddenFor(model => model.QueueMinderGroup.TravelerTypeGuid)%>
                    <label id="lblTravelerTypeMsg"/>
                </td>
            </tr>
			<tr id="MeetingIDRow">
                <td><label for="MeetingID">Meeting</label></td>
                <td><%= Html.DropDownListFor(model => model.QueueMinderGroup.MeetingID, ViewData["Meetings"] as SelectList, "Please Select...", new { autocomplete="off" })%></td>
                <td><%= Html.ValidationMessageFor(model => model.QueueMinderGroup.MeetingID) %></td>
            </tr>
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right"><input type="submit" value="Create Follow Up Queue Group" class="red" title="Create Follow Up Queue Group"/></td>
            </tr>
        </table>
        <%= Html.HiddenFor(model => model.QueueMinderGroup.QueueMinderGroupId)%>
        <%= Html.HiddenFor(model => model.QueueMinderGroup.SourceSystemCode)%>
<% } %>
    </div>
</div>

<script src="<%=Url.Content("~/Scripts/ERD/QueueMinderGroup.js")%>" type="text/javascript"></script>
</asp:Content>


