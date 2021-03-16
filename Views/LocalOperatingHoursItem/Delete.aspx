<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.LocalOperatingHoursItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - LocalOperatingHoursGroups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Local Operating Hours Item</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
	          <tr> 
		        <th class="row_header" colspan="3">Delete Local Operating Hours Item</th> 
	        </tr> 
          
            <tr>
                <td>Day</td>
                <td><%= Html.Encode(Model.WeekdayName)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>OpeningTime</td>
                <td><%= Html.TextBoxFor(model => model.OpeningTime)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>ClosingTime</td>
                <td><%= Html.TextBoxFor(model => model.ClosingTime)%></td>
                <td></td>
            </tr>        
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left"><%= Html.ActionLink("Back to List", "List", new { id = Model.LocalOperatingHoursGroupId }, new { @class = "red", title = "Back To List" })%></td>
                <td class="row_footer_blank_right" colspan="2">
                <% using (Html.BeginForm()) { %>
                    <%= Html.AntiForgeryToken() %>
                    <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                    <%= Html.HiddenFor(model => model.LocalOperatingHoursGroupId) %>
                    <%= Html.HiddenFor(model => model.VersionNumber) %>
                <%}%>
                </td>                
           </tr>
        </table>
       <%= Html.HiddenFor(model => model.VersionNumber) %>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_localoperatinghours').click();
		$("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
})
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("LocalOperatingHoursGroup", "Main", new { controller = "LocalOperatingHoursGroup", action = "List", }, new { title = "LocalOperatingHoursGroup" })%> &gt;
<%=Html.RouteLink(Model.LocalOperatingHoursGroupName, "Main", new { controller = "LocalOperatingHoursGroup", action = "View", id = Model.LocalOperatingHoursGroupId.ToString() }, new { title = Model.LocalOperatingHoursGroupName })%> &gt;
<%=Html.RouteLink("LocalOperatingHoursItems", "Main", new { controller = "LocalOperatingHoursItem", action = "List", id = Model.LocalOperatingHoursGroupId.ToString() }, new { title = "LocalOperatingHoursItems" })%> &gt;
</asp:Content>