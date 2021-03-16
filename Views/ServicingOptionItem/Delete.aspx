<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ServicingOptionItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Servicing Option Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Servicing Option Item</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Delete Servicing Option Item</th> 
		        </tr> 
                <tr>
                    <td>Servicing Option Group Name</td>
                    <td><%= Html.Encode(Model.ServicingOptionGroupName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Servicing Option Item Sequence</td>
                    <td><%= Html.Encode(Model.ServicingOptionItemSequence)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Servicing Option Name</td>
                    <td><%= Html.Encode(Model.ServicingOptionName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Value</td>
                    <td><%= Html.Encode(Model.ServicingOptionItemValue)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Departure Time Window</td>
                    <td><%= Html.Encode(Model.DepartureTimeWindowMinutes)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Arrival Time Window</td>
                    <td><%= Html.Encode(Model.ArrivalTimeWindowMinutes)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Max Stops</td>
                    <td><%= Html.Encode(Model.MaximumStops)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Max Connection Time</td>
                    <td><%= Html.Encode(Model.MaximumConnectionTimeMinutes)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Use Alternate Airport?</td>
                    <td><%= Html.Encode(Model.UseAlternateAirportFlag ?? false)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>No Penalty?</td>
                    <td><%= Html.Encode(Model.NoPenaltyFlag ?? false)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>No Restrictions?</td>
                    <td><%= Html.Encode(Model.NoRestrictionsFlag ?? false)%></td>
                    <td></td>
                </tr>
                <% if(Model.GDSRequiredFlag == true) { %>
				<tr>
                    <td>GDS</td>
                    <td><%= Html.Encode(Model.GDSName)%></td>
                    <td></td>
                </tr>
				<% } %>
                <tr>
                    <td>Instruction</td>
                    <td><%= Html.Encode(Model.ServicingOptionItemInstruction)%></td>
                    <td></td>
                </tr>     
                <tr>
                    <td>Display In Application</td>
                    <td><%= Html.Encode(Model.DisplayInApplicationFlag ?? false)%></td>
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
                        <%= Html.HiddenFor(model => model.ServicingOptionItemId)%>
                        <%= Html.HiddenFor(model => model.VersionNumber) %>
                    <%}%>
                    </td>                
               </tr>
            </table>

    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_servicingoptions').click();
	    $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Servicing Option Groups", "Main", new { controller = "ServicingOptionGroup", action = "ListUnDeleted", }, new { title = "Servicing Option Groups" })%> &gt;
<%=Html.RouteLink(Model.ServicingOptionGroupName, "Default", new { controller = "ServicingOptionGroup", action = "View", id = Model.ServicingOptionGroupId }, new { title = Model.ServicingOptionGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "ServicingOptionItem", action = "List", id = Model.ServicingOptionGroupId }, new { title = "Servicing Option Items" })%> &gt;
<%= Html.Encode(Model.ServicingOptionName)%>
</asp:Content>