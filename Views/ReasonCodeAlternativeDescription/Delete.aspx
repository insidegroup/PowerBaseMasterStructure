<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ReasonCodeAlternativeDescription>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - ReasonCodeGroups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Reason Code Item Alternative Description</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Delete AirlineAdvice</th> 
		    </tr> 
             <tr>
                    <td><label for="ReasonCodeTravelerDescription">Reason Code Description</label></td>
                    <td><%=Html.Encode(ViewData["ReasonCodeDescription"])%></td>
                    <td></td>
                </tr> 
			<tr>
                <td>Language</td>
                <td><%= Html.Encode(Model.LanguageName)%></td>
                <td></td>
            </tr>
            <tr>
                <td>Alternative Description</td>
                <td><%= Html.Encode(Model.ReasonCodeAlternativeDescription1)%></td>
                <td></td>
            </tr>         
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right" colspan="2">
                <% using (Html.BeginForm()) { %>
                    <%= Html.AntiForgeryToken() %>
                    <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
					<%= Html.HiddenFor(model => model.ReasonCodeItemId) %>
					<%= Html.HiddenFor(model => model.VersionNumber) %>
                <%}%>
                </td>                
            </tr>
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_reasoncodes').click();
	    $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
 </script>
 </asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Reason Code Groups", "Main", new { controller = "ReasonCodeGroup", action = "ListUnDeleted", }, new { title = "Reason Code Groups" })%> &gt;
<%=Html.RouteLink(ViewData["ReasonCodeGroupName"].ToString(), "Default", new { controller = "ReasonCodeGroup", action = "View", id = ViewData["ReasonCodeGroupId"].ToString() }, new { title = ViewData["ReasonCodeGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "ReasonCodeItem", action = "List", id = ViewData["ReasonCodeGroupId"].ToString() }, new { title = "Items" })%> &gt;
<%=Html.RouteLink(ViewData["ReasonCodeItem"].ToString(), "Default", new { controller = "ReasonCodeItem", action = "Edit", id = ViewData["ReasonCodeItemId"].ToString() }, new { title = ViewData["ReasonCodeItem"].ToString() })%> &gt;
<%=Html.RouteLink("Alternative Descriptions", "Default", new { controller = "ReasonCodeAlternativeDescription", action = "List", id = ViewData["ReasonCodeItemId"].ToString() }, new { title = "Alternative Descriptions" })%>
</asp:Content>
