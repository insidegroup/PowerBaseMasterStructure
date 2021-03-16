<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.Meeting>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Meeting Groups
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="headerContent" runat="server">
	<link href="<%=Url.Content("~/Style/jquery.timepicker.min.css")%>" rel="Stylesheet" type="text/css" media="screen" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Meeting Groups</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete="off" })){%>
        <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" border="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Meeting Group</th> 
		        </tr> 
                <tr>
                    <td>Meeting Name</td>
                    <td><%= Html.TextBoxFor(model => model.MeetingName, new { autocomplete="off", maxlength="50" })%><span class="error"> *</span></td>
                    <td>
						<label id="lblAuto"></label>
						<label id="lblMeetingNameMsg"/>
                    </td>
                </tr>
                <tr>
                    <td><label for="MeetingReferenceNumber">Meeting Reference</label></td>
                    <td><%= Html.TextBoxFor(model => model.MeetingReferenceNumber, new { autocomplete="off", maxlength="50" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.MeetingReferenceNumber) %></td>
                </tr>  
				<tr>
                    <td><label for="MeetingLocation">Meeting Site</label></td>
                    <td><%= Html.TextBoxFor(model => model.MeetingLocation, new { autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.MeetingLocation) %></td>
                </tr>  
				<tr>
                    <td><label for="CityCode">Meeting City</label></td>
                    <td><%= Html.TextBox("CityName", "", new { autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CityCode)%>                   
                        <label id="lblCityCode"></label>
						<%= Html.HiddenFor(model => model.CityCode) %>
                    </td>
                </tr>  
				<tr>
                    <td><label for="MeetingStartDate">Meeting Start Date</label></td>
                    <td><%= Html.EditorFor(model => model.MeetingStartDate, new { autocomplete="off" })%><span class="error"> *</span></td>                    
                    <td><%= Html.ValidationMessageFor(model => model.MeetingStartDate) %></td>
                </tr> 
                <tr>
                    <td><label for="MeetingEndDate">Meeting End Date</label> </td>
                    <td><%= Html.EditorFor(model => model.MeetingEndDate, new { autocomplete="off" })%><span class="error"> *</span></td>                    
                    <td><%= Html.ValidationMessageFor(model => model.MeetingEndDate) %></td>
                </tr> 
				<tr>
                    <td><label for="MeetingArriveByDateTime">Meeting Arrive By Date/Time</label> </td>
                    <td colspan="2">
						<%= Html.EditorFor(model => model.MeetingArriveByDateTime, new { autocomplete="off" })%> 
						<%= Html.TextBoxFor(model => model.MeetingArriveByTime, new { autocomplete="off" })%>                    
						<%= Html.ValidationMessageFor(model => model.MeetingArriveByDateTime) %>
					</td>
                </tr>
				<tr>
                    <td><label for="MeetingLeaveAfterDateTime">Meeting Leave After Date/Time</label> </td>
                    <td colspan="2">
						<%= Html.EditorFor(model => model.MeetingLeaveAfterDateTime, new { autocomplete="off" })%> 
						<%= Html.TextBoxFor(model => model.MeetingLeaveAfterTime, new { autocomplete="off" })%>                   
						<%= Html.ValidationMessageFor(model => model.MeetingLeaveAfterDateTime) %>
                    </td>
                </tr> 
				<tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.EnabledFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledFlag)%></td>
                </tr>  
                <tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledDate)%></td>
                </tr> 
				<tr>
                    <td><label for="ExpiryDate">Expiry Date</label></td>
                    <td><%= Html.EditorFor(model => model.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ExpiryDate)%></td>
                </tr> 
                <tr>
                    <td><label for="HierarchyType">Hierarchy Type</label></td>
                    <td><%= Html.Raw(Model.HierarchyType)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.HierarchyType)%></td>
                </tr>               
                <tr>
                    <td><label id="lblHierarchyItem"/>Hierarchy Item</td>
                    <td><%= Html.TextBoxFor(model => model.HierarchyItem, new { size = "30", autocomplete="off" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.HierarchyItem)%>
                        <%= Html.Hidden("HierarchyCode")%>
                        <label id="lblHierarchyItemMsg"/>
                    </td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Meeting Group" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.HierarchyType)%>
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/jquery.timepicker.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/ERD/Meeting.js")%>" type="text/javascript"></script>
</asp:Content>

 <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Meeting Groups", "Main", new { controller = "Meeting", action = "ListUnDeleted", }, new { title = "Meeting Groups" })%> &gt;
Create
</asp:Content>