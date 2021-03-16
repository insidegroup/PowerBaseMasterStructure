<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TripTypeItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - TripTypeGroups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Trip Type Items</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		          <tr> 
			        <th class="row_header" colspan="3">Remove Trip Type Item</th> 
		        </tr> 
		        <tr>
                    <td>Trip Type Group</td>
                    <td><%= Html.Encode(Model.TripTypeGroupName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Trip Type Description</td>
                    <td><%= Html.Encode(Model.TripTypeDescription)%></td>
                    <td></td>
                 </tr>         
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Remove" title="Confirm Remove" class="red"/>
                            <%= Html.HiddenFor(model => model.TripTypeGroupId)%>
                            <%= Html.HiddenFor(model => model.VersionNumber)%>

                    <%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>

<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_triptypegroups').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
})
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("TripTypeGroups", "Main", new { controller = "TripTypeGroup", action = "List", }, new { title = "TripTypeGroups" })%> &gt;
<%=Html.RouteLink(Model.TripTypeGroupName, "Default", new { controller = "TripTypeGroup", action = "View", id = Model.TripTypeGroupId }, new { title = Model.TripTypeGroupName })%> &gt;
<%=Html.RouteLink("TripTypeItems", "Default", new { controller = "TripTypeItem", action = "List", id = Model.TripTypeGroupId }, new { title = "TripTypeItems" })%>
</asp:Content>